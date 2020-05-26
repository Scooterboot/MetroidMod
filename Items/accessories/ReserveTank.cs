using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.accessories
{
	public class ReserveTank : RTankAccessory
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reserve Tank");
			Tooltip.SetDefault("Stores a heart picked up when at full health\n" + 
				"Automatically uses the stored heart to save you from death");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 1;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("ReserveTank");
			item.rare = 2;
			item.value = 20000;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.reserveTanks = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "EnergyTank", 1);
			recipe.AddIngredient(ItemID.LifeCrystal);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class ReserveTank2 : RTankAccessory
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reserve Tank MK2");
			Tooltip.SetDefault("Stores up to 2 hearts picked up when at full health\n" + 
				"Automatically uses the stored hearts to save you from death");
		}
		public override void SetDefaults()
		{
			item.width = 64;
			item.height = 32;
			item.maxStack = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			item.consumable = false;
			item.rare = 3;
			item.value = 40000;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.reserveTanks = 2;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ReserveTank", 2);
			recipe.AddIngredient(ItemID.Bone, 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class ReserveTank3 : RTankAccessory
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reserve Tank MK3");
			Tooltip.SetDefault("Stores up to 3 hearts picked up when at full health\n" + 
				"Automatically uses the stored hearts to save you from death");
		}
		public override void SetDefaults()
		{
			item.width = 64;
			item.height = 54;
			item.maxStack = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			item.consumable = false;
			item.rare = 4;
			item.value = 60000;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.reserveTanks = 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ReserveTank2", 1);
			recipe.AddIngredient(null, "ReserveTank", 1);
			recipe.AddIngredient(ItemID.SoulofLight, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class ReserveTank4 : RTankAccessory
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reserve Tank MK4");
			Tooltip.SetDefault("Stores up to 4 hearts picked up when at full health\n" + 
				"Automatically uses the stored hearts to save you from death");
		}
		public override void SetDefaults()
		{
			item.width = 64;
			item.height = 54;
			item.maxStack = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			item.consumable = false;
			item.rare = 5;
			item.value = 80000;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.reserveTanks = 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ReserveTank3", 1);
			recipe.AddIngredient(null, "ReserveTank", 1);
			recipe.AddIngredient(ItemID.HallowedBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class ReserveTank5 : RTankAccessory
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reserve Tank MK5");
			Tooltip.SetDefault("Stores up to 4 hearts picked up when at full health\n" + 
				"Automatically uses the stored hearts to save you from death\n" + 
				"Stored hearts restore 25 health each");
		}
		public override void SetDefaults()
		{
			item.width = 64;
			item.height = 54;
			item.maxStack = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			item.consumable = false;
			item.rare = 6;
			item.value = 100000;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.reserveTanks = 4;
			mp.reserveHeartsValue = 25;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ReserveTank4", 1);
			recipe.AddIngredient(ItemID.LifeFruit, 1);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	
	public abstract class RTankAccessory : ModItem
	{
		public override bool CanEquipAccessory(Player player, int slot)
		{
			if (slot < 10)
			{
				int index = FindDifferentEquippedExclusiveAccessory().index;
				if (index != -1)
				{
					return slot == index;
				}
			}
			return base.CanEquipAccessory(player, slot);
		}

		public override bool CanRightClick()
		{
			int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
			for (int i = 13; i < 13 + maxAccessoryIndex; i++)
			{
				if (Main.LocalPlayer.armor[i].type == item.type) return false;
			}

			if (FindDifferentEquippedExclusiveAccessory().accessory != null)
			{
				return true;
			}
			return base.CanRightClick();
		}

		public override void RightClick(Player player)
		{
			var (index, accessory) = FindDifferentEquippedExclusiveAccessory();
			if (accessory != null)
			{
				Main.LocalPlayer.QuickSpawnClonedItem(accessory);
				Main.LocalPlayer.armor[index] = item.Clone();
			}
		}

		protected (int index, Item accessory) FindDifferentEquippedExclusiveAccessory()
		{
			int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
			for (int i = 3; i < 3 + maxAccessoryIndex; i++)
			{
				Item otherAccessory = Main.LocalPlayer.armor[i];
				if (!otherAccessory.IsAir && !item.IsTheSameAs(otherAccessory) && otherAccessory.modItem is RTankAccessory)
				{
					return (i, otherAccessory);
				}
			}
			return (-1, null);
		}
	}
}