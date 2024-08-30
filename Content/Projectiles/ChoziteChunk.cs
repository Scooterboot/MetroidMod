using System;
using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles
{
	public class ChoziteChunk : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 18;
			Projectile.aiStyle = 1;
			//Projectile.usesLocalNPCImmunity = true;
			//Projectile.localNPCHitCooldown = 1;
		}

		public override void AI()
		{
			if (Projectile.direction == -1)
				Projectile.spriteDirection= -1;

			float ballrotoffset = 0f;
			if (Projectile.velocity.Y != Vector2.Zero.Y)
			{
				if (Projectile.velocity.X != 0f)
					ballrotoffset += 0.05f * Projectile.velocity.X;
				else
					ballrotoffset += 0.25f * Projectile.direction;
			}
			else if (Projectile.velocity.X < 0f)
				ballrotoffset -= 0.2f;
			else if (Projectile.velocity.X > 0f)
				ballrotoffset += 0.2f;

			if (Projectile.velocity.X != 0f)
				ballrotoffset += 0.025f * Projectile.velocity.X;
			else
				ballrotoffset += 0.125f * Projectile.direction;
			Projectile.rotation += ballrotoffset;
			//Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 87, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void OnKill(int timeLeft)
		{
			mProjectile.Diffuse(Projectile, 87);
			SoundEngine.PlaySound(SoundID.Shatter, Projectile.position);
			mProjectile.Explode(10);
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height) && Projectile.Hitbox.Intersects(target.Hitbox))
			{
				return null;
			}
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
