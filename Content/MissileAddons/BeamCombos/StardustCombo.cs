using System;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MissileAddons.BeamCombos
{
	public class StardustCombo : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override Color PrimaryColor => MetroidMod.iceColor;
		public override Color SecondaryColor => MetroidMod.iceSecondaryColor;
		public override int ShotDust => DustID.IceTorch;
		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Charge;
			base.SetStaticDefaults();
		}
		public override void SetItemDefaults(Item item)
		{
			base.SetItemDefaults(item);
			Item.value = 70000;
			Item.rare = ItemRarityID.LightRed;
		}
		public override void SetProjectileDefaults(MProjectile mProjectile)
		{
			Projectile Projectile = mProjectile.Projectile;
			base.SetProjectileDefaults(mProjectile);
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.timeLeft = 1000;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FragmentStardust, 15)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
		public override void OnKill(MProjectile mProjectile, int timeLeft)
		{
			Projectile P = mProjectile.Projectile;

			P.position.X = P.position.X + (float)(P.width / 2);
			P.position.Y = P.position.Y + (float)(P.height / 2);
			P.width += 48;
			P.height += 48;
			P.position.X = P.position.X - (float)(P.width / 2);
			P.position.Y = P.position.Y - (float)(P.height / 2);

			for (int i = 0; i < 25; i++)
			{
				int d = Dust.NewDust(P.position, P.width, P.height, 88, 0f, 0f, 100, default(Color), 5f);
				Main.dust[d].velocity *= 1.4f;
				Main.dust[d].noGravity = true;
				d = Dust.NewDust(P.position, P.width, P.height, 87, 0f, 0f, 100, default(Color), 3f);
				Main.dust[d].velocity *= 1.4f;
				Main.dust[d].noGravity = true;
			}

			var entitySource = P.GetSource_Death();
			for (int i = 0; i < 8; i++)
			{
				float angle = ((float)Math.PI / 4) * i;
				int num54 = Projectile.NewProjectile(entitySource, P.Center.X, P.Center.Y, 0f, 0f, ModContent.ProjectileType<StardustComboDiffusionShot>(), P.damage, P.knockBack, P.owner);
				StardustComboDiffusionShot difShot = (StardustComboDiffusionShot)Main.projectile[num54].ModProjectile;
				difShot.spin = angle;
			}

			float k = 0f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<StardustFrozenTerrain>() && Main.projectile[i].owner == P.owner)
				{
					if (k < Main.projectile[i].ai[0])
					{
						k = Main.projectile[i].ai[0];
					}
				}
			}

			int x = (int)MathHelper.Clamp(P.Center.X / 16, 0, Main.maxTilesX - 2);
			int y = (int)MathHelper.Clamp(P.Center.Y / 16, 0, Main.maxTilesY - 2);
			Vector2 pos = new Vector2((float)x * 16f + 8f, (float)y * 16f + 8f);
			int ft = Projectile.NewProjectile(P.GetSource_Death(), pos.X, pos.Y, 0f, 0f, ModContent.ProjectileType<StardustFrozenTerrain>(), P.damage, P.knockBack, P.owner);
			Main.projectile[ft].ai[0] = k + 1;

			SoundEngine.PlaySound(Sounds.Items.Weapons.IceSpreaderImpactSound, P.Center);
			SoundEngine.PlaySound(Sounds.Items.Weapons.StardustAfterImpactSound, P.Center);
		}
		public override void OnHitNPC(MProjectile mProjectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(), 300, true);
		} //replace with inflicts buff later
		public override bool PreDrawProjectile(MProjectile mProjectile, ref Color lightColor)
		{
			Projectile Projectile = mProjectile.Projectile;
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch, ShotTexture);
			return false;
		}
	}
	public class StardustComboDiffusionShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Dragon");
		}

		bool initialised = false;
		float radius = 5f;//0.0f;
		public float spin = 0.0f;
		float SpinIncrease = 0.05f;
		Vector2 basePosition = new Vector2(0f, 0f);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.extraUpdates = 0;
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.scale = 1f;
			Projectile.timeLeft = 140;//175;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}

		const int SegLength = 6;
		Vector2[] segmentPos = new Vector2[SegLength];
		float[] segmentRot = new float[SegLength];

		public void initialise()
		{
			basePosition = Projectile.Center;
			for (int i = 0; i < segmentPos.Length; i++)
			{
				segmentPos[i] = Projectile.Center;
			}
			initialised = true;
		}
		public override void AI()
		{
			Projectile P = Projectile;
			if (!initialised)
			{
				initialise();
			}
			SpinIncrease += 0.0005f;
			radius += 3.0f;
			spin += SpinIncrease;
			P.position = (basePosition - new Vector2(P.width / 2, P.height / 2)) + spin.ToRotationVector2() * radius;

			Vector2 vel = P.position - P.oldPos[0];
			if (vel != Vector2.Zero)
			{
				vel.Normalize();
				P.rotation = vel.ToRotation() + MathHelper.PiOver2;
			}

			Color color = MetroidMod.iceColor;
			Lighting.AddLight(P.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			segmentPos[0] = P.Center;
			segmentRot[0] = P.rotation;

			for (int i = 1; i < segmentPos.Length; i++)
			{
				Vector2 pos = segmentPos[i - 1] - segmentPos[i];
				segmentRot[i] = pos.ToRotation() + MathHelper.PiOver2;
				float len = pos.Length();
				int width = P.width / 2;

				len = (len - (float)width) / len;
				pos.X *= len;
				pos.Y *= len;
				segmentPos[i] += pos;

				if (Main.rand.NextBool(30))
				{
					Vector2 dustPos = segmentPos[i] - new Vector2(P.width / 2, P.height / 2);
					int num1049 = Dust.NewDust(dustPos, P.width, P.height, 135, 0f, 0f, 0, default(Color), 2f);
					Main.dust[num1049].noGravity = true;
					Main.dust[num1049].fadeIn = 2f;
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Projectile P = Projectile;
			for (int i = 0; i < segmentPos.Length; i++)
			{
				Vector2 pos = segmentPos[i] - new Vector2(P.width / 2, P.height / 2);
				Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, projHitbox.Width, projHitbox.Height);
				return rect.Intersects(targetHitbox);
			}
			return null;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(), 600, true);
			target.AddBuff(BuffID.Frostburn, 600, true);
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				for (int num70 = 0; num70 < 5; num70++)
				{
					int num71 = Dust.NewDust(Projectile.oldPos[i], Projectile.width, Projectile.height, DustID.GemSapphire, 0f, 0f, 100, default(Color), 4f);
					Main.dust[num71].noGravity = true;
					num71 = Dust.NewDust(Projectile.oldPos[i], Projectile.width, Projectile.height, DustID.GemTopaz, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num71].noGravity = true;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile P = Projectile;
			SpriteBatch sb = Main.spriteBatch;
			for (int i = segmentPos.Length - 1; i >= 0; i--)
			{
				Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
				if (i == segmentPos.Length - 1)
				{
					tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/MissileAddons/BeamCombos/StardustComboDiffusionShot3").Value;
				}
				else if (i > 0)
				{
					if (i % 2 == 0)
					{
						tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/MissileAddons/BeamCombos/StardustComboDiffusionShot2").Value;
					}
					else
					{
						tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/MissileAddons/BeamCombos/StardustComboDiffusionShot1").Value;
					}
				}
				Color color = P.GetAlpha(Color.White);
				color.A /= 2;

				sb.Draw(tex,
				segmentPos[i] - Main.screenPosition,
				new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)),
				color,
				segmentRot[i],
				new Vector2((float)tex.Width / 2f, (float)tex.Height / 2),
				1f,
				SpriteEffects.None,
				0f);
			}
			return false;
		}
	}
	public class StardustFrozenTerrain : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Terrain");
		}

		int size = 42;
		public override void SetDefaults()
		{
			Projectile.width = size;
			Projectile.height = size;
			Projectile.scale = 0.75f;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1200;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Projectile.extraUpdates = 0;
		}

		const int MaxRange = 424;//528;
		int range = 0;
		float[,] rotation = new float[MaxRange * 2 / 16, MaxRange * 2 / 16];
		float[,] alpha = new float[MaxRange * 2 / 16, MaxRange * 2 / 16];
		Vector2[,] addedPos = new Vector2[MaxRange * 2 / 16, MaxRange * 2 / 16];

		int[] freezeDelay = new int[Main.maxNPCs];

		bool guardSpawned = false;
		int damage = 0;

		bool init = false;
		public override void AI()
		{
			Projectile P = Projectile;
			if (!init)
			{
				for (int x = 0; x < rotation.GetLength(0); x++)
				{
					for (int y = 0; y < rotation.GetLength(1); y++)
					{
						rotation[x, y] = (float)Main.rand.Next(360) * ((float)Math.PI / 180);
					}
				}
				for (int x = 0; x < addedPos.GetLength(0); x++)
				{
					for (int y = 0; y < addedPos.GetLength(1); y++)
					{
						addedPos[x, y].X = (float)Main.rand.Next(-40, 41) * 0.1f;
						addedPos[x, y].Y = (float)Main.rand.Next(-40, 41) * 0.1f;
					}
				}
				P.spriteDirection = 1;
				if (Main.rand.NextBool(2))
				{
					P.spriteDirection = -1;
				}
				damage = P.damage;
				P.damage = 0;
				init = true;
			}

			range = Math.Min(range + 3, MaxRange);

			if (range > MaxRange / 2 && !guardSpawned)
			{
				int g = Projectile.NewProjectile(Projectile.GetSource_FromAI(), P.Center.X, P.Center.Y - 40f, 0f, 0f, ModContent.ProjectileType<StardustComboGuardian>(), damage, P.knockBack, P.owner);
				Main.projectile[g].ai[0] = P.whoAmI;
				guardSpawned = true;
			}

			int xmin = (int)(P.Center.X - MaxRange) / 16;
			int xmax = (int)(P.Center.X + MaxRange) / 16;
			int ymin = (int)(P.Center.Y - MaxRange) / 16;
			int ymax = (int)(P.Center.Y + MaxRange) / 16;
			for (int x = xmin; x < xmax; x++)
			{
				for (int y = ymin; y < ymax; y++)
				{
					Vector2 pos = new Vector2((float)x * 16f + 8f, (float)y * 16f + 8f);
					if (Main.tile[x, y] != null && Main.tile[x, y].HasTile)
					{
						if (Vector2.Distance(pos, P.Center) <= range)
						{
							int fSize = (int)((float)size * P.scale * MathHelper.Clamp(alpha[x - xmin, y - ymin], 0f, 1f));
							if (fSize > 0)
							{
								Rectangle projRect = new Rectangle((int)pos.X - fSize / 2, (int)pos.Y - fSize / 2, fSize, fSize);
								foreach (NPC who in Main.ActiveNPCs) //this is laggy and inneficient, probably
								{
									NPC npc = Main.npc[who.whoAmI];
									if (!npc.friendly && !npc.dontTakeDamage)
									{
										Rectangle npcRect = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);

										if (projRect.Intersects(npcRect))
										{
											if (freezeDelay[who.whoAmI] <= 0)
											{
												npc.AddBuff(ModContent.BuffType<Buffs.IceFreeze>(), 600, true);
												freezeDelay[who.whoAmI] = 20;
											}
											else
											{
												freezeDelay[who.whoAmI]--;
											}
										}
									}
								}
							}
						}
					}
					if (Vector2.Distance(pos, P.Center) <= range || range >= MaxRange)
					{
						float rate = 0.1f;
						if (P.timeLeft > 30)
						{
							alpha[x - xmin, y - ymin] = Math.Min(alpha[x - xmin, y - ymin] + rate, 1f + 2f * (Vector2.Distance(pos, P.Center) / MaxRange));
						}
						else
						{
							alpha[x - xmin, y - ymin] = Math.Max(alpha[x - xmin, y - ymin] - rate, 0f);
						}
					}
				}
			}

			int max = 3;
			if (P.timeLeft > 30)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					if (checkOtherProj(P, Main.projectile[i]) && Main.projectile[i] != P)
					{
						if (P.ai[0] > max && Main.projectile[i].ai[0] == 1)
						{
							Main.projectile[i].timeLeft = 30;
						}
					}
				}

				bool flag = false;
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					if (checkOtherProj(P, Main.projectile[i]))
					{
						if (Main.projectile[i].ai[0] == 1)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					for (int i = 0; i < Main.maxProjectiles; i++)
					{
						if (checkOtherProj(P, Main.projectile[i]))
						{
							Main.projectile[i].ai[0]--;
						}
					}
				}
			}
		}

		bool checkOtherProj(Projectile P, Projectile otherProj)
		{
			return (otherProj.active && otherProj.timeLeft > 30 && otherProj.type == P.type && otherProj.owner == P.owner);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 50);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = Projectile;

			SpriteEffects effects = SpriteEffects.None;
			if (P.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;

			int xmin = (int)(P.Center.X - MaxRange) / 16;
			int xmax = (int)(P.Center.X + MaxRange) / 16;
			int ymin = (int)(P.Center.Y - MaxRange) / 16;
			int ymax = (int)(P.Center.Y + MaxRange) / 16;
			for (int x = xmin; x < xmax; x++)
			{
				for (int y = ymin; y < ymax; y++)
				{
					if (Main.tile[x, y] != null && Main.tile[x, y].HasTile)
					{
						Color tileColor = Lighting.GetColor(x, y);
						tileColor.B = (byte)Math.Max((int)tileColor.B, 25);
						Color color = P.GetAlpha(tileColor);
						float alphaScale = MathHelper.Clamp(alpha[x - xmin, y - ymin], 0f, 1f);

						Vector2 pos = new Vector2((float)x * 16f + 8f, (float)y * 16f + 8f);

						int num = 50;
						Rectangle screenRect = new Rectangle((int)(Main.screenPosition.X - (float)num), (int)(Main.screenPosition.Y - (float)num), Main.screenWidth + num * 2, Main.screenHeight + num * 2);
						Rectangle rect = new Rectangle((int)pos.X - 23, (int)pos.Y - 23, 56, 56);
						if (screenRect.Intersects(rect))
						{
							if (Vector2.Distance(pos, P.Center) <= range)
							{
								Vector2 pos2 = pos + addedPos[x - xmin, y - ymin];

								sb.Draw(tex, new Vector2((float)((int)(pos2.X - Main.screenPosition.X)), (float)((int)(pos2.Y - Main.screenPosition.Y))),
								new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)),
								color * alphaScale, rotation[x - xmin, y - ymin],
								new Vector2((float)tex.Width / 2f, (float)tex.Height / 2f),
								P.scale * alphaScale, effects, 0f);
							}
							else if (Vector2.Distance(pos, P.Center) <= range + 16)
							{
								float trot = (float)Math.Atan2((pos.Y - P.Center.Y), (pos.X - P.Center.X));
								Vector2 pos2 = P.Center + addedPos[x - xmin, y - ymin] + trot.ToRotationVector2() * range;
								Color color2 = color * alphaScale;
								Color color3 = color2 * 0.5f;
								color3.A = color2.A;

								sb.Draw(tex, new Vector2((float)((int)(pos2.X - Main.screenPosition.X)), (float)((int)(pos2.Y - Main.screenPosition.Y))),
								new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)),
								color3, rotation[x - xmin, y - ymin],
								new Vector2((float)tex.Width / 2f, (float)tex.Height / 2f),
								P.scale * alphaScale, effects, 0f);
							}
						}
					}
				}
			}

			return false;
		}
	}
	public class StardustComboGuardian : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Guardian");
			Main.projFrames[Projectile.type] = 12;
		}
		public override void SetDefaults()
		{
			//Projectile.netImportant = true;
			Projectile.width = 50;
			Projectile.height = 80;
			Projectile.aiStyle = -1;//120;
			Projectile.penetrate = -1;
			//Projectile.timeLeft *= 5;
			Projectile.timeLeft = 1200;
			//Projectile.minion = true;
			Projectile.friendly = true;
			//Projectile.minionSlots = 0f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			//Projectile.netImportant = true;
			Projectile.alpha = 255;
		}

		int damage = 0;
		public override void AI()
		{
			Projectile P = Projectile;

			if (!Main.projectile[(int)P.ai[0]].active)
			{
				P.Kill();
				return;
			}

			if (damage == 0)
			{
				damage = Projectile.damage;
				Projectile.damage = 0;
			}

			float distance = 425f;

			Lighting.AddLight(P.Center, 0.9f, 0.9f, 0.7f);
			if (P.alpha == 255)
			{
				P.alpha = 0;
				for (int i = 0; i < 30; i++)
				{
					int dust = Dust.NewDust(P.position, P.width, P.height, 135, 0f, 0f, 200, default(Color), 1.7f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 3f;
					dust = Dust.NewDust(P.position, P.width, P.height, 135, 0f, 0f, 100, default(Color), 1f);
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].fadeIn = 2.5f;
				}
			}
			if (P.localAI[0] > 0f)
			{
				P.localAI[0] -= 1f;
			}

			if (P.ai[1] == 0f)
			{
				int target = -1;
				foreach (NPC who in Main.ActiveNPCs)
				{
					NPC npc = Main.npc[who.whoAmI];
					if (npc.CanBeChasedBy(P, false))
					{
						//if (P.Distance(npc.Center) < distance)
						if (Main.projectile[(int)P.ai[0]].Distance(npc.Center) < distance)
						{
							target = who.whoAmI;
						}
					}
				}
				if (target != -1)
				{
					NPC npc = Main.npc[target];
					P.direction = (P.spriteDirection = (npc.Center.X > P.Center.X).ToDirectionInt());
					float xDiff = Math.Abs(npc.Center.X - P.Center.X);
					float yDiff = Math.Abs(npc.Center.Y - P.Bottom.Y);
					float yDir = (float)(npc.Center.Y > P.Bottom.Y).ToDirectionInt();
					if (xDiff > 20f)
					{
						P.velocity.X = P.velocity.X + 0.1f * (float)P.direction;
					}
					else
					{
						P.velocity.X = P.velocity.X * 0.7f;
					}
					if (yDiff > 10f)
					{
						P.velocity.Y = P.velocity.Y + 0.1f * yDir;
					}
					else
					{
						P.velocity.Y = P.velocity.Y * 0.7f;
					}
					if (P.localAI[0] == 0f && P.owner == Main.myPlayer && xDiff < 200f)
					{
						P.localAI[1] = 0f;
						P.ai[1] = 1f;
						P.netUpdate = true;
						P.localAI[0] = 90f;
					}
				}
				else
				{
					P.velocity *= 0.8f;
				}

				P.frameCounter++;
				if (P.frameCounter >= 9)
				{
					P.frameCounter = 0;
					P.frame++;
					if (P.frame >= Main.projFrames[P.type] - 4)
					{
						P.frame = 0;
					}
				}
			}
			else if (P.ai[1] == 1f)
			{
				P.velocity.X = P.velocity.X * 0.9f;
				P.localAI[1] += 1f;
				if (P.localAI[1] == 3f && P.owner == Main.myPlayer)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), P.Center.X, P.Center.Y, 0f, 0f, ProjectileID.StardustGuardianExplosion, damage, 6f, P.owner, 0f, 5f);
				}
				if (P.localAI[1] >= 6f)
				{
					P.localAI[1] = 0f;
					P.ai[1] = 0f;
					P.netUpdate = true;
				}
				if (P.frame < Main.projFrames[P.type] - 4)
				{
					P.frame = Main.projFrames[P.type] - 1;
					P.frameCounter = 0;
				}

				P.frameCounter++;
				if (P.frameCounter >= 5)
				{
					P.frameCounter = 0;
					P.frame--;
					if (P.frame < Main.projFrames[P.type] - 5)
					{
						P.frame = Main.projFrames[P.type] - 1;
					}
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			Projectile P = Projectile;
			for (int i = 0; i < 30; i++)
			{
				int dust = Dust.NewDust(P.position, P.width, P.height, 135, 0f, 0f, 200, default(Color), 1.7f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 3f;
				dust = Dust.NewDust(P.position, P.width, P.height, 135, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 2.5f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = Projectile;

			SpriteEffects effects = SpriteEffects.None;
			if (P.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			Color color = Lighting.GetColor((int)P.Center.X / 16, (int)P.Center.Y / 16);

			Vector2 pos = P.Center + Vector2.UnitY * P.gfxOffY - Main.screenPosition;
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
			Rectangle rectangle = tex.Frame(1, Main.projFrames[P.type], 0, P.frame);
			Color alpha = P.GetAlpha(color);
			Vector2 origin = rectangle.Size() / 2f;

			alpha.A /= 2;

			Main.spriteBatch.Draw(tex, pos, new Rectangle?(rectangle), alpha, P.rotation, origin, P.scale, effects, 0f);

			return false;
		}
	}
}
