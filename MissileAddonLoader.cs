using System.Collections.Generic;
using System.Linq;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MetroidMod
{
	public static class MissileAddonLoader
	{
		/// <summary>
		/// List of all missile addons that exist.
		/// </summary>
		internal static readonly List<ModMissileAddon> addons = new();

		internal static readonly Dictionary<int, string> unloadedAddons = new();

		//The following methods are all used in order to obtain an addon's ModMissileAddon value through its other forms.
		internal static bool TryGetValue(this IList<ModMissileAddon> list, int type, out ModMissileAddon missile) =>
			list.TryGetValue(i => i.Type == type, out missile);
		internal static bool TryGetValue(this IList<ModMissileAddon> list, string fullName, out ModMissileAddon missile) =>
			list.TryGetValue(i => i.FullName == fullName, out missile);
		internal static bool TryGetValue(this IList<ModMissileAddon> list, Item item, out ModMissileAddon missile) =>
			list.TryGetValue(i => i.ItemType == item.type, out missile);
		public static bool TryGetAddon(Item item, out ModMissileAddon missile) =>
			addons.TryGetValue(item, out missile);
		public static bool TryGetAddon(int type, out ModMissileAddon missile) =>
			addons.TryGetValue(type, out missile);
		public static bool TryGetAddon(string fullName, out ModMissileAddon missile) =>
			addons.TryGetValue(fullName, out missile);
		public static bool TryGetAddon<T>(out ModMissileAddon missile) =>
			addons.TryGetValue(i => i is T, out missile);

		/// <summary>
		/// The total number of missile addons that exist.
		/// </summary>
		public static int AddonCount => addons.Count;

		/// <summary>
		/// Gets the ModMissileAddon of an addon through its <b>Item value.</b><br/>
		/// Used to access an addon's properties for further use.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ModMissileAddon GetAddon(Item item) =>
			addons.TryGetValue(item, out ModMissileAddon missile) ? missile : null;
		/// <summary>
		/// Gets the ModMissileAddon of an addon through its <b>index number.</b><br/>
		/// Used to access an addon's properties for further use.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ModMissileAddon GetAddon(int type) =>
			addons.TryGetValue(type, out ModMissileAddon missile) ? missile : null;

		/// <summary>
		/// Gets the ModMissileAddon of an addon through its <b>name text.</b><br/>
		/// Used to access an addon's properties for further use.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ModMissileAddon GetAddon(string fullName) =>
			addons.TryGetValue(fullName, out ModMissileAddon missile) ? missile : null;

		/// <summary>
		/// Gets the ModMissileAddon of an addon through <b>idfk</b><br/>
		/// Used to access an addon's properties for further use.<br/>
		/// NOTE: Can someone else check this thing and tell me how it gets the thing?   -Z
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ModMissileAddon GetAddon<T>() where T : ModMissileAddon =>
			addons.TryGetValue(i => i is T, out ModMissileAddon missile) ? missile : null;

		public static bool AddonPreAI(ModMissileAddon[] addons, MProjectile shot)
		{
			bool endValue = true;

			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				endValue = addons[i].PreAI(shot);
				if (!endValue) { break; }
			}
			return endValue;
		}
		public static bool IsAMissileTile(Tile tile)
		{
			foreach (ModMissileAddon addon in addons)
			{
				if (tile.TileType == addon.TileType) { return true; }
			}
			return false;
		}

		//The following methods are simply some under-the-hood stuff to make sure things actually load properly.
		internal static void ReloadTypes(TagCompound unloadedTag)
		{
			unloadedAddons.Clear();
			Dictionary<string, object> unloaded = new(unloadedTag);
			foreach ((string name, object type) in unloaded)
			{
				unloadedAddons[(int)type] = name;
			}

			HashSet<int> reserveTypes = new();
			foreach ((int type, string name) in unloadedAddons)
			{
				if (addons.TryGetValue(name, out ModMissileAddon missile))
				{
					missile.ChangeType(type);
					reserveTypes.Add(type);
				}
			}

			int freeType = 3;
			foreach (ModMissileAddon missile in addons)
			{
				if (reserveTypes.Contains(missile.Type)) { continue; }

				while (reserveTypes.Contains(freeType)) { freeType++; }

				missile.ChangeType(freeType);
				freeType++;
			}
		}
		//Method Stackems ahead.
		//This is the REAL fun part: where projectile behavior is added onto the Missile shots in real-time.
		/// <summary>
		/// Runs the OnSpawn() behavior of every addon in a given array.
		/// <br/>The reason it's OnInitialized instead of OnSpawn is because you can't really insert addons before OnSpawn() runs.
		/// </summary>
		/// <param name="addons"></param>
		/// <param name="shot"></param>
		/// <param name="source"></param>
		public static void AddonOnInitialized(ModMissileAddon[] addons, MProjectile shot, IEntitySource source)
		{
			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				addons[i].OnSpawn(shot, source);
			}
		}
		/// <summary>
		/// Runs <see cref="ModMissileAddon.AI(MProjectile)"/> on each installed addon.
		/// </summary>
		/// <param name="addons"></param>
		/// <param name="shot"></param>
		public static void AddonAI(ModMissileAddon[] addons, MProjectile shot)
		{
			//MetroidMod.Instance.Logger.Info(addons.Length);
			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				addons[i].AI(shot);
			}
		}
		/// <summary>
		/// Runs <see cref="ModMissileAddon.PostAI(MProjectile)"/> on each installed addon.
		/// </summary>
		/// <param name="addons"></param>
		/// <param name="shot"></param>
		public static void AddonPostAI(ModMissileAddon[] addons, MProjectile shot)
		{
			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				addons[i].PostAI(shot);
			}
		}
		/// <summary>
		/// Used to acquire a Missile shot's sound effects through its filepath and current modifiers.
		/// <br/>Mostly comprised of failsafes to prevent grabbing assets that don't exist.
		/// </summary>
		/// <param name="soundSource">The base sound filepath, upon which the mod strings will be applied.</param>
		/// <param name="fallback">The sound effect to be used should the grabber fail. The default fallbacks are as follows:
		/// <br/>Shooting: <see cref="MetroidMod.MissileShotFallbackSFX"/>
		/// <br/>Impact: <see cref="MetroidMod.MissileImpactFallbackSFX"/>
		/// <br/>Charging: <see cref="MetroidMod.MissileChargeFallbackSFX"/></param>
		/// <returns></returns>
		public static SoundStyle ShotSoundGrabber(string soundSource, SoundStyle fallback)
		{
			if (ModContent.RequestIfExists(soundSource, out Asset<SoundEffect> noModSound))
			{
				return new SoundStyle(soundSource);
			}
			else
			{
				return fallback;
			}
		}
		/// <summary>
		/// Used to acquire textures from the filepaths in the addon files and 2 modifier keywords.
		/// <br/>Mostly comprised of failsafes to prevent grabbing assets that don't exist.
		/// </summary>
		/// <param name="shapeSource"></param>
		/// <returns></returns>
		public static Asset<Texture2D> ShotTextureGrabber(string shapeSource)
		{
			if (ModContent.RequestIfExists(shapeSource, out Asset<Texture2D> noModShot))
			{
				return noModShot;
			}
			else
			{
				MetroidMod.Instance.Logger.Info("Didn't work lmao");
				return MetroidMod.MissileFallbackTexture;
			}
		}
		/// <summary>
		/// Combines all of the <b>weapon-side stats</b> of every installed missile addon.
		/// <br/>These values will be applied to the weapon itself.
		/// <br/>Array values are as follows:
		/// <br/>[0]: Damage Base
		/// <br/>[1]: Damage Multiplier
		/// <br/>[2]: BaseSpeed
		/// <br/>[3]: SpeedMult
		/// </summary>
		/// <param name="missileAddons">The array containing the Missile addons whose stacks need statting.<br/>...stats need stacking*</param>
		/// <returns></returns>
		public static float[] WeaponStatStacker(Item[] missileAddons)
		{
			MetroidMod.Instance.Logger.Info("Stacking Missile Stats...");
			float[] totals = [0, 0, 0, 0, 0];
			ModMissileAddon[] addons = missileAddons //Converts the Item array into a ModMissileAddon array, allowing for direct stat access.
				.Select(GetAddon)
				.ToArray();

			//Run through all installed addons that actually add stats (i.e. leaving out the ammo slot)
			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				if (addons[i].Overridden) { continue; }
				totals[0] += addons[i].BaseDamage;
				totals[1] += addons[i].DamageMult;
				totals[2] += addons[i].BaseSpeed;
				totals[3] += addons[i].SpeedMult;
				totals[4] += addons[i].AddShots;
			}
			MetroidMod.Instance.Logger.Info("Missile stats stacked!");
			return totals;
		}

		/// <summary>
		/// Runs <see cref="ModMissileAddon.OnHitNPC(MProjectile, NPC, NPC.HitInfo, int)"/> on each installed addon.
		/// </summary>
		/// <param name="addons"></param>
		/// <param name="shot"></param>
		public static void AddonOnHitNPC(ModMissileAddon[] addons, MProjectile shot, NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				addons[i].OnHitNPC(shot, target, hit, damageDone);
			}
		}

		public static void AddonOnHitPlayer(ModMissileAddon[] addons, MProjectile shot, Player target, Player.HurtInfo info)
		{
			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				addons[i].OnHitPlayer(shot, target, info);
			}
		}

		public static bool AddonTileCollideStyle(ModMissileAddon[] addons, MProjectile shot, ref int width, ref int height, ref bool fallThrough, ref Microsoft.Xna.Framework.Vector2 hitboxCenterFrac)
		{
			bool endValue = true;

			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				endValue = addons[i].TileCollideStyle(shot, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
				if (!endValue) { break; }
			}
			return endValue;
		}

		public static bool AddonOnTileCollide(ModMissileAddon[] addons, MProjectile shot, Microsoft.Xna.Framework.Vector2 oldVelocity)
		{
			bool endValue = true;

			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				addons[i].OnTileCollide(shot, oldVelocity);
				if (!endValue) { break; }
			}
			return endValue;
		}

		public static void AddonOnKill(ModMissileAddon[] addons, MProjectile shot, int timeLeft)
		{
			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				addons[i].OnKill(shot, timeLeft);
			}
		}
		internal static void Unload()
		{
			addons.Clear();
			unloadedAddons.Clear();
		}
	}
}
