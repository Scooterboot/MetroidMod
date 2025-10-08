using MetroidMod.Common.Players;
using SubworldLibrary;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

//using MetroidMod.Buffs;
using static Terraria.ModLoader.ModContent;
using System.Linq;

namespace MetroidMod.Content.Tiles
{
	public class MetroidDeepnestExit : ModTile
	{
		private readonly float PlayerMaxRange = 64.0f;
		private readonly float RightClickRange = 20.0f;

		public static LocalizedText HoverText;

		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.NotReallySolid[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Width = 11;
			TileObjectData.newTile.Height = 14;
			int[] heights = Enumerable.Repeat((int)16, TileObjectData.newTile.Height).ToArray();
			heights[TileObjectData.newTile.Height - 1] = 18;
			TileObjectData.newTile.CoordinateHeights = heights;
			TileObjectData.newTile.Origin = new Point16(TileObjectData.newTile.Width / 2, TileObjectData.newTile.Height - 3);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(232, 170, 57), name);
			MinPick = 225;
			MineResist = 25f;

			HoverText = this.GetLocalization("HoverText");
		}


		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX == 5 * 18 && tile.TileFrameY >= 7 * 18)
			{
				r = 1f;
				g = 0.95f;
				b = 0.69f;
			}
		}
		public override bool Slope(int i, int j) { return false; }

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY)
		{
			width = 2;
			height = 2;
		}
		public override void MouseOver(int i, int j)
		{
			if (Main.LocalPlayer.Distance(TileEntrancePos(i, j)) < PlayerMaxRange &&
				Main.MouseWorld.Distance(TileEntrancePos(i, j)) < RightClickRange)
			{
				Main.LocalPlayer.noThrow = 2;
				Main.LocalPlayer.cursorItemIconEnabled = true;
				Main.LocalPlayer.cursorItemIconID = ItemType<Items.Tiles.MetroidDeepnestExit>();
				Main.LocalPlayer.cursorItemIconText = HoverText.Value;
			}
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			new EntitySource_TileBreak(i, j);
		}

		public override bool RightClick(int i, int j)
		{
			if (Main.LocalPlayer.Distance(TileEntrancePos(i, j)) < PlayerMaxRange &&
				Main.MouseWorld.Distance(TileEntrancePos(i, j)) < RightClickRange &&
				!MUtils.AnyBossesActive() && SubworldSystem.IsActive<Subworlds.MetroidDeepnest>())
			{
				SubworldSystem.Exit();
				return (true);
			}
			return (false);
		}
		private static Vector2 TileEntrancePos(int x, int y)
		{
			Vector2 center = new Vector2(x * 16, y * 16) + new Vector2(8, 8);

			if (Main.tile[x, y].TileFrameX < 4 * 18)
			{
				center.X += 16 * (4 - Main.tile[x, y].TileFrameX / 18);
			}
			if (Main.tile[x, y].TileFrameX > 6 * 18)
			{
				center.X -= 16 * (Main.tile[x, y].TileFrameX / 18 - 6);
			}
			if (Main.tile[x, y].TileFrameY < 5 * 18)
			{
				center.Y += 16 * (5 - Main.tile[x, y].TileFrameY / 18);
			}
			Dust.NewDustPerfect(center, DustID.BlueFairy, Vector2.Zero).noGravity = true;

			return center;
		}
	}
}
