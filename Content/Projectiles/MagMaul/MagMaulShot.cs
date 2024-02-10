using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.MagMaul
{
	public class MagMaulShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("MagMaul Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.aiStyle = 14;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
		}

		public override void AI()
		{
			//Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 286, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(Sounds.Items.Weapons.MagMaulExplode, Projectile.position);
			Projectile.width += 20;
			Projectile.height += 20;
			Projectile.scale = 1.6f;
			Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);
			mProjectile.Diffuse(Projectile, 286);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
