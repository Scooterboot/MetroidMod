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
	public class GravitySuitBreastplate : ModItem
	{
        public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
        {
            equips.Add(EquipType.Body);
            return true;
        }

        public override void SetDefaults()
        {
            item.name = "Gravity Suit Breastplate";
            item.width = 18;
            item.height = 18;
            item.rare = 4;
            item.value = 30000;
            item.defense = 10;
            AddTooltip("5% increased ranged damage");
            AddTooltip("Immune to fire blocks");
            AddTooltip("Immune to chill and freeze effects");
            AddTooltip("Immune to knockback");
            AddTooltip("+20 overheat capacity");
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.fireWalk = true;
            player.noKnockback = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 20;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == mod.ItemType("GravitySuitHelmet") && body.type == mod.ItemType("GravitySuitBreastplate") && legs.type == mod.ItemType("GravitySuitGreaves"));
        }

        public override void UpdateArmorSet(Player p)
        {
            p.setBonus = "10% increased ranged damage" + "\r\n"
                + "Free movement in liquid" + "\r\n"
                + "Immune to lava damage" + "\r\n"
                + "Default gravity in space" + "\r\n"
                + "Negates fall damage" + "\r\n"
                + "Infinite breath" + "\r\n"
                + "35% decreased overheat use";
            p.rangedDamage += 0.10f;
            p.ignoreWater = true;
            p.lavaImmune = true;
            p.gravity = 1;
            p.noFallDmg = true;
            p.gills = true;
            MPlayer mp = p.GetModPlayer<MPlayer>(mod);
            mp.overheatCost -= 0.35f;
            mp.enableSenseMove = true;
            mp.visorGlow = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "VariaSuitBreastplate");
            recipe.AddIngredient(null, "GravityGel", 20);
            recipe.AddIngredient(ItemID.Wire, 12);
            recipe.AddIngredient(ItemID.CursedFlame, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "VariaSuitBreastplate");
            recipe.AddIngredient(null, "GravityGel", 20);
            recipe.AddIngredient(ItemID.Wire, 12);
            recipe.AddIngredient(ItemID.Ichor, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
