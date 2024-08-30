using System;
using MetroidMod.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.BattleHammer
{
	public class BattleHammerShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("BattleHammer Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.scale = .75f;
			Projectile.aiStyle = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 110, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Projectile.timeLeft >= 1)
				modifiers.ArmorPenetration += Luminite ? 15 : DiffBeam ? 10 : 5;
			base.ModifyHitNPC(target, ref modifiers);
		}
		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(Sounds.Items.Weapons.BattleHammerImpactSound, Projectile.position);
			mProjectile.Explode(Luminite ? 80 : DiffBeam ? 60 : 20, Luminite ? 10 : DiffBeam ? 5 : 3);
			mProjectile.Diffuse(Projectile, 110);
			mProjectile.Diffuse(Projectile, 55);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
