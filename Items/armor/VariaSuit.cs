using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class VariaSuitBreastplate : PowerSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit Breastplate");
			Tooltip.SetDefault("+30 overheat capacity\n" +
			"20% decreased overheat use\n" +
			"5% decreased Missile Charge Combo cost");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 3;
			item.value = 9000;
			item.defense = 8;
		}
		public override void UpdateEquip(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 30;
			mp.overheatCost -= 0.20f;
			mp.missileCost -= 0.05f;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("VariaSuitHelmet") && body.type == mod.ItemType("VariaSuitBreastplate") && legs.type == mod.ItemType("VariaSuitGreaves"));
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Allows the ability to Sense Move" + "\r\n" + 
						"Double tap a direction (when enabled)" + "\r\n" + 
						"Immunity to fire blocks" + "\r\n" + 
						"Immunity to chill and freeze effects";
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.senseMove = true;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PowerSuitBreastplate");
			recipe.AddIngredient(ItemID.HellstoneBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VariaSuitGreaves : PowerSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit Greaves");
			Tooltip.SetDefault("Allows you to slide down walls\n" +
			"Negates fall damage\n" +
			"10% increased movement speed");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 3;
			item.value = 6000;
			item.defense = 7;
		}
		public override void UpdateEquip(Player player)
		{
			player.spikedBoots += 1;
			player.noFallDmg = true;
			player.moveSpeed += 0.10f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PowerSuitGreaves");
			recipe.AddIngredient(ItemID.HellstoneBar, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VariaSuitHelmet : PowerSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit Helmet");
			Tooltip.SetDefault("15% increased hunter damage\n" + 
			"5% increased hunter critical strike chance\n" + 
			"Emits light and grants improved night vision\n" +
			"55% increased underwater breathing");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 3;
			item.value = 6000;
			item.defense = 7;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.15f;
			HunterDamagePlayer.ModPlayer(player).hunterCrit += 5;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.visorGlow = true;
			mp.breathMult = 1.55f;
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PowerSuitHelmet");
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}