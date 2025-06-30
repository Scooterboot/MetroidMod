using Terraria.Localization;

namespace MetroidMod.ID
{
	public class BeamAddonSlotID
	{
		/// <summary>
		/// No slot. Used as a placeholder.
		/// </summary>
		public const short None = -1;
		/// <summary>
		/// The <b>base</b> upon which other addons modify, and can be stored in <b>Quick-Swap</b> to change weapons on the fly.
		/// <br/>Things like the <b>Charge Beam</b> go here.
		/// </summary>
		public const short Primary = 0;
		/// <summary>
		/// Typically for addons that <b>apply after-effects</b> to your beam shot.
		/// <br/>Things like the <b>Ice Beam</b> go here.
		/// </summary>
		public const short Ability = 1;
		/// <summary>
		/// Typically for addons that affect how the beam <b>interacts with terrain</b> or the laws of physics.
		/// <br/>Things like the <b>Wave Beam</b> go here.
		/// </summary>
		public const short Ion = 2;
		/// <summary>
		/// Typically for addons that affect <b>energy distribution</b>. Usually this means projectile count.
		/// <br/> Things like the <b>Spazer Beam</b> go here.
		/// </summary>
		public const short Spread = 3;
		/// <summary>
		/// Typically for addons that affect how the beam <b>interacts with enemies</b>.
		/// <br/> Things like the <b>Plasma Beam</b> go here.
		/// </summary>
		public const short Secondary = 4;
		/// <summary>
		/// Used <i>exclusively</i> for ammunition, like UA Expansions. <br/><b>This slot does not get checked with the others.</b>
		/// </summary>
		public const short Ammo = 5;
		/// <summary>
		/// The total beam slot count.<br/>Not an actual slot.
		/// </summary>
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
