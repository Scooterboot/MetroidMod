using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles
{
	public class HyperBeamShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
		}

		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			MPlayer mp = Main.player[projectile.owner].GetModPlayer<MPlayer>();
			Lighting.AddLight(projectile.Center, (float)mp.r/255f, (float)mp.g/255f, (float)mp.b/255f);
			mProjectile.WaveCollide(projectile,18);
		}
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			MPlayer mp = Main.player[projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.PlasmaDrawTrail(projectile,Main.player[projectile.owner],sb,10,1f,new Color(mp.r, mp.g, mp.b, 255));
			return false;
		}
		public override void Kill(int timeLeft)
		{
			MPlayer mp = Main.player[projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.DustyDeath(projectile, 66, true, 1f, new Color(mp.r, mp.g, mp.b, 255));
		}
	}
}