using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Projectiles;

namespace MetroidMod.Default
{
	[Autoload(false)]
	[CloneByReference]
	internal class MBWeaponProjectile : ModProjectile
	{
		[CloneByReference]
		public ModMBWeapon modMBAddon;
		public MBWeaponProjectile(ModMBWeapon modMBAddon)
		{
			this.modMBAddon = modMBAddon;
		}

		protected override bool CloneNewInstances => true;

		internal readonly float light_scale = 0.2f;

		internal readonly float Xthreshold = 8f; //max speed
		internal readonly float BombRadius = 50f; //max speed

		public override string Texture => modMBAddon.ProjectileTexture;
		public override string Name => $"{modMBAddon.Name}Projectile";

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault(modMBAddon.DisplayName.GetDefault());
			Main.projFrames[Type] = 6;
		}
		public override void SetDefaults()
		{
			modMBAddon.Projectile = Projectile;
			modMBAddon.ProjectileType = Type;
			Projectile.width = 10;
			Projectile.height = 10;

			Projectile.light = 0.2f;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
			Projectile.timeLeft = 40;

			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.ownerHitCheck = true;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				if (Projectile.ai[1]++ > 5)
				{
					Projectile.ai[1] = 6;
					if (Projectile.velocity.Y == 0F && Projectile.velocity.X != 0f)
					{
						Projectile.velocity.X *= .97f;
						if (Projectile.velocity.X > -.01f && Projectile.velocity.X < .01f)
						{
							Projectile.velocity.X = 0;
							Projectile.netUpdate = true;
						}
					}
					Projectile.velocity.Y += .2f;
				}
				Projectile.rotation += Projectile.velocity.X * .1f;
				if (Projectile.velocity.Y > 16)
					Projectile.velocity.Y = 16;
			}

