using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class NebulaPhazonSuitBreastplate : PhazonSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Phazon Suit Breastplate");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+34 overheat capacity\n" +
			"Immunity to fire blocks\n" +
			"Immunity to chill and freeze effects\n" +
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
			return (head.type == mod.ItemType("NebulaPhazonSuitHelmet") && body.type == mod.ItemType("NebulaPhazonSuitBreastplate") && legs.type == mod.ItemType("NebulaPhazonSuitGreaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Press the Sense move key while moving near an enemy to dodge in that direction" + "\r\n" + 
						"20% increased hunter damage" + "\r\n" + 
						"40% decreased overheat use" + "\r\n" + 
						"Free movement in liquid" + "\r\n" + 
						"Grants 7 seconds of lava immunity" + "\r\n" + 
						"Infinite breath" + "\r\n" + 
						"Negates fall damage" + "\r\n" + 
						"Immune to damage caused by blue Phazon blocks" + 
						"Enables Phazon Beam use";
			HunterDamagePlayer.ModPlayer(p).hunterDamageMult += 0.20f;
			//p.rangedDamage += 0.20f;
			p.ignoreWater = true;
			p.lavaMax += 420;
			p.noFallDmg = true;
			p.gills = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			mp.overheatCost -= 0.40f;
			mp.SenseMove(p);
			mp.visorGlow = true;
			mp.phazonImmune = true;
			mp.phazonRegen = 4;
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 55, 255);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
			player.armorEffectDrawOutlines = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PhazonSuitBreastplate");
			recipe.AddIngredient(ItemID.LunarBar, 16);
			recipe.AddIngredient(ItemID.FragmentNebula, 20);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class NebulaPhazonSuitGreaves : PhazonSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Phazon Suit Greaves");
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
			recipe.AddIngredient(null, "PhazonSuitGreaves");
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ItemID.FragmentNebula, 15);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class NebulaPhazonSuitHelmet : PhazonSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Phazon Suit Helmet");
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
			recipe.AddIngredient(null, "PhazonSuitHelmet");
			recipe.AddIngredient(ItemID.LunarBar, 8);
			recipe.AddIngredient(ItemID.FragmentNebula, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}