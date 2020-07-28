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
			Tooltip.SetDefault("+100 overheat capacity\n" +
			"40% decreased overheat use\n" +
			"40% decreased Missile Charge Combo cost\n" +
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
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 100;
			mp.overheatCost -= 0.40f;
			mp.missileCost -= 0.40f;
			player.noKnockback = true;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("VortexGravitySuitHelmet") && body.type == mod.ItemType("VortexGravitySuitBreastplate") && legs.type == mod.ItemType("VortexGravitySuitGreaves"));
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Allows the ability to Sense Move" + "\r\n" + 
						"Double tap a direction (when enabled)" + "\r\n" + 
						"Immunity to fire blocks" + "\r\n" + 
						"Immunity to chill and freeze effects" + "\r\n" + 
						"Free movement in liquid" + "\r\n" + 
						"Grants indefinite lava immunity" + "\r\n" + 
						"Default gravity in space" + "\r\n" + 
						"Immune to Distorted and Amplified Gravity effects";
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.senseMove = true;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.ignoreWater = true;
			player.lavaImmune = true;
			player.gravity = Player.defaultGravity;
			player.buffImmune[BuffID.VortexDebuff] = true;
			player.buffImmune[mod.BuffType("GravityDebuff")] = true;
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
			Tooltip.SetDefault("Allows you to cling to walls\n" +
			"Negates fall damage\n" +
			"20% increased movement speed");
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
			player.spikedBoots += 2;
			player.noFallDmg = true;
			player.moveSpeed += 0.20f;
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
			Tooltip.SetDefault("35% increased hunter damage\n" + 
			"Emits light and grants improved night vision\n" +
			"Infinite breath underwater");
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
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.35f;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.visorGlow = true;
			player.nightVision = true;
			player.gills = true;
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