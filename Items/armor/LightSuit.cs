using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class LightSuitBreastplate : DarkSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Suit Breastplate");
			/*Tooltip.SetDefault("5% increased ranged damage\n" +
			 "Immune to fire blocks\n" +
			 "Immune to chill and freeze effects\n" +
			 "+25 overheat capacity");*/
			Tooltip.SetDefault("You shouldn't have this");
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
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 25;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("LightSuitHelmet") && body.type == mod.ItemType("LightSuitBreastplate") && legs.type == mod.ItemType("LightSuitGreaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Allows the ability to Sense Move" + "\r\n"
				+ "Double tap a direction (when enabled)" + "\r\n"
				+ "15% increased ranged damage" + "\r\n"
				+ "Negates fall damage" + "\r\n"
				+ "Infinite breath" + "\r\n"
				+ "35% decreased overheat use" + "\r\n"
				+ "Immune to damage from the Dark World" + "\r\n"
				+ "Immune to damage from Dark Water";
			p.rangedDamage += 0.15f;
			p.noFallDmg = true;
			p.gills = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			//code for protection from Dark World/Dark Water goes here
			mp.overheatCost -= 0.35f;
			mp.senseMove = true;
			mp.visorGlow = true;
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 248, 224);
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DarkSuitBreastplate");
			recipe.AddIngredient(ItemID.HallowedBar, 25);
			//recipe.AddIngredient(null, "", 10); Dark World material
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override void AddRecipes() {}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class LightSuitGreaves : DarkSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Suit Greaves");
			/*Tooltip.SetDefault("5% increased ranged damage\n" +
			"20% increased movement speed\n" +
			"+25 overheat capacity\n" +
			"Allows you to cling to walls");*/
			Tooltip.SetDefault("You shouldn't have this");
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
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DarkSuitGreaves");
			recipe.AddIngredient(ItemID.HallowedBar, 20);
			//recipe.AddIngredient(null, "", 10); Dark World Material
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override void AddRecipes() {}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LightSuitHelmet : DarkSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Suit Helmet");
			/*Tooltip.SetDefault("5% increased ranged damage\n" +
			"+25 overheat capacity\n" +
			"Improved night vision");*/
			Tooltip.SetDefault("You shouldn't have this");
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
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DarkSuitHelmet");
			recipe.AddIngredient(ItemID.HallowedBar, 15);
			//recipe.AddIngredient(null, "", 10); Dark World Material
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override void AddRecipes() {}
	}
}