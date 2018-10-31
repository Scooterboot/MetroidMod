using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.boss
{
	public class OmegaPirateGrenade : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omega Pirate");
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.timeLeft = 600;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.ignoreWater = true;
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 1f;
		}
		
		public override void AI()
		{
			projectile.rotation += 0.5f;
			projectile.localAI[0]++;
			if(projectile.localAI[0] > 20)
			{
				projectile.velocity.Y += 0.25f;
			}
			
			for(int i = 0; i < 3; i++)
			{
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 68, 0f, 0f, 100, Color.White, 1+(i/2));
				Main.dust[dust2].noGravity = true;
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			projectile.penetrate--;
		}
		public override void Kill(int timeLeft)
		{
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width += 48;
			projectile.height += 48;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

			//Main.PlaySound(2,(int)projectile.position.X,(int)projectile.position.Y,14);
			Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/ElitePirate_GrenadeExplodeSound"));
			
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 68, 0f, 0f, 100, default(Color), 3f);
				Main.dust[num72].noGravity = true;
			}
			projectile.Damage();
		}
	}
}