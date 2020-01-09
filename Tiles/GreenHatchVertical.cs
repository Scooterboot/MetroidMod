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
	public class GreenHatchVertical : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.NotReallySolid[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3); 
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Green Hatch");
			AddMapEntry(new Color(0, 160, 0), name);
            adjTiles = new int[] { TileID.ClosedDoor };
        }

        public override bool Slope(int i, int j) { return false; }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = mod.ItemType("GreenHatch");
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("GreenHatch"));
        }

		public override bool NewRightClick(int i, int j)
		{
			HitWire(i, j);
			return (true);
		}
		public override void HitWire(int i, int j)
		{
			int x = i - Main.tile[i, j].frameX / 18;
			int y = j - Main.tile[i, j].frameY / 18;
			for (int l = x; l < x + 3; l++)
			{
				for (int m = y; m < y + 3; m++)
				{
					if (Main.tile[l, m] == null)
						Main.tile[l, m] = new Tile();
					Main.tile[l, m].active(true);
					Main.tile[l, m].type = (ushort)mod.TileType("GreenHatchOpenVertical");
				}
			}

			if (Main.netMode != 1 && Wiring.running)
			{
				for (int ix = x; ix < x + 3; ++ix)
					for (int iy = y; iy < y + 3; ++iy)
						Wiring.SkipWire(ix, iy);
			}

			NetMessage.SendTileSquare(-1, x + 1, y + 1, 3, TileChangeType.None);

			Main.PlaySound(SoundLoader.customSoundType, i * 16, j * 16, mod.GetSoundSlot(SoundType.Custom, "Sounds/HatchOpenSound"));
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
                zero = Vector2.Zero;

            Main.spriteBatch.Draw(mod.GetTexture("Tiles/GreenHatchVerticalDoor"), new Vector2(i * 16 - (int)Main.screenPosition.X, (j - 1) * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, 48), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return true;
        }
    }
}
