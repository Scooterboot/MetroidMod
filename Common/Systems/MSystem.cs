#region Using directives

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MetroidModPorted.Common.Players;
using Terraria.UI;

#endregion

namespace MetroidModPorted.Common.Systems
{
	public partial class MSystem : ModSystem
	{
		public static MetroidBossDown bossesDown;
		public static Rectangle TorizoRoomLocation = new(0, 0, 80, 40);

		public static ushort[,] mBlockType = new ushort[Main.maxTilesX, Main.maxTilesY];

		public static bool[,] hit = new bool[Main.maxTilesX, Main.maxTilesY];
		public static bool[,] dontRegen = new bool[Main.maxTilesX, Main.maxTilesY];
		/*
		* Important! we assume the timers will always be the same length inside one Queue.
		* Tuple< EndTime, Pos >
		*/
		public static Queue<Tuple<int, Vector2>> timers = new();
		public static Queue<Tuple<int, Vector2>> nextTick = new();
		public static Queue<Tuple<int, Vector2>> regenTimers = new();
		public static Queue<Tuple<int, Vector2>> quickRegenTimers = new();

		public static Queue<Tuple<int, Vector2>> doorTimers = new();
		public static Queue<Tuple<int, Vector2>> nextDoorTimers = new();

		public static int Timer = 0;
		public static int regenTime = 300;

		private List<int> weaponBlockItems = new();

		internal static ModKeybind SpiderBallKey;
		internal static ModKeybind BoostBallKey;
		internal static ModKeybind PowerBombKey;
		internal static ModKeybind VisorUIKey;

		public override void Load()
		{
			SpiderBallKey = KeybindLoader.RegisterKeybind(Mod, "Spider Ball", "X");
			BoostBallKey = KeybindLoader.RegisterKeybind(Mod, "Boost Ball", "F");
			PowerBombKey = KeybindLoader.RegisterKeybind(Mod, "Power Bomb", "Z");
			// TODO: (Create and) implement Visor UI
			VisorUIKey = KeybindLoader.RegisterKeybind(Mod, "Show Visor UI", "V");
		}
		public override void Unload()
		{
			//mBlockType
			//hit.Clear();
			//dontRegen.Clear();
			timers.Clear();
			nextTick.Clear();
		}

		public override void OnWorldLoad()
		{
			weaponBlockItems = new List<int>() {
				ModContent.ItemType<Content.Items.Tiles.Destroyable.CrumbleBlock>(),
				ModContent.ItemType<Content.Items.Tiles.Destroyable.CrumbleBlockSpeed>(),
				ModContent.ItemType<Content.Items.Tiles.Destroyable.BombBlock>(),
				ModContent.ItemType<Content.Items.Tiles.Destroyable.MissileBlock>(),
				ModContent.ItemType<Content.Items.Tiles.Destroyable.FakeBlock>(),
				ModContent.ItemType<Content.Items.Tiles.Destroyable.BoostBlock>(),
				ModContent.ItemType<Content.Items.Tiles.Destroyable.PowerBombBlock>(),
				ModContent.ItemType<Content.Items.Tiles.Destroyable.SuperMissileBlock>(),
				ModContent.ItemType<Content.Items.Tiles.Destroyable.ScrewAttackBlock>(),
				ModContent.ItemType<Content.Items.Tiles.Destroyable.FakeBlockHint>(),
				ModContent.ItemType<Content.Items.Tiles.Destroyable.CrumbleBlockSlow>(),
				ModContent.ItemType<Content.Items.Tiles.Destroyable.BombBlockChain>(),
				ModContent.ItemType<Content.Items.Tools.ChoziteCutter>(),
				ModContent.ItemType<Content.Items.Tools.ChoziteWrench>()
			};
			bossesDown = MetroidBossDown.downedNone;
			mBlockType = new ushort[Main.maxTilesX, Main.maxTilesY];
			hit = new bool[Main.maxTilesX, Main.maxTilesY];
			timers = new Queue<Tuple<int, Vector2>>();
			nextTick = new Queue<Tuple<int, Vector2>>();
			regenTimers = new Queue<Tuple<int, Vector2>>();
			quickRegenTimers = new Queue<Tuple<int, Vector2>>();
			Timer = 0;
		}

