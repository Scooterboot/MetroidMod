using System;
using MetroidMod.Content.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Common.GlobalProjectiles
{
	internal class BeamEffectGlobalProjectile : GlobalProjectile
	{
		public override void SetDefaults(Projectile entity)
		{
			bool isBeamShot = entity.ModProjectile is MProjectile;
			if (!isBeamShot)
			{
				return;
			}
			// [Joost] Why were we globally overriding everything to 16 i frames? This hsould be done individually
			//entity.usesLocalNPCImmunity = true;
			//entity.localNPCHitCooldown = 16;
			bool isBlaze = entity.ModProjectile.Name.Contains("Red");//|| entity.ModProjectile is MProjectile MP && MP.shot.Contains("red");
			bool isCharged = entity.ModProjectile.Name.Contains("Charge");

			if (isBlaze)
			{
				entity.penetrate = Math.Max(entity.penetrate, isCharged ? 5 : 2);
			}
		}
	}
}
