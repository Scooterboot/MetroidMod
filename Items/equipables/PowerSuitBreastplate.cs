using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Body)]
    public class PowerSuitBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Power Suit Breastplate");
            Tooltip.SetDefault("5% increased ranged damage\n" +
            "+5 overheat capacity");
		}

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 2;
            item.value = 9000;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == mod.ItemType("PowerSuitHelmet") && body.type == mod.ItemType("PowerSuitBreastplate") && legs.type == mod.ItemType("PowerSuitGreaves"));
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Hold the Sense move key and left/right while an enemy is moving towards you to dodge" + "\r\n" + "10% decreased overheat use" + "\r\n" + "Negates fall damage" + "\r\n" + "30% increased underwater breathing";
            player.noFallDmg = true;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.breathMult = 1.3f;
            mp.overheatCost -= 0.10f;
            mp.SenseMove(player);
            mp.visorGlow = true;
            if (!mp.ballstate)
            {
                Lighting.AddLight((int)((float)player.Center.X / 16f), (int)((float)(player.position.Y + 8f) / 16f), 0, 0.973f, 0.44f);
            }
        }

		public override void UpdateVanitySet(Player P)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);
			mp.isPowerSuit = true;
			mp.thrusters = true;
			if(Main.netMode != 2)
			{
				mp.thrusterTexture = mod.GetTexture("Gore/powerSuit_thrusters");
			}
			mp.visorGlowColor = new Color(0, 248, 112);
			if(P.velocity.Y != 0f && ((P.controlRight && P.direction == 1) || (P.controlLeft && P.direction == -1)) && mp.shineDirection == 0 && !mp.shineActive && !mp.ballstate)
			{
				mp.jet = true;
			}
			else if(mp.shineDirection == 0 || mp.shineDirection == 5)
			{
				mp.jet = false;
			}
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 15);
            recipe.AddIngredient(null, "ChoziteBreastplate");
            recipe.AddIngredient(null, "EnergyTank");
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
