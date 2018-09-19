using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.Mobs
{
    public class MetareeRock : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 6;
            projectile.scale = 2;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.tileCollide = false;

            projectile.numHits = 1;
        }

        public override bool PreAI()
        {
            if (projectile.ai[0]++ >= 20) // Hacky, god help me.
                projectile.tileCollide = true;

            projectile.rotation += (Math.Abs(projectile.velocity.Y) + Math.Abs(projectile.velocity.X)) * (projectile.velocity.X > 0f ? 0.02F : -0.02F);
            projectile.velocity.Y += 0.2F;
            projectile.velocity.X *= 0.98F;

            return false;
        }

        public override void Kill(int timeLeft)
        {
            int num4;
            for (int num471 = 0; num471 < 30; num471 = num4 + 1)
            {
                int num472 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 1);
                if (Main.rand.Next(2) == 0)
                {
                    Dust dust = Main.dust[num472];
                    dust.scale *= 1.4f;
                }
                num4 = num471;
            }
        }
    }
}
