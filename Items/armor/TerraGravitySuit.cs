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
			Tooltip.SetDefault("+75 overheat capacity\n" +
			"35% decreased overheat use\n" +
			"30% decreased Missile Charge Combo cost\n" +
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
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 75;
			mp.overheatCost -= 0.35f;
			mp.missileCost -= 0.30f;
			player.noKnockback = true;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("TerraGravitySuitHelmet") && body.type == mod.ItemType("TerraGravitySuitBreastplate") && legs.type == mod.ItemType("TerraGravitySuitGreaves"));
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Allows the ability to Sense Move" + "\r\n" + 
						"Double tap a direction (when enabled)" + "\r\n" + 
						"Immunity to fire blocks" + "\r\n" + 
						"Immunity to chill and freeze effects" + "\r\n" + 
						"Free movement in liquid" + "\r\n" + 
						"Grants 14 seconds of lava immunity" + "\r\n" + 
						"Default gravity in space" + "\r\n" + 
						"Immune to Distorted and Amplified Gravity effects";
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.senseMove = true;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.ignoreWater = true;
			player.lavaMax += 840;
			player.gravity = Player.defaultGravity;
			player.buffImmune[BuffID.VortexDebuff] = true;
			player.buffImmune[mod.BuffType("GravityDebuff")] = true;
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
			DisplayName.SetDefault("Gravity Suit Greaves");
			Tooltip.SetDefault("Allows you to cling to walls\n" +
			"Negates fall damage\n" +
			"20% increased movement speed");
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
			player.spikedBoots += 2;
			player.noFallDmg = true;
			player.moveSpeed += 0.20f;
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
			Tooltip.SetDefault("30% increased hunter damage\n" + 
			"Emits light and grants improved night vision\n" +
			"Infinite breath underwater");
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
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.30f;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.visorGlow = true;
			player.nightVision = true;
			player.gills = true;
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