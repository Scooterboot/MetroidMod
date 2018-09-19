using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.Mobs
{
    public class CacatacSpike : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 4;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.scale = 1.5F;

            projectile.numHits = 1;
            projectile.timeLeft = 60;
        }

        public override bool PreAI()
        {
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);

            return false;
        }

        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 8; ++i)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GrassBlades, projectile.velocity.X, projectile.velocity.Y);
            }
        }
    }
}
