using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.MagMaul
{
	public class MagMaulChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
		}
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
			Projectile.width += 44;
			Projectile.height += 44;
			Projectile.scale = 3f;
			/*Projectile.position.X = Projectile.position.X + (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (Projectile.height / 2);
			Projectile.position.X = Projectile.position.X + (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (Projectile.height / 2);*/
			//mProjectile.Diffuse(Projectile, 286);
			Projectile.Damage();
			foreach (NPC target in Main.npc)
			{
				if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height))
				{
					Projectile.Damage();
					Projectile.usesLocalNPCImmunity = true;
					Projectile.localNPCHitCooldown = 1;
				}
			}
			SoundEngine.PlaySound(Sounds.Items.Weapons.MagMaulExplode, Projectile.position);
			mProjectile.DustyDeath(Projectile, 286);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(24, 600);
		}
	}
}
