using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles
{
	public class ShaderTestProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.width = 76;
			Projectile.height = 76;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = TextureAssets.Projectile[Type].Value;
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			int y4 = frameHeight * Projectile.frame;
			Vector2 drawPos = new Vector2((int)(Projectile.Center.X - Main.screenPosition.X), (int)(Projectile.Center.Y - Main.screenPosition.Y + Projectile.gfxOffY));

			//Required before drawing the texture
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

			MiscShaderData shaderData = GameShaders.Misc["MetroidModDualTint"];
			shaderData.UseColor(new Color(120, 248, 248));
			shaderData.UseSecondaryColor(new Color(40, 96, 208));
			shaderData.UseImage0(TextureAssets.Projectile[Projectile.type]);
			shaderData.UseSaturation(0.6f);
			shaderData.UseOpacity(0.96f);

			DrawData data = new DrawData(tex, drawPos, new Rectangle?(new Rectangle(0, y4, tex.Width, frameHeight)), Color.White, Projectile.rotation, new Vector2(tex.Width / 2f, frameHeight / 2f), Projectile.scale, effects);
			shaderData.Apply(data); //Applies the shader to the drawData
			data.Draw(Main.spriteBatch);

			shaderData = GameShaders.Misc["MetroidModPaletteShader"];
			shaderData.UseColor(new Color(120, 248, 248)); //Primary color is the bright colors
			shaderData.UseSecondaryColor(new Color(40, 96, 208)); //Secondary is the dark colors
			shaderData.UseOpacity(1f); //Affects brightness of the 'core' (the white of the texture)
									   //Defaulting to 1f to keep the core bright
			shaderData.UseSaturation(0f); //Affects saturation of the 'core'
										  //0 to keep the core white instead of being the primary color
			shaderData.UseImage0(TextureAssets.Projectile[Projectile.type]);

			drawPos.Y += frameHeight;
			data = new DrawData(tex, drawPos, new Rectangle?(new Rectangle(0, y4, tex.Width, frameHeight)), Color.White, Projectile.rotation, new Vector2(tex.Width / 2f, frameHeight / 2f), Projectile.scale, effects);
			shaderData.Apply(data);
			data.Draw(Main.spriteBatch);

			//Required after drawing the texture
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

			return false;
		}
	}
}
