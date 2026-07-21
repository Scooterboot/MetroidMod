using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Metroids
{
	public class ElectricBall : ModProjectile
	{
		public override void SetDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
			Projectile.width = Projectile.height = 24;
			Projectile.scale = 0.25f;
			Projectile.hostile = true;
			Projectile.friendly = false;

			Projectile.timeLeft = 300;
		}

		public override bool PreAI()
		{
			if (Projectile.timeLeft > 12) //Falling
			{
				if (Projectile.scale < 1f)
				{
					Projectile.scale += 0.05f;
					Projectile.position.X = Projectile.Center.X - (float)(24 * Projectile.scale / 2f);
					Projectile.position.Y = Projectile.Center.Y - (float)(24 * Projectile.scale / 2f);
					Projectile.width = (int)Math.Round(24 * Projectile.scale);
					Projectile.height = (int)Math.Round(24 * Projectile.scale);
				}
				if (Projectile.velocity.Y < 6)
				{
					Projectile.velocity.Y += 0.2f;
				}
				Projectile.frameCounter++;
				if (Projectile.frameCounter >= 3)
				{
					Projectile.frameCounter = 0;
					Projectile.frame = (Projectile.frame + 1) % 4;
				}
			}
			else //Burst
			{
				if (Projectile.timeLeft == 11)
				{
					SoundEngine.PlaySound(SoundID.Item92.WithPitchOffset(0.35f).WithVolumeScale(0.25f), Projectile.Center);

					for (int i = 0; i < 8; ++i)
					{
						Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
					}
				}
				Projectile.velocity = Vector2.Zero;
				Projectile.scale = 7f - (Projectile.timeLeft * 0.5f);
				Projectile.position.X = Projectile.Center.X - (float)(24 * Projectile.scale / 2f);
				Projectile.position.Y = Projectile.Center.Y - (float)(24 * Projectile.scale / 2f);
				Projectile.width = (int)Math.Round(24 * Projectile.scale);
				Projectile.height = (int)Math.Round(24 * Projectile.scale);
			}
			return false;
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (Collides(Projectile.position, Projectile.Size, target.position, target.Size))
			{
				return base.CanHitNPC(target);
			}
			return false;
		}
		public override bool CanHitPlayer(Player target)
		{
			if (Collides(Projectile.position, Projectile.Size, target.position, target.Size))
			{
				return base.CanHitPlayer(target);
			}
			return false;
		}
		public override bool CanHitPvp(Player target)
		{
			if (Collides(Projectile.position, Projectile.Size, target.position, target.Size))
			{
				return base.CanHitPvp(target);
			}
			return false;
		}
		public bool Collides(Vector2 ellipsePos, Vector2 ellipseDim, Vector2 boxPos, Vector2 boxDim)
		{
			Vector2 ellipseCenter = ellipsePos + 0.5f * ellipseDim;
			float x = 0f; //ellipse center
			float y = 0f; //ellipse center
			if (boxPos.X > ellipseCenter.X)
			{
				x = boxPos.X - ellipseCenter.X; //left corner
			}
			else if (boxPos.X + boxDim.X < ellipseCenter.X)
			{
				x = boxPos.X + boxDim.X - ellipseCenter.X; //right corner
			}
			if (boxPos.Y > ellipseCenter.Y)
			{
				y = boxPos.Y - ellipseCenter.Y; //top corner
			}
			else if (boxPos.Y + boxDim.Y < ellipseCenter.Y)
			{
				y = boxPos.Y + boxDim.Y - ellipseCenter.Y; //bottom corner
			}
			float a = ellipseDim.X / 2f;
			float b = ellipseDim.Y / 2f;
			return x * x / (a * a) + y * y / (b * b) < 1; //point collision detection
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(tex.Width * 0.5f, (tex.Height / Main.projFrames[Projectile.type]) * 0.5f);
			Rectangle rect = new Rectangle(0, (tex.Height / Main.projFrames[Projectile.type]) * Projectile.frame, tex.Width, tex.Height / Main.projFrames[Projectile.type]);

			Texture2D texBurst = ModContent.Request<Texture2D>($"{Texture}_Burst").Value;
			Vector2 burstOrigin = new Vector2(texBurst.Width * 0.5f, (texBurst.Height / 6) * 0.5f);
			int burstFrame = 5 - (Projectile.timeLeft / 2);
			Rectangle burstRect = new Rectangle(0, (texBurst.Height / 6) * burstFrame, texBurst.Width, texBurst.Height / 6);

			Texture2D texRing = ModContent.Request<Texture2D>($"{Texture}_BurstRing").Value;
			Vector2 ringOrigin = new Vector2(texRing.Width * 0.5f, texRing.Height * 0.5f);
			Rectangle ringRect = new Rectangle(0, 0, texRing.Width, texRing.Height);

			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Color color = Color.White;
			color *= 0.9f;

			if (Projectile.timeLeft > 12) //Falling
			{
				Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(rect), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);
			}
			if (Projectile.timeLeft < 12)
			{
				Main.EntitySpriteDraw(texBurst, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(burstRect), color, Projectile.rotation, burstOrigin, 1f, effects, 0);

				Color ringColor = Color.White;
				if (Projectile.timeLeft < 10)
				{
					ringColor *= (Projectile.timeLeft * 0.1f);
				}
				float ringScale = Projectile.scale * 0.3f;
				Main.EntitySpriteDraw(texRing, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(ringRect), ringColor, Projectile.rotation, ringOrigin, ringScale, effects, 0);

			}
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.timeLeft > 12) //Falling
			{
				Projectile.timeLeft = 12;
			}
			return false;
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (Projectile.timeLeft > 12) //Falling
			{
				Projectile.timeLeft = 12;
			}
			target.AddBuff(BuffID.Electrified, 300);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.timeLeft > 12) //Falling
			{
				Projectile.timeLeft = 12;
			}
			target.AddBuff(BuffID.Electrified, 300);
		}

		public override void OnKill(int timeLeft)
		{
			/*for (int i = 0; i < 8; ++i)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
			}*/
		}
	}
}
