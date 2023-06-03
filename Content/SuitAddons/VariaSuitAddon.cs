using Terraria;
using Terraria.ID;
using MetroidMod.Common.Players;
using MetroidMod.ID;

namespace MetroidMod.Content.SuitAddons
{
	public class VariaSuitAddon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitGreaves_Legs";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => y >= Main.UnderworldLayer;

		public override double GenerationChance(int x, int y) => 20;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Varia Suit");
			/* Tooltip.SetDefault("+6 defense\n" +
				"+15 overheat capacity\n" +
				"5% decreased overheat use\n" +
				"5% decreased Missile Charge Combo cost\n" +
				"5% increased hunter damage\n" +
				"5% increased hunter critical strike chance\n" +
				"55% increased underwater breathing\n" +
				"10% increased movement speed\n" +
				"10% increased energy barrier efficiency\n" + // Provisional name
				"20% increased energy barrier resilience\n" + // Provisional name
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
			player.statDefense += 6;
			player.nightVision = true;
			player.fireWalk = true;
			player.lavaRose = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.moveSpeed += 0.10f;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.05f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 5;
			mp.maxOverheat += 15;
			mp.overheatCost -= 0.10f;
			mp.missileCost -= 0.05f;
			mp.breathMult = 1.55f;
			mp.EnergyDefenseEfficiency += 0.1f;
			mp.EnergyExpenseEfficiency += 0.2f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.HellstoneBar, 45)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
