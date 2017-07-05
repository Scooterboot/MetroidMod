using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Torizo
{
	public class TorizoBeam : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo");
		}
		public override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 36;
			projectile.scale = 1.5f;
			projectile.aiStyle = 1;
			projectile.hostile = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 600;
			projectile.tileCollide = false;
			projectile.light = 0.3f;
			aiType = ProjectileID.Bullet;
		}
	}
}

