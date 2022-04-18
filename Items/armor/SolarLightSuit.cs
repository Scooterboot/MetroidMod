using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class SolarLightSuitBreastplate : LightSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Light Suit Breastplate");
			/*Tooltip.SetDefault("5% increased ranged damage\n" +
			"Immune to fire blocks\n" +
			"Immune to chill and freeze effects\n" +
			"+34 overheat capacity");*/
			Tooltip.SetDefault("You shouldn't have this");
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
			player.rangedDamage += 0.05f;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 34;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("SolarLightSuitHelmet") && body.type == mod.ItemType("SolarLightSuitBreastplate") && legs.type == mod.ItemType("SolarLightSuitGreaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Allows the ability to Sense Move" + "\n"
				+ "Double tap a direction (when enabled)" + "\n"
				//+ "20% increased ranged damage" + "\n"
				+ "Negates fall damage" + "\n"
				+ "Infinite breath" + "\n"
				+ "40% decreased overheat use" + "\n"
				+ "Immune to damage from the Dark World" + "\n"
				+ "Immune to damage from Dark Water";
			//p.rangedDamage += 0.20f;
			p.noFallDmg = true;
			p.gills = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			//code for protection from Dark World/Dark Water goes here
			mp.overheatCost -= 0.40f;
			mp.senseMove = true;
			mp.visorGlow = true;
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 238, 127);
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "LightSuitBreastplate");
			recipe.AddIngredient(ItemID.LunarBar, 20);
			recipe.AddIngredient(ItemID.FragmentSolar, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override void AddRecipes() {}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class SolarLightSuitGreaves : LightSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Light Suit Greaves");
			/*Tooltip.SetDefault("5% increased ranged damage\n" +
			"20% increased movement speed\n" +
			"+33 overheat capacity\n" +
			"Allows somersaulting & wall jumping");*/
			Tooltip.SetDefault("You shouldn't have this");
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
			player.rangedDamage += 0.05f;
			player.moveSpeed += 0.20f;
			player.GetModPlayer<MPlayer>().enableWallJump = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 33;
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "LightSuitGreaves");
			recipe.AddIngredient(ItemID.LunarBar, 15);
			recipe.AddIngredient(ItemID.FragmentSolar, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override void AddRecipes() {}
	}
	[AutoloadEquip(EquipType.Head)]
	public class SolarLightSuitHelmet : LightSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Light Suit Helmet");
			/*Tooltip.SetDefault("5% increased ranged damage\n" +
			"+33 overheat capacity\n" +
			"Improved night vision");*/
			Tooltip.SetDefault("You shouldn't have this");
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
			player.rangedDamage += 0.05f;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 33;
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "LightSuitHelmet");
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ItemID.FragmentSolar, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override void AddRecipes() {}
	}
}