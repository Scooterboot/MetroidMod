using System;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.BeamAddons
{
    class HyperBeam : ModBeamAddon
    {
		//As this is the first VIB addon, it will serve as an example.

		#region Stat values
		int bd = 200;
		float dm = 0;
		int bs = 0;
		float sm = 0;
		#endregion

		public override Color PrimaryColor => Color.White;

		public override float CoreSaturation => 0.5f;

		public override int ShotDust => DustID.RainbowTorch;

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Primary;
			VIB = true;

			BaseDamage = bd;
			DamageMult = dm;
			BaseSpeed = bs;
			SpeedMult = sm;
		}

		public override void SetItemDefaults(Item item)
		{

			item.rare = ItemRarityID.Expert;
		}

		#region taste the rainbow
		//All the code for the rainbow effects on the sprite
		public override void PostDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D tex = ModContent.Request<Texture2D>(ItemTexture + "Rainbow").Value;
			drawColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			sb.Draw(tex, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
		}
		public override void PostDrawInWorld(Item item, SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			DrawColors(item, sb);//, Main.player[Item.owner]);
			lightColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			alphaColor = lightColor;
		}
		public void DrawColors(Item item, SpriteBatch sb)//, Player player)
		{
			//MetroidMod.Instance.Logger.Debug("CONSOLESPAM");
			Texture2D tex = ModContent.Request<Texture2D>(ItemTexture + "Rainbow").Value;
			float rotation = item.velocity.X * 0.2f;
			float num3 = 1f;
			float num4 = (float)(item.height - tex.Height);
			float num5 = (float)(item.width / 2 - tex.Width / 2);
			sb.Draw(tex, new Vector2(item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num5, item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num4 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), num3, SpriteEffects.None, 0f);
		}

		public override void PostDrawTile(int i, int j, SpriteBatch sb)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			sb.Draw(ModContent.Request<Texture2D>(TileTexture + "Rainbow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
		#endregion
	}
}
