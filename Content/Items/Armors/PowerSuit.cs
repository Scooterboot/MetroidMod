using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.Items.Armors
{
	[AutoloadEquip(EquipType.Body)]
	public class PowerSuitBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Suit Breastplate");
			Tooltip.SetDefault("+15 overheat capacity\n" +
			"10% decreased overheat use");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = 9000;
			Item.defense = 6;
		}
		public override void UpdateEquip(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 15;
			mp.overheatCost -= 0.10f;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<PowerSuitHelmet>() && body.type == ModContent.ItemType<PowerSuitBreastplate>() && legs.type == ModContent.ItemType<PowerSuitGreaves>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Allows the ability to Sense Move" + "\n" +
							"Double tap a direction (when enabled)";// + 
							//SuitAddonLoader.GetSetBonusText(player);
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.senseMove = true;
			mp.ShouldShowArmorUI = true;
			SuitAddonLoader.OnUpdateArmorSet(player);
		}
		public override void ArmorSetShadows(Player player)
		{
			SuitAddonLoader.ArmorSetShadows(player);
		}
		public override void UpdateVanitySet(Player P)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.isPowerSuit = true;
			mp.visorGlowColor = new Color(0, 248, 112);
			if(P.velocity.Y != 0f && ((P.controlRight && P.direction == 1) || (P.controlLeft && P.direction == -1) || mp.SMoveEffect > 0) && mp.shineDirection == 0 && !mp.shineActive && !mp.ballstate)
			{
				mp.jet = true;
			}
			else if(mp.shineDirection == 0 || mp.shineDirection == 5)
			{
				mp.jet = false;
			}
			SuitAddonLoader.OnUpdateVanitySet(P);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ChoziteBreastplate>(1)
				.AddIngredient<Tiles.EnergyTank>(1)
				.AddRecipeGroup(MetroidModPorted.EvilBarRecipeGroupID, 20)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class PowerSuitGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Suit Greaves");
			Tooltip.SetDefault("Allows somersaulting & wall jumping\n" +
			"Negates fall damage");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = 6000;
			Item.defense = 5;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<MPlayer>().EnableWallJump = true;
			player.noFallDmg = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ChoziteGreaves>(1)
				.AddIngredient<Tiles.EnergyTank>(1)
				.AddRecipeGroup(MetroidModPorted.EvilBarRecipeGroupID, 15)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class PowerSuitHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Suit Helmet");
			Tooltip.SetDefault("10% increased hunter damage\n" +
			"Emits light and grants improved night vision\n" +
			"30% increased underwater breathing");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = 6000;
			Item.defense = 5;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.10f;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.breathMult = 1.3f;
			mp.visorGlow = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ChoziteHelmet>(1)
				.AddIngredient<Tiles.EnergyTank>(1)
				.AddRecipeGroup(MetroidModPorted.EvilBarRecipeGroupID, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
