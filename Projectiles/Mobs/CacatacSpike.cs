using System;
using Microsoft.Xna.Framework;

using Terraria;
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

            projectile.numHits = 1;           
        }

        public override bool PreAI()
        {
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
            projectile.velocity.Y += 0.04F;
            projectile.velocity.X *= 0.98F;

            return false;
        }
    }
}
