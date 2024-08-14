using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Enums;
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

		public override string Name =>
			$"{Hatch.Name}{(Open ? "Open" : string.Empty)}{(Vertical ? "Vertical" : string.Empty)}";
		public override string Texture => 
			$"{nameof(MetroidMod)}/Content/Hatches/HatchBase{(Vertical ? "Vertical" : string.Empty)}";

		public Color MapColor => Hatch.PrimaryColor;

		/// <summary>
		/// The tile entity associated with this tile. In the event this is a hatch from a version
		/// of the mod that had no tile entities (or the tile entity somehow magically died),
		/// we ensure we create a new one so it's available at all times.
		/// </summary>
		public HatchTileEntity TileEntity(int i, int j)
		{
			if (TileUtils.TryGetTileEntityAs(i, j, out HatchTileEntity tileEntity))
			{
				return tileEntity;
			}

			var position = TileUtils.GetTopLeftTileInMultitile(i, j);
			int id = ModContent.GetInstance<HatchTileEntity>().Place(position.X, position.Y);
			return (HatchTileEntity)Terraria.DataStructures.TileEntity.ByID[id];
		}


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

			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<HatchTileEntity>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.newTile.UsesCustomCanPlace = true;

			TileObjectData.addTile(Type);

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			AddMapEntry(MapColor, Hatch.DisplayName);
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

			TileEntity(i, j).Behavior.HitRightClick(withKeycard);
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = Hatch.ItemType;
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			return false;
		}

		public override void HitWire(int i, int j)
		{
			HatchTileEntity tileEntity = TileEntity(i, j);
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
			ModContent.GetInstance<HatchTileEntity>().Kill(i, j);
		}
	}
}
