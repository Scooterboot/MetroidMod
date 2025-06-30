using Terraria.Localization;

namespace MetroidMod.ID
{
	public class MissileAddonSlotID
	{
		/// <summary>
		/// Not a real slot, used as a placeholder.
		/// </summary>
		public const short None = -1;
		/// <summary>
		/// Used to hold <b>Charge Combos</b>.<br/>Charge combos can go into the <b>Charge Combo Quick-Swap</b> array.
		/// <br/><br/>Examples: Wavebuster, Storm Missile, Nova Laser
		/// </summary>
		public const short Charge = 0;
		/// <summary>
		/// Used to hold upgrades to the <b>base missile projectile.</b>
		/// <br/><br/>Things like the <b>Super Missile</b> go here.
		/// </summary>
		public const short Primary = 1;
		/// <summary>
		/// Used <i>exclusively</i> for <b>Missile Tanks</b>.
		/// </summary>
		public const short Expansion = 2;
		/// <summary>
		/// Not a real slot, used to count the total number of slots.
		/// </summary>
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
