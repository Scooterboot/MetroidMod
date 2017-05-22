using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
	public class MorphBallV2 : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Morph Ball V2";
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.toolTip = "Normal Morph Ball abilities\n" + 
			 "Upgraded drill feature can mine Hellstone\n" + 
			"While active:\n" + 
			 "-Press Spider Ball Key to activate Spider Ball\n" + 
			 "-Press Power Bomb Key to set off a Power Bomb (20 second cooldown)\n" + 
			"-Hold Boost Ball Key to charge a Boost Ball";
			item.value = 60000;
			item.rare = 4;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MorphBall");
			recipe.AddIngredient("Adamantite Bar", 3);
			recipe.AddIngredient("Wire", 35);
			recipe.AddIngredient("Soul of Fright", 20);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MorphBall");
			recipe.AddIngredient("Titanium Bar", 3);
			recipe.AddIngredient("Wire", 35);
			recipe.AddIngredient("Soul of Fright", 20);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			mp.morphBall = true;
			mp.MorphBallBasic(player);
			mp.SpiderBall(player);
			mp.Drill(player,65);
			mp.PowerBomb(player);
			mp.BoostBall(player);
		}
		public override Color? GetAlpha(Color lightColor)
        {
			Player player = Main.player[item.owner];
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            return mp.currentMorphColor;
        }
		public override void PostDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale )
		{
			Player player = Main.player[item.owner];
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			itemColor = mp.currentMorphColor;
			drawColor = mp.currentMorphColor2;
			Texture2D tex = mod.GetTexture("Items/equipables/MorphBallV2_Lights");
			sb.Draw(tex, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			sb.Draw(mod.GetTexture("Gore/Spiderball"), position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
		}
public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI )
		{
			DrawLights(sb, Main.player[item.owner]);
			DrawSpider(sb, Main.player[item.owner]);
			MPlayer mp = Main.player[item.owner].GetModPlayer<MPlayer>(mod);
			lightColor = mp.currentMorphColor2;
			alphaColor = lightColor;
		}
		public void DrawLights(SpriteBatch sb, Player player)
		{
			Texture2D tex = mod.GetTexture("Items/equipables/MorphBallV2_Lights");
			float rotation = item.velocity.X * 0.2f;
			float num3 = 1f;
			float num4 = (float)(item.height - tex.Height);
			float num5 = (float)(item.width / 2 - tex.Width / 2);
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			sb.Draw(tex, new Vector2(item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num5, item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num4 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), mp.currentMorphColor2, rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), num3, SpriteEffects.None, 0f);
		}
		public void DrawSpider(SpriteBatch sb, Player player)
		{
			Texture2D tex = mod.GetTexture("Gore/Spiderball");
			float rotation = item.velocity.X * 0.2f;
			float num3 = 1f;
			float num4 = (float)(item.height - tex.Height);
			float num5 = (float)(item.width / 2 - tex.Width / 2);
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			sb.Draw(tex, new Vector2(item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num5, item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num4 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), mp.currentMorphColor2, rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), num3, SpriteEffects.None, 0f);
		}
			public override bool CanEquipAccessory(Player player, int slot)
		{
			 for (int k = 3; k < 8 + player.extraAccessorySlots; k++)
            {
                if (player.armor[k].type == mod.ItemType("MorphBall"))
                {
                    return false;
                }
            }
return true;
		
		}
	}
}
