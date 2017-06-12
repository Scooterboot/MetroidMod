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

		/*public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if((projectile.Name.Contains("Plasma") && projectile.Name.Contains("Red")) || projectile.Name.Contains("Nova"))
			{
				damage += (int)(((float)target.defense*0.5f) * 0.1f);
			}
		}*/
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
				target.AddBuff(mod.BuffType("IceFreeze"),300,true);
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

		public int waveStyle = 0;
		public int delay = 0;
		public int waveDir = -1;
		public float wavesPerSecond = 0f;
		public float amplitude = 0f;
		public int waveDepth = 8;
		float t = 0f;
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
				}
				delay = Math.Max(delay - 1, 0);
				i *= projectile.direction;
				if(!spaze)
				{
					i *= waveDir;
				}
				float shift = amplitude * (float)Math.Sin(t) * i;
				
				pos += P.velocity;
				
				float rot = (float)Math.Atan2((P.velocity.Y),(P.velocity.X));
				P.position.X = pos.X + (float)Math.Cos(rot+((float)Math.PI/2))*shift;
				P.position.Y = pos.Y + (float)Math.Sin(rot+((float)Math.PI/2))*shift;
				
				if(!P.tileCollide)
				{
					waveDepth = 8;
					if(projectile.Name.Contains("Spazer") || projectile.Name.Contains("Wide"))
					{
						waveDepth = 11;
					}
					if(projectile.Name.Contains("Plasma") || projectile.Name.Contains("Nova"))
					{
						waveDepth = 14;
					}
					if(projectile.Name.Contains("Charge"))
					{
						waveDepth += 2;
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

			if(Main.tile[i,j].active() && Main.tileSolid[Main.tile[i,j].type] && !Main.tileSolidTop[Main.tile[i,j].type])
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
					Main.projectile[num54].tileCollide = projectile.tileCollide;
					MProjectile mpr = (MProjectile)Main.projectile[num54].modProjectile;
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
	}
}
