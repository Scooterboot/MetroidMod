using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using MetroidModPorted.Default;
using MetroidModPorted.ID;

namespace MetroidModPorted
{
	public abstract class ModMBSpecial : ModMBAddon
	{
		/// <summary>
		/// The <see cref="ModProjectile"/> this addon controls.
		/// </summary>
		public ModProjectile Projectile { get; internal set; }
		/// <summary>
		/// The <see cref="ModProjectile"/> this addon controls. This one, specifically, is the explosion from a power bomb.
		/// </summary>
		public ModProjectile ExplosionProjectile { get; internal set; }
		public int ProjectileType { get; internal set; }
		public int ExplosionProjectileType { get; internal set; }
		public abstract string ProjectileTexture { get; }
		public abstract string ExplosionTexture { get; }
		public float DamageMultiplier { get; set; } = 1;
		public int Knockback { get; set; } = 3;
		internal override sealed void InternalStaticDefaults()
		{
			AddonSlot = MorphBallAddonSlotID.Special;
		}
		public override void Load()
		{
			base.Load();
			Projectile = new MBSpecialProjectile(this);
			ExplosionProjectile = new MBSpecialExplosion(this);
			if (Projectile == null) { throw new Exception("WTF happened here? MBSpecialProjectile is null!"); }
			if (Projectile == null) { throw new Exception("WTF happened here? MBSpecialExplosion is null!"); }
			Mod.AddContent(Projectile);
			Mod.AddContent(ExplosionProjectile);
		}
		public virtual bool ExplosionAI(ref float speed, ref Color color) { return true; }
		public virtual void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2) { }
		public virtual void OnHitNPC(NPC target, int damage, float knockback, bool crit) { }
		public virtual bool ExplosionPreDraw(ref Color lightColor) { return true; }
	}
}
