using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.Imperialist
{
	public class ImperialistShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperialist Shot");
            //Main.projFrames[Projectile.type] = 2;
        }
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.scale = 2f;
			//Projectile.penetrate = 1;
			//Projectile.aiStyle = 48;

        }

		public override void AI()
		{
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
            //if (Projectile.numUpdates == 0)
			{
				Projectile.frame++;
			}
			//if (Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}
			int dustType = 271;
			int shootSpeed = 60;
			int distance = 0;
			int accuracy = 120;
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 3, dustType, 2f);
			mProjectile.HomingBehavior(Projectile, shootSpeed, distance, accuracy);
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 271, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
            if (Projectile.Name.Contains("Spazer") || Projectile.Name.Contains("Vortex"))
            {
                mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
            }
        }

		public override void Kill(int timeLeft)
		{
			int dustType = 64;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
    public class VortexImperialistShot : ImperialistShot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Name = "Vortex Imperialist Shot";

            mProjectile.amplitude = 5f * Projectile.scale;
            mProjectile.wavesPerSecond = 1f;
        }
    }
    public class SpazerImperialistShot : ImperialistShot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Name = "Spazer Imperialist Shot";

            mProjectile.amplitude = 5f * Projectile.scale;
            mProjectile.wavesPerSecond = 1f;
        }
    }
}
