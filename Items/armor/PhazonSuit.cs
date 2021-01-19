using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class PhazonSuitBreastplate : GravitySuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon Suit Breastplate");
			Tooltip.SetDefault("+75 overheat capacity\n" +
			"35% decreased overheat use\n" +
			"20% decreased Missile Charge Combo cost\n" +
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
			mp.missileCost -= 0.20f;
			player.noKnockback = true;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("PhazonSuitHelmet") && body.type == mod.ItemType("PhazonSuitBreastplate") && legs.type == mod.ItemType("PhazonSuitGreaves"));
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Allows the ability to Sense Move" + "\r\n" + 
						"Double tap a direction (when enabled)" + "\r\n" + 
						"Immunity to fire blocks" + "\r\n" + 
						"Immunity to chill and freeze effects" + "\r\n" + 
						"Free movement in liquid" + "\r\n" + 
						"Grants 7 seconds of lava immunity" + "\r\n" + 
						"Immune to damage caused by blue Phazon blocks" + "\r\n" + 
						"Enables Phazon Beam use";
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.senseMove = true;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.ignoreWater = true;
			player.lavaMax += 420; // blaze it
			mp.phazonImmune = true;
			mp.canUsePhazonBeam = true;
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 64, 0);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GravitySuitBreastplate");
			recipe.AddIngredient(null, "PhazonBar", 25);
			recipe.AddIngredient(null, "PurePhazon", 20);
			recipe.AddTile(null, "NovaWorkTableTile");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class PhazonSuitGreaves : GravitySuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon Suit Greaves");
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
			recipe.AddIngredient(null, "PhazonBar", 20);
			recipe.AddIngredient(null, "PurePhazon", 15);
			recipe.AddTile(null, "NovaWorkTableTile");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class PhazonSuitHelmet : GravitySuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon Suit Helmet");
			Tooltip.SetDefault("30% increased hunter damage\n" + 
			"15% increased hunter critical strike chance\n" + 
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
			HunterDamagePlayer.ModPlayer(player).hunterCrit += 15;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.visorGlow = true;
			player.nightVision = true;
			player.gills = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GravitySuitHelmet");
			recipe.AddIngredient(null, "PhazonBar", 15);
			recipe.AddIngredient(null, "PurePhazon", 10);
			recipe.AddTile(null, "NovaWorkTableTile");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}