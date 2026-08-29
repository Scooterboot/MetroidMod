// using MetroidMod.Common.Players;
// using MetroidMod.ID;
// using Terraria;
// using Terraria.DataStructures;
// using Terraria.ID;
// using Terraria.Localization;
// using Terraria.ModLoader;

// namespace MetroidMod.Content.SuitAddons
// {
// 	public class FusionOmegaAddon : ModSuitAddon
// 	{
// 		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/FusionOmega/FusionOmegaItem";

// 		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/FusionOmega/FusionOmegaTile";

// 		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/FusionOmega/FusionOmegaSuitHelmet_Head";

// 		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/FusionOmega/FusionOmegaSuitBreastplate_Body";

// 		//public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/FusionOmega/FusionOmegaSuitBreastplate_Arms_Glow";

// 		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/FusionOmega/FusionOmegaSuitGreaves_Legs";

// 		public override bool AddOnlyAddonItem => false;

// 		//public override bool CanGenerateOnChozoStatue() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || MSystem.bossesDown.HasFlag(MetroidBossDown.downedKraid);

// 		//public override double GenerationChance() => 4;

// 		//This is where all of the suit addon's stats are stored.
// 		//They're outside a method so it can be directly accessed by the localization.
// 		//Put in the numbers like they'd be seen on the tooltip. The values are automatically adjusted for the actual stats.
// 		public static int suitDef = 35; //Added suit defense
// 		public static int energyCap = 4; //Added E-tank capacity
// 		public static float energyEff = 40f; //%Increased energy damage absorption
// 		public static float energyRes = 37.5f; //%Increased energy DR
// 		public static int overheatCap = 80; //Added maximum overheat
// 		public static float overheatCost = 20f; //%Decreased overheat cost
// 		public static float comboCost = 25f; //%Decreased Charge Combo cost
// 		public static float huntDamage = 30f; //%Increased hunter damage
// 		public static int huntCrit = 15; //Increased hunter crit
// 		public static float speedUp = 10f; //%Increased movement speed
// 		public static float extraBreath = 200f; //%Increased breath meter

// 		public override LocalizedText ItemTooltip => base.ItemTooltip.WithFormatArgs(suitDef, energyCap, energyEff, energyRes, overheatCap, overheatCost, comboCost, huntDamage, huntCrit, speedUp, extraBreath);

// 		public override void SetStaticDefaults()
// 		{
// 			// DisplayName.SetDefault("Varia Suit V2");
// 			/* Tooltip.SetDefault("+15 defense\n" +
// 				"+30 overheat capacity\n" +
// 				"15% decreased overheat use\n" +
// 				"10% decreased Missile Charge Combo cost\n" +
// 				"10% increased hunter damage\n" +
// 				"7% increased hunter critical strike chance\n" +
// 				"80% increased underwater breathing\n" +
// 				"10% increased movement speed\n" +
// 				"20% increased energy barrier efficiency\n" + // Provisional name
// 				"37.5% increased energy barrier resilience\n" + // Provisional name
// 				"Immunity to fire blocks" + "\n" +
// 				"Immunity to chill and freeze effects"); */
// 			AddonSlot = SuitAddonSlotID.Suit_Barrier;
// 			ItemNameLiteral = false;

// 			//Main.RegisterItemAnimation(Type, new DrawAnimationVertical(3, 10));
// 			//ItemID.Sets.AnimatesAsSoul[Item.type] = true;
// 		}
// 		public override void SetItemDefaults(Item item)
// 		{
// 			item.width = 18;
// 			item.height = 18;
// 			item.value = Item.buyPrice(2, 0, 0, 0);
// 			item.rare = ItemRarityID.Red;
// 			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(3, 10));
// 			ItemID.Sets.AnimatesAsSoul[item.type] = true;
// 		}

// 		public override void OnUpdateArmorSet(Player player, int stack)
// 		{
// 			player.statDefense += suitDef;
// 			player.nightVision = true;
// 			player.fireWalk = true;
// 			player.lavaRose = true;
// 			player.buffImmune[BuffID.OnFire] = true;
// 			player.buffImmune[BuffID.Burning] = true;
// 			player.buffImmune[BuffID.Chilled] = true;
// 			player.buffImmune[BuffID.Frozen] = true;
// 			player.moveSpeed += speedUp / 100;
// 			player.breathEffectiveness += extraBreath / 100;
// 			MPlayer mp = player.GetModPlayer<MPlayer>();
// 			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100;
// 			HunterDamagePlayer.ModPlayer(player).HunterCrit += huntCrit;
// 			mp.tankCapacity += energyCap;
// 			mp.maxOverheat += overheatCap;
// 			mp.overheatCost -= overheatCost / 100;
// 			mp.missileCost -= comboCost / 100;
// 			mp.EnergyDefenseEfficiency += energyEff / 100;
// 			mp.EnergyExpenseEfficiency += energyRes / 100;
// 			mp.canHyper = true;
// 			mp.UACost -= 0.10f;
// 			mp.reserveTanks += 6;
// 		}
// 		public override void AddRecipes()
// 		{
// 			if (MUtils.JoostActive())
// 			{
// 				if (ModContent.TryFind("JoostMod", "IceCoreX", out ModItem saxCore))
// 				{
// 					CreateRecipe()
// 						.AddSuitAddon<VariaSuitV2Addon>()
// 						.AddIngredient(saxCore.Type)
// 						.Register();
// 				}
// 			}
// 			else if (MUtils.CalamityActive())
// 			{
// 				if (ModContent.TryFind("CalamityMod", "EndothermicEnergy", out ModItem endoEnergy))
// 				{
// 					CreateRecipe()
// 						.AddSuitAddon<VariaSuitV2Addon>()
// 						.AddIngredient(endoEnergy.Type, 20)
// 						.AddTile(TileID.LunarCraftingStation)
// 						.Register();
// 				}
// 			}
// 			else if (MUtils.ThoriumActive())
// 			{
// 				if (ModContent.TryFind("ThoriumMod", "OceanEssence", out ModItem oceanEss) &&
// 					ModContent.TryFind("ThoriumMod", "InfernoEssence", out ModItem infernoEss) &&
// 					ModContent.TryFind("ThoriumMod", "DeathEssence", out ModItem deathEss))
// 				{
// 					CreateRecipe()
// 						.AddSuitAddon<VariaSuitV2Addon>()
// 						.AddIngredient(oceanEss.Type, 3)
// 						.AddIngredient(infernoEss.Type, 3)
// 						.AddIngredient(deathEss.Type, 3)
// 						.AddTile(TileID.LunarCraftingStation)
// 						.Register();
// 				}
// 			}
// 		}
// 	}
// }
