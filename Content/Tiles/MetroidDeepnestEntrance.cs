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
using System.Linq;

//using MetroidMod.Buffs;

namespace MetroidMod.Content.Tiles
{
	public class MetroidDeepnestEntrance : ModTile
	{
		private readonly float PlayerMaxRange = 64.0f;
		private readonly float RightClickRange = 20.0f;

		public static LocalizedText HoverText;

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.NotReallySolid[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Width = 13;
			TileObjectData.newTile.Height = 15;
			int[] heights = Enumerable.Repeat((int)16, TileObjectData.newTile.Height).ToArray();
			heights[TileObjectData.newTile.Height - 1] = 18;
			TileObjectData.newTile.CoordinateHeights = heights;
			TileObjectData.newTile.Origin = new Point16(TileObjectData.newTile.Width / 2, TileObjectData.newTile.Height - 3);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(28, 27, 3), name);
			MinPick = 225;
			MineResist = 25f;

			HoverText = this.GetLocalization("HoverText");
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
				Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<Items.Tiles.MetroidDeepnestEntrance>();
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
				!MUtils.AnyBossesActive() && !SubworldSystem.AnyActive())
			{
				Main.LocalPlayer.MetroidPlayer().posInRealWorld = Main.LocalPlayer.position;
				SubworldSystem.MovePlayerToSubworld<Subworlds.MetroidDeepnest>(Main.LocalPlayer.whoAmI);
				return (true);
			}
			return (false);
		}

		private static Vector2 TileEntrancePos(int x, int y)
		{
			Vector2 center = new Vector2(x * 16, y * 16) + new Vector2(8, 8);

			if (Main.tile[x, y].TileFrameX < 5 * 18)
			{
				center.X += 16 * (5 - Main.tile[x, y].TileFrameX / 18);
			}
			if (Main.tile[x, y].TileFrameX > 7 * 18)
			{
				center.X -= 16 * (Main.tile[x, y].TileFrameX / 18 - 7);
			}
			if (Main.tile[x, y].TileFrameY < 6 * 18)
			{
				center.Y += 16 * (6 - Main.tile[x, y].TileFrameY / 18);
			}
			Dust.NewDustPerfect(center, DustID.BlueFairy, Vector2.Zero).noGravity = true;

			return center;
		}
	}
}
