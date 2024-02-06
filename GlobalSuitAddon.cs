using Terraria;
using Terraria.ModLoader;

namespace MetroidMod
{
	public abstract class GlobalSuitAddon : ModType
	{
		protected override void Register()
		{
			SuitAddonLoader.globalAddons.Add(this);
		}

		/// <inheritdoc cref="GlobalItem.UpdateAccessory(Item, Player, bool)"/>
		public virtual void UpdateAccessory(ModSuitAddon item, Player player, bool hideVisual) { }

		/// <inheritdoc cref="GlobalItem.UpdateInventory(Item, Player)"/>
		public virtual void UpdateInventory(ModSuitAddon item, Player player) { }

		/// <inheritdoc cref="GlobalItem.UpdateArmorSet(Player, string)"/>
		public virtual void OnUpdateArmorSet(ModSuitAddon[] suitAddons, Player player) { }

		/// <inheritdoc cref="GlobalItem.UpdateVanitySet(Player, string)"/>
		public virtual void OnUpdateVanitySet(ModSuitAddon[] suitAddons, Player player) { }

		/// <inheritdoc cref="GlobalItem.ArmorSetShadows(Player, string)"/>
		public virtual void ArmorSetShadows(ModSuitAddon[] suitAddons, Player player) { }
	}
}
