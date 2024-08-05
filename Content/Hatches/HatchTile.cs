using System;
using MetroidMod.Content.Tiles.Hatch;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Enums;
using MetroidMod.Content.Items.Tiles;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Hatches
{
	/// <summary>
	/// This is a template for the hatch combinations that exist for every hatch:
	/// Open, closed, vertical and horizontal. There might be a better way to achieve this
	/// but at the moment we need to have each state combination be its own ModTile.
	/// </summary>
	internal class HatchTile(ModHatch Hatch, bool Open, bool Vertical) : ModTile
	{
		public ModHatch Hatch { get; } = Hatch;
		public bool Open { get; } = Open;
		public bool Vertical { get; } = Vertical;

		// TODO the _ is to avoid conflicts, but also ensure that old worlds load properly
		public override string Name =>
			$"{Hatch.Name}_{(Open ? "Open" : string.Empty)}{(Vertical ? "Vertical" : string.Empty)}";
		public override string Texture =>
			Vertical ? ModContent.GetInstance<Tiles.Hatch.BlueHatchVertical>().Texture
				: ModContent.GetInstance<Tiles.Hatch.BlueHatch>().Texture;

		public Color MapColor => Hatch.PrimaryColor;
		public ModTileEntity TileEntity => ModContent.GetInstance<HatchTileEntity>();


		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;

			TileID.Sets.NotReallySolid[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 4;

			if (Vertical)
			{
				TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
				TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
				TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			}

			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16];

			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(TileEntity.Hook_AfterPlacement, -1, 0, true);
			TileObjectData.newTile.UsesCustomCanPlace = true;

			TileObjectData.addTile(Type);

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			AddMapEntry(MapColor, CreateMapEntryName());
			AdjTiles = [TileID.ClosedDoor];

			RegisterItemDrop(Hatch.ItemType);
		}

		public override bool Slope(int i, int j) { return false; }
		public override bool RightClick(int i, int j)
		{
			bool withKeycard = false;
			
			if(Main.LocalPlayer.TryGetModPlayer(out MPlayer mp))
			{
				if (Hatch is Variants.RedHatch && mp.RedKeycard) withKeycard = true;
				if (Hatch is Variants.GreenHatch && mp.GreenKeycard) withKeycard = true;
				if (Hatch is Variants.YellowHatch && mp.YellowKeycard) withKeycard = true;
			}

			TileUtils.GetTileEntity<HatchTileEntity>(i, j).Behavior.HitRightClick(withKeycard);
			return true;
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			HatchTileEntity tileEntity = TileUtils.GetTileEntity<HatchTileEntity>(i, j);

			Tile tile = Main.tile[i, j];

			int animationFrame = tileEntity.Animation.DoorAnimationFrame;
			if (animationFrame != 4)
			{
				Texture2D texture = Mod.Assets.Request<Texture2D>(
					tileEntity.Appearance.GetTexturePath(Vertical), AssetRequestMode.ImmediateLoad
				).Value;

				Vector2 zero = Main.drawToScreen ? Vector2.Zero : new(Main.offScreenRange, Main.offScreenRange);
				Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
				Rectangle source = new(tile.TileFrameX, tile.TileFrameY + animationFrame * (18 * 4), 16, 16);

				spriteBatch.Draw(
					texture,
					position,
					source,
					Lighting.GetColor(i, j),
					0f,
					Vector2.Zero,
					1f,
					SpriteEffects.None,
					0f
				);
			}

			return true;
		}

		public override void HitWire(int i, int j)
		{
			HatchTileEntity tileEntity = TileUtils.GetTileEntity<HatchTileEntity>(i, j);
			var origin = tileEntity.Position;

			var trigger = GetHatchWireTriggerAt(i - origin.X, j - origin.Y);
			switch (trigger)
			{
				case HatchWireTrigger.ToggleDoor:
					tileEntity.Behavior.WireToggleDoor();
					break;
				case HatchWireTrigger.ToggleBlue:
					tileEntity.Behavior.WireToggleBlue();
					break;
				case HatchWireTrigger.ToggleLock:
					tileEntity.Behavior.WireToggleLocked();
					break;
			}

			// Prevent the same functionality from being triggered more than once
			for (int sy = 0; sy < 4; sy++)
			{
				for (int sx = 0; sx < 4; sx++)
				{
					var iterTrigger = GetHatchWireTriggerAt(sx, sy);
					if(trigger == iterTrigger)
					{
						Wiring.SkipWire(origin.X + sx, origin.Y + sy);
					}
				}
			}
		}

		private HatchWireTrigger GetHatchWireTriggerAt(int i, int j)
		{
			if(Vertical)
			{
				// Do all calculations assuming a horizontal hatch for simplicity
				(i, j) = (j, i);
			}

			bool isOuterPart = i == 0 || i == 3;
			
			if (isOuterPart)
			{
				return HatchWireTrigger.ToggleDoor;
			}

			bool firstVerticalHalf = j < 2;

			return firstVerticalHalf ? HatchWireTrigger.ToggleBlue : HatchWireTrigger.ToggleLock;
		}

		private enum HatchWireTrigger
		{
			ToggleDoor,
			ToggleBlue,
			ToggleLock,
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			TileEntity.Kill(i, j);
		}
	}
}
