using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.boss
{
	public class NightmareLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare");
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.timeLeft = 1200;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.width = 14;
			projectile.height = 14;
			projectile.scale = 1f;
		}

		Vector2 laserPos = Vector2.Zero;
		public override void AI()
		{
			NPC Head = Main.npc[(int)projectile.ai[0]];
			NPC Arm = Main.npc[(int)projectile.ai[1]];

			if(Head != null && Head.active && Arm != null && Arm.active)
			{
				laserPos = Arm.Center + new Vector2(17*Head.direction,15);
				if(Arm.ai[1] == 1)
				{
					laserPos = Arm.Center + new Vector2(17*Head.direction,16);
				}
				if(Arm.ai[1] == 2)
				{
					laserPos = Arm.Center + new Vector2(17*Head.direction,9);
				}
				if(Arm.type == mod.NPCType("Nightmare_ArmFront"))
				{
					laserPos = Arm.Center + new Vector2(13*Head.direction,17);
					if(Arm.ai[1] == 2)
					{
						laserPos = Arm.Center + new Vector2(19*Head.direction,17);
					}
					if(Arm.ai[1] == 3)
					{
						laserPos = Arm.Center + new Vector2(25*Head.direction,19);
					}
				}
				Player player = Main.player[Head.target];
				projectile.localAI[0]++;
				if(projectile.localAI[0] < 24)
				{
					float targetRot = (float)Math.Atan2(player.Center.Y - projectile.Center.Y, player.Center.X - projectile.Center.X);
					projectile.velocity = targetRot.ToRotationVector2() * 14;
					projectile.Center = laserPos;
				}
				else if(projectile.localAI[0] < 25)
				{
					Main.PlaySound(SoundID.Item12,projectile.Center);
				}
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			}
			else
			{
				projectile.Kill();
			}
		}

		bool drawFlag = false;
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			if(projectile.localAI[0] <= 28)
			{
				Texture2D tex = mod.GetTexture("Projectiles/boss/NightmareLaserCharge");
				int num108 = tex.Height / 7;
				int y4 = num108 * (int)(projectile.localAI[0] / 4f);
				sb.Draw(tex, laserPos - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), projectile.GetAlpha(Color.White), 0f, new Vector2((float)tex.Width/2f, (float)num108/2f), projectile.scale, SpriteEffects.None, 0f);
			}
			if(projectile.localAI[0] >= 24)
			{
				Texture2D tex = Main.projectileTexture[projectile.type];
				int num108 = tex.Height / Main.projFrames[projectile.type];
				int y4 = num108 * projectile.frame;
				
				float h = ((float)num108*projectile.scale);
				
				float dist = MathHelper.Clamp((Vector2.Distance(projectile.Center,laserPos)+((float)projectile.height/2f))/h,0f,1f);
				int height = (int)((float)num108*dist);
				if(dist >= 1f)
				{
					drawFlag = true;
				}
				if(drawFlag)
				{
					height = num108;
				}
				sb.Draw(tex, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, height)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2((float)tex.Width/2f, (float)num108/3f), projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}