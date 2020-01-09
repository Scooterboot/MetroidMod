using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Body)]
    public class VortexGravitySuitBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vortex Gravity Suit Breastplate");
            Tooltip.SetDefault("5% increased ranged damage\n" +
             "Immune to fire blocks\n" +
             "Immune to chill and freeze effects\n" +
             "Immune to knockback\n" +
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
            player.noKnockback = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            MPlayer mp = player.GetModPlayer<MPlayer>();
            mp.maxOverheat += 34;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == mod.ItemType("VortexGravitySuitHelmet") && body.type == mod.ItemType("VortexGravitySuitBreastplate") && legs.type == mod.ItemType("VortexGravitySuitGreaves"));
        }

        public override void UpdateArmorSet(Player p)
        {
            p.setBonus = "Hold the Sense move key and left/right while an enemy is moving towards you to dodge" + "\r\n"
                + "20% increased ranged damage" + "\r\n"
                + "Free movement in liquid" + "\r\n"
                + "Default gravity in space" + "\r\n"
                + "Immune to lava damage" + "\r\n"
                + "Immune to Distorted and Amplified Gravity" + "\r\n"
                + "Negates fall damage" + "\r\n"
                + "Infinite breath" + "\r\n"
                + "40% decreased overheat use";
            p.rangedDamage += 0.20f;
            p.ignoreWater = true;
            p.gravity = Player.defaultGravity;
            p.lavaImmune = true;
            p.noFallDmg = true;
            p.gills = true;
			p.buffImmune[BuffID.VortexDebuff] = true;
			p.buffImmune[mod.BuffType("GravityDebuff")] = true;
            MPlayer mp = p.GetModPlayer<MPlayer>();
            mp.overheatCost -= 0.40f;
            mp.SenseMove(p);
            mp.visorGlow = true;
        }

        public override void UpdateVanitySet(Player P)
        {
            MPlayer mp = P.GetModPlayer<MPlayer>();
            mp.isPowerSuit = true;
            mp.thrusters = true;
            if (Main.netMode != 2)
            {
                mp.thrusterTexture = mod.GetTexture("Gore/powerSuit_thrusters");
            }
            mp.visorGlowColor = new Color(67, 255, 255);
            if (P.velocity.Y != 0f && ((P.controlRight && P.direction == 1) || (P.controlLeft && P.direction == -1)) && mp.shineDirection == 0 && !mp.shineActive && !mp.ballstate)
            {
                mp.jet = true;
            }
            else if (mp.shineDirection == 0 || mp.shineDirection == 5)
            {
                mp.jet = false;
            }
        }
		
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TerraGravitySuitBreastplate");
            recipe.AddIngredient(ItemID.LunarBar, 20);
            recipe.AddIngredient(ItemID.FragmentVortex, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
