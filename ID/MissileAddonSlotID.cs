using Terraria.Localization;

namespace MetroidMod.ID
{
	public class MissileAddonSlotID
	{
		public const short None = -1;
		public const short Charge = 0;
		public const short Primary = 1;
		public const short Expansion = 2;
		public const short Count = 3;

		/// <summary>
		/// Takes in a Missile Slot number and returns its name as a localized string.
		/// </summary>
		/// <param name="slot"></param>
		/// <returns></returns>
		public static string GetSlotName(int slot)
		{
			switch (slot)
			{
				case 0:
					return Language.GetTextValue("Mods.MetroidMod.MissileSlotDictionary.ChargeCombo");
				case 1:
					return Language.GetTextValue("Mods.MetroidMod.MissileSlotDictionary.Primary");
				case 2:
					return Language.GetTextValue("Mods.MetroidMod.MissileSlotDictionary.Tanks");
				default:
					return Language.GetTextValue("Mods.MetroidMod.MissileSlotDictionary.None");
			}
		}
	}
}
