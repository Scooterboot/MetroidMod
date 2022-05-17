using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace MetroidModPorted.Content.Projectiles.Boss
{
	public class Omega_PhazonParticle : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon Particle");
		}
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1200;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.scale = 0f;
			Projectile.alpha = 255;
		}
		float acc = 3f;
		float projSpeed = 15f;
		public override void AI()
		{
			Projectile P = Projectile;
			
			Vector2 dest = new Vector2(P.ai[0],P.ai[1]);
			
			Vector2 vec = dest - P.Center;
			float num2 = vec.Length();
			num2 = projSpeed / num2;
			vec *= num2;
			P.velocity.X = (P.velocity.X * acc + vec.X) / (acc + 1f);
			P.velocity.Y = (P.velocity.Y * acc + vec.Y) / (acc + 1f);
			
			if(Vector2.Distance(dest,P.Center+P.velocity) <= 5f)
			{
				P.Kill();
			}
			
			P.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			mProjectile.DustLine(P.position, P.velocity, P.rotation, 0, 3, 68, 1.5f);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
	}
}
