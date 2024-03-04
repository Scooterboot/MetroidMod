using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace MetroidMod.Content.Projectiles.missilecombo
{
	public class StardustComboGuardian : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Guardian");
			Main.projFrames[Projectile.type] = 12;
		}
		public override void SetDefaults()
		{
			//Projectile.netImportant = true;
			Projectile.width = 50;
			Projectile.height = 80;
			Projectile.aiStyle = -1;//120;
			Projectile.penetrate = -1;
			//Projectile.timeLeft *= 5;
			Projectile.timeLeft = 1200;
			//Projectile.minion = true;
			Projectile.friendly = true;
			//Projectile.minionSlots = 0f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			//Projectile.netImportant = true;
			Projectile.alpha = 255;
		}
		
		int damage = 0;
		public override void AI()
		{
			Projectile P = Projectile;
			
			if(!Main.projectile[(int)P.ai[0]].active)
			{
				P.Kill();
				return;
			}
			
			if(damage == 0)
			{
				damage = Projectile.damage;
				Projectile.damage = 0;
			}
			
			float distance = 425f;
			
			Lighting.AddLight(P.Center, 0.9f, 0.9f, 0.7f);
			if (P.alpha == 255)
			{
				P.alpha = 0;
				for (int i = 0; i < 30; i++)
				{
					int dust = Dust.NewDust(P.position, P.width, P.height, 135, 0f, 0f, 200, default(Color), 1.7f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 3f;
					dust = Dust.NewDust(P.position, P.width, P.height, 135, 0f, 0f, 100, default(Color), 1f);
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].fadeIn = 2.5f;
				}
			}
			if (P.localAI[0] > 0f)
			{
				P.localAI[0] -= 1f;
			}
			
			if(P.ai[1] == 0f)
			{
				int target = -1;
				for (int i = 0; i < 200; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy(P, false))
					{
						//if (P.Distance(npc.Center) < distance)
						if(Main.projectile[(int)P.ai[0]].Distance(npc.Center) < distance)
						{
							target = i;
						}
					}
				}
				if (target != -1)
				{
					NPC npc = Main.npc[target];
					P.direction = (P.spriteDirection = (npc.Center.X > P.Center.X).ToDirectionInt());
					float xDiff = Math.Abs(npc.Center.X - P.Center.X);
					float yDiff = Math.Abs(npc.Center.Y - P.Bottom.Y);
					float yDir = (float)(npc.Center.Y > P.Bottom.Y).ToDirectionInt();
					if (xDiff > 20f)
					{
						P.velocity.X = P.velocity.X + 0.1f * (float)P.direction;
					}
					else
					{
						P.velocity.X = P.velocity.X * 0.7f;
					}
					if (yDiff > 10f)
					{
						P.velocity.Y = P.velocity.Y + 0.1f * yDir;
					}
					else
					{
						P.velocity.Y = P.velocity.Y * 0.7f;
					}
					if (P.localAI[0] == 0f && P.owner == Main.myPlayer && xDiff < 200f)
					{
						P.localAI[1] = 0f;
						P.ai[1] = 1f;
						P.netUpdate = true;
						P.localAI[0] = 90f;
					}
				}
				else
				{
					P.velocity *= 0.8f;
				}
				
				P.frameCounter++;
				if (P.frameCounter >= 9)
				{
					P.frameCounter = 0;
					P.frame++;
					if (P.frame >= Main.projFrames[P.type] - 4)
					{
						P.frame = 0;
					}
				}
			}
			else if(P.ai[1] == 1f)
			{
				P.velocity.X = P.velocity.X * 0.9f;
				P.localAI[1] += 1f;
				if (P.localAI[1] == 10f && P.owner == Main.myPlayer)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), P.Center.X, P.Center.Y, 0f, 0f, ProjectileID.StardustGuardianExplosion, damage, 6f, P.owner, 0f, 5f);
				}
				if (P.localAI[1] >= 20f)
				{
					P.localAI[1] = 0f;
					P.ai[1] = 0f;
					P.netUpdate = true;
				}
				if (P.frame < Main.projFrames[P.type] - 4)
				{
					P.frame = Main.projFrames[P.type] - 1;
					P.frameCounter = 0;
				}
				
				P.frameCounter++;
				if (P.frameCounter >= 5)
				{
					P.frameCounter = 0;
					P.frame--;
					if (P.frame < Main.projFrames[P.type] - 5)
					{
						P.frame = Main.projFrames[P.type] - 1;
					}
				}
			}
		}
		
		public override void Kill(int timeLeft)
		{
			Projectile P = Projectile;
			for (int i = 0; i < 30; i++)
			{
				int dust = Dust.NewDust(P.position, P.width, P.height, 135, 0f, 0f, 200, default(Color), 1.7f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 3f;
				dust = Dust.NewDust(P.position, P.width, P.height, 135, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 2.5f;
			}
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = Projectile;
			
			SpriteEffects effects = SpriteEffects.None;
			if (P.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			
			Color color = Lighting.GetColor((int)P.Center.X / 16, (int)P.Center.Y / 16);
			
			Vector2 pos = P.Center + Vector2.UnitY * P.gfxOffY - Main.screenPosition;
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
			Rectangle rectangle = tex.Frame(1, Main.projFrames[P.type], 0, P.frame);
			Color alpha = P.GetAlpha(color);
			Vector2 origin = rectangle.Size() / 2f;
			
			alpha.A /= 2;
			
			Main.spriteBatch.Draw(tex, pos, new Rectangle?(rectangle), alpha, P.rotation, origin, P.scale, effects, 0f);
			
			return false;
		}
	}
}
