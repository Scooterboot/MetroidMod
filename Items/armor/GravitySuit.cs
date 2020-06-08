using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class GravitySuitBreastplate : VariaSuitV2Breastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Suit Breastplate");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+20 overheat capacity\n" +
			"Immune to fire blocks\n" +
			"Immune to chill and freeze effects\n" +
			"Immune to knockback");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 5;
			item.value = 30000;
			item.defense = 15;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			player.fireWalk = true;
			player.noKnockback = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 20;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("GravitySuitHelmet") && body.type == mod.ItemType("GravitySuitBreastplate") && legs.type == mod.ItemType("GravitySuitGreaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Hold the Sense move key and left/right while an enemy is moving towards you to dodge" + "\r\n" + 
						"10% increased hunter damage" + "\r\n" + 
						"30% decreased overheat use" + "\r\n" + 
						"20% decreased Missile Charge Combo cost" + "\r\n" + 
						"Free movement in liquid" + "\r\n" + 
						"Grants 7 seconds of lava immunity" + "\r\n" + 
						"Infinite breath" + "\r\n" + 
						"Negates fall damage";
			HunterDamagePlayer.ModPlayer(p).hunterDamageMult += 0.10f;
			//p.rangedDamage += 0.10f;
			p.ignoreWater = true;
			p.lavaMax += 420;
			p.noFallDmg = true;
			p.gills = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			mp.overheatCost -= 0.3f;
			mp.missileCost -= 0.2f;
			mp.SenseMove(p);
			mp.visorGlow = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitV2Breastplate");
			recipe.AddIngredient(ItemID.HallowedBar, 24);
			recipe.AddIngredient(null, "GravityGel", 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class GravitySuitGreaves : VariaSuitV2Greaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Suit Greaves");
			Tooltip.SetDefault("5% increased hunter damage\n" + 
			"+20 overheat capacity\n" + 
			"10% increased movement speed\n" + 
			"Allows you to cling to walls");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 5;
			item.value = 24000;
			item.defense = 13;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			player.moveSpeed += 0.10f;
			player.spikedBoots += 2;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 20;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitV2Greaves");
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddIngredient(null, "GravityGel", 17);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class GravitySuitHelmet : VariaSuitV2Helmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Suit Helmet");
			Tooltip.SetDefault("5% increased hunter damage\n" + 
			"+20 overheat capacity\n" + 
			"Improved night vision");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 5;
			item.value = 24000;
			item.defense = 12;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 20;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitV2Helmet");
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddIngredient(null, "GravityGel", 17);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}