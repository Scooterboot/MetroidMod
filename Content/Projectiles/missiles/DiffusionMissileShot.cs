using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace MetroidMod.Content.Projectiles.missiles
{
	public class DiffusionMissileShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Diffusion Missile Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.scale = 2.25f;
			Projectile.velocity *= 0.25f;
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
			if (Projectile.ai[0] > (5f+(float)Projectile.extraUpdates) && Projectile.extraUpdates < 5)
			{
				Projectile.extraUpdates++;
				Projectile.ai[0] = 0f;
			}
		}
		public override void OnKill(int timeLeft)
		{
			Projectile P = Projectile;
			P.position.X = P.position.X + (float)(P.width / 2);
			P.position.Y = P.position.Y + (float)(P.height / 2);
			P.width += 32;
			P.height += 32;
			P.position.X = P.position.X - (float)(P.width / 2);
			P.position.Y = P.position.Y - (float)(P.height / 2);

			//SoundEngine.PlaySound(SoundID.Item14,P.position);
			
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
			
			int difType = ModContent.ProjectileType<DiffusionShot>();
			int num = 4;
			if(P.Name.Contains("Ice"))
			{
				difType = ModContent.ProjectileType<IceDiffusionShot>();
			}
			if(P.Name.Contains("Stardust"))
			{
				difType = ModContent.ProjectileType<StardustDiffusionShot>();
				num = 6;
			}
			if(P.Name.Contains("Nebula"))
			{
				difType = ModContent.ProjectileType<NebulaDiffusionShot>();
				num = 5;
			}
			var entitySource = Projectile.GetSource_FromAI();
			for (int i = 0; i < num; i++)
			{
				float angle = ((float)(Math.PI*2)/num)*i;
				int proj = Projectile.NewProjectile(entitySource,P.Center.X,P.Center.Y,0f,0f,difType,P.damage,P.knockBack,P.owner);
				DiffusionShot difShot = (DiffusionShot)Main.projectile[proj].ModProjectile;
				difShot.spin = angle;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
	public class IceDiffusionMissileShot : DiffusionMissileShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Diffusion Missile Shot");
		}
	}
	public class StardustDiffusionMissileShot : DiffusionMissileShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Diffusion Missile Shot");
		}
	}
	public class NebulaDiffusionMissileShot : DiffusionMissileShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Diffusion Missile Shot");
		}
	}
}
