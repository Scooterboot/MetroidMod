using MetroidMod.Common.Players;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace MetroidMod.Content.SuitAddons
{
	public class LightSuitAddon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/LightSuit/LightSuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/LightSuit/LightSuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/LightSuit/LightSuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/LightSuit/LightSuitBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/LightSuit/LightSuitBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/LightSuit/LightSuitGreaves_Legs";

		public override bool AddOnlyAddonItem => false;

		//This is where all of the suit addon's stats are stored.
		//They're outside a method so it can be directly accessed by the localization.
		//Put in the numbers like they'd be seen on the tooltip. The values are automatically adjusted for the actual stats.
		public static int suitDef = 19; //Added suit defense
		public static float energyEff = 40f; //%Increased energy damage absorption
		public static float energyRes = 17.5f; //%Increased energy DR
		public static int overheatCap = 30; //Added maximum overheat
		public static float overheatCost = 10f; //%Decreased overheat cost
		public static float comboCost = 10f; //%Decreased Charge Combo cost
		public static float huntDamage = 10f; //%Increased hunter damage
		public static int huntCrit = 8; //Increased hunter crit
		public static float speedUp = 10f; //%Increased movement speed

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(suitDef, energyEff, energyRes, overheatCap, overheatCost, comboCost, huntDamage, huntCrit, speedUp);

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Light Suit");
			/* Tooltip.SetDefault("You shouldn't have this."/*"+19 defense\n" +
				"+30 overheat capacity\n" +
				"10% decreased overheat use\n" +
				"10% decreased Missile Charge Combo cost\n" +
				"10% increased hunter damage\n" +
				"8% increased hunter critical strike chance\n" +
				"10% increased movement speed\n" +
				"40% increased energy barrier efficiency\n" + // Provisional name
				"17.5% increased energy barrier resilience\n" + // Provisional name
				"Immune to damage from the Dark World\n" +
				"Immune to damage from Dark Water"); */
			AddonSlot = SuitAddonSlotID.Suit_Primary;
			ItemNameLiteral = false;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(0, 11, 70, 0);
			item.rare = ItemRarityID.Lime;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.statDefense += suitDef;
			player.moveSpeed += speedUp / 100;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += huntCrit;
			mp.maxOverheat += overheatCap;
			mp.overheatCost -= overheatCost / 100;
			mp.missileCost -= comboCost / 100;
			mp.EnergyDefenseEfficiency += energyEff / 100;
			mp.EnergyExpenseEfficiency += energyRes / 100;
			// code for protection from Dark World/Dark Water goes here
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 248, 224);
		}
	}
}
