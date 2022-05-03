﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
//using Terraria.GameContent.Liquid;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace MetroidModPorted
{
	public static class BeamLoader
	{
		//internal static readonly List<(Entity entity, int liquidType)> wetEntities = new();
		//internal static readonly List<Entity> lastWetEntities = new();
		internal static readonly List<ModBeam> beams = new();
		internal static readonly List<GlobalBeam> globalBeams = new();
		internal static readonly List<BeamCombination> beamCombinations = new();
		/*{
			new LiquidCollision(0, 1).SetTileType(TileID.Obsidian).SetSound(SoundID.LiquidsWaterLava),
			new LiquidCollision(0, 2).SetTileType(TileID.HoneyBlock).SetSound(SoundID.LiquidsHoneyWater),
			new LiquidCollision(1, 2).SetTileType(TileID.CrispyHoneyBlock).SetSound(SoundID.LiquidsHoneyLava),
		};*/
		internal static readonly Dictionary<int, string> unloadedBeams = new();

        //public static int BucketsRecipeGroupID => MetroidModPorted.beamsRecipeGroupID;

        public static BeamCombination ComboGet(int beam1, int beam2, int beam3 = -1, int beam4 = -1, int beam5 = -1)
        {
            BeamCombination lc = null;
			if (beamCombinations.Any(l =>
			{
				bool @is = l.Is(out var beamCombination, beam1, beam2, beam3, beam4, beam5);
				lc = beamCombination;
				return @is;
			}))
				{ return lc; }

            lc = new BeamCombination(beam1, beam2, beam3, beam4, beam5);
            beamCombinations.Add(lc);
            return lc;
        }

        internal static bool ComboContains(int beam1, int beam2, int beam3 = -1, int beam4 = -1, int beam5 = -1) =>
            beamCombinations.Any(l => l.Is(out _, beam1, beam2, beam3, beam4, beam5));
        internal static bool TryGetValue<T>(this IList<T> list, Func<T, bool> predict, out T value) =>
            (value = list.FirstOrDefault(predict)) != null;
        internal static bool TryGetValue(this IList<ModBeam> list, int type, out ModBeam modBeam) =>
            list.TryGetValue(i => i.Type == type, out modBeam);
		internal static bool TryGetValue(this IList<ModBeam> list, Item item, out ModBeam modBeam) =>
			list.TryGetValue(i => i.Item.Type == item.type, out modBeam);
        internal static bool TryGetValue(this IList<ModBeam> list, string fullName, out ModBeam modBeam) =>
            list.TryGetValue(i => i.FullName == fullName, out modBeam);

        public static int BeamCount => beams.Count;

        public static ModBeam GetBeam(int type) =>
            beams.TryGetValue(type, out ModBeam modBeam) ? modBeam : null;

        public static ModBeam GetBeam(string fullName) =>
            beams.TryGetValue(fullName, out ModBeam modBeam) ? modBeam : null;

        public static ModBeam GetBeam<T>() where T : ModBeam =>
            beams.TryGetValue(i => i is T, out ModBeam modBeam) ? modBeam : null;

		internal static void SetupBeamCombinations()
		{

		}

        internal static void OnUpdate()
        {
            /*for (int i = 0; i < wetEntities.Count; i++)
            {
                if (!lastWetEntities.Contains(wetEntities[i].entity))
                {
                    var (entity, liquidType) = wetEntities[i];
                    OnOutLiquid(liquidType, entity);
                    wetEntities.RemoveAt(i);
                }
            }
            lastWetEntities.Clear();*/
        }

		public static bool CanShoot(Player player, Item[] addons)
		{
			bool flag = true;

			foreach (Item beam in addons)
			{
				if (beam == null || beam.type == ItemID.None) { continue; }
				if (TryGetValue(beams, beam, out ModBeam modBeam))
				{
					flag &= modBeam.CanUse(player);
					foreach (GlobalBeam gbeam in globalBeams)
					{
						flag &= gbeam.CanUse(player, beam);
					}
				}
			}

			return flag;
		}

        public static bool OnShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, Item[] addons)
        {
            bool flag = true;

			foreach (Item item in addons)
			{
				if (TryGetValue(beams, item.type, out ModBeam modBeam))
				{
					flag &= modBeam.OnShoot(player, source, position, velocity, type, damage, knockback);
					foreach (GlobalBeam globalBeam in globalBeams)
					{
						flag &= globalBeam.OnShoot(player, source, position, velocity, type, damage, knockback, modBeam);
					}
				}
			}

            return flag;
        }

		internal static void ReloadTypes(TagCompound unloadedTag)
		{
			unloadedBeams.Clear();
			Dictionary<string, object> unloaded = new(unloadedTag);
			foreach ((string name, object type) in unloaded)
			{
				unloadedBeams[(int)type] = name;
			}

			HashSet<int> reserveTypes = new();
			foreach ((int type, string name) in unloadedBeams)
			{
				if (beams.TryGetValue(name, out ModBeam modBeam))
				{
					modBeam.ChangeType(type);
					reserveTypes.Add(type);
				}
			}

			int freeType = 3;
			foreach (ModBeam modBeam in beams)
			{
				if (reserveTypes.Contains(modBeam.Type)) { continue; }

				while (reserveTypes.Contains(freeType)) { freeType++; }

				modBeam.ChangeType(freeType);
				freeType++;
			}
		}

		internal static void Unload()
		{
			beams.Clear();
			globalBeams.Clear();
			beamCombinations.Clear();
			unloadedBeams.Clear();
		}
    }
}