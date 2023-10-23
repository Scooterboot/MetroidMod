using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using System;
using System.Collections;

namespace MetroidMod.ID
{
	public static class BeamID
	{
		// Can potentially be extended to include Suit or Missile slots.
		public enum SlotType
		{
			Charge,
			PrimaryA,
			PrimaryB,
			Secondary,
			Utility
		}

		// Can encompass other addon types by renaming to "Addon". Will potentially lead to a laundry list switch block in getSlotType() and Dictionary below.
		public enum Beam
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

		// Will replace BeamAddonSlotID.cs
		public static int getSlotType(Beam beam)
		{
			switch(beam)
			{
				case Beam.Charge:
				case Beam.ChargeV2:
				case Beam.Luminite:
				case Beam.Hyper:
				case Beam.Phazon:
				case Beam.BattleHammer:
				case Beam.Imperialist:
				case Beam.Judicator:
				case Beam.Magmaul:
				case Beam.ShockCoil:
				case Beam.VoltDriver:
				case Beam.OmegaCannon:
					return SlotType.Charge;
				case Beam.Wave:
				case Beam.WaveV2:
				case Beam.Nebula:
					return SlotType.Utility;
				case Beam.Spazer:
				case Beam.Wide:
				case Beam.Vortex:
					return SlotType.PrimaryA;
				case Beam.PlasmaR:
				case Beam.PlasmaG:
				case Beam.Nova:
				case Beam.Solar:
					return SlotType.PrimaryB;
				case Beam.Ice:
				case Beam.IceV2:
				case Beam.Stardust:
					return SlotType.Secondary;
				default:
					return -1;
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
