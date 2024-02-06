using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using System.IO;

namespace MetroidMod.Content.Projectiles.Boss
{
	public class NightmareGravityField : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gravity Field");
		}
		public override void SetDefaults()
		{
			Projectile.scale = 0f;
			Projectile.width = 20;
			Projectile.height = 20;

			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 1200;

			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}

		int num = 1;
		readonly float fieldRadius = 3000f;

		bool initialized = false;

		public override bool PreAI()
		{
			if (!initialized)
			{
				SoundEngine.PlaySound(Sounds.NPCs.Nightmare_GravityField_Activate, Projectile.Center);

				for (int num70 = 0; num70 < 15; num70++)
				{
					int num71 = Dust.NewDust(Projectile.Center - new Vector2(18), 36, 36, 54, 0f, 0f, 100, default(Color), 1f + Main.rand.Next(3));
					Main.dust[num71].noGravity = true;
				}

				initialized = true;
			}
			return (true);
		}

		public override void AI()
		{
			NPC Head = Main.npc[(int)Projectile.ai[0]];
			NPC Tail = Main.npc[(int)Projectile.ai[1]];
			
			if(Projectile.localAI[0] == 1)
			{
				Projectile.localAI[1]++;
				if(Projectile.localAI[1] == 20)
				{
					SoundEngine.PlaySound(Sounds.NPCs.Nightmare_GravityField_Deactivate, Projectile.Center);
				}
				num = -1;
				if(Projectile.scale <= 0f)
				{
					Projectile.Kill();
					return;
				}
			}
			
			if(Tail == null || !Tail.active || Head == null || !Head.active)
				Projectile.localAI[0] = 1;
			else
			{
			
				Player player = Main.player[Head.target];
				
				if(Vector2.Distance(Projectile.Center,player.Center) < fieldRadius * Projectile.scale)
					player.AddBuff(ModContent.BuffType<Buffs.GravityDebuff>(), 1);
				
				Projectile.Center = new Vector2(Tail.Center.X + 26 * Head.direction, Tail.Center.Y + 14);
				Projectile.spriteDirection = Head.direction;
			}
			
			Projectile.position.X += Projectile.width / 2f;
			Projectile.position.Y += Projectile.height / 2f;
			Projectile.scale = MathHelper.Clamp(Projectile.scale + 0.025f*num,0f,1f);
			Projectile.width = (int)(20 * Projectile.scale);
			Projectile.height = (int)(20 * Projectile.scale);
			Projectile.position.X -= Projectile.width / 2f;
			Projectile.position.Y -= Projectile.height / 2f;
			
			Projectile.timeLeft = 10;
			Projectile.rotation += 0.25f;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Color color25 = Lighting.GetColor((int)((double)Projectile.position.X + (double)Projectile.width * 0.5) / 16, (int)(((double)Projectile.position.Y + (double)Projectile.height * 0.5) / 16.0));
			
			Vector2 vector60 = Projectile.position + new Vector2((float)Projectile.width, (float)Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D texture2D31 = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Color alpha4 = Projectile.GetAlpha(color25);
			Vector2 origin8 = new Vector2((float)texture2D31.Width, (float)texture2D31.Height) / 2f;
			
			Color color57 = alpha4 * 0.8f;
			color57.A /= 2;
			Color color58 = Color.Lerp(alpha4, Color.Black, 0.5f);
			color58.A = alpha4.A;
			float num279 = 0.95f + (Projectile.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
			color58 *= num279;
			float scale13 = 0.6f + Projectile.scale * 0.6f * num279;
			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Extra[50].Value, vector60, null, color58, -Projectile.rotation + 0.35f, origin8, scale13, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Extra[50].Value, vector60, null, alpha4, -Projectile.rotation, origin8, Projectile.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			Main.spriteBatch.Draw(texture2D31, vector60, null, color57, -Projectile.rotation * 0.7f, origin8, Projectile.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Extra[50].Value, vector60, null, alpha4 * 0.8f, Projectile.rotation * 0.5f, origin8, Projectile.scale * 0.9f, spriteEffects, 0f);
			Main.spriteBatch.Draw(texture2D31, vector60, null, alpha4, Projectile.rotation, origin8, Projectile.scale, spriteEffects, 0f);
			
			float size = fieldRadius * 2.4f;
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			
			DrawData value9 = new DrawData(ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/Boss/NightmareGravityField_Blank").Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)size, (int)size)), new Color(160,128,160), 0f, new Vector2(size / 2, size / 2), new Vector2(1.5f,1f) * Projectile.scale, spriteEffects, 0);
			GameShaders.Misc["ForceField"].UseColor(new Vector3(1f));
			GameShaders.Misc["ForceField"].Apply(new DrawData?(value9));
			value9.Draw(Main.spriteBatch);
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			
			if(Projectile.localAI[0] != 1)
			{
				Filters.Scene.Activate("HeatDistortion", Projectile.Center, new object[0]);
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
