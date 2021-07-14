using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.vanity
{
	public class VanityPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vanity Pack");
			Tooltip.SetDefault("Right click to open\n" +
			"Contains:\n"+
			"Ancient Power Suit vanity set\n"+
			"Ancient Varia Suit vanity set\n"+
			"Ancient Varia Suit V2 vanity set");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 24;
			item.height = 24;
			//item.expert = true;
			item.rare = 4;
			item.value = Item.sellPrice(0,0,10,0);
		}
		
		public override bool CanRightClick() => true;
		
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("LegacyPowerSuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyPowerSuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyPowerSuitGreaves"));
			
			player.QuickSpawnItem(mod.ItemType("LegacyVariaSuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyVariaSuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyVariaSuitGreaves"));
			
			player.QuickSpawnItem(mod.ItemType("LegacyVariaSuitV2Helmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyVariaSuitV2Breastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyVariaSuitV2Greaves"));
		}
	}
	
	public class VanityPack_Prime : VanityPack
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vanity Pack - Prime");
			Tooltip.SetDefault("Right click to open\n" +
			"Contains:\n"+
			"Ancient Gravity Suit vanity set\n"+
			"Ancient Terra Gravity Suit vanity set\n"+
			"Ancient Phazon Suit vanity set");
		}
		
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("LegacyGravitySuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyGravitySuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyGravitySuitGreaves"));
			
			player.QuickSpawnItem(mod.ItemType("LegacyTerraGravitySuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyTerraGravitySuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyTerraGravitySuitGreaves"));
			
			player.QuickSpawnItem(mod.ItemType("LegacyPhazonSuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyPhazonSuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyPhazonSuitGreaves"));
		}
	}
	
	public class VanityPack_Echoes : VanityPack
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vanity Pack - Echoes");
			Tooltip.SetDefault("Right click to open\n" +
			"Contains:\n"+
			"Ancient Dark Suit vanity set\n"+
			"Ancient Light Suit vanity set");
		}
		
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("LegacyDarkSuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyDarkSuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyDarkSuitGreaves"));
			
			player.QuickSpawnItem(mod.ItemType("LegacyLightSuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyLightSuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyLightSuitGreaves"));
		}
	}
	
	public class VanityPack_Corruption : VanityPack
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vanity Pack - Corruption");
			Tooltip.SetDefault("Right click to open\n" +
			"Contains:\n"+
			"Ancient PED Suit vanity set\n"+
			"Ancient Hazard Shield Suit vanity set");
		}
		
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("LegacyPEDSuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyPEDSuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyPEDSuitGreaves"));
			
			player.QuickSpawnItem(mod.ItemType("LegacyHazardShieldSuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyHazardShieldSuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyHazardShieldSuitGreaves"));
		}
	}
	
	public class VanityPack_Lunar : VanityPack
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vanity Pack - Lunar");
			Tooltip.SetDefault("Right click to open\n" +
			"Contains:\n"+
			"Ancient Vortex Gravity Suit vanity set\n"+
			"Ancient Nebula Phazon Suit vanity set\n"+
			"Ancient Solar Light Suit vanity set\n"+
			"Ancient Stardust Hazard Shield Suit vanity set");
		}
		
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("LegacyVortexGravitySuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyVortexGravitySuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyVortexGravitySuitGreaves"));
			
			player.QuickSpawnItem(mod.ItemType("LegacyNebulaPhazonSuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyNebulaPhazonSuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyNebulaPhazonSuitGreaves"));
			
			player.QuickSpawnItem(mod.ItemType("LegacySolarLightSuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacySolarLightSuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacySolarLightSuitGreaves"));
			
			player.QuickSpawnItem(mod.ItemType("LegacyStardustHazardShieldSuitHelmet"));
			player.QuickSpawnItem(mod.ItemType("LegacyStardustHazardShieldSuitBreastplate"));
			player.QuickSpawnItem(mod.ItemType("LegacyStardustHazardShieldSuitGreaves"));
		}
	}
}