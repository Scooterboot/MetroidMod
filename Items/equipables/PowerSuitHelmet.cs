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
	public class PowerSuitHelmet : ModItem
	{
        public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
        {
            equips.Add(EquipType.Head);
            return true;
        }

        public override void SetDefaults()
        {
            item.name = "Power Suit Helmet";
            item.width = 18;
            item.height = 18;
            item.rare = 2;
            item.value = 6000;
            item.defense = 1;
            AddTooltip("5% increased ranged damage");
            AddTooltip("+5 overheat capacity");
            AddTooltip("Improved night vision");
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.nightVision = true;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 20);
            recipe.AddIngredient(ItemID.Topaz);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
