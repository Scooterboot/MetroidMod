using System;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Default
{
	[Autoload(false)]
	
	internal class MissileAddonProjectile(ModMissileAddon modMissileAddon) : MProjectile
	{
		public ModMissileAddon modMissileAddon = modMissileAddon;

		public override string Texture => modMissileAddon.ShotTexture;
		public override string Name => modMissileAddon.Name + "Projectile";

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			modMissileAddon.SetProjectileDefaults(mProjectile);
		}

		#region behavior
		public override void OnSpawn(IEntitySource source)
		{
			modMissileAddon.OnSpawn(mProjectile, source);
		}

		public override bool PreAI()
		{
			return modMissileAddon.PreAI(mProjectile);
		}
		public override void AI()
		{
			modMissileAddon.AI(mProjectile);
		}
		public override void PostAI()
		{
			modMissileAddon.PostAI(mProjectile);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return modMissileAddon.TileCollideStyle(mProjectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return modMissileAddon.OnTileCollide(mProjectile, oldVelocity);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			modMissileAddon.OnHitNPC(mProjectile, target, hit, damageDone);
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			modMissileAddon.OnHitPlayer(mProjectile, target, info);
		}

		public override void OnKill(int timeLeft)
		{
			modMissileAddon.OnKill(mProjectile, timeLeft);
		}

		#endregion

		#region drawing
		public override bool PreDraw(ref Color lightColor)
		{
			return modMissileAddon.PreDrawProjectile(mProjectile, ref lightColor);
		}

		public override void PostDraw(Color lightColor)
		{
			modMissileAddon.PostDrawProjectile(mProjectile, lightColor);
		}
		#endregion

		//Don't forget these two methods. Very important.
		public override ModProjectile Clone(Projectile newEntity)
		{
			MissileAddonProjectile obj = (MissileAddonProjectile)base.Clone(newEntity);
			obj.modMissileAddon = modMissileAddon;
			return obj;
		}

		public override ModProjectile NewInstance(Projectile entity)
		{
			var inst = Clone(entity);
			return inst;
		}
	}
}
