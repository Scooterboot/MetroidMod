using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using MetroidMod.Default;
using MetroidMod.ID;

namespace MetroidMod
{
	public abstract class ModMBSpecial : ModMBAddon
	{
		/// <summary>
		/// The <see cref="ModProjectile"/> this addon controls.
		/// </summary>
		public ModProjectile ModProjectile;
		/// <summary>
		/// The <see cref="ModProjectile"/> this addon controls. This one, specifically, is the explosion from a power bomb.
		/// </summary>
		public ModProjectile ModExplosionProjectile;
		/// <summary>
		/// The <see cref="Projectile"/> this addon controls.
		/// </summary>
		public Projectile Projectile;
		/// <summary>
		/// The <see cref="Projectile"/> this addon controls. This one, specifically, is the explosion from a power bomb.
		/// </summary>
		public Projectile ExplosionProjectile;
		public int ProjectileType { get; internal set; }
		public int ExplosionProjectileType { get; internal set; }
		public abstract string ProjectileTexture { get; }
		public abstract string ExplosionTexture { get; }
		public virtual string ExplosionSound => $"{MetroidMod.Instance.Name}/Assets/Sounds/PowerBombExplode";
		public float DamageMultiplier { get; set; } = 1;
		public int Knockback { get; set; } = 3;
		internal override sealed void InternalStaticDefaults()
		{
			AddonSlot = MorphBallAddonSlotID.Special;
		}
		public override void Load()
		{
			base.Load();
			ModProjectile = new MBSpecialProjectile(this);
			ModExplosionProjectile = new MBSpecialExplosion(this);
			if (ModProjectile == null) { throw new Exception("WTF happened here? MBSpecialProjectile is null!"); }
			if (ModExplosionProjectile == null) { throw new Exception("WTF happened here? MBSpecialExplosion is null!"); }
			Mod.AddContent(ModProjectile);
			Mod.AddContent(ModExplosionProjectile);
		}
		public virtual void SetExplosionProjectileDefaults(Projectile proj) { }
		public virtual void ExplosionAI() { }
		public virtual bool Kill(int timeLeft) { return true; }
		public virtual void OnHitNPC(NPC target, int damage, float knockback, bool crit) { }
		public virtual bool ExplosionPreDraw(ref Color lightColor) { return true; }
	}
}
