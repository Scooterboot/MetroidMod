using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class StardustHazardShieldSuitBreastplate : HazardShieldSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Hazard Shield Suit Breastplate");
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
			return (head.type == mod.ItemType("StardustHazardShieldSuitHelmet") && body.type == mod.ItemType("StardustHazardShieldSuitBreastplate") && legs.type == mod.ItemType("StardustHazardShieldSuitGreaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Hold the Sense move key and left/right while an enemy is moving towards you to dodge" + "\r\n"
				+ "20% increased ranged damage" + "\r\n"
				+ "Press the Hypermode key to activate Hypermode (take 100 damage to gain +50% damage for 20 seconds, 120s cooldown)" + "\r\n"
				+ "Greatly increased health regen when standing on Phazon" + "\r\n"
				+ "Negates fall damage" + "\r\n"
				+ "Infinite breath" + "\r\n"
				+ "40% decreased overheat use" + "\r\n"
				+ "Debuffs tick down 3 times as fast";
			p.rangedDamage += 0.20f;
			p.gills = true;
			p.noFallDmg = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			mp.phazonImmune = true;
			mp.phazonRegen = 8;
			mp.overheatCost -= 0.40f;
			mp.SenseMove(p);
			mp.visorGlow = true;
			mp.hazardShield = true;
			//code to activate Hypermode goes here; might need to add a Hypermode hook to MPlayer like Sense Move
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(196, 248, 255);
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "HazardShieldSuitBreastplate");
			recipe.AddIngredient(ItemID.LunarBar, 20);
			recipe.AddIngredient(ItemID.FragmentStardust, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
	[AutoloadEquip(EquipType.Legs)]
	public class StardustHazardShieldSuitGreaves : HazardShieldSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Hazard Shield Suit Greaves");
			/*Tooltip.SetDefault("5% increased ranged damage\n" +
			"20% increased movement speed\n" +
			"+33 overheat capacity\n" +
			"Allows you to cling to walls");*/
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
			player.spikedBoots += 2;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 33;
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "HazardShieldSuitGreaves");
			recipe.AddIngredient(ItemID.LunarBar, 15);
			recipe.AddIngredient(ItemID.FragmentStardust, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
	[AutoloadEquip(EquipType.Head)]
	public class StardustHazardShieldSuitHelmet : HazardShieldSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Hazard Shield Suit Helmet");
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
			recipe.AddIngredient(null, "HazardShieldSuitHelmet");
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ItemID.FragmentStardust, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
}