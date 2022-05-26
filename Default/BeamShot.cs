using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

using MetroidModPorted.Content.DamageClasses;
using MetroidModPorted.Content.Projectiles;

namespace MetroidModPorted.Default
{
	/*
	[Autoload(false)]
	internal class BeamShot : ModProjectile
	{
		[CloneByReference]
		public ModBeam[] modBeam;
		[CloneByReference]
		public BeamCombination beamCombo;
		[CloneByReference]
		public BeamShot beamShot;

		public BeamShot()
		{
			beamShot = this;
		}

		protected override bool CloneNewInstances => true;

		public override string Texture => beamCombo.projectileTexture;//modBeam.BeamProjectileTexture;

		public override string Name => MMPUtils.ConcatBeamNames(modBeam)+"Shot";//modBeam.Name + "Shot";

		public BeamShot(params ModBeam[] modBeams)
		{
			modBeam = modBeams;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(MMPUtils.ConcatBeamNames(modBeam));//modBeam[].Name[0].ToString().ToUpper() + modBeam[].Name.Remove(0, 1).ToLower() + " Beam");
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

			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			hunter = true;

			Projectile.extraUpdates = 2;
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				Projectile.oldPos[i] = Projectile.position;
			}

			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;

			for (int i = 0; i < Projectile.oldRot.Length; i++)
			{
				Projectile.oldRot[i] = Projectile.rotation;
			}
		}

		bool[] npcPrevHit = new bool[Main.maxNPCs];
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.penetrate != 1)
			{
				npcPrevHit[target.whoAmI] = true;
			}
		}
		public override void PostAI()
		{
			for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
			{
				Projectile.oldPos[i] = Projectile.oldPos[i - 1];
			}
			Projectile.oldPos[0] = Projectile.position;


			for (int i = Projectile.oldRot.Length - 1; i > 0; i--)
			{
				Projectile.oldRot[i] = Projectile.oldRot[i - 1];
			}
			Projectile.oldRot[0] = Projectile.rotation;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.width = 0;
			Projectile.height = 0;
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
		Vector2 pos = new Vector2(0, 0);
		bool initialized = false;
		void initialize(Projectile P)
		{
			pos = P.position;
			initialized = true;
		}
		public void WaveBehavior(Projectile P, bool spaze = false)
		{
			if (!initialized)
			{
				initialize(P);
				waveStyle = (int)P.ai[1];
				P.ai[1] = 0;
			}
			else
			{
				float increment = ((float)Math.PI * 2) / 60f;
				float i = 1;
				if (waveStyle == 1)
				{
					i = -1;
				}
				if (waveStyle == 2)
				{
					i = 0;
				}
				if (waveStyle == 3)
				{
					i = 2;
					if (P.Name.Contains("Hyper"))
					{
						i = 1.5f;
					}
				}
				if (waveStyle == 4)
				{
					i = -2;
					if (P.Name.Contains("Hyper"))
					{
						i = -1.5f;
					}
				}
				if (delay <= 0)
				{
					if (spaze)
					{
						t = Math.Min(t + increment * wavesPerSecond, (float)Math.PI / 2);
					}
					else
					{
						t += increment * wavesPerSecond;
					}
					if (t >= (float)Math.PI * 2)
					{
						t -= (float)Math.PI * 2;
					}

					if (waveStyle == 3 || waveStyle == 4)
					{
						t2 = Math.Min(t2 + increment * wavesPerSecond / 4, (float)Math.PI / 2);
					}
				}
				delay = Math.Max(delay - 1, 0);
				i *= P.direction;
				if (!spaze)
				{
					i *= waveDir;
				}
				float shift = amplitude * (float)Math.Sin(t) * i;
				if (!spaze && (waveStyle == 3 || waveStyle == 4))
				{
					shift = amplitude * (float)Math.Sin(t - t2) * i;
				}

				pos += P.velocity;

				float rot = (float)Math.Atan2((P.velocity.Y), (P.velocity.X));
				P.position.X = pos.X + (float)Math.Cos(rot + ((float)Math.PI / 2)) * shift;
				P.position.Y = pos.Y + (float)Math.Sin(rot + ((float)Math.PI / 2)) * shift;

				if (!P.tileCollide && !P.Name.Contains("Hyper"))
				{
					/*waveDepth = 4;
					if (P.Name.Contains("Spazer"))
					{
						waveDepth = 6;
					}
					if (P.Name.Contains("Plasma"))
					{
						waveDepth = 8;
					}
					if (P.Name.Contains("V2"))
					{
						waveDepth = 6;
					}
					if (P.Name.Contains("Wide"))
					{
						waveDepth = 9;
					}
					if (P.Name.Contains("Nova"))
					{
						waveDepth = 12;
					}
					if (P.Name.Contains("Nebula"))
					{
						waveDepth = 8;
					}
					if (P.Name.Contains("Vortex"))
					{
						waveDepth = 12;
					}
					if (P.Name.Contains("Solar"))
					{
						waveDepth = 16;
					}
					if (P.Name.Contains("Charge"))
					{
						waveDepth += 2;
						if (P.Name.Contains("V2") || P.Name.Contains("Wide") || P.Name.Contains("Nova"))
						{
							waveDepth += 1;
						}
						if (P.Name.Contains("Nebula"))
						{
							waveDepth += 2;
						}
					}* /
					WaveCollide(P, waveDepth);
				}
			}
		}

		int d = 0;
		public void WaveCollide(Projectile P, int depth)
		{
			int i = (int)MathHelper.Clamp((P.Center.X) / 16f, 0, Main.maxTilesX - 1);
			int j = (int)MathHelper.Clamp((P.Center.Y) / 16f, 0, Main.maxTilesY - 1);

			if (Main.tile[i, j] != null && Main.tile[i, j].HasTile && Main.tileSolid[(int)Main.tile[i, j].BlockType] && !Main.tileSolidTop[(int)Main.tile[i, j].BlockType])
			{
				if (P.numUpdates == 0)
				{
					d++;
				}
			}
			else if (P.numUpdates == 0 && d > 0)
			{
				d--;
			}
			if (d >= depth)
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
				int num = Math.Max((int)Math.Ceiling((float)freq * Main.gfxQuality), 1);
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

		public static void DustyDeath(Projectile projectile, int dustType, bool noGravity = true, float scale = 1f, Color color = default(Color))
		{
			Vector2 pos = projectile.position;
			int freq = 20;
			if (projectile.Name.Contains("Charge"))
			{
				freq = 40;
			}
			for (int i = 0; i < freq; i++)
			{
				int dust = Dust.NewDust(pos, projectile.width, projectile.height, dustType, 0, 0, 100, color, projectile.scale * scale);
				Main.dust[dust].velocity = new Vector2((Main.rand.Next(freq) - (freq / 2)) * 0.125f, (Main.rand.Next(freq) - (freq / 2)) * 0.125f);
				Main.dust[dust].noGravity = noGravity;
			}
			SoundStyle sound = new($"{MetroidModPorted.Instance.Name}/Assets/Sounds/BeamImpactSound");
			if (projectile.Name.Contains("Ice"))
			{
				sound = new($"{MetroidModPorted.Instance.Name}/Assets/Sounds/IceImpactSound");
			}
			SoundEngine.PlaySound(sound, projectile.Center);
		}

		public bool canDiffuse = false;

		public void Diffuse(Projectile projectile, int dustType, Color color = default(Color), bool noGravity = true, float scale = 1f)
		{
			if (canDiffuse)
			{
				if (projectile.owner != Main.myPlayer) return;

				if (color == default(Color))
				{
					color = Color.White;
				}
				Vector2 vel = Vector2.Zero;
				for (int i = 0; i < 30; i++)
				{
					int DiffuseID = ModContent.ProjectileType<DiffusionBeam>();
					vel = new Vector2((Main.rand.Next(50) - 25) * 0.1f, (Main.rand.Next(50) - 25) * 0.1f);
					Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), projectile.Center, vel, DiffuseID, (int)(projectile.damage / 3f),
						0.1f, projectile.owner, dustType, (color.R << 16 | color.G << 8 | color.B))];
					p.tileCollide = projectile.tileCollide;
					p.Name = projectile.Name;
					p.netUpdate = true;
				}

				SoundStyle sound = new($"{MetroidModPorted.Instance.Name}/Assets/Sounds/BeamImpactSound");
				if (projectile.Name.Contains("Ice"))
				{
					sound = new($"{MetroidModPorted.Instance.Name}/Assets/Sounds/IceImpactSound");
				}
				SoundEngine.PlaySound(sound, projectile.Center);//SoundLoader.CustomSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, sound);
			}
			else
			{
				DustyDeath(projectile, dustType, noGravity, scale, color);
			}
		}

		public static void DrawCentered(Projectile projectile)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;//Main.projectileTexture[projectile.type];
			int num108 = tex.Height / Main.projFrames[projectile.type];
			int y4 = num108 * projectile.frame;
			Main.EntitySpriteDraw(tex, new Vector2((float)((int)(projectile.Center.X - Main.screenPosition.X)), (float)((int)projectile.Center.Y - Main.screenPosition.Y + projectile.gfxOffY)), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2((float)tex.Width / 2f, (float)num108 / 2f), projectile.scale, effects, 0);
		}

		public static void DrawCenteredTrail(Projectile projectile, int amount = 10, float scaleDrop = 0.5f)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[projectile.ModProjectile.Type].Value;//Main.projectileTexture[projectile.type];
			int num108 = tex.Height / Main.projFrames[projectile.type];
			int y4 = num108 * projectile.frame;

			int amt = Math.Min(amount, 10);
			for (int i = amt - 1; i > -1; i--)
			{
				Color color23 = Color.White;
				color23 = projectile.GetAlpha(color23);
				color23 *= (float)(amt - i) / ((float)amt);
				float scale = MathHelper.Lerp(projectile.scale, projectile.scale * scaleDrop, (float)i / amt);
				Main.EntitySpriteDraw(tex, (projectile.oldPos[i] + new Vector2((float)projectile.width / 2, (float)projectile.height / 2)) - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), color23, projectile.oldRot[i], new Vector2((float)tex.Width / 2f, (float)num108 / 2f), scale, effects, 0);
			}
			Main.EntitySpriteDraw(tex, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2((float)tex.Width / 2f, (float)num108 / 2f), projectile.scale, effects, 0);
		}

		bool drawFlag = false;
		public void PlasmaDraw(Projectile projectile, Player player)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[projectile.ModProjectile.Type].Value;//Main.projectileTexture[projectile.type];
			int num108 = tex.Height / Main.projFrames[projectile.type];
			int y4 = num108 * projectile.frame;

			float h = ((float)num108 * projectile.scale);

			float dist = MathHelper.Clamp((Vector2.Distance(projectile.Center, player.Center) + ((float)projectile.height / 2f)) / h, 0f, 1f);
			int height = (int)((float)num108 * dist);
			if (dist >= 1f)
			{
				drawFlag = true;
			}
			if (drawFlag)
			{
				height = num108;
			}
			Main.EntitySpriteDraw(tex, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, height)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2((float)tex.Width / 2f, (float)projectile.height / projectile.scale / 2f), projectile.scale, effects, 0);
		}
		public void PlasmaDrawTrail(Projectile projectile, Player player, int amount = 10, float scaleDrop = 0.5f, Color color = default(Color))
		{
			Color color2 = Color.White;
			if (color != default(Color))
			{
				color2 = color;
			}
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[projectile.ModProjectile.Type].Value;//Main.projectileTexture[projectile.type];
			int num108 = tex.Height / Main.projFrames[projectile.type];
			int y4 = num108 * projectile.frame;

			float h = ((float)num108 * projectile.scale);

			float dist = MathHelper.Clamp((Vector2.Distance(projectile.Center, player.Center) + ((float)projectile.height / 2f)) / h, 0f, 1f);
			int height = (int)((float)num108 * dist);
			if (dist >= 1f)
			{
				drawFlag = true;
			}
			else
			{
				for (int i = 0; i < projectile.oldPos.Length; i++)
				{
					projectile.oldPos[i] = projectile.position;
				}
				for (int i = 0; i < projectile.oldRot.Length; i++)
				{
					projectile.oldRot[i] = projectile.rotation;
				}
			}
			if (drawFlag)
			{
				height = num108;
			}
			int amt = Math.Min(amount, 10);
			for (int i = amt - 1; i > -1; i--)
			{
				Vector2 center = projectile.oldPos[i] + new Vector2((float)projectile.width / 2, (float)projectile.height / 2);
				float oldDist = MathHelper.Clamp((Vector2.Distance(center, player.Center) + ((float)projectile.height / 2f)) / h, 0f, 1f);
				int oldHeight = (int)((float)num108 * oldDist);

				Color color23 = color2;
				color23 = projectile.GetAlpha(color23);
				color23 *= (float)(amt - i) / ((float)amt);
				float scale = MathHelper.Lerp(projectile.scale, projectile.scale * scaleDrop, (float)i / amt);
				Main.EntitySpriteDraw(tex, center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, oldHeight)), color23, projectile.oldRot[i], new Vector2((float)tex.Width / 2f, (float)projectile.height / projectile.scale / 2f), scale, effects, 0);
			}
			Main.EntitySpriteDraw(tex, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, y4, tex.Width, height)), projectile.GetAlpha(color2), projectile.rotation, new Vector2((float)tex.Width / 2f, (float)projectile.height / projectile.scale / 2f), projectile.scale, effects, 0);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(canDiffuse);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			canDiffuse = reader.ReadBoolean();
		}

		public override ModProjectile Clone(Projectile newEntity)
		{
			var inst = (BeamShot)base.Clone(newEntity);
			inst.modBeam = modBeam;
			inst.beamShot = beamShot;
			inst.canDiffuse = canDiffuse;
			return inst;
		}

		public override ModProjectile NewInstance(Projectile entity)
		{
			BeamShot inst = (BeamShot)base.NewInstance(entity);
			inst.modBeam = modBeam;
			inst.beamShot = beamShot;
			inst.canDiffuse = canDiffuse;
			return inst;
		}
	}
	*/
}
