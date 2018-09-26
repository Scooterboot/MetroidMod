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
	public class MissileStation : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.NotReallySolid[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2); 
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 18 };
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Missile Station");
			AddMapEntry(new Color(132, 4, 20), name);
		}

		public override bool Slope(int i, int j) { return false; }
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = mod.ItemType("MissileStation");
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType("MissileStation"));
		}

        public override void RightClick(int i, int j)
		{
            Player player = Main.LocalPlayer;
            
            for(int k = 0; k < player.inventory.Length; ++k)
            {
                if(player.inventory[k].type == mod.ItemType("MissileLauncher"))
                {
                    MGlobalItem mi = player.inventory[i].GetGlobalItem<MGlobalItem>(mod);
                    int requiredAmount = mi.maxMissiles - mi.statMissiles;
                    mi.statMissiles += requiredAmount;
                }
            }
            Main.PlaySound(7, (int)player.position.X, (int)player.position.Y, 1);
        }
		public override void HitWire(int i, int j)
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
		}
	}
}
