using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class TerraGravitySuitBreastplate : GravitySuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Gravity Suit Breastplate");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+25 overheat capacity\n" +
			"Immune to fire blocks\n" +
			"Immune to chill and freeze effects\n" +
			"Immune to knockback");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 7;
			item.value = 45000;
			item.defense = 18;
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
			mp.maxOverheat += 25;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("TerraGravitySuitHelmet") && body.type == mod.ItemType("TerraGravitySuitBreastplate") && legs.type == mod.ItemType("TerraGravitySuitGreaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Hold the Sense move key and left/right while an enemy is moving towards you to dodge" + "\r\n" + 
						"15% increased hunter damage" + "\r\n" + 
						"35% decreased overheat use" + "\r\n" + 
						"Free movement in liquid" + "\r\n" + 
						"Grants 14 seconds of lava immunity" + "\r\n" + 
						"Infinite breath" + "\r\n" + 
						"Negates fall damage" + "\r\n" + 
						"Default gravity in space" + "\r\n" + 
						"Immune to Distorted and Amplified Gravity effects";
			HunterDamagePlayer.ModPlayer(p).hunterDamageMult += 0.15f;
			//p.rangedDamage += 0.15f;
			p.ignoreWater = true;
			p.gravity = Player.defaultGravity;
			p.lavaMax += 840;
			p.noFallDmg = true;
			p.gills = true;
			p.buffImmune[BuffID.VortexDebuff] = true;
			p.buffImmune[mod.BuffType("GravityDebuff")] = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			mp.overheatCost -= 0.35f;
			mp.SenseMove(p);
			mp.visorGlow = true;
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(138, 255, 252);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GravitySuitBreastplate");
			recipe.AddIngredient(ItemID.ChlorophyteBar, 25);
			recipe.AddIngredient(null, "NightmareCoreXFragment", 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class TerraGravitySuitGreaves : GravitySuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Gravity Suit Greaves");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+25 overheat capacity\n" +
			"20% increased movement speed\n" +
			"Allows you to cling to walls");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 7;
			item.value = 36000;
			item.defense = 17;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			player.moveSpeed += 0.20f;
			player.spikedBoots += 2;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 25;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GravitySuitGreaves");
			recipe.AddIngredient(ItemID.ChlorophyteBar, 20);
			recipe.AddIngredient(null, "NightmareCoreXFragment", 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class TerraGravitySuitHelmet : GravitySuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Gravity Suit Helmet");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+25 overheat capacity\n" +
			"Improved night vision");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 7;
			item.value = 36000;
			item.defense = 15;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 25;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GravitySuitHelmet");
			recipe.AddIngredient(ItemID.ChlorophyteBar, 15);
			recipe.AddIngredient(null, "NightmareCoreXFragment", 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}