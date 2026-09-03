using MetroidMod.Common.Configs;
using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles
{
	public class SpeedBall : ModProjectile
	{
		private int SpeedSound = 0;
		public ActiveSound activeSound;
		public SoundEffectInstance soundInstance;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Mock Ball");
		}
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Projectile.penetrate = -1;
			Projectile.timeLeft = 9000;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 7;
			Projectile.alpha = 255;
		}
		public override void AI()
		{
			Player P = Main.player[Projectile.owner];
			Projectile.position.X = P.Center.X - (Projectile.width / 2);
			Projectile.position.Y = P.Center.Y - (Projectile.height / 2);

			if (!MConfigItems.Instance.muteSpeedBooster)
			{
				SpeedSound++;
				if (SpeedSound == 4)
				{
					if (SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.SpeedBoosterStartup, P.position), out activeSound))
					{
						soundInstance = activeSound.Sound;
					}

				}
				if (soundInstance != null && SpeedSound == 82)
				{
					soundInstance.Stop();
					if (SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.SpeedBoosterLoop, P.position), out activeSound))
					{
						soundInstance = activeSound.Sound;

						SpeedSound = 68;
					}
				}
			}
			MPlayer mp = P.GetModPlayer<MPlayer>();
			if (!mp.ballstate || !mp.speedBoosting || mp.SMoveEffect > 0)
			{
				if (soundInstance != null)
				{
					soundInstance.Stop(true);
				}
				Projectile.Kill();
			}
			foreach (Projectile Pr in Main.projectile) if (Pr != null)
				{
					if (Pr.active && (Pr.type == ModContent.ProjectileType<ShineBall>() || Pr.type == ModContent.ProjectileType<SpeedBoost>()))
					{
						if (soundInstance != null)
						{
							soundInstance.Stop(true);
						}
						Projectile.Kill();
						return;
					}
				}
			Lighting.AddLight((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), 0, 0.75f, 1f);

			if (activeSound != null)
			{
				activeSound.Position = P.Center;
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			if (target.knockBackResist > 0)
			{
				target.velocity = new Vector2(player.velocity.X * target.knockBackResist, -8);
			}
			player.GiveImmuneTimeForCollisionAttack(4);
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.SourceDamage.Flat += (int)(target.damage * 1.5f);
		}
	}
}
