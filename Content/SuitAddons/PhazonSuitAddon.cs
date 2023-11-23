using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using MetroidMod.Common.Players;
using MetroidMod.ID;
using MetroidMod.Content;
using MetroidMod.Content.Items.Weapons;
using Terraria.ModLoader;
using MetroidMod.Common.Systems;

namespace MetroidMod.Content.SuitAddons
{
	public class PhazonSuitAddon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitGreaves_Legs";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => ((WorldGen.drunkWorldGen || WorldGen.everythingWorldGen) && Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues) || MSystem.bossesDown.HasFlag(MetroidBossDown.downedOmegaPirate);

		public override double GenerationChance(int x, int y) => 4;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phazon Suit");
			/* Tooltip.SetDefault("+10 defense\n" +
				"+15 overheat capacity\n" +
				"5% decreased overheat use\n" +
				"5% decreased Missile Charge Combo cost\n" +
				"5% increased hunter damage\n" +
				"5% increased hunter critical strike chance\n" +
				"10% increased movement speed\n" +
				"15% increased energy barrier efficiency\n" + // Provisional name
				"10% increased energy barrier resilience\n" + // Provisional name
				"Immune to damage caused by blue Phazon blocks\n" +
				"Enables Phazon Beam use"); */
			ItemID.Sets.ShimmerTransformToItem[ItemType] = SuitAddonLoader.GetAddon<TerraGravitySuitAddon>().ItemType;
			AddonSlot = SuitAddonSlotID.Suit_Augment;
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
			player.statDefense += 10;
			player.moveSpeed += 0.10f;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.05f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 5;
			mp.maxOverheat += 15;
			mp.overheatCost -= 0.05f;
			mp.missileCost -= 0.05f;
			mp.EnergyDefenseEfficiency += 0.15f;
			mp.EnergyExpenseEfficiency += 0.10f;
			mp.phazonImmune = true;
			mp.canUsePhazonBeam = true;
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 64, 0);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.PhazonBar>(60)
				.AddIngredient<Items.Miscellaneous.PurePhazon>(45)
				.AddTile<Tiles.NovaWorkTableTile>()
				.Register();
		}
	}
}
