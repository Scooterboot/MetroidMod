using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.missiles
{
	public class DiffusionShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diffusion Shot");
		}
		
		bool initialised = false;
		float radius = 0.0f;
		public float spin = 0.0f;
		float SpinIncrease = 0.05f;
		Vector2 basePosition = new Vector2(0f,0f);
		
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.extraUpdates = 0;
			projectile.width = 32;
			projectile.height = 32;
			projectile.scale = 2f;
			projectile.timeLeft = 175;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			Main.projFrames[projectile.type] = 5;
		}

		public void initialise()
		{
			basePosition = projectile.Center;
			initialised = true;
		}
		public override void AI()
		{
			if(!initialised)
			{
				initialise();
			}
			SpinIncrease += 0.001f;
			radius += 2.0f;
			spin += SpinIncrease;
			projectile.rotation = 0f;
			projectile.position = (basePosition - new Vector2(projectile.width/2,projectile.height/2)) + spin.ToRotationVector2()*radius;

			projectile.frameCounter++;
			int frame = 2;
			if(projectile.frameCounter < frame)
			{
				projectile.frame = 0;
			}
			else if(projectile.frameCounter < frame * 2)
			{
				projectile.frame = 1;
			}
			else if(projectile.frameCounter < frame * 3)
			{
				projectile.frame = 2;
			}
			else if(projectile.frameCounter < frame * 4)
			{
				projectile.frame = 3;
			}
			else if(projectile.frameCounter < frame * 5)
			{
				projectile.frame = 4;
			}
			else if(projectile.frameCounter < frame * 6)
			{
				projectile.frame = 3;
			}
			else if(projectile.frameCounter < frame * 7)
			{
				projectile.frame = 2;
			}
			else if(projectile.frameCounter < frame * 8 - 1)
			{
				projectile.frame = 1;
			}
			else
			{
				projectile.frame = 1;
				projectile.frameCounter = 0;
			}
			int dustType = 6;
			Color color = MetroidMod.plaRedColor;
			if(projectile.Name.Contains("Ice"))
			{
				dustType = 135;
				color = MetroidMod.iceColor;
			}
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, default(Color), 2.5f);
			Main.dust[dust].noGravity = true;
		}
		
		public override void Kill(int timeLeft)
		{
			int dustType = 6;
			if(projectile.Name.Contains("Ice"))
			{
				dustType = 135;
			}
			for(int i = 0; i < projectile.oldPos.Length; i++)
			{
				for (int num70 = 0; num70 < 5; num70++)
				{
					int num71 = Dust.NewDust(projectile.oldPos[i], projectile.width, projectile.height, dustType, 0f, 0f, 100, default(Color), 4f);
					Main.dust[num71].noGravity = true;
				}
			}
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.DrawCentered(projectile, sb);
			return false;
		}
	}
	public class IceDiffusionShot : DiffusionShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Diffusion Shot");
		}
	}
}