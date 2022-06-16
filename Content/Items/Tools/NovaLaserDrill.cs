using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.Tools
{
	public class NovaLaserDrill : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Laser Drill");
			//Tooltip.SetDefault("Capable of mining Phazon");
			Tooltip.SetDefault("This is the only tool that can mine Phazon");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 16;
			Item.scale = 0.8f;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 4;
			Item.useTime = 4;
			Item.pick = 200;//215;
			Item.damage = 35;
			Item.knockBack = 3f;
			Item.value = 15000;
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<Projectiles.NovaLaserDrillShot>();
			Item.UseSound = SoundID.Item23;
			Item.tileBoost = 10;
			Item.DamageType = DamageClass.Melee;//Item.melee = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.ChlorophyteBar, 12)
				.AddIngredient(ItemID.CursedFlame, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.Picksaw);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddIngredient(ItemID.CursedFlame, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();*/

			CreateRecipe(1)
				.AddIngredient(ItemID.ChlorophyteBar, 12)
				.AddIngredient(ItemID.Ichor, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			/*recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.Picksaw);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddIngredient(ItemID.Ichor, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
		
		string altTexture => Texture + "_Alt";
		public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			MGlobalItem mi = Item.GetGlobalItem<MGlobalItem>();
			Texture2D tex = Terraria.GameContent.TextureAssets.Item[Type].Value;//Main.itemTexture[Item.type];
			if(MetroidMod.UseAltWeaponTextures)
			{
				mi.itemTexture = ModContent.Request<Texture2D>(altTexture).Value;//GetTexture(altTexture);
			}
			else
			{
				mi.itemTexture = Terraria.GameContent.TextureAssets.Item[Type].Value;//Main.itemTexture[Item.type];
			}
			if(mi.itemTexture != null)
			{
				tex = mi.itemTexture;
			}
			float num5 = (float)(Item.height - tex.Height);
			float num6 = (float)(Item.width / 2 - tex.Width / 2);
			sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num6, Item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num5 + 2f),
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), alphaColor, rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color ItemColor, Vector2 origin, float scale)
		{
			MGlobalItem mi = Item.GetGlobalItem<MGlobalItem>();
			Texture2D tex = Terraria.GameContent.TextureAssets.Item[Type].Value;
			if(MetroidMod.UseAltWeaponTextures)
			{
				mi.itemTexture = ModContent.Request<Texture2D>(altTexture).Value;
			}
			else
			{
				mi.itemTexture = Terraria.GameContent.TextureAssets.Item[Type].Value;
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
			bool flag13 = player.position.X / 16f - (float)Player.tileRangeX - (float)Item.tileBoost <= (float)Player.tileTargetX && (player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)Item.tileBoost - 1f >= (float)Player.tileTargetX && player.position.Y / 16f - (float)Player.tileRangeY - (float)Item.tileBoost <= (float)Player.tileTargetY && (player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)Item.tileBoost - 2f >= (float)Player.tileTargetY;
			if (player.noBuilding)
			{
				flag13 = false;
			}
			Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
			if (flag13 && tile != null && tile.HasTile && !player.mouseInterface && (tile.TileType == ModContent.TileType<Content.Tiles.PhazonTile>() || tile.TileType == ModContent.TileType<Content.Tiles.PhazonCore>()))
			{
				Item.pick = 1000;
			}
			else
			{
				Item.pick = 200;
			}
		}
	}
}
