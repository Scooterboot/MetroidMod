using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.GlobalItems;

namespace MetroidModPorted.Content.Items.Addons
{
	public class HyperBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hyper Beam");
			Tooltip.SetDefault("Power Beam Addon\n" +
			"Slot Type: Charge\n" +
			"Increases base damage from 14 to 35, and base overheat use from 4 to 7\n" +
			"Slightly decreases firerate\n" +
			"Affected by addons regardless of version\n" + 
			"Disables freeze and other debuff effects\n" +
			"'Da babeh'");
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 2500;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.HyperBeamTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.Meowmere, 1)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddIngredient(ItemID.FragmentStardust, 10)
				.AddIngredient(ItemID.FragmentSolar, 10)
				.AddIngredient(ItemID.FragmentNebula, 10)
				.AddIngredient(ItemID.FragmentVortex, 10)
				.AddTile(TileID.LunarCraftingStation)
				.Register();

			CreateRecipe(1)
				.AddIngredient(ItemID.LastPrism, 1)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddIngredient(ItemID.FragmentStardust, 10)
				.AddIngredient(ItemID.FragmentSolar, 10)
				.AddIngredient(ItemID.FragmentNebula, 10)
				.AddIngredient(ItemID.FragmentVortex, 10)
				.AddTile(TileID.LunarCraftingStation)
				.Register();

			CreateRecipe(1)
				.AddIngredient(ItemID.RainbowCrystalStaff, 1)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddIngredient(ItemID.FragmentStardust, 10)
				.AddIngredient(ItemID.FragmentSolar, 10)
				.AddIngredient(ItemID.FragmentNebula, 10)
				.AddIngredient(ItemID.FragmentVortex, 10)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
		public override void PostDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale )
		{
			Texture2D tex = ModContent.Request<Texture2D>("MetroidModPorted/Content/Items/Addons/HyperBeamAddonColors").Value;
			drawColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			sb.Draw(tex, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
		}
		public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI )
		{
			DrawColors(sb);//, Main.player[Item.owner]);
			lightColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			alphaColor = lightColor;
		}
		public void DrawColors(SpriteBatch sb)//, Player player)
		{
			Texture2D tex = ModContent.Request<Texture2D>("MetroidModPorted/Content/Items/Addons/HyperBeamAddonColors").Value;
			float rotation = Item.velocity.X * 0.2f;
			float num3 = 1f;
			float num4 = (float)(Item.height - tex.Height);
			float num5 = (float)(Item.width / 2 - tex.Width / 2);
			sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num5, Item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num4 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), num3, SpriteEffects.None, 0f);
		}
	}
}
