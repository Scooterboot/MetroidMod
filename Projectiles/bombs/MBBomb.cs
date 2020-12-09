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
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 1;
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
			if (projectile.ai[0] == 0)
			{
				if (projectile.ai[1]++ > 5)
				{
					projectile.ai[1] = 6;
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
			projectile.width = (int)(BombRadius*2f);
			projectile.height = (int)(BombRadius*2f);
			projectile.position.X = projectile.position.X - (projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (projectile.height / 2);

			projectile.Damage();
			
			for (int i = 0; i < 200; ++i)
			{
				NPC npc = Main.npc[i];
				if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.type != NPCID.TargetDummy && !npc.boss)
				{
					Vector2 direction = npc.Center - projectile.Center;
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
				if (player.active && ((player.hostile && player.team != Main.player[projectile.owner].team) || player.whoAmI == projectile.owner) && !player.dead)
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
							direction.Y = -BombRadius;
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
			
			Main.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/BombExplode"));

			int dustType = 59, dustType2 = 61;
			float dustScale = 5f, dustScale2 = 5f;
			if(projectile.type == mod.ProjectileType("PoisonBomb"))
			{
				dustType = 0;
				dustScale = 2f;
			}
			if(projectile.type == mod.ProjectileType("FireBomb"))
			{
				dustType = 6;
				dustType2 = 6;
				dustScale2 = 3f;
			}
			if(projectile.type == mod.ProjectileType("FrostburnBomb"))
			{
				dustType = 135;
				dustType2 = 135;
				dustScale2 = 3f;
			}
			if(projectile.type == mod.ProjectileType("CursedFlameBomb"))
			{
				dustType = 75;
				dustType2 = 75;
				dustScale2 = 3f;
			}
			if(projectile.type == mod.ProjectileType("IchorBomb"))
			{
				dustType = 169;
				dustType2 = 170;
				dustScale = 4f;
				dustScale2 = 2f;
			}
			if(projectile.type == mod.ProjectileType("ShadowflameBomb"))
			{
				dustType = 62;
				dustType2 = 27;
				dustScale2 = 3f;
			}
			if(projectile.type == mod.ProjectileType("CrystalBomb"))
			{
				dustType = 70;
				dustScale = 3f;
				dustType2 = 70;
				dustScale2 = 2f;
				
				int max = 9;
				float angle = Main.rand.Next(360 / max);
				for(int i = 0; i < max; i++)
				{
					//Vector2 vel = Main.rand.NextVector2CircularEdge(5f, 5f);
					float rot = (float)Angle.ConvertToRadians(angle + ((360f / max) * i));
					Vector2 vel = rot.ToRotationVector2() * 10f;
					Projectile.NewProjectile(projectile.Center, vel, ProjectileID.CrystalShard, projectile.damage / 2, 1, projectile.owner);
				}
			}
			if(projectile.type == mod.ProjectileType("VenomBomb"))
			{
				dustType = 171;
				dustType2 = 205;
				dustScale = 2.5f;
				dustScale2 = 2.5f;
			}
			if(projectile.type == mod.ProjectileType("PhazonBomb"))
			{
				dustType = 68;
				dustType2 = 68;
				dustScale = 3f;
				dustScale2 = 2f;
			}
			if(projectile.type == mod.ProjectileType("PumpkinBomb"))
			{
				dustType = 6;
				dustType2 = 6;
				
				int max = 3;
				float angle = Main.rand.Next(360 / max);
				for(int i = 0; i < max; i++)
				{
					float rot = (float)Angle.ConvertToRadians(angle + ((360f / max) * i));
					Vector2 vel = rot.ToRotationVector2() * 5f;
					Projectile proj = Main.projectile[Projectile.NewProjectile(projectile.Center, vel, ProjectileID.JackOLantern, projectile.damage / max, projectile.knockBack + 3, projectile.owner)];
					proj.timeLeft = 60;
				}
			}
			if(projectile.type == mod.ProjectileType("BetsyBomb"))
			{
				dustType = 55;
				dustType2 = 158;
				dustScale = 3f;
				dustScale2 = 3f;
			}
			if(projectile.type == mod.ProjectileType("SolarFireBomb"))
			{
				dustType = 158;
				dustType2 = 259;
				dustScale = 4f;
				dustScale2 = 2f;
			}
			
			for (int i = 0; i < 25; i++)
			{
				int newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, default(Color), dustScale);
				Main.dust[newDust].velocity *= 1.4f;
				Main.dust[newDust].noGravity = true;

				newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType2, 0f, 0f, 100, default(Color), dustScale2);
				Main.dust[newDust].velocity *= 1.4f;
				Main.dust[newDust].noGravity = true;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(projectile.timeLeft > 0)
			{
				projectile.timeLeft = 0;
			}
			
			if(projectile.type == mod.ProjectileType("PoisonBomb"))
				target.AddBuff(BuffID.Poisoned, 600);
			if(projectile.type == mod.ProjectileType("FireBomb"))
				target.AddBuff(BuffID.OnFire, 600);
			if(projectile.type == mod.ProjectileType("FrostburnBomb"))
				target.AddBuff(BuffID.Frostburn, 600);
			if(projectile.type == mod.ProjectileType("CursedFlameBomb"))
				target.AddBuff(BuffID.CursedInferno, 600);
			if(projectile.type == mod.ProjectileType("IchorBomb"))
				target.AddBuff(BuffID.Ichor, 600);
			if(projectile.type == mod.ProjectileType("ShadowflameBomb"))
				target.AddBuff(BuffID.ShadowFlame, 600);
			if(projectile.type == mod.ProjectileType("VenomBomb"))
				target.AddBuff(BuffID.Venom, 600);
			if(projectile.type == mod.ProjectileType("PhazonBomb"))
				target.AddBuff(mod.BuffType("PhazonDebuff"), 600);
			//if(projectile.type == mod.ProjectileType("PumpkinBomb"))
			//	Projectile.NewProjectile(projectile.Center, projectile.DirectionTo(target.Center) * 8, ProjectileID.FlamingJack, (int)(damage * 1.5f), knockback + 3, projectile.owner, target.whoAmI);
			if(projectile.type == mod.ProjectileType("BetsyBomb"))
			{
				target.AddBuff(BuffID.BetsysCurse, 600);
				target.AddBuff(BuffID.Oiled, 600);
				target.AddBuff(BuffID.OnFire, 600);
			}
			if(projectile.type == mod.ProjectileType("SolarFireBomb"))
				target.AddBuff(BuffID.Daybreak, 600);
		}
	}
	public class PoisonBomb : MBBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poison Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
	}
	public class FireBomb : MBBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
	}
	public class FrostburnBomb : MBBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frostburn Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
	}
	public class CursedFlameBomb : MBBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Fire Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
	}
	public class IchorBomb : MBBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
	}
	public class ShadowflameBomb : MBBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
	}
	public class CrystalBomb : MBBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
	}
	public class VenomBomb : MBBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Venom Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
	}
	public class PhazonBomb : MBBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
	}
	public class PumpkinBomb : MBBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pumpkin Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
	}
	public class BetsyBomb : MBBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Betsy Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
	}
	public class SolarFireBomb : MBBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Fire Morph Ball Bomb");
			Main.projFrames[projectile.type] = 6;
		}
	}
}
