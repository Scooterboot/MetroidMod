using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;

namespace MetroidMod.Content.Tiles.Hatch
{
	public class YellowHatch : BlueHatch
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
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16, 16 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Yellow Hatch");
			AddMapEntry(new Color(248, 232, 56), name);
			AdjTiles = new int[] { TileID.ClosedDoor };
			MinPick = 210;
			
			otherDoorID = ModContent.TileType<YellowHatchOpen>();
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Tiles.YellowHatch>();
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			new EntitySource_TileBreak(i, j); //Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<Items.Tiles.YellowHatch>());
		}

		public override bool RightClick(int i, int j)
		{
			if (/*Common.Configs.MConfigMain.Instance.veryBrokenHatchControl || */Main.LocalPlayer.controlUseTile)
			{
				if (Main.LocalPlayer.TryGetModPlayer(out Common.Players.MPlayer mp) && mp.YellowKeycard)
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
			r = 0.5f;
			g = 0.4f;
			b = 0.05f;
		}
		
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawDoor(i,j,spriteBatch,ModContent.Request<Texture2D>($"{Mod.Name}/Content/Tiles/Hatch/YellowHatchDoor").Value);
			return true;
		}
	}
}
