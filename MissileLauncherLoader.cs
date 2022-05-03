using Microsoft.Xna.Framework;
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
	public static class MissileLauncherLoader
	{
		internal static readonly List<ModMissileAddon> missileAddons = new();
		internal static readonly List<GlobalMissileAddon> globalMissileAddons = new();
		//internal static readonly List<BeamCombination> beamCombinations = new();
		internal static readonly Dictionary<int, string> unloadedMissileAddons = new();

		internal static bool TryGetValue(this IList<ModMissileAddon> list, int type, out ModMissileAddon modMissileAddon) =>
			list.TryGetValue(i => i.Type == type, out modMissileAddon);
		internal static bool TryGetValue(this IList<ModMissileAddon> list, string fullName, out ModMissileAddon modMissileAddon) =>
			list.TryGetValue(i => i.FullName == fullName, out modMissileAddon);

		public static int MissileCount => missileAddons.Count;

		public static ModMissileAddon GetMissileAddon(int type) =>
			missileAddons.TryGetValue(type, out var modMissileAddon) ? modMissileAddon : null;

		public static ModMissileAddon GetMissileAddon(string fullName) =>
			missileAddons.TryGetValue(fullName, out var modMissileAddon) ? modMissileAddon : null;

		public static ModMissileAddon GetMissileAddon<T>() where T : ModMissileAddon =>
			missileAddons.TryGetValue(i => i is T, out var modMissileAddon) ? modMissileAddon : null;

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

		public static bool OnShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			bool flag = true;

			if (missileAddons.TryGetValue(type, out var modMissileAddon))
			{
				flag &= modMissileAddon.OnShoot(player, source, position, velocity, type, damage, knockback);
			}

			foreach (var globalMissileAddon in globalMissileAddons)
			{
				flag &= globalMissileAddon.OnShoot(player, source, position, velocity, type, damage, knockback);
			}

			return flag;
		}
	}
}
