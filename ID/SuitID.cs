using System;
using System.Collections;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using MetroidMod.Content.Items;

namespace MetroidMod.ID
{
        public static class SuitID
        {
                public enum SlotType
                {
                        Tank_Energy,
                        Tank_Reserve,
                        Missile_Expansion,      // Will be used to reconfigure where missiles are tracked/stored.
                        Varia,
                        Utility,                // Dark Suit, Gravity Suit, P.E.D. Suit
                        Utility2,               // Light Suit, Terra Gravity Suit, Phazon Suit, Hazard Shield
                        Lunar,
                        Grip,
                        Attack,
                        Boots,                  // Hi-Jump Boots, Space Jump Boots
                        Jump,                   // Space Jump
                        Speed,                  // Speed Booster
                        Visor_Scan,
                        Visor_Utility,          // Thermal Visor, Dark Visor, Command Visor
                        Visor_Utility2          // XRay Visor, Echo Visor
                }
                
                public enum Addon
                {
                        Tank_Energy,            // Energy
                        Tank_Reserve,           // Reserve
                        Missile_Expansion,      // Will be used to reconfigure where missiles are tracked/stored.
                        Varia,                  // Varia
                        VariaV2,
                        Dark_Suit,              // Utility
                        Gravity,
                        PED,
                        Light,                  // Utility2
                        TerraGravity,
                        Phazon,
                        HazardShield,
                        Solar,                  // Lunar
                        Stardust,
                        Nebula,
                        Vortex,
                        PowerGrip,              // Grip
                        ScrewAttack,            // Attack
                        HiJump,                 // Boots
                        SpaceJump_Boots,
                        SpaceJump,              // Jump
                        SpeedBooster,           // Speed
                        Scan,                   // Visor_Scan
                        Thermal,                // Visor_Utility
                        Dark_Visor,
                        Command,
                        XRay,                   // Visor_Utility2
                        Echo                        
                }

                public static int getSlotType(Addon suit)
                {
                        switch(suit)
                        {
                                case Addon.Tank_Energy:
                                        return SlotType.Tank_Energy;
                                case Addon.Tank_Reserve:
                                        return SlotType.Tank_Reserve;
                                case Addon.Missile_Expansion:
                                        return SlotType.Missile_Expansion;
                                case Addon.Varia:
                                case Addon.VariaV2:
                                        return SlotType.Varia;
                                case Addon.Dark_Suit:
                                case Addon.Gravity:
                                case Addon.PED:
                                        return SlotType.Utility;
                                case Addon.Light:
                                case Addon.TerraGravity:
                                case Addon.Phazon:
                                case Addon.HazardShield:
                                        return SlotType.Utility2;
                                case Addon.Solar:
                                case Addon.Stardust:
                                case Addon.Nebula:
                                case Addon.Vortex:
                                        return SlotType.Lunar;
                                case Addon.PowerGrip:
                                        return SlotType.Grip;
                                case Addon.ScrewAttack:
                                        return SlotType.Attack;
                                case Addon.HiJump:
                                case Addon.SpaceJump_Boots:
                                        return SlotType.Boots;
                                case Addon.SpaceJump:
                                        return SlotType.Jump;
                                case Addon.SpeedBooster:
                                        return SlotType.Speed;
                                case Addon.Scan:
                                        return SlotType.Visor_Scan;
                                case Addon.Thermal:
                                case Addon.Dark_Visor:
                                case Addon.Command:
                                        return SlotType.Visor_Utility;
                                case Addon.XRay:
                                case Addon.Echo:
                                        return SlotType.Visor_Utility2;
                                default:
                                        return -1; // Suit Addon doesn't exist
                        }
                }

                private static Dictionary<int, int> dict = new HashTable()
                {
                        {Addon.Tank_Energy, ModContent.ItemType<SuitAddons.EnergyTank>()},
			{Addon.Tank_Reserve, ModContent.ItemType<SuitAddons.ReserveTank>()},
			{Addon.MissileExpansion, ModContent.ItemType<Tiles.MissileExpansion>()},
			{Addon.Varia, ModContent.ItemType<SuitAddons.VariaSuitAddon>()},
			{Addon.VariaV2, ModContent.ItemType<SuitAddons.VariaSuitV2Addon>()},
			{Addon.Dark_Suit, ModContent.ItemType<SuitAddons.DarkSuitAddon>()},
			{Addon.Gravity, ModContent.ItemType<SuitAddons.GravitySuitAddon>()},
			{Addon.PED, ModContent.ItemType<SuitAddons.PEDSuitAddon>()},
			{Addon.Light, ModContent.ItemType<SuitAddons.LightSuitAddon>()},
			{Addon.TerraGravity, ModContent.ItemType<SuitAddons.TerraGravitySuitAddon>()},
			{Addon.Phazon, ModContent.ItemType<SuitAddons.PhazonSuitAddon>()},
			{Addon.HazardShield, ModContent.ItemType<SuitAddons.HazardShieldAddon>()},
			{Addon.Solar, ModContent.ItemType<SuitAddons.SolarAugment>()},
			//{Addon.Stardust, ModContent.ItemType<SuitAddons.StardustAugment>()},
			{Addon.Nebula, ModContent.ItemType<SuitAddons.NebulaAugment>()},
			{Addon.Vortex, ModContent.ItemType<SuitAddons.VortexAugment>()},
			{Addon.PowerGrip, ModContent.ItemType<SuitAddons.PowerGrip>()},
			{Addon.ScrewAttack, ModContent.ItemType<SuitAddons.ScrewAttack>()},
			{Addon.HiJump, ModContent.ItemType<SuitAddons.HiJumpBoots>()},
			{Addon.SpaceJump_Boots, ModContent.ItemType<SuitAddons.SpaceJumpBoots>()},
			{Addon.SpaceJump, ModContent.ItemType<SuitAddons.SpaceJump>()},
			{Addon.SpeedBooster, ModContent.ItemType<SuitAddons.SpeedBooster>()},
			{Addon.Scan, ModContent.ItemType<SuitAddons.ScanVisor>()},
			//{Addon.Thermal, ModContent.ItemType<SuitAddons.ThermalVisor>()},
			//{Addon.Dark_Visor, ModContent.ItemType<SuitAddons.DarkVisor>()},
			//{Addon.Command, ModContent.ItemType<SuitAddons.CommandVisor>()},
			{Addon.XRay, ModContent.ItemType<SuitAddons.XRayScope>()}//,
			//{Addon.Echo, ModContent.ItemType<SuitAddons.EchoVisor>()}
                }

		public static int getItemTypeOf(Addon suit)
		{
			return this.dict[(int) suit];
		}
                
        }
}
