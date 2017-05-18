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
	public class PowerSuitBreastplate : ModItem
	{
        public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
        {
            equips.Add(EquipType.Body);
            return true;
        }

        public override void SetDefaults()
        {
            item.name = "Power Suit Breastplate";
            item.width = 18;
            item.height = 18;
            item.rare = 2;
            item.value = 9000;
            item.defense = 4;
            AddTooltip("5% increased ranged damage");
            AddTooltip("+5 overheat capacity");
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == mod.ItemType("Power Suit Helmet") && body.type == mod.ItemType("PowerSuitBreastplate") && legs.type == mod.ItemType("PowerSuitGreaves"));
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "10% decreased overheat use" + "\r\n" + "Negates fall damage" + "\r\n" + "30% increased underwater breathing";
            player.breathMax = (int)(player.breathMax * 1.3f);
            player.noFallDmg = true;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			mp.overheatCost -= 0.10f;
			mp.enableSenseMove = true;
			mp.visorGlow = true;
        }
		
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 30);
            recipe.AddIngredient(ItemID.Topaz, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
