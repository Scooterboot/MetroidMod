using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles
{
	public class GrappleBeamShot : ModProjectile
	{
			//private float grappleRotation = 0f;
			private bool isHooked;
			private int grappleSwing = -1;
			private float maxDist;
			private int jump = 0;
			private int soundDelay = 41;
			
		public override void SetDefaults()
		{
				projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
				projectile.name = "Grapple Beam";
				projectile.width = 18;
				projectile.height = 18;


		}


		public override bool? SingleGrappleHook(Player player)
		{
			return true;
		}

		// Use player to kill oldest hook. For hooks that kill the oldest when shot, not when the newest latches on: Like SkeletronHand
		// You can also change the projectile likr: Dual Hook, Lunar Hook
		public override void UseGrapple(Player player, ref int type)
		{
			Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/GrappleBeamSound"));
			int hooksOut = 0;
			int oldestHookIndex = -1;
			int oldestHookTimeLeft = 100000;
			for (int i = 0; i < 1000; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == projectile.whoAmI && Main.projectile[i].type == projectile.type)
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

public override bool PreAI()
{
	return false;
}
		public override void PostAI()
		{
			Player owner = Main.player[projectile.owner];
			MPlayer mp = owner.GetModPlayer<MPlayer>(mod);
				mp.grappleBeamIsHooked = isHooked;
			if (owner.dead || (Vector2.Distance(owner.Center, projectile.Center) > 465 && !isHooked) || (Vector2.Distance(owner.Center, projectile.Center) > 525 && isHooked))
			{
				projectile.Kill();
				isHooked = false;
				return;
			}
			
			
			
			Terraria.Projectile P = projectile;
	/*		Vector2 mountedCenter = Main.player[P.owner].MountedCenter;
			Vector2 vector6 = new Vector2(P.position.X + (float)P.width * 0.5f, P.position.Y + (float)P.height * 0.5f);
			float num69 = mountedCenter.X - vector6.X;
			float num70 = mountedCenter.Y - vector6.Y;
			float num71 = (float)Math.Sqrt((double)(num69 * num69 + num70 * num70));
			P.rotation = (float)Math.Atan2((double)num70, (double)num69) - 1.57f;
	*/		if (isHooked)
			{
				P.ai[0] = 2f;
				projectile.velocity = default(Vector2);
				projectile.timeLeft = 2;
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
				for (int num128 = num124; num128 < num125; num128++)
				{
					for (int num129 = num126; num129 < num127; num129++)
					{
						if (Main.tile[num128, num129] == null)
						{
							Main.tile[num128, num129] = new Tile();
						}
						Vector2 vector9;
						vector9.X = (float)(num128 * 16);
						vector9.Y = (float)(num129 * 16);
						if (P.position.X + (float)(P.width / 2) > vector9.X && P.position.X + (float)(P.width / 2) < vector9.X + 16f && P.position.Y + (float)(P.height / 2) > vector9.Y && P.position.Y + (float)(P.height / 2) < vector9.Y + 16f && Main.tile[num128, num129].nactive() && (Main.tileSolid[(int)Main.tile[num128, num129].type] || Main.tile[num128, num129].type == 314))
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
					grappleSwing = P.whoAmI;
					//owner.grapCount++;
				}
			}
			else
			{
				P.ai[0] = 0f;
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
				for (int num115 = num111; num115 < num112; num115++)
				{
					int num116 = num113;
					while (num116 < num114)
					{
						if (Main.tile[num115, num116] == null)
						{
							Main.tile[num115, num116] = new Tile();
						}
						Vector2 vector8;
						vector8.X = (float)(num115 * 16);
						vector8.Y = (float)(num116 * 16);
						if (P.position.X + (float)P.width > vector8.X && P.position.X < vector8.X + 16f && P.position.Y + (float)P.height > vector8.Y && P.position.Y < vector8.Y + 16f && Main.tile[num115, num116].nactive() && (Main.tileSolid[(int)Main.tile[num115, num116].type] || Main.tile[num115, num116].type == 314))
						{
							maxDist = Vector2.Distance(owner.Center, projectile.Center);
							//if (owner.grapCount < 10)
							//{
								grappleSwing = P.whoAmI;
								//owner.grapCount++;
							//}
							P.velocity.X = 0f;
							P.velocity.Y = 0f;
							Main.PlaySound(0, num115 * 16, num116 * 16, 1, 1f, 0f);
							isHooked = true;
							P.position.X = (float)(num115 * 16 + 8 - P.width / 2);
							P.position.Y = (float)(num116 * 16 + 8 - P.height / 2);
							P.damage = 0;
							P.netUpdate = true;
							//Vector2 dif = P.position - owner.position;
							//float dist = (float)Math.Sqrt (dif.X * dif.X + dif.Y *dif.Y);
							//mp.maxDist = dist;
							break;
						}
						else
						{
							num116++;
						}
					}
					if (isHooked)
					{
						break;
					}
				}
			}
			if (Main.myPlayer == P.owner)
			{
				int num117 = 0;
				int num118 = -1;
				int num119 = 100000;
				int num121 = 1;
				for (int num122 = 0; num122 < 1000; num122++)
				{
						if (Main.projectile[num122].timeLeft < num119)
						{
							num118 = num122;
							num119 = Main.projectile[num122].timeLeft;
						}
						num117++;
				}
				if (num117 > num121)
				{
					Main.projectile[num118].Kill();
				}
			}
			int tweak2 = 0;
			Player player = Main.player[projectile.owner];
			if(grappleSwing >= 0)
			{
				soundDelay++;
			if (soundDelay > 41)
			{
				 Main.PlaySound(SoundLoader.customSoundType, (int)owner.position.X, (int)owner.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/GrappleLoop"));
				soundDelay = 0;
			}
					if (player.mount.Active)
					{
						player.mount.Dismount(player);
					}
					float targetrotation = (float)Math.Atan2(((projectile.Center.Y-player.Center.Y)*player.direction),((projectile.Center.X-player.Center.X)*player.direction));
					mp.grappleRotation = targetrotation;
					player.wingTime = 0f;
					player.rocketTime = player.rocketTimeMax;
					player.rocketDelay = 0;
					player.rocketFrame = false;
					player.canRocket = false;
					player.rocketRelease = false;
					player.fallStart = (int)(player.Center.Y / 16f);
				
					player.sandStorm = false;
					MPlayer mPlayer = player.GetModPlayer<MPlayer>(mod);
					//mPlayer.statSpaceJumps = mPlayer.maxSpaceJumps;
					Vector2 v = player.Center - projectile.Center;
					float dist = Vector2.Distance(player.Center, projectile.Center);
					//owner.velocity = tileCollide;
					bool up = (player.controlUp);
					bool down = (player.controlDown && maxDist < 465);
					//float dif = (dist - maxDist)/maxDist;
					float ndist = Vector2.Distance(player.Center + player.velocity, projectile.Center);
					float ddist = ndist - dist;
					//v /= dist;
					//owner.velocity -= v * ddist;
					//v *= maxDist;
					//owner.position = projectile.position + v;
					float num4 = projectile.Center.X - player.Center.X;
					float num5 = projectile.Center.Y - player.Center.Y;
					float num6 = (float)System.Math.Sqrt((double)(num4 * num4 + num5 * num5));
					float num7 = ddist+player.gravity;
					if ((player.controlLeft || player.controlRight) && player.velocity.X < 20f && player.velocity.X > -20f)
					{
						player.velocity.X *= 1.025f;
					}
					if(up)
					{
						num7 = 11;
						maxDist = dist;
					}
					if(down)
					{
						num7 = -11;
						maxDist = dist;
					}
					float num8;
					if (num6 > num7)
					{
						num8 = num7 / num6;
					}
					else
					{
						num8 = 1f;
					}
					num4 *= num8;
					num5 *= num8;
					Vector2 vect = new Vector2(num4, num5);
					if(up || down)
					{
						player.velocity = vect;
						tweak2 = 2;
					}
					else if (dist >= maxDist)
					{
						player.velocity += vect;
						player.maxRunSpeed = 15f;
						player.runAcceleration *= 3f;

					}
					if(tweak2 > 0)
					{
						if(!up && !down)
						{
							player.velocity *= 0;
						}
						tweak2--;
					}
					if (player.releaseJump)
					{
						jump = 1;
					}
						
						if (player.controlJump && jump >= 1)
						{
							projectile.Kill();
							player.wingTime = (float)player.wingTimeMax;
							if (!player.controlDown && player.velocity.Y > -Player.jumpSpeed)
							{
								player.velocity.Y -= Player.jumpSpeed;
								player.jump = Player.jumpHeight / 2;
							}
							grappleSwing = 0;
							player.grapCount = 0;
							return;
						}
			}
		}
		public override bool PreDrawExtras(SpriteBatch spriteBatch)
		{
			return false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
			Vector2 vector14 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
			float num84 = mountedCenter.X - vector14.X;
			float num85 = mountedCenter.Y - vector14.Y;
			float rotation13 = (float)Math.Atan2((double)num85, (double)num84) - 1.57f;
			bool flag11 = true;
			while (flag11)
			{
				float num86 = (float)Math.Sqrt((double)(num84 * num84 + num85 * num85));
				if (num86 < 30f)
				{
					flag11 = false;
				}
				else if (float.IsNaN(num86))
				{
					flag11 = false;
				}
				else
				{
					num86 = 24f / num86;
					num84 *= num86;
					num85 *= num86;
					vector14.X += num84;
					vector14.Y += num85;
					num84 = mountedCenter.X - vector14.X;
					num85 = mountedCenter.Y - vector14.Y;
					Color color15 = Lighting.GetColor((int)vector14.X / 16, (int)(vector14.Y / 16f));
					Main.spriteBatch.Draw(mod.GetTexture("Gore/GrappleBeamChain"), new Vector2(vector14.X - Main.screenPosition.X, vector14.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain30Texture.Width, Main.chain30Texture.Height)), color15, rotation13, new Vector2((float)Main.chain30Texture.Width * 0.5f, (float)Main.chain30Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
				}
			}
			return true;
		}
	}
}
