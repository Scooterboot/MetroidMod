using Terraria.Localization;

namespace MetroidMod.ID
{
	public class BeamAddonSlotID
	{
		public const short None = -1;
		public const short Charge = 0;
		public const short Secondary = 1;
		public const short Utility = 2;
		public const short PrimaryA = 3;
		public const short PrimaryB = 4;
		public const short UA = 5;
		public const short Count = 6;

		/// <summary>
		/// Takes in a Beam Slot number and returns its name as a localized string.
		/// </summary>
		/// <param name="slot"></param>
		/// <returns></returns>
		public static string GetSlotName(int slot)
		{
			switch (slot)
			{
				case 0:
					return Language.GetTextValue("Mods.MetroidMod.BeamSlotDictionary.Primary");
				case 1:
					return Language.GetTextValue("Mods.MetroidMod.BeamSlotDictionary.Ability");
				case 2:
					return Language.GetTextValue("Mods.MetroidMod.BeamSlotDictionary.Ion");
				case 3:
					return Language.GetTextValue("Mods.MetroidMod.BeamSlotDictionary.Spread");
				case 4:
					return Language.GetTextValue("Mods.MetroidMod.BeamSlotDictionary.Secondary");
				case 5:
					return Language.GetTextValue("Mods.MetroidMod.BeamSlotDictionary.Ammo");
				default:
					return Language.GetTextValue("Mods.MetroidMod.BeamSlotDictionary.None");
			}
		}
	}
}
