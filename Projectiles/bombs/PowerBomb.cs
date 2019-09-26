using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Projectiles.bombs
{
	public class PowerBomb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Bomb");
			Main.projFrames[projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = 0;
			projectile.timeLeft = 60;//138;
			projectile.ownerHitCheck = true;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.light = 0.2f;
		}
		public override void AI()
		{
			float scalez = 0.2f;
			Lighting.AddLight(projectile.Center, scalez, scalez, scalez);

			if (projectile.frameCounter++ >= (int)(projectile.timeLeft / 7.5f))
			{
				projectile.frame = (projectile.frame + 1) % 6;
				projectile.frameCounter = 0;
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/PowerBombExplode"));
			int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("PowerBombExplosion"), projectile.damage, 3, projectile.owner);
		}
	}
	public class VortexBomb : PowerBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Bomb");
			Main.projFrames[projectile.type] = 6;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/VortexBombExplode"));
			int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("VortexBombExplosion"), projectile.damage, 0, projectile.owner);
		}
	}
	public class SolarBomb : PowerBomb
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Bomb");
			Main.projFrames[projectile.type] = 6;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SolarBombExplode"));
			int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("SolarBombExplosion"), projectile.damage*2, 3, projectile.owner);
		}
	}
}