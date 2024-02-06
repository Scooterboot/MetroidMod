using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Mobs
{
	public class EvirProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Evir Spike");
		}
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 12;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.scale = 1.5F;
			
			Projectile.numHits = 1;
		}

		public override bool PreAI()
		{
			if (Projectile.velocity != Vector2.Zero)
				Projectile.rotation += .2F * Projectile.direction;

			if (!Main.npc[(int)Projectile.ai[0]].active)
				Projectile.Kill();

			return false;
		}
	}
}
