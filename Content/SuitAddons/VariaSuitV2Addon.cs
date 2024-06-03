using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Common.Systems;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;

namespace MetroidMod.Content.SuitAddons
{
	public class VariaSuitV2Addon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Item";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Tile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Helmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Breastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Breastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Greaves_Legs";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || MSystem.bossesDown.HasFlag(MetroidBossDown.downedKraid);

		public override double GenerationChance(int x, int y) => 4;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Varia Suit V2");
			/* Tooltip.SetDefault("+15 defense\n" +
				"+30 overheat capacity\n" +
				"15% decreased overheat use\n" +
				"10% decreased Missile Charge Combo cost\n" +
				"10% increased hunter damage\n" +
				"7% increased hunter critical strike chance\n" +
				"80% increased underwater breathing\n" +
				"10% increased movement speed\n" +
				"20% increased energy barrier efficiency\n" + // Provisional name
				"37.5% increased energy barrier resilience\n" + // Provisional name
				"Immunity to fire blocks" + "\n" +
				"Immunity to chill and freeze effects"); */
			AddonSlot = SuitAddonSlotID.Suit_Barrier;
			ItemNameLiteral = false;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(0, 2, 10, 0);
			item.rare = ItemRarityID.Orange;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			player.statDefense += 15;
			player.nightVision = true;
			player.fireWalk = true;
			player.lavaRose = true;
			player.buffImmune[BuffID.OnFire] = true;
			player.buffImmune[BuffID.Burning] = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.moveSpeed += 0.10f;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.10f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 7;
			mp.tankCapacity += 4;
			mp.maxOverheat += 30;
			mp.overheatCost -= 0.15f;
			mp.missileCost -= 0.10f;
			mp.breathMult = 1.8f;
			mp.EnergyDefenseEfficiency += 0.2f;
			mp.EnergyExpenseEfficiency += 0.375f;
			mp.canHyper = true;
			mp.UACost -= 0.10f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddSuitAddon<VariaSuitAddon>(1)
				.AddRecipeGroup(MUtils.CalamityActive() ? MetroidMod.T1HMBarRecipeGroupID : MetroidMod.T3HMBarRecipeGroupID, 5)
				.AddIngredient<Items.Miscellaneous.KraidTissue>(30)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
