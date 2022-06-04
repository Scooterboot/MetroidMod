using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Enums;
using System.IO;

namespace MetroidModPorted.Content.Projectiles.missilecombo
{
	public class WavebusterShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wavebuster Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 5;
		}
		
		Vector2 targetPos;
		bool setTargetPos = false;
		
		Projectile Lead;

		NPC target;
		
		const float Max_Range = 300f;
		float range = Max_Range;
		const float Max_Distance = 300f;
		float distance = Max_Distance;

		Vector2 oPos;
		Vector2 mousePos;
		
		SoundEffectInstance soundInstance;
		bool soundPlayed = false;
		int soundDelay = 0;
		
		int ampSyncCooldown = 20;
		float[] amp = new float[3];
		float[] ampDest = new float[3];

		public override void AI()
		{
			
			Projectile P = Projectile;
			Player O = Main.player[P.owner];
			
			Lead = Main.projectile[(int)P.ai[0]];
			if(!Lead.active || Lead.owner != P.owner || Lead.type != ModContent.ProjectileType<ChargeLead>())
			{
				P.Kill();
				return;
			}
			
			if(P.numUpdates == 0)
			{
				P.frame++;
			}
			if(P.frame > 1)
			{
				P.frame = 0;
			}
			
			range = Max_Range;
			distance = Max_Distance;
			
			oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			
			if(Lead != null && Lead.active)
			{
				for(int k = 0; k < range; k++)
				{
					float targetrot = (float)Math.Atan2((P.Center.Y - Lead.Center.Y), (P.Center.X - Lead.Center.X));
					Vector2 tilePos = Lead.Center + targetrot.ToRotationVector2() * k;
					int i = (int)MathHelper.Clamp(tilePos.X / 16,0,Main.maxTilesX-2);
					int j = (int)MathHelper.Clamp(tilePos.Y / 16,0,Main.maxTilesY-2);

					if(Main.tile[i,j] != null && Main.tile[i,j].HasTile && Main.tileSolid[Main.tile[i,j].TileType] && !Main.tileSolidTop[Main.tile[i,j].TileType])
					{
						range = Math.Max(range-1,1);
						distance = Math.Max(distance-1,1);
					}
					else
					{
						range = Math.Min(range+1,Max_Range);
						distance = Math.Min(distance+1,Max_Distance);
					}
				}
			}
			
			if (P.owner == Main.myPlayer)
			{
				P.netUpdate = true;
				
				Vector2 diff = Main.MouseWorld - oPos;
				diff.Normalize();
				
				mousePos = oPos + diff * Math.Min(Vector2.Distance(oPos,Main.MouseWorld),range);
				
				target = null;
				for(int i = 0; i < Main.maxNPCs; i++)
				{
					if(Main.npc[i].active && Main.npc[i].lifeMax > 5 && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)
					{
						NPC npc = Main.npc[i];
						Rectangle npcRect = new Rectangle((int)npc.position.X,(int)npc.position.Y,npc.width,npc.height);
						
						float point = 0f;
						if(Vector2.Distance(oPos,npc.Center) < range && 
						Collision.CheckAABBvLineCollision(npcRect.TopLeft(), npcRect.Size(), oPos, P.Center, P.width, ref point))
						{
							range = Vector2.Distance(oPos,npc.Center);
							mousePos = oPos + diff * Math.Min(Vector2.Distance(oPos,Main.MouseWorld),range);
						}
						
						bool flag = (Vector2.Distance(oPos,npc.Center) <= range+distance && Vector2.Distance(npc.Center,mousePos) <= distance);
						
						if (Main.npc[i].CanBeChasedBy(P, false))
						{
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
								
								if(Vector2.Distance(oPos,target.Center) > range+distance || Vector2.Distance(target.Center,mousePos) > distance)
								{
									target = null;
								}
							}
						}
					}
				}
				
				if(!setTargetPos)
				{
					targetPos = P.Center;
					setTargetPos = true;
					return;
				}
				else if(target != null && target.active)
				{
					targetPos = target.Center;
				}
				else
				{
					if(P.numUpdates == 0)
					{
						//targetPos = new Vector2(mousePos.X + Main.rand.Next(-30, 31), mousePos.Y + Main.rand.Next(-30, 31));
						targetPos = oPos + diff * range;
						targetPos.X += (float)Main.rand.Next(-30, 31) * (Vector2.Distance(oPos,P.Center) / Max_Range);
						targetPos.Y += (float)Main.rand.Next(-30, 31) * (Vector2.Distance(oPos,P.Center) / Max_Range);
					}
				}
				
				if(P.numUpdates == 0)
				{
					if(soundDelay <= 0)
					{
						if(!soundPlayed)
						{
							SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.WavebusterStart, O.position), out ActiveSound result);
							soundInstance = result.Sound;
							soundPlayed = true;
							soundDelay = 50;
						}
						else
						{
							if(soundInstance != null)
							{
								soundInstance.Stop(true);
							}
							SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.WavebusterLoop, O.position), out ActiveSound result);
							soundInstance = result.Sound;
							soundDelay = 138;
						}
					}
					else
					{
						soundDelay--;
					}
					for (int i = 0; i < 3; i++)
					{
						ampDest[i] = Main.rand.Next(-30, 31);
					}
				}

				if (ampSyncCooldown-- <= 0)
				{
					ampSyncCooldown = 20;
					Projectile.netUpdate2 = true;
				}
			}
			
			float speed = Math.Max(8f,Vector2.Distance(targetPos,P.Center) * 0.025f);
			float targetAngle = (float)Math.Atan2((targetPos.Y - P.Center.Y), (targetPos.X - P.Center.X));
			P.velocity = targetAngle.ToRotationVector2() * speed;
			
			if(O.controlUseItem)
			{
				P.timeLeft = 10;
			}
			else
			{
				P.Kill();
			}

			if (P.numUpdates == 0)
			{
				for (int i = 0; i < 3; i++)
				{
					ampDest[i] = Main.rand.Next(-30, 31);
				}
			}

			for (int i = 0; i < 3; i++)
			{
				if (amp[i] < ampDest[i])
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
			if(soundInstance != null)
			{
				soundInstance.Stop(true);
			}
		}

		public override void CutTiles()
		{
			if(Lead != null && Lead.active)
			{
				DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
				Utils.PlotTileLine(Lead.Center, Projectile.Center, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
			}
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if(Lead != null && Lead.active)
			{
				float point = 0f;
				return projHitbox.Intersects(targetHitbox) ||
					Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Lead.Center, Projectile.Center, Projectile.width, ref point);
			}
			return false;
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = Projectile;
			
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
			int num108 = tex.Height / Main.projFrames[P.type];
			int y4 = num108 * P.frame;
			
			Texture2D tex2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/missilecombo/WavebusterShot2").Value;
			int numH = tex2.Height / 4;
			
			if(Lead != null && Lead.active)
			{
				float targetrot = (float)Math.Atan2((P.Center.Y - Lead.Center.Y), (P.Center.X - Lead.Center.X));
				float dist = Math.Max(Vector2.Distance(Lead.Center,P.Center),1);
				
				double trot = targetrot+Math.PI/2;
				
				float shift = 0;
				int num = (int)Math.Max(Math.Ceiling(dist/8),1);
				float num4 = num/4;
				Vector2[] pos = new Vector2[num];
				for(int i = 0; i < num; i++)
				{
					float scale = P.scale;
					if(P.frame == 0)
					{
						scale *= 0.8f;
					}
					
					if(num4 >= 1)
					{
						if(i < num4)
						{
							shift = MathHelper.Lerp(0,amp[0],(i / num4));
						}
						else if(i < num/2)
						{
							shift = MathHelper.Lerp(amp[0],amp[1],((i - num4) / num4));
						}
						else if(i < num4*3)
						{
							shift = MathHelper.Lerp(amp[1],amp[2],((i - num/2) / num4));
						}
						else
						{
							shift = MathHelper.Lerp(amp[2],0,((i - num4*3) / num4));
							scale *= (num4 - (i - num4*3)*0.5f) / num4;
						}
					}
					
					pos[i] = Lead.Center + targetrot.ToRotationVector2() * (dist/num) * i;
					pos[i].X += (float)Math.Cos(trot)*shift * (Vector2.Distance(oPos,P.Center) / Max_Range);
					pos[i].Y += (float)Math.Sin(trot)*shift * (Vector2.Distance(oPos,P.Center) / Max_Range);
					
					float rot = (float)Math.Atan2((pos[i].Y - Lead.Center.Y), (pos[i].X - Lead.Center.X)) + (float)Math.PI/2;
					if(i > 0)
					{
						rot = (float)Math.Atan2((pos[i].Y - pos[i-1].Y), (pos[i].X - pos[i-1].X)) + (float)Math.PI/2;
					}
					sb.Draw(tex, 
					pos[i] - Main.screenPosition, 
					new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), 
					P.GetAlpha(Color.White), 
					rot, 
					new Vector2((float)tex.Width/2f, (float)num108/2), 
					new Vector2(scale,1f), 
					SpriteEffects.None, 
					0f);
					
					sb.Draw(tex2,
					pos[i] - Main.screenPosition,
					new Rectangle?(new Rectangle(0,numH*Main.rand.Next(4),tex2.Width,numH)),
					P.GetAlpha(Color.White),
					rot,
					new Vector2((float)tex2.Width/2,(float)numH/2),
					(float)(Main.rand.Next(21)/10),
					SpriteEffects.None,
					0f);
					
					Lighting.AddLight(pos[i], (MetroidModPorted.waveColor2.R / 255f) * P.scale, (MetroidModPorted.waveColor2.G / 255f) * P.scale, (MetroidModPorted.waveColor2.B / 255f) * P.scale);
					
					if (Main.rand.Next(25) == 0)
					{
						Vector2 dPos = pos[i]-new Vector2(tex.Width/2,tex.Width/2);
						Main.dust[Dust.NewDust(dPos, tex.Width, tex.Width, 62, 0, 0, 100, default(Color), 2f)].noGravity=true;
					}
				}
			}
			
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(targetPos);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			targetPos = reader.ReadVector2();
		}
	}
}
