using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class HazardShieldSuitBreastplate : PEDSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hazard Shield Suit Breastplate");
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
			return (head.type == mod.ItemType("HazardShieldSuitHelmet") && body.type == mod.ItemType("HazardShieldSuitBreastplate") && legs.type == mod.ItemType("HazardShieldSuitGreaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Allows the ability to Sense Move" + "\r\n"
				+ "Double tap a direction (when enabled)" + "\r\n"
				+ "15% increased ranged damage" + "\r\n"
				+ "Press the Hypermode key to activate Hypermode (take 100 damage to gain +50% damage for 20 seconds, 120s cooldown)" + "\r\n"
				+ "Increased health regen when standing on Phazon" + "\r\n"
				+ "Negates fall damage" + "\r\n"
				+ "Infinite breath" + "\r\n"
				+ "35% decreased overheat use" + "\r\n"
				+ "Debuffs tick down twice as fast";
			p.rangedDamage += 0.15f;
			p.gills = true;
			p.noFallDmg = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			mp.phazonImmune = true;
			mp.phazonRegen = 4;
			mp.overheatCost -= 0.35f;
			mp.senseMove = true;
			mp.visorGlow = true;
			mp.hazardShield = true;
			//code to activate Hypermode goes here; might need to add a Hypermode hook to MPlayer like Sense Move
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(0, 228, 255);
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PEDSuitBreastplate");
			recipe.AddIngredient(ItemID.ShroomiteBar, 25);
			//recipe.AddIngredient(null, "", 10);
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddTile(null, "NovaWorkTableTile");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override void AddRecipes() {}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class HazardShieldSuitGreaves : PEDSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hazard Shield Suit Greaves");
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
			recipe.AddIngredient(null, "PEDSuitGreaves");
			recipe.AddIngredient(ItemID.ShroomiteBar, 20);
			//recipe.AddIngredient(null, "", 10);  /TBD
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddTile(null, "NovaWorkTableTile");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override void AddRecipes() {}
	}
	[AutoloadEquip(EquipType.Head)]
	public class HazardShieldSuitHelmet : PEDSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hazard Shield Suit Helmet");
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
			recipe.AddIngredient(null, "PEDSuitHelmet");
			recipe.AddIngredient(ItemID.ShroomiteBar, 15);
			//recipe.AddIngredient(null, "", 10);  /TBD
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddTile(null, "NovaWorkTableTile");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override void AddRecipes() {}
	}
}