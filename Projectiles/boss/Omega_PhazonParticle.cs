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

namespace MetroidMod.Projectiles.boss
{
	public class Omega_PhazonParticle : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon Particle");
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.timeLeft = 1200;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.width = 1;
			projectile.height = 1;
			projectile.scale = 0f;
			projectile.alpha = 255;
		}
		float acc = 3f;
		float projSpeed = 15f;
		public override void AI()
		{
			Projectile P = projectile;
			
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
			
			P.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			mProjectile.DustLine(P.position, P.velocity, P.rotation, 0, 3, 68, 1.5f);
		}
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			return false;
		}
	}
}