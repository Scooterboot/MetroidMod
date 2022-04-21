using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Projectiles
{
	[Autoload(true)]
	class PowerBeamShot : BeamShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/PowerBeamShot";
		public override string Name => "PowerBeamShot";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 1.5f;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			//Color color = modBeam.BeamColor;
			//Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if(Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			DustyDeath(Projectile, 64);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			DrawCentered(Projectile);
			return false;
		}
	}
}
