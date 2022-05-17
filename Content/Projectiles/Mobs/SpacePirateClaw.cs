using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Projectiles.Mobs
{
    public class SpacePirateClaw : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12; Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.friendly = false;

            Projectile.numHits = 2;
        }

        public override bool PreAI()
        {
            Projectile.rotation += .35F * Projectile.direction;

            Projectile.velocity.X *= .98F;
            Projectile.velocity.Y += .1F;
            return false;
        }
    }
}
