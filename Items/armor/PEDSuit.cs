using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class PEDSuitBreastplate : VariaSuitV2Breastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("P.E.D. Suit Breastplate");
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
			item.value = 25000;
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
			return (head.type == mod.ItemType("PEDSuitHelmet") && body.type == mod.ItemType("PEDSuitBreastplate") && legs.type == mod.ItemType("PEDSuitGreaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Allows the ability to Sense Move" + "\n" + 
				"Double tap a direction (when enabled)" + "\n" + 
				"Press the Hypermode key to activate Hypermode (take 100 damage to gain +50% damage for 20 seconds, 120 s cooldown)" + "\n" +
				"Slightly increased health regen when standing on Phazon" + "\n" +
				//"10% increased ranged damage" + "\n" +
				"30% decreased overheat use" + "\n" +
				"Negates fall damage" + "\n" +
				"Infinite breath";
			//p.rangedDamage += 0.1f;
			p.gills = true;
			p.noFallDmg = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			mp.phazonImmune = true;
			mp.phazonRegen = 2;
			mp.overheatCost -= 0.30f;
			mp.senseMove = true;
			mp.visorGlow = true;
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
			recipe.AddIngredient(null, "VariaSuitV2Breastplate");
			//Phazon biome materials go here
			recipe.AddIngredient(null, "EnergyTank");
			//recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override void AddRecipes() {}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class PEDSuitGreaves : VariaSuitV2Greaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("P.E.D. Suit Greaves");
			/*Tooltip.SetDefault("5% increased ranged damage\n" +
			 "10% increased movement speed\n" +
			 "+20 overheat capacity\n" +
			 "Allows somersaulting");*/
			Tooltip.SetDefault("You shouldn't have this");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 5;
			item.value = 20000;
			item.defense = 13;
		}
		public override void UpdateEquip(Player player)
		{
			player.rangedDamage += 0.05f;
			player.moveSpeed += 0.1f;
			player.GetModPlayer<MPlayer>().canSomersault = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 20;
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitV2Greaves");
			//Phazon biome materials go here
			recipe.AddIngredient(null, "EnergyTank");
			//recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override void AddRecipes() {}
	}
	[AutoloadEquip(EquipType.Head)]
	public class PEDSuitHelmet : VariaSuitV2Helmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("P.E.D. Suit Helmet");
			/*Tooltip.SetDefault("25% increased hunter damage\n" + 
			"10% increased hunter critical strike chance\n" + 
			"+20 overheat capacity\n" +
			"Improved night vision");*/
			Tooltip.SetDefault("You shouldn't have this");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 5;
			item.value = 18000;
			item.defense = 12;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.25f;
			HunterDamagePlayer.ModPlayer(player).hunterCrit += 10;
			//player.rangedDamage += 0.05f;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 20;
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitV2Helmet");
			//Phazon biome materials go here
			recipe.AddIngredient(null, "EnergyTank");
			//recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override void AddRecipes() {}
	}
}