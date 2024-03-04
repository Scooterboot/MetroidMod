using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Boss
{
	public class NightmareLaserBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nightmare");
		}
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.scale = 1f;
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
			NPC Head = Main.npc[(int)Projectile.ai[0]];
			NPC Arm = Main.npc[(int)Projectile.ai[1]];

			if (Head != null && Head.active && Arm != null && Arm.active)
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
				if(Arm.type == ModContent.NPCType<NPCs.Nightmare.Nightmare_ArmFront>())
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
				
				Projectile.Center = laserPos;
				Projectile.velocity.X = Math.Sign(player.Center.X - Projectile.Center.X);
				Projectile.velocity.Y = 0f;
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - (float)(Math.PI/2);
				Projectile.localAI[1] = distance;
				
				if(delay <= 10)
				{
					delay++;
				}
				else if(charge < chargeMax)
				{
					charge++;
				}
				chargeFrame = (int)(3f * ((float)charge / (float)chargeMax));
				if(Projectile.localAI[0] == 1)
				{
					if(Projectile.timeLeft > 60)
					{
						Projectile.timeLeft = 60;
					}
					if(drawWidth > 0)
					{
						drawWidth--;
					}
					charge = chargeMax;
					chargeFrame = 5;
					for(int i = 0; i < 20; i++)
					{
						Vector2 pos = Projectile.Center + Projectile.velocity * Main.rand.Next((int)Projectile.localAI[1]);
						int num71 = Dust.NewDust(new Vector2(pos.X-7,pos.Y-7), 14, 14, 57, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num71].noGravity = true;
					}
				}
				DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
				Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], Projectile.width, DelegateMethods.CastLight);
			}
			else
				Projectile.Kill();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Color color45 = Color.White * ((float)charge / (float)chargeMax);
			Texture2D tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/Boss/NightmareLaserCharge").Value;
			int num108 = tex.Height / 7;
			int y4 = num108 * chargeFrame;
			Main.spriteBatch.Draw(tex, laserPos - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), color45, rot, new Vector2((float)tex.Width/2f, (float)num108/2f), Projectile.scale, SpriteEffects.None, 0f);
			
			if (Projectile.velocity == Vector2.Zero)
			{
				return false;
			}
			Texture2D texture2D22 = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			float num230 = Projectile.localAI[1];
			int width = texture2D22.Width-(drawWidth*2);
			
			Rectangle rectangle8 = new Rectangle(drawWidth, 0, width, 22);
			Main.spriteBatch.Draw(texture2D22, Projectile.Center.Floor() - Main.screenPosition, new Rectangle?(rectangle8), color45, Projectile.rotation, rectangle8.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
			
			num230 -= 33f * Projectile.scale;
			Vector2 value22 = Projectile.Center.Floor();
			value22 += Projectile.velocity * Projectile.scale * 10.5f;
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
					Main.spriteBatch.Draw(texture2D22, value22 - Main.screenPosition, new Rectangle?(rectangle8), color45, Projectile.rotation, new Vector2((float)(rectangle8.Width / 2), 0f), Projectile.scale, SpriteEffects.None, 0f);
					num231 += (float)rectangle8.Height * Projectile.scale;
					value22 += Projectile.velocity * (float)rectangle8.Height * Projectile.scale;
				}
			}

			rectangle8 = new Rectangle(drawWidth, 56, width, 22);
			Main.spriteBatch.Draw(texture2D22, value22 - Main.screenPosition - Projectile.velocity, new Rectangle?(rectangle8), color45, Projectile.rotation, texture2D22.Frame(1, 1, 0, 0).Top(), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		
		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if(Projectile.localAI[0] == 1)
			{
				float point = 0f;
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], Projectile.width, ref point);
			}
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)Projectile.localAI[0]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.localAI[0] = (float)reader.ReadDouble();
		}
	}
}
