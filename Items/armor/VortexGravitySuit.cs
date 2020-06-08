using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class VortexGravitySuitBreastplate : TerraGravitySuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Gravity Suit Breastplate");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+34 overheat capacity\n" +
			"Immune to fire blocks\n" +
			"Immune to chill and freeze effects\n" +
			"Immune to knockback");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 10;
			item.value = 60000;
			item.defense = 22;
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
			mp.maxOverheat += 34;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("VortexGravitySuitHelmet") && body.type == mod.ItemType("VortexGravitySuitBreastplate") && legs.type == mod.ItemType("VortexGravitySuitGreaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Hold the Sense move key and left/right while an enemy is moving towards you to dodge" + "\r\n" + 
						"20% increased hunter damage" + "\r\n" + 
						"40% decreased overheat use" + "\r\n" + 
						"40% decreased Missile Charge Combo cost" + "\r\n" + 
						"Free movement in liquid" + "\r\n" + 
						"Grants indefinite lava immunity" + "\r\n" + 
						"Infinite breath" + "\r\n" + 
						"Negates fall damage" + "\r\n" + 
						"Default gravity in space" + "\r\n" + 
						"Immune to Distorted and Amplified Gravity effects";
			HunterDamagePlayer.ModPlayer(p).hunterDamageMult += 0.20f;
			//p.rangedDamage += 0.20f;
			p.ignoreWater = true;
			p.gravity = Player.defaultGravity;
			p.lavaImmune = true;
			p.noFallDmg = true;
			p.gills = true;
			p.buffImmune[BuffID.VortexDebuff] = true;
			p.buffImmune[mod.BuffType("GravityDebuff")] = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			mp.overheatCost -= 0.40f;
			mp.missileCost -= 0.4f;
			mp.SenseMove(p);
			mp.visorGlow = true;
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(67, 255, 255);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TerraGravitySuitBreastplate");
			recipe.AddIngredient(ItemID.LunarBar, 16);
			recipe.AddIngredient(ItemID.FragmentVortex, 20);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VortexGravitySuitGreaves : TerraGravitySuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Gravity Suit Greaves");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+33 overheat capacity\n" +
			"20% increased movement speed\n" +
			"Allows you to cling to walls");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 10;
			item.value = 48000;
			item.defense = 20;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			player.moveSpeed += 0.20f;
			player.spikedBoots += 2;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 33;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TerraGravitySuitGreaves");
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ItemID.FragmentVortex, 15);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VortexGravitySuitHelmet : TerraGravitySuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Gravity Suit Helmet");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+33 overheat capacity\n" +
			"Improved night vision");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 10;
			item.value = 48000;
			item.defense = 18;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 33;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TerraGravitySuitHelmet");
			recipe.AddIngredient(ItemID.LunarBar, 8);
			recipe.AddIngredient(ItemID.FragmentVortex, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}