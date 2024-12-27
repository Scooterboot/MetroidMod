using System;
using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using MetroidMod.ID;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Common.Players;
using MetroidMod.Common.GlobalItems;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Microsoft.Xna.Framework.Audio;

namespace MetroidMod
{
	/// <summary>
	/// Manages ModBeamAddons, stacks effects of addons installed into arm cannons, and provides helpful methods to retrieve beam addon information.
	/// </summary>
	public static class BeamAddonLoader 
	{
		#region Accessor methods
		//I'll be honest I dunno what half this shit does, it's mostly copied from the modsuitaddon equivalent   -Z

		/// <summary>
		/// List of all beam addons that exist.
		/// </summary>
		internal static readonly List<ModBeamAddon> addons = new();

		internal static readonly Dictionary<int, string> unloadedAddons = new();

		//The following methods are the internals for the TryGetAddon() and GetAddon() methods.
		internal static bool TryGetValue(this IList<ModBeamAddon> list, int type, out ModBeamAddon beam) =>
			list.TryGetValue(i => i.Type == type, out beam);
		internal static bool TryGetValue(this IList<ModBeamAddon> list, string fullName, out ModBeamAddon beam) =>
			list.TryGetValue(i => i.FullName == fullName, out beam);
		internal static bool TryGetValue(this IList<ModBeamAddon> list, Item item, out ModBeamAddon beam) =>
			list.TryGetValue(i => i.ItemType == item.type, out beam);

		//The following methods are all used in order to obtain an addon's ModBeamAddon value through its other forms.
		public static bool TryGetAddon(Item item, out ModBeamAddon beam) =>
			addons.TryGetValue(item, out beam);
		public static bool TryGetAddon(int type, out ModBeamAddon beam) =>
			addons.TryGetValue(type, out beam);
		public static bool TryGetAddon(string fullName, out ModBeamAddon beam) =>
			addons.TryGetValue(fullName, out beam);
		public static bool TryGetAddon<T>(out ModBeamAddon beam) =>
			addons.TryGetValue(i => i is T, out beam);

		/// <summary>
		/// The total number of beam addons that exist.
		/// </summary>
		public static int AddonCount => addons.Count;

		/// <summary>
		/// Gets the ModBeamAddon of an addon through its <b>Item value.</b><br/>
		/// Used to access an addon's properties for further use.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ModBeamAddon GetAddon(Item item) =>
			addons.TryGetValue(item, out ModBeamAddon beam) ? beam : null;
		/// <summary>
		/// Gets the ModBeamAddon of an addon through its <b>index number.</b><br/>
		/// Used to access an addon's properties for further use.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ModBeamAddon GetAddon(int type) =>
			addons.TryGetValue(type, out ModBeamAddon beam) ? beam : null;

		/// <summary>
		/// Gets the ModBeamAddon of an addon through its <b>name text.</b><br/>
		/// Used to access an addon's properties for further use.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ModBeamAddon GetAddon(string fullName) =>
			addons.TryGetValue(fullName, out ModBeamAddon beam) ? beam : null;

		/// <summary>
		/// Gets the ModBeamAddon of an addon through <b>idfk</b><br/>
		/// Used to access an addon's properties for further use.<br/>
		/// NOTE: Can someone else check this thing and tell me how it gets the thing?   -Z
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ModBeamAddon GetAddon<T>() where T : ModBeamAddon =>
			addons.TryGetValue(i => i is T, out ModBeamAddon beam) ?beam : null;

		public static bool IsABeamTile(Tile tile)
		{
			foreach (ModBeamAddon addon in addons)
			{
				if (tile.TileType == addon.TileType) { return true; }
			}
			return false;
		}
		#endregion

		#region Addon data stackers

		//This is where the magic happens.
		//Each of these methods takes in data from the beam addons and uses them to change the Power Beam.

