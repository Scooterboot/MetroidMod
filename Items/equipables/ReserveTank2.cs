using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
	public class ReserveTank2 : ModItem
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
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.reserveTanks = 2;
        }
        public override bool CanEquipAccessory(Player player, int slot)
        {
            for (int k = 3; k < 8 + player.extraAccessorySlots; k++)
            {
                if (player.armor[k].type == mod.ItemType("ReserveTank") || player.armor[k].type == mod.ItemType("ReserveTank3") || player.armor[k].type == mod.ItemType("ReserveTank4") || player.armor[k].type == mod.ItemType("ReserveTank5"))
                {
                    return false;
                }
            }
            return true;
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
}