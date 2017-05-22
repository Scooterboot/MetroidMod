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
	public class VariaSuitBreastplate : ModItem
	{
        public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
        {
            equips.Add(EquipType.Body);
            return true;
        }

        public override void SetDefaults()
        {
            item.name = "Varia Suit Breastplate";
            item.width = 18;
            item.height = 18;
            item.rare = 3;
            item.value = 9000;
            item.defense = 9;
            AddTooltip("5% increased ranged damage");
            AddTooltip("Immunity to fire blocks");
            AddTooltip("Immunity to chill and freeze effects");
            AddTooltip("+10 overheat capacity");
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.fireWalk = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == mod.ItemType("VariaSuitHelmet") && body.type == mod.ItemType("VariaSuitBreastplate") && legs.type == mod.ItemType("VariaSuitGreaves"));
        }

        public override void UpdateArmorSet(Player p)
        {
            p.setBonus = "Press the Sense Move key while moving near an enemy to dodge in that direction" + "\r\n" + "5% increased ranged damage" + "\r\n" + "20% decreased overheat use" + "\r\n" + "Negates fall damage" + "\r\n" + "70% increased underwater breathing";
            p.rangedDamage += 0.05f;
            p.breathMax = (int)(p.breathMax * 1.7f);
            p.noFallDmg = true;
            MPlayer mp = p.GetModPlayer<MPlayer>(mod);
            mp.overheatCost -= 0.20f;
             mp.SenseMove(p);
            mp.visorGlow = true;
            if(!mp.ballstate)
			{
				Lighting.AddLight((int)((float)p.Center.X/16f), (int)((float)(p.position.Y+8f)/16f), 0, 0.973f, 0.44f);
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
            recipe.AddIngredient(null, "PowerSuitBreastplate");
            recipe.AddIngredient(ItemID.HellstoneBar, 30);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
