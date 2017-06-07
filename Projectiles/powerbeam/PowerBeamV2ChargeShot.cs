using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.powerbeam
{
	public class PowerBeamV2ChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Beam V2 Charge Shot");
			Main.projFrames[projectile.type] = 3;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
		}

		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64, 0, 0, 100, default(Color), projectile.scale);
			Main.dust[dust].noGravity = true;
			
			Projectile P = projectile;
			P.frame = 0;
			for(int i = 0; i < Main.projectile.Length; i++)
			{
				if(Main.projectile[i].active && Main.projectile[i].owner == P.owner && Main.projectile[i].type == P.type && Main.projectile[i].whoAmI != P.whoAmI)
				{
					Projectile P2 = Main.projectile[i];
					if(Vector2.Distance(P.position, P2.position) <= 32f)
					{
						if(P2.ai[0] == 1f && P.ai[0] == -1f)
						{
							P.frame = 1;
						}
						else if(P2.ai[0] == -1f && P.ai[0] == 1f)
						{
							P.frame = 2;
						}
						/*Vector2 vect = P.position - P2.position;
						Vector2 pos = P.position - (vect/2);
						Color color = new Color();
						if(Main.rand.Next(4) < 1)
						{
							int dust = Dust.NewDust(new Vector2(pos.X+(P.width/4),pos.Y+(P.height/4)), 1, 1, 64, 0, 0, 100, color, 3f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 0f;
						}*/
					}
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.Diffuse(projectile, 64, default(Color), true, 2f);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.DrawCenteredTrail(projectile, sb, 5, 1f);
			return false;
		}
	}
}