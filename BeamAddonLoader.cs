using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using System.Drawing.Text;
using MetroidMod.ID;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Common.Players;
using MetroidMod.Common.GlobalItems;

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
				MetroidMod.Instance.Logger.Info("VIBe Check - Slot " + i + "\nContains: " + addons[i]);
				if (addons[i] == null || addons[i].VIB == false) { continue; }
				if (addons[i].VIB == true) { winners = [i, i, 1, 0]; MetroidMod.Instance.Logger.Info("Slot " + i + " passed the VIBe Check"); return winners; }
			}
			MetroidMod.Instance.Logger.Info("You have failed the VIBe Check");


			//special thanks to my buddy Snek for this stuff, I was prolly just gonna do a buncha else-ifs lol    -Z
			int highestShapePriorityIndex = -1;  //Compare ShapePriority values of all installed beams, determine which is the highest
			int highestShapePriority = -1;
			MetroidMod.Instance.Logger.Info("Starting shape priority check \nValue starts at -1");
			for (int i = 0; i < addons.Length - 1; i++)
			{
				MetroidMod.Instance.Logger.Info("Shape Priority Check - Slot " + i + ", Contains: " + addons[i]);
				if (addons[i]?.ShapePriority >= highestShapePriority)
				{
					MetroidMod.Instance.Logger.Info("Value is workable");
					highestShapePriorityIndex = i;
					highestShapePriority = addons[i].ShapePriority;
				}
			}
			MetroidMod.Instance.Logger.Info("Result: Slot " + highestShapePriorityIndex);


			for (int i = 0; i < addons.Length - 1; ++i) //Compare ColorPriority values of all installed beams, determine which is the highest
			{
				if (addons[i] == null) { fuckYouIceBeam[i] = -1; continue; }
				fuckYouIceBeam[i] = addons[i].ColorPriority;
			}
			ModBeamAddon[] colorOrder = [addons[BeamAddonSlotID.Primary], addons[BeamAddonSlotID.Spread], addons[BeamAddonSlotID.Ion], addons[BeamAddonSlotID.Secondary], addons[BeamAddonSlotID.Ability]]; //something something 20XX
			int highestColorPriorityIndex = -1;
			int highestColorPriority = -1;
			int willItOverride = 0;
			MetroidMod.Instance.Logger.Info("Starting color priority check \nValue starts at -1");
			for (int i = 0; i < colorOrder.Length; i++)
			{
				MetroidMod.Instance.Logger.Info("Color Priority Check - Slot " + i + "\nContains: " + colorOrder[i]);
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
			}

			MetroidMod.Instance.Logger.Info("Result: Slot " + highestShapePriorityIndex);
			winners = [highestShapePriorityIndex, highestColorPriorityIndex, 0, willItOverride]; //If there are no winners it should turn up -1, -1, 0, 0
			MetroidMod.Instance.Logger.Info("winners value: [" + winners[0] + ", " + winners[1] + ", " + winners[2] + ", " + winners[3] +"]");
			return winners;
		}
		/// <summary>
		/// Combines all of the <b>weapon-side stats</b> of every installed beam addon.
		/// <br/>These values will be applied to the weapon itself.
		/// </summary>
		/// <param name="beamAddons"></param>
		/// <returns></returns>
		public static float[] WeaponStatStacker(Item[] beamAddons)
		{
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
