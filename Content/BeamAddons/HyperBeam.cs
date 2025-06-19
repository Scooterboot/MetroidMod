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
		int bd = 0;
		float dm = 0;
		int bs = 0;
		float sm = 0;
		#endregion

		public override Color PrimaryColor => Color.White;

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
		public override void PostDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D tex = ModContent.Request<Texture2D>(ItemTexture + "Rainbow").Value;
			drawColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			sb.Draw(tex, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
		}
		public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			DrawColors(sb);//, Main.player[Item.owner]);
			lightColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			alphaColor = lightColor;
		}
		public void DrawColors(SpriteBatch sb)//, Player player)
		{
			Texture2D tex = ModContent.Request<Texture2D>(ItemTexture + "Rainbow").Value;
			float rotation = Item.velocity.X * 0.2f;
			float num3 = 1f;
			float num4 = (float)(Item.height - tex.Height);
			float num5 = (float)(Item.width / 2 - tex.Width / 2);
			sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num5, Item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num4 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), num3, SpriteEffects.None, 0f);
		}
		#endregion
	}
}
