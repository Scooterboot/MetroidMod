using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class VariaSuitBreastplate : PowerSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit Breastplate");
			Tooltip.SetDefault("5% increased ranged damage\n" +
			 "Immunity to fire blocks\n" +
			 "Immunity to chill and freeze effects\n" +
			 "+10 overheat capacity");
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
			player.rangedDamage += 0.05f;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 10;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("VariaSuitHelmet") && body.type == mod.ItemType("VariaSuitBreastplate") && legs.type == mod.ItemType("VariaSuitGreaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Hold the Sense move key and left/right while an enemy is moving towards you to dodge" + "\r\n" + 
						"5% increased ranged damage" + "\r\n" + 
						"25% decreased overheat use" + "\r\n" + 
						"55% increased underwater breathing" + "\r\n" + 
						"Negates fall damage";
			p.rangedDamage += 0.05f;
			p.noFallDmg = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			mp.breathMult = 1.55f;
			mp.overheatCost -= 0.20f;
			mp.SenseMove(p);
			mp.visorGlow = true;
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
			Tooltip.SetDefault("5% increased ranged damage\n" +
			 "10% increased movement speed\n" +
			 "+10 overheat capacity\n" +
			 "Allows you to slide down walls");
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
			player.rangedDamage += 0.05f;
			player.moveSpeed += 0.10f;
			player.spikedBoots += 1;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 10;
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
			Tooltip.SetDefault("5% increased ranged damage\n" + 
			"+10 overheat capacity\n" + 
			"Improved night vision");
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
			player.rangedDamage += 0.05f;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 10;
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