using MetroidMod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Accessories
{
	public class ReserveTank : RTankAccessory
	{
		//All the important numbers changes need to be up here so the dynamic localization thing can access them.
		//They're written as the percent changes so it's easier for the thing to read them
		//On the plus side it'll make changing stats easier!   -Z
		public static int tankCount = 1; //amount of reserve heart tanks provided

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(tankCount);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Life Reserve Tank");
			/* Tooltip.SetDefault("Stores a heart picked up when at full health\n" + 
				"Automatically uses the stored heart to save you from death"); */

			Item.ResearchUnlockCount = 4;
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 1;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			//Item.consumable = true;
			//Item.createTile = mod.TileType("ReserveTank");
			Item.rare = ItemRarityID.Green;
			Item.value = 50000;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.reserveTanks = tankCount;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(SuitAddonLoader.GetAddon<SuitAddons.ReserveTank>().ItemType, 1)
				.Register();
		}
	}
	public class ReserveTank2 : RTankAccessory
	{
		public static int tankCount = 2; //amount of reserve heart tanks provided

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(tankCount);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Life Reserve Tank MK2");
			/* Tooltip.SetDefault("Stores up to 2 hearts picked up when at full health\n" + 
				"Automatically uses the stored hearts to save you from death"); */

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 32;
			Item.maxStack = 1;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = false;
			Item.rare = ItemRarityID.Orange;
			Item.value = 40000;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.reserveTanks = tankCount;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ReserveTank>(2)
				.AddIngredient(ItemID.Bone, 5)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ReserveTank", 2);
			recipe.AddIngredient(ItemID.Bone, 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
	public class ReserveTank3 : RTankAccessory
	{
		public static int tankCount = 3; //amount of reserve heart tanks provided

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(tankCount);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Life Reserve Tank MK3");
			/* Tooltip.SetDefault("Stores up to 3 hearts picked up when at full health\n" + 
				"Automatically uses the stored hearts to save you from death"); */

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 54;
			Item.maxStack = 1;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = false;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 60000;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.reserveTanks = tankCount;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ReserveTank2>(1)
				.AddIngredient<ReserveTank>(1)
				.AddIngredient(ItemID.SoulofLight, 1)
				.AddIngredient(ItemID.SoulofNight, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ReserveTank2", 1);
			recipe.AddIngredient(null, "ReserveTank", 1);
			recipe.AddIngredient(ItemID.SoulofLight, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
	public class ReserveTank4 : RTankAccessory
	{
		public static int tankCount = 4; //amount of reserve heart tanks provided

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(tankCount);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Life Reserve Tank MK4");
			/* Tooltip.SetDefault("Stores up to 4 hearts picked up when at full health\n" + 
				"Automatically uses the stored hearts to save you from death"); */

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 54;
			Item.maxStack = 1;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = false;
			Item.rare = ItemRarityID.Pink;
			Item.value = 80000;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.reserveTanks = tankCount;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ReserveTank3>(1)
				.AddIngredient<ReserveTank>(1)
				.AddIngredient(ItemID.HallowedBar, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ReserveTank3", 1);
			recipe.AddIngredient(null, "ReserveTank", 1);
			recipe.AddIngredient(ItemID.HallowedBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
	public class ReserveTank5 : RTankAccessory
	{
		public static int tankCount = 4; //amount of reserve heart tanks provided

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(tankCount);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Life Reserve Tank MK5");
			/* Tooltip.SetDefault("Stores up to 4 hearts picked up when at full health\n" + 
				"Automatically uses the stored hearts to save you from death\n" + 
				"Stored hearts restore 25 health each"); */

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 54;
			Item.maxStack = 1;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = false;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = 100000;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.reserveTanks = tankCount;
			mp.reserveHeartsValue = 25;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ReserveTank4>(1)
				.AddIngredient(ItemID.LifeFruit, 1)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ReserveTank4", 1);
			recipe.AddIngredient(ItemID.LifeFruit, 1);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}

	public abstract class RTankAccessory : ModItem
	{
		public override bool CanEquipAccessory(Player player, int slot, bool modded)
		{
			if (slot < 10)
			{
				int index = FindDifferentEquippedExclusiveAccessory().index;
				if (index != -1)
				{
					return slot == index;
				}
			}
			return base.CanEquipAccessory(player, slot, modded);
		}

		public override bool CanRightClick()
		{
			int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
			for (int i = 13; i < 13 + maxAccessoryIndex; i++)
			{
				if (Main.LocalPlayer.armor[i].type == Item.type) { return false; }
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
				Main.LocalPlayer.QuickSpawnItem(new Terraria.DataStructures.EntitySource_DropAsItem(player), accessory);
				Main.LocalPlayer.armor[index] = Item.Clone();
			}
		}

		protected (int index, Item accessory) FindDifferentEquippedExclusiveAccessory()
		{
			int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
			for (int i = 3; i < 3 + maxAccessoryIndex; i++)
			{
				Item otherAccessory = Main.LocalPlayer.armor[i];
				if (!otherAccessory.IsAir && otherAccessory.type != Item.type && otherAccessory.ModItem is RTankAccessory)
				{
					return (i, otherAccessory);
				}
			}
			return (-1, null);
		}
	}
}
