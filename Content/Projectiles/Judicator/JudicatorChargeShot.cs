using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using MetroidMod.Content.Items.Weapons;
using Terraria.GameContent;

namespace MetroidMod.Content.Projectiles.Judicator
{
	public class JudicatorChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Judicator Charge Shot");
		}
		private int GetDepth(MProjectile mp)
		{
			return mp.waveDepth;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 20;
			Projectile.height = 32;
			Projectile.scale = 2f;
			Projectile.timeLeft = 60;
			
			string S  = PowerBeam.SetCondition();
			if (S.Contains("green"))
			{
				Projectile.penetrate = 9;
			}
			if (S.Contains("nova"))
			{
				Projectile.penetrate = 11;
			}
			if (S.Contains("solar"))
			{
				Projectile.penetrate = 16;
			}
		}

		public override void AI()
		{
			string S = PowerBeam.SetCondition();
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
			if (Projectile.timeLeft == 1) //shadowfreeze
			{
				if(S.Contains("wave") || S.Contains("nebula"))
				{
					Projectile.tileCollide = false;
				}
				MProjectile meep = mProjectile;
				Projectile.width *= (int)Math.Abs(Projectile.velocity.Y * Projectile.width / Projectile.width) + GetDepth(meep);
				Projectile.height *= (int)Math.Abs(Projectile.velocity.X * Projectile.height / Projectile.height) + GetDepth(meep);
				Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorFreeze, Projectile.position);
			target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(), 300);
			target.AddBuff(44, 300);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
