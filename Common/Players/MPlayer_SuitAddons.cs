using System;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Common.Players
{
	public partial class MPlayer : ModPlayer
	{
		// Failsaves.
		private Item[] _suitAddons;
		public Item[] SuitAddons
		{
			get
			{
				if (_suitAddons == null)
				{
					_suitAddons = new Item[SuitAddonSlotID.Count];
					for (int i = 0; i < _suitAddons.Length; i++)
					{
						_suitAddons[i] = new Item();
						_suitAddons[i].TurnToAir();
					}
				}

				return _suitAddons;
			}
			set { _suitAddons = value; }
		}
		public bool ShouldShowArmorUI = false;
		public void ResetEffects_SuitAddons()
		{
			ShouldShowArmorUI = false;
		}
		public void PreUpdate_SuitAddons()
		{

		}
		public void PostUpdateMiscEffects_SuitAddons()
		{

		}
		public void PostUpdateRunSpeeds_SuitAddons()
		{

		}
		public void PostUpdate_SuitAddons()
		{

		}
	}
}
