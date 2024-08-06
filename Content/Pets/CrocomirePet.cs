using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Pets
{
	public class CrocomirePet : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 18;
			Main.projPet[Projectile.type] = true;

			// This code is needed to customize the vanity pet display in the player select screen. Quick explanation:
			// * It uses fluent API syntax, just like Recipe
			// * You start with ProjectileID.Sets.SimpleLoop, specifying the start and end frames as well as the speed, and optionally if it should animate from the end after reaching the end, effectively "bouncing"
			// * To stop the animation if the player is not highlighted/is standing, as done by most grounded pets, add a .WhenNotSelected(0, 0) (you can customize it just like SimpleLoop)
			// * To set offset and direction, use .WithOffset(x, y) and .WithSpriteDirection(-1)
			// * To further customize the behavior and animation of the pet (as its AI does not run), you have access to a few vanilla presets in DelegateMethods.CharacterPreview to use via .WithCode(). You can also make your own, showcased in MinionBossPetProjectile
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 12)
				.WithOffset(-10f, -10f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.BabyDino); // Copy the stats of the Zephyr Fish
			Projectile.height = 32;
			
			AIType = ProjectileID.BerniePet; // Mimic as the Zephyr Fish during AI.
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Vector2 drawOrigin = new Vector2(tex.Width * 0.5f, (tex.Height / Main.projFrames[Projectile.type]) * 0.5f);
			Color color = lightColor;
			Rectangle rect = new Rectangle(0, Projectile.frame * tex.Height / Main.projFrames[Projectile.type], tex.Width, tex.Height / Main.projFrames[Projectile.type]);
			Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle?(rect), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);

			Texture2D eyelidTex = ModContent.Request<Texture2D>($"{Texture}_Eyelids").Value;
			int frameX = 0;
			int frameY = 0;
			Vector2 off = new Vector2(0, (Projectile.frame >= 3 && Projectile.frame <= 5) ? -2 : 0);
			if (Projectile.localAI[0] < 12)
				frameX = Math.Min((int)Projectile.localAI[0], 5);
			else if (Projectile.localAI[0] < 18)
				frameX = (17 - (int)Projectile.localAI[0]);
			if (Projectile.frame == 11)
				frameY = 1;
			if (Projectile.frame > 11)
				frameY = 2;
			Rectangle eyeRect = new Rectangle(frameX * eyelidTex.Width / 6, frameY * eyelidTex.Height / 3, eyelidTex.Width / 6, eyelidTex.Height / 3);
			Main.EntitySpriteDraw(eyelidTex, Projectile.Center - Main.screenPosition + off, new Rectangle?(eyeRect), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);

			return false;
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];

			player.petFlagBerniePet = false; // Relic from AIType
			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			Projectile.spriteDirection = Projectile.direction;
			// Keep the projectile from disappearing as long as the player isn't dead and has the pet buff.
			if (!player.dead && player.HasBuff(ModContent.BuffType<CrocomirePetBuff>())) {
				Projectile.timeLeft = 2;
			}
			Projectile.localAI[0]++;
			if (Projectile.localAI[0] > 275)
				Projectile.localAI[0] = 0;
			if (Projectile.velocity.X == 0)
			{
				Projectile.localAI[1]++;
				Projectile.localAI[2] = 0;
				if (Projectile.localAI[1] < 192)
				{
					int f = (int)(Projectile.localAI[1] / 12) % 4;
					Projectile.frame = (f == 3) ? 1 : f;
				}
				else
				{
					int t = (int)(Projectile.localAI[1] - 192) / 6;
					switch (t)
					{
						case 0:
						case 1:
						case 2:
						case 3:
						case 4:
						case 5:
							Projectile.frame = 11 + t;
							break;
						case 6:
							Projectile.frame = 13;
							break;
						case 7:
							Projectile.frame = 14;
							break;
						case 8:
							Projectile.frame = 17;
							break;
						case 9:
							Projectile.frame = 11;
							break;
						default:
							Projectile.frame = 0;
							Projectile.localAI[1] = 0;
							break;
					}
				}
			}
			else
			{
				Projectile.localAI[1] = 0;
				Projectile.localAI[2]++;
				if ((int)Projectile.localAI[2] % 6 == 0)
					Projectile.frame++;
				if(Projectile.frame > 10)
				{
					Projectile.frame = 3;
				}
			}
		}
	}
}
