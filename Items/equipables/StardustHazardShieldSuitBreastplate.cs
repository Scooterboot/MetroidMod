using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Body)]
    public class StardustHazardShieldSuitBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stardust Hazard Shield Suit Breastplate");
            Tooltip.SetDefault("5% increased ranged damage\n" +
             "Immune to fire blocks\n" +
             "Immune to chill and freeze effects\n" +
             "+34 overheat capacity");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 10;
            item.value = 60000;
            item.defense = 22;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.fireWalk = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 34;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == mod.ItemType("StardustHazardShieldSuitHelmet") && body.type == mod.ItemType("StardustHazardShieldSuitBreastplate") && legs.type == mod.ItemType("StardustHazardShieldSuitGreaves"));
        }

        public override void UpdateArmorSet(Player p)
        {
            p.setBonus = "Hold the Sense move key and left/right while an enemy is moving towards you to dodge" + "\r\n"
                + "20% increased ranged damage" + "\r\n"
                + "Press the Hypermode key to activate Hypermode (take 100 damage to gain +50% damage for 20 seconds, 120s cooldown)" + "\r\n"
                + "Greatly increased health regen when standing on Phazon" + "\r\n"
                + "Negates fall damage" + "\r\n"
                + "Infinite breath" + "\r\n"
                + "40% decreased overheat use" + "\r\n"
                + "Debuffs tick down 3 times as fast";
            p.rangedDamage += 0.20f;
            p.gills = true;
            p.noFallDmg = true;
            MPlayer mp = p.GetModPlayer<MPlayer>(mod);
            mp.phazonImmune = true;
            mp.phazonRegen = 8;
            mp.overheatCost -= 0.40f;
            mp.SenseMove(p);
            mp.visorGlow = true;
            mp.hazardShield = true;
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
            recipe.AddIngredient(null, "HazardShieldSuitBreastplate");
            recipe.AddIngredient(ItemID.LunarBar, 20);
            recipe.AddIngredient(ItemID.FragmentStardust, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        */
    }
}
