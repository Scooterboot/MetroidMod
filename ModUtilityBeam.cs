using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MetroidModPorted.Common.Players;
using MetroidModPorted.ID;

namespace MetroidModPorted
{
	/*
	public abstract class ModUtilityBeam : ModBeam
	{
		public virtual int ShotAmount { get; set; } = 2;

		public override sealed void Load()
		{
			base.Load();
			AddonSlot = BeamAddonSlotID.Utility;
			AddonChargeDamage = 0f;
			AddonChargeHeat = 0f;
		}

		internal override sealed void InternalStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Utility;
			AddonChargeDamage = 0f;
			AddonChargeHeat = 0f;
		}

		public sealed override bool OnShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return base.OnShoot(player, source, position, velocity, type, damage, knockback);
		}
	}
	*/
}
