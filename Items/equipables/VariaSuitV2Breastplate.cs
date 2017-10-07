using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Body)]
    public class VariaSuitV2Breastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Varia Suit V2 Breastplate");
            Tooltip.SetDefault("7% increased ranged damage\n" +
             "Immunity to fire blocks\n" +
             "Immunity to chill and freeze effects\n" +
             "+15 overheat capacity");
        }
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 4;
            item.value = 15000;
            item.defense = 11;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.07f;
            player.fireWalk = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 15;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == mod.ItemType("VariaSuitV2Helmet") && body.type == mod.ItemType("VariaSuitV2Breastplate") && legs.type == mod.ItemType("VariaSuitV2Greaves"));
        }

        public override void UpdateArmorSet(Player p)
        {
            p.setBonus = "Hold the Sense move key and left/right while an enemy is moving towards you to dodge" + "\r\n" + "5% increased ranged damage" + "\r\n" + "25% decreased overheat use" + "\r\n" + "Negates fall damage" + "\r\n" + "80% increased underwater breathing";
            p.rangedDamage += 0.05f;
            p.noFallDmg = true;
            MPlayer mp = p.GetModPlayer<MPlayer>(mod);
            mp.breathMult = 1.8f;
            mp.overheatCost -= 0.25f;
            mp.SenseMove(p);
            mp.visorGlow = true;
            if (!mp.ballstate)
            {
                Lighting.AddLight((int)((float)p.Center.X / 16f), (int)((float)(p.position.Y + 8f) / 16f), 0, 0.973f, 0.44f);
            }
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
            recipe.AddIngredient(null, "VariaSuitBreastplate");
            recipe.AddIngredient(null, "KraidTissue", 24);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
