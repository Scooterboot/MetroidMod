using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles
{
	public class GrappleBeamShot : ModProjectile
	{
        internal int[] GrappableNPCs = new int[]
            {
                ModLoader.GetMod("MetroidMod").NPCType("Ripper"),
                ModLoader.GetMod("MetroidMod").NPCType("Powamp")
            };
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grapple Beam");

		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);

			projectile.width = 18;
			projectile.height = 18;
			//projectile.aiStyle = 0;
			projectile.timeLeft = 3600;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.penetrate = 1;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.extraUpdates = 3;
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

		private Player owner;
		private bool isHooked;
		public override bool PreAI()
		{
			owner = Main.player[projectile.owner];
			return false;
		}
		public override void PostAI()
		{
			if (owner.dead || (Vector2.Distance(owner.Center, projectile.Center) > 400 && !isHooked) || (Vector2.Distance(owner.Center, projectile.Center) > 450 && isHooked) || (owner.controlJump && owner.releaseJump))
			{
				projectile.Kill();
				isHooked = false;
				return;
			}
			Projectile P = projectile;
			MPlayer mp = owner.GetModPlayer<MPlayer>(mod);
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
                        num124 = 0;
                    if (num125 > Main.maxTilesX)
                        num125 = Main.maxTilesX;
                    if (num126 < 0)
                        num126 = 0;
                    if (num127 > Main.maxTilesY)
                        num127 = Main.maxTilesY;

                    bool flag3 = true;
                    for (int x = num124; x < num125; x++)
                    {
                        for (int y = num126; y < num127; y++)
                        {
                            if (Main.tile[x, y] == null)
                                Main.tile[x, y] = new Tile();

                            Tile tile = Main.tile[x, y];

                            Vector2 vector9;
                            vector9.X = x * 16;
                            vector9.Y = y * 16;
                            if (P.position.X + (float)(P.width / 2) > vector9.X && P.position.X + (float)(P.width / 2) < vector9.X + 16f && P.position.Y + (float)(P.height / 2) > vector9.Y && P.position.Y + (float)(P.height / 2) < vector9.Y + 16f && tile.nactive() && (Main.tileSolid[tile.type] || tile.type == 314))
                                flag3 = false;
                        }
                    }
                    if (flag3)
                        isHooked = false;
                    else //if (owner.grapCount < 10)
                        mp.grapplingBeam = P.whoAmI;
                        //owner.grapCount++;
                }
                else // Hooked onto NPC
                {
                    if (Main.npc[(int)projectile.ai[1]].active)
                    {
                        NPC target = Main.npc[(int)projectile.ai[1]];
                        mp.grapplingBeam = P.whoAmI;
                        projectile.position = target.position + new Vector2(0, target.height / 2);
                        projectile.velocity = Vector2.Zero;
                    }
                    else
                        isHooked = false;
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
					num111 = 0;
				if (num112 > Main.maxTilesX)
					num112 = Main.maxTilesX;
				if (num113 < 0)
					num113 = 0;
				if (num114 > Main.maxTilesY)
					num114 = Main.maxTilesY;

				for (int x = num111; x < num112; x++)
				{
					int y = num113;
					while (y < num114)
					{
						if (Main.tile[x, y] == null)
							Main.tile[x, y] = new Tile();

                        Tile tile = Main.tile[x, y];

                        Vector2 vector8;
						vector8.X = x * 16;
						vector8.Y = y * 16;
						if (P.position.X + P.width > vector8.X && P.position.X < vector8.X + 16f && P.position.Y + P.height > vector8.Y && P.position.Y < vector8.Y + 16f && tile.nactive() && (Main.tileSolid[tile.type] || tile.type == 314))
						{
							Main.PlaySound(SoundLoader.customSoundType, (int)owner.Center.X, (int)owner.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/GrappleLatch"));
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
                    if(projectile.getRect().Intersects(Main.npc[i].getRect()))
                    {
                        NPC target = Main.npc[i];

                        if(GrappableNPCs.Contains(target.type))
                        {
                            projectile.ai[1] = i;

                            projectile.velocity *= 0;
                            isHooked = true;
                            projectile.netUpdate = true;
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
					Main.projectile[oldestProjectile].Kill();
			}
		}
		public override bool PreKill(int timeLeft)
		{
			isHooked = false;
			return true;
		}

		public override bool PreDraw(SpriteBatch s, Color lightColor)
		{
			this.DrawChain(owner.Center, projectile.Center, mod.GetTexture("Gore/GrappleBeamChain"), s);
			return true;
		}
		public override bool PreDrawExtras(SpriteBatch spriteBatch)
		{
			return false;
		}
		public void DrawChain(Vector2 start, Vector2 end, Texture2D name, SpriteBatch spriteBatch)
		{
			start -= Main.screenPosition;
			end -= Main.screenPosition;

			int linklength = name.Height;
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
				spriteBatch.Draw(name,
				new Rectangle((int)links[i].X, (int)links[i].Y, name.Width, linklength), null,
				color, rotation+1.57f, new Vector2(name.Width/2f, linklength), SpriteEffects.None, 1f);

				Lighting.AddLight(LR, 229f/255f, 249f/255f, 255f/255f);
			}
		}
	}
}
