using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Body)]
    public class PhazonSuitBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phazon Suit Breastplate");
            Tooltip.SetDefault("5% increased ranged damage\n" +
             "Immunity to fire blocks\n" +
             "Immunity to chill and freeze effects\n" +
             "Immune to knockback\n" + 
             "+25 overheat capacity");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 7;
            item.value = 45000;
            item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.fireWalk = true;
            player.noKnockback = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 25;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == mod.ItemType("PhazonSuitHelmet") && body.type == mod.ItemType("PhazonSuitBreastplate") && legs.type == mod.ItemType("PhazonSuitGreaves"));
        }

        public override void UpdateArmorSet(Player p)
        {
            p.setBonus = "Press the Sense move key while moving near an enemy to dodge in that direction" + "\r\n"
                + "15% increased ranged damage" + "\r\n"
                + "Free movement in liquid" + "\r\n"
                + "Immune to lava damage for 7 seconds" + "\r\n"
                + "Negates fall damage" + "\r\n"
                + "Infinite breath" + "\r\n"
                + "35% decreased overheat use" + "\r\n"
                + "Immune to damage from standing on Phazon blocks"
                + "Enables Phazon Beam use";
            p.rangedDamage += 0.15f;
            p.ignoreWater = true;
            p.lavaMax += 420;
            p.noFallDmg = true;
            p.gills = true;
            MPlayer mp = p.GetModPlayer<MPlayer>(mod);
            mp.overheatCost -= 0.35f;
            mp.SenseMove(p);
            mp.visorGlow = true;
            if (!mp.ballstate)
            {
                Lighting.AddLight((int)(p.Center.X / 16f), (int)((p.position.Y + 8f) / 16f), 0, 0.973f, 0.44f);
            }
            mp.phazonImmune = true;
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
            recipe.AddIngredient(null, "GravitySuitBreastplate");
            recipe.AddIngredient(ItemID.SpectreBar, 25);
            recipe.AddIngredient(null, "PurePhazon", 10);
            recipe.AddIngredient(null, "EnergyTank");
            recipe.AddTile(null, "NovaWorkTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
