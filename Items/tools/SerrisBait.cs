using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace MetroidMod.Items.tools
{
    public class SerrisBait : ModItem
    {
        public override void SetDefaults()
		{
			item.name = "Serris Bait";
			item.maxStack = 20;
			item.consumable = true;
			item.width = 12;
			item.height = 12;
			item.toolTip = "Summons Serris";
			item.useTime = 45;
			item.useAnimation = 45;
			item.useStyle = 4;
			item.noMelee = true;
			item.value = 1000;
			item.rare = 7;

		}
        public override void AddRecipes()
		{
				ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(66, 20);
			recipe.AddIngredient("Rotten Chunk", 13);
            recipe.AddIngredient("Worm", 5);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
            recipe = new ModRecipe(mod);
			recipe.AddIngredient(66, 20);
			recipe.AddIngredient("Vertebrae", 13);
            recipe.AddIngredient("Worm", 5);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(mod.NPCType("Serrus_Head"));
		}
		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Serris_Head"));
            if (Main.netMode == 2)
            {
					NetMessage.SendData(23, -1, -1, "", mod.NPCType("Serris_Head"), 0.0f, 0.0f, 0.0f, 0);
            }
            if(Main.netMode != 2)
            { 
                Main.PlaySound(15,(int)player.position.X,(int)player.position.Y,0);
            }
			return true;
		}
	}
}