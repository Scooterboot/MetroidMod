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
			Tooltip.SetDefault("5% increased hunter damage\n" +
			 "+10 overheat capacity\n" +
			 "Immunity to fire blocks\n" +
			 "Immunity to chill and freeze effects");
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
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
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
			p.setBonus = "Allows the ability to Sense Move" + "\r\n" + 
						"Double tap a direction (when enabled)" + "\r\n" + 
						"5% increased hunter damage" + "\r\n" + 
						"25% decreased overheat use" + "\r\n" + 
						"55% increased underwater breathing" + "\r\n" + 
						"Negates fall damage";
			HunterDamagePlayer.ModPlayer(p).hunterDamageMult += 0.05f;
			//p.rangedDamage += 0.05f;
			p.noFallDmg = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			mp.breathMult = 1.55f;
			mp.overheatCost -= 0.20f;
			mp.senseMove = true;
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
			Tooltip.SetDefault("5% increased hunter damage\n" +
			 "+10 overheat capacity\n" +
			 "10% increased movement speed\n" +
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
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
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
			Tooltip.SetDefault("5% increased hunter damage\n" + 
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
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
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