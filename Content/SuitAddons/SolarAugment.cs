using MetroidMod.Common.Players;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace MetroidMod.Content.SuitAddons
{
	public class SolarAugment : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentGreaves_Legs";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => false;//WorldGen.drunkWorldGen && Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues;

		public override double GenerationChance(int x, int y) => 4;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Solar Augment");
			/* Tooltip.SetDefault("You shouldn't have this."/*"+29 defense\n" +
				"+55 overheat capacity\n" +
				"15% decreased overheat use\n" +
				"15% decreased Missile Charge Combo cost\n" +
				"15% increased hunter damage\n" +
				"13% increased hunter critical strike chance\n" +
				"60% increased energy barrier efficiency\n" + // Provisional name
				"32.5% increased energy barrier resilience\n" + // Provisional name
				"Immune to damage from the Dark World\n" +
				"Immune to damage from Dark Water"
				); */
			AddonSlot = SuitAddonSlotID.Suit_Primary;
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
			mp.EnergyDefenseEfficiency += 0.60f;
			mp.EnergyExpenseEfficiency += 0.325f;
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 254, 204);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
			player.armorEffectDrawOutlines = true;
		}
		/* Commented out because Dark World doesn't exist (yet)
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.LunarBar, 36)
				.AddIngredient(ItemID.FragmentSolar, 45)
				.AddSuitAddon<LightSuitAddon>(1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
		*/
	}
}
