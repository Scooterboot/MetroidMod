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
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

using MetroidModPorted.Common.Players;
using MetroidModPorted.Content.DamageClasses;

namespace MetroidModPorted.Content.Projectiles
{
	public abstract class MProjectile : ModProjectile
	{
		public MProjectile mProjectile;
		public MProjectile()
		{
			mProjectile = this;
		}
		
		public bool hunter = false;
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			
			//Projectile.melee = false;
			//Projectile.ranged = false;
			//Projectile.magic = false;
			//Projectile.thrown = false;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			hunter = true;
			
			Projectile.extraUpdates = 2;
			for(int i = 0; i < Projectile.oldPos.Length; i++)
			{
				Projectile.oldPos[i] = Projectile.position;
			}
			
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			
			for(int i = 0; i < Projectile.oldRot.Length; i++)
			{
				Projectile.oldRot[i] = Projectile.rotation;
			}
		}
		
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[Projectile.owner];
			if (hunter && Main.rand.Next(1, 101) <= HunterDamagePlayer.ModPlayer(player).HunterCrit+player.inventory[player.selectedItem].crit)
			{
				crit = true;
			}
		}
		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
		{
			Player player = Main.player[Projectile.owner];
			if (hunter && Main.rand.Next(1, 101) <= HunterDamagePlayer.ModPlayer(player).HunterCrit+player.inventory[player.selectedItem].crit)
			{
				crit = true;
			}
		}

		bool[] npcPrevHit = new bool[Main.maxNPCs];
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{	
			if(Projectile.Name.Contains("Plasma") && Projectile.Name.Contains("Red"))
			{
				if(Projectile.Name.Contains("Ice"))
				{
					target.AddBuff(44,300);
				}
				else
				{
					target.AddBuff(24,300);
				}
			}
			
			if(Projectile.Name.Contains("Nova"))
			{
				if(Projectile.Name.Contains("Ice"))
				{
					target.AddBuff(44,300);
				}
				else
				{
					target.AddBuff(39,300);
				}
			}
			if(Projectile.Name.Contains("Ice") || Projectile.Name.Contains("Stardust"))
			{
				string buffName = "IceFreeze";
				if(Projectile.Name.Contains("Missile"))
					buffName = "InstantFreeze";

				target.AddBuff(Mod.Find<ModBuff>(buffName).Type, 300);
			}
			
			if(Projectile.Name.Contains("Solar"))
			{
				target.AddBuff(189,300);
			}
			
			if(Projectile.penetrate != 1)
			{
				npcPrevHit[target.whoAmI] = true;
			}
		}
		public override void PostAI()
		{
			for (int i = Projectile.oldPos.Length-1; i > 0; i--)
			{
				Projectile.oldPos[i] = Projectile.oldPos[i - 1];
			}
			Projectile.oldPos[0] = Projectile.position;


			for (int i = Projectile.oldRot.Length-1; i > 0; i--)
			{
				Projectile.oldRot[i] = Projectile.oldRot[i - 1];
			}
			Projectile.oldRot[0] = Projectile.rotation;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
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
				waveStyle = (int)P.ai[1];
				P.ai[1] = 0;
			}
			else
			{
				float increment = ((float)Math.PI*2)/60f;
				float i = 1;
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
					if(P.Name.Contains("Hyper"))
					{
						i = 1.5f;
					}
				}
				if(waveStyle == 4)
				{
					i = -2;
					if(P.Name.Contains("Hyper"))
					{
						i = -1.5f;
					}
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
				i *= P.direction;
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
				
				if(!P.tileCollide && !P.Name.Contains("Hyper"))
				{
					waveDepth = 4;
					if(P.Name.Contains("Spazer"))
					{
						waveDepth = 6;
					}
					if(P.Name.Contains("Plasma"))
					{
						waveDepth = 8;
					}
					if(P.Name.Contains("V2"))
					{
						waveDepth = 6;
					}
					if(P.Name.Contains("Wide"))
					{
						waveDepth = 9;
					}
					if(P.Name.Contains("Nova"))
					{
						waveDepth = 12;
					}
					if(P.Name.Contains("Nebula"))
					{
						waveDepth = 8;
					}
					if(P.Name.Contains("Vortex"))
					{
						waveDepth = 12;
					}
					if(P.Name.Contains("Solar"))
					{
						waveDepth = 16;
					}
					if(P.Name.Contains("Charge"))
					{
						waveDepth += 2;
						if(P.Name.Contains("V2") || P.Name.Contains("Wide") || P.Name.Contains("Nova"))
						{
							waveDepth += 1;
						}
						if(P.Name.Contains("Nebula"))
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

			if(Main.tile[i,j] != null && Main.tile[i,j].HasTile && Main.tileSolid[Main.tile[i,j].TileType] && !Main.tileSolidTop[Main.tile[i,j].TileType])
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

		public void HomingBehavior(Projectile P, float speed = 8f, float accuracy = 11f, float distance = 600f)
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
			float num243 = speed;
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

		public void DustyDeath(Projectile Projectile, int dustType, bool noGravity = true, float scale = 1f, Color color = default(Color))
		{
			Vector2 pos = Projectile.position;
			int freq = 20;
			if(Projectile.Name.Contains("Charge"))
			{
				freq = 40;
			}
			for (int i = 0; i < freq; i++)
			{
				int dust = Dust.NewDust(pos, Projectile.width, Projectile.height, dustType, 0, 0, 100, color, Projectile.scale*scale);
				Main.dust[dust].velocity = new Vector2((Main.rand.Next(freq)-(freq/2))*0.125f, (Main.rand.Next(freq)-(freq/2))*0.125f);
				Main.dust[dust].noGravity = noGravity;
			}
			SoundStyle sound = new($"{MetroidModPorted.Instance.Name}/Assets/Sounds/BeamImpactSound");
			if(Projectile.Name.Contains("Ice"))
			{
				sound = new($"{MetroidModPorted.Instance.Name}/Assets/Sounds/IceImpactSound");
			}
			SoundEngine.PlaySound(sound, Projectile.Center);
		}
		
		public bool canDiffuse = false;

		public void Diffuse(Projectile Projectile, int dustType, Color color = default(Color), bool noGravity = true, float scale = 1f)
		{
			if(canDiffuse)
			{
				if (Projectile.owner != Main.myPlayer) return;

				if(color == default(Color))
				{
					color = Color.White;
				}
				var entitySource = Projectile.GetSource_FromAI();
				Vector2 vel = Vector2.Zero;
				for (int i = 0; i < 30; i++)
				{
					int DiffuseID = ModContent.ProjectileType<DiffusionBeam>();
					vel = new Vector2((Main.rand.Next(50) - 25) * 0.1f, (Main.rand.Next(50) - 25) * 0.1f);
					Projectile p = Main.projectile[Projectile.NewProjectile(entitySource, Projectile.Center, vel, DiffuseID, (int)(Projectile.damage / 3f),
						0.1f, Projectile.owner, dustType, (color.R << 16 | color.G << 8 | color.B))];
					p.tileCollide = Projectile.tileCollide;
					p.Name = Projectile.Name;
					p.netUpdate = true;
				}

				SoundStyle sound = new($"{MetroidModPorted.Instance.Name}/Assets/Sounds/BeamImpactSound");
				if(Projectile.Name.Contains("Ice"))
				{
					sound = new($"{MetroidModPorted.Instance.Name}/Assets/Sounds/IceImpactSound");
				}
				SoundEngine.PlaySound(sound, Projectile.Center);
			}
			else
			{
				DustyDeath(Projectile, dustType, noGravity, scale, color);
			}
		}

		public void DrawCentered(Projectile Projectile, SpriteBatch sb)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num108 = tex.Height / Main.projFrames[Projectile.type];
			int y4 = num108 * Projectile.frame;
			sb.Draw(tex, new Vector2((float)((int)(Projectile.Center.X - Main.screenPosition.X)), (float)((int)(Projectile.Center.Y - Main.screenPosition.Y + Projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2((float)tex.Width/2f, (float)num108/2f), Projectile.scale, effects, 0f);
		}
		
		public void DrawCenteredTrail(Projectile Projectile, SpriteBatch sb, int amount = 10, float scaleDrop = 0.5f)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num108 = tex.Height / Main.projFrames[Projectile.type];
			int y4 = num108 * Projectile.frame;

			int amt = Math.Min(amount,10);
			for(int i = amt-1; i > -1; i--)
			{
				Color color23 = Color.White;
				color23 = Projectile.GetAlpha(color23);
				color23 *= (float)(amt - i) / ((float)amt);
				float scale = MathHelper.Lerp(Projectile.scale, Projectile.scale*scaleDrop, (float)i / amt);
				sb.Draw(tex, (Projectile.oldPos[i] + new Vector2((float)Projectile.width/2,(float)Projectile.height/2)) - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), color23, Projectile.oldRot[i], new Vector2((float)tex.Width/2f, (float)num108/2f), scale, effects, 0f);
			}
			sb.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2((float)tex.Width/2f, (float)num108/2f), Projectile.scale, effects, 0f);
		}

		bool drawFlag = false;
		public void PlasmaDraw(Projectile Projectile,Player player, SpriteBatch sb)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num108 = tex.Height / Main.projFrames[Projectile.type];
			int y4 = num108 * Projectile.frame;
			
			float h = ((float)num108*Projectile.scale);
			
			float dist = MathHelper.Clamp((Vector2.Distance(Projectile.Center,player.Center)+((float)Projectile.height/2f))/h,0f,1f);
			int height = (int)((float)num108*dist);
			if(dist >= 1f)
			{
				drawFlag = true;
			}
			if(drawFlag)
			{
				height = num108;
			}
			sb.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, height)), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2((float)tex.Width/2f, (float)Projectile.height/Projectile.scale/2f), Projectile.scale, effects, 0f);
		}
		public void PlasmaDrawTrail(Projectile Projectile,Player player, SpriteBatch sb, int amount = 10, float scaleDrop = 0.5f, Color color = default(Color))
		{
			Color color2 = Color.White;
			if(color != default(Color))
			{
				color2 = color;
			}
			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num108 = tex.Height / Main.projFrames[Projectile.type];
			int y4 = num108 * Projectile.frame;
			
			float h = ((float)num108*Projectile.scale);
			
			float dist = MathHelper.Clamp((Vector2.Distance(Projectile.Center,player.Center)+((float)Projectile.height/2f))/h,0f,1f);
			int height = (int)((float)num108*dist);
			if(dist >= 1f)
			{
				drawFlag = true;
			}
			else
			{
				for(int i = 0; i < Projectile.oldPos.Length; i++)
				{
					Projectile.oldPos[i] = Projectile.position;
				}
				for(int i = 0; i < Projectile.oldRot.Length; i++)
				{
					Projectile.oldRot[i] = Projectile.rotation;
				}
			}
			if(drawFlag)
			{
				height = num108;
			}
			int amt = Math.Min(amount,10);
			for(int i = amt-1; i > -1; i--)
			{
				Vector2 center = Projectile.oldPos[i] + new Vector2((float)Projectile.width/2,(float)Projectile.height/2);
				float oldDist = MathHelper.Clamp((Vector2.Distance(center,player.Center)+((float)Projectile.height/2f))/h,0f,1f);
				int oldHeight = (int)((float)num108*oldDist);
				
				Color color23 = color2;
				color23 = Projectile.GetAlpha(color23);
				color23 *= (float)(amt - i) / ((float)amt);
				float scale = MathHelper.Lerp(Projectile.scale, Projectile.scale*scaleDrop, (float)i / amt);
				sb.Draw(tex, center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, oldHeight)), color23, Projectile.oldRot[i], new Vector2((float)tex.Width/2f, (float)Projectile.height/Projectile.scale/2f), scale, effects, 0f);
			}
			sb.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, height)), Projectile.GetAlpha(color2), Projectile.rotation, new Vector2((float)tex.Width/2f, (float)Projectile.height/Projectile.scale/2f), Projectile.scale, effects, 0f);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(canDiffuse);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			canDiffuse = reader.ReadBoolean();
		}
	}
}
