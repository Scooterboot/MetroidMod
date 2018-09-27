using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.Mobs
{
    public class EvirProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Evir Spike");
        }
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 12;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.scale = 1.5F;
            
            projectile.numHits = 1;
        }

        public override bool PreAI()
        {
            if (projectile.velocity != Vector2.Zero)
                projectile.rotation += .2F * projectile.direction;

            if (!Main.npc[(int)projectile.ai[0]].active)
                projectile.Kill();

            return false;
        }
    }
}
