using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;
using MetroidMod.Content.Projectiles;
using Terraria.DataStructures;

namespace MetroidMod.Content.BeamAddons
{
	//This here is how you make a pretty basic beam addon.
	//Almost every important variable and method should be labelled, so I won't comment too much on their functions,
	//But I'll be sure to point out the best ways to use stuff.           -Z
	internal class TestBeam : ModBeamAddon
	{
		//If you keep important stats outside of a method, you can plug them into the tooltip!
		//Highly recommend doing this as the system is already designed around it and it means you only have to do the tooltips once
		//and then it just automatically updates every time you alter a value
		#region Beam stat values
		int bd = 50; //base damage
		int bs = -5; //base speed
		float bv = 0f; //base velocity
		int bo = 0; //base overheat
		float dm = 0f; //damage multiplier
		float sm = 0f; //speed multiplier
		float vm = 0f; //velocity multiplier
		float om = -95f; //overheat multiplier
		int cc = 0; //crit chance

		int sa = 3; //shots added
		#endregion

		#region Asset Grabbing
		//The ModBeamAddon system is able to automatically grab assets without having to override anything yourself.

		//To take advantage of this, all assets should be stored in the following filepath structure:
		//[[[->  {ModName}/Assets/{TypeOfAsset}/BeamAddons/{BeamAddonName}/  <-]]]

		//As a general rule of thumb, take the name of the path's variable and remove the type of asset from it
		//(i.e. basic shot assets should be named Shot (ShotTexture - Texture = Shot, ShotSound - Sound = Shot))
		//The system is designed to automatically grab assets stored in these locations.

		//If you want to make variations on these (i.e. for Charge Beam), you MUST append (defined here as "to add at the end of something") extra keywords at the end of the normal name.
		//(i.e. if you're making a charged shot, you MUST name it the same thing as its basic form with "Charged" added at the end. NO SPACES.)
		//This is because the asset-grabbing systems are designed to automatically append keywords to the filepaths used to grab assets as needed.
		//Because of that, you MUST follow the keyword system even if you override an asset's base filepath.

		//These are all the variables for finding assets and their default filepaths.
		//Again, you DO NOT need to override these to take advantage of the system.
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/BeamAddons/{Name}/Item";
		public override string TileTexture => $"{Mod.Name}/Assets/Textures/BeamAddons/{Name}/Tile";
		public override string ShotTexture => $"{Mod.Name}/Assets/Textures/BeamAddons/{Name}/Shot";
		public override string ShotSound => $"{Mod.Name}/Assets/Sounds/BeamAddons/{Name}/Shot";
		public override string ImpactSound => $"{Mod.Name}/Assets/Sounds/BeamAddons/{Name}/Impact";

		//If you're copypasting from this file, you don't need to keep this part unless you want to use your own custom directories.
		//However, regardless of where you put them, you MUST follow the file name format, because the entire system is built around it.
		//Remember: for a charged shot texture, you MUST title the file "ShotCharged" or it will NOT find it.
		#endregion

		//I'll be honest, I have NO clue what the point of this thing is.
		//Odds are you'll never need to turn this to true.
		public override bool AddOnlyAddonItem => false;


		//Primary color is your beam shot's main color. Secondary is the accent color.
		public override Color PrimaryColor => new(0, 0, 255, 1f);

		public override Color SecondaryColor => Color.Black;
		public override float CoreSaturation => 0.5f;

		//If you don't want to make custom dust, see the Terraria Wiki or the Modder's Toolkit to find the vanilla dust that's right for you
		public override int ShotDust => 33;

		public override void SetStaticDefaults()
		{
			//For the most part it doesn't matter TOO much where an addon goes,
			//But if you're doing more complicated stuff it's a good idea to follow the recommended sorting pattern,
			//If only to prevent conflicts with other stuff.
			AddonSlot = BeamAddonSlotID.Secondary;
			//You might wanna tweak these values to get it into just the right visibility niche you want.
			ShapePriority = 1;
			ColorPriority = 3;
			SoundOverride = false;
			//Be real careful with this sucker.
			//I would NOT recommend using ArrayPassive if this thing is on,
			VIB = false;

			//This is also where stat modifiers are set. In this example, the actual values are set at the beginning.
			//9 times outta 10, you're better off using multipliers instead of modifying the base, since the base is the thing the multipliers multiply off of.
			BaseDamage = bd;
			BaseSpeed = bs;
			//BaseVelocity = bv;
			//BaseOverheat = bo;
			//Here's where those multipliers start, by the by.
			//DamageMult = dm;
			//SpeedMult = sm;
			//VelocityMult = vm;
			OverheatMult = om;
			//CritChance = cc;
			//Be VERY careful about adding extra shots. That gets real busted real fast.
			AddShots = sa;
		}

		//This is the part where you set the values of the addon's item form.
		//Remember to use lowercase-"item" as opposed to capitalized-"Item", since it needs to apply to the item being plugged in
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(0, 6, 9, 0);
			item.rare = ItemRarityID.Cyan;
		}

		#region Addon Combos
		public override string SetStaticCombos(Item[] addons)
		{
			//The following is a really basic check for a specific installed addon.
			//It converts the array of beam addon items into proper ModBeamAddons and checks the slot Ice Beam uses for Ice Beam.
			//If you want more checks, add more booleans
			ModBeamAddon[] beamAddons = addons
				.Select(BeamAddonLoader.GetAddon)
				.ToArray();
			bool hasIce = false;


			if (beamAddons[BeamAddonSlotID.Ability] == BeamAddonLoader.GetAddon<IceBeam>())
			{
				hasIce = true;
			}

			if (hasIce)
			{
				//Choose something unique that describes the particular combination to make things easier on yourself.
				//For instance, this keyword is "Fuck" because  F U C K .
				return "Fuck";
			}
			else
			{
				//Return blank if it doesn't get anything, keeps things clean.
				return "";
			}
		}

		public override int[] ComboVisualsGet(string modifier)
		{
			// Really basic checker. This is where you plug in any special properties needed to make special textured work properly.
			// For instance, frame counts
			switch (modifier)
			{
				case "Charged":
					return [2, -1];
					//First value is how many frames the shot texture will have.
					//Second value is dust ID for this specific combo. Use -1 if you don't want to think about it, or -2 if you don't want any of the stock dust

				case "Fuck":
					return [2, DustID.JungleSpore];

				case "FuckCharged":
					return [5, DustID.CursedTorch];

				default:
					return [0, -1];
			}
		}
		#endregion
		//If you want your addon to do cool shit, you gotta whip up a projectile behavior modifier.
		//It's not REQUIRED to have one of these for the addon to be able to function, without anything in these areas it just won't let the beam do anything new on its own
		public override void OnSpawn(MProjectile mpshot, IEntitySource source)
		{
			mpshot.symmetry = true;
		}
	}
}
