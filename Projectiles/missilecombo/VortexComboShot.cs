using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.missilecombo
{
	public class VortexComboShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Combo Shot");
			//Main.projFrames[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 1f;
			projectile.timeLeft = 120;
			//projectile.timeLeft = 60+Main.rand.Next(61);
			projectile.penetrate = -1;
			projectile.extraUpdates = 0;
			projectile.alpha = 255;
		}
		
		bool initialized = false;
		bool checkbreak = false;
		int damage = 0;
		Vector2 velocity = Vector2.Zero;
		Projectile Lead;
		public override void AI()
		{
			Projectile P = projectile;
			Player O = Main.player[P.owner];
			Lead = Main.projectile[(int)P.ai[0]];
			
			float speed = 10f;//14f;
			
			if(Lead == null || !Lead.active || Lead.owner != P.owner || Lead.type != mod.ProjectileType("ChargeLead") || !O.controlUseItem)
			{
				P.Kill();
				return;
			}
			else
			{
				velocity = Vector2.Normalize(Lead.velocity)*speed;
			}
			
			if(!initialized)
			{
				/*if (P.owner == Main.myPlayer)
				{
					Main.PlaySound(2,(int)P.Center.X,(int)P.Center.Y,8);//43);
				}*/
				P.rotation = (float)Angle.ConvertToRadians(Main.rand.Next(36)*10);
				P.scale = 0f;
				damage = P.damage;
				P.damage = 0;
				
				Vector2 vel = P.velocity;
				vel = vel.RotatedBy(P.rotation, default(Vector2));
				P.velocity = Vector2.Normalize(vel)*(4f+Main.rand.Next(4));
				
				//P.timeLeft = 60+Main.rand.Next(61);
				
				P.ai[1] = -1;
				
				initialized = true;
				return;
			}
			
			if (P.owner == Main.myPlayer)
			{
				P.netUpdate = true;
				
				Vector2 point = Main.MouseWorld;
				
				//P.ai[1] = -1;
				for(int i = 0; i < Main.maxNPCs; i++)
				{
					if(Main.npc[i].active && Main.npc[i].lifeMax > 5 && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)
					//if(Main.npc[i].active && Main.npc[i].CanBeChasedBy(P,false))
					{
						NPC npc = Main.npc[i];
						
						bool flag = (npc.Distance(point) < 500 && Collision.CanHit(P.position,P.width,P.height,npc.position,npc.width,npc.height));
						
						int numTarget = 0;
						if(flag)
						{
							for(int j = 0; j < Main.maxProjectiles; j++)
							{
								if(Main.projectile[j].active && Main.projectile[j].owner == P.owner && Main.projectile[j].type == P.type)
								{
									if(Main.projectile[j].ai[1] == i && j != P.whoAmI)
									{
										numTarget++;
									}
								}
							}
						}
						if(numTarget < 5)
						{
							if(P.ai[1] == -1)
							{
								if(flag)
								{
									P.ai[1] = i;
								}
							}
							else
							{
								if(!checkbreak && i != P.ai[1] && flag && npc.Distance(point) < Main.npc[(int)P.ai[1]].Distance(point))
								{
									P.ai[1] = i;
								}
							}
						}
						else if(P.ai[1] == -1 && Main.rand.Next(2) == 0 && !checkbreak)
						{
							P.ai[1] = i;
						}
					}
					if(i >= Main.maxNPCs-1)
					{
						checkbreak = true;
					}
				}
				if(P.ai[1] != -1)
				{
					if(!Main.npc[(int)P.ai[1]].active)
					{
						P.ai[1] = -1;
						checkbreak = false;
					}
					else if(Main.npc[(int)P.ai[1]].Distance(point) > 350)
					{
						P.ai[1] = -1;
						checkbreak = false;
					}
					else
					{
						Vector2 diff2 = Main.npc[(int)P.ai[1]].Center - P.Center;
						diff2.Normalize();
						if (float.IsNaN(diff2.X) || float.IsNaN(diff2.Y))
						{
							diff2 = -Vector2.UnitY;
						}
						velocity = diff2*speed;
					}
				}
				/*else
				{
					Vector2 diff2 = Main.MouseWorld - P.Center;
					diff2.Normalize();
					if (float.IsNaN(diff2.X) || float.IsNaN(diff2.Y))
					{
						diff2 = -Vector2.UnitY;
					}
					velocity = diff2*speed;
				}*/
			}
			
			Vector2 diff = Lead.Center - P.Center;
			diff.Normalize();
			if (float.IsNaN(diff.X) || float.IsNaN(diff.Y))
			{
				diff = -Vector2.UnitY;
			}
			P.velocity += diff*MathHelper.Lerp(0.1f,1f,Math.Max((Vector2.Distance(Lead.Center,P.Center)-300f)/300f,0f));
			
			Color color = MetroidMod.lumColor;
			Lighting.AddLight(P.Center, color.R/255f,color.G/255f,color.B/255f);
			
			//if(P.numUpdates == 0)
			//{
				int dust = Dust.NewDust(P.position, P.width, P.height, 229, 0, 0, 100, default(Color), P.scale);
				Main.dust[dust].noGravity = true;
				
				/*P.frame++;
				if(P.frame >= Main.projFrames[projectile.type])
				{
					P.frame = 0;
				}*/
				P.scale = Math.Min(P.scale + 0.05f, 0.5f);//1f);
				P.alpha = Math.Max(P.alpha - 15, 0);
				P.rotation -= 0.104719758f * 2f;
			//}
			
			P.position.X += (float)P.width/2f;
			P.position.Y += (float)P.height/2f;
			P.width = (int)(8f * P.scale);
			P.height = (int)(8f * P.scale);
			P.position.X -= (float)P.width/2f;
			P.position.Y -= (float)P.height/2f;
		}
		public override void Kill(int timeLeft)
		{
			Projectile P = projectile;
			if(velocity != Vector2.Zero)
			{
				int v = Projectile.NewProjectile(P.Center.X, P.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("VortexComboShot2"), damage, P.knockBack, P.owner);
				Main.PlaySound(SoundLoader.customSoundType, (int)P.Center.X, (int)P.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/WideBeamSound"));
			}
			
			for (int i = 0; i < 30; i++)
			{
				int dust = Dust.NewDust(P.Center-new Vector2(18,18), 36, 36, 229, 0, 0, 100, default(Color), 2f);
				Main.dust[dust].velocity = new Vector2((Main.rand.Next(30)-15), (Main.rand.Next(30)-15)) * 0.125f;
				Main.dust[dust].noGravity = true;
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			//return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 100);
			return new Color(255 - projectile.alpha, 255 - projectile.alpha, 255 - projectile.alpha, 255 - projectile.alpha);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Projectile P = projectile;
			
			Texture2D tex = Main.projectileTexture[P.type];
			Texture2D tex2 = Main.extraTexture[50];
			
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (P.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			
			Vector2 pos = P.Center - Main.screenPosition;
			
			Color color25 = Lighting.GetColor((int)P.Center.X / 16, (int)P.Center.Y / 16);
			Color alpha4 = P.GetAlpha(color25);
			
			Vector2 origin = new Vector2((float)tex.Width, (float)tex.Height) / 2f;
			
			Color color55 = alpha4 * 0.8f;
			color55.A /= 2;
			Color color56 = Color.Lerp(alpha4, Color.Black, 0.5f);
			color56.A = alpha4.A;
			float num273 = 0.95f + (P.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
			color56 *= num273;
			float scale12 = 0.6f + P.scale * 0.6f * num273;
			
			sb.Draw(tex2, pos, null, color56, -P.rotation + 0.35f, origin, scale12, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(tex2, pos, null, alpha4, -P.rotation, origin, P.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(tex, pos, null, color55, -P.rotation * 0.7f, origin, P.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(tex2, pos, null, alpha4 * 0.8f, P.rotation * 0.5f, origin, P.scale * 0.9f, spriteEffects, 0f);
			alpha4.A = 0;
			
			sb.Draw(tex, pos, null, alpha4, P.rotation, origin, P.scale, spriteEffects, 0f);
			return false;
		}
	}
}