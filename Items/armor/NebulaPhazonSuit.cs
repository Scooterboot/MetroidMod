using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.armor
{
	[AutoloadEquip(EquipType.Body)]
	public class NebulaPhazonSuitBreastplate : PhazonSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Phazon Suit Breastplate");
			Tooltip.SetDefault("+100 overheat capacity\n" +
			"40% decreased overheat use\n" +
			"25% decreased Missile Charge Combo cost\n" +
			"Immune to knockback");
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
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 100;
			mp.overheatCost -= 0.40f;
			mp.missileCost -= 0.25f;
			player.noKnockback = true;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("NebulaPhazonSuitHelmet") && body.type == mod.ItemType("NebulaPhazonSuitBreastplate") && legs.type == mod.ItemType("NebulaPhazonSuitGreaves"));
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Allows the ability to Sense Move" + "\n" + 
						"Double tap a direction (when enabled)" + "\n" + 
						"Immunity to fire blocks" + "\n" + 
						"Immunity to chill and freeze effects" + "\n" + 
						"Free movement in liquid" + "\n" + 
						"Grants 7 seconds of lava immunity" + "\n" + 
						"Immune to damage caused by blue Phazon blocks" + "\n" + 
						"Enables Phazon Beam use";
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.senseMove = true;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.ignoreWater = true;
			player.lavaMax += 420; // blaze it
			mp.phazonImmune = true;
			mp.canUsePhazonBeam = true;
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 55, 255);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
			player.armorEffectDrawOutlines = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PhazonSuitBreastplate");
			recipe.AddIngredient(ItemID.LunarBar, 16);
			recipe.AddIngredient(ItemID.FragmentNebula, 20);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class NebulaPhazonSuitGreaves : PhazonSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Phazon Suit Greaves");
			Tooltip.SetDefault("Allows somersaulting & wall jumping\n" +
			"Negates fall damage\n" +
			"20% increased movement speed");
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
			player.GetModPlayer<MPlayer>().enableWallJump = true;
			player.noFallDmg = true;
			player.moveSpeed += 0.20f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PhazonSuitGreaves");
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ItemID.FragmentNebula, 15);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class NebulaPhazonSuitHelmet : PhazonSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Phazon Suit Helmet");
			Tooltip.SetDefault("35% increased hunter damage\n" + 
			"20% increased hunter critical strike chance\n" + 
			"Emits light and grants improved night vision\n" +
			"Infinite breath underwater");
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
			HunterDamagePlayer.ModPlayer(player).hunterDamageMult += 0.35f;
			HunterDamagePlayer.ModPlayer(player).hunterCrit += 20;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.visorGlow = true;
			player.nightVision = true;
			player.gills = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PhazonSuitHelmet");
			recipe.AddIngredient(ItemID.LunarBar, 8);
			recipe.AddIngredient(ItemID.FragmentNebula, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}