using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetroidModPorted.Content.DamageClasses;

namespace MetroidModPorted.Default
{
	[Autoload(false)]
	internal class MBSpecialProjectile : ModProjectile
	{
		public ModMBSpecial modMBAddon;
		public MBSpecialExplosion explosion;
		public MBSpecialProjectile(ModMBSpecial modMBAddon)
		{
			this.modMBAddon = modMBAddon;
		}

		public override string Texture => modMBAddon.ProjectileTexture;
		public override string Name => $"{modMBAddon.Name}Projectile";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(modMBAddon.DisplayName.GetDefault());
			Main.projFrames[Type] = 6;
		}
		public override void SetDefaults()
		{
			explosion = (MBSpecialExplosion)modMBAddon.ModExplosionProjectile;
			modMBAddon.Projectile = Projectile;
			modMBAddon.ProjectileType = Type;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;//138;
			Projectile.ownerHitCheck = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Projectile.light = 0.2f;
		}
		public override void AI()
		{
			float scalez = 0.2f;
			Lighting.AddLight(Projectile.Center, scalez, scalez, scalez);

			if (Projectile.frameCounter++ >= (int)(Projectile.timeLeft / 7.5f))
			{
				Projectile.frame = (Projectile.frame + 1) % 6;
				Projectile.frameCounter = 0;
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)Projectile.position.X, (int)Projectile.position.Y, SoundLoader.GetSoundSlot(modMBAddon.ExplosionSound));
			if (!modMBAddon.Kill(timeLeft)) { return; }
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, modMBAddon.ExplosionProjectileType, (int)Math.Floor(Projectile.damage*modMBAddon.DamageMultiplier), modMBAddon.Knockback, Projectile.owner);
		}
	}
}
