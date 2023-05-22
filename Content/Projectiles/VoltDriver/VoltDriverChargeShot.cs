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
			// DisplayName.SetDefault("Volt Driver Charge Shot");
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
			int shootSpeed = 2;
			if (Items.Weapons.PowerBeam.shooty.Contains("wave") || Items.Weapons.PowerBeam.shooty.Contains("nebula"))
			{
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile);
			}
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
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 3, dustType, 2f);
			mProjectile.HomingBehavior(Projectile, shootSpeed);
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width += 125;
			Projectile.height += 125;
			Projectile.scale = 3f;
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
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.Name.Contains("Red"))
			{
				if (Projectile.Name.Contains("Ice"))
				{
					target.AddBuff(44, 300);
				}
				else
				{
					target.AddBuff(24, 300);
				}
			}
			if (Projectile.Name.Contains("Nova"))
			{
				if (Projectile.Name.Contains("Ice"))
				{
					target.AddBuff(44, 300);
				}
				else
				{
					target.AddBuff(39, 300);
				}
			}
			if (Projectile.Name.Contains("Ice") || Projectile.Name.Contains("Stardust"))
			{
				string buffName = "IceFreeze";
				target.AddBuff(Mod.Find<ModBuff>(buffName).Type, 300);
			}

			if (Projectile.Name.Contains("Solar"))
			{
				target.AddBuff(189, 300);
			}
			SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverDaze, Projectile.position);
			target.AddBuff(31, 180);
		}
	}
}
