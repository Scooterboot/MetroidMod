using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class GravitySuitBreastplate : VariaSuitV2Breastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Suit Breastplate");
			Tooltip.SetDefault("+60 overheat capacity\n" +
			"30% decreased overheat use\n" +
			"15% decreased Missile Charge Combo cost\n" +
			"Immune to knockback");
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
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 60;
			mp.overheatCost -= 0.30f;
			mp.missileCost -= 0.15f;
			player.noKnockback = true;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("GravitySuitHelmet") && body.type == mod.ItemType("GravitySuitBreastplate") && legs.type == mod.ItemType("GravitySuitGreaves"));
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Allows the ability to Sense Move" + "\n" + 
						"Double tap a direction (when enabled)" + "\n" + 
						"Immunity to fire blocks" + "\n" + 
						"Immunity to chill and freeze effects" + "\n" + 
						"Free movement in liquid" + "\n" + 
						"Grants 7 seconds of lava immunity";
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.senseMove = true;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.ignoreWater = true;
			player.lavaMax += 420; // blaze it
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitV2Breastplate");
			recipe.AddIngredient(ItemID.HallowedBar, 24);
			recipe.AddIngredient(null, "GravityGel", 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class GravitySuitGreaves : VariaSuitV2Greaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Suit Greaves");
			Tooltip.SetDefault("Allows somersaulting & wall jumping\n" +
			"Negates fall damage\n" +
			"10% increased movement speed");
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
            player.GetModPlayer<MPlayer>().enableWallJump = true;
            player.noFallDmg = true;
			player.moveSpeed += 0.10f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitV2Greaves");
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddIngredient(null, "GravityGel", 17);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class GravitySuitHelmet : VariaSuitV2Helmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Suit Helmet");
			Tooltip.SetDefault("25% increased hunter damage\n" + 
			"10% increased hunter critical strike chance\n" + 
			"Emits light and grants improved night vision\n" +
			"Infinite breath underwater");
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
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.25f;
			HunterDamagePlayer.ModPlayer(player).hunterCrit += 10;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.visorGlow = true;
			player.nightVision = true;
			player.gills = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitV2Helmet");
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddIngredient(null, "GravityGel", 17);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}