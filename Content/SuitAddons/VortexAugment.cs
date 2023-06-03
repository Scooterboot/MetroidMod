using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using MetroidMod.Common.Players;
using MetroidMod.ID;

namespace MetroidMod.Content.SuitAddons
{
	public class VortexAugment : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentGreaves_Legs";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => WorldGen.drunkWorldGen;
		public override double GenerationChance(int x, int y) => 1;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vortex Augment");
			/* Tooltip.SetDefault("+10 defense\n" +
				"+25 overheat capacity\n" +
				"5% decreased overheat use\n" +
				"5% decreased Missile Charge Combo cost\n" +
				"5% increased hunter damage\n" +
				"5% increased hunter critical strike chance\n" +
				"30% increased energy barrier efficiency\n" + // Provisional name
				"20% increased energy barrier resilience\n" // Provisional name
				); */
			ItemID.Sets.ShimmerTransformToItem[Type] = SuitAddonLoader.GetAddon<NebulaAugment>().ItemType;
			AddonSlot = SuitAddonSlotID.Suit_LunarAugment;
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(0, 15, 60, 0);
			item.rare = ItemRarityID.Red;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.statDefense += 10;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.05f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 5;
			mp.maxOverheat += 25;
			mp.overheatCost -= 0.05f;
			mp.missileCost -= 0.05f;
			mp.EnergyDefenseEfficiency += 0.30f;
			mp.EnergyExpenseEfficiency += 0.20f;
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(67, 255, 255);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.LunarBar, 36)
				.AddIngredient(ItemID.FragmentVortex, 45)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
