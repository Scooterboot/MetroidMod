using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.accessories
{
	public class PowerGrip : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Grip");
			Tooltip.SetDefault("Allows the user to grab onto ledges\n" + "Does not need to be equipped; works while in inventory");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 10000;
			item.rare = 2;
			item.accessory = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("PowerGripTile");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
            mp.powerGrip = true;
        }
        public override void UpdateInventory(Player player)
        {
            MPlayer mp = player.GetModPlayer<MPlayer>();
            mp.powerGrip = true;
        }
		public override void UpdateVanity(Player player, EquipType type)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
            mp.powerGrip = true;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ChoziteBar", 4);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
