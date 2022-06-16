using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.IO;

using MetroidMod.Content.DamageClasses;

namespace MetroidMod.Content.Projectiles
{
	public class DiffusionBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diffusion Beam");
		}
		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 20;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Projectile.extraUpdates = 2;
		}

		public override void PostAI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			if(Projectile.numUpdates == 0)
			{
				int dust_type = (int)Projectile.ai[0];
				int raw_color = (int)Projectile.ai[1];
				Color color = new Color((raw_color >> 16) & 0xff, (raw_color >> 8) & 0xff, raw_color & 0xff);

				int dust = Dust.NewDust(Projectile.Center, 1, 1, dust_type, 0, 0, 100, color, 2.0f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = new Vector2((Main.rand.Next(50)-25)*0.1f, (Main.rand.Next(50)-25)*0.1f);
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.tileCollide)
			{
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				return false;
			}
			return true;
		}
	}
}