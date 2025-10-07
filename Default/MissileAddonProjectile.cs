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
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;
			modMissileAddon.OnSpawn(mProjectile, source);
		}

		public override bool PreAI()
		{
			if (Override != null)
			{
				return (modMissileAddon.PreAI(mProjectile) 
					&& Override.PreAI(mProjectile));
			}
			else
			{
				return modMissileAddon.PreAI(mProjectile);
			}
		}
		public override void AI()
		{
			modMissileAddon.AI(mProjectile);
			if (Override != null) { Override.AI(mProjectile); }
		}
		public override void PostAI()
		{
			modMissileAddon.PostAI(mProjectile);
			if (Override != null) { Override.PostAI(mProjectile); }
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			if (Override != null)
			{
				return (modMissileAddon.TileCollideStyle(mProjectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac) 
					&& Override.TileCollideStyle(mProjectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac));
			}
			else 
			{ 
				return modMissileAddon.TileCollideStyle(mProjectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac); 
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Override != null)
			{
				return (modMissileAddon.OnTileCollide(mProjectile, oldVelocity)
					&& Override.OnTileCollide(mProjectile, oldVelocity));
			}
			else
			{
				return modMissileAddon.OnTileCollide(mProjectile, oldVelocity);
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			modMissileAddon.OnHitNPC(mProjectile, target, hit, damageDone);
			if (Override != null) { Override.OnHitNPC(mProjectile, target, hit, damageDone); }
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			modMissileAddon.OnHitPlayer(mProjectile, target, info);
			if (Override != null) { Override.OnHitPlayer(mProjectile, target, info); }
		}

		public override void OnKill(int timeLeft)
		{
			modMissileAddon.OnKill(mProjectile, timeLeft);
			if (Override != null) { Override.OnKill(mProjectile, timeLeft); }
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
