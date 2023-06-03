using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using MetroidMod.Common.Players;
using MetroidMod.ID;
using MetroidMod.Content;
using MetroidMod.Content.Items.Weapons;
using Terraria.ModLoader;

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

		public override bool CanGenerateOnChozoStatue(int x, int y) => WorldGen.drunkWorldGen;

		public override double GenerationChance(int x, int y) => 4;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phazon Suit");
			/* Tooltip.SetDefault("+19 defense\n" +
				"+30 overheat capacity\n" +
				"10% decreased overheat use\n" +
				"10% decreased Missile Charge Combo cost\n" +
				"10% increased hunter damage\n" +
				"8% increased hunter critical strike chance\n" +
				"10% increased movement speed\n" +
				"30% increased energy barrier efficiency\n" + // Provisional name
				"17.5% increased energy barrier resilience\n" + // Provisional name
				"Infinite breath underwater\n" +
				"Immune to knockback\n" +
				"Free movement in liquid\n" +
				"Grants 7 seconds of lava immunity\n" +
				"Immune to damage caused by blue Phazon blocks\n" +
				"Enables Phazon Beam use"); */
			ItemID.Sets.ShimmerTransformToItem[Type] = SuitAddonLoader.GetAddon<TerraGravitySuitAddon>().ItemType;
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
			player.statDefense += 19;
			player.noKnockback = true;
			player.ignoreWater = true;
			player.gills = true;
			player.lavaMax += 420; // blaze it
			player.moveSpeed += 0.10f;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.10f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 8;
			mp.maxOverheat += 30;
			mp.overheatCost -= 0.10f;
			mp.missileCost -= 0.10f;
			mp.EnergyDefenseEfficiency += 0.30f;
			mp.EnergyExpenseEfficiency += 0.175f;
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
				.AddSuitAddon<GravitySuitAddon>(1)
				.AddTile<Tiles.NovaWorkTableTile>()
				.Register();
		}
	}
}