			#region Visuals
			if (Projectile.frameCounter++ >= (int)(Projectile.timeLeft / 3.75f))
			{
				Projectile.frame = (Projectile.frame + 1) % 6;
				Projectile.frameCounter = 0;
			}
			Lighting.AddLight(Projectile.Center, light_scale, light_scale, light_scale);
			#endregion
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.ai[0] == 0)
			{
				if (Projectile.velocity.X != oldVelocity.X)
					Projectile.velocity.X = Projectile.velocity.X * -.5f;
				if (Projectile.velocity.Y != oldVelocity.X && Projectile.velocity.Y > 1f)
					Projectile.velocity.Y = Projectile.velocity.Y * -.5f;
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.position.X = Projectile.position.X + (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (Projectile.height / 2);
			Projectile.width = (int)(BombRadius * 2f);
			Projectile.height = (int)(BombRadius * 2f);
			Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);

			Projectile.Damage();

			for (int i = 0; i < 200; ++i)
			{
				NPC npc = Main.npc[i];
				if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.type != NPCID.TargetDummy && !npc.boss)
				{
					Vector2 direction = npc.Center - Projectile.Center;
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
				if (player.active && ((player.hostile && player.team != Main.player[Projectile.owner].team) || player.whoAmI == Projectile.owner) && !player.dead)
				{
					Vector2 direction = player.Center - Projectile.Center;
					float distance = direction.Length();
					direction.Normalize();
					if (distance < BombRadius)
					{
						direction *= (BombRadius - distance);
						if (player.whoAmI == Projectile.owner)
						{
							if (Math.Abs(player.Center.X - Projectile.Center.X) <= 2f)
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
			foreach (NPC target in Main.npc)
			{
				if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height))
				{
					Projectile.Damage();
					Projectile.usesLocalNPCImmunity = true;
					Projectile.localNPCHitCooldown = 1;
				}
			}
			SoundEngine.PlaySound(Sounds.Suit.BombExplode, Projectile.Center);

			int dustType = 59, dustType2 = 61;
			float dustScale = 5f, dustScale2 = 5f;
			modMBAddon.Kill(timeLeft, ref dustType, ref dustType2, ref dustScale, ref dustScale2);
			/*if (Projectile.type == mod.ProjectileType("PoisonBomb"))
			{
				dustType = 0;
				dustScale = 2f;
			}
			if (Projectile.type == mod.ProjectileType("FireBomb"))
			{
				dustType = 6;
				dustType2 = 6;
				dustScale2 = 3f;
			}
			if (Projectile.type == mod.ProjectileType("FrostburnBomb"))
			{
				dustType = 135;
				dustType2 = 135;
				dustScale2 = 3f;
			}
			if (Projectile.type == mod.ProjectileType("CursedFlameBomb"))
			{
				dustType = 75;
				dustType2 = 75;
				dustScale2 = 3f;
			}
			if (Projectile.type == mod.ProjectileType("IchorBomb"))
			{
				dustType = 169;
				dustType2 = 170;
				dustScale = 4f;
				dustScale2 = 2f;
			}
			if (Projectile.type == mod.ProjectileType("ShadowflameBomb"))
			{
				dustType = 62;
				dustType2 = 27;
				dustScale2 = 3f;
			}
			if (Projectile.type == mod.ProjectileType("CrystalBomb"))
			{
				dustType = 70;
				dustScale = 3f;
				dustType2 = 70;
				dustScale2 = 2f;

				int max = 9;
				float angle = Main.rand.Next(360 / max);
				for (int i = 0; i < max; i++)
				{
					//Vector2 vel = Main.rand.NextVector2CircularEdge(5f, 5f);
					float rot = (float)Angle.ConvertToRadians(angle + ((360f / max) * i));
					Vector2 vel = rot.ToRotationVector2() * 10f;
					Projectile.NewProjectile(Projectile.Center, vel, ProjectileID.CrystalShard, Projectile.damage / 2, 1, Projectile.owner);
				}
			}
			if (Projectile.type == mod.ProjectileType("VenomBomb"))
			{
				dustType = 171;
				dustType2 = 205;
				dustScale = 2.5f;
				dustScale2 = 2.5f;
			}
			if (Projectile.type == mod.ProjectileType("PhazonBomb"))
			{
				dustType = 68;
				dustType2 = 68;
				dustScale = 3f;
				dustScale2 = 2f;
			}
			if (Projectile.type == mod.ProjectileType("PumpkinBomb"))
			{
				dustType = 6;
				dustType2 = 6;

				int max = 3;
				float angle = Main.rand.Next(360 / max);
				for (int i = 0; i < max; i++)
				{
					float rot = (float)Angle.ConvertToRadians(angle + ((360f / max) * i));
					Vector2 vel = rot.ToRotationVector2() * 5f;
					Projectile proj = Main.Projectile[Projectile.NewProjectile(Projectile.Center, vel, ProjectileID.JackOLantern, Projectile.damage / max, Projectile.knockBack + 3, Projectile.owner)];
					proj.timeLeft = 60;
				}
			}
			if (Projectile.type == mod.ProjectileType("BetsyBomb"))
			{
				dustType = 55;
				dustType2 = 158;
				dustScale = 3f;
				dustScale2 = 3f;
			}
			if (Projectile.type == mod.ProjectileType("SolarFireBomb"))
			{
				dustType = 158;
				dustType2 = 259;
				dustScale = 4f;
				dustScale2 = 2f;
			}*/

			for (int i = 0; i < 25; i++)
			{
				int newDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default(Color), dustScale);
				Main.dust[newDust].velocity *= 1.4f;
				Main.dust[newDust].noGravity = true;

				newDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType2, 0f, 0f, 100, default(Color), dustScale2);
				Main.dust[newDust].velocity *= 1.4f;
				Main.dust[newDust].noGravity = true;
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.timeLeft > 0)
			{
				Projectile.timeLeft = 0;
			}

			modMBAddon.OnHitNPC(target, hit, damageDone);

			/*if (Projectile.type == mod.ProjectileType("PoisonBomb"))
				target.AddBuff(BuffID.Poisoned, 600);
			if (Projectile.type == mod.ProjectileType("FireBomb"))
				target.AddBuff(BuffID.OnFire, 600);
			if (Projectile.type == mod.ProjectileType("FrostburnBomb"))
				target.AddBuff(BuffID.Frostburn, 600);
			if (Projectile.type == mod.ProjectileType("CursedFlameBomb"))
				target.AddBuff(BuffID.CursedInferno, 600);
			if (Projectile.type == mod.ProjectileType("IchorBomb"))
				target.AddBuff(BuffID.Ichor, 600);
			if (Projectile.type == mod.ProjectileType("ShadowflameBomb"))
				target.AddBuff(BuffID.ShadowFlame, 600);
			if (Projectile.type == mod.ProjectileType("VenomBomb"))
				target.AddBuff(BuffID.Venom, 600);
			if (Projectile.type == mod.ProjectileType("PhazonBomb"))
				target.AddBuff(mod.BuffType("PhazonDebuff"), 600);*/
			//if(Projectile.type == mod.ProjectileType("PumpkinBomb"))
			//	Projectile.NewProjectile(Projectile.Center, Projectile.DirectionTo(target.Center) * 8, ProjectileID.FlamingJack, (int)(damage * 1.5f), knockback + 3, Projectile.owner, target.whoAmI);
			/*if (Projectile.type == mod.ProjectileType("BetsyBomb"))
			{
				target.AddBuff(BuffID.BetsysCurse, 600);
				target.AddBuff(BuffID.Oiled, 600);
				target.AddBuff(BuffID.OnFire, 600);
			}
			if (Projectile.type == mod.ProjectileType("SolarFireBomb"))
				target.AddBuff(BuffID.Daybreak, 600);*/
		}

		public override ModProjectile Clone(Projectile newEntity)
		{
			MBWeaponProjectile inst = (MBWeaponProjectile)base.Clone(newEntity);
			inst.modMBAddon = modMBAddon;
			return inst;
		}

		public override ModProjectile NewInstance(Projectile entity)
		{
			MBWeaponProjectile inst = (MBWeaponProjectile)base.NewInstance(entity);
			inst.modMBAddon = modMBAddon;
			return inst;
		}
	}
}
