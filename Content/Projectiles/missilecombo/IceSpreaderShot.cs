using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.missilecombo
{
	public class IceSpreaderShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Spreader Shot");
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
			
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 3, 135, 2f);
		}
		public override void OnKill(int timeLeft)
		{
			Projectile P = Projectile;
			
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(P.position, P.width, P.height, 135, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
			}

			var entitySource = Projectile.GetSource_Death();
			for (int i = 0; i < 360; i += 10)
			{
				float rot = (float)Angle.ConvertToRadians(i);
				int num54 = Projectile.NewProjectile(entitySource, P.Center.X, P.Center.Y, 0f, 0f, ModContent.ProjectileType<IceSpreaderDiffusionShot>(),P.damage,P.knockBack,P.owner);
				IceSpreaderDiffusionShot difShot = (IceSpreaderDiffusionShot)Main.projectile[num54].ModProjectile;
				difShot.spin = rot;
			}
			
			int x = (int)MathHelper.Clamp(P.Center.X / 16,0,Main.maxTilesX-2);
			int y = (int)MathHelper.Clamp(P.Center.Y / 16,0,Main.maxTilesY-2);
			Vector2 pos = new Vector2((float)x*16f + 8f,(float)y*16f + 8f);
			int ft = Projectile.NewProjectile(entitySource, pos.X, pos.Y, 0f, 0f, ModContent.ProjectileType<IceSpreaderFrozenTerrain>(),0,0f,P.owner);
			
			Terraria.Audio.SoundEngine.PlaySound(Sounds.Items.Weapons.IceSpreaderImpactSound, P.Center);
		}
		
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{	
			target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(),300,true);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
}
