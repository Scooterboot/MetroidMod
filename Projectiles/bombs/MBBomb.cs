using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.bombs
{
	public class MBBomb : ModProjectile
	{
		internal readonly float light_scale = 0.2f;

		internal readonly float Xthreshold = 8f; //max speed
		internal readonly float BombRadius = 50f; //max speed

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;

			projectile.light = 0.2f;
			projectile.aiStyle = -1;
			projectile.penetrate = 1;
			projectile.timeLeft = 40;

			projectile.ranged = true;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.ownerHitCheck = true;
		}

		public override void AI()
		{
			Main.player[projectile.owner].heldProj = projectile.whoAmI;
			if (projectile.owner == Main.myPlayer && projectile.timeLeft < 4)
			{
				projectile.tileCollide = false;
				// Set to transparant. This projectile technically lives as transparant for about 3 frames.
				projectile.alpha = 255;
				// change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
				projectile.position.X = projectile.position.X + (projectile.width / 2);
				projectile.position.Y = projectile.position.Y + (projectile.height / 2);
				projectile.width = 100;
				projectile.height = 100;
				projectile.position.X = projectile.position.X - (projectile.width / 2);
				projectile.position.Y = projectile.position.Y - (projectile.height / 2);
				projectile.damage = 10;
				projectile.knockBack = 4f;
			}

			if (projectile.ai[0] == 0)
			{
				if (projectile.ai[1]++ > 5)
				{
					projectile.ai[1] = 5;
					if (projectile.velocity.Y == 0F && projectile.velocity.X != 0f)
					{
						projectile.velocity.X *= .97f;
						if (projectile.velocity.X > -.01f && projectile.velocity.X < .01f)
						{
							projectile.velocity.X = 0;
							projectile.netUpdate = true;
						}
					}
					projectile.velocity.Y += .2f;
				}
				projectile.rotation += projectile.velocity.X * .1f;
				if (projectile.velocity.Y > 16)
					projectile.velocity.Y = 16;
			}

			#region Visuals
			if (projectile.frameCounter++ >= (int)(projectile.timeLeft / 3.75f))
			{
				projectile.frame = (projectile.frame + 1) % 6;
                projectile.frameCounter = 0;
			}
			Lighting.AddLight(projectile.Center, light_scale, light_scale, light_scale);
			#endregion
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.ai[0] == 0)
			{
				if (projectile.velocity.X != 0)
					projectile.velocity.X = projectile.velocity.X * -.5f;
				if (projectile.velocity.Y != 0 && projectile.velocity.Y > 1f)
					projectile.velocity.Y = projectile.velocity.Y * -.5f;
			}
			return (false);
		}

		public override void Kill(int timeLeft)
		{
			projectile.position.X = projectile.position.X + (projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (projectile.height / 2);
			projectile.width = 8;
			projectile.height = 8;
			projectile.position.X = projectile.position.X - (projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (projectile.height / 2);
			projectile.active = false;

			Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/BombExplode"));

			for (int i = 0; i < 25; i++)
			{
				int newDust = Dust.NewDust(new Vector2(projectile.position.X-BombRadius, projectile.position.Y-BombRadius), projectile.width+(int)BombRadius*2, projectile.height+(int)BombRadius*2, 59, 0f, 0f, 100, default(Color), 5f);
				Main.dust[newDust].velocity *= 1.4f;
				Main.dust[newDust].noGravity = true;

                newDust = Dust.NewDust(new Vector2(projectile.position.X-BombRadius, projectile.position.Y-BombRadius), projectile.width+(int)BombRadius*2, projectile.height+(int)BombRadius*2, 61, 0f, 0f, 100, default(Color), 5f);
				Main.dust[newDust].velocity *= 1.4f;
				Main.dust[newDust].noGravity = true;
			}
			Rectangle rect = new Rectangle((int)(projectile.position.X-BombRadius), (int)(projectile.position.Y-BombRadius), (int)(projectile.width+BombRadius*2), (int)(projectile.height+BombRadius*2));
            
            for (int i = 0; i < 200; ++i)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.type != NPCID.TargetDummy && !npc.boss)
                {
                    Vector2 direction = npc.position - projectile.position;
                    float distance = direction.Length();
                    direction.Normalize();
                    if (distance < BombRadius)
                    {
                        npc.velocity += direction * (BombRadius - distance);

                        if (npc.velocity.X > Xthreshold)
                            npc.velocity.X = Xthreshold;
                        if (npc.velocity.X < -Xthreshold)
                            npc.velocity.X = -Xthreshold;
                        if (npc.velocity.Y > Xthreshold)
                            npc.velocity.Y = Xthreshold;
                        if (npc.velocity.Y < -Xthreshold)
                            npc.velocity.Y = -Xthreshold;
                    }
                }
            }

            for (int i = 0; i < 255; ++i)
            {
                Player player = Main.player[i];
                if (((player.active && player.hostile && player.team != Main.player[projectile.owner].team) || player.whoAmI == projectile.owner) && !player.dead)
                {
                    Vector2 direction = player.Center - projectile.Center;
                    float distance = direction.Length();
                    direction.Normalize();
                    if (distance < BombRadius)
                    {
                        player.velocity += direction * (BombRadius - distance);
                        player.GetModPlayer<MPlayer>(mod).spiderball = false;

                        if (player.velocity.X > Xthreshold)
                            player.velocity.X = Xthreshold;
                        if (player.velocity.X < -Xthreshold)
                            player.velocity.X = -Xthreshold;
                        if (player.velocity.Y > Xthreshold)
                            player.velocity.Y = Xthreshold;
                        if (player.velocity.Y < -Xthreshold)
                            player.velocity.Y = -Xthreshold;
                    }
                }
            }
		}
	}
}
