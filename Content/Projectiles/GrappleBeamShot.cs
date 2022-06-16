using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using MetroidMod.Common.Players;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles
{
	public class GrappleBeamShot : ModProjectile
	{
		internal int[] GrappableNPCs = new int[]
		{
			ModContent.NPCType<NPCs.Mobs.Utility.Ripper>(),
			ModContent.NPCType<NPCs.Mobs.Utility.Powamp>()
		};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grapple Beam");
			Main.projFrames[Type] = 2;
		}
		int chainFrame = 0;
		int chainFrame2 = 0;
		float time = 0f;
		float increment = 0f;
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.GemHookAmethyst);

			Projectile.width = 18;
			Projectile.height = 18;
			//Projectile.aiStyle = 0;
			Projectile.timeLeft = 3600;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			Projectile.DamageType = ModContent.GetInstance<DamageClasses.HunterDamageClass>();//Projectile.ranged = true;
			Projectile.extraUpdates = 3;
		}

		public override bool? SingleGrappleHook(Player player)
		{
			return true;
		}

		// Use player to kill oldest hook. For hooks that kill the oldest when shot, not when the newest latches on: Like SkeletronHand
		// You can also change the projectile likr: Dual Hook, Lunar Hook
		public override void UseGrapple(Player player, ref int type)
		{
			//Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/GrappleBeamSound"));
			int hooksOut = 0;
			int oldestHookIndex = -1;
			int oldestHookTimeLeft = 100000;
			for (int i = 0; i < 1000; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == Projectile.whoAmI && Main.projectile[i].type == Type)
				{
					hooksOut++;
					if (Main.projectile[i].timeLeft < oldestHookTimeLeft)
					{
						oldestHookIndex = i;
						oldestHookTimeLeft = Main.projectile[i].timeLeft;
					}
				}
			}
			if (hooksOut > 1)
			{
				Main.projectile[oldestHookIndex].Kill();
			}
		}

		// Amethyst Hook is 300, Static Hook is 600
		public override float GrappleRange()
		{
			return 400f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			numHooks = 1;
		}

		// default is 11, Lunar is 24
		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 40f;
		}

		private Player owner;
		private bool isHooked;
		public override bool PreAI()
		{
			owner = Main.player[Projectile.owner];
			increment = ((float)Math.PI*2)/60f;
			return false;
		}
		public override void PostAI()
		{
			if (owner.dead || (Vector2.Distance(owner.Center, Projectile.Center) > 400 && !isHooked) || (Vector2.Distance(owner.Center, Projectile.Center) > 450 && isHooked) || (owner.controlJump && owner.releaseJump))
			{
				Projectile.Kill();
				isHooked = false;
				return;
			}
			Projectile P = Projectile;
			MPlayer mp = owner.GetModPlayer<MPlayer>();
			if (isHooked)
			{
				P.ai[0] = 2f;
				P.velocity = default(Vector2);
				P.timeLeft = 2;

				if (P.ai[1] <= 0)
				{
					int num124 = (int)(P.position.X / 16f) - 1;
					int num125 = (int)((P.position.X + (float)P.width) / 16f) + 2;
					int num126 = (int)(P.position.Y / 16f) - 1;
					int num127 = (int)((P.position.Y + (float)P.height) / 16f) + 2;
					if (num124 < 0)
					{
						num124 = 0;
					}
					if (num125 > Main.maxTilesX)
					{
						num125 = Main.maxTilesX;
					}
					if (num126 < 0)
					{
						num126 = 0;
					}
					if (num127 > Main.maxTilesY)
					{
						num127 = Main.maxTilesY;
					}

					bool flag3 = true;
					for (int x = num124; x < num125; x++)
					{
						for (int y = num126; y < num127; y++)
						{
							/*if (Main.tile[x, y] == null)
							{
								Main.tile[x, y] = new Tile();
							}*/

							Tile tile = Main.tile[x, y];

							Vector2 vector9;
							vector9.X = x * 16;
							vector9.Y = y * 16;
							if (P.position.X + (float)(P.width / 2) > vector9.X && P.position.X + (float)(P.width / 2) < vector9.X + 16f && P.position.Y + (float)(P.height / 2) > vector9.Y && P.position.Y + (float)(P.height / 2) < vector9.Y + 16f && tile.HasUnactuatedTile && (Main.tileSolid[tile.TileType] || tile.TileType == 314))
							{
								flag3 = false;
							}
						}
					}
					if (flag3)
					{
						isHooked = false;
					}
					else //if (owner.grapCount < 10)
					{
						mp.grapplingBeam = P.whoAmI;
						//owner.grapCount++;
					}
				}
				else // Hooked onto NPC
				{
					if (Main.npc[(int)Projectile.ai[1]].active)
					{
						NPC target = Main.npc[(int)Projectile.ai[1]];
						mp.grapplingBeam = P.whoAmI;
						Projectile.position = target.position + new Vector2(0, target.height / 2);
						Projectile.velocity = Vector2.Zero;
					}
					else
					{
						isHooked = false;
					}
				}
			}
			else
			{
				P.ai[0] = 0f;

				// Tile check.
				int num111 = (int)(P.position.X / 16f) - 1;
				int num112 = (int)((P.position.X + (float)P.width) / 16f) + 2;
				int num113 = (int)(P.position.Y / 16f) - 1;
				int num114 = (int)((P.position.Y + (float)P.height) / 16f) + 2;
				if (num111 < 0)
				{
					num111 = 0;
				}
				if (num112 > Main.maxTilesX)
				{
					num112 = Main.maxTilesX;
				}
				if (num113 < 0)
				{
					num113 = 0;
				}
				if (num114 > Main.maxTilesY)
				{
					num114 = Main.maxTilesY;
				}

				for (int x = num111; x < num112; x++)
				{
					int y = num113;
					while (y < num114)
					{
						/*if (Main.tile[x, y] == null)
						{
							Main.tile[x, y] = new Tile();
						}*/

						Tile tile = Main.tile[x, y];

						Vector2 vector8;
						vector8.X = x * 16;
						vector8.Y = y * 16;
						if (P.position.X + P.width > vector8.X && P.position.X < vector8.X + 16f && P.position.Y + P.height > vector8.Y && P.position.Y < vector8.Y + 16f && tile.HasUnactuatedTile && (Main.tileSolid[tile.TileType] || tile.TileType == 314))
						{
							SoundEngine.PlaySound(Sounds.Items.Tools.GrappleLatch, owner.Center);
							mp.maxDist = Vector2.Distance(owner.Center, P.Center);
							//if (owner.grapCount < 10)
							//{
								mp.grapplingBeam = P.whoAmI;
								//owner.grapCount++;
							//}

							P.velocity *= 0;
							isHooked = true;
							P.position.X = (float)(x * 16 + 8 - P.width / 2);
							P.position.Y = (float)(y * 16 + 8 - P.height / 2);
							P.damage = 0;
							P.netUpdate = true;

							if (Main.myPlayer == P.owner)
							{
								//NetMessage.SendData(13, -1, -1, "", P.owner, 0f, 0f, 0f, 0f);
								break;
							}
							//Vector2 dif = P.position - owner.position;
							//float dist = (float)Math.Sqrt (dif.X * dif.X + dif.Y *dif.Y);
							//mp.maxDist = dist;
							break;
						}
						else
						{
							y++;
						}
					}
					if (isHooked)
					{
						break;
					}
				}

				// NPC check.
				for(int i = 0; i < 200; ++i)
				{
					NPC target = Main.npc[i];
					if(Projectile.getRect().Intersects(target.getRect()) && target.active)
					{
						if(GrappableNPCs.Contains(target.type))
						{
							Projectile.ai[1] = i;

							Projectile.velocity *= 0;
							isHooked = true;
							Projectile.netUpdate = true;
							SoundEngine.PlaySound(Sounds.Items.Tools.GrappleLatch, owner.Center);
							mp.maxDist = Vector2.Distance(owner.Center, P.Center);
							mp.grapplingBeam = P.whoAmI;
							break;
						}
					}
				}
			}

			if (Main.myPlayer == P.owner)
			{
				int amountOfGrapples = 0;
				int oldestProjectile = -1;
				int oldestProjectileTimeLeft = 100000;
				int maxAllowedGrapples = 1;
				for (int i = 0; i < 1000; i++)
				{
					Projectile targetProj = Main.projectile[i];
					if (targetProj.active && targetProj.owner == P.owner && (targetProj.type == P.type || targetProj.aiStyle == 7))
					{
						if (targetProj.timeLeft < oldestProjectileTimeLeft)
						{
							oldestProjectile = i;
							oldestProjectileTimeLeft = targetProj.timeLeft;
						}
						amountOfGrapples++;
					}
				}
				if (amountOfGrapples > maxAllowedGrapples)
				{
					Main.projectile[oldestProjectile].Kill();
				}
			}
			
			P.frameCounter++;
			if(P.frameCounter > 4)
			{
				P.frame++;
				P.frameCounter = 0;
			}
			if(P.frame >= Main.projFrames[P.type])
			{
				P.frame = 0;
			}
			chainFrame = Main.rand.Next(4);
			chainFrame2 = Main.rand.Next(4);
			time += increment * 2f;
			if(time >= (float)Math.PI*2)
			{
				time -= (float)Math.PI*2;
			}
		}
		public override bool PreKill(int timeLeft)
		{
			isHooked = false;
			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile P = Projectile;
			MPlayer mp = owner.GetModPlayer<MPlayer>();
			
			Asset<Texture2D> tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/GrappleBeamChain2");
			float dist = Math.Max(Vector2.Distance(owner.Center, Projectile.Center),1);
			float maxDist = 400;
			if(isHooked)
			{
				maxDist = Math.Max(mp.maxDist,1);
			}
			
			int numH = tex.Value.Height / 4;
			
			Vector2 chain = Projectile.Center - owner.Center;
			int linklength = Math.Max(numH-1,1);
			int numlinks = (int)Math.Ceiling(dist/linklength);
			Vector2[] pos = new Vector2[numlinks];
			
			float amplitude = 3 + 6f * (1f - dist/maxDist);
			
			for(int j = 0; j < 3; j++)
			{
				if(j == 0 || j == 2)
				{
					for(int i = 0; i < numlinks; i++)
					{
						pos[i] = owner.Center + chain/numlinks * i;
						
						int k = 1;
						if(j > 0)
						{
							k = -1;
						}
						
						float t = 0f;
						t += increment * (i * maxDist/dist) * 2f;
						if(t >= (float)Math.PI*2)
						{
							t -= (float)Math.PI*2;
						}
						
						float shift = amplitude * (float)Math.Sin(t + time) * k;
						
						float rot = (float)Math.Atan2(chain.Y, chain.X) + (float)Math.PI/2;
						pos[i].X += (float)Math.Cos(rot)*shift;
						pos[i].Y += (float)Math.Sin(rot)*shift;
						
						Color color = Color.White;
						Main.EntitySpriteDraw(tex.Value, pos[i] - Main.screenPosition, new Rectangle?(new Rectangle(0, numH * chainFrame2, tex.Value.Width, numH)), color, rot, new Vector2((float)tex.Value.Width / 2, (float)numH / 2), Projectile.scale, SpriteEffects.None, 0);
						//s.Draw(tex,pos[i] - Main.screenPosition,new Rectangle?(new Rectangle(0,numH*chainFrame2,tex.Width,numH)),color,rot,new Vector2((float)tex.Width/2,(float)numH/2),projectile.scale,SpriteEffects.None,0f);
					}
				}
				else
				{
					DrawChain(owner.Center, Projectile.Center, ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/GrappleBeamChain", AssetRequestMode.ImmediateLoad), chainFrame, 4);
				}
			}
			tex = Terraria.GameContent.TextureAssets.Projectile[P.type];//Main.projectileTexture[P.type];
			int num = tex.Value.Height / Main.projFrames[Type];
			Main.spriteBatch.Draw(tex.Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, num*P.frame, tex.Value.Width, num)), Projectile.GetAlpha(Color.White), 0f, new Vector2((float)tex.Value.Width/2, (float)num/2), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawExtras()
		{
			return false;
		}
		public void DrawChain(Vector2 start, Vector2 end, Asset<Texture2D> name, int frame = 0, int frameCount = 0)
		{
			int numH = name.Value.Height;
			if(frameCount > 0)
			{
				numH = name.Value.Height/frameCount;
			}
			
			start -= Main.screenPosition;
			end -= Main.screenPosition;

			int linklength = numH-1;
			Vector2 chain = end - start;

			float length = (float)chain.Length();
			int numlinks = (int)Math.Ceiling(length/linklength);
			Vector2[] links = new Vector2[numlinks];
			float rotation = (float)Math.Atan2(chain.Y, chain.X);

			for (int i = 0; i < numlinks; i++)
			{
				links[i] =start + chain/numlinks * i;
				Vector2 LR = links[i]+Main.screenPosition;

				Color color = Lighting.GetColor((int)((links[i].X+Main.screenPosition.X)/16), (int)((links[i].Y+Main.screenPosition.Y)/16));
				//spriteBatch.Draw(name, new Rectangle((int)links[i].X, (int)links[i].Y, name.Width, linklength), null, color, rotation+1.57f, new Vector2(name.Width/2f, linklength), SpriteEffects.None, 1f);
				//spriteBatch.Draw(name,links[i],new Rectangle?(new Rectangle(0,numH*frame,name.Width,numH)),color,rotation+1.57f,new Vector2(name.Width/2f,numH/2f),Projectile.scale,SpriteEffects.None,0f);
				Main.EntitySpriteDraw(name.Value, links[i], new Rectangle?(new Rectangle(0, numH*frame, name.Value.Width, numH)), color, rotation + 1.57f, new Vector2(name.Value.Width / 2f, numH / 2f), Projectile.scale, SpriteEffects.None, 0);

				Lighting.AddLight(LR, 229f/255f, 249f/255f, 255f/255f);
			}
		}
	}
}
