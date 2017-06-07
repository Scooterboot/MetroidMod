using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace MetroidMod.Items.tools
{
    public class GrappleBeam : ModItem
    {
        public override void SetDefaults()
        {
            //clone and modify the ones we want to copy
            item.CloneDefaults(ItemID.AmethystHook);
            item.shootSpeed = 25f; // how quickly the hook is shot.
            item.shoot = mod.ProjectileType("GrappleBeamShot");
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grappling Beam");
			Tooltip.SetDefault("'Swingy!'\n" + 
            "Press left or right to swing\n" + 
            "Press up or down to ascend or descend the grapple");
		}
        public override void AddRecipes()  //How to craft this item
        {
            ModRecipe recipe = new ModRecipe(mod); 
            recipe.AddIngredient(null, "ChoziteBar", 15);
            recipe.AddIngredient(null, "EnergyShard", 3);
            recipe.AddTile(TileID.Anvils);   
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}