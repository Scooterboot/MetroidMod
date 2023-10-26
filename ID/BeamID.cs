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
		public sealed class Util
		{
			[AttributeUsage(AttributeTargets.Field)]
			public class SlotAttribute : System.Attribute
			{
				public int Slot { get; set; }

				public SlotAttribute() { }
				public SlotAttribute(int slot)
				{
					this.Slot = slot;
				}

				public override string ToString()
				{
					return ((SlotType) this.Slot).ToString();
				}
			}

			[AttributeUsage(AttributeTargets.Field)]
			public class VersionAttribute : System.Attribute
			{
				public int Version { get; set; }

				public VersionAttribute() { }
				public VersionAttribute(int version)
				{
					this.Version = version;
				}
	
				public override string ToString()
				{
					return this.Version;
				}
			}

			[AttributeUsage(AttributeTargets.Field)]
			public class ItemIDAttribute : System.Attribute
			{
				public int ItemID { get; set; }

				public ItemIDAttribute() { }
				public ItemIDAttribute(int itemID)
				{
					this.ItemID = itemID;
				}

				public override string ToString()
				{
					return this.ItemID;
				}
			}

			[AttributeUsage(AttributeTargets.Enum)]
			public class EnumEnforcementAttribute : System.Attribute
			{
				public enum EnforcementTypeEnum
				{
					ThrowException,
					DefaultToValue
				}
				public EnforcementTypeEnum EnforcementType { get; set; }
				public EnumEnforcementAttribute()
				{
					this.EnforcementType = EnforcementTypeEnum.DefaultToValue;
				}
				public EnumEmforcementAttribute(EnforcementTypeEnum enforcementType)
				{
					this.EnforcementType = enforcementType;
				}
			}

			// Made plural to allow for possiblity of higher beam modularity
			public static int[] GetSlots(this Enum value)
			{
				return GetSlots(value);
			}
			public static int[] GetSlots<T>(object value)
			{
				return GetSlots(value);
			}
			public static int[] GetSlots(object value)
			{
				if (value == null) 
					return null;

				Type type = value.GetType();

				if (!type.IsEnum)
					throw new ApplicationException("Value parameter must be an enum.");

				FieldInfo fieldInfo = type.GetField(value.ToString());
				object[] slotAttributes = fieldInfo.GetCustomAttributes(typeof(SlotAttribute), false);

				if (slotAttributes == null || slotAttributes.Length == 0)
				{
					object[] enforcementAttributes = fieldInfo.GetCustomAttributes(typeof(EnumEnforcementAttribute), false);

					if (enforcementAttributes != null && enforcementAttributes.Length == 1)
					{
						EnumEnforcementAttribute enforcementAttribute = (EnumEnforcementAttribute)enforcementAttributes[0];
						if (enforcementAttribute.EnforcementType == EnumEnforcementAttribute.EnforcementTypeEnum.ThrowException)
							throw new ApplicationException("No Slot attributes exist in enforced enum of type '" + type.Name + "', value '" + value.ToString() + "'.");
					}
					return new int[0];
				}
				else if (slotAttributes.Length >= 1)
				{
					int[] slots = new int[slotAttributes.Length];
					for (int i = 0; i < slotAttributes.Length; i++)
					{
						slots[0] = slotAttributes[0].Slot;
					}
					return slots;
				}
			}

			// Made plural to allow for possibility of higher beam modularity
			public static int[] GetVersions(this Enum value)
			{
				return GetVersions(value);
			}
			public static int[] GetVersions<T>(object value)
			{
				return GetVersions(value);
			}
			public static int[] GetVersions(object value)
			{
				if (value == null) 
					return null;

				Type type = value.GetType();

				if (!type.IsEnum)
					throw new ApplicationException("Value parameter must be an enum.");

				FieldInfo fieldInfo = type.GetField(value.ToString());
				object[] versionAttributes = fieldInfo.GetCustomAttributes(typeof(VersionAttribute), false);

				if (versionAttributes == null || versionAttributes.Length == 0)
				{
					object[] enforcementAttributes = fieldInfo.GetCustomAttributes(typeof(EnumEnforcementAttribute), false);

					if (enforcementAttributes != null && enforcementAttributes.Length == 1)
					{
						EnumEnforcementAttribute enforcementAttribute = (EnumEnforcementAttribute)enforcementAttributes[0];
						if (enforcementAttribute.EnforcementType == EnumEnforcementAttribute.EnforcementTypeEnum.ThrowException)
							throw new ApplicationException("No Slot attributes exist in enforced enum of type '" + type.Name + "', value '" + value.ToString() + "'.");
					}
					return new int[0];
				}
				else if (versionAttributes.Length >= 1)
				{
					int[] versions = new int[versionAttributes.Length];
					for (int i = 0; i < versionAttributes.Length; i++)
					{
						versions[0] = versionAttributes[0].Version;
					}
					return versions;
				}
			}

			public static int GetItemID(this Enum value)
			{
				return GetItemID(value);
			}
			public static int GetItemID<T>(object value)
			{
				return GetItemID(value);
			}
			public static int GetItemID(object value)
			{
				if (value == null) 
					return null;

				Type type = value.GetType();

				if (!type.IsEnum)
					throw new ApplicationException("Value parameter must be an enum.");

				FieldInfo fieldInfo = type.GetField(value.ToString());
				object[] itemIDAttributes = fieldInfo.GetCustomAttributes(typeof(ItemIDAttribute), false);

				if (itemIDAttributes == null || itemIDAttributes.Length == 0)
				{
					object[] enforcementAttributes = fieldInfo.GetCustomAttributes(typeof(EnumEnforcementAttribute), false);

					if (enforcementAttributes != null && enforcementAttributes.Length == 1)
					{
						EnumEnforcementAttribute enforcementAttribute = (EnumEnforcementAttribute)enforcementAttributes[0];
						if (enforcementAttribute.EnforcementType == EnumEnforcementAttribute.EnforcementTypeEnum.ThrowException)
							throw new ApplicationException("No Slot attributes exist in enforced enum of type '" + type.Name + "', value '" + value.ToString() + "'.");
					}
					return -1;  // BEAM DOESN'T HAVE A DEFINED ITEMID BY ATTRIBUTES
				}
				else if (itemIDAttributes.Length > 1)
					throw new ApplicationException("Too many ItemID attributes exist in enum of type '" + type.Name + "', value '" + value.ToString() + "'.");
				return itemIDAttributes[0].ItemID;
			}
			
		}
		
		public enum SlotType
		{
			Charge,
			PrimaryA,
			PrimaryB,
			Secondary,
			Utility
		}

		[Util.EnumEnforcement(Util.EnumEnforcement.EnumEnforcementType.DefaultToValue)]
		public enum Beam 
		{
			[Util.ItemID(ModContent.ItemType<Addons.ChargeBeamAddon>()), Util.Version(1), Util.Slot(SlotType.Charge)]
			Charge,
			[Util.ItemID(ModContent.ItemType<Addons.V2.ChargeBeamV2Addon>()), Util.Version(2), Util.Slot(SlotType.Charge)]
			ChargeV2,
			[Util.ItemID(ModContent.ItemType<Addons.V3.LuminiteBeamAddon>()), Util.Version(3), Util.Slot(SlotType.Charge)]
			Luminite,
			[Util.ItemID(ModContent.ItemType<Addons.HyperBeamAddon>()), Util.Version(0), Util.Slot(SlotType.Charge)]
			Hyper,
			[Util.ItemID(ModContent.ItemType<Addons.PhazonBeamAddon>()), Util.Version(0), Util.Slot(SlotType.Charge)]
			Phazon,
			[Util.ItemID(ModContent.ItemType<Addons.Hunters.BattleHammerAddon>()), Util.Version(0), Util.Slot(SlotType.Charge)]
			BattleHammer,
			[Util.ItemID(ModContent.ItemType<Addons.Hunters.ImperialistAddon>()), Util.Version(0), Util.Slot(SlotType.Charge)]
			Imperialist,
			[Util.ItemID(ModContent.ItemType<Addons.Hunters.JudicatorAddon>()), Util.Version(0), Util.Slot(SlotType.Charge)]
			Judicator,
			[Util.ItemID(ModContent.ItemType<Addons.Hunters.MagMaulAddon>()), Util.Version(0), Util.Slot(SlotType.Charge)]
			Magmaul,
			[Util.ItemID(ModContent.ItemType<Addons.Hunters.ShockCoilAddon>()), Util.Version(0), Util.Slot(SlotType.Charge)]
			ShockCoil,
			[Util.ItemID(ModContent.ItemType<Addons.Hunters.VoltDriverAddon>()), Util.Version(0), Util.Slot(SlotType.Charge)]
			VoltDriver,
			[Util.ItemID(ModContent.ItemType<Addons.Hunters.OmegaCannonAddon>()), Util.Version(0), Util.Slot(SlotType.Charge)]
			OmegaCannon,
			[Util.ItemID(ModContent.ItemType<Addons.WaveBeamAddon>()), Util.Version(1), Util.Slot(SlotType.Utility)]
			Wave,
			[Util.ItemID(ModContent.ItemType<Addons.V2.WaveBeamV2Addon>()), Util.Version(2), Util.Slot(SlotType.Utility)]
			WaveV2,
			[Util.ItemID(ModContent.ItemType<Addons.V3.NebulaBeamAddon>()), Util.Version(3), Util.Slot(SlotType.Utility)]
			Nebula,
			[Util.ItemID(ModContent.ItemType<Addons.SpazerAddon>()), Util.Version(1), Util.Slot(SlotType.PrimaryA)]
			Spazer,
			[Util.ItemID(ModContent.ItemType<Addons.V2.WideBeamAddon>()), Util.Version(2), Util.Slot(SlotType.PrimaryA)]
			Wide,
			[Util.ItemID(ModContent.ItemType<Addons.V3.VortexBeamAddon>()), Util.Version(3), Util.Slot(SlotType.PrimaryA)]
			Vortex,
			[Util.ItemID(ModContent.ItemType<Addons.PlasmaBeamRedAddon>()), Util.Version(2), Util.Slot(SlotType.PrimaryB)]
			PlasmaR,
			[Util.ItemID(ModContent.ItemType<Addons.PlasmaBeamGreenAddon>()), Util.Version(2), Util.Slot(SlotType.PrimaryB)]
			PlasmaG,
			[Util.ItemID(ModContent.ItemType<Addons.V2.NovaBeamAddon>()), Util.Version(2), Util.Slot(SlotType.PrimaryB)]
			Nova,
			[Util.ItemID(ModContent.ItemType<Addons.V3.SolarBeamAddon>()), Util.Version(3), Util.Slot(SlotType.PrimaryB)]
			Solar,
			[Util.ItemID(ModContent.ItemType<Addons.IceBeamAddon>()), Util.Version(1), Util.Slot(SlotType.Secondary)]
			Ice,
			[Util.ItemID(ModContent.ItemType<Addons.V2.IceBeamV2Addon>()), Util.Version(2), Util.Slot(SlotType.Secondary)]
			IceV2,
			[Util.ItemID(ModContent.ItemType<Addons.V3.StardustBeamAddon>()), Util.Version(3), Util.Slot(SlotType.Secondary)]
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
		
	}
}
