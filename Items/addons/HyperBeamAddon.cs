using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
    public class HyperBeamAddon : ModItem
    {
		public override void SetDefaults()
		{
			item.name = "Hyper Beam";
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.toolTip = "Power Beam Addon\n" +
			"Slot Type: Charge\n" +
			"'Mother, time to go!'\n" + 
			"Shots ignore 50% of enemy defense\n" + 
			"Primary A and B addons will work together\n" + 
			"Only activates when the Secondary, Utility, and Primary A and B slots are in use";
			item.value = 2500;
			item.rare = 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient("Hallowed Bar", 3);
            recipe.AddIngredient("Soul of Might", 20);
			recipe.AddIngredient("Soul of Sight", 20);
			recipe.AddIngredient("Soul of Fright", 20);
            recipe.AddIngredient("Rainbow Gun");
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void PostDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale )
		{
			//DrawColors(sb, Main.localPlayer);
			drawColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
		}
		public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI )
		{
			//DrawColors(sb, Main.localPlayer);
			lightColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			alphaColor = lightColor;
		}
	/*	public void DrawColors(SpriteBatch sb, Player player)
		{
			Texture2D tex = mod.GetTexture("Items/addons/HyperBeamAddonColors");
			float rotation = item.velocity.X * 0.2f;
			float num3 = 1f;
			float num4 = (float)(item.height - tex.Height);
			float num5 = (float)(item.width / 2 - tex.Width / 2);
			MPlayer mp = player.GetSubClass<MPlayer>();
			sb.Draw(tex, new Vector2(item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num5, item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num4 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), new Color(mp.r, mp.g, mp.b), rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), num3, SpriteEffects.None, 0f);
		}*/
	}
}