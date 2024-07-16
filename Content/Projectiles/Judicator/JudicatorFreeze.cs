using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Judicator
{
	public class JudicatorFreeze : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Judicator Freeze");
		}

		int size = 42;
		public override void SetDefaults()
		{
			Projectile.width = size;
			Projectile.height = size;
			Projectile.scale = 0.75f;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 60;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 0;
		}

		static int range = 320;
		float[,] rotation = new float[range * 2 / 16, range * 2 / 16];
		float[,] alpha = new float[range * 2 / 16, range * 2 / 16];
		Vector2[,] addedPos = new Vector2[range * 2 / 16, range * 2 / 16];

		int[] freezeDelay = new int[Main.maxNPCs];

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
					Vector2 pos = new Vector2((float)x * 16f + 8f, (float)y * 16f + 8f);
					if (Main.tile[x, y] != null && Main.tile[x, y].HasTile)
					{
						if (Vector2.Distance(pos, P.Center) <= range)
						{
							int fSize = (int)((float)size * P.scale * MathHelper.Clamp(alpha[x - xmin, y - ymin], 0f, 1f));
							if (fSize > 0)
							{
								Rectangle projRect = new Rectangle((int)pos.X - fSize / 2, (int)pos.Y - fSize / 2, fSize, fSize);
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
												npc.AddBuff(44, 300);
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

			int xmin = (int)(P.Center.X - range) / 32;
			int xmax = (int)(P.Center.X + range) / 32;
			int ymin = (int)(P.Center.Y - range) / 32;
			int ymax = (int)(P.Center.Y + range) / 32;
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
			return false;
		}
	}
}
