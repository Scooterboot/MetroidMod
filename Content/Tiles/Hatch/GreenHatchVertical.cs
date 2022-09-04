using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;

namespace MetroidMod.Content.Tiles.Hatch
{
	public class GreenHatchVertical : BlueHatch
	{
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
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16, 16 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Green Hatch");
			AddMapEntry(new Color(0, 160, 0), name);
			AdjTiles = new int[] { TileID.ClosedDoor };
			MinPick = 150;
			
			otherDoorID = ModContent.TileType<GreenHatchOpenVertical>();
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Tiles.GreenHatch>();
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<Items.Tiles.GreenHatch>());
		}

		public override bool RightClick(int i, int j)
		{
			if (/*Common.Configs.MServerConfig.Instance.veryBrokenHatchControl || */Main.LocalPlayer.controlUseTile)
			{
				if (Main.LocalPlayer.TryGetModPlayer(out Common.Players.MPlayer mp) && mp.GreenKeycard)
				{
					HitWire(i, j);
					if (Main.netMode == NetmodeID.MultiplayerClient) { SendRightClick(i, j); }
					return true;
				}
			}
			return false;
		}
		
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.05f;
			g = 0.5f;
			b = 0.05f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawDoor(i,j,spriteBatch,ModContent.Request<Texture2D>($"{Mod.Name}/Content/Tiles/Hatch/GreenHatchVerticalDoor").Value);
			return true;
		}
	}
}
