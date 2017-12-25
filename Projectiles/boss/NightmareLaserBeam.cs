using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.boss
{
	public class NightmareLaserBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare");
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.timeLeft = 600;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.width = 14;
			projectile.height = 14;
			projectile.scale = 1f;
		}

		int delay = 0;
		int charge = 0;
		const int chargeMax = 180;
		int drawWidth = 6;
		float rot = 0f;
		int chargeFrame = 0;
		
		float distance = 3000f;
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
				
				rot += 0.125f*Head.direction;
				
				projectile.Center = laserPos;
				projectile.velocity.X = Math.Sign(player.Center.X - projectile.Center.X);
				projectile.velocity.Y = 0f;
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - (float)(Math.PI/2);
				projectile.localAI[1] = distance;
				
				if(delay <= 10)
				{
					delay++;
				}
				else if(charge < chargeMax)
				{
					charge++;
				}
				chargeFrame = (int)(3f * ((float)charge / (float)chargeMax));
				if(projectile.localAI[0] == 1)
				{
					if(projectile.timeLeft > 60)
					{
						projectile.timeLeft = 60;
					}
					if(drawWidth > 0)
					{
						drawWidth--;
					}
					charge = chargeMax;
					chargeFrame = 5;
					for(int i = 0; i < 20; i++)
					{
						Vector2 pos = projectile.Center + projectile.velocity * Main.rand.Next((int)projectile.localAI[1]);
						int num71 = Dust.NewDust(new Vector2(pos.X-7,pos.Y-7), 14, 14, 57, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num71].noGravity = true;
					}
				}
				DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
				Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], projectile.width, DelegateMethods.CastLight);
			}
			else
			{
				projectile.Kill();
			}
		}

		bool drawFlag = false;
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Color color45 = Color.White * ((float)charge / (float)chargeMax);
			Texture2D tex = mod.GetTexture("Projectiles/boss/NightmareLaserCharge");
			int num108 = tex.Height / 7;
			int y4 = num108 * chargeFrame;
			sb.Draw(tex, laserPos - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), color45, rot, new Vector2((float)tex.Width/2f, (float)num108/2f), projectile.scale, SpriteEffects.None, 0f);
			
			if (projectile.velocity == Vector2.Zero)
			{
				return false;
			}
			Texture2D texture2D22 = Main.projectileTexture[projectile.type];
			float num230 = projectile.localAI[1];
			int width = texture2D22.Width-(drawWidth*2);
			
			Rectangle rectangle8 = new Rectangle(drawWidth, 0, width, 22);
			sb.Draw(texture2D22, projectile.Center.Floor() - Main.screenPosition, new Rectangle?(rectangle8), color45, projectile.rotation, rectangle8.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
			
			num230 -= 33f * projectile.scale;
			Vector2 value22 = projectile.Center.Floor();
			value22 += projectile.velocity * projectile.scale * 10.5f;
			rectangle8 = new Rectangle(drawWidth, 25, width, 28);
			if (num230 > 0f)
			{
				float num231 = 0f;
				while (num231 + 1f < num230)
				{
					if (num230 - num231 < (float)rectangle8.Height)
					{
						rectangle8.Height = (int)(num230 - num231);
					}
					sb.Draw(texture2D22, value22 - Main.screenPosition, new Rectangle?(rectangle8), color45, projectile.rotation, new Vector2((float)(rectangle8.Width / 2), 0f), projectile.scale, SpriteEffects.None, 0f);
					num231 += (float)rectangle8.Height * projectile.scale;
					value22 += projectile.velocity * (float)rectangle8.Height * projectile.scale;
				}
			}
			
			rectangle8 = new Rectangle(drawWidth, 56, width, 22);
			sb.Draw(texture2D22, value22 - Main.screenPosition - projectile.velocity, new Rectangle?(rectangle8), color45, projectile.rotation, texture2D22.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		
		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if(projectile.localAI[0] == 1)
			{
				float point = 0f;
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], projectile.width, ref point);
			}
			return false;
		}
	}
}