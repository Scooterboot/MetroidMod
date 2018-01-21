using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Head)]
    public class HazardShieldSuitHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hazard Shield Suit Helmet");
            Tooltip.SetDefault("5% increased ranged damage\n" +
            "+25 overheat capacity\n" +
            "Improved night vision\n" +
            "Increased jump height");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 7;
            item.value = 36000;
            item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.nightVision = true;
            player.jumpBoost = true;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 25;
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PEDSuitHelmet");
            recipe.AddIngredient(ItemID.ShroomiteBar, 15);
            //recipe.AddIngredient(null, "", 10);  /TBD
            recipe.AddIngredient(null, "EnergyTank");
            recipe.AddTile(null, "NovaWorkTableTile");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
    }
}
