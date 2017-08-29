using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
	public class MorphBall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball");
			Tooltip.SetDefault("Press the morph ball key to roll into a ball\n" + 
			"While active:\n" + 
			"-Left Click to initiate a drill feature\n" + 
			"-Right Click to set off a bomb (Deals 10 damage)\n" + 
			"Morph Ball's colors are based on your shirt and undershirt colors");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 40000;
			item.rare = 2;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 20);
			recipe.AddIngredient(ItemID.Diamond, 3);
			recipe.AddIngredient(ItemID.GoldBar, 5);
			recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 20);
			recipe.AddIngredient(ItemID.Diamond, 3);
			recipe.AddIngredient(ItemID.PlatinumBar, 5);
			recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			mp.morphBall = true;
			mp.MorphBallBasic(player);
			mp.Drill(player,35);
		}
			public override Color? GetAlpha(Color lightColor)
        {
			Player player = Main.player[item.owner];
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            return mp.morphItemColor;
        }
		public override void PostDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale )
		{
			Player player = Main.player[item.owner];
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			itemColor = mp.morphItemColor;
			drawColor = mp.morphColorLights;
			Texture2D tex = mod.GetTexture("Items/equipables/MorphBall_Lights");
			sb.Draw(tex, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
		}
public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI )
		{
			DrawLights(sb, Main.player[item.owner]);
			MPlayer mp = Main.player[item.owner].GetModPlayer<MPlayer>(mod);
			lightColor = mp.morphColorLights;
			alphaColor = lightColor;
		}
		public void DrawLights(SpriteBatch sb, Player player)
		{
			Texture2D tex = mod.GetTexture("Items/equipables/MorphBall_Lights");
			float rotation = item.velocity.X * 0.2f;
			float num3 = 1f;
			float num4 = (float)(item.height - tex.Height);
			float num5 = (float)(item.width / 2 - tex.Width / 2);
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			sb.Draw(tex, new Vector2(item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num5, item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num4 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), mp.morphColorLights, rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), num3, SpriteEffects.None, 0f);
		}
		public override bool CanEquipAccessory(Player player, int slot)
		{
			 for (int k = 3; k < 8 + player.extraAccessorySlots; k++)
            {
                if (player.armor[k].type == mod.ItemType("MorphBallV2"))
                {
                    return false;
                }
            }
return true;
		
		}
	}
}