		/// <summary>
		/// Checks the piority values of the loaded addons and determines what the projectile should look like.<br/>
		/// Method checks for VIB, then ShapePriority, then ColorPriority.<br/>
		/// Unique combination graphics should be checked for within the beam with the highest ShapePriority in the combination<br/>
		/// <i>(i.e. Fusion's DNA-esque Plasma+Wave would be stored and checked for in Plasma)</i>
		/// </summary>
		/// <param name="slot1"></param>
		/// <param name="slot2"></param>
		/// <param name="slot3"></param>
		/// <param name="slot4"></param>
		/// <param name="slot5"></param>
		/// <returns></returns>
		public static int[] VisualPriority(Item[] beamAddons)
		{
			//let it be known there was originally gonna be a third array here for the VIB(e) check results called vibRibbon        -Z
			ModBeamAddon[] addons = beamAddons //Creates a version of BeamAddonAccess that can be fed into the visual priority system
				.Select(selector: GetAddon)
				.ToArray();
			int[] shapeOfPew = new int[addons.Length]; //store all the ShapePriority check results (I couldn't come up with a secondary joke (mostly because I didn't feel like trying))
			int[] fuckYouIceBeam = new int[addons.Length]; //store all the ColorPriority check results here at Big Zek Hell's Arrays
			int[] winners; //Will contain all of the results
			//In order:
			//[0] = The slot containing the highest ShapePriority (0-4)
			//[1] = The slot containing the highest ColorPriority (0-4)
			//[2] = 0 if the VIBe check failed, 1 if it passed
			//[3] = 0 if ColorPriority doesn't have SoundOverride, 1 if it does

			MetroidMod.Instance.Logger.Info("Starting VIBe check");
			for (int i = 0; i < addons.Length - 1; ++i) //Check all addon slots for if VIB is true
			{
				MetroidMod.Instance.Logger.Info("VIBe Check - Slot " + i + "- Contains: " + addons[i]);
				if (addons[i] == null || addons[i].VIB == false) { continue; }
				if (addons[i].VIB == true) { winners = [i, i, 1, 0]; MetroidMod.Instance.Logger.Info("Slot " + i + " passed the VIBe Check"); return winners; }
			} //Iterate through the slots looking for a VIB addon
			MetroidMod.Instance.Logger.Info("You have failed the VIBe Check");


			//special thanks to my buddy Snek for this stuff, I was prolly just gonna do a buncha else-ifs lol    -Z
			int highestShapePriorityIndex = -1;  //Compare ShapePriority values of all installed beams, determine which is the highest
			int highestShapePriority = -1;
			MetroidMod.Instance.Logger.Info("Starting shape priority check");
			for (int i = 0; i < addons.Length - 1; i++)
			{
				MetroidMod.Instance.Logger.Info("Shape Priority Check - Slot " + i + "- Contains: " + addons[i]);
				if (addons[i]?.ShapePriority >= highestShapePriority)
				{
					MetroidMod.Instance.Logger.Info("Value is workable");
					highestShapePriorityIndex = i;
					highestShapePriority = addons[i].ShapePriority;
				}
			} //Iterate through the addons and determine which has the highest ShapePriority value
			MetroidMod.Instance.Logger.Info("Result: Slot " + highestShapePriorityIndex);


			for (int i = 0; i < addons.Length - 1; ++i) //Compare ColorPriority values of all installed beams, determine which is the highest
			{
				if (addons[i] == null) { fuckYouIceBeam[i] = -1; continue; }
				fuckYouIceBeam[i] = addons[i].ColorPriority;
			}
			//Color order is here to facilitate the port priority. Check the ShapePriority value's description for the order of priority.
			ModBeamAddon[] colorOrder = [addons[BeamAddonSlotID.Primary], addons[BeamAddonSlotID.Spread], addons[BeamAddonSlotID.Ion], addons[BeamAddonSlotID.Secondary], addons[BeamAddonSlotID.Ability]]; //something something 20XX
			int highestColorPriorityIndex = -1;
			int highestColorPriority = -1;
			int willItOverride = 0;
			MetroidMod.Instance.Logger.Info("Starting color priority check");
			for (int i = 0; i < colorOrder.Length; i++)
			{
				MetroidMod.Instance.Logger.Info("Color Priority Check - Slot " + i + "- Contains: " + colorOrder[i]);
				if (colorOrder[i]?.ColorPriority >= highestColorPriority)
				{
					MetroidMod.Instance.Logger.Info("Value is workable");
					highestColorPriorityIndex = colorOrder[i].AddonSlot;
					highestColorPriority = colorOrder[i].ColorPriority;
				}
			}
			if (highestColorPriorityIndex != -1)
			{
				if (addons[highestColorPriorityIndex].SoundOverride) //Check if the winner has sound override enabled.
				{ MetroidMod.Instance.Logger.Info("SoundOverride detected!"); willItOverride = 1; }
				else { MetroidMod.Instance.Logger.Info("No SoundOverride here."); willItOverride = 0; }
			} //See if the sound override is enabled

			MetroidMod.Instance.Logger.Info("Result: Slot " + highestShapePriorityIndex);
			winners = [highestShapePriorityIndex, highestColorPriorityIndex, 0, willItOverride]; //If there are no winners it should turn up -1, -1, 0, 0
			MetroidMod.Instance.Logger.Info("winners value: [" + winners[0] + ", " + winners[1] + ", " + winners[2] + ", " + winners[3] +"]");
			return winners;
		}

