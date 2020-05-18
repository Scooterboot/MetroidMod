using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace MetroidMod.Items.accessories
{
    public class TerraBooster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Booster");
            Tooltip.SetDefault("Allows the user to run insanely fast and extra mobility on ice\n" +
            "Allows somersaulting\n" +
            "Damage enemies while running or somersaulting\n" +
            "Damage increases with enemy's damage\n" +
            "Allows the user to jump up to 10 times in a row\n" +
            "Jumps recharge mid-air\n" +
            "Holding left/right while jumping midair gives a boost\n" + 
            "Provides the ability to walk on water and lava\n" + 
            "Grants immunity to fire blocks and 7 seconds lava immunity\n" +
			"Increases jump height and prevents fall damage");
        }
        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 32;
            item.maxStack = 1;
            item.value = 250000;
            item.rare = 9;
            item.accessory = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ScrewSpaceBooster");
            recipe.AddIngredient(ItemID.FrostsparkBoots);
            recipe.AddIngredient(ItemID.LavaWaders);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            MPlayer mp = player.GetModPlayer<MPlayer>();
            player.accRunSpeed = 6.75f;
            player.moveSpeed += 0.2f;
            player.iceSkate = true;
            player.waterWalk = true;
            player.fireWalk = true;
            player.lavaMax += 420;
            mp.speedBooster = true;
            mp.spaceJump = true;
            mp.screwAttack = Math.Max(3,mp.screwAttack);
			mp.hiJumpBoost = true;
			player.noFallDmg = true;
        }
    }
}