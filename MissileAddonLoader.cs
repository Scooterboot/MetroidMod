using System.Collections.Generic;
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

		/// <summary>
		/// Creates a total interact value from the sum of that value across all installed beam addons, plus an optional multiplier.
		/// </summary>
		/// <param name="addons"></param>
		/// <param name="concreteMatter">If true, add tileInteract. If false, add entityInteract.</param>
		/// <param name="multiplier">Used to apply a multiplier to an interact value.<br/>Charge shots use this.</param>
		/// <returns></returns>
		public static int InteractStacker(ModMissileAddon[] addons, bool concreteMatter, float multiplier = 1f)
		{
			int total = 0;
			MetroidMod.Instance.Logger.Info("Stacking " + (concreteMatter ? "TileInteract" : "NPCInteract"));

			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				total += (concreteMatter) ? addons[i].TileInteract : addons[i].EntityInteract;
			}//iterate through array and add all interact values.
			MetroidMod.Instance.Logger.Info("Subtotal: " + total + "  ||  Applying multiplier...");
			//Apply charge modifier as well.
			float totalFloat = total * multiplier;
			total = (int)totalFloat;
			MetroidMod.Instance.Logger.Info("Final value: " + total);
			return total;
		}
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
		//This is the REAL fun part: where projectile behavior is added onto the beam shots in real-time.
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
		/// Runs <see cref="ModBeamAddon.AI(MProjectile)"/> on each installed addon.
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
		/// Runs <see cref="ModBeamAddon.PostAI(MProjectile)"/> on each installed addon.
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
		/// Used to acquire a beam shot's sound effects through its filepath and current modifiers.
		/// <br/>Mostly comprised of failsafes to prevent grabbing assets that don't exist.
		/// </summary>
		/// <param name="soundSource">The base sound filepath, upon which the mod strings will be applied.</param>
		/// <param name="modA">Appends onto soundSource, <i>before</i> modB.</param>
		/// <param name="modB">Appends onto soundSource, <i>after</i> modA.</param>
		/// <param name="fallback">The sound effect to be used should the grabber fail. The default fallbacks are as follows:
		/// <br/>Shooting: <see cref="MetroidMod.BeamShotFallbackSFX"/>
		/// <br/>Impact: <see cref="MetroidMod.BeamImpactFallbackSFX"/>
		/// <br/>Charging: <see cref="MetroidMod.BeamChargeFallbackSFX"/></param>
		/// <returns></returns>
		public static SoundStyle ShotSoundGrabber(string soundSource, string modA, string modB, SoundStyle fallback)
		{
			//I still greatly dislike the amount of else-ifs here     -Z
			#region grabber explanation
			//This nasty else-if chain exists NOT to gauge how modded an asset path is, but to catch asset combinations that don't exist.
			//If all the assets are present and correctly named, this method shouldn't pass through anything below the first if.
			//For instance, a basic shot with zero modifiers would only pass through the first if, as mod strings default to blank, meaning it becomes:
			//[soundSource] + "" + ""
			//which still just equals [soundSource] and is therefore a valid asset path
			//(Unless, of course, the basic shot sound is missing or misnamed, in which case it'll end up at the failsafe at the bottom)
			//If the attempted filepath modification leads to a blank, like:
			//[soundSource] + "ModifierWithNoAssets" + "Charged"
			//it'll attempt to grab another asset from its general "asset tree" to fill it in.
			//for instance, this would fail the first else-if and succeed at the second, resulting in "[soundSource]Charged" being selected instead
			//The fallback sound should only call if there's literally NO adjacent assets in this particular filepath configuration, including a basic shot.
			//The chain tries to get a modA path first, since the first layer of mod is a lot more "permanent" than the second
			//(in the sense that it's applied during array updating and not when shooting)
			#endregion
			//TODO: Too many overloads because of how modB works, being a temporary value and all. Find out how to fix.		-Z

			if (ModContent.RequestIfExists(soundSource + modA + modB, out Asset<SoundEffect> fullModSound))
			{
				//For some reason you can't just convert sound effects to soundstyles but it still works somehow so idfk /shrug
				return new SoundStyle(soundSource + modA + modB);
			}
			else if (ModContent.RequestIfExists(soundSource + modA, out Asset<SoundEffect> firstModSound))
			{
				return new SoundStyle(soundSource + modA);
			}
			else if (ModContent.RequestIfExists(soundSource + modB, out Asset<SoundEffect> lastModSound))
			{
				return new SoundStyle(soundSource + modB);
			}
			else if (ModContent.RequestIfExists(soundSource, out Asset<SoundEffect> noModSound))
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
		/// <param name="modA"></param>
		/// <param name="modB"></param>
		/// <returns></returns>
		public static Asset<Texture2D> ShotTextureGrabber(string shapeSource, string modA, string modB)
		{
			//I dislike the large amounts of else-ifs here           -Z
			#region grabber explanation
			//This nasty else-if chain exists NOT to gauge how modded an asset path is, but to catch asset combinations that don't exist.
			//If all the assets are present and correctly named, this method shouldn't pass through anything below the first if.
			//For instance, a basic shot with zero modifiers would only pass through the first if, as mod strings default to blank, meaning it becomes:
			//[shapeSource] + "" + ""
			//which still just equals [shapeSource] and is therefore a valid asset path
			//(Unless, of course, the basic shot texture is missing or misnamed, in which case it'll end up at the failsafe at the bottom)
			//If the attempted filepath modification leads to a blank, like:
			//[shapeSource] + "ModifierWithNoAssets" + "Charged"
			//it'll attempt to grab another asset from its general "asset tree" to fill it in.
			//for instance, this would fail the first else-if and succeed at the second, resulting in "[shapeSource]Charged" being selected instead
			//The fallback texture should only call if there's literally NO adjacent assets in this particular filepath configuration, including a basic shot.
			//The chain tries to get a modA path first, since the first layer of mod is a lot more "permanent" than the second
			//(in the sense that it's applied during array updating and not when shooting)
			#endregion

			MetroidMod.Instance.Logger.Info("Texture-grabbin time. Path: " + shapeSource + modA + modB);
			if (ModContent.RequestIfExists(shapeSource + modA + modB, out Asset<Texture2D> fullModShot))
			{
				return fullModShot;
			}
			else if (ModContent.RequestIfExists(shapeSource + modA, out Asset<Texture2D> firstModShot))
			{
				return firstModShot;
			}
			else if (ModContent.RequestIfExists(shapeSource + modB, out Asset<Texture2D> lastModShot))
			{
				return lastModShot;
			}
			else if (ModContent.RequestIfExists(shapeSource, out Asset<Texture2D> noModShot))
			{
				return noModShot;
			}
			else
			{
				MetroidMod.Instance.Logger.Info("Didn't work lmao");
				return MetroidMod.PowerBeamFallbackTexture;
			}
		}
		/// <summary>
		/// Runs <see cref="ModBeamAddon.OnHitNPC(MProjectile, NPC, NPC.HitInfo, int)"/> on each installed addon.
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
