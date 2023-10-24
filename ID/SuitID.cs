using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using System;
using System.Collections;

namespace MetroidMod.ID
{
        public class SuitID
        {
                public enum SuitSlot
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
                        Missile_Expansion, // Will be used to reconfigure where missiles are tracked/stored.
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
                                        return SuitSlot.Tank_Energy;
                                case Addon.Tank_Reserve:
                                        return SuitSlot.Tank_Reserve;
                                case Addon.Missile_Expansion:
                                        return SuitSlot.Missile_Expansion;
                                case Addon.Varia:
                                case Addon.VariaV2:
                                        return SuitSlot.Varia;
                                case Addon.Dark_Suit:
                                case Addon.Gravity:
                                case Addon.PED:
                                        return SuitSlot.Utility;
                                case Addon.Light:
                                case Addon.TerraGravity:
                                case Addon.Phazon:
                                case Addon.HazardShield:
                                        return SuitSlot.Utility2;
                                case Addon.Solar:
                                case Addon.Stardust:
                                case Addon.Nebula:
                                case Addon.Vortex:
                                        return SuitSlot.Lunar;
                                case Addon.PowerGrip:
                                        return SuitSlot.Grip;
                                case Addon.ScrewAttack:
                                        return SuitSlot.Attack;
                                case Addon.HiJump:
                                case Addon.SpaceJump_Boots:
                                        return SuitSlot.Boots;
                                case Addon.SpaceJump:
                                        return SuitSlot.Jump;
                                case Addon.SpeedBooster:
                                        return SuitSlot.Speed;
                                case Addon.Scan:
                                        return SuitSlot.Visor_Scan;
                                case Addon.Thermal:
                                case Addon.Dark_Visor:
                                case Addon.Command:
                                        return SuitSlot.Visor_Utility;
                                case Addon.XRay:
                                case Addon.Echo:
                                        return SuitSlot.Visor_Utility2;
                                default:
                                        return -1; // Suit Addon doesn't exist
                        }
                }

                private Dictionary<int, int> dict = new HashTable()
                {
                        {Addon.Tank_Energy, ModContent.ItemType<>()}
                }
                
                
        }
}
