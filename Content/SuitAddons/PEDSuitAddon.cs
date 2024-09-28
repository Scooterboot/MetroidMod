using MetroidMod.Common.Players;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

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

		//This is where all of the suit addon's stats are stored.
		//They're outside a method so it can be directly accessed by the localization.
		//Put in the numbers like they'd be seen on the tooltip. The values are automatically adjusted for the actual stats.
		public static int suitDef = 9; //Added suit defense
		public static float energyEff = 5f; //%Increased energy damage absorption
		public static float energyRes = 7.5f; //%Increased energy DR
		public static int overheatCap = 15; //Added maximum overheat
		public static float overheatCost = 5f; //%Decreased overheat cost
		public static float comboCost = 5f; //%Decreased Charge Combo cost
		public static float huntDamage = 5f; //%Increased hunter damage
		public static int huntCrit = 3; //Increased hunter crit

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(suitDef, energyEff, energyRes, overheatCap, overheatCost, comboCost, huntDamage, huntCrit);

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("PED Suit");
			/* Tooltip.SetDefault("You shouldn't have this."/*"+9 defense\n" +
				"+15 overheat capacity\n" +
				"5% decreased overheat use\n" +
				"5% decreased Missile Charge Combo cost\n" +
				"5% increased hunter damage\n" +
				"3% increased hunter critical strike chance\n" +
				"5% increased energy barrier efficiency\n" + // Provisional name
				"7.5% increased energy barrier resilience\n" + // Provisional name
				"Press the Hypermode key to activate Hypermode (take 100 damage to gain +50% damage for 20 seconds, 120 s cooldown)\n" +
				"Slightly increased health regen when standing on Phazon"); */
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
			player.statDefense += suitDef;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += huntCrit;
			mp.phazonImmune = true;
			mp.phazonRegen += 2;
			// workaround for Hazard Shield being applied earlier than PED Suit causing hazard shield buffing PED to not work
			if (mp.hazardShield > 0) { mp.phazonRegen += 2; }
			mp.maxOverheat += overheatCap;
			mp.overheatCost -= overheatCost / 100;
			mp.missileCost -= comboCost / 100;
			mp.EnergyDefenseEfficiency += energyEff / 100;
			mp.EnergyExpenseEfficiency += energyRes / 100;
			// code to activate Hypermode goes here; might need to add a Hypermode hook to MPlayer like Sense Move
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(0, 228, 255);
		}
	}
}
