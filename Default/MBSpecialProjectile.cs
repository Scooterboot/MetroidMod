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
	[CloneByReference]
	internal class MBSpecialProjectile : ModProjectile
	{
		[CloneByReference]
		public ModMBSpecial modMBAddon;
		[CloneByReference]
		public MBSpecialExplosion explosion;
		public MBSpecialProjectile(ModMBSpecial modMBAddon)
		{
			this.modMBAddon = modMBAddon;
		}

		protected override bool CloneNewInstances => true;

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
			SoundEngine.PlaySound(new SoundStyle(modMBAddon.ExplosionSound), Projectile.position);
			if (!modMBAddon.Kill(timeLeft)) { return; }
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, modMBAddon.ExplosionProjectileType, (int)Math.Floor(Projectile.damage*modMBAddon.DamageMultiplier), modMBAddon.Knockback, Projectile.owner);
		}

		public override ModProjectile Clone(Projectile newEntity)
		{
			MBSpecialProjectile inst = (MBSpecialProjectile)base.Clone(newEntity);
			inst.modMBAddon = modMBAddon;
			inst.explosion = explosion;
			return inst;
		}

		public override ModProjectile NewInstance(Projectile entity)
		{
			MBSpecialProjectile inst = (MBSpecialProjectile)base.NewInstance(entity);
			inst.modMBAddon = modMBAddon;
			inst.explosion = explosion;
			return inst;
		}
	}
}
