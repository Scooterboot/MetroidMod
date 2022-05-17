using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using MetroidModPorted.Common.Players;
using MetroidModPorted.ID;

namespace MetroidModPorted.Content.SuitAddons
{
	public class LightSuitAddon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/LightSuit/LightSuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/LightSuit/LightSuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/LightSuit/LightSuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/LightSuit/LightSuitBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/LightSuit/LightSuitBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/LightSuit/LightSuitGreaves_Legs";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Suit");
			Tooltip.SetDefault("You shouldn't have this."/*"+10 defense\n" +
				"+15 overheat capacity\n" +
				"5% decreased overheat use\n" +
				"5% decreased Missile Charge Combo cost\n" +
				"5% increased hunter damage\n" +
				"5% increased hunter critical strike chance\n" +
				"10% increased movement speed\n" +
				"25% increased energy barrier efficiency\n" + // Provisional name
				"10% increased energy barrier resilience\n" + // Provisional name
				"Immune to damage from the Dark World\n" +
				"Immune to damage from Dark Water"*/);
			AddonSlot = SuitAddonSlotID.Suit_Augment;
			ItemNameLiteral = false;
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = Terraria.Item.buyPrice(0, 11, 70, 0);
			item.rare = ItemRarityID.Lime;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.statDefense += 10;
			player.moveSpeed += 0.10f;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.05f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 5;
			mp.maxOverheat += 15;
			mp.overheatCost -= 0.05f;
			mp.missileCost -= 0.05f;
			mp.EnergyDefenseEfficiency += 0.25f;
			mp.EnergyExpenseEfficiency += 0.10f;
			// code for protection from Dark World/Dark Water goes here
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 248, 224);
		}
	}
}
