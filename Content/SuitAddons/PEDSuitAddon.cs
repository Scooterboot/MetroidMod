using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using MetroidMod.Common.Players;
using MetroidMod.ID;

namespace MetroidMod.Content.SuitAddons
{
	public class PEDSuitAddon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/PEDSuit/PEDSuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/PEDSuit/PEDSuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/PEDSuit/PEDSuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/PEDSuit/PEDSuitBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/PEDSuit/PEDSuitBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/PEDSuit/PEDSuitGreaves_Legs";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("PED Suit");
			Tooltip.SetDefault("You shouldn't have this."/*"+9 defense\n" +
				"+15 overheat capacity\n" +
				"5% decreased overheat use\n" +
				"5% decreased Missile Charge Combo cost\n" +
				"5% increased hunter damage\n" +
				"3% increased hunter critical strike chance\n" +
				"5% increased energy barrier efficiency\n" + // Provisional name
				"7.5% increased energy barrier resilience\n" + // Provisional name
				"Press the Hypermode key to activate Hypermode (take 100 damage to gain +50% damage for 20 seconds, 120 s cooldown)\n" +
				"Slightly increased health regen when standing on Phazon"*/);
			AddonSlot = SuitAddonSlotID.Suit_Primary;
			ItemNameLiteral = false;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(0, 7, 80, 0);
			item.rare = ItemRarityID.Pink;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.statDefense += 9;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.05f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 3;
			mp.phazonImmune = true;
			mp.phazonRegen += 2;
			// workaround for Hazard Shield being applied earlier than PED Suit causing hazard shield buffing PED to not work
			if (mp.hazardShield > 0) { mp.phazonRegen += 2; }
			mp.maxOverheat += 15;
			mp.overheatCost -= 0.05f;
			mp.missileCost -= 0.05f;
			mp.EnergyDefenseEfficiency += 0.05f;
			mp.EnergyExpenseEfficiency += 0.075f;
			// code to activate Hypermode goes here; might need to add a Hypermode hook to MPlayer like Sense Move
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(0, 228, 255);
		}
	}
}
