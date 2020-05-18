using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class PhazonSuitBreastplate : GravitySuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon Suit Breastplate");
			Tooltip.SetDefault("5% increased ranged damage\n" +
			 "Immunity to fire blocks\n" +
			 "Immunity to chill and freeze effects\n" +
			 "Immune to knockback\n" + 
			 "+25 overheat capacity");
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
			player.rangedDamage += 0.05f;
			player.fireWalk = true;
			player.noKnockback = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 25;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("PhazonSuitHelmet") && body.type == mod.ItemType("PhazonSuitBreastplate") && legs.type == mod.ItemType("PhazonSuitGreaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Press the Sense move key while moving near an enemy to dodge in that direction" + "\r\n"
				+ "15% increased ranged damage" + "\r\n"
				+ "Free movement in liquid" + "\r\n"
				+ "Immune to lava damage for 7 seconds" + "\r\n"
				+ "Negates fall damage" + "\r\n"
				+ "Infinite breath" + "\r\n"
				+ "35% decreased overheat use" + "\r\n"
				+ "Immune to damage from standing on Phazon blocks"
				+ "Enables Phazon Beam use";
			p.rangedDamage += 0.15f;
			p.ignoreWater = true;
			p.lavaMax += 420;
			p.noFallDmg = true;
			p.gills = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			mp.overheatCost -= 0.35f;
			mp.SenseMove(p);
			mp.visorGlow = true;
			mp.phazonImmune = true;
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
			recipe.AddIngredient(ItemID.SpectreBar, 25);
			recipe.AddIngredient(null, "PurePhazon", 10);
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
			Tooltip.SetDefault("5% increased ranged damage\n" +
			"20% increased movement speed\n" +
			"+25 overheat capacity\n" +
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
			player.rangedDamage += 0.05f;
			player.moveSpeed += 0.20f;
			player.spikedBoots += 2;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 25;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GravitySuitGreaves");
			recipe.AddIngredient(ItemID.SpectreBar, 20);
			recipe.AddIngredient(null, "PurePhazon", 10);
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
			Tooltip.SetDefault("5% increased ranged damage\n" +
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
			player.rangedDamage += 0.05f;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 25;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GravitySuitHelmet");
			recipe.AddIngredient(ItemID.SpectreBar, 15);
			recipe.AddIngredient(null, "PurePhazon", 10);
			recipe.AddTile(null, "NovaWorkTableTile");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}