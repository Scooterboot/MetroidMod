using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.BattleHammer
{
	public class BattleHammerShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BattleHammer Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = .5f;
			Projectile.penetrate = 1;
			Projectile.aiStyle = 1;

        }

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);

            if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 110, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
            if (Projectile.Name.Contains("Spazer") || Projectile.Name.Contains("Vortex"))
            {
                mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
            }
        }
		
		public override void Kill(int timeLeft)
		{
			int dustType = 107;
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width += 125;
			Projectile.height += 125;
			Projectile.scale = 5f;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
			mProjectile.Diffuse(Projectile, 110);
			mProjectile.Diffuse(Projectile, 55);
			SoundEngine.PlaySound(Sounds.Items.Weapons.BattleHammerImpactSound, Projectile.position);
			Projectile.Damage();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
    public class SpazerBattleHammerShot : BattleHammerShot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Name = "Spazer BattleHammer Shot";

            mProjectile.amplitude = 20f * Projectile.scale;
            mProjectile.wavesPerSecond = 1f;
            mProjectile.delay = 4;
        }
    }
    public class VortexBattleHammerShot : BattleHammerShot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Name = "Vortex BattleHammer Shot";

            mProjectile.amplitude = 20f * Projectile.scale;
            mProjectile.wavesPerSecond = 1f;
            mProjectile.delay = 4;
        }
    }
}
