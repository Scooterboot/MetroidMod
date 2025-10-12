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
			modMissileAddon.SetProjectileDefaults(this);
		}

		#region behavior
		public override void OnSpawn(IEntitySource source)
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
			base.OnSpawn(source);
			MetroidMod.Instance.Logger.Info(this + " VS " + mProjectile + "\nARE THEY THE SAME???? " + (this == mProjectile));
			modMissileAddon.OnSpawn(this, source);
		}

		public override bool PreAI()
		{
			if (Override != null)
			{
				return modMissileAddon.PreAI(this)
					&& Override.PreAI(this);
			}
			else
			{
				return modMissileAddon.PreAI(this);
			}
		}
		public override void AI()
		{
			modMissileAddon.AI(this);
			if (Override != null) { Override.AI(this); }
		}
		public override void PostAI()
		{
			base.PostAI();
			modMissileAddon.PostAI(this);
			if (Override != null) { Override.PostAI(this); }
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			if (Override != null)
			{
				return modMissileAddon.TileCollideStyle(this, ref width, ref height, ref fallThrough, ref hitboxCenterFrac)
					&& Override.TileCollideStyle(this, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
			}
			else
			{
				return modMissileAddon.TileCollideStyle(this, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Override != null)
			{
				return modMissileAddon.OnTileCollide(this, oldVelocity)
					&& Override.OnTileCollide(this, oldVelocity);
			}
			else
			{
				return modMissileAddon.OnTileCollide(this, oldVelocity);
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			modMissileAddon.OnHitNPC(this, target, hit, damageDone);
			if (Override != null) { Override.OnHitNPC(this, target, hit, damageDone); }
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			modMissileAddon.OnHitPlayer(this, target, info);
			if (Override != null) { Override.OnHitPlayer(this, target, info); }
		}

		public override void OnKill(int timeLeft)
		{
			modMissileAddon.OnKill(this, timeLeft);
			if (Override != null) { Override.OnKill(this, timeLeft); }
		}

		#endregion

		#region drawing
		public override bool PreDraw(ref Color lightColor)
		{
			return modMissileAddon.PreDrawProjectile(this, ref lightColor);
		}

		public override void PostDraw(Color lightColor)
		{
			modMissileAddon.PostDrawProjectile(this, lightColor);
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
