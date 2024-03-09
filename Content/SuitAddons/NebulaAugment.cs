using MetroidMod.Common.Players;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace MetroidMod.Content.SuitAddons
{
	public class NebulaAugment : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/NebulaAugment/NebulaAugmentItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/NebulaAugment/NebulaAugmentTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/NebulaAugment/NebulaAugmentHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/NebulaAugment/NebulaAugmentBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/NebulaAugment/NebulaAugmentBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/NebulaAugment/NebulaAugmentGreaves_Legs";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || NPC.downedMoonlord;

		public override double GenerationChance(int x, int y) => 1;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Augment");
			/* Tooltip.SetDefault("+29 defense\n" +
				"+55 overheat capacity\n" +
				"15% decreased overheat use\n" +
				"15% decreased Missile Charge Combo cost\n" +
				"15% increased hunter damage\n" +
				"13% increased hunter critical strike chance\n" +
				"10% increased movement speed\n" +
				"60% increased energy barrier efficiency\n" + // Provisional name
				"37.5% increased energy barrier resilience\n" + // Provisional name
				"Infinite breath underwater\n" +
				"Immune to knockback\n" +
				"Free movement in liquid\n" +
				"Grants 7 seconds of lava immunity\n" +
				"Immune to damage caused by blue Phazon blocks\n" +
				"Enables Phazon Beam use");*/
			ItemID.Sets.ShimmerTransformToItem[ItemType] = SuitAddonLoader.GetAddon<VortexAugment>().ItemType;
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
			player.statDefense += 29;
			player.noKnockback = true;
			player.ignoreWater = true;
			if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
			{
				player.gills = true;
			}
			player.lavaMax += 420; // blaze it
			player.moveSpeed += 0.10f;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.15f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 13;
			mp.maxOverheat += 55;
			mp.overheatCost -= 0.15f;
			mp.missileCost -= 0.15f;
			mp.EnergyDefenseEfficiency += 0.60f;
			mp.EnergyExpenseEfficiency += 0.375f;
			mp.phazonImmune = true;
			mp.canUsePhazonBeam = true;
		}
		public override void OnUpdateVanitySet(Player player)
		{
			player.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 55, 255);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
			player.armorEffectDrawOutlines = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.LunarBar, 36)
				.AddIngredient(ItemID.FragmentNebula, 45)
				.AddSuitAddon<PhazonSuitAddon>(1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