		public override void PreUpdateWorld()
		{
			UpdateTimers();
			UpdateRegenTimers();
			Timer++;
		}

		public override void UpdateUI(GameTime gameTime)
		{
			Player player = Main.LocalPlayer;
			if (player.selectedItem < 10)
			{
				MetroidModPorted.Instance.oldSelectedItem = MetroidModPorted.Instance.selectedItem;
				MetroidModPorted.Instance.selectedItem = player.selectedItem;
			}
		}

		public static void AddRegenBlock(int x, int y, bool quick = false)
		{
			Vector2 pos = new(x, y);
			hit[x, y] = true;
			if (!dontRegen[x, y])
			{
				if (quick)
				{
					quickRegenTimers.Enqueue(new Tuple<int, Vector2>((int)(Timer) + regenTime / 5, pos));
				}
				else
				{
					regenTimers.Enqueue(new Tuple<int, Vector2>((int)(Timer) + regenTime, pos));
				}
			}
			if (Main.tile[(int)pos.X, (int)pos.Y].HasTile)
			{
				SoundEngine.PlaySound(SoundID.Item51, pos * 16);
				for (int d = 0; d < 4; d++)
				{
					Dust.NewDust(pos * 16, 16, 16, DustID.Stone);
				}
			}
			Wiring.DeActive(x, y);
			if (mBlockType[x, y] == 12)
			{
				int left = mBlockType[x - 1, y];
				int right = mBlockType[x + 1, y];
				int up = mBlockType[x, y - 1];
				int down = mBlockType[x, y + 1];
				if (left == 12 && Main.tile[x - 1, y].HasTile)
				{
					hit[x - 1, y] = true;
					nextTick.Enqueue(new Tuple<int, Vector2>((int)(Timer) + 1, new Vector2(x - 1, y)));
				}
				if (right == 12 && Main.tile[x + 1, y].HasTile)
				{
					hit[x + 1, y] = true;
					nextTick.Enqueue(new Tuple<int, Vector2>((int)(Timer) + 1, new Vector2(x + 1, y)));
				}
				if (up == 12 && Main.tile[x, y - 1].HasTile)
				{
					hit[x, y - 1] = true;
					nextTick.Enqueue(new Tuple<int, Vector2>((int)(Timer) + 1, new Vector2(x, y - 1)));
				}
				if (down == 12 && Main.tile[x, y + 1].HasTile)
				{
					hit[x, y + 1] = true;
					nextTick.Enqueue(new Tuple<int, Vector2>((int)(Timer) + 1, new Vector2(x, y + 1)));
				}
			}
		}

		public void UpdateTimers()
		{
			if (nextTick.Count > 0)
			{
				Tuple<int, Vector2> timer = nextTick.Peek();
				if (timer.Item1 <= Timer)
				{
					Vector2 pos = timer.Item2;
					AddRegenBlock((int)pos.X, (int)pos.Y, MSystem.mBlockType[(int)pos.X, (int)pos.Y] != 12);
					Player player = Main.player[Main.myPlayer];
					MPlayer mp = player.GetModPlayer<MPlayer>();
					if (mp.falling)
					{
						player.velocity.X = 0;
						player.oldVelocity.X = 0;
					}
					nextTick.Dequeue();
					UpdateTimers();
				}
			}
			if (timers.Count > 0)
			{
				Tuple<int, Vector2> timer = timers.Peek();
				if (timer.Item1 <= Timer)
				{
					Vector2 pos = timer.Item2;
					AddRegenBlock((int)pos.X, (int)pos.Y, true);
					Player player = Main.player[Main.myPlayer];
					MPlayer mp = player.GetModPlayer<MPlayer>();
					if (mp.falling)
					{
						player.velocity.X = 0;
						player.oldVelocity.X = 0;
					}
					timers.Dequeue();
					UpdateTimers();
				}
			}
		}

