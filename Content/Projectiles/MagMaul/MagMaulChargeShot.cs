using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.MagMaul
{
	public class MagMaulChargeShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.scale = 1.5f;
			Projectile.aiStyle = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
		}

		public override void AI()
		{
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			Projectile.rotation += 0.5f * Projectile.direction;
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 286, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void OnKill(int timeLeft)
		{
			//Projectile.damage /= (int)3.5;
			if(Luminite || DiffBeam)
			{
				mProjectile.Explode(Luminite ? 88 : DiffBeam ? 44 : 22, Luminite ? 4f : DiffBeam ? 3f : 2f);
			}
			SoundEngine.PlaySound(Sounds.Items.Weapons.MagMaulExplode, Projectile.position);
			mProjectile.Diffuse(Projectile, 286);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Luminite || DiffBeam)
			{
				target.AddBuff(24, 600);
			}
		}
	}
}
