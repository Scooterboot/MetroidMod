using MetroidMod.Common.Players;
using MetroidMod.ID;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.SuitAddons
{
	public class FusionOmegaAddon : ModSuitUpgrade
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/FusionOmega/FusionOmegaItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/FusionOmega/FusionOmegaTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/FusionOmega/FusionOmegaSuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/FusionOmega/FusionOmegaSuitBreastplate_Body";

		//public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/FusionOmega/FusionOmegaSuitBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/FusionOmega/FusionOmegaSuitGreaves_Legs";

		//public override bool CanGenerateOnChozoStatue() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || MSystem.bossesDown.HasFlag(MetroidBossDown.downedKraid);

		//public override double GenerationChance() => 4;

		//This is where all of the suit addon's stats are stored.
		//They're outside a method so it can be directly accessed by the localization.
		//Put in the numbers like they'd be seen on the tooltip. The values are automatically adjusted for the actual stats.
		public static int suitDef = 35; //Added suit defense
		public static int energyCap = 4; //Added E-tank capacity
		public static float energyEff = 40f; //%Increased energy damage absorption
		public static float energyRes = 37.5f; //%Increased energy DR
		public static int overheatCap = 80; //Added maximum overheat
		public static float overheatCost = 20f; //%Decreased overheat cost
		public static float comboCost = 25f; //%Decreased Charge Combo cost
		public static float huntDamage = 30f; //%Increased hunter damage
		public static int huntCrit = 15; //Increased hunter crit
		public static float speedUp = 10f; //%Increased movement speed
		public static float extraBreath = 200f; //%Increased breath meter

		public override LocalizedText ItemTooltip => base.ItemTooltip.WithFormatArgs(suitDef, energyCap, energyEff, energyRes, overheatCap, overheatCost, comboCost, huntDamage, huntCrit, speedUp, extraBreath);

		public override BreastplateAddonSlot AddonSlot => BreastplateAddonSlot.Barrier;

		public override void ItemSetDefaults()
		{
            base.ItemSetDefaults();
            
			GeneratedModItem.Item.width = 18;
			GeneratedModItem.Item.height = 18;
			GeneratedModItem.Item.value = Item.buyPrice(2, 0, 0, 0);
			GeneratedModItem.Item.rare = ItemRarityID.Red;
			Main.RegisterItemAnimation(ItemType, new DrawAnimationVertical(3, 10));
			ItemID.Sets.AnimatesAsSoul[ItemType] = true;
		}

		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.statDefense += suitDef;
			player.nightVision = true;
			player.fireWalk = true;
			player.lavaRose = true;
			player.buffImmune[BuffID.OnFire] = true;
			player.buffImmune[BuffID.Burning] = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.moveSpeed += speedUp / 100;
			player.breathEffectiveness += extraBreath / 100;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += huntCrit;
			mp.tankCapacity += energyCap;
			mp.maxOverheat += overheatCap;
			mp.overheatCost -= overheatCost / 100;
			mp.missileCost -= comboCost / 100;
			mp.EnergyDefenseEfficiency += energyEff / 100;
			mp.EnergyExpenseEfficiency += energyRes / 100;
			mp.canHyper = true;
			mp.UACost -= 0.10f;
			mp.reserveTanks += 6;
		}
		public override void ItemAddRecipes()
		{
			if (MUtils.JoostActive())
			{
				if (ModContent.TryFind("JoostMod", "IceCoreX", out ModItem saxCore))
				{
					GeneratedModItem.CreateRecipe()
						.AddSuitAddon<VariaSuitV2Addon>()
						.AddIngredient(saxCore.Type)
						.Register();
				}
			}
			else if (MUtils.CalamityActive())
			{
				if (ModContent.TryFind("CalamityMod", "EndothermicEnergy", out ModItem endoEnergy))
				{
					GeneratedModItem.CreateRecipe()
						.AddSuitAddon<VariaSuitV2Addon>()
						.AddIngredient(endoEnergy.Type, 20)
						.AddTile(TileID.LunarCraftingStation)
						.Register();
				}
			}
			else if (MUtils.ThoriumActive())
			{
				if (ModContent.TryFind("ThoriumMod", "OceanEssence", out ModItem oceanEss) &&
					ModContent.TryFind("ThoriumMod", "InfernoEssence", out ModItem infernoEss) &&
					ModContent.TryFind("ThoriumMod", "DeathEssence", out ModItem deathEss))
				{
					GeneratedModItem.CreateRecipe()
						.AddSuitAddon<VariaSuitV2Addon>()
						.AddIngredient(oceanEss.Type, 3)
						.AddIngredient(infernoEss.Type, 3)
						.AddIngredient(deathEss.Type, 3)
						.AddTile(TileID.LunarCraftingStation)
						.Register();
				}
			}
		}
	}
}
