using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using MetroidMod.Items;

namespace MetroidMod.Tiles
{
	public class EnergyStation : ModTile
	{
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
            int x = i - (Main.tile[i, j].frameX / 18) % 2;
            int y = j - (Main.tile[i, j].frameY / 18) % 2;
            Vector2 worldPos = new Vector2((x * 16) + 16, (y * 16) + 16);
            Player player = Main.LocalPlayer;
            if (player.Distance(worldPos) < 50)
            {
                player.noThrow = 2;
                player.showItemIcon = true;
                player.showItemIcon2 = mod.ItemType("EnergyStation");
            }
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType("EnergyStation"));
		}

		public override void RightClick(int i, int j)
		{
            //HitWire(i, j);
            int x = i - (Main.tile[i, j].frameX / 18) % 2;
            int y = j - (Main.tile[i, j].frameY / 18) % 2;
            Vector2 worldPos = new Vector2((x * 16) + 16, (y * 16) + 16);
            Player player = Main.player[Player.FindClosest(worldPos, 16, 16)];
            if (player.Distance(worldPos) < 50)
            {
                player.AddBuff(mod.BuffType("EnergyRecharge"), 2);
            }
        }
		/*public override void HitWire(int i, int j)
		{
			int x = i - (Main.tile[i, j].frameX / 18) % 2;
			int y = j - (Main.tile[i, j].frameY / 18) % 2;
			Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType("MissilePickup"), 10 + Main.rand.Next(16));
			if (Wiring.running)
			{
				Wiring.SkipWire(x, y);
				Wiring.SkipWire(x, y + 1);
				Wiring.SkipWire(x + 1, y);
				Wiring.SkipWire(x + 1, y + 1);
			}
		}*/
	}
}
