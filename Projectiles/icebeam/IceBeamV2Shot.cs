using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.icebeam
{
	public class IceBeamV2Shot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Beam V2 Shot";
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 1.5f;
			Main.projFrames[projectile.type] = 3;
		}

		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.iceColor;
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if(projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 59, 0, 0, 100, default(Color), projectile.scale);
				Main.dust[dust].noGravity = true;
			}
			
			Projectile P = projectile;
			P.frame = 0;
			for(int i = 0; i < Main.projectile.Length; i++)
			{
				if(Main.projectile[i].active && Main.projectile[i].owner == P.owner && Main.projectile[i].type == P.type && Main.projectile[i].whoAmI != P.whoAmI)
				{
					Projectile P2 = Main.projectile[i];
					if(Vector2.Distance(P.position, P2.position) <= 24f)
					{
						if(P2.ai[0] == 1f && P.ai[0] == -1f)
						{
							P.frame = 1;
						}
						else if(P2.ai[0] == -1f && P.ai[0] == 1f)
						{
							P.frame = 2;
						}
					}
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(projectile, 59);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			//mProjectile.DrawCentered(projectile, sb);
			mProjectile.PlasmaDraw(projectile, Main.player[projectile.owner], sb);
			return false;
		}
	}
}