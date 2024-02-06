using MetroidMod.Common.Players;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

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

		public override bool CanGenerateOnChozoStatue(int x, int y) => ((WorldGen.drunkWorldGen || WorldGen.everythingWorldGen) && Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues) || NPC.downedMoonlord;
		public override double GenerationChance(int x, int y) => 1;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vortex Augment");
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
				"Grants 14 seconds of lava immunity\n" +
				"Default gravity in space\n" +
				"Immune to Distorted and Amplified Gravity effects"); */
			ItemID.Sets.ShimmerTransformToItem[ItemType] = SuitAddonLoader.GetAddon<NebulaAugment>().ItemType;
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
			player.moveSpeed += 0.10f;
			player.lavaMax += 840;
			player.gravity = Player.defaultGravity;
			player.buffImmune[BuffID.VortexDebuff] = true;
			player.buffImmune[Terraria.ModLoader.ModContent.BuffType<Buffs.GravityDebuff>()] = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.15f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 13;
			mp.maxOverheat += 55;
			mp.overheatCost -= 0.15f;
			mp.missileCost -= 0.15f;
			mp.EnergyDefenseEfficiency += 0.60f;
			mp.EnergyExpenseEfficiency += 0.375f;
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
				.AddSuitAddon<TerraGravitySuitAddon>(1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
