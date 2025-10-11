using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Dusts
{
	public class FlashTrail : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.frame = new Rectangle(0, 0, 6, 6);
		}

		public override bool Update(Dust dust)
		{
			dust.scale -= 0.05f;
			if (dust.scale <= 0)
			{
				dust.active = false;
			}
			return false;
		}
		/*
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return Color.White;
		}
		*/
		public override bool PreDraw(Dust dust)
		{
			Texture2D tex = (Texture2D)ModContent.Request<Texture2D>($"{Texture}");
			Vector2 drawOrigin = new Vector2(tex.Width * 0.5f, tex.Height * 0.5f);
			Color color = new Color((dust.color.R + 255) / 2, (dust.color.G + 255) / 2, (dust.color.B + 255) / 2);
			color *= (255 - dust.alpha) / 255f;
			Main.EntitySpriteDraw(tex, dust.position - Main.screenPosition, new Rectangle?(dust.frame), color, dust.rotation, drawOrigin, dust.scale, SpriteEffects.None, 0);

			return false;
		}
	}
}
