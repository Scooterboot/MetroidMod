using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace MetroidMod.Content.Projectiles.missiles
{
	public class SuperMissileShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Super Missile Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.scale = 2f;
			Projectile.extraUpdates = 0;
			Projectile.timeLeft = 2000;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			
			int dustType = 6;
			float scale = 2f;
			if(Projectile.Name.Contains("Ice"))
			{
				dustType = 135;
			}
			if(Projectile.Name.Contains("Stardust") || Projectile.Name.Contains("Nebula"))
			{
				dustType = 87;
				scale = 1f;
				int dustType2 = 88;
				if(Projectile.Name.Contains("Nebula"))
				{
					dustType = 255;
					scale = 1.5f;
					dustType2 = 240;//254;
				}
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType2, 0, 0, 100, default(Color), 2f);
				Main.dust[dust].noGravity = true;
			}
			mProjectile.DustLine(Projectile.Center-Projectile.velocity*0.5f, Projectile.velocity, Projectile.rotation, 5, 3, dustType, scale);
			
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] > (5f+(float)Projectile.extraUpdates) && Projectile.extraUpdates < 10)
			{
				Projectile.extraUpdates++;
				Projectile.ai[0] = 0f;
			}
			
			if(mProjectile.seeking && mProjectile.seekTarget > -1)
			{
				float num236 = Projectile.position.X;
				float num237 = Projectile.position.Y;
				bool flag5 = false;
				Projectile.ai[1] += 1f;
				if(Projectile.ai[1] > 5f && (Projectile.numUpdates <= 0 || (Projectile.numUpdates <= 1 && (Projectile.Name.Contains("Stardust") || Projectile.Name.Contains("Nebula")))))
				{
					Projectile.ai[1] = 5f;
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
			Projectile P = Projectile;
			int size = 80;
			if(P.Name.Contains("Stardust"))
			{
				size = 100;
			}
			P.position.X = P.position.X + (float)(P.width / 2);
			P.position.Y = P.position.Y + (float)(P.height / 2);
			P.width += size;
			P.height += size;
			P.position.X = P.position.X - (float)(P.width / 2);
			P.position.Y = P.position.Y - (float)(P.height / 2);

			//SoundEngine.PlaySound(SoundID.Item14,P.position);

			if (mProjectile.homing)
			{
				mProjectile.HomingBehavior(Projectile);
				SoundEngine.PlaySound(Sounds.Items.Weapons.MissileExplodeHunters, Projectile.position);
			}
			int dustType = 6;
			int dustType2 = 30;
			float scale = 1f;
			if(P.Name.Contains("Ice"))
			{
				dustType = 135;
				SoundEngine.PlaySound(Sounds.Items.Weapons.IceMissileExplode,Projectile.position);
			}
			else
			{
				SoundEngine.PlaySound(Sounds.Items.Weapons.SuperMissileExplode,Projectile.position);
			}
			if(P.Name.Contains("Stardust"))
			{
				dustType = 88;
				dustType2 = 87;
				scale = 0.6f;
			}
			if(P.Name.Contains("Nebula"))
			{
				dustType = 255;
				dustType2 = 240;
				scale = 0.75f;
			}
			for (int num70 = 0; num70 < 25f*(2f-scale); num70++)
			{
				int num71 = Dust.NewDust(P.position, P.width, P.height, dustType, 0f, 0f, 100, default(Color), 5f*scale);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(P.position, P.width, P.height, dustType2, 0f, 0f, 100, default(Color), 3f*scale);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
			}
			P.Damage();
			
			if(P.Name.Contains("Nebula"))
			{
				var entitySource = P.GetSource_Death();
				int n = Projectile.NewProjectile(entitySource, P.Center.X, P.Center.Y, 0f, 0f, ModContent.ProjectileType<NebulaMissileImpact>(),P.damage,P.knockBack,P.owner);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
	public class IceSuperMissileShot : SuperMissileShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Super Missile Shot");
		}
	}
	public class StardustMissileShot : SuperMissileShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Missile Shot");
		}
	}
	public class NebulaMissileShot : SuperMissileShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Missile Shot");
		}
	}
}
