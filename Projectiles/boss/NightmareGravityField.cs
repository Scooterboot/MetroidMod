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
using System.IO;

namespace MetroidMod.Projectiles.boss
{
	public class NightmareGravityField : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Field");
		}
		public override void SetDefaults()
		{
			projectile.scale = 0f;
			projectile.width = 20;
			projectile.height = 20;

			projectile.aiStyle = -1;
			projectile.penetrate = -1;
			projectile.timeLeft = 1200;

			projectile.hostile = true;
			projectile.friendly = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
		}

		int num = 1;
		readonly float fieldRadius = 3000f;

		bool initialized = false;

		public override bool PreAI()
		{
			if (!initialized)
			{
				Main.PlaySound(SoundLoader.customSoundType, projectile.Center, mod.GetSoundSlot(SoundType.Custom, "Sounds/Nightmare_GravityField_Activate"));

				for (int num70 = 0; num70 < 15; num70++)
				{
					int num71 = Dust.NewDust(projectile.Center - new Vector2(18), 36, 36, 54, 0f, 0f, 100, default(Color), 1f + Main.rand.Next(3));
					Main.dust[num71].noGravity = true;
				}

				initialized = true;
			}
			return (true);
		}

		public override void AI()
		{
			NPC Head = Main.npc[(int)projectile.ai[0]];
			NPC Tail = Main.npc[(int)projectile.ai[1]];
			
			if(projectile.localAI[0] == 1)
			{
				projectile.localAI[1]++;
				if(projectile.localAI[1] == 20)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Nightmare_GravityField_Deactivate"));
				}
				num = -1;
				if(projectile.scale <= 0f)
				{
					projectile.Kill();
					return;
				}
			}
			
			if(Tail == null || !Tail.active || Head == null || !Head.active)
				projectile.localAI[0] = 1;
			else
			{
			
				Player player = Main.player[Head.target];
				
				if(Vector2.Distance(projectile.Center,player.Center) < fieldRadius * projectile.scale)
					player.AddBuff(mod.BuffType("GravityDebuff"), 1);
				
				projectile.Center = new Vector2(Tail.Center.X + 26 * Head.direction, Tail.Center.Y + 14);
				projectile.spriteDirection = Head.direction;
			}
			
			projectile.position.X += projectile.width / 2f;
			projectile.position.Y += projectile.height / 2f;
			projectile.scale = MathHelper.Clamp(projectile.scale + 0.025f*num,0f,1f);
			projectile.width = (int)(20 * projectile.scale);
			projectile.height = (int)(20 * projectile.scale);
			projectile.position.X -= projectile.width / 2f;
			projectile.position.Y -= projectile.height / 2f;
			
			projectile.timeLeft = 10;
			projectile.rotation += 0.25f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Color color25 = Lighting.GetColor((int)((double)projectile.position.X + (double)projectile.width * 0.5) / 16, (int)(((double)projectile.position.Y + (double)projectile.height * 0.5) / 16.0));
			
			Vector2 vector60 = projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			Texture2D texture2D31 = Main.projectileTexture[projectile.type];
			Color alpha4 = projectile.GetAlpha(color25);
			Vector2 origin8 = new Vector2((float)texture2D31.Width, (float)texture2D31.Height) / 2f;
			
			Color color57 = alpha4 * 0.8f;
			color57.A /= 2;
			Color color58 = Color.Lerp(alpha4, Color.Black, 0.5f);
			color58.A = alpha4.A;
			float num279 = 0.95f + (projectile.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
			color58 *= num279;
			float scale13 = 0.6f + projectile.scale * 0.6f * num279;
			spriteBatch.Draw(Main.extraTexture[50], vector60, null, color58, -projectile.rotation + 0.35f, origin8, scale13, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			spriteBatch.Draw(Main.extraTexture[50], vector60, null, alpha4, -projectile.rotation, origin8, projectile.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			spriteBatch.Draw(texture2D31, vector60, null, color57, -projectile.rotation * 0.7f, origin8, projectile.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			spriteBatch.Draw(Main.extraTexture[50], vector60, null, alpha4 * 0.8f, projectile.rotation * 0.5f, origin8, projectile.scale * 0.9f, spriteEffects, 0f);
			spriteBatch.Draw(texture2D31, vector60, null, alpha4, projectile.rotation, origin8, projectile.scale, spriteEffects, 0f);
			
			float size = fieldRadius * 2.4f;
			
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			
			DrawData value9 = new DrawData(mod.GetTexture("Projectiles/boss/NightmareGravityField_Blank"), projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)size, (int)size)), new Color(160,128,160), 0f, new Vector2(size / 2, size / 2), new Vector2(1.5f,1f) * projectile.scale, spriteEffects, 0);
			GameShaders.Misc["ForceField"].UseColor(new Vector3(1f));
			GameShaders.Misc["ForceField"].Apply(new DrawData?(value9));
			value9.Draw(spriteBatch);
			
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			
			if(projectile.localAI[0] != 1)
			{
				Filters.Scene.Activate("HeatDistortion", projectile.Center, new object[0]);
			}
			
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)projectile.localAI[0]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.localAI[0] = (float)reader.ReadDouble();
		}
	}
}