using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles
{
	public class MProjectile : ModProjectile
    {
        public MProjectile mProjectile;
		public MProjectile()
		{
			mProjectile = this;
		}
		
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.timeLeft = 600;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.extraUpdates = 2;
			for(int i = 0; i < projectile.oldPos.Length; i++)
			{
				projectile.oldPos[i] = projectile.position;
			}
			
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			
			for(int i = 0; i < projectile.oldRot.Length; i++)
			{
				projectile.oldRot[i] = projectile.rotation;
            }
        }

		bool[] npcPrevHit = new bool[Main.maxNPCs];
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{	
			if(projectile.Name.Contains("Plasma") && projectile.Name.Contains("Red"))
			{
				if(projectile.Name.Contains("Ice"))
				{
					target.AddBuff(44,300,true);
				}
				else
				{
					target.AddBuff(24,300,true);
				}
			}
			
			if(projectile.Name.Contains("Nova"))
			{
				if(projectile.Name.Contains("Ice"))
				{
					target.AddBuff(44,300,true);
				}
				else
				{
					target.AddBuff(39,300,true);
				}
			}
			if(projectile.Name.Contains("Ice"))
			{
				if(projectile.Name.Contains("Missile"))
				{
					target.AddBuff(mod.BuffType("InstantFreeze"),300,true);
				}
				else
				{
					target.AddBuff(mod.BuffType("IceFreeze"),300,true);
				}
			}
			
			if(projectile.Name.Contains("Solar"))
			{
				target.AddBuff(189,300,true);
			}
			
			if(projectile.Name.Contains("Stardust"))
			{
				target.AddBuff(mod.BuffType("IceFreeze"),300,true);
			}
			
			if(projectile.penetrate > 1)
			{
				npcPrevHit[target.whoAmI] = true;
			}
		}
        public override void PostAI()
		{	
			for (int i = projectile.oldPos.Length-1; i > 0; i--)
			{
				projectile.oldPos[i] = projectile.oldPos[i - 1];
			}
			projectile.oldPos[0] = projectile.position;


			for (int i = projectile.oldRot.Length-1; i > 0; i--)
			{
				projectile.oldRot[i] = projectile.oldRot[i - 1];
			}
			projectile.oldRot[0] = projectile.rotation;
		}
		
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 0;
			height = 0;
			return true;
		}
		
		public bool seeking = false;
		public int seekTarget = -1;

		public int waveStyle = 0;
		public int delay = 0;
		public int waveDir = -1;
		public float wavesPerSecond = 0f;
		public float amplitude = 0f;
		public int waveDepth = 8;
		float t = 0f;
		float t2 = 0f;
		Vector2 pos = new Vector2(0,0);
		bool initialized = false;
		void initialize(Projectile P)
		{
			pos = P.position;
			initialized = true;
		}
		public void WaveBehavior(Projectile P, bool spaze = false)
		{
			if(!initialized)
			{
				initialize(P);
			}
			else
			{
				float increment = ((float)Math.PI*2)/60f;
				int i = 1;
				if(waveStyle == 1)
				{
					i = -1;
				}
				if(waveStyle == 2)
				{
					i = 0;
				}
				if(waveStyle == 3)
				{
					i = 2;
				}
				if(waveStyle == 4)
				{
					i = -2;
				}
				if(delay <= 0)
				{
					if(spaze)
					{
						t = Math.Min(t + increment * wavesPerSecond, (float)Math.PI/2);
					}
					else
					{
						t += increment * wavesPerSecond;
					}
					if(t >= (float)Math.PI*2)
					{
						t -= (float)Math.PI*2;
					}
					
					if(waveStyle == 3 || waveStyle == 4)
					{
						t2 = Math.Min(t2 + increment * wavesPerSecond / 4, (float)Math.PI/2);
					}
				}
				delay = Math.Max(delay - 1, 0);
				i *= projectile.direction;
				if(!spaze)
				{
					i *= waveDir;
				}
				float shift = amplitude * (float)Math.Sin(t) * i;
				if(!spaze && (waveStyle == 3 || waveStyle == 4))
				{
					shift = amplitude * (float)Math.Sin(t - t2) * i;
				}
				
				pos += P.velocity;
				
				float rot = (float)Math.Atan2((P.velocity.Y),(P.velocity.X));
				P.position.X = pos.X + (float)Math.Cos(rot+((float)Math.PI/2))*shift;
				P.position.Y = pos.Y + (float)Math.Sin(rot+((float)Math.PI/2))*shift;
				
				if(!P.tileCollide)
				{
					waveDepth = 4;
					if(projectile.Name.Contains("Spazer"))
					{
						waveDepth = 6;
					}
					if(projectile.Name.Contains("Plasma"))
					{
						waveDepth = 8;
					}
					if(projectile.Name.Contains("V2"))
					{
						waveDepth = 6;
					}
					if(projectile.Name.Contains("Wide"))
					{
						waveDepth = 9;
					}
					if(projectile.Name.Contains("Nova"))
					{
						waveDepth = 12;
					}
					if(projectile.Name.Contains("Nebula"))
					{
						waveDepth = 8;
					}
					if(projectile.Name.Contains("Vortex"))
					{
						waveDepth = 12;
					}
					if(projectile.Name.Contains("Solar"))
					{
						waveDepth = 16;
					}
					if(projectile.Name.Contains("Charge"))
					{
						waveDepth += 2;
						if(projectile.Name.Contains("V2") || projectile.Name.Contains("Wide") || projectile.Name.Contains("Nova"))
						{
							waveDepth += 1;
						}
						if(projectile.Name.Contains("Nebula"))
						{
							waveDepth += 2;
						}
					}
					WaveCollide(P,waveDepth);
				}
			}
		}
		
		int d = 0;
		public void WaveCollide(Projectile P, int depth)
		{
			int i = (int)MathHelper.Clamp((P.Center.X) / 16f,0,Main.maxTilesX-1);
			int j = (int)MathHelper.Clamp((P.Center.Y) / 16f,0,Main.maxTilesY-1);

			if(Main.tile[i,j] != null && Main.tile[i,j].active() && Main.tileSolid[Main.tile[i,j].type] && !Main.tileSolidTop[Main.tile[i,j].type])
			{
				if(P.numUpdates == 0)
				{
					d++;
				}
			}
			else if(P.numUpdates == 0 && d > 0)
			{
				d--;
			}
			if(d >= depth)
			{
				P.Kill();
			}
		}

		public void HomingBehavior(Projectile P, float accuracy = 11f, float distance = 600f)
		{
			float num236 = P.position.X;
			float num237 = P.position.Y;
			float num238 = distance;
			bool flag5 = false;
			P.ai[0] += 1f;
			if (P.ai[0] > 10f)
			{
				P.ai[0] = 10f;
				for (int num239 = 0; num239 < 200; num239++)
				{
					//bool? flag3 = NPCLoader.CanBeHitByProjectile(Main.npc[num239], P);
					//if (Main.npc[num239].CanBeChasedBy(P, false) && !npcPrevHit[num239]  && (!flag3.HasValue || flag3.Value))
                    			if (Main.npc[num239].CanBeChasedBy(P, false) && !npcPrevHit[num239])
					{
						float num240 = Main.npc[num239].position.X + (float)(Main.npc[num239].width / 2);
						float num241 = Main.npc[num239].position.Y + (float)(Main.npc[num239].height / 2);
						float num242 = Math.Abs(P.position.X + (float)(P.width / 2) - num240) + Math.Abs(P.position.Y + (float)(P.height / 2) - num241);
						if (num242 < num238 && Collision.CanHit(P.position, P.width, P.height, Main.npc[num239].position, Main.npc[num239].width, Main.npc[num239].height))
						{
							num238 = num242;
							num236 = num240;
							num237 = num241;
							flag5 = true;
						}
					}
				}
			}
			if (!flag5)
			{
				num236 = P.position.X + (float)(P.width / 2) + P.velocity.X * 100f;
				num237 = P.position.Y + (float)(P.height / 2) + P.velocity.Y * 100f;
			}
			float num243 = 8f;
			Vector2 vector22 = new Vector2(P.position.X + (float)P.width * 0.5f, P.position.Y + (float)P.height * 0.5f);
			float num244 = num236 - vector22.X;
			float num245 = num237 - vector22.Y;
			float num246 = (float)Math.Sqrt((double)(num244 * num244 + num245 * num245));
			num246 = num243 / num246;
			num244 *= num246;
			num245 *= num246;
			P.velocity.X = (P.velocity.X * accuracy + num244) / (accuracy + 1f);
			P.velocity.Y = (P.velocity.Y * accuracy + num245) / (accuracy + 1f);
		}
		
		int dustDelayCounter = 0;
		public void DustLine(Vector2 Position, Vector2 Velocity, float rotation, int dustDelay, int freq, int dustType, float scale, Color color = default(Color))
		{
			dustDelayCounter++;
			if (dustDelayCounter >= dustDelay)
			{
				int num = Math.Max((int)Math.Ceiling((float)freq*Main.gfxQuality),1);
				for (int l = 0; l < num; l++)
				{
					float x = (Position.X - Velocity.X / (float)num * (float)l);
					float y = (Position.Y - Velocity.Y / (float)num * (float)l);
					int num20 = Dust.NewDust(new Vector2(x, y), 1, 1, dustType, 0f, 0f, 100, color, scale);
					Main.dust[num20].position.X = x;
					Main.dust[num20].position.Y = y;
					Main.dust[num20].velocity *= 0f;
					Main.dust[num20].noGravity = true;
					Main.dust[num20].rotation = rotation;
				}
				dustDelayCounter = dustDelay;
			}
		}

		public void DustyDeath(Projectile projectile, int dustType, bool noGravity = true, float scale = 1f, Color color = default(Color))
		{
			Vector2 pos = projectile.position;
			int freq = 20;
			if(projectile.Name.Contains("Charge"))
			{
				freq = 40;
			}
			for (int i = 0; i < freq; i++)
			{
				int dust = Dust.NewDust(pos, projectile.width, projectile.height, dustType, 0, 0, 100, color, projectile.scale*scale);
				Main.dust[dust].velocity = new Vector2((Main.rand.Next(freq)-(freq/2))*0.125f, (Main.rand.Next(freq)-(freq/2))*0.125f);
				Main.dust[dust].noGravity = noGravity;
			}
			int sound = mod.GetSoundSlot(SoundType.Custom, "Sounds/BeamImpactSound");
			if(projectile.Name.Contains("Ice"))
			{
				sound = mod.GetSoundSlot(SoundType.Custom, "Sounds/IceImpactSound");
			}
			Main.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y,sound);
		}
		
		public bool canDiffuse = false;

		public int diffusionDustType = 0;
		public Color diffusionColor = default(Color);

		public void Diffuse(Projectile projectile, int dustType, Color color = default(Color), bool noGravity = true, float scale = 1f)
		{
			if(canDiffuse)
			{
				for (int i = 0; i < 30; i++)
				{
					int DiffuseID = mod.ProjectileType("DiffusionBeam");
					int num54 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (Main.rand.Next(50)-25)*0.1f, (Main.rand.Next(50)-25)*0.1f, DiffuseID,(int)((float)projectile.damage/3f),0.1f,projectile.owner);
					Projectile pr = Main.projectile[num54];
					pr.tileCollide = projectile.tileCollide;
					pr.Name = projectile.Name;
					MProjectile mpr = (MProjectile)pr.modProjectile;
					mpr.diffusionDustType = dustType;
					mpr.diffusionColor = color;
				}
				int sound = mod.GetSoundSlot(SoundType.Custom, "Sounds/BeamImpactSound");
				if(projectile.Name.Contains("Ice"))
				{
					sound = mod.GetSoundSlot(SoundType.Custom, "Sounds/IceImpactSound");
				}
				Main.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y,sound);
			}
			else
			{
				DustyDeath(projectile, dustType, noGravity, scale, color);
			}
		}

		public void DrawCentered(Projectile projectile, SpriteBatch sb)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Main.projectileTexture[projectile.type];
			int num108 = tex.Height / Main.projFrames[projectile.type];
			int y4 = num108 * projectile.frame;
			sb.Draw(tex, new Vector2((float)((int)(projectile.Center.X - Main.screenPosition.X)), (float)((int)(projectile.Center.Y - Main.screenPosition.Y + projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2((float)tex.Width/2f, (float)num108/2f), projectile.scale, effects, 0f);
		}
		
		public void DrawCenteredTrail(Projectile projectile, SpriteBatch sb, int amount = 10, float scaleDrop = 0.5f)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Main.projectileTexture[projectile.type];
			int num108 = tex.Height / Main.projFrames[projectile.type];
			int y4 = num108 * projectile.frame;

			int amt = Math.Min(amount,10);
			for(int i = amt-1; i > -1; i--)
			{
				Color color23 = Color.White;
				color23 = projectile.GetAlpha(color23);
				color23 *= (float)(amt - i) / ((float)amt);
				float scale = MathHelper.Lerp(projectile.scale, projectile.scale*scaleDrop, (float)i / amt);
				sb.Draw(tex, (projectile.oldPos[i] + new Vector2((float)projectile.width/2,(float)projectile.height/2)) - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), color23, projectile.oldRot[i], new Vector2((float)tex.Width/2f, (float)num108/2f), scale, effects, 0f);
			}
			sb.Draw(tex, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2((float)tex.Width/2f, (float)num108/2f), projectile.scale, effects, 0f);
		}

		bool drawFlag = false;
		public void PlasmaDraw(Projectile projectile,Player player, SpriteBatch sb)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Main.projectileTexture[projectile.type];
			int num108 = tex.Height / Main.projFrames[projectile.type];
			int y4 = num108 * projectile.frame;
			
			float h = ((float)num108*projectile.scale);
			
			float dist = MathHelper.Clamp((Vector2.Distance(projectile.Center,player.Center)+((float)projectile.height/2f))/h,0f,1f);
			int height = (int)((float)num108*dist);
			if(dist >= 1f)
			{
				drawFlag = true;
			}
			if(drawFlag)
			{
				height = num108;
			}
			sb.Draw(tex, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, height)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2((float)tex.Width/2f, (float)projectile.height/2f), projectile.scale, effects, 0f);
		}
		public void PlasmaDrawTrail(Projectile projectile,Player player, SpriteBatch sb, int amount = 10, float scaleDrop = 0.5f, Color color = default(Color))
		{
			Color color2 = Color.White;
			if(color != default(Color))
			{
				color2 = color;
			}
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Main.projectileTexture[projectile.type];
			int num108 = tex.Height / Main.projFrames[projectile.type];
			int y4 = num108 * projectile.frame;
			
			float h = ((float)num108*projectile.scale);
			
			float dist = MathHelper.Clamp((Vector2.Distance(projectile.Center,player.Center)+((float)projectile.height/2f))/h,0f,1f);
			int height = (int)((float)num108*dist);
			if(dist >= 1f)
			{
				drawFlag = true;
			}
			else
			{
				for(int i = 0; i < projectile.oldPos.Length; i++)
				{
					projectile.oldPos[i] = projectile.position;
				}
				for(int i = 0; i < projectile.oldRot.Length; i++)
				{
					projectile.oldRot[i] = projectile.rotation;
				}
			}
			if(drawFlag)
			{
				height = num108;
			}
			int amt = Math.Min(amount,10);
			for(int i = amt-1; i > -1; i--)
			{
				Color color23 = color2;
				color23 = projectile.GetAlpha(color23);
				color23 *= (float)(amt - i) / ((float)amt);
				float scale = MathHelper.Lerp(projectile.scale, projectile.scale*scaleDrop, (float)i / amt);
				sb.Draw(tex, (projectile.oldPos[i] + new Vector2((float)projectile.width/2,(float)projectile.height/2)) - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, height)), color23, projectile.oldRot[i], new Vector2((float)tex.Width/2f, (float)projectile.height/2f), scale, effects, 0f);
			}
			sb.Draw(tex, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, height)), projectile.GetAlpha(color2), projectile.rotation, new Vector2((float)tex.Width/2f, (float)projectile.height/2f), projectile.scale, effects, 0f);
		}

        /* Networking section */
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(waveDir);
            writer.Write(waveStyle);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            waveDir = reader.ReadInt32();
            waveStyle = reader.ReadInt32();
        }
    }
}
