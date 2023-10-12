using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.MagMaul
{
	public class MagMaulChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("MagMaul Charge Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.scale = 1.5f;
			Projectile.aiStyle = 1;
		}

		public override void AI()
		{
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			if (Projectile.numUpdates == 0)
			{
				Projectile.rotation += 0.5f * Projectile.direction;
				Projectile.frame++;
			}
			if (Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 286, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			if(mProjectile.canDiffuse)
			{
				Projectile.width += 125;
				Projectile.height += 125;
				Projectile.scale = 3f;
				Projectile.position.X = Projectile.position.X + (Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y + (Projectile.height / 2);
				Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);
				//mProjectile.Diffuse(Projectile, 286);
				Projectile.Damage();
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
