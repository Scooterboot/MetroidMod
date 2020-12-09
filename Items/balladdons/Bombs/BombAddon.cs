using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.balladdons.Bombs
{
	public class BombAddon : HunterDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 10;
			item.noMelee = true;
			item.width = 32;
			item.height = 32;
			item.maxStack = 1;
			item.value = 2500;
			item.rare = 2;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("BombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.ballSlotType = 1;
			mItem.bombType = mod.ProjectileType("MBBomb");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 2);
			recipe.AddIngredient(null, "EnergyShard", 3);
			recipe.AddIngredient(ItemID.Bomb, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class PoisonBombAddon : BombAddon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poison Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb\n" +
			"Poisons foes");
		}
		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();
			item.damage = 16;
			item.value = 5000;
			item.rare = 2;
			item.createTile = mod.TileType("PoisonBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.bombType = mod.ProjectileType("PoisonBomb");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BombAddon");
			recipe.AddIngredient(ItemID.JungleSpores, 5);
			recipe.AddIngredient(ItemID.Stinger, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class FireBombAddon : BombAddon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb\n" +
			"Sets enemies on fire");
		}
		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();
			item.damage = 24;
			item.value = 7500;
			item.rare = 3;
			item.createTile = mod.TileType("FireBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.bombType = mod.ProjectileType("FireBomb");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BombAddon");
			recipe.AddIngredient(ItemID.HellstoneBar, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PoisonBombAddon");
			recipe.AddIngredient(ItemID.HellstoneBar, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class FrostburnBombAddon : BombAddon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frostburn Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb\n" +
			"Frostburns enemies");
		}
		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();
			item.damage = 32;
			item.value = 10000;
			item.rare = 3;
			item.createTile = mod.TileType("FrostburnBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.bombType = mod.ProjectileType("FrostburnBomb");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CobaltBar, 5);
			recipe.AddIngredient(ItemID.FrostCore);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PalladiumBar, 5);
			recipe.AddIngredient(ItemID.FrostCore);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class CursedFlameBombAddon : BombAddon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Fire Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb\n" +
			"Burns enemies with Cursed Flames");
		}
		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();
			item.damage = 38;
			item.value = 15000;
			item.rare = 4;
			item.createTile = mod.TileType("CursedFlameBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.bombType = mod.ProjectileType("CursedFlameBomb");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MythrilBar, 5);
			recipe.AddIngredient(ItemID.CursedFlame, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.OrichalcumBar, 5);
			recipe.AddIngredient(ItemID.CursedFlame, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class IchorBombAddon : BombAddon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb\n" +
			"Decreases enemy defense");
		}
		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();
			item.damage = 30;
			item.value = 15000;
			item.rare = 4;
			item.createTile = mod.TileType("IchorBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.bombType = mod.ProjectileType("IchorBomb");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MythrilBar, 5);
			recipe.AddIngredient(ItemID.Ichor, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.OrichalcumBar, 5);
			recipe.AddIngredient(ItemID.Ichor, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class ShadowflameBombAddon : BombAddon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb\n" +
			"Inflicts enemies with Shadowflame");
		}
		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();
			item.damage = 44;
			item.value = 20000;
			item.rare = 5;
			item.createTile = mod.TileType("ShadowflameBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.bombType = mod.ProjectileType("ShadowflameBomb");
		}
		public override void AddRecipes() {}
	}
	public class CrystalBombAddon : BombAddon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb\n" +
			"Fires off Crystal shards on detonation");
		}
		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();
			item.damage = 52;
			item.value = 30000;
			item.rare = 6;
			item.createTile = mod.TileType("CrystalBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.bombType = mod.ProjectileType("CrystalBomb");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 5);
			recipe.AddIngredient(ItemID.CrystalShard, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class VenomBombAddon : BombAddon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Venom Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb\n" +
			"Inflicts enemies with Acid Venom");
		}
		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();
			item.damage = 64;
			item.value = 40000;
			item.rare = 7;
			item.createTile = mod.TileType("VenomBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.bombType = mod.ProjectileType("VenomBomb");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
			recipe.AddIngredient(ItemID.VialofVenom, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class PhazonBombAddon : BombAddon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb\n" +
			"Inflicts enemies with Phazon radiation poisoning");
		}
		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();
			item.damage = 77;
			item.value = 50000;
			item.rare = 8;
			item.createTile = mod.TileType("PhazonBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.bombType = mod.ProjectileType("PhazonBomb");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PhazonBar", 5);
			recipe.AddTile(null, "NovaWorkTableTile");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class PumpkinBombAddon : BombAddon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pumpkin Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb\n" +
			"Fires off Jack 'O Lanterns on detonation\n" +
			"'I took a grenade to the face, dude!'");
		}
		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();
			item.damage = 75;
			item.value = 50000;
			item.rare = 8;
			item.createTile = mod.TileType("PumpkinBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.bombType = mod.ProjectileType("PumpkinBomb");
		}
		public override void AddRecipes() {}
	}
	public class BetsyBombAddon : BombAddon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Betsy Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb\n" +
			"Explodes in defense reducing miasma that also sets enemies on fire");
		}
		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();
			item.damage = 90;
			item.value = 60000;
			item.rare = 9;
			item.createTile = mod.TileType("BetsyBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.bombType = mod.ProjectileType("BetsyBomb");
		}
		public override void AddRecipes() {}
	}
	public class SolarFireBombAddon : BombAddon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Fire Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb\n" +
			"Burns foes with the fury of the sun");
		}
		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();
			item.damage = 150;
			item.value = 50000;
			item.rare = 10;
			item.createTile = mod.TileType("SolarFireBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.bombType = mod.ProjectileType("SolarFireBomb");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentSolar, 5);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}