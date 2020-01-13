using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

using MetroidMod.Buffs;
using static Terraria.ModLoader.ModContent;

namespace MetroidMod.Tiles
{
	public class EnergyStation : ModTile
	{
		readonly float rightclickRange = 50.0f;

		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
            Main.tileSolidTop[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2); 
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 18 };
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Energy Station");
			AddMapEntry(new Color(232, 170, 57), name);
		}

		public override bool Slope(int i, int j) { return false; }

		public override void MouseOver(int i, int j)
		{
			if (Main.LocalPlayer.Distance(TileCenter(i, j)) < rightclickRange)
			{
				Main.LocalPlayer.noThrow = 2;
				Main.LocalPlayer.showItemIcon = true;
				Main.LocalPlayer.showItemIcon2 = mod.ItemType("EnergyStation");
            }
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType("EnergyStation"));
		}

		public override bool NewRightClick(int i, int j)
		{
			if (Main.LocalPlayer.Distance(TileCenter(i, j)) < rightclickRange)
			{
				Main.LocalPlayer.AddBuff(BuffType<EnergyRecharge>(), 2);
				return (true);
			}
			return (false);
		}

		Vector2 TileCenter(int x, int y)
		{
			Vector2 center = new Vector2(x * 16, y * 16);

			if (Main.tile[x, y].frameX == 0)
				center.X += 16;
			if (Main.tile[x, y].frameY == 0)
				center.Y += 16;

			return center;
		}
	}
}
