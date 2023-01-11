using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.missiles
{
	public class HomingMissileShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Homing Missile Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 1.5f;
			Projectile.timeLeft = 1000;
			Projectile.aiStyle = 0;
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            int accuracy = 240;
            int dustType = 6;
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 3, dustType, 2f);
			mProjectile.HomingBehavior(Projectile);
		}

		public override void Kill(int timeLeft)
		{
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width += 48;
			Projectile.height += 48;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);

			//Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item14,Projectile.position);
			SoundEngine.PlaySound(Sounds.Items.Weapons.MissileExplode, Projectile.position);
			int dustType = 6;
			if (Projectile.Name.Contains("Ice"))
			{
				dustType = 135;
			}
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 30, 0f, 0f, 100, default(Color), 3f);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
			}
			SoundEngine.PlaySound(Sounds.Items.Weapons.MissileExplodeHunters, Projectile.position);
			Projectile.Damage();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);

			writer.Write(mProjectile.seeking);
			writer.Write(mProjectile.seekTarget);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);

			mProjectile.seeking = reader.ReadBoolean();
			mProjectile.seekTarget = reader.ReadInt32();
		}
	}
}
