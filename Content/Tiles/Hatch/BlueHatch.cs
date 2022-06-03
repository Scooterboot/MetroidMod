using System;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MetroidModPorted.Common.Systems;

namespace MetroidModPorted.Content.Tiles.Hatch
{
	public class BlueHatch : ModTile
	{
		public int otherDoorID = 0;

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
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Blue Hatch");
			AddMapEntry(new Color(56, 112, 224), name);
			AdjTiles = new int[] { TileID.ClosedDoor };

			otherDoorID = ModContent.TileType<BlueHatchOpen>();
		}

		public override bool Slope(int i, int j) { return false; }

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Tiles.BlueHatch>();
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<Items.Tiles.BlueHatch>());
		}

		public override bool RightClick(int i, int j)
		{
			HitWire(i, j);
			return true;
		}

		public override void HitWire(int i, int j)
		{
			ToggleHatch(i, j, (ushort)otherDoorID, Name.Contains("Open"));
			if (MetroidModPorted.AutocloseHatchesEnabled)
			{
				MSystem.doorTimers.Enqueue(new Tuple<int, Vector2>(MSystem.Timer + 60 * MetroidModPorted.AutocloseHatchesTime, new Vector2(i, j)));
			}
		}

		public void ToggleHatch(int i, int j, ushort type, bool isOpen = false)
		{
			int x = i - (Main.tile[i, j].TileFrameX / 18) % 4;
			int y = j - (Main.tile[i, j].TileFrameY / 18) % 4;
			if (isOpen)
			{
				for (int l = x; l < x + 4; l++)
				{
					for (int m = y; m < y + 4; m++)
					{
						if (!Collision.EmptyTile(l, m, true))
						{
							MSystem.nextDoorTimers.Enqueue(new Tuple<int, Vector2>((int)(MSystem.Timer) + 60, new Vector2(i, j)));
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
						Main.tile[l, m].ResetToType(type);
					Main.tile[l, m].Get<TileWallWireStateData>().HasTile = true;
					Main.tile[l, m].Get<TileTypeData>().Type = (ushort)type;
				}
			}
			if (Main.netMode != NetmodeID.MultiplayerClient && Wiring.running)
			{
				for (int ix = x; ix < x + 4; ++ix)
					for (int iy = y; iy < y + 4; ++iy)
						Wiring.SkipWire(ix, iy);
			}

			NetMessage.SendTileSquare(-1, x + 1, y + 1, 4, TileChangeType.None);

			SoundEngine.PlaySound(isOpen ? Sounds.Tiles.HatchClose : Sounds.Tiles.HatchOpen, new(i * 16, j * 16));
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.05f;
			g = 0.05f;
			b = 0.5f;
		}

		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			if (type == Type)
			{
				short doorHeight = 72;
				Tile tile = Main.tile[i, j];
				while (tile.TileFrameY + frameYOffset >= doorHeight)
				{
					frameYOffset -= doorHeight;
				}
				if (tile.TileFrameY >= doorHeight)
				{
					if (tile.TileFrameY >= doorHeight * 4)
					{
						tile.TileFrameY -= doorHeight;
					}
					tile.TileFrameY -= doorHeight;
				}
			}
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawDoor(i, j, spriteBatch, ModContent.Request<Texture2D>($"{Mod.Name}/Content/Tiles/Hatch/BlueHatchDoor").Value);
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
			if (tile.TileFrameY < doorHeight * 4)
			{
				spriteBatch.Draw(tex, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
	}
}
