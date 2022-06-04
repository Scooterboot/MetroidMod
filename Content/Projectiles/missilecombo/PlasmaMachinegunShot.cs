using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Projectiles.missilecombo
{
	public class PlasmaMachinegunShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Machinegun Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.scale = 1f;
			Projectile.penetrate = 15;//-1;
			Projectile.usesLocalNPCImmunity = true;
       	 	Projectile.localNPCHitCooldown = 4;
			
			mProjectile.amplitude = 4f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 1;
		}
		
		bool initialized = false;
		Vector2 start = Vector2.Zero;
		Vector2 startPos = Vector2.Zero;
		Projectile Lead;
		public override void AI()
		{
			Projectile P = Projectile;
			Player O = Main.player[P.owner];
			Lead = Main.projectile[(int)P.ai[0]];
			
			Color color = MetroidModPorted.plaGreenColor;
			Lighting.AddLight(P.Center, color.R/255f,color.G/255f,color.B/255f);
			
			mProjectile.WaveBehavior(P, false);
			
			if(P.numUpdates == 0)
			{
				int dust = Dust.NewDust(P.position, P.width, P.height, 61, 0, 0, 100, default(Color), P.scale);
				Main.dust[dust].noGravity = true;
				
				P.frame++;
				if(P.frame >= Main.projFrames[Projectile.type])
				{
					P.frame = 0;
				}
			}
			
			if(!initialized)
			{
				P.rotation = (float)Math.Atan2(P.velocity.Y, P.velocity.X) + 1.57f;
				for(int i = 0; i < P.oldPos.Length; i++)
				{
					P.oldPos[i] = P.position;
				}
				for(int i = 0; i < P.oldRot.Length; i++)
				{
					P.oldRot[i] = P.rotation;
				}
				start = P.Center - Lead.Center;
				initialized = true;
				return;
			}
			else
			{
				Vector2 velocity = P.position - P.oldPos[0];
				if(Vector2.Distance(P.position, P.position+velocity) < Vector2.Distance(P.position,P.position+P.velocity))
				{
					velocity = P.velocity;
				}
				P.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
				
				startPos = Lead.Center + O.velocity + start;
			}
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, 61);
		}
		
		/*public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 100);
		}*/
		
		bool drawFlag = false;
		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			if(initialized)
			{
				Projectile P = Projectile;
				Player O = Main.player[P.owner];
				Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
				
				float scaleDrop = 0.5f;
				Color color = default(Color);
				
				Color color2 = Color.White;
				if(color != default(Color))
				{
					color2 = color;
				}
				SpriteEffects effects = SpriteEffects.None;
				if (P.spriteDirection == -1)
				{
					effects = SpriteEffects.FlipHorizontally;
				}
				Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
				int height = tex.Height / Main.projFrames[P.type];
				int y4 = height * P.frame;
				
				float dist = 0f;
				if(Lead != null && Lead.active)
				{
					dist = Vector2.Distance(oPos,Lead.Center);
				}
				
				float vel = Math.Min(Vector2.Distance(P.Center,startPos), P.velocity.Length());
				
				int amt = 10;
				for(int i = amt-1; i > -1; i--)
				{
					if(Vector2.Distance(oPos,P.oldPos[i]+P.Size/2f) >= dist)
					{
						Color color23 = color2;
						color23 = P.GetAlpha(color23);
						color23 *= (float)(amt - i) / ((float)amt);
						//color23.A = (byte)((float)color23.A * ((float)(amt - i) / (float)amt));
						float scale = MathHelper.Lerp(P.scale, P.scale*scaleDrop, (float)i / amt);
						
						float vel2 = Math.Min(Vector2.Distance(P.oldPos[i]+P.Size/2f,startPos), P.velocity.Length());
						if(vel2 > 0)
						{
							for(float j = vel2; j > 0; j--)
							{
								//Color color4 = color23;
								//color4 *= (float)(vel2 - j) / ((float)vel2);
								//color4.A = (byte)((float)color4.A * ((float)(vel2 - j) / (float)vel2));
								Vector2 oldPos = P.oldPos[i] + P.Size/2f - Vector2.Normalize(P.velocity) * j;
								sb.Draw(tex, oldPos - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, height)), 
								color23, P.oldRot[i], new Vector2((float)tex.Width/2f, (float)P.height/2f), scale, effects, 0f);
							}
						}
						
						sb.Draw(tex, P.oldPos[i] + P.Size/2f - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, height)), 
						color23, P.oldRot[i], new Vector2((float)tex.Width/2f, (float)P.height/2f), scale, effects, 0f);
					}
				}
				if(vel > 0)
				{
					for(float j = vel; j > 0; j--)
					{
						//Color color3 = P.GetAlpha(color2);
						//color3 *= (float)(vel - j) / ((float)vel);
						//color3.A = (byte)((float)color3.A * ((float)(vel - j) / (float)vel));
						Vector2 pos = P.Center - Vector2.Normalize(P.velocity) * j;
						sb.Draw(tex, pos - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, height)), 
						P.GetAlpha(color2), P.rotation, new Vector2((float)tex.Width/2f, (float)P.height/2f), P.scale, effects, 0f);
					}
				}
				sb.Draw(tex, P.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, height)), 
				P.GetAlpha(color2), P.rotation, new Vector2((float)tex.Width/2f, (float)P.height/2f), P.scale, effects, 0f);
			}
			
			return false;
		}
	}
}
