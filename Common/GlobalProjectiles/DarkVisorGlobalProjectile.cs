using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Common.GlobalProjectiles
{
	internal class DarkVisorGlobalProjectile : GlobalProjectile
	{
		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
			lightColor = new Color(255, 0, 0);
			return true;
		}
	}
}
