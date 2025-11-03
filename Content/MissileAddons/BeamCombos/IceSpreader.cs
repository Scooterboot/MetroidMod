using System;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MissileAddons.BeamCombos
{
	public class IceSpreader : ModMissileAddon
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
				.AddIngredient(ItemID.IceRod, 1)
				.AddRecipeGroup(MetroidMod.T3HMBarRecipeGroupID, 10)
				.AddIngredient(ItemID.Sapphire, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
		public override void OnKill(MProjectile mProjectile, int timeLeft)
		{
			Projectile P = mProjectile.Projectile;

			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(P.position, P.width, P.height, 135, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
			}

			var entitySource = P.GetSource_Death();
			for (int i = 0; i < 360; i += 10)
			{
				float rot = (float)Angle.ConvertToRadians(i);
				int num54 = Projectile.NewProjectile(entitySource, P.Center.X, P.Center.Y, 0f, 0f, ModContent.ProjectileType<IceSpreaderDiffusionShot>(), P.damage, P.knockBack, P.owner);
				IceSpreaderDiffusionShot difShot = (IceSpreaderDiffusionShot)Main.projectile[num54].ModProjectile;
				difShot.spin = rot;
			}

			int x = (int)MathHelper.Clamp(P.Center.X / 16, 0, Main.maxTilesX - 2);
			int y = (int)MathHelper.Clamp(P.Center.Y / 16, 0, Main.maxTilesY - 2);
			Vector2 pos = new Vector2((x * 16f) + 8f, (y * 16f) + 8f);
			int ft = Projectile.NewProjectile(entitySource, pos.X, pos.Y, 0f, 0f, ModContent.ProjectileType<IceSpreaderFrozenTerrain>(), 0, 0f, P.owner);

			//Terraria.Audio.SoundEngine.PlaySound(Sounds.Items.Weapons.IceSpreaderImpactSound, P.Center);
		}
	}
	public class IceSpreaderDiffusionShot : MProjectile
	{
		private bool initialised = false;
		private float radius = 0.0f;
		public float spin = 0.0f;
		private Vector2 basePosition = new Vector2(0f, 0f);
		private Vector2 prevPosition = new Vector2(0f, 0f);

		private float alpha = 1f;

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 80;//40;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Projectile.extraUpdates = 2;
		}

		public void initialise()
		{
			basePosition = Projectile.Center;
			initialised = true;
		}
		public override void AI()
		{
			if (!initialised)
			{
				initialise();
			}
			Projectile P = Projectile;
			radius = Math.Min(radius + 8f, 320f);
			spin += (float)(Math.PI / 32);
			P.rotation = 0f;
			P.position = basePosition - new Vector2(P.width / 2, P.height / 2) + (spin.ToRotationVector2() * radius);

			int dust = Dust.NewDust(P.position, P.width, P.height, 135, 0, 0, 100, default(Color), 3f + (3f * (P.timeLeft / 40f)));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = new Vector2((Main.rand.Next(50) - 25) * 0.1f, (Main.rand.Next(50) - 25) * 0.1f);

			if (P.timeLeft < 40)
			{
				alpha -= 1f / 40;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(), 600, true);
			Player O = Main.player[Projectile.owner];
			target.immune[O.whoAmI] = 10;
			Projectile.localNPCHitCooldown = 10;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return lightColor * alpha;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCenteredTrail(Projectile, Main.spriteBatch);
			return false;
		}
	}
	public class IceSpreaderFrozenTerrain : MProjectile
	{
		private readonly int size = 42;
		public override void SetDefaults()
		{
			Projectile.width = size;
			Projectile.height = size;
			Projectile.scale = 0.75f;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Projectile.extraUpdates = 0;
		}

		private static readonly int range = 320;
		private readonly float[,] rotation = new float[range * 2 / 16, range * 2 / 16];
		private readonly float[,] alpha = new float[range * 2 / 16, range * 2 / 16];
		private readonly Vector2[,] addedPos = new Vector2[range * 2 / 16, range * 2 / 16];

		private readonly int[] freezeDelay = new int[Main.maxNPCs];

		private bool init = false;
		public override void AI()
		{
			Projectile P = Projectile;
			if (!init)
			{
				for (int x = 0; x < rotation.GetLength(0); x++)
				{
					for (int y = 0; y < rotation.GetLength(1); y++)
					{
						rotation[x, y] = Main.rand.Next(360) * ((float)Math.PI / 180);
					}
				}
				for (int x = 0; x < addedPos.GetLength(0); x++)
				{
					for (int y = 0; y < addedPos.GetLength(1); y++)
					{
						addedPos[x, y].X = Main.rand.Next(-40, 41) * 0.1f;
						addedPos[x, y].Y = Main.rand.Next(-40, 41) * 0.1f;
					}
				}
				P.spriteDirection = 1;
				if (Main.rand.NextBool(2))
				{
					P.spriteDirection = -1;
				}
				//init = true;
			}

			int xmin = (int)(P.Center.X - range) / 16;
			int xmax = (int)(P.Center.X + range) / 16;
			int ymin = (int)(P.Center.Y - range) / 16;
			int ymax = (int)(P.Center.Y + range) / 16;
			for (int x = xmin; x < xmax; x++)
			{
				for (int y = ymin; y < ymax; y++)
				{
					Vector2 pos = new Vector2((x * 16f) + 8f, (y * 16f) + 8f);
					if (Main.tile[x, y] != null && Main.tile[x, y].HasTile)
					{
						if (Vector2.Distance(pos, P.Center) <= range)
						{
							int fSize = (int)(size * P.scale * MathHelper.Clamp(alpha[x - xmin, y - ymin], 0f, 1f));
							if (fSize > 0)
							{
								Rectangle projRect = new Rectangle((int)pos.X - (fSize / 2), (int)pos.Y - (fSize / 2), fSize, fSize);
								foreach (NPC who in Main.ActiveNPCs)
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
					if (!init)
					{
						alpha[x - xmin, y - ymin] = -(Vector2.Distance(pos, P.Center) / range);
					}
					else
					{
						float rate = 0.1f;
						if (P.timeLeft > 20)
						{
							alpha[x - xmin, y - ymin] = Math.Min(alpha[x - xmin, y - ymin] + rate, 1f + (Vector2.Distance(pos, P.Center) / range));
						}
						else
						{
							alpha[x - xmin, y - ymin] = Math.Max(alpha[x - xmin, y - ymin] - rate, 0f);
						}
					}
				}
			}

			init = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(lightColor.R, lightColor.G, lightColor.B, 50);
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

			int xmin = (int)(P.Center.X - range) / 16;
			int xmax = (int)(P.Center.X + range) / 16;
			int ymin = (int)(P.Center.Y - range) / 16;
			int ymax = (int)(P.Center.Y + range) / 16;
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

						Vector2 pos = new Vector2((x * 16f) + 8f, (y * 16f) + 8f);

						if (Vector2.Distance(pos, P.Center) <= range)
						{
							Vector2 pos2 = pos + addedPos[x - xmin, y - ymin];

							sb.Draw(tex, new Vector2((int)(pos2.X - Main.screenPosition.X), (int)(pos2.Y - Main.screenPosition.Y)),
							new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)),
							color * alphaScale, rotation[x - xmin, y - ymin],
							new Vector2(tex.Width / 2f, tex.Height / 2f),
							P.scale * alphaScale, effects, 0f);
						}
						else if (Vector2.Distance(pos, P.Center) <= range + 16)
						{
							float trot = (float)Math.Atan2(pos.Y - P.Center.Y, pos.X - P.Center.X);
							Vector2 pos2 = P.Center + addedPos[x - xmin, y - ymin] + (trot.ToRotationVector2() * range);
							Color color2 = color * alphaScale;
							Color color3 = color2 * 0.5f;
							color3.A = color2.A;

							sb.Draw(tex, new Vector2((int)(pos2.X - Main.screenPosition.X), (int)(pos2.Y - Main.screenPosition.Y)),
							new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)),
							color3, rotation[x - xmin, y - ymin],
							new Vector2(tex.Width / 2f, tex.Height / 2f),
							P.scale * alphaScale, effects, 0f);
						}
					}
				}
			}
			return false;
		}
	}
}
