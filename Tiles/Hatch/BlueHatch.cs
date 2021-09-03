using System;

using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MetroidMod.Common.Worlds;

namespace MetroidMod.Tiles.Hatch
{
	public class BlueHatch : ModTile
	{
        public int otherDoorID = 0;
        
		public override void SetDefaults()
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
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Blue Hatch");
			AddMapEntry(new Color(56, 112, 224), name);
            adjTiles = new int[]{ TileID.ClosedDoor };
			
			otherDoorID = mod.TileType("BlueHatchOpen");
        }

        public override bool Slope(int i, int j) { return false; }
        
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = mod.ItemType("BlueHatch");
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("BlueHatch"));
        }

        public override bool NewRightClick(int i, int j)
		{
			HitWire(i, j);
			return true;
		}
        
		public override void HitWire(int i, int j)
		{
			ToggleHatch(i, j, (ushort)otherDoorID);
            MWorld.doorTimers.Enqueue(new Tuple<int,Vector2>((int)(MWorld.Timer) + 60 * 30, new Vector2(i, j)));
		}
        
		public void ToggleHatch(int i, int j, ushort type, bool isOpen = false)
		{
			int x = i - (Main.tile[i, j].frameX / 18) % 4;
			int y = j - (Main.tile[i, j].frameY / 18) % 4;
			if(isOpen)
			{
				for (int l = x; l < x + 4; l++)
				{
					for (int m = y; m < y + 4; m++)
					{
						if(!Collision.EmptyTile(l, m, true))
						{
                            MWorld.nextDoorTimers.Enqueue(new Tuple<int,Vector2>((int)(MWorld.Timer) + 60, new Vector2(i, j)));
							return;
						}
					}
				}
			}
			for (int l = x; l < x + 4; l++)
			{
				for (int m = y; m < y + 4; m++)
				{
					if (Main.tile[l, m] == null)
						Main.tile[l, m] = new Tile();
					Main.tile[l, m].active(true);
					Main.tile[l, m].type = type;
				}
			}
			if (Main.netMode != 1 && Wiring.running)
			{
				for (int ix = x; ix < x + 4; ++ix)
					for (int iy = y; iy < y + 4; ++iy)
						Wiring.SkipWire(ix, iy);
			}

			NetMessage.SendTileSquare(-1, x + 1, y + 1, 4, TileChangeType.None);

			string sound = "Sounds/HatchOpenSound";
			if(isOpen)
			{
				sound = "Sounds/HatchCloseSound";
			}
			Main.PlaySound(SoundLoader.customSoundType, i * 16, j * 16, mod.GetSoundSlot(SoundType.Custom, sound));
		}
		
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.05f;
			g = 0.05f;
			b = 0.5f;
		}
		
		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			if(type == Type)
			{
				short doorHeight = 72;
				Tile tile = Main.tile[i, j];
				while(tile.frameY+frameYOffset >= doorHeight)
				{
					frameYOffset -= doorHeight;
				}
				if(tile.frameY >= doorHeight)
				{
					if(tile.frameY >= doorHeight*4)
					{
						tile.frameY -= doorHeight;
					}
					tile.frameY -= doorHeight;
				}
			}
		}
		
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            DrawDoor(i,j,spriteBatch,mod.GetTexture("Tiles/Hatch/BlueHatchDoor"));
			return true;
        }
		public static void DrawDoor(int i, int j, SpriteBatch spriteBatch, Texture2D tex)
		{
			Tile tile = Main.tile[i, j];
			
            short doorHeight = 72;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			if(tile.frameY < doorHeight*4)
			{
				spriteBatch.Draw(tex, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
    }
}