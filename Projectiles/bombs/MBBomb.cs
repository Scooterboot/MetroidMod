using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.bombs
{
	public class MBBomb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = 14;
			projectile.timeLeft = 40;
			projectile.ownerHitCheck = true;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.light = 0.2f;
		}

		public override void AI()
		{
			Main.player[projectile.owner].heldProj = projectile.whoAmI;
			if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 3)
			{
				projectile.tileCollide = false;
				// Set to transparant. This projectile technically lives as  transparant for about 3 frames
				projectile.alpha = 255;
				// change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
				projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
				projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
				projectile.width = 100;
				projectile.height = 100;
				projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
				projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
				projectile.damage = 10;
				projectile.knockBack = 4f;
			}
			float scalez = 0.2f;
			Lighting.AddLight(projectile.Center, scalez, scalez, scalez);

			#region frames
			if (projectile.frameCounter++ >= (int)(projectile.timeLeft / 3.75f))
			{
				projectile.frame = (projectile.frame + 1) % 6;
                projectile.frameCounter = 0;
			}
			#endregion
		}

		public override void Kill(int timeLeft)
		{
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = 8;
			projectile.height = 8;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			projectile.active = false;

			Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/BombExplode"));
			
			float Xthreshold = 8f; //max speed
			float BombRadius = 50f; //max speed
			for (int i = 0; i < 25; i++)
			{
				int newDust = Dust.NewDust(new Vector2(this.projectile.position.X-BombRadius, this.projectile.position.Y-BombRadius), this.projectile.width+(int)BombRadius*2, this.projectile.height+(int)BombRadius*2, 59, 0f, 0f, 100, default(Color), 5f);
				Main.dust[newDust].velocity *= 1.4f;
				Main.dust[newDust].noGravity = true;

                newDust = Dust.NewDust(new Vector2(this.projectile.position.X-BombRadius, this.projectile.position.Y-BombRadius), this.projectile.width+(int)BombRadius*2, this.projectile.height+(int)BombRadius*2, 61, 0f, 0f, 100, default(Color), 5f);
				Main.dust[newDust].velocity *= 1.4f;
				Main.dust[newDust].noGravity = true;
			}
			Rectangle rect = new Rectangle((int)(projectile.position.X-BombRadius), (int)(projectile.position.Y-BombRadius), (int)(projectile.width+BombRadius*2), (int)(projectile.height+BombRadius*2));
            
            for (int i = 0; i < 200; ++i)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.type != 488 && !npc.boss)
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
                if ((player.active && player.hostile && player.team != Main.player[projectile.owner].team) || player.whoAmI == projectile.owner)
                {
                    Vector2 direction = player.Center - projectile.Center;
                    float distance = direction.Length();
                    direction.Normalize();
                    if (distance < BombRadius)
                    {
						direction *= (BombRadius - distance);
						if(player.whoAmI == projectile.owner)
						{
							if(Math.Abs(player.Center.X-projectile.Center.X) <= 2f)
							{
								direction.X = 0f;
							}
							if(player.Center.Y < projectile.position.Y+projectile.height)
							{
								direction.Y = -BombRadius;
							}
						}
                        player.velocity += direction;// * (BombRadius - distance);
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
