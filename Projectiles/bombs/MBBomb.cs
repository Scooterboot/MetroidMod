using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
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
                //projectile.damage = 10;
				projectile.knockBack = 4f;
			}

			if (projectile.ai[0] == 0)
			{
				if (projectile.localAI[0]++ > 5)
				{
					projectile.localAI[0] = 6;
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
				if (projectile.velocity.X != oldVelocity.X)
					projectile.velocity.X = projectile.velocity.X * -.5f;
				if (projectile.velocity.Y != oldVelocity.X && projectile.velocity.Y > 1f)
					projectile.velocity.Y = projectile.velocity.Y * -.5f;
			}
			return false;
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
						direction *= (BombRadius - distance);
						if(player.whoAmI == projectile.owner)
						{
							if(Math.Abs(player.Center.X-projectile.Center.X) <= 2f)
							{
								direction.X = 0f;
							}
							if(player.Center.Y < projectile.Center.Y+BombRadius)
							{
								direction.Y = -BombRadius;
							}
						}
                        player.velocity += direction;// * (BombRadius - distance);
                        player.GetModPlayer<MPlayer>().spiderball = false;

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
            if ((int)projectile.ai[1] == 7) //Crystal
            {
                for(int i = 0; i < 3; i++)
                {
                    Vector2 vel = Main.rand.NextVector2CircularEdge(5f, 5f);
                    Projectile.NewProjectile(projectile.Center, vel, ProjectileID.CrystalShard, projectile.damage / 2, 1, projectile.owner);
                }
            }
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.timeLeft > 3)
            {
                projectile.timeLeft = 3;
            }

            switch ((int)projectile.ai[1])
            {
                case 1:
                    target.AddBuff(BuffID.Poisoned, 600);
                    break;
                case 2:
                    target.AddBuff(BuffID.OnFire, 600);
                    break;
                case 3:
                    target.AddBuff(BuffID.Frostburn, 600);
                    break;
                case 4:
                    target.AddBuff(BuffID.CursedInferno, 600);
                    break;
                case 5:
                    target.AddBuff(BuffID.Ichor, 600);
                    break;
                case 6:
                    target.AddBuff(BuffID.ShadowFlame, 600);
                    break;
                case 7: //Crystal
                    break;
                case 8:
                    target.AddBuff(BuffID.Venom, 600);
                    break;
                case 9:
                    target.AddBuff(mod.BuffType("PhazonDebuff"), 600);
                    break;
                case 10: //Pumpkin Bomb
                    Projectile.NewProjectile(projectile.Center, projectile.DirectionTo(target.Center) * 8, ProjectileID.FlamingJack, (int)(damage * 1.5f), knockback + 3, projectile.owner, target.whoAmI);
                    break;
                case 11:
                    target.AddBuff(BuffID.BetsysCurse, 600);
                    target.AddBuff(BuffID.Oiled, 600);
                    target.AddBuff(BuffID.OnFire, 600);
                    break;
                case 12:
                    target.AddBuff(BuffID.Daybreak, 600);
                    break;
            }
        }
        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            switch ((int)projectile.ai[1])
            {
                case 1:
                    tex = ModContent.GetTexture("MetroidMod/Projectiles/bombs/PoisonBomb");
                    break;
                case 2:
                    tex = ModContent.GetTexture("MetroidMod/Projectiles/bombs/FireBomb");
                    break;
                case 3:
                    tex = ModContent.GetTexture("MetroidMod/Projectiles/bombs/FrostburnBomb");
                    break;
                case 4:
                    tex = ModContent.GetTexture("MetroidMod/Projectiles/bombs/CursedFlameBomb");
                    break;
                case 5:
                    tex = ModContent.GetTexture("MetroidMod/Projectiles/bombs/IchorBomb");
                    break;
                case 6:
                    tex = ModContent.GetTexture("MetroidMod/Projectiles/bombs/ShadowflameBomb");
                    break;
                case 7:
                    tex = ModContent.GetTexture("MetroidMod/Projectiles/bombs/CrystalBomb");
                    break;
                case 8:
                    tex = ModContent.GetTexture("MetroidMod/Projectiles/bombs/VenomBomb");
                    break;
                case 9:
                    tex = ModContent.GetTexture("MetroidMod/Projectiles/bombs/PhazonBomb");
                    break;
                case 10:
                    tex = ModContent.GetTexture("MetroidMod/Projectiles/bombs/PumpkinBomb");
                    break;
                case 11:
                    tex = ModContent.GetTexture("MetroidMod/Projectiles/bombs/BetsyBomb");
                    break;
                case 12:
                    tex = ModContent.GetTexture("MetroidMod/Projectiles/bombs/SolarFireBomb");
                    break;
            }
            Rectangle? rect = new Rectangle?(new Rectangle(0, projectile.frame * (tex.Height / Main.projFrames[projectile.type]), tex.Width, tex.Height / Main.projFrames[projectile.type]));
            sb.Draw(tex, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), rect, lightColor, projectile.rotation, new Vector2(tex.Width / 2, (tex.Height / Main.projFrames[projectile.type]) / 2), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
