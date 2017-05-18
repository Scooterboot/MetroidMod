using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod
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
            item.defense = 6;
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
            p.setBonus = "5% increased ranged damage" + "\r\n" + "20% decreased overheat use" + "\r\n" + "Negates fall damage" + "\r\n" + "70% increased underwater breathing";
            p.rangedDamage += 0.05f;
            p.breathMax = (int)(p.breathMax * 1.7f);
            p.noFallDmg = true;
            MPlayer mp = p.GetModPlayer<MPlayer>(mod);
            mp.overheatCost -= 0.20f;
            mp.enableSenseMove = true;
            mp.visorGlow = true;
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