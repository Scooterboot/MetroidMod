using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.VoltDriver
{
	public class VoltDriverChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Volt Driver Charge Shot");
			Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.scale = 1f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 0;
			base.SetDefaults();
		}

		public override void AI()
		{
			if (shot.Contains("wave") || shot.Contains("nebula"))
			{
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile);
			}
			float shootSpeed = Luminite ? 4f : 2f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			if (Projectile.numUpdates == 0)
			{
				Projectile.rotation += 0.5f * Projectile.direction;
				Projectile.frame++;
			}
			if (Projectile.frame > 3)
			{
				Projectile.frame = 0;
			}
			int dustType = 269;
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 3, dustType, 2f);
			mProjectile.HomingBehavior(Projectile, shootSpeed, 11f, !DiffBeam && !Luminite ? 0f : 300f);
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}

		public override void OnKill(int timeLeft)
		{
			Projectile.width += 32;
			Projectile.height += 32;
			Projectile.scale = 3f;
			mProjectile.Diffuse(Projectile, 269);
			SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverChargeImpactSound, Projectile.position);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if(target.active && !target.buffImmune[31] && (Luminite || DiffBeam))
			{
				SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverDaze, Projectile.position);
				target.AddBuff(31, 180);
			}
		}
	}
}
