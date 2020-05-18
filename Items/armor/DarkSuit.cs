using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class DarkSuitBreastplate : VariaSuitV2Breastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Suit Breastplate");
			/*Tooltip.SetDefault("5% increased ranged damage\n" +
			 "Immunity to fire blocks\n" +
			 "Immunity to chill and freeze effects\n" +
			 "+20 overheat capacity");*/
			Tooltip.SetDefault("You shouldn't have this");
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
			player.rangedDamage += 0.05f;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 20;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("DarkSuitHelmet") && body.type == mod.ItemType("DarkSuitBreastplate") && legs.type == mod.ItemType("DarkSuitGreaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Press the Sense Move key while moving near an enemy to dodge in that direction" + "\r\n" + "5% increased ranged damage" + "\r\n" + "30% decreased overheat use" + "\r\n" + "Negates fall damage" + "\r\n" + "Infinite breath" + "\r\n" + "Reduces damage from the Dark World";
			p.rangedDamage += 0.05f;
			p.gills = true;
			p.noFallDmg = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			mp.overheatCost -= 0.30f;
			mp.SenseMove(p);
			mp.visorGlow = true;
			//code to reduce damage from Dark World goes here: without the Dark Suit, the player takes 10 damage per second; with the Dark Suit, the player takes 1 damage per second
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 64, 64);
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitV2Breastplate");
			//Dark World materials go here
			recipe.AddIngredient(null, "EnergyTank");
			//recipe.AddIngredient(ItemID.SoulofFright, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
	[AutoloadEquip(EquipType.Legs)]
	public class DarkSuitGreaves : VariaSuitV2Greaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Suit Greaves");
			/*Tooltip.SetDefault("5% increased ranged damage\n" +
			 "10% increased movement speed\n" +
			 "+20 overheat capacity\n" +
			 "Allows you to cling to walls");*/
			Tooltip.SetDefault("You shouldn't have this");
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
			player.rangedDamage += 0.05f;
			player.moveSpeed += 0.1f;
			player.spikedBoots += 2;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 20;
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitV2Greaves");
			//Dark World materials go here
			recipe.AddIngredient(null, "EnergyTank");
			//recipe.AddIngredient(ItemID.SoulofFright, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
	[AutoloadEquip(EquipType.Head)]
	public class DarkSuitHelmet : VariaSuitV2Helmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Suit Helmet");
			/*Tooltip.SetDefault("5% increased ranged damage\n" +
			"+20 overheat capacity\n" +
			"Improved night vision");*/
			Tooltip.SetDefault("You shouldn't have this");
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
			player.rangedDamage += 0.05f;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 20;
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitV2Helmet");
			//Dark World materials go here
			recipe.AddIngredient(null, "EnergyTank");
			//recipe.AddIngredient(ItemID.SoulofFright, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
}