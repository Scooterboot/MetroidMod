using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Enums;
//using MetroidMod;
//using MetroidMod.Projectiles.chargelead;

namespace MetroidModPorted.Content.Projectiles.missilecombo
{
	public class NovaLaserShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Laser Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 5;
		}
		
		const float Max_Range = 2200f;
		float maxRange = 0f;
		
		float scaleUp = 0f;
		
		Projectile Lead;
		
		SoundEffectInstance soundInstance;

		bool initialize = false;
		public override void AI()
		{
			Projectile P = Projectile;
			Player O = Main.player[P.owner];
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			
			Lead = Main.projectile[(int)P.ai[0]];
			if(!Lead.active || Lead.owner != P.owner || Lead.type != ModContent.ProjectileType<ChargeLead>() || !O.controlUseItem)
			{
				P.Kill();
				return;
			}
			else
			{
				if(!initialize)
				{
					
					initialize = true;
				}
				
				if (P.owner == Main.myPlayer)
				{
					if(soundInstance == null || soundInstance.State != SoundState.Playing)
					{
						SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.NovaLaserLoop, O.position), out ActiveSound result);
						soundInstance = result.Sound;
						soundInstance.Volume = Main.soundVolume;
					}
				}
				P.velocity = Vector2.Normalize(Lead.velocity);
				P.Center = oPos;
				P.timeLeft = 2;
				P.rotation = P.velocity.ToRotation() - 1.57f;
				
				maxRange = Math.Min(maxRange + 16f, Max_Range);
				
				for (P.ai[1] = 0f; P.ai[1] <= maxRange; P.ai[1] += 4f)
				{
					Vector2 end = oPos + P.velocity * P.ai[1];
					if(CollideMethods.CheckCollide(end, 0, 0))
					{
						P.ai[1] -= 4f;
						break;
					}
				}
				
				float leadDist = Vector2.Distance(oPos,Lead.Center);
				for(float i = leadDist; i < P.ai[1]; i += P.width)
				{
					if (Main.rand.Next(25) == 0)
					{
						float k = Math.Min(i, P.ai[1]);
						Vector2 dPos = (oPos - P.Size/2) + P.velocity * k;
						Main.dust[Dust.NewDust(dPos, P.width, P.width, 75, 0, 0, 100, default(Color), 2f)].noGravity=true;
					}
				}
				
				Vector2 dustPos = oPos + P.velocity * P.ai[1];
				float num1 = P.velocity.ToRotation() + (Main.rand.Next(2) == 1 ? -1.0f : 1.0f) * 1.57f;
				float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
				Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
				Dust dust = Main.dust[Dust.NewDust(dustPos, 0, 0, 75, dustVel.X, dustVel.Y, 100, default(Color), 2f)];
				dust.noGravity = true;
				dust.velocity *= 3f;
				dust.position = dustPos;
				
				Color color = MetroidModPorted.novColor;
				DelegateMethods.v3_1 = new Vector3(color.R/255f,color.G/255f,color.B/255f);
				Utils.PlotTileLine(P.Center, P.Center + P.velocity * P.ai[1], 26, DelegateMethods.CastLight);
				
				if(P.numUpdates == 0)
				{
					scaleUp = Math.Min(scaleUp + 0.1f, 1f);
					P.frame++;
					if(P.frame >= Main.projFrames[Projectile.type])
					{
						P.frame = 0;
					}
				}
				ChargeLead chLead = (ChargeLead)Lead.ModProjectile;
				chLead.extraScale = 0.75f * scaleUp;
			}
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override void CutTiles()
		{
			Projectile P = Projectile;
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(P.Center, P.Center + P.velocity * (P.ai[1] + 4f), (P.width + 16) * P.scale, DelegateMethods.CutTiles);
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(39,600,true);
			target.immune[Projectile.owner] = 4;
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Projectile P = Projectile;
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), P.Center,
				P.Center + P.velocity * P.ai[1], P.width, ref point);
		}
		
		public override void Kill(int timeLeft)
		{
			if(soundInstance != null)
			{
				soundInstance.Stop(true);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			if(Lead != null && Lead.active)
			{
				Projectile P = Projectile;
				Player O = Main.player[P.owner];
				Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
				
				Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
				
				int tHeight = tex.Height / Main.projFrames[Projectile.type];
				
				Vector2 scale = new Vector2(scaleUp,1f);
				
				int bodyFrameCount = 3;
				
				int tailHeight = 22;
				int headHeight = 22;
				int bodyHeight = 30 / bodyFrameCount;
				
				float leadDist = Vector2.Distance(oPos,Lead.Center);
				
				for (float i = leadDist; i <= P.ai[1]; i += bodyHeight)
				{
					Vector2 pos = P.Center + P.velocity * i;
					
					int height = Math.Min(bodyHeight, (int)(P.ai[1] - i - headHeight/2));
					
					int frame = Main.rand.Next(bodyFrameCount);
					
					if(height > 0)
					{
						sb.Draw(tex, pos - Main.screenPosition, 
						new Rectangle?(new Rectangle(0, tailHeight+2 + (tHeight*P.frame) + (bodyFrameCount*frame), tex.Width, height)), 
						P.GetAlpha(Color.White), P.rotation, 
						new Vector2((float)tex.Width/2f, 0f), 
						scale, SpriteEffects.None, 0f);
					}
				}
				
				if(P.ai[1] > leadDist+headHeight/2)
				{
					Vector2 pos2 = P.Center + P.velocity * P.ai[1];
					sb.Draw(tex, pos2 - Main.screenPosition, 
					new Rectangle?(new Rectangle(0, tailHeight+2+(bodyHeight*bodyFrameCount)+2 + (tHeight*P.frame), tex.Width, headHeight)), 
					P.GetAlpha(Color.White), P.rotation, 
					new Vector2((float)tex.Width/2f, headHeight-3), 
					scale, SpriteEffects.None, 0f);
				}
			}
			
			return false;
		}
	}
}
