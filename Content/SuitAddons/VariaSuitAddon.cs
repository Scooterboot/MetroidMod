﻿using Terraria;
using Terraria.ID;
using MetroidModPorted.Common.Players;
using MetroidModPorted.ID;

namespace MetroidModPorted.Content.SuitAddons
{
	public class VariaSuitAddon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitTile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitHelmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitBreastplate_Body";

		public override string ArmorTextureArmsGlow => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitBreastplate_Arms_Glow";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitGreaves_Legs";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit");
			Tooltip.SetDefault("+6 defense\n" +
				"+15 overheat capacity\n" +
				"5% decreased overheat use\n" +
				"5% decreased Missile Charge Combo cost\n" +
				"5% increased hunter damage\n" +
				"5% increased hunter critical strike chance\n" +
				"55% increased underwater breathing\n" +
				"10% increased movement speed\n" +
				"Immunity to fire blocks" + "\n" + 
				"Immunity to chill and freeze effects");
			AddonSlot = SuitAddonSlotID.Suit_Varia;
			ItemNameLiteral = false;
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = Terraria.Item.buyPrice(0, 2, 10, 0);
			item.rare = ItemRarityID.Orange;
		}
		public override void OnUpdateArmorSet(Player player)
		{
			player.statDefense += 6;
			player.nightVision = true;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.moveSpeed += 0.10f;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.05f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 5;
			mp.maxOverheat += 15;
			mp.overheatCost -= 0.10f;
			mp.missileCost -= 0.05f;
			mp.visorGlow = true;
			mp.breathMult = 1.55f;
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