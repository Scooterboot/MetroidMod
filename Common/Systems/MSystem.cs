﻿#region Using directives

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Localization;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MetroidMod.Common.Players;
using MetroidMod.Content.Tiles;
using MetroidMod.Content.Tiles.ItemTile;
using MetroidMod.Content.Tiles.Hatch;
using MetroidMod.Content.Walls;
using MetroidMod.Content.NPCs.GoldenTorizo;
using MetroidMod.Content.NPCs.Torizo;

#endregion

namespace MetroidMod.Common.Systems
{
	[LegacyName("MWorld")]
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
		public override void AddRecipeGroups()
		{
			MetroidMod.MorphBallBombsRecipeGroupID = RecipeGroup.RegisterGroup("MetroidMod:MorphBallBombs", new RecipeGroup(() => "Any Morph Ball Bombs", MBAddonLoader.GetAddon<Content.MorphBallAddons.Bomb>().ItemType) { IconicItemId = MBAddonLoader.GetAddon<Content.MorphBallAddons.Bomb>().ItemType });

			MetroidMod.T1PHMBarRecipeGroupID = RecipeGroup.RegisterGroup("MetroidMod:Tier1PHMBar", new RecipeGroup(() => "Any Copper-Tier Bar", ItemID.CopperBar, ItemID.TinBar) { IconicItemId = ItemID.CopperBar });
			MetroidMod.GoldPlatinumBarRecipeGroupID = RecipeGroup.RegisterGroup("MetroidMod:GoldPlatinumBar", new RecipeGroup(() => "Any Gold-Tier Bar", ItemID.GoldBar, ItemID.PlatinumBar) { IconicItemId = ItemID.GoldBar });
			MetroidMod.EvilBarRecipeGroupID = RecipeGroup.RegisterGroup("MetroidMod:WorldEvilBar", new RecipeGroup(() => "Any Evil Bar", ItemID.DemoniteBar, ItemID.CrimtaneBar) { IconicItemId = ItemID.DemoniteBar });
			MetroidMod.EvilMaterialRecipeGroupID = RecipeGroup.RegisterGroup("MetroidMod:WorldEvilMaterial", new RecipeGroup(() => "Any Pre-Hardmode Evil Material", ItemID.ShadowScale, ItemID.TissueSample) { IconicItemId = ItemID.ShadowScale });
			MetroidMod.EvilHMMaterialRecipeGroupID = RecipeGroup.RegisterGroup("MetroidMod:WorldEvilHMMaterial", new RecipeGroup(() => "Any Hardmode Evil Material", ItemID.CursedFlame, ItemID.Ichor) { IconicItemId = ItemID.CursedFlame });
			MetroidMod.T1HMBarRecipeGroupID = RecipeGroup.RegisterGroup("MetroidMod:Tier1HMBar", new RecipeGroup(() => "Any Tier-1 Hardmode Bar", ItemID.CobaltBar, ItemID.PalladiumBar) { IconicItemId = ItemID.CobaltBar });
			MetroidMod.T2HMBarRecipeGroupID = RecipeGroup.RegisterGroup("MetroidMod:Tier2HMBar", new RecipeGroup(() => "Any Tier-2 Hardmode Bar", ItemID.MythrilBar, ItemID.OrichalcumBar) { IconicItemId = ItemID.MythrilBar });
			MetroidMod.T3HMBarRecipeGroupID = RecipeGroup.RegisterGroup("MetroidMod:Tier3HMBar", new RecipeGroup(() => "Any Tier-3 Hardmode Bar", ItemID.AdamantiteBar, ItemID.TitaniumBar) { IconicItemId = ItemID.AdamantiteBar });
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
				MetroidMod.Instance.oldSelectedItem = MetroidMod.Instance.selectedItem;
				MetroidMod.Instance.selectedItem = player.selectedItem;
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
				if (left == 12 && Main.tile[x - 1, y].HasTile && !Main.tile[x - 1, y].IsActuated)
				{
					hit[x - 1, y] = true;
					nextTick.Enqueue(new Tuple<int, Vector2>((int)(Timer) + 1, new Vector2(x - 1, y)));
				}
				if (right == 12 && Main.tile[x + 1, y].HasTile && !Main.tile[x + 1, y].IsActuated)
				{
					hit[x + 1, y] = true;
					nextTick.Enqueue(new Tuple<int, Vector2>((int)(Timer) + 1, new Vector2(x + 1, y)));
				}
				if (up == 12 && Main.tile[x, y - 1].HasTile && !Main.tile[x, y - 1].IsActuated)
				{
					hit[x, y - 1] = true;
					nextTick.Enqueue(new Tuple<int, Vector2>((int)(Timer) + 1, new Vector2(x, y - 1)));
				}
				if (down == 12 && Main.tile[x, y + 1].HasTile && !Main.tile[x, y + 1].IsActuated)
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
					bool open = (hatchtype == (ushort)ModContent.TileType<BlueHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<BlueHatchOpenVertical>()
							|| hatchtype == (ushort)ModContent.TileType<RedHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<RedHatchOpenVertical>()
							|| hatchtype == (ushort)ModContent.TileType<GreenHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<GreenHatchOpenVertical>()
							|| hatchtype == (ushort)ModContent.TileType<YellowHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<YellowHatchOpenVertical>());
					if (open)
					{
						BlueHatch hatch = (TileLoader.GetTile(hatchtype) as BlueHatch);
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
					bool open = (hatchtype == (ushort)ModContent.TileType<BlueHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<BlueHatchOpenVertical>()
							|| hatchtype == (ushort)ModContent.TileType<RedHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<RedHatchOpenVertical>()
							|| hatchtype == (ushort)ModContent.TileType<GreenHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<GreenHatchOpenVertical>()
							|| hatchtype == (ushort)ModContent.TileType<YellowHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<YellowHatchOpenVertical>());
					if (open)
					{
						BlueHatch hatch = (TileLoader.GetTile(hatchtype) as BlueHatch);
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
					bool open = (hatchtype == (ushort)ModContent.TileType<BlueHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<BlueHatchOpenVertical>()
							|| hatchtype == (ushort)ModContent.TileType<RedHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<RedHatchOpenVertical>()
							|| hatchtype == (ushort)ModContent.TileType<GreenHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<GreenHatchOpenVertical>()
							|| hatchtype == (ushort)ModContent.TileType<YellowHatchOpen>()
							|| hatchtype == (ushort)ModContent.TileType<YellowHatchOpenVertical>());
					if (open)
					{
						BlueHatch hatch = (TileLoader.GetTile(hatchtype) as BlueHatch);
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
				int x = (int)reader.ReadSingle();
				int y = (int)reader.ReadSingle();
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
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/CrumbleBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 2)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/CrumbleBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 3)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/BombBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 4)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/MissileBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 5)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/FakeBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 6)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/BoostBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 7)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/PowerBombBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 8)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/SuperMissileBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 9)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/ScrewAttackBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 10)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/FakeBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 11)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/CrumbleBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
						if (mBlockType[i, j] == 12)
						{
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/BombBlock").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
						}
					}
					if (!revealed)
					{
						if (mBlockType[i, j] == 10)
						{
							color = new Color(color.R, color.G, color.B, 64);
							spriteBatch.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Breakable/FakeBlockHint").Value, drawPos, new Rectangle(0, 0, 16, 16), color, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
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
				tasks.Insert(ShiniesIndex + 1, new PassLegacy("Chozite Ore", delegate (GenerationProgress progress, GameConfiguration configuration) {
					progress.Message = "Generating Chozite Ore";

					for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 9E-05); k++)
					{
						WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY), (double)WorldGen.genRand.Next(4, 7), WorldGen.genRand.Next(4, 7), ModContent.TileType<Content.Tiles.ChoziteOreTile>(), false, 0f, 0f, false, true);
					}
				}));
			}
			if (PotsIndex != -1)
			{
				tasks.Insert(PotsIndex - 3, new PassLegacy("Chozo Statues", delegate (GenerationProgress progress, GameConfiguration configuration) {
					progress.Message = "Placing Chozo Statues";
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
				tasks.Insert(PotsIndex - 2, new PassLegacy("Missile Expansions", delegate (GenerationProgress progress, GameConfiguration configuration) {
					progress.Message = "Placing Missile Expansions";
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
				}));

				tasks.Insert(PotsIndex - 1, new PassLegacy("Chozo Ruins", ChozoRuins));
			}
		}

		public static ushort StatueItem(int i, int j)
		{
			//Mod mod = MetroidMod.Instance;

			bool dungeon = Main.wallDungeon[(int)Main.tile[i, j].WallType];
			bool jungle = (i >= Main.maxTilesX * 0.2 && i <= Main.maxTilesX * 0.35);
			if (WorldGen.dEnteranceX < Main.maxTilesX / 2)
			{
				jungle = (i >= Main.maxTilesX * 0.65 && i <= Main.maxTilesX * 0.8);
			}

			ushort item  = (ushort)ModContent.TileType<MorphBallTile>();
			if (dungeon)
			{
				item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.IceBeamTile>();
			}
			else if (jungle && WorldGen.genRand.Next(10) <= 5)
			{
				item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.SpazerTile>();
			}
			else
			{
				int baseX = Main.maxTilesX / 2;
				int baseY = (int)WorldGen.rockLayer;
				//float dist = (float)((Math.Abs(i - baseX) / (Main.maxTilesX / 2)) + (Math.Max(j - baseY, 0) / (Main.maxTilesY - WorldGen.rockLayer))) / 2;

				//int rand = WorldGen.genRand.Next((int)Math.Max(100 * (1 - dist), 5));
				WeightedChance[] list = new WeightedChance[SuitAddonLoader.AddonCount + 2 + MBAddonLoader.AddonCount + 3];
				int index = 0;
				// Okay, the goal is to do weighted random.
				foreach (ModSuitAddon addon in SuitAddonLoader.addons)
				{
					if (addon.CanGenerateOnChozoStatue(i, j)) { list[index++] = new WeightedChance(() => { item = (ushort)addon.TileType; }, addon.GenerationChance(i, j)); }
				}
				/*foreach(ModBeam beam in BeamLoader.beams)
				{
					if (beam.CanGenerateOnChozoStatue(i, j)) { list[index++] = new WeightedChance(() => { item = (ushort)beam.TileType; }, RarityLoader.RarityCount - beam.Item.Item.rare); }
				}*/
				foreach (ModMBAddon addon in MBAddonLoader.addons)
				{
					if (addon.CanGenerateOnChozoStatue(i, j)) { list[index++] = new WeightedChance(() => { item = (ushort)addon.TileType; }, addon.GenerationChance(i, j)); }
				}
				list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.ShockCoilTile>(); }, RarityLoader.RarityCount - 3);
				list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.MagMaulTile>(); }, RarityLoader.RarityCount - 3);
				list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.BattleHammerTile>(); }, RarityLoader.RarityCount - 3);
				list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.VoltDriverTile>(); }, RarityLoader.RarityCount - 3);
				list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.ImperialistTile>(); }, RarityLoader.RarityCount - 3);
				list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.JudicatorTile>(); }, RarityLoader.RarityCount - 3);
				//list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.MorphBallTile>(); }, RarityLoader.RarityCount - 4);
				//list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.XRayScopeTile>(); }, RarityLoader.RarityCount - 4);
				list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.ChargeBeamTile>(); }, 32);
				list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.WaveBeamTile>(); }, 24);
				if (WorldGen.drunkWorldGen)
				{
					list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.HyperBeamTile>(); }, 1);
					list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.PhazonBeamTile>(); }, 1);
                    list[index++] = new WeightedChance(() => { item = (ushort)ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.OmegaCannonTile>(); }, 1);
                }
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
			//Mod mod = MetroidMod.Instance;

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
				if (num2 >= 2)// && !Main.tile[i, k - 1].HasTile && !Main.tile[i, k - 2].HasTile && !Main.tile[i, k - 3].HasTile && !Main.tile[i + 1, k - 1].HasTile && !Main.tile[i + 1, k - 2].HasTile && !Main.tile[i + 1, k - 3].HasTile && !Main.tile[i + 2, k - 1].HasTile && !Main.tile[i + 2, k - 2].HasTile && !Main.tile[i + 2, k - 3].HasTile)
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

					WorldGen.PlaceObject(statueX, statueY, ModContent.TileType<ChozoStatueNatural>(), false, 0, 0, -1, -dir);
					WorldGen.PlaceObject(statueX2, statueY, ModContent.TileType<ChozoStatueArmNatural>(), false, 0, 0, -1, -dir);

					ushort item = StatueItem(statueX2, statueY - 2);
					Main.tile[statueX2, statueY - 2].Get<TileWallWireStateData>().HasTile = true;
					Main.tile[statueX2, statueY - 2].Get<TileTypeData>().Type = item;

					Main.tile[statueX2, statueY - 2].Get<TileWallWireStateData>().TileFrameX = 0;
					Main.tile[statueX2, statueY - 2].Get<TileWallWireStateData>().TileFrameY = 0;

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
			//Mod mod = MetroidMod.Instance;
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
					Main.tile[i, num].Get<TileTypeData>().Type = (ushort)ModContent.TileType<MissileExpansionTile>();

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


		private void ChozoRuins(GenerationProgress progress, GameConfiguration configuration)
		{
			Rectangle uDesert = WorldGen.UndergroundDesertLocation;

			progress.Message = "Chozo Ruins...Determining X Position";

			int ruinsWidth = WorldGen.genRand.Next(50, 60);
			int ruinsHeight = WorldGen.genRand.Next(20, 30);

			int ruinsX = 0;

			int dir = 1;

			int center = (int)(uDesert.X + uDesert.Width / 2);

			if (Main.maxTilesX / 2 < center)
			{
				dir = 1;
			}
			else
			{
				dir = -1;
			}

			//------ debug code
			/*if(WorldGen.crimson)
			{
				dir = -1;
			}
			else
			{
				dir = 1;
			}*/
			//------

			int surface = uDesert.Y + uDesert.Height / 2;

			if (dir == 1)
			{
				//ruinsX = uDesert.X+uDesert.Width-ruinsWidth - (int)uDesert.Width/8;
				int numX = uDesert.X + uDesert.Width - ruinsWidth - (int)uDesert.Width / 8;
				while (numX > uDesert.X + uDesert.Width / 2 + 30)
				{
					int numY2 = 0;
					while (numY2 < surface)
					{
						if (Main.tile[numX, numY2].HasTile && Main.tile[numX, numY2].TileType == TileID.Sand)
						{
							break;
						}
						numY2++;
					}
					if (!Main.tile[numX, numY2 - 1].HasTile)
					{
						break;
					}
					numX--;
				}
				ruinsX = numX;
			}
			else
			{
				//ruinsX = uDesert.X + (int)uDesert.Width/8;
				int numX = uDesert.X + ruinsWidth + (int)uDesert.Width / 8;
				while (numX < uDesert.X + uDesert.Width / 2 - 30)
				{
					int numY2 = 0;
					while (numY2 < surface)
					{
						if (Main.tile[numX, numY2].HasTile && Main.tile[numX, numY2].TileType == TileID.Sand)
						{
							break;
						}
						numY2++;
					}
					if (!Main.tile[numX, numY2 - 1].HasTile)
					{
						break;
					}
					numX++;
				}
				ruinsX = numX - ruinsWidth;
			}

			progress.Message = "Chozo Ruins...Determining Y Position";

			int ruinsY = 0;
			while ((double)ruinsY < surface)
			{
				if (Main.tile[ruinsX, ruinsY].HasTile && (Main.tile[ruinsX, ruinsY].TileType == TileID.Sand || dir == -1))
				{
					break;
				}
				ruinsY++;
			}
			int numY = 0;
			while ((double)numY < surface)
			{
				if (Main.tile[ruinsX + ruinsWidth, numY].HasTile && (Main.tile[ruinsX + ruinsWidth, numY].TileType == TileID.Sand || dir == 1))
				{
					break;
				}
				numY++;
			}
			if (numY > ruinsY)
			{
				ruinsY = numY;
			}

			progress.Message = "Chozo Ruins...Building Temple";

			ChozoRuins_Temple(ruinsX, ruinsY - ruinsHeight + 2 + WorldGen.genRand.Next(3), ruinsWidth, ruinsHeight, dir);
		}
		private static void ChozoRuins_Temple(int x, int y, int width, int height, int dir)
		{
			BasicStructure(x, y, width, height, 4, ModContent.TileType<ChozoBrickNatural>(), ModContent.WallType<ChozoBrickWallNatural>(), 1);

			Hatch(x, y + height - 8);
			Hatch(x + width - 4, y + height - 8);

			for (int i = 0; i < 4; i++)
			{
				WorldGen.KillWall(x, y + height - 8 + i);
				WorldGen.PlaceWall(x, y + height - 8 + i, ModContent.WallType<ChozoBrickWallNatural>());
				WorldGen.KillWall(x + width - 1, y + height - 8 + i);
				WorldGen.PlaceWall(x + width - 1, y + height - 8 + i, ModContent.WallType<ChozoBrickWallNatural>());
			}

			for (int i = 0; i < 7; i++)
			{
				WorldGen.KillTile(x - 1, y + height - 15 + i);
				WorldGen.PlaceTile(x - 1, y + height - 15 + i, ModContent.TileType<ChozoBrickNatural>());
				WorldGen.KillTile(x + width, y + height - 15 + i);
				WorldGen.PlaceTile(x + width, y + height - 15 + i, ModContent.TileType<ChozoBrickNatural>());
			}
			Tile.SmoothSlope(x - 1, y + height - 15, false);
			Tile.SmoothSlope(x + width, y + height - 15, false);

			for (int j = 0; j < 6; j++)
			{
				for (int i = -1 - j; i < width + 1 + j; i++)
				{
					int xx = x + i, yy = y + height - 4 + j;
					//if(Main.tile[xx, yy].HasTile || i >= -1 && i <= width)
					//{
					if (j != 0 || i < 0 || i >= width)
					{
						DestroyChest(xx, yy);
						WorldGen.KillTile(xx, yy);
					}
					WorldGen.PlaceTile(xx, yy, ModContent.TileType<ChozoBrickNatural>());
					//}
				}
			}

			int shaftWidth = 24;
			int shaftX = x + width - 4 - shaftWidth - WorldGen.genRand.Next(width / 6);
			if (dir == 1)
			{
				shaftX = x + 4 + WorldGen.genRand.Next(width / 6);
			}
			ChozoRuins_FirstShaft(shaftX, y + height - 4, shaftWidth, WorldGen.genRand.Next(50, 60), dir);
		}
		private static void ChozoRuins_FirstShaft(int x, int y, int width, int height, int dir)
		{
			BasicStructure(x, y, width, height, 4, ModContent.TileType<ChozoBrickNatural>(), ModContent.WallType<ChozoBrickWallNatural>(), 1);

			VerticalHatch(x + width / 2 - 2, y);

			for (int i = -3; i < 3; i++)
			{
				WorldGen.PlaceTile(x + width / 2 + i, y + 6, 19, false, false, -1, 17);
			}
			for (int j = 11; j < height - 5; j += 5)
			{
				for (int i = 0; i < 3; i++)
				{
					int platform = x + 6 + Main.rand.Next(12);
					WorldGen.PlaceTile(platform - 1, y + j, 19, false, false, -1, 17);
					WorldGen.PlaceTile(platform, y + j, 19, false, false, -1, 17);
					WorldGen.PlaceTile(platform + 1, y + j, 19, false, false, -1, 17);
				}
			}

			int chestRoomWidth = 20;
			int chestRoomHeight = 16;
			int chestRoomX = x + width - 4;
			int chestRoomY = y + height - chestRoomHeight - WorldGen.genRand.Next(height / 2);
			int doorX = chestRoomX;
			int doorY = chestRoomY + chestRoomHeight / 2 - 2;

			int numX = doorX - 2;
			if (dir == -1)
			{
				chestRoomX = x - chestRoomWidth + 4;
				doorX = x;
				numX = doorX + 4;
			}
			int numY = doorY + 4;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					WorldGen.KillTile(numX + i, numY + j);
					WorldGen.PlaceTile(numX + i, numY + j, ModContent.TileType<ChozoBrickNatural>());
				}
			}

			ChozoRuins_ChestRoom(chestRoomX, chestRoomY, chestRoomWidth, chestRoomHeight, ItemID.FlyingCarpet);
			Hatch(doorX, doorY);

			int morphHallWidth = WorldGen.genRand.Next(40, 50);
			int morphHallHeight = 22;
			int morphHallX = x - morphHallWidth + 4;
			int morphHallY = y + WorldGen.genRand.Next(16, 24);
			doorX = x;
			doorY = morphHallY + morphHallHeight - 10;
			numX = doorX + 4;
			if (dir == -1)
			{
				morphHallX = x + width - 4;
				doorX = morphHallX;
				numX = doorX - 2;
			}
			numY = doorY + 4;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					WorldGen.KillTile(numX + i, numY + j);
					WorldGen.PlaceTile(numX + i, numY + j, ModContent.TileType<ChozoBrickNatural>());
				}
			}

			ChozoRuins_MorphHall(morphHallX, morphHallY, morphHallWidth, morphHallHeight, dir);
			Hatch(doorX, doorY);

			int hallWidth = WorldGen.genRand.Next(50, 60);
			int hallHeight = 16;
			int hallX = x + width - hallWidth;
			int hallY = y + height - 4;
			if (dir == -1)
			{
				hallX = x;
			}

			ChozoRuins_Hall(hallX, hallY, hallWidth, hallHeight, dir);
			VerticalHatch(x + width / 2 - 2, y + height - 4);
		}
		private static void ChozoRuins_MorphHall(int x, int y, int width, int height, int dir)
		{
			BasicStructure(x, y, width, height, 4, ModContent.TileType<ChozoBrickNatural>(), ModContent.WallType<ChozoBrickWallNatural>(), 1);

			Mod mod = MetroidMod.Instance;

			for (int j = 0; j < 9; j++)
			{

				int k = 0;
				if (j >= 3)
				{
					k = 1;
				}
				if (j >= 6)
				{
					k = 2;
				}
				if (dir == 1)
				{
					for (int i = 0; i < 4 + k; i++)
					{
						WorldGen.KillTile(x + 14 + i, y + height - 14 + j);
						WorldGen.PlaceTile(x + 14 + i, y + height - 14 + j, ModContent.TileType<ChozoBrickNatural>());
					}
				}
				else
				{
					for (int i = -k; i < 4; i++)
					{
						WorldGen.KillTile(x + width - 18 + i, y + height - 14 + j);
						WorldGen.PlaceTile(x + width - 18 + i, y + height - 14 + j, ModContent.TileType<ChozoBrickNatural>());
					}
				}

				if (j < 5)
				{
					int numX = x + 4;
					if (dir == -1)
					{
						numX = x + width - 9;
					}
					WorldGen.KillTile(numX + j, y + height - 5);
					WorldGen.PlaceTile(numX + j, y + height - 5, ModContent.TileType<ChozoBrickNatural>());
					WorldGen.KillTile(numX - dir + j, y + height - 6);
					WorldGen.PlaceTile(numX - dir + j, y + height - 6, ModContent.TileType<ChozoBrickNatural>());
				}
			}

			int statueX = x + 5;
			int statueX2 = statueX + 1;
			if (dir == -1)
			{
				statueX = x + width - 5;
				statueX2 = statueX - 2;
			}
			int statueY = y + height - 7;
			WorldGen.PlaceObject(statueX, statueY, ModContent.TileType<ChozoStatueNatural>(), false, 0, 0, -1, -dir);
			WorldGen.PlaceObject(statueX2, statueY, ModContent.TileType<ChozoStatueArmNatural>(), false, 0, 0, -1, -dir);

			Main.tile[statueX2, statueY - 2].Get<TileWallWireStateData>().HasTile = true;
			Main.tile[statueX2, statueY - 2].Get<TileTypeData>().Type = (ushort)ModContent.TileType<MorphBallTile>();

			Main.tile[statueX2, statueY - 2].Get<TileWallWireStateData>().TileFrameX = 0;
			Main.tile[statueX2, statueY - 2].Get<TileWallWireStateData>().TileFrameY = 0;
		}
		private static void ChozoRuins_Hall(int x, int y, int width, int height, int dir)
		{
			Mod mod = MetroidMod.Instance;

			BasicStructure(x, y, width, height, 4, ModContent.TileType<ChozoBrickNatural>(), ModContent.WallType<ChozoBrickWallNatural>(), 1);

			for (int i = 0; i < 20; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (j < 2)
					{
						WorldGen.PlaceTile(x + width / 2 - 10 + i, y + height - 6 + j, ModContent.TileType<ChozoBrickNatural>());
					}
					if (i > 1 && i < 18)
					{
						WorldGen.PlaceTile(x + width / 2 - 10 + i, y + 4 + j, ModContent.TileType<ChozoBrickNatural>());
					}
				}
			}
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if (i <= 0 || j <= 0 || i >= 9 || j >= 3)
					{
						WorldGen.KillTile(x + width / 2 - 5 + i, y + 4 + j);
					}
					if (i < 2 && j <= 0)
					{
						WorldGen.KillTile(x + width / 2 - 1 + i, y + 8);
					}
				}
			}
			WorldGen.PlaceObject(x + width / 2, y + 4, ModContent.TileType<MissileExpansionTile>());

			int shaftWidth = 24;
			int shaftHeight = WorldGen.genRand.Next(70, 80);
			int shaftX = x - shaftWidth + 4;
			int shaftY = y;
			int doorX = shaftX + shaftWidth - 4;
			int doorY = y + height / 2 - 2;
			int numX = doorX - 2;
			if (dir == -1)
			{
				shaftX = x + width - 4;
				doorX = shaftX;
				numX = doorX + 4;
			}
			int numY = doorY + 4;
			ChozoRuins_SecondShaft(shaftX, shaftY, shaftWidth, shaftHeight, dir);
			Hatch(doorX, doorY);
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					WorldGen.KillTile(numX + i, numY + j);
					WorldGen.PlaceTile(numX + i, numY + j, ModContent.TileType<ChozoBrickNatural>());
				}
			}
		}
		private static void ChozoRuins_SecondShaft(int x, int y, int width, int height, int dir)
		{
			BasicStructure(x, y, width, height, 4, ModContent.TileType<ChozoBrickNatural>(), ModContent.WallType<ChozoBrickWallNatural>(), 1);

			for (int j = 11; j < height - 5; j += 5)
			{
				for (int i = 0; i < 3; i++)
				{
					int platform = x + 6 + Main.rand.Next(12);
					WorldGen.PlaceTile(platform - 1, y + j, 19, false, false, -1, 17);
					WorldGen.PlaceTile(platform, y + j, 19, false, false, -1, 17);
					WorldGen.PlaceTile(platform + 1, y + j, 19, false, false, -1, 17);
				}
			}

			int chestRoomWidth = 20;
			int chestRoomHeight = 16;
			int chestRoomX = x - chestRoomWidth + 4;
			int chestRoomY = y + WorldGen.genRand.Next(height / 3);
			int doorX = x;
			int doorY = chestRoomY + chestRoomHeight / 2 - 2;

			int numX = doorX + 4;
			if (dir == -1)
			{
				chestRoomX = x + width - 4;
				doorX = chestRoomX;
				numX = doorX - 2;
			}
			int numY = doorY + 4;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					WorldGen.KillTile(numX + i, numY + j);
					WorldGen.PlaceTile(numX + i, numY + j, ModContent.TileType<ChozoBrickNatural>());
				}
			}
			ChozoRuins_ChestRoom(chestRoomX, chestRoomY, chestRoomWidth, chestRoomHeight, ItemID.SandstorminaBottle);
			Hatch(doorX, doorY);

			int saveRoomWidth = 20;
			int saveRoomHeight = 16;
			int saveRoomX = x + width - 4;
			int saveRoomY = y + height - saveRoomHeight;
			doorX = saveRoomX;
			doorY = saveRoomY + saveRoomHeight / 2 - 2;
			numX = doorX - 2;
			if (dir == -1)
			{
				saveRoomX = x - saveRoomWidth + 4;
				doorX = x;
				numX = doorX + 4;
			}
			numY = doorY + 4;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					WorldGen.KillTile(numX + i, numY + j);
					WorldGen.PlaceTile(numX + i, numY + j, ModContent.TileType<ChozoBrickNatural>());
				}
			}
			ChozoRuins_SaveRoom(saveRoomX, saveRoomY);
			Hatch(doorX, doorY);

			int bombRoomWidth = 36;
			int bombRoomHeight = 16;
			int bombRoomX = x - bombRoomWidth + 4;
			int bombRoomY = y + height - bombRoomHeight;
			doorX = x;
			doorY = bombRoomY + bombRoomHeight - 10;
			if (dir == -1)
			{
				bombRoomX = x + width - 4;
				doorX = bombRoomX;
			}
			ChozoRuins_BombRoom(bombRoomX, bombRoomY, dir);
			Hatch(doorX, doorY);

			int bossRoomWidth = 80;
			int bossRoomHeight = 40;
			int bossRoomX = saveRoomX + saveRoomWidth - 4;
			int bossRoomY = y + height - bossRoomHeight;
			doorX = bossRoomX;
			doorY = bossRoomY + bossRoomHeight - 10;
			if (dir == -1)
			{
				bossRoomX = saveRoomX - bossRoomWidth + 4;
				doorX = saveRoomX;
			}
			ChozoRuins_BossRoom(bossRoomX, bossRoomY, bossRoomWidth, bossRoomHeight, dir);
			Hatch(doorX, doorY);
		}
		private static void ChozoRuins_BombRoom(int x, int y, int dir)
		{
			Mod mod = MetroidMod.Instance;

			int width = 36;
			int height = 16;
			BasicStructure(x, y, width, height, 4, ModContent.TileType<ChozoBrickNatural>(), ModContent.WallType<ChozoBrickWallNatural>(), 1);

			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < height - 9; j++)
				{
					WorldGen.PlaceTile(x + width / 2 - 2 + i, y + 4 + j, ModContent.TileType<ChozoBrickNatural>());
				}
				int xx = x + width / 2 + 2 + i;
				if (dir == -1)
				{
					xx = x + width / 2 - 6 + i;
				}
				WorldGen.PlaceTile(xx, y + height - 6, ModContent.TileType<ChozoBrickNatural>());
			}

			int statueX = x + width / 2 + 3;
			int statueX2 = statueX + 1;
			if (dir == -1)
			{
				statueX = x + width / 2 - 3;
				statueX2 = statueX - 2;
			}
			int statueY = y + height - 7;
			WorldGen.PlaceObject(statueX, statueY, ModContent.TileType<ChozoStatueNatural>(), false, 0, 0, -1, -dir);
			WorldGen.PlaceObject(statueX2, statueY, ModContent.TileType<ChozoStatueArmNatural>(), false, 0, 0, -1, -dir);

			Main.tile[statueX2, statueY - 2].Get<TileWallWireStateData>().HasTile = true;
			Main.tile[statueX2, statueY - 2].Get<TileTypeData>().Type = (ushort)MBAddonLoader.GetAddon<Content.MorphBallAddons.Bomb>().TileType;

			Main.tile[statueX2, statueY - 2].Get<TileWallWireStateData>().TileFrameX = 0;
			Main.tile[statueX2, statueY - 2].Get<TileWallWireStateData>().TileFrameY = 0;
			//WorldGen.PlaceObject(statueX2, statueY - 2, MBAddonLoader.GetAddon<Content.MorphBallAddons.Bomb>().TileType);

			for (int i = 0; i < 5; i++)
			{
				int numX = x + 4 + i;
				if (dir == -1)
				{
					numX = x + width - 9 + i;
				}
				WorldGen.PlaceTile(numX, y + height - 5, ModContent.TileType<ChozoBrickNatural>());
				WorldGen.PlaceTile(numX - dir, y + height - 6, ModContent.TileType<ChozoBrickNatural>());
			}
			statueX = x + 5;
			statueX2 = statueX + 1;
			if (dir == -1)
			{
				statueX = x + width - 5;
				statueX2 = statueX - 2;
			}
			WorldGen.PlaceObject(statueX, statueY, ModContent.TileType<ChozoStatueNatural>(), false, 0, 0, -1, -dir);
			WorldGen.PlaceObject(statueX2, statueY, ModContent.TileType<ChozoStatueArmNatural>(), false, 0, 0, -1, -dir);

			Main.tile[statueX2, statueY - 2].Get<TileWallWireStateData>().HasTile = true;
			Main.tile[statueX2, statueY - 2].Get<TileTypeData>().Type = (ushort)SuitAddonLoader.GetAddon<Content.SuitAddons.PowerGrip>().TileType;

			Main.tile[statueX2, statueY - 2].Get<TileWallWireStateData>().TileFrameX = 0;
			Main.tile[statueX2, statueY - 2].Get<TileWallWireStateData>().TileFrameY = 0;
			//WorldGen.PlaceObject(statueX2, statueY - 2, SuitAddonLoader.GetAddon<Content.SuitAddons.PowerGrip>().TileType);
		}
		private static void ChozoRuins_BossRoom(int x, int y, int width, int height, int dir)
		{
			BasicStructure(x, y, width, height, 4, ModContent.TileType<ChozoBrickNatural>(), ModContent.WallType<ChozoBrickWallNatural>(), 1);

			int stepsX = x + 4;
			if (dir == -1)
			{
				stepsX = x + width - 5;
			}
			WorldGen.PlaceTile(stepsX, y + height - 6, ModContent.TileType<ChozoBrickNatural>());
			WorldGen.PlaceTile(stepsX, y + height - 5, ModContent.TileType<ChozoBrickNatural>());
			WorldGen.PlaceTile(stepsX + dir, y + height - 5, ModContent.TileType<ChozoBrickNatural>());

			NPC.NewNPC(NPC.GetSource_NaturalSpawn(), 8 + (x + width - 6) * 16, (y + height - 4) * 16, ModContent.NPCType<Content.NPCs.Torizo.IdleTorizo>());
			TorizoRoomLocation.X = x;
			TorizoRoomLocation.Y = y;
		}

		private static void ChozoRuins_SaveRoom(int x, int y)
		{
			Mod mod = MetroidMod.Instance;

			int width = 20;
			int height = 16;
			BasicStructure(x, y, width, height, 4, ModContent.TileType<ChozoBrickNatural>(), ModContent.WallType<ChozoBrickWallNatural>());

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					WorldGen.PlaceTile(x + i, y + height - 6 + j, ModContent.TileType<ChozoBrickNatural>());
					WorldGen.PlaceTile(x + i, y + 4 + j, ModContent.TileType<ChozoBrickNatural>());
				}
			}

			int numX = x + width / 2 - 2;
			WorldGen.KillTile(numX, y + height - 6);
			WorldGen.KillTile(numX + 1, y + height - 6);
			WorldGen.KillTile(numX + 2, y + height - 6);
			WorldGen.KillTile(numX + 3, y + height - 6);
			Tile.SmoothSlope(numX - 1, y + height - 6, false);
			Tile.SmoothSlope(numX + 4, y + height - 6, false);

			WorldGen.PlaceObject(numX + 2, y + height - 6, ModContent.TileType<SaveStation>(), false, 0, 0, -1, 1);

			WorldGen.KillTile(numX, y + 5);
			WorldGen.KillTile(numX + 1, y + 5);
			WorldGen.KillTile(numX + 2, y + 5);
			WorldGen.KillTile(numX + 3, y + 5);
			Tile.SmoothSlope(numX - 1, y + 5, false);
			Tile.SmoothSlope(numX + 4, y + 5, false);

			WorldGen.PlaceTile(x + 6, y + 6, ModContent.TileType<ChozoBrickNatural>());
			WorldGen.PlaceTile(x + width - 7, y + 6, ModContent.TileType<ChozoBrickNatural>());
			WorldGen.PlaceObject(x + 6, y + 7, 10, false, 29, 0, -1, 1);
			WorldGen.PlaceObject(x + width - 7, y + 7, 10, false, 29, 0, -1, 1);
		}

		private static void ChozoRuins_ChestRoom(int x, int y, int width, int height, int itemType)
		{
			BasicStructure(x, y, width, height, 4, ModContent.TileType<ChozoBrickNatural>(), ModContent.WallType<ChozoBrickWallNatural>(), 1);

			for (int j = 0; j < 2; j++)
			{
				for (int i = -j; i < 4 + j; i++)
				{
					int numX = x + width / 2 - 2;
					WorldGen.KillTile(numX + i, y + height - 6 + j);
					WorldGen.PlaceTile(numX + i, y + height - 6 + j, ModContent.TileType<ChozoBrickNatural>());
				}
			}
			Mod mod = MetroidMod.Instance;
			int xx = x + width / 2;
			int yy = y + height - 7;
			WorldGen.AddBuriedChest(xx, yy, itemType, false, 1);
			for (int l = xx - 1; l < xx + 1; l++)
			{
				for (int m = yy - 1; m < yy + 1; m++)
				{
					//if (Main.tile[l, m] == null)
						//Main.tile[l, m] = new Tile();
					Tile tile = Main.tile[l, m];
					tile.HasTile = true;
					tile.TileType = (ushort)ModContent.TileType<ChozoChest>();
					tile.TileFrameX = (short)((tile.TileFrameX / 18 % 2) * 18);
					tile.TileFrameY = (short)((tile.TileFrameY / 18 % 2) * 18);
				}
			}
		}

		private static void BasicStructure(int x, int y, int width, int height, int thickness, int tileType, int wallType, int ruinedWallType = 0, int ruinedWallRand = 6)
		{
			int thick = thickness;
			if (thick < 1)
			{
				thick = 1;
			}
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int wallType2 = wallType;
					int rand = WorldGen.genRand.Next(ruinedWallRand);
					if (rand == 0 && ruinedWallType >= 1)
					{
						wallType2 = -1;
					}
					if (rand == 1 && ruinedWallType >= 2)
					{
						wallType2 = 0;
					}
					if (i > 0 && i < width - 1 && j > 0 && j < height - 1 && wallType2 >= 0)
					{
						WorldGen.KillWall(x + i, y + j);
						WorldGen.PlaceWall(x + i, y + j, wallType2);
					}
					DestroyChest(x + i, y + j);
					WorldGen.KillTile(x + i, y + j);

					if (i < thick || j < thick || i >= width - thick || j >= height - thick)
					{
						WorldGen.PlaceTile(x + i, y + j, ModContent.TileType<ChozoBrickNatural>());
					}
				}
			}
		}
		private static void Hatch(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			for (int x = i; x < i + 4; x++)
			{
				for (int y = j; y < j + 4; y++)
				{
					DestroyChest(x, y);
					WorldGen.KillTile(x, y);
				}
			}
			WorldGen.PlaceObject(i + 1, j + 2, ModContent.TileType<BlueHatch>(), false, 0, 0, -1, 1);
		}
		private static void VerticalHatch(int i, int j)
		{
			Mod mod = MetroidMod.Instance;
			for (int x = i; x < i + 4; x++)
			{
				for (int y = j; y < j + 4; y++)
				{
					DestroyChest(x, y);
					WorldGen.KillTile(x, y);
				}
			}
			WorldGen.PlaceObject(i + 1, j + 2, ModContent.TileType<BlueHatchVertical>(), false, 0, 0, -1, 1);
		}
		private static void DestroyChest(int x, int y)
		{
			if (!Chest.DestroyChest(x, y))
			{
				int id = Chest.FindChest(x, y);
				Chest.DestroyChestDirect(x, y, id);
			}
		}

		int meteorSpawnAttempt = 0;
		int spawnCounter = 0;
		int spawnCounter2 = 0;
		public override void PostUpdateEverything()
		{
			if (Main.hardMode && NPC.downedPlantBoss && !spawnedPhazonMeteor && meteorSpawnAttempt <= 0)
			{
				DropPhazonMeteor();
				meteorSpawnAttempt = 3600;
			}
			if (meteorSpawnAttempt > 0 && !spawnedPhazonMeteor)
			{
				meteorSpawnAttempt--;
			}

			if (!bossesDown.HasFlag(MetroidBossDown.downedTorizo) && !NPC.AnyNPCs(ModContent.NPCType<Torizo>()) && !NPC.AnyNPCs(ModContent.NPCType<IdleTorizo>()) && TorizoRoomLocation.X > 0 && TorizoRoomLocation.Y > 0)
			{
				Rectangle room = TorizoRoomLocation;
				if (spawnCounter <= 0)
				{
					Vector2 pos = new Vector2(room.X + 8, room.Y + room.Height - 4);
					if (room.X > Main.maxTilesX / 2)
					{
						pos.X = (room.X + room.Width - 8);
					}
					pos *= 16f;

					NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)pos.X, (int)pos.Y, ModContent.NPCType<IdleTorizo>());
				}
				else
				{
					spawnCounter--;
				}
			}
			else
			{
				spawnCounter = 300;
			}

			if (NPC.downedGolemBoss && bossesDown.HasFlag(MetroidBossDown.downedTorizo) &&
				!bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo) && !NPC.AnyNPCs(ModContent.NPCType<GoldenTorizo>()) && !NPC.AnyNPCs(ModContent.NPCType<IdleGoldenTorizo>()) && TorizoRoomLocation.X > 0 && TorizoRoomLocation.Y > 0)
			{
				Rectangle room = TorizoRoomLocation;
				if (spawnCounter2 <= 0)
				{
					Vector2 pos = new Vector2(room.X + 8, room.Y + room.Height - 4);
					if (room.X > Main.maxTilesX / 2)
					{
						pos.X = (room.X + room.Width - 8);
					}
					pos *= 16f;

					NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)pos.X, (int)pos.Y, ModContent.NPCType<IdleGoldenTorizo>());
				}
				else
				{
					spawnCounter2--;
				}
			}
			else
			{
				spawnCounter2 = 300;
			}
		}

		public override void PreUpdateEntities()
		{
			Main.tileSolid[ModContent.TileType<BlueHatchOpen>()] = false;
			Main.tileSolid[ModContent.TileType<BlueHatchOpenVertical>()] = false;
			Main.tileSolid[ModContent.TileType<RedHatchOpen>()] = false;
			Main.tileSolid[ModContent.TileType<RedHatchOpenVertical>()] = false;
			Main.tileSolid[ModContent.TileType<GreenHatchOpen>()] = false;
			Main.tileSolid[ModContent.TileType<GreenHatchOpenVertical>()] = false;
			Main.tileSolid[ModContent.TileType<YellowHatchOpen>()] = false;
			Main.tileSolid[ModContent.TileType<YellowHatchOpenVertical>()] = false;
		}

		//public override void MidUpdateTimeWorld()
		public override void PostUpdateTime()
		{
			Main.tileSolid[ModContent.TileType<BlueHatchOpen>()] = true;
			Main.tileSolid[ModContent.TileType<BlueHatchOpenVertical>()] = true;
			Main.tileSolid[ModContent.TileType<RedHatchOpen>()] = true;
			Main.tileSolid[ModContent.TileType<RedHatchOpenVertical>()] = true;
			Main.tileSolid[ModContent.TileType<GreenHatchOpen>()] = true;
			Main.tileSolid[ModContent.TileType<GreenHatchOpenVertical>()] = true;
			Main.tileSolid[ModContent.TileType<YellowHatchOpen>()] = true;
			Main.tileSolid[ModContent.TileType<YellowHatchOpenVertical>()] = true;
		}
	}
}
