using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
	public class ReserveTank5 : ModItem
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
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.reserveTanks = 4;
            mp.reserveHeartsValue = 25;
        }
        public override bool CanEquipAccessory(Player player, int slot)
        {
            for (int k = 3; k < 8 + player.extraAccessorySlots; k++)
            {
                if (player.armor[k].type == mod.ItemType("ReserveTank") || player.armor[k].type == mod.ItemType("ReserveTank2") || player.armor[k].type == mod.ItemType("ReserveTank3") || player.armor[k].type == mod.ItemType("ReserveTank4"))
                {
                    return false;
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ReserveTank4", 1);
            recipe.AddIngredient(ItemID.LifeFruit, 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}