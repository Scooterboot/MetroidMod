using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class VariaSuitV2Breastplate : VariaSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit V2 Breastplate");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+15 overheat capacity\n" +
			"Immunity to fire blocks\n" +
			"Immunity to chill and freeze effects");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 4;
			item.value = 18000;
			item.defense = 11;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 15;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("VariaSuitV2Helmet") && body.type == mod.ItemType("VariaSuitV2Breastplate") && legs.type == mod.ItemType("VariaSuitV2Greaves"));
		}
		public override void UpdateArmorSet(Player p)
		{
			p.setBonus = "Allows the ability to Sense Move" + "\r\n" + 
						"Double tap a direction (when enabled)" + "\r\n" + 
						"5% increased hunter damage" + "\r\n" + 
						"25% decreased overheat use" + "\r\n" + 
						"10% decreased Missile Charge Combo cost" + "\r\n" + 
						"80% increased underwater breathing" + "\r\n" + 
						"Negates fall damage";
			HunterDamagePlayer.ModPlayer(p).hunterDamageMult += 0.05f;
			//p.rangedDamage += 0.05f;
			p.noFallDmg = true;
			MPlayer mp = p.GetModPlayer<MPlayer>();
			mp.breathMult = 1.8f;
			mp.overheatCost -= 0.25f;
			mp.missileCost -= 0.1f;
			mp.senseMove = true;
			mp.visorGlow = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitBreastplate");
			recipe.AddIngredient(ItemID.MythrilBar, 20);
			recipe.AddIngredient(null, "KraidTissue", 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitBreastplate");
			recipe.AddIngredient(ItemID.OrichalcumBar, 20);
			recipe.AddIngredient(null, "KraidTissue", 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VariaSuitV2Greaves : VariaSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit V2 Greaves");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+15 overheat capacity\n" +
			"10% increased movement speed\n" +
			"Allows you to slide down walls");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 4;
			item.value = 12000;
			item.defense = 10;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			player.moveSpeed += 0.1f;
			player.spikedBoots += 1;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 15;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitGreaves");
			recipe.AddIngredient(ItemID.MythrilBar, 15);
			recipe.AddIngredient(null, "KraidTissue", 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitGreaves");
			recipe.AddIngredient(ItemID.OrichalcumBar, 15);
			recipe.AddIngredient(null, "KraidTissue", 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VariaSuitV2Helmet : VariaSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit V2 Helmet");
			Tooltip.SetDefault("5% increased hunter damage\n" +
			"+15 overheat capacity\n" +
			"Improved night vision");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 4;
			item.value = 12000;
			item.defense = 10;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.05f;
			//player.rangedDamage += 0.05f;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 15;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitHelmet");
			recipe.AddIngredient(ItemID.MythrilBar, 10);
			recipe.AddIngredient(null, "KraidTissue", 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VariaSuitHelmet");
			recipe.AddIngredient(ItemID.OrichalcumBar, 10);
			recipe.AddIngredient(null, "KraidTissue", 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}