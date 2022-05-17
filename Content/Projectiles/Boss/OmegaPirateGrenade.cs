using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Projectiles.Boss
{
	public class OmegaPirateGrenade : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omega Pirate");
		}
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 1f;
		}
		
		public override void AI()
		{
			Projectile.rotation += 0.5f;
			Projectile.localAI[0]++;
			if(Projectile.localAI[0] > 20)
			{
				Projectile.velocity.Y += 0.25f;
			}
			
			for(int i = 0; i < 3; i++)
			{
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, 0f, 0f, 100, Color.White, 1+(i/2));
				Main.dust[dust2].noGravity = true;
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			Projectile.penetrate--;
		}
		public override void Kill(int timeLeft)
		{
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width += 48;
			Projectile.height += 48;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);

			//Main.PlaySound(2,(int)Projectile.position.X,(int)Projectile.position.Y,14);
			SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)Projectile.position.X, (int)Projectile.position.Y, SoundLoader.GetSoundSlot(Mod, "Assets/Sounds/ElitePirate_GrenadeExplodeSound"));
			
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, 0f, 0f, 100, default(Color), 3f);
				Main.dust[num72].noGravity = true;
			}
			Projectile.Damage();
		}
	}
}
