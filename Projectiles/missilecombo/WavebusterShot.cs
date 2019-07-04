using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;
using Terraria.Enums;

namespace MetroidMod.Projectiles.missilecombo
{
	public class WavebusterShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wavebuster Shot");
			Main.projFrames[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.extraUpdates = 5;
		}
		
		Vector2 targetPos;
		bool setTargetPos = false;
		
		Projectile Lead;

		NPC target;
		
		SoundEffectInstance soundInstance;
		bool soundPlayed = false;
		int soundDelay = 0;
		
		float[] amp = new float[3];
		float[] ampDest = new float[3];
		public override void AI()
		{
			
			Projectile P = projectile;
			Player O = Main.player[P.owner];
			
			Lead = Main.projectile[(int)P.ai[0]];
			if(!Lead.active || Lead.owner != P.owner || Lead.type != mod.ProjectileType("ChargeLead"))
			{
				if(soundInstance != null)
				{
					soundInstance.Stop(true);
				}
				P.Kill();
				return;
			}
			
			if(projectile.numUpdates == 0)
			{
				projectile.frame++;
			}
			if(projectile.frame > 1)
			{
				projectile.frame = 0;
			}
			
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			
			float maxRange = 300;
			float distance = 300f;
			float accuracy = 11f;
			
			if(Lead != null && Lead.active)
			{
				for(int k = 0; k < maxRange; k++)
				{
					float targetrot = (float)Math.Atan2((P.Center.Y - Lead.Center.Y), (P.Center.X - Lead.Center.X));
					Vector2 tilePos = Lead.Center + targetrot.ToRotationVector2() * k;
					int i = (int)MathHelper.Clamp((tilePos.X) / 16f,0,Main.maxTilesX-1);
					int j = (int)MathHelper.Clamp((tilePos.Y) / 16f,0,Main.maxTilesY-1);

					if(Main.tile[i,j] != null && Main.tile[i,j].active() && Main.tileSolid[Main.tile[i,j].type] && !Main.tileSolidTop[Main.tile[i,j].type])
					{
						maxRange = Math.Max(maxRange-1,1);
						distance = Math.Max(distance-1,1);
					}
					else
					{
						maxRange = Math.Min(maxRange+1,300);
						distance = Math.Min(distance+1,300);
					}
				}
			}
			
			if (P.owner == Main.myPlayer)
			{
				float MY = Main.mouseY + Main.screenPosition.Y;
				float MX = Main.mouseX + Main.screenPosition.X;
				if (O.gravDir == -1f)
				{
					MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
				}
				
				float targetrotation = (float)Math.Atan2((MY - oPos.Y), (MX - oPos.X));

				Vector2 mousePos = oPos + targetrotation.ToRotationVector2() * Math.Min(Vector2.Distance(oPos,new Vector2(MX,MY)),maxRange);
				
				for(int i = 0; i < Main.maxNPCs; i++)
				{
					if (Main.npc[i].CanBeChasedBy(P, false))
					{
						NPC npc = Main.npc[i];
						bool flag = (Vector2.Distance(oPos,npc.Center) <= maxRange+distance && Vector2.Distance(npc.Center,mousePos) <= distance);
						if(target == null || !target.active)
						{
							if(flag)
							{
								target = npc;
							}
						}
						else
						{
							if(npc != target && flag && Vector2.Distance(npc.Center,mousePos) < Vector2.Distance(target.Center,mousePos))
							{
								target = npc;
							}
							
							if(Vector2.Distance(oPos,target.Center) > maxRange+distance || Vector2.Distance(target.Center,mousePos) > distance)
							{
								target = null;
							}
						}
					}
				}
				
				if(!setTargetPos)
				{
					targetPos = projectile.Center;
					setTargetPos = true;
					return;
				}
				else if(target != null && target.active)
				{
					targetPos = target.Center;
				}
				else
				{
					if(projectile.numUpdates <= 0)
					{
						targetPos = new Vector2(mousePos.X + Main.rand.Next(-30, 31), mousePos.Y + Main.rand.Next(-30, 31));
					}
				}
				
				float num243 = Math.Max(8f,Vector2.Distance(targetPos,P.Center) * 0.025f);
				float num244 = targetPos.X - P.Center.X;
				float num245 = targetPos.Y - P.Center.Y;
				float num246 = (float)Math.Sqrt((double)(num244 * num244 + num245 * num245));
				num246 = num243 / num246;
				num244 *= num246;
				num245 *= num246;
				P.velocity.X = (P.velocity.X * accuracy + num244) / (accuracy + 1f);
				P.velocity.Y = (P.velocity.Y * accuracy + num245) / (accuracy + 1f);
				
				if(projectile.numUpdates <= 0)
				{
					if(soundDelay <= 0)
					{
						if(!soundPlayed)
						{
							soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)O.position.X, (int)O.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/WavebusterStart"));
							soundPlayed = true;
							soundDelay = 104;
						}
						else
						{
							if(soundInstance != null)
							{
								soundInstance.Stop(true);
							}
							soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)O.position.X, (int)O.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/WavebusterLoop"));
							soundDelay = 140;
						}
					}
					else
					{
						soundDelay--;
					}
				}
				
