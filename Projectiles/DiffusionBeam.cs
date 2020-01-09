using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.IO;

namespace MetroidMod.Projectiles
{
	public class DiffusionBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diffusion Beam");
		}
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.timeLeft = 20;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.extraUpdates = 2;
		}

		public override void PostAI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			if(projectile.numUpdates == 0)
			{
				int dust_type = (int)projectile.ai[0];
				int raw_color = (int)projectile.ai[1];
				Color color = new Color((raw_color >> 16) & 0xff, (raw_color >> 8) & 0xff, raw_color & 0xff);

				int dust = Dust.NewDust(projectile.Center, 1, 1, dust_type, 0, 0, 100, color, 2.0f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = new Vector2((Main.rand.Next(50)-25)*0.1f, (Main.rand.Next(50)-25)*0.1f);
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.tileCollide)
			{
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				return false;
			}
			return true;
		}
	}
}