		public void UpdateRegenTimers()
		{
			if (regenTimers.Count > 0)
			{
				Tuple<int, Vector2> timer = regenTimers.Peek();
				if (timer.Item1 <= Timer)
				{
					Vector2 pos = timer.Item2;
					if (!Collision.EmptyTile((int)pos.X, (int)pos.Y, true))
					{
						quickRegenTimers.Enqueue(new Tuple<int, Vector2>((int)(Timer) + regenTime / 5, pos));
					}
					else
					{
						Wiring.ReActive((int)pos.X, (int)pos.Y);
					}
					regenTimers.Dequeue();
					UpdateRegenTimers();
				}
			}
			if (quickRegenTimers.Count > 0)
			{
				Tuple<int, Vector2> timer = quickRegenTimers.Peek();
				if (timer.Item1 <= Timer)
				{
					Vector2 pos = timer.Item2;
					if (!Collision.EmptyTile((int)pos.X, (int)pos.Y, true))
					{
						quickRegenTimers.Enqueue(new Tuple<int, Vector2>((int)(Timer) + regenTime / 5, pos));
					}
					else
					{
						Wiring.ReActive((int)pos.X, (int)pos.Y);
					}
					quickRegenTimers.Dequeue();
					UpdateRegenTimers();
				}
			}
			if (doorTimers.Count > 0)
			{
				Tuple<int, Vector2> timer = doorTimers.Peek();
				if (timer.Item1 <= Timer)
				{
					Vector2 pos = timer.Item2;
					int hatchtype = Main.tile[(int)pos.X, (int)pos.Y].TileType;
					bool open = (hatchtype == (ushort)ModContent.TileType<Content.Tiles.Hatch.BlueHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<Content.Tiles.Hatch.RedHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<Content.Tiles.Hatch.GreenHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<Content.Tiles.Hatch.YellowHatchOpen>());
					if (open)
					{
						Content.Tiles.Hatch.BlueHatch hatch = (TileLoader.GetTile(hatchtype) as Content.Tiles.Hatch.BlueHatch);
						hatch.ToggleHatch((int)pos.X, (int)pos.Y, (ushort)hatch.otherDoorID, true);
					}
					doorTimers.Dequeue();
					UpdateRegenTimers();
				}
			}
			if (nextDoorTimers.Count > 0)
			{
				Tuple<int, Vector2> timer = nextDoorTimers.Peek();
				if (timer.Item1 <= Timer)
				{
					Vector2 pos = timer.Item2;
					int hatchtype = Main.tile[(int)pos.X, (int)pos.Y].TileType;
					bool open = (hatchtype == (ushort)ModContent.TileType<Content.Tiles.Hatch.BlueHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<Content.Tiles.Hatch.RedHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<Content.Tiles.Hatch.GreenHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<Content.Tiles.Hatch.YellowHatchOpen>());
					if (open)
					{
						Content.Tiles.Hatch.BlueHatch hatch = (TileLoader.GetTile(hatchtype) as Content.Tiles.Hatch.BlueHatch);
						hatch.ToggleHatch((int)pos.X, (int)pos.Y, (ushort)hatch.otherDoorID, true);
					}
					nextDoorTimers.Dequeue();
					UpdateRegenTimers();
				}
			}
		}

		public override void SaveWorldData(TagCompound tag)
		{
			List<Vector2> positions = new();
			List<ushort> types = new();
			List<bool> regens = new();
			for (int row = 0; row < Main.maxTilesX; row++)
			{
				for (int column = 0; column < Main.maxTilesY; column++)
				{
					if (mBlockType[row, column] > 0)
					{
						positions.Add(new Vector2(row, column));
						types.Add(mBlockType[row, column]);
						regens.Add(dontRegen[row, column]);
					}
				}
			}
			tag["downed"] = (int)bossesDown;
			tag["spawnedPhazonMeteor"] = spawnedPhazonMeteor;
			tag["TorizoRoomLocation.X"] = TorizoRoomLocation.X;
			tag["TorizoRoomLocation.Y"] = TorizoRoomLocation.Y;
			tag["BlockTypePositions"] = positions;
			tag["BlockTypes"] = types;
			tag["BlockRegen"] = regens;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			int downed = tag.GetAsInt("downed");
			bossesDown = (MetroidBossDown)downed;
			spawnedPhazonMeteor = tag.Get<bool>("spawnedPhazonMeteor");

			TorizoRoomLocation.X = tag.Get<int>("TorizoRoomLocation.X");
			TorizoRoomLocation.Y = tag.Get<int>("TorizoRoomLocation.Y");
			IList<Vector2> positions = tag.GetList<Vector2>("BlockTypePositions");
			IList<ushort> types = tag.GetList<ushort>("BlockTypes");
			IList<bool> regens = tag.GetList<bool>("BlockRegen");
			for (int i = 0; i < positions.Count; i++)
			{
				Vector2 pos = positions[i];
				if (i < types.Count)
				{
					mBlockType[(int)pos.X, (int)pos.Y] = types[i];
					if (i < regens.Count)
					{
						dontRegen[(int)pos.X, (int)pos.Y] = regens[i];
					}
					Wiring.ReActive((int)pos.X, (int)pos.Y);
				}
			}
			for (int row = 0; row < Main.maxTilesX; row++)
			{
				for (int column = 0; column < Main.maxTilesY; column++)
				{
					int hatchtype = Main.tile[row, column].TileType;
					bool open = (hatchtype == (ushort)ModContent.TileType<Content.Tiles.Hatch.BlueHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<Content.Tiles.Hatch.RedHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<Content.Tiles.Hatch.GreenHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<Content.Tiles.Hatch.YellowHatchOpen>());
					if (open)
					{
						Content.Tiles.Hatch.BlueHatch hatch = (TileLoader.GetTile(hatchtype) as Content.Tiles.Hatch.BlueHatch);
						hatch.ToggleHatch(row, column, (ushort)hatch.otherDoorID, true);
					}
				}
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write((int)bossesDown);
			writer.Write(spawnedPhazonMeteor);
			writer.Write(TorizoRoomLocation.X);
			writer.Write(TorizoRoomLocation.Y);
			List<Vector2> positions = new();
			List<ushort> types = new();
			List<bool> regens = new();
			for (int row = 0; row < Main.maxTilesX; row++)
			{
				for (int column = 0; column < Main.maxTilesY; column++)
				{
					if (mBlockType[row, column] > 0)
					{
						positions.Add(new Vector2(row, column));
						types.Add(mBlockType[row, column]);
						regens.Add(dontRegen[row, column]);
					}
				}
			}
			writer.Write(types.Count);
			for (int i = 0; i < types.Count; i++)
			{
				writer.Write(positions[i].X);
				writer.Write(positions[i].Y);
				writer.Write(types[i]);
				writer.Write(regens[i]);
			}
		}

		public override void NetReceive(BinaryReader reader)
		{
			bossesDown = (MetroidBossDown)reader.ReadInt32();
			spawnedPhazonMeteor = reader.ReadBoolean();
			TorizoRoomLocation.X = reader.ReadInt32();
			TorizoRoomLocation.Y = reader.ReadInt32();
			mBlockType = new ushort[Main.maxTilesX, Main.maxTilesY];
			dontRegen = new bool[Main.maxTilesX, Main.maxTilesY];
			int count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				int x = reader.ReadInt32();
				int y = reader.ReadInt32();
				mBlockType[x, y] = (ushort)reader.ReadInt16();
				dontRegen[x, y] = (bool)reader.ReadBoolean();
			}
		}

