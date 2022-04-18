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
			Tooltip.SetDefault("+45 overheat capacity\n" +
			"25% decreased overheat use\n" +
			"10% decreased Missile Charge Combo cost");
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
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 45;
			mp.overheatCost -= 0.25f;
			mp.missileCost -= 0.10f;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("VariaSuitV2Helmet") && body.type == mod.ItemType("VariaSuitV2Breastplate") && legs.type == mod.ItemType("VariaSuitV2Greaves"));
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Allows the ability to Sense Move" + "\n" + 
						"Double tap a direction (when enabled)" + "\n" + 
						"Immunity to fire blocks" + "\n" + 
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
			Tooltip.SetDefault("Allows somersaulting & wall jumping\n" +
			"Negates fall damage\n" +
			"10% increased movement speed");
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
            player.GetModPlayer<MPlayer>().enableWallJump = true;
            player.noFallDmg = true;
			player.moveSpeed += 0.10f;
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
			Tooltip.SetDefault("20% increased hunter damage\n" + 
			"7% increased hunter critical strike chance\n" + 
			"Emits light and grants improved night vision\n" +
			"80% increased underwater breathing");
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
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.20f;
			HunterDamagePlayer.ModPlayer(player).hunterCrit += 7;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.visorGlow = true;
			mp.breathMult = 1.8f;
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