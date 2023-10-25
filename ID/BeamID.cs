using System;
using System.Collections;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using MetroidMod.Content.Items;

namespace MetroidMod.ID
{
	public static class BeamID
	{
		public enum SlotType
		{
			Charge,
			PrimaryA,
			PrimaryB,
			Secondary,
			Utility
		}

		public enum Beam // Going to go back over this enum with Attributes to give it the proper "database" style and get rid of the ugly HashTable dict.
		{
			Charge,
			ChargeV2,
			Luminite,
			Hyper,
			Phazon,
			BattleHammer,
			Imperialist,
			Judicator,
			Magmaul,
			ShockCoil,
			VoltDriver,
			OmegaCannon,
			Wave,
			WaveV2,
			Nebula,
			Spazer,
			Wide,
			Vortex,
			PlasmaR,
			PlasmaG,
			Nova,
			Solar,
			Ice,
			IceV2,
			Stardust
		}

		public static int[] getBeamsBySlot(SlotType slot)
		{
			switch(slot)
			{
				case SlotType.Charge:
					return new sealed int[] {Beam.Charge, Beam.ChargeV2, Beam.Luminite, Beam.Hyper, Beam.Phazon, Beam.BattleHammer, Beam.Imperialist, Beam.Judicator, Beam.Magmaul, Beam.ShockCoil, Beam.VoltDriver, Beam.OmegaCannon};
				case SlotType.Utility:
					return new sealed int[] {Beam.Wave, Beam.WaveV2, Beam.Nebula};
				case SlotType.PrimaryA:
					return new sealed int[] {Beam.Spazer, Beam.Wide, Beam.Vortex};
				case SlotType.PrimaryB:
					return new sealed int[] {Beam.PlasmaR, Beam.PlasmaG, Beam.Nova, Beam.Solar};
				case SlotType.Secondary:
					return new sealed int[] {Beam.Ice, Beam.IceV2, Beam.Stardust};
				default:
					return null;
			}
		}

		public static int[] getBeamsByVersion(int ver)
		{
			switch(ver)
			{
				case 1:
					return new sealed int[] {Beam.Charge, Beam.Wave, Beam.Spazer, Beam.Ice};
				case 2:
					return new sealed int[] {Beam.ChargeV2, Beam.WaveV2, Beam.Wide, Beam.IceV2, Beam.PlasmaG, Beam.PlasmaR, Beam.Nova};
				case 3:
					return new sealed int[] {Beam.Luminite, Beam.Nebula, Beam.Vortex, Beam.Solar, Beam.Stardust};
				case 0:
					return new sealed int[] {Beam.Hyper, Beam.Phazon, Beam.BattleHammer, Beam.Imperialist, Beam.Judicator, Beam.Magmaul, Beam.ShockCoil, Beam.VoltDriver, Beam.OmegaCannon};
				default:
					return null;
			}
		}

		// This could potentially substantially reduce backend processing time and eventually remove much clutter from PowerBeam.cs
		private static Dictionary<int, int> dict = new HashTable()
		{	
			{Beam.Charge, 		ModContent.ItemType<Addons.ChargeBeamAddon>()},
			{Beam.ChargeV2, 	ModContent.ItemType<Addons.V2.ChargeBeamV2Addon>()},
			{Beam.Luminite, 	ModContent.ItemType<Addons.V3.LuminiteBeamAddon>()},
			{Beam.Hyper, 		ModContent.ItemType<Addons.HyperBeamAddon>()},
			{Beam.Phazon, 		ModContent.ItemType<Addons.PhazonBeamAddon>()},
			{Beam.BattleHammer, 	ModContent.ItemType<Addons.Hunters.BattleHammerAddon>()},
			{Beam.Imperialist, 	ModContent.ItemType<Addons.Hunters.ImperialistAddon>()},
			{Beam.Judicator, 	ModContent.ItemType<Addons.Hunters.JudicatorAddon>()},
			{Beam.Magmaul, 		ModContent.ItemType<Addons.Hunters.MagMaulAddon>()},
			{Beam.ShockCoil, 	ModContent.ItemType<Addons.Hunters.ShockCoilAddon>()},
			{Beam.VoltDriver, 	ModContent.ItemType<Addons.Hunters.VoltDriverAddon>()},
			{Beam.OmegaCannon, 	ModContent.ItemType<Addons.Hunters.OmegaCannonAddon>()},
			{Beam.Wave, 		ModContent.ItemType<Addons.WaveBeamAddon>()},
			{Beam.WaveV2, 		ModContent.ItemType<Addons.V2.WaveBeamV2Addon>()},
			{Beam.Nebula, 		ModContent.ItemType<Addons.V3.NebulaBeamAddon>()},
			{Beam.Spazer, 		ModContent.ItemType<Addons.SpazerAddon>()},
			{Beam.Wide, 		ModContent.ItemType<Addons.V2.WideBeamAddon>()},
			{Beam.Vortex, 		ModContent.ItemType<Addons.V3.VortexBeamAddon>()},
			{Beam.PlasmaR, 		ModContent.ItemType<Addons.PlasmaBeamRedAddon>()},
			{Beam.PlasmaG, 		ModContent.ItemType<Addons.PlasmaBeamGreenAddon>()},
			{Beam.Nova, 		ModContent.ItemType<Addons.V2.NovaBeamAddon>()},
			{Beam.Solar, 		ModContent.ItemType<Addons.V3.SolarBeamAddon>()},
			{Beam.Ice, 		ModContent.ItemType<Addons.IceBeamAddon>()},
			{Beam.IceV2, 		ModContent.ItemType<Addons.V2.IceBeamV2Addon>()},
			{Beam.Stardust, 	ModContent.ItemType<Addons.V3.StardustBeamAddon>()}
		};

		// Will replace any need for excessive ItemType<> calls
		public static int getItemTypeOf(Beam beam)
		{
			return this.dict[(int) beam];
		}
		
	}
}
