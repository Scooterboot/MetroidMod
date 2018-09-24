using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.Mobs
{
    public class SpacePirateClaw : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12; projectile.height = 12;
            projectile.hostile = true;
            projectile.friendly = false;

            projectile.numHits = 2;
        }

        public override bool PreAI()
        {
            projectile.rotation += .35F * projectile.direction;

            projectile.velocity.X *= .98F;
            projectile.velocity.Y += .1F;
            return false;
        }
    }
}
