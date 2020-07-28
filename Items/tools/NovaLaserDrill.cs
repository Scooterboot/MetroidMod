using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Items.tools
{
	public class NovaLaserDrill : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Laser Drill");
			//Tooltip.SetDefault("Capable of mining Phazon");
			Tooltip.SetDefault("This is the only tool that can mine Phazon");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 16;
			item.scale = 0.8f;
			item.useStyle = 5;
			item.useAnimation = 4;
			item.useTime = 4;
			item.pick = 200;//215;
			item.damage = 35;
			item.knockBack = 3f;
			item.value = 15000;
			item.rare = 2;
			item.shoot = mod.ProjectileType("NovaLaserDrillShot");
			item.UseSound = SoundID.Item23;
			item.tileBoost = 10;
			item.melee = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.channel = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.Picksaw);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddIngredient(ItemID.CursedFlame, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.Picksaw);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddIngredient(ItemID.Ichor, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		string altTexture => this.Texture + "_Alt";
		public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
			Texture2D tex = Main.itemTexture[item.type];
			if(MetroidMod.UseAltWeaponTextures)
			{
				mi.itemTexture = ModContent.GetTexture(altTexture);
			}
			else
			{
				mi.itemTexture = Main.itemTexture[item.type];
			}
			if(mi.itemTexture != null)
			{
				tex = mi.itemTexture;
			}
			float num5 = (float)(item.height - tex.Height);
			float num6 = (float)(item.width / 2 - tex.Width / 2);
			sb.Draw(tex, new Vector2(item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num6, item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num5 + 2f),
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), alphaColor, rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
			Texture2D tex = Main.itemTexture[item.type];
			if(MetroidMod.UseAltWeaponTextures)
			{
				mi.itemTexture = ModContent.GetTexture(altTexture);
			}
			else
			{
				mi.itemTexture = Main.itemTexture[item.type];
			}
			if(mi.itemTexture != null)
			{
				tex = mi.itemTexture;
			}
			sb.Draw(tex, position, new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		
		public override void HoldItem(Player player)
		{
			bool flag13 = player.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX && (player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX && player.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY && (player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY;
			if (player.noBuilding)
			{
				flag13 = false;
			}
			Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
			if (flag13 && tile != null && tile.active() && !player.mouseInterface && (tile.type == mod.TileType("PhazonTile") || tile.type == mod.TileType("PhazonCore")))
			{
				item.pick = 1000;
			}
			else
			{
				item.pick = 200;
			}
		}
	}
}