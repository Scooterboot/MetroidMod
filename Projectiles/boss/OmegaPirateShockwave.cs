using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using MetroidMod.NPCs.OmegaPirate;

namespace MetroidMod.Projectiles.boss
{
	public class OmegaPirateShockwave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omega Pirate");
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.timeLeft = 1200;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.width = 30;
			projectile.height = 0;
			projectile.scale = 1f;
			Main.projFrames[projectile.type] = 2;
		}
		
		int lightningFrame = 0;
		
		float scaleY = 0f;

		float alpha = 0.25f;
		public override void AI()
		{
			projectile.frame++;
			if(projectile.frame > 1)
			{
				projectile.frame = 0;
			}
			
			/*lightningFrame++;
			if(lightningFrame > 7)
			{
				lightningFrame = 0;
			}*/
			lightningFrame = Main.rand.Next(8);
			
			if(projectile.ai[1] == 0)
			{
				scaleY = Math.Min(scaleY+(projectile.ai[0]/5f),projectile.ai[0]);
				if(scaleY >= projectile.ai[0])
				{
					projectile.ai[1] = 1;
				}
			}
			if(projectile.ai[1] > 0)
			{
				scaleY = Math.Max(scaleY-((projectile.ai[0]+(projectile.ai[1]/10))/100),0f);
				if(scaleY < 0.1f)
				{
					projectile.Kill();
				}
			}
			
			projectile.position.X += projectile.width / 2f;
			projectile.position.Y += projectile.height / 2f;
			projectile.width = 30;
			projectile.height = (int)((float)(500 * scaleY));
			projectile.position.X -= projectile.width / 2f;
			projectile.position.Y -= projectile.height / 2f;
			
			if(projectile.ai[0] >= 1.5f)
			{
				projectile.localAI[0] = -1;
			}
			
			if(projectile.ai[1] > 0 && projectile.ai[0] > 0.1f)
			{
				projectile.ai[1]++;
				if(projectile.ai[1] == 2)
				{
					int shock1 = Projectile.NewProjectile(projectile.Center.X+(30f*projectile.spriteDirection),projectile.Center.Y,0f,0f,mod.ProjectileType("OmegaPirateShockwave"),projectile.damage,8f);
					if(projectile.localAI[0] > 0)
					{
						Main.projectile[shock1].ai[0] = projectile.ai[0] + 0.5f;
					}
					else
					{
						Main.projectile[shock1].ai[0] = projectile.ai[0] - 0.06f;
					}
					Main.projectile[shock1].localAI[0] = projectile.localAI[0];
					Main.projectile[shock1].localAI[1] = projectile.localAI[1];
					Main.projectile[shock1].spriteDirection = projectile.spriteDirection;
				}
			}
			
			Color dustColor = Color.Lerp(OmegaPirate.minGlowColor,OmegaPirate.maxGlowColor,projectile.localAI[1]);
			//for(int i = 0; i < 4; i++)
			//{
				int dust1 = Dust.NewDust(new Vector2(projectile.position.X-4,projectile.Center.Y-4), projectile.width+4, 5, 63, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, dustColor, 4f);
				Main.dust[dust1].noGravity = true;
				Main.dust[dust1].noLight = true;
			//}
			for(int i = 0; i < 3; i++)
			{
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 63, 0f, 0f, 100, dustColor, (1f+i)*2f);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].noLight = true;
			}
		}

		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Color color = Color.Lerp(OmegaPirate.minGlowColor,OmegaPirate.maxGlowColor,projectile.localAI[1]);
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Main.projectileTexture[projectile.type];
			int num108 = tex.Width / Main.projFrames[projectile.type];
			int x4 = num108 * projectile.frame;
			sb.Draw(tex, new Vector2((float)((int)(projectile.Center.X - Main.screenPosition.X)), (float)((int)(projectile.Center.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(x4, 0, num108, tex.Height)), projectile.GetAlpha(Color.White)*alpha, projectile.rotation, new Vector2((float)num108/2f, (float)tex.Height), new Vector2(1f,scaleY)*projectile.scale, effects, 0f);
			
			Texture2D tex2 = mod.GetTexture("Projectiles/boss/OmegaPirateShockwaveLightning");
			int num2 = tex2.Width / 8;
			int x2 = num2 * lightningFrame;
			int height = (int)((float)Math.Min(tex2.Height*scaleY,tex2.Height));
			sb.Draw(tex2, new Vector2((float)((int)(projectile.Center.X - Main.screenPosition.X)), (float)((int)(projectile.Center.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(x2, 0, num2, height)), color, projectile.rotation, new Vector2((float)num2/2f, (float)height), 0.5f*Math.Max(scaleY,1f)*projectile.scale, effects, 0f);
			
			effects |= SpriteEffects.FlipVertically;
			sb.Draw(tex, new Vector2((float)((int)(projectile.Center.X - Main.screenPosition.X)), (float)((int)(projectile.Center.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(x4, 0, num108, tex.Height)), projectile.GetAlpha(Color.White)*alpha, projectile.rotation, new Vector2((float)num108/2f, 0f), new Vector2(1f,scaleY)*projectile.scale, effects, 0f);
			sb.Draw(tex2, new Vector2((float)((int)(projectile.Center.X - Main.screenPosition.X)), (float)((int)(projectile.Center.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(x2, 0, num2, height)), color, projectile.rotation, new Vector2((float)num2/2f, 0f), 0.5f*Math.Max(scaleY,1f)*projectile.scale, effects, 0f);
			return false;
		}
	}
}