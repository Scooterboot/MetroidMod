using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidModPorted.Content.Items.Vanity.Legacy;

namespace MetroidModPorted.Content.Items.Vanity
{
	public class VanityPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vanity Pack");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}\n" +
			"Contains:\n"+
			"Ancient Power Suit vanity set\n"+
			"Ancient Varia Suit vanity set\n"+
			"Ancient Varia Suit V2 vanity set");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			//Item.expert = true;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 0, 10, 0);
		}
		
		public override bool CanRightClick() => true;
		
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyPowerSuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyPowerSuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyPowerSuitGreaves>());
			
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyVariaSuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyVariaSuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyVariaSuitGreaves>());
			
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyVariaSuitV2Helmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyVariaSuitV2Breastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyVariaSuitV2Greaves>());
		}
	}
	
	public class VanityPack_Prime : VanityPack
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vanity Pack - Prime");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}\n" +
			"Contains:\n"+
			"Ancient Gravity Suit vanity set\n"+
			"Ancient Terra Gravity Suit vanity set\n"+
			"Ancient Phazon Suit vanity set");
		}
		
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyGravitySuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyGravitySuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyGravitySuitGreaves>());
			
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyTerraGravitySuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyTerraGravitySuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyTerraGravitySuitGreaves>());
			
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyPhazonSuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyPhazonSuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyPhazonSuitGreaves>());
		}
	}
	
	public class VanityPack_Echoes : VanityPack
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vanity Pack - Echoes");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}\n" +
			"Contains:\n"+
			"Ancient Dark Suit vanity set\n"+
			"Ancient Light Suit vanity set");
		}
		
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyDarkSuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyDarkSuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyDarkSuitGreaves>());
			
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyLightSuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyLightSuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyLightSuitGreaves>());
		}
	}
	
	public class VanityPack_Corruption : VanityPack
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vanity Pack - Corruption");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}\n" +
			"Contains:\n"+
			"Ancient PED Suit vanity set\n"+
			"Ancient Hazard Shield Suit vanity set");
		}
		
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyPEDSuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyPEDSuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyPEDSuitGreaves>());
			
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyHazardShieldSuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyHazardShieldSuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyHazardShieldSuitGreaves>());
		}
	}
	
	public class VanityPack_Lunar : VanityPack
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vanity Pack - Lunar");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}\n" +
			"Contains:\n"+
			"Ancient Vortex Gravity Suit vanity set\n"+
			"Ancient Nebula Phazon Suit vanity set\n"+
			"Ancient Solar Light Suit vanity set\n"+
			"Ancient Stardust Hazard Shield Suit vanity set");
		}
		
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyVortexGravitySuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyVortexGravitySuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyVortexGravitySuitGreaves>());
			
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyNebulaPhazonSuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyNebulaPhazonSuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyNebulaPhazonSuitGreaves>());
			
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacySolarLightSuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacySolarLightSuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacySolarLightSuitGreaves>());
			
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyStardustHazardShieldSuitHelmet>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyStardustHazardShieldSuitBreastplate>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LegacyStardustHazardShieldSuitGreaves>());
		}
	}
}
