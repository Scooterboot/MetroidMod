using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Body)]
    public class PEDSuitBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PED Suit Breastplate");
            Tooltip.SetDefault("5% increased ranged damage\n" +
             "Immunity to fire blocks\n" +
             "Immunity to chill and freeze effects\n" +
             "+20 overheat capacity");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 5;
            item.value = 25000;
            item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.fireWalk = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 20;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == mod.ItemType("PEDSuitHelmet") && body.type == mod.ItemType("PEDSuitBreastplate") && legs.type == mod.ItemType("PEDSuitGreaves"));
        }

        public override void UpdateArmorSet(Player p)
        {
            p.setBonus = "Press the Sense Move key while moving near an enemy to dodge in that direction" + "\r\n" +
                "Press the Hypermode key to activate Hypermode (take 100 damage to gain +50% damage for 20 seconds, 120 s cooldown)" + "\r\n" +
                "Slightly increased health regen when standing on Phazon" + "\r\n" +
                "10% increased ranged damage" + "\r\n" +
                "30% decreased overheat use" + "\r\n" +
                "Negates fall damage" + "\r\n" +
                "Infinite breath";
            p.rangedDamage += 0.1f;
            p.gills = true;
            p.noFallDmg = true;
            MPlayer mp = p.GetModPlayer<MPlayer>(mod);
            mp.phazonImmune = true;
            mp.phazonRegen = 2;
            mp.overheatCost -= 0.30f;
            mp.SenseMove(p);
            mp.visorGlow = true;
            if (!mp.ballstate)
            {
                Lighting.AddLight((int)(p.Center.X / 16f), (int)((p.position.Y + 8f) / 16f), 0, 0.973f, 0.44f);
            }
            //code to activate Hypermode goes here; might need to add a Hypermode hook to MPlayer like Sense Move
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

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "VariaSuitV2Breastplate");
            //Phazon biome materials go here
            recipe.AddIngredient(null, "EnergyTank");
            //recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
    }
}
