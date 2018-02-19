using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;

namespace MetroidMod.Tiles
{
	public class BlueHatchOpen : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.DrawsWalls[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3); 
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			TileID.Sets.HousingWalls[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Blue Hatch");
			AddMapEntry(new Color(56, 112, 224), name);
			dustType = 1;
			adjTiles = new int[]{ TileID.OpenDoor };
		}

		public override bool Slope(int i, int j)
		{
			return false;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = mod.ItemType("BlueHatch");
		}
		public override void RightClick(int i, int j)
		{
			HitWire(i, j);
		}
public override void HitWire(int i, int j)
		{
			Main.PlaySound(SoundLoader.customSoundType, i * 16, j * 16,  mod.GetSoundSlot(SoundType.Custom, "Sounds/HatchCloseSound"));
			int x = i - (Main.tile[i, j].frameX / 18) % 3;
			int y = j - (Main.tile[i, j].frameY / 18) % 3;
			for (int l = x; l < x + 3; l++)
			{
				for (int m = y; m < y + 3; m++)
				{
					if (Main.tile[l, m] == null)
					{
						Main.tile[l, m] = new Tile();
					}
					if (Main.tile[l, m].active() && Main.tile[l, m].type == Type)
					{
						Main.tile[l,m].type = (ushort)mod.TileType("BlueHatch");
					}
				}
			}
			if (Wiring.running)
			{
				Wiring.SkipWire(x, y);
				Wiring.SkipWire(x, y + 1);
				Wiring.SkipWire(x, y + 2);
				Wiring.SkipWire(x + 1, y);
				Wiring.SkipWire(x + 1, y + 1);
				Wiring.SkipWire(x + 1, y + 2);
			}
			NetMessage.SendTileSquare(-1, x, y + 1, 3);
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("BlueHatch"));
		}
	}
}
