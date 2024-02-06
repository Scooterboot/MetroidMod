using System;
using MetroidMod.Common.Players;
using MetroidMod.Default;
using MetroidMod.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod
{
	public abstract class ModMBWeapon : ModMBAddon
	{
		public int BombDamage { get; set; } = 0;
		/// <summary>
		/// The <see cref="ModProjectile"/> this addon controls.
		/// </summary>
		public ModProjectile ModProjectile { get; internal set; }
		/// <summary>
		/// The <see cref="Projectile"/> this addon controls.
		/// </summary>
		public Projectile Projectile { get; internal set; }
		public int ProjectileType { get; internal set; }
		public abstract string ProjectileTexture { get; }
		internal override sealed void InternalStaticDefaults()
		{
			AddonSlot = MorphBallAddonSlotID.Weapon;
		}
		public override void Load()
		{
			base.Load();
			ModProjectile = new MBWeaponProjectile(this);
			if (ModProjectile == null) { throw new Exception("WTF happened here? MBWeaponProjectile is null!"); }
			Mod.AddContent(ModProjectile);
		}
		public override void Unload()
		{
			ModProjectile.Unload();
			ModProjectile = null;
			Projectile = null;
			base.Unload();
		}
		public override void UpdateEquip(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.bombDamage = player.GetWeaponDamage(Item);
			mp.Bomb(player, ProjectileType, Item);
		}
		public virtual void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2) { }
		public virtual void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }
	}
}