				if(O.controlUseItem)
				{
					P.timeLeft = 10;
				}
				else
				{
					if(soundInstance != null)
					{
						soundInstance.Stop(true);
					}
					P.Kill();
				}
			}
			
			if(projectile.numUpdates <= 0)
			{
				for(int i = 0; i < 3; i++)
				{
					ampDest[i] = Main.rand.Next(-60, 61);
				}
			}
			
			for(int i = 0; i < 3; i++)
			{
				if(amp[i] < ampDest[i])
				{
					amp[i] += 3;
				}
				else
				{
					amp[i] -= 3;
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			
		}

		public override void CutTiles()
		{
			if(Lead != null && Lead.active)
			{
				DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
				Utils.PlotTileLine(Lead.Center, projectile.Center, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
			}
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if(Lead != null && Lead.active)
			{
				float point = 0f;
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Lead.Center, projectile.Center, projectile.width, ref point);
			}
			return false;
		}
		
		/*public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 200);
		}*/
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Projectile P = projectile;
			
			Texture2D tex = Main.projectileTexture[P.type];
			int num108 = tex.Height / Main.projFrames[P.type];
			int y4 = num108 * P.frame;
			
			Texture2D tex2 = mod.GetTexture("Projectiles/missilecombo/WavebusterShot2");
			int numH = tex2.Height / 4;
			
			if(Lead != null && Lead.active)
			{
				float targetrot = (float)Math.Atan2((P.Center.Y - Lead.Center.Y), (P.Center.X - Lead.Center.X));
				float dist = Vector2.Distance(Lead.Center,P.Center);
				
				float t = 0;
				int num = (int)Math.Ceiling(dist/2);
				Vector2[] pos = new Vector2[num];
				for(int i = 0; i < num; i++)
				{
					float scale = P.scale;
					if(P.frame == 0)
					{
						scale *= 0.8f;
					}
					pos[i] = Lead.Center + targetrot.ToRotationVector2() * (dist/num) * i;
					
					float amplitude = 30f;
					
					float tdest = 0;
					float num4 = num/4;
					if(i < num/4)
					{
						tdest = ((float)Math.PI/2) * (amp[0] / 30);
					}
					else if(i < num/2)
					{
						tdest = ((float)Math.PI/2) * (amp[1] / 30);
					}
					else if(i < num4*3)
					{
						tdest = ((float)Math.PI/2) * (amp[2] / 30);
					}
					else
					{
						tdest = 0;
						scale *= (num4 - (i - num4*3)*0.5f) / num4;
					}
					
					/*if(tdest < -(float)Math.PI)
					{
						tdest += (float)Math.PI;
					}
					if(tdest >= (float)Math.PI)
					{
						tdest -= (float)Math.PI;
					}*/
					float incr = (float)Math.PI/dist;
					
					if(t < tdest)
					{
						t = Math.Min(t + incr, tdest);
					}
					else
					{
						t = Math.Max(t - incr, tdest);
					}
					
					
					float shift = amplitude * (float)Math.Sin(t);
					
					float trot = targetrot+(float)Math.PI/2;
					pos[i].X += (float)Math.Cos(trot)*shift;
					pos[i].Y += (float)Math.Sin(trot)*shift;
					
					if(i > 0)
					{
						float rot = (float)Math.Atan2((pos[i].Y - pos[i-1].Y), (pos[i].X - pos[i-1].X)) + (float)Math.PI/2;
						sb.Draw(tex, 
						pos[i] - Main.screenPosition, 
						new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), 
						P.GetAlpha(Color.White), 
						rot, 
						new Vector2((float)tex.Width/2f, (float)num108-1), 
						scale, 
						SpriteEffects.None, 
						0f);
						
						if(i % Math.Max((numH/2)-1,1) == 0 || i >= num-1)
						{
							sb.Draw(tex2,
							pos[i] - Main.screenPosition,
							new Rectangle?(new Rectangle(0,numH*Main.rand.Next(4),tex2.Width,numH)),
							P.GetAlpha(Color.White),
							rot,
							new Vector2((float)tex2.Width/2,(float)numH/2),
							(float)(Main.rand.Next(21)/10),
							SpriteEffects.None,
							0f);
						}
						
						Lighting.AddLight(pos[i], (MetroidMod.waveColor2.R / 255f) * P.scale, (MetroidMod.waveColor2.G / 255f) * P.scale, (MetroidMod.waveColor2.B / 255f) * P.scale);
						
						if (Main.rand.Next(25) == 0)
						{
							Vector2 dPos = pos[i]-new Vector2(tex.Width/2,tex.Width/2);
							Main.dust[Dust.NewDust(dPos, tex.Width, tex.Width, 62, 0, 0, 100, default(Color), 2f)].noGravity=true;
						}
					}
				}
			}
			
			return false;
		}
	}
}