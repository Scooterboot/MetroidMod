using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.missiles
{
	public class MissileShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.timeLeft = 1000;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			
			int dustType = 6;
			if(Projectile.Name.Contains("Ice"))
			{
				dustType = 135;
			}
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 3, dustType, 2f);
			
			if(mProjectile.seeking && mProjectile.seekTarget > -1)
			{
				float num236 = Projectile.position.X;
				float num237 = Projectile.position.Y;
				bool flag5 = false;
				Projectile.ai[0] += 1f;
				if (Projectile.ai[0] > 5f && Projectile.numUpdates <= 0)
				{
					Projectile.ai[0] = 5f;
					int num239 = mProjectile.seekTarget;
					if(Main.npc[num239].active)
					{
						num236 = Main.npc[num239].position.X + (float)(Main.npc[num239].width / 2);
						num237 = Main.npc[num239].position.Y + (float)(Main.npc[num239].height / 2);
						flag5 = true;
					}
					else
					{
						mProjectile.seekTarget = -1;
					}
				}
				if (!flag5)
				{
					num236 = Projectile.position.X + (float)(Projectile.width / 2) + Projectile.velocity.X * 100f;
					num237 = Projectile.position.Y + (float)(Projectile.height / 2) + Projectile.velocity.Y * 100f;
				}
				float num243 = 8f;
				Vector2 vector22 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num244 = num236 - vector22.X;
				float num245 = num237 - vector22.Y;
				float num246 = (float)Math.Sqrt((double)(num244 * num244 + num245 * num245));
				num246 = num243 / num246;
				num244 *= num246;
				num245 *= num246;
				Projectile.velocity.X = (Projectile.velocity.X * 11f + num244) / 12f;
				Projectile.velocity.Y = (Projectile.velocity.Y * 11f + num245) / 12f;
			}
			if (mProjectile.homing)
			{
				mProjectile.HomingBehavior(Projectile);
			}
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
			SoundEngine.PlaySound(Sounds.Items.Weapons.MissileExplode,Projectile.position);
			if (mProjectile.homing)
			{
				mProjectile.HomingBehavior(Projectile);
				SoundEngine.PlaySound(Sounds.Items.Weapons.MissileExplodeHunters, Projectile.position);
			}
			int dustType = 6;
			if(Projectile.Name.Contains("Ice"))
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
	public class IceMissileShot : MissileShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Missile Shot");
		}
	}
}
