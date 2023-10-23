

namespace MetroidMod.ID
{
	public class BeamID
	{
		public enum Charge
                {
                        Charge,
                        ChargeV2,
                        Luminite,
                        Hyper,
                        Phazon
                }
                public enum Hunters
                {
                        BattleHammer,
                        Imperialist,
                        Judicator,
                        Magmaul,
                        ShockCoil,
                        VoltDriver,
                        OmegaCannon
                }
                public enum Utility
                {
                        Wave,
                        WaveV2,
                        Nebula
                }
                public enum PrimaryA
                {
                        Spazer,
                        Wide,
                        Vortex
                }
                public enum PrimaryB
                {
                        PlasmaR,
                        PlasmaG,
                        Nova,
                        Solar
                }
                public enum Secondary
                {
                        Ice,
                        IceV2,
                        Stardust
                }

		// This could potentially substantially reduce backend processing time and eventually remove much clutter from PowerBeam.cs
		public static HashTable Dict = new HashTable{
			{Charge.Charge, 	ModContent.ItemType<Addons.ChargeBeamAddon>()},
			{Charge.ChargeV2, 	ModContent.ItemType<Addons.V2.ChargeBeamV2Addon>()},
			{Charge.Luminite, 	ModContent.ItemType<Addons.V3.LuminiteBeamAddon>()},
			{Charge.Hyper, 		ModContent.ItemType<Addons.HyperBeamAddon>()},
			{Charge.Phazon, 	ModContent.ItemType<Addons.PhazonBeamAddon>()},
			{Hunters.BattleHammer, 	ModContent.ItemType<Addons.Hunters.BattleHammerAddon>()},
			{Hunters.Imperialist, 	ModContent.ItemType<Addons.Hunters.ImperialistAddon>()},
			{Hunters.Judicator, 	ModContent.ItemType<Addons.Hunters.JudicatorAddon>()},
			{Hunters.Magmaul, 	ModContent.ItemType<Addons.Hunters.MagMaulAddon>()},
			{Hunters.ShockCoil, 	ModContent.ItemType<Addons.Hunters.ShockCoilAddon>()},
			{Hunters.VoltDriver, 	ModContent.ItemType<Addons.Hunters.VoltDriverAddon>()},
			{Hunters.OmegaCannon, 	ModContent.ItemType<Addons.Hunters.OmegaCannonAddon>()},
			{Utility.Wave, 		ModContent.ItemType<Addons.WaveBeamAddon>()},
			{Utility.WaveV2, 	ModContent.ItemType<Addons.V2.WaveBeamV2Addon>()},
			{Utility.Nebula, 	ModContent.ItemType<Addons.V3.NebulaBeamAddon>()},
			{PrimaryA.Spazer, 	ModContent.ItemType<Addons.SpazerAddon>()},
			{PrimaryA.Wide, 	ModContent.ItemType<Addons.V2.WideBeamAddon>()},
			{PrimaryA.Vortex, 	ModContent.ItemType<Addons.V3.VortexBeamAddon>()},
			{PrimaryB.PlasmaR, 	ModContent.ItemType<Addons.PlasmaBeamRedAddon>()},
			{PrimaryB.PlasmaG, 	ModContent.ItemType<Addons.PlasmaBeamGreenAddon>()},
			{PrimaryB.Nova, 	ModContent.ItemType<Addons.V2.NovaBeamAddon>()},
			{PrimaryB.Solar, 	ModContent.ItemType<Addons.V3.SolarBeamAddon>()},
			{Secondary.Ice, 	ModContent.ItemType<Addons.IceBeamAddon>()},
			{Secondary.IceV2, 	ModContent.ItemType<Addons.V2.IceBeamV2Addon>()},
			{Secondary.Stardust, 	ModContent.ItemType<Addons.V3.StardustBeamAddon>()}
		};
		
	}
}