		/// <summary>
		/// Used to acquire textures from the filepaths in the addon files and 2 modifier keywords.
		/// <br/>Mostly comprised of failsafes to prevent grabbing assets that don't exist.
		/// </summary>
		/// <param name="shapeSource"></param>
		/// <param name="modA"></param>
		/// <param name="modB"></param>
		/// <returns></returns>
		public static Texture2D ShotTextureGrabber(string shapeSource, string modA, string modB)
		{
			//I dislike the large amounts of else-ifs here           -Z

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
			MetroidMod.Instance.Logger.Info("Texture-grabbin time. Path: " + shapeSource + " " + " ");
			if (ModContent.RequestIfExists(shapeSource + modA + modB, out Asset<Texture2D> fullModShot))
			{
				return (Texture2D)fullModShot;
			}
			else if (ModContent.RequestIfExists(shapeSource + modA, out Asset<Texture2D> firstModShot))
			{
				return (Texture2D)firstModShot;
			}
			else if (ModContent.RequestIfExists(shapeSource + modB, out Asset<Texture2D> lastModShot))
			{
				return (Texture2D)lastModShot;
			}
			else if (ModContent.RequestIfExists(shapeSource, out Asset<Texture2D> noModShot))
			{
				return (Texture2D)noModShot;
			}
			else
			{
				MetroidMod.Instance.Logger.Info("Didn't work lmao");
				return MetroidMod.PowerBeamFallbackTexture;
			}
		}
		/// <summary>
		/// Used to acquire a beam shot's shooting sound effect through its filepath and current modifiers.
		/// <br/>Mostly comprised of failsafes to prevent grabbing assets that don't exist.
		/// </summary>
		/// <param name="soundSource"></param>
		/// <param name="modA"></param>
		/// <param name="modB"></param>
		/// <returns></returns>
		public static SoundStyle ShotSoundGrabber(string soundSource, string modA, string modB)
		{
			//I still greatly dislike the amount of else-ifs here     -Z

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
				return MetroidMod.BeamShotFallbackSFX;
			}
		}
		/// <summary>
		/// Used to acquire a beam shot's impact sound effect through its filepath and current modifiers.
		/// <br/>Mostly comprised of failsafes to prevent grabbing assets that don't exist.
		/// <br/>Also noticeably less complex than the other ones because this one's much more likely to turn up blank.
		/// </summary>
		/// <param name="soundSource"></param>
		/// <param name="modA"></param>
		/// <param name="modB"></param>
		/// <returns></returns>
		public static SoundStyle ImpactSoundGrabber(string soundSource, string modA, string modB)
		{
			//I am not going to put nearly as much effort into this one since 90% of beam addons don't even fuck with this sound effect anyway
			if (ModContent.RequestIfExists(soundSource + modA + modB, out Asset<SoundEffect> modSound))
			{
				return new SoundStyle(soundSource + modA + modB);
			}
			else if (ModContent.RequestIfExists(soundSource, out Asset<SoundEffect> noModSound))
			{
				//this one is shorter because no impact sfx is kind of the default, will expand if necessary
				return new SoundStyle(soundSource);
			}
			else
			{
				return MetroidMod.BeamImpactFallbackSFX;
			}
		}

		/// <summary>
		/// Combines all of the <b>weapon-side stats</b> of every installed beam addon.
		/// <br/>These values will be applied to the weapon itself.
		/// </summary>
		/// <param name="beamAddons"></param>
		/// <returns></returns>
		public static float[] WeaponStatStacker(Item[] beamAddons)
		{
			MetroidMod.Instance.Logger.Info("Stacking Beam Stats...");
			float[] totals = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			ModBeamAddon[] addons = beamAddons //Converts the Item array into a ModBeamAddon array, allowing for direct stat access.
				.Select(GetAddon)
				.ToArray();

			//Run through all installed addons that actually add stats (i.e. leaving out the ammo slot)
			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				totals[0] += addons[i].BaseDamage;
				totals[1] += addons[i].DamageMult;
				totals[2] += addons[i].BaseSpeed;
				totals[3] += addons[i].SpeedMult;
				totals[4] += addons[i].BaseVelocity;
				totals[5] += addons[i].VelocityMult;
				totals[6] += addons[i].CritChance;
				totals[7] += addons[i].BaseOverheat;
				totals[8] += addons[i].OverheatMult;
				totals[9] += addons[i].AddShots;
			}
			MetroidMod.Instance.Logger.Info("Beam stats stacked");
			return totals;
		}

		#endregion

		#region Under-the-hood stuff
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
				if (addons.TryGetValue(name, out ModBeamAddon beam))
				{
					beam.ChangeType(type);
					reserveTypes.Add(type);
				}
			}

			int freeType = 3;
			foreach (ModBeamAddon beam in addons)
			{
				if (reserveTypes.Contains(beam.Type)) { continue; }

				while (reserveTypes.Contains(freeType)) { freeType++; }

				beam.ChangeType(freeType);
				freeType++;
			}
		}

		internal static void Unload()
		{
			addons.Clear();
			unloadedAddons.Clear();
		}
		#endregion
	}
}
