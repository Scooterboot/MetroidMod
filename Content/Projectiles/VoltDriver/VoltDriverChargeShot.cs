using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.VoltDriver
{
	public class VoltDriverChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Volt Driver Charge Shot");
			Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.scale = 1f;

        }

		public override void AI()
		{
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
            if (Projectile.numUpdates == 0)
			{
				Projectile.rotation += 0.5f*Projectile.direction;
				Projectile.frame++;
			}
			if(Projectile.frame > 3)
			{
				Projectile.frame = 0;
			}
			int dustType = 269;
			int shootSpeed = 2;
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 3, dustType, 2f);
			mProjectile.HomingBehavior(Projectile, shootSpeed);
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
            if (Projectile.Name.Contains("Spazer") || Projectile.Name.Contains("Vortex"))
            {
                mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
            }
        }

		public override void Kill(int timeLeft)
		{
			int dustType = 269;
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width += 250;
			Projectile.height += 250;
			Projectile.scale = 5f;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
			mProjectile.Diffuse(Projectile, 269);
			SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverChargeImpactSound, Projectile.position);
			Projectile.Damage();
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverChargeImpactSound, Projectile.position);
			target.AddBuff (31, 180);
		}
	}
    public class VortexVoltDriverChargeShot : VoltDriverChargeShot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Name = "Vortex Volt Driver Charge Shot";

            mProjectile.amplitude = 25f * Projectile.scale;
            mProjectile.wavesPerSecond = 1f;
            mProjectile.delay = 4;
        }
    }
    public class SpazerVoltDriverChargeShot : VoltDriverChargeShot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Name = "Spazer Volt Driver Charge Shot";

            mProjectile.amplitude = 25f * Projectile.scale;
            mProjectile.wavesPerSecond = 1f;
            mProjectile.delay = 4;
        }
    }
}