		public override void PostDrawTiles()
		{
			SpriteBatch spriteBatch = Main.spriteBatch;
			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int x1 = (int)((Main.screenPosition.X) / 16f - 1f);
			int x2 = (int)((Main.screenPosition.X + (float)Main.screenWidth) / 16f) + 2;
			int y1 = (int)((Main.screenPosition.Y) / 16f - 1f);
			int y2 = (int)((Main.screenPosition.Y + (float)Main.screenHeight) / 16f) + 5;
			if (x1 < 0)
			{
				x1 = 0;
			}
			if (x2 > Main.maxTilesX)
			{
				x2 = Main.maxTilesX;
			}
			if (y1 < 0)
			{
				y1 = 0;
			}
			if (y2 > Main.maxTilesY)
			{
				y2 = Main.maxTilesY;
			}
			for (int i = x1; i < x2; i++)
			{
				for (int j = y1; j < y2; j++)
				{
					//Tile tile = Main.tile[i, j];
					Color color = Lighting.GetColor(i, j);
					if (!Main.tile[i, j].HasTile || Main.tile[i, j].IsActuated || !Main.tileSolid[Main.tile[i, j].TileType])
					{
						color *= 0.5f;
					}
					bool draw = false;
					if (Main.myPlayer < 256 && Main.myPlayer >= 0)
					{
						draw = weaponBlockItems.Contains(Main.player[Main.myPlayer].HeldItem.type);
					}
					float scale = 1f;
					Vector2 screenPos = Main.screenPosition;
					int xOff = -12 * 16;
					int yOff = -12 * 16;
					Vector2 drawPos = new Vector2((float)(i * 16 + xOff - (int)screenPos.X), (float)(j * 16 + yOff - (int)screenPos.Y)) + zero;

					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, null, null, Main.GameViewMatrix.ZoomMatrix);

					bool revealed = (hit[i, j] && (Main.tile[i, j].HasTile && !Main.tile[i, j].IsActuated));
					if (draw || revealed)
					{
						if (dontRegen[i, j] && draw)
						{
							color.B /= 2;
							color.G /= 2;
						}
						if (mBlockType[i, j] == 1)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/CrumbleBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 2)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/CrumbleBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 3)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/BombBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 4)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/MissileBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 5)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/FakeBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 6)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/BoostBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 7)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/PowerBombBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 8)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/SuperMissileBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 9)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/ScrewAttackBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 10)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/FakeBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 11)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/CrumbleBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 12)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/BombBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
					}
					if (!revealed)
					{
						if (mBlockType[i, j] == 10)
						{
							color = new Color(color.R, color.G, color.B, 64);
							spriteBatch.Draw(ModContent.Request<Texture2D>("Assets/Textures/Breakable/FakeBlockHint").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
					}

					spriteBatch.End();
				}
			}
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
			int PotsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
			if (ShiniesIndex != -1)
			{
				tasks.Insert(ShiniesIndex + 1, new PassLegacy("Chozite Ore", delegate {
					//progress.Message = "Generating Chozite Ore";

					for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 9E-05); k++)
					{
						WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY), (double)WorldGen.genRand.Next(4, 7), WorldGen.genRand.Next(4, 7), ModContent.TileType<Content.Tiles.ChoziteOreTile>(), false, 0f, 0f, false, true);
					}
				}));
			}
			if (PotsIndex != -1)
			{
				tasks.Insert(PotsIndex - 3, new PassLegacy("Chozo Statues", delegate {
					//progress.Message = "Placing Chozo Statues";
					for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 1E-05); i++)
					{
						float num2 = (float)((double)i / ((double)(Main.maxTilesX * Main.maxTilesY) * 1E-05));
						bool flag = false;
						int num3 = 0;
						while (!flag)
						{
							if (AddChozoStatue(WorldGen.genRand.Next(100, Main.maxTilesX - 100), WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 100)))
							{
								flag = true;
							}
							else
							{
								num3++;
								if (num3 >= 10000)
								{
									flag = true;
								}
							}
						}

					}
				}));
				/*tasks.Insert(PotsIndex - 2, new PassLegacy("Missile Expansions", delegate {
					//progress.Message = "Placing Missile Expansions";
					for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 15E-06); i++)
					{
						float num2 = (float)((double)i / ((double)(Main.maxTilesX * Main.maxTilesY) * 15E-06));
						bool flag = false;
						int num3 = 0;
						while (!flag)
						{
							if (AddExpansion(WorldGen.genRand.Next(1, Main.maxTilesX), WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 100)))
							{
								flag = true;
							}
							else
							{
								num3++;
								if (num3 >= 10000)
								{
									flag = true;
								}
							}
						}

					}
				}));*/

				/*tasks.Insert(PotsIndex - 1, new PassLegacy("Chozo Ruins", ChozoRuins));*/
			}
		}

		public static ushort StatueItem(int i, int j)
		{
			//Mod mod = MetroidModPorted.Instance;

			bool dungeon = Main.wallDungeon[(int)Main.tile[i, j].WallType];
			bool jungle = (i >= Main.maxTilesX * 0.2 && i <= Main.maxTilesX * 0.35);
			if (WorldGen.dEnteranceX < Main.maxTilesX / 2)
			{
				jungle = (i >= Main.maxTilesX * 0.65 && i <= Main.maxTilesX * 0.8);
			}

			ushort item  = (ushort)ModContent.TileType<Content.Tiles.ItemTile.MorphBallTile>();
			if (dungeon)
			{
				//item = (ushort)ModContent.Find<ModTile>("IceBeamTile").Type;
			}
			else if (jungle && WorldGen.genRand.Next(10) <= 5)
			{
				//item = (ushort)ModContent.Find<ModTile>("SpazerTile").Type;
			}
			else
			{
				int baseX = Main.maxTilesX / 2;
				int baseY = (int)WorldGen.rockLayer;
				//float dist = (float)((Math.Abs(i - baseX) / (Main.maxTilesX / 2)) + (Math.Max(j - baseY, 0) / (Main.maxTilesY - WorldGen.rockLayer))) / 2;

				//int rand = WorldGen.genRand.Next((int)Math.Max(100 * (1 - dist), 5));
				WeightedChance[] list = new WeightedChance[SuitAddonLoader.AddonCount + BeamLoader.BeamCount + MBAddonLoader.AddonCount + 2];
				int index = 0;
				// Okay, the goal is to do weighted random.
				foreach(ModSuitAddon addon in SuitAddonLoader.addons)
				{
					if (addon.CanGenerateOnChozoStatue(Main.tile[i, j])) { list[index++] = new WeightedChance(() => { item = (ushort)addon.TileType; }, RarityLoader.RarityCount - addon.Item.rare); }
				}
				foreach(ModBeam beam in BeamLoader.beams)
				{
					if (beam.CanGenerateOnChozoStatue(Main.tile[i, j])) { list[index++] = new WeightedChance(() => { item = (ushort)beam.TileType; }, RarityLoader.RarityCount - beam.Item.Item.rare); }
				}
				foreach(ModMBAddon addon in MBAddonLoader.addons)
				{
					if (addon.CanGenerateOnChozoStatue(Main.tile[i, j])) { list[index++] = new WeightedChance(() => { item = (ushort)addon.TileType; }, RarityLoader.RarityCount - addon.Item.rare); }
				}
				list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.MorphBallTile>(); }, RarityLoader.RarityCount - 4);
				//list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.XRayScopeTile>(); }, RarityLoader.RarityCount - 4);
				Array.Resize(ref list, index);
				double numericValue = WorldGen.genRand.Next(0, (int)list.Sum(p => p.Ratio));

				foreach (WeightedChance parameter in list)
				{
					numericValue -= parameter.Ratio;

					if (!(numericValue <= 0)) { continue; }

					parameter.Func();
					break;
				}
				/*if (rand < 1)
				{
					item = (ushort)ModContent.Find<ModTile>("SpiderBallTile").Type;
				}
				else if (rand < 3)
				{
					item = (ushort)ModContent.Find<ModTile>("XRayScopeTile").Type;
				}
				else if (rand < 8)
				{
					item = (ushort)ModContent.Find<ModTile>("HiJumpBootsTile").Type;
				}
				else if (rand < 13)
				{
					item = (ushort)ModContent.Find<ModTile>("BoostBallTile").Type;
				}
				else if (rand < 25)
				{
					item = (ushort)ModContent.Find<ModTile>("WaveBeamTile").Type;
				}
				else if (rand < 40)
				{
					item = (ushort)ModContent.Find<ModTile>("BombTile").Type;
				}
				else if (rand < 60)
				{
					item = (ushort)ModContent.Find<ModTile>("ChargeBeamTile").Type;
				}
				else
				{
					item = (ushort)ModContent.Find<ModTile>("MorphBallTile").Type;
				}*/
			}
			return item;
		}
		public static bool AddChozoStatue(int i, int j)
		{
			//Mod mod = MetroidModPorted.Instance;

			int k = j;
			while (k < Main.maxTilesY)
			{
				int num2 = 0;
				ushort type = TileID.Stone;
				for (int l = 0; l < 3; l++)
				{
					if (Main.tile[i + l, k].HasTile && Main.tileSolid[(int)Main.tile[i + l, k].TileType])
					{
						num2++;
						type = Main.tile[i + l, k].TileType;
					}
				}
				if (num2 >= 2)// && !Main.tile[i, k - 1].active() && !Main.tile[i, k - 2].active() && !Main.tile[i, k - 3].active() && !Main.tile[i + 1, k - 1].active() && !Main.tile[i + 1, k - 2].active() && !Main.tile[i + 1, k - 3].active() && !Main.tile[i + 2, k - 1].active() && !Main.tile[i + 2, k - 2].active() && !Main.tile[i + 2, k - 3].active())
				{
					int num = k - 1;
					if (Main.tile[i, num - 1].LiquidType == LiquidID.Lava || Main.tile[i + 1, num - 1].LiquidType == LiquidID.Lava || Main.tile[i + 2, num - 1].LiquidType == LiquidID.Lava)
					{
						return false;
					}
					if (!WorldGen.EmptyTileCheck(i, i + 2, num - 2, num, -1))
					{
						return false;
					}
					if (Main.tile[i, num].WallType == WallID.LihzahrdBrickUnsafe)
					{
						return false;
					}

					int rand = WorldGen.genRand.Next(4); // 0 & 1 = statue only, 2 = statue + brick base, 3 = statue + brick shrine
					if (Main.wallDungeon[(int)Main.tile[i, num].WallType])
					{
						rand = 0;
					}

					if (rand <= 1)
					{
						for (int tx = i; tx < i + 3; tx++)
						{
							Main.tile[tx, k].Get<TileWallWireStateData>().Slope = SlopeType.Solid;
							Main.tile[tx, k].Get<TileWallWireStateData>().IsHalfBlock = false;
							if (!Main.tile[tx, k].HasTile)
							{
								Main.tile[tx, k].Get<TileWallWireStateData>().HasTile = true;
								Main.tile[tx, k].Get<TileTypeData>().Type = type;
							}
						}
					}
					else
					{
						if (rand == 3)
						{
							for (int wx = i - 1; wx < i + 4; wx++)
							{
								for (int wy = k - 4; wy < k; wy++)
								{
									WorldGen.KillTile(wx, wy);

									if (WorldGen.genRand.Next(4) > 0)
									{
										WorldGen.KillWall(wx, wy);
										WorldGen.PlaceWall(wx, wy, WallID.Cave4Unsafe);
									}
								}
							}
							for (int tx = i - 2; tx < i + 5; tx++)
							{
								Main.tile[tx, k].Get<TileWallWireStateData>().Slope = SlopeType.Solid;
								Main.tile[tx, k].Get<TileWallWireStateData>().IsHalfBlock = false;
								Main.tile[tx, k].Get<TileWallWireStateData>().HasTile = true;
								Main.tile[tx, k].Get<TileTypeData>().Type = 38;
								Main.tile[tx, k - 5].Get<TileWallWireStateData>().Slope = SlopeType.Solid;
								Main.tile[tx, k - 5].Get<TileWallWireStateData>().IsHalfBlock = false;
								Main.tile[tx, k - 5].Get<TileWallWireStateData>().HasTile = true;
								Main.tile[tx, k - 5].Get<TileTypeData>().Type = 38;
							}
							Main.tile[i - 2, k - 4].Get<TileWallWireStateData>().Slope = SlopeType.Solid;
							Main.tile[i - 2, k - 4].Get<TileWallWireStateData>().IsHalfBlock = false;
							Main.tile[i - 2, k - 4].Get<TileWallWireStateData>().HasTile = true;
							Main.tile[i - 2, k - 4].Get<TileTypeData>().Type = 38;

							Main.tile[i + 4, k - 4].Get<TileWallWireStateData>().Slope = SlopeType.Solid;
							Main.tile[i + 4, k - 4].Get<TileWallWireStateData>().IsHalfBlock = false;
							Main.tile[i + 4, k - 4].Get<TileWallWireStateData>().HasTile = true;
							Main.tile[i + 4, k - 4].Get<TileTypeData>().Type = 38;
						}
						for (int tx2 = i - 1; tx2 < i + 4; tx2++)
						{
							Main.tile[tx2, k].Get<TileWallWireStateData>().Slope = SlopeType.Solid;
							Main.tile[tx2, k].Get<TileWallWireStateData>().IsHalfBlock = false;
							Main.tile[tx2, k].Get<TileWallWireStateData>().HasTile = true;
							Main.tile[tx2, k].Get<TileTypeData>().Type = 38;
							if (rand == 3)
							{
								Main.tile[tx2, k - 6].Get<TileWallWireStateData>().Slope = SlopeType.Solid;
								Main.tile[tx2, k - 6].Get<TileWallWireStateData>().IsHalfBlock = false;
								Main.tile[tx2, k - 6].Get<TileWallWireStateData>().HasTile = true;
								Main.tile[tx2, k - 6].Get<TileTypeData>().Type = 38;
							}
						}
					}

					int dir = 1;
					if (WorldGen.genRand.NextBool(2))
					{
						dir = -1;
					}

					int statueX = i + 2;
					int statueX2 = i;
					if (dir == 1)
					{
						statueX = i + 1;
						statueX2 = i + 2;
					}
					int statueY = num;

					WorldGen.PlaceObject(statueX, statueY, ModContent.TileType<Content.Tiles.ChozoStatueNatural>(), false, 0, 0, -1, -dir);
					WorldGen.PlaceObject(statueX2, statueY, ModContent.TileType<Content.Tiles.ChozoStatueArmNatural>(), false, 0, 0, -1, -dir);

					ushort item = StatueItem(statueX2, statueY - 2);
					WorldGen.PlaceObject(statueX2, statueY - 2, item);

					return true;
				}
				else
				{
					k++;
				}
			}
			return false;
		}
		public static bool AddExpansion(int i, int j)
		{
			//Mod mod = MetroidModPorted.Instance;
			int k = j;
			while (k < Main.maxTilesY)
			{
				if (Main.tile[i, k].HasTile && Main.tileSolid[(int)Main.tile[i, j].TileType] && !Main.tile[i, k - 1].HasTile)
				{
					int num = k - 1;
					if (Main.tile[i, num].LiquidType == LiquidID.Lava || Main.tile[i, num - 1].LiquidType == LiquidID.Lava)
					{
						return false;
					}
					Main.tile[i, k].Get<TileWallWireStateData>().Slope = SlopeType.Solid;
					Main.tile[i, k].Get<TileWallWireStateData>().IsHalfBlock = false;
					Main.tile[i, num].Get<TileWallWireStateData>().HasTile = true;
					Main.tile[i, num].Get<TileTypeData>().Type = (ushort)ModContent.Find<ModTile>($"MissileExpansionTile").Type;

					Main.tile[i, num].Get<TileWallWireStateData>().TileFrameX = 0;
					Main.tile[i, num].Get<TileWallWireStateData>().TileFrameY = 0;
					return true;
				}
				else
				{
					k++;
				}
			}
			return false;
		}
		public override void PreUpdateEntities()
		{
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.BlueHatchOpen>()] = false;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.BlueHatchOpenVertical>()] = false;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.RedHatchOpen>()] = false;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.RedHatchOpenVertical>()] = false;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.GreenHatchOpen>()] = false;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.GreenHatchOpenVertical>()] = false;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.YellowHatchOpen>()] = false;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.YellowHatchOpenVertical>()] = false;
		}

		//public override void MidUpdateTimeWorld()
		public override void PostUpdateTime()
		{
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.BlueHatchOpen>()] = true;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.BlueHatchOpenVertical>()] = true;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.RedHatchOpen>()] = true;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.RedHatchOpenVertical>()] = true;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.GreenHatchOpen>()] = true;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.GreenHatchOpenVertical>()] = true;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.YellowHatchOpen>()] = true;
			Main.tileSolid[ModContent.TileType<Content.Tiles.Hatch.YellowHatchOpenVertical>()] = true;
		}
	}
}
