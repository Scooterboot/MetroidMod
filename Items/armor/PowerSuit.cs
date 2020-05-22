using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class PowerSuitBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Suit Breastplate");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+5 overheat capacity");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 2;
			item.value = 9000;
			item.defense = 6;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 5;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("PowerSuitHelmet") && body.type == mod.ItemType("PowerSuitBreastplate") && legs.type == mod.ItemType("PowerSuitGreaves"));
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Hold the Sense move key and left/right while an enemy is moving towards you to dodge" + "\r\n" + 
							"10% decreased overheat use" + "\r\n" + 
							"30% increased underwater breathing" + "\r\n" + 
							"Negates fall damage";
			player.noFallDmg = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.breathMult = 1.3f;
			mp.overheatCost -= 0.10f;
			mp.SenseMove(player);
			mp.visorGlow = true;
		}
		public override void UpdateVanitySet(Player P)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.isPowerSuit = true;
			mp.visorGlowColor = new Color(0, 248, 112);
			if(P.velocity.Y != 0f && ((P.controlRight && P.direction == 1) || (P.controlLeft && P.direction == -1)) && mp.shineDirection == 0 && !mp.shineActive && !mp.ballstate)
			{
				mp.jet = true;
			}
			else if(mp.shineDirection == 0 || mp.shineDirection == 5)
			{
				mp.jet = false;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBreastplate");
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddIngredient(ItemID.DemoniteBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBreastplate");
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddIngredient(ItemID.CrimtaneBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class PowerSuitGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Suit Greaves");
			Tooltip.SetDefault("5% increased hunter damage\n" + 
			"+5 overheat capacity\n" +
			"Allows you to slide down walls");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 2;
			item.value = 6000;
			item.defense = 5;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			player.spikedBoots += 1;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteGreaves");
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddIngredient(ItemID.DemoniteBar, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteGreaves");
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddIngredient(ItemID.CrimtaneBar, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class PowerSuitHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Suit Helmet");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+5 overheat capacity\n" +
			"Improved night vision");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 2;
			item.value = 6000;
			item.defense = 5;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteHelmet");
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddIngredient(ItemID.DemoniteBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteHelmet");
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddIngredient(ItemID.CrimtaneBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}