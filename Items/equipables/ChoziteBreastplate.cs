using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Body)]
    class ChoziteBreastplate : ModItem
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Breastplate");
		}
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 1;
            item.value = 5000;
            item.defense = 4;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == mod.ItemType("ChoziteHelmet") && body.type == mod.ItemType("ChoziteBreastplate") && legs.type == mod.ItemType("ChoziteGreaves"));
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+2 defense" + "\r\n"
                + "Allows you to slide down walls";
            player.statDefense += 2;
            player.spikedBoots += 1;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.visorGlow = true;
        }

        public override void UpdateVanitySet(Player P)
        {
            MPlayer mp = P.GetModPlayer<MPlayer>(mod);
            mp.isPowerSuit = true;
            mp.thrusters = true;
            if (Main.netMode != 2)
            {
                mp.thrusterTexture = mod.GetTexture("Gore/powerSuit_thrusters");
            }
            mp.visorGlowColor = new Color(0, 248, 112);
            if (P.velocity.Y != 0f && ((P.controlRight && P.direction == 1) || (P.controlLeft && P.direction == -1)) && mp.shineDirection == 0 && !mp.shineActive && !mp.ballstate)
            {
                mp.jet = true;
            }
            else if (mp.shineDirection == 0 || mp.shineDirection == 5)
            {
                mp.jet = false;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ChoziteBar", 30);
            recipe.AddIngredient(ItemID.Topaz, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
