using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

using MetroidModPorted.Common.Players;
using MetroidModPorted.Default;
using MetroidModPorted.ID;

namespace MetroidModPorted
{
	public abstract class ModMBWeapon : ModMBAddon
	{
		public int BombDamage { get; set; } = 0;
		/// <summary>
		/// The <see cref="ModProjectile"/> this addon controls.
		/// </summary>
		public ModProjectile Projectile { get; internal set; }
		public int ProjectileType { get; internal set; }
		public abstract string ProjectileTexture { get; }
		internal override sealed void InternalStaticDefaults()
		{
			AddonSlot = MorphBallAddonSlotID.Weapon;
		}
		public override void Load()
		{
			base.Load();
			Projectile = new MBWeaponProjectile(this);
			if (Projectile == null) { throw new Exception("WTF happened here? MBWeaponProjectile is null!"); }
			Mod.AddContent(Projectile);
		}
		public override void UpdateEquip(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.bombDamage = player.GetWeaponDamage(Item);
			mp.Bomb(player, ProjectileType, Item);
		}
		public virtual void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2) { }
		public virtual void OnHitNPC(NPC target, int damage, float knockback, bool crit) { }
	}
}
