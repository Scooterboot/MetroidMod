using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using MetroidMod.NPCs.OmegaPirate;

namespace MetroidMod.Projectiles.boss
{
	public class OmegaPirateLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omega Pirate");
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.timeLeft = 1200;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.width = 20;
			projectile.height = 20;
			projectile.scale = 1f;
			projectile.extraUpdates = 100;
		}
		
		public override void AI()
		{
			Color dustColor = Color.Lerp(OmegaPirate.minGlowColor,OmegaPirate.maxGlowColor,projectile.localAI[0]);
			for(int i = 0; i < 3; i++)
			{
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 63, 0f, 0f, 100, dustColor, 5f);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].noLight = true;
				Main.dust[dust2].velocity *= 1.4f;
			}
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			return false;
		}
	}
}