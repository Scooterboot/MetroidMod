using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidModPorted
{
	public abstract class GlobalBeam : ModType
	{
		public virtual bool OnChargeShoot()
		{
			return true;
		}
		public virtual bool OnShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, ModBeam beam)
		{
			return true;
		}
		public virtual bool CanUse(Player player, Item beam)
		{
			return true;
		}
        protected override sealed void Register()
        {
            BeamLoader.globalBeams.Add(this);
        }
        public override sealed void SetupContent() => SetStaticDefaults();

        public override void SetStaticDefaults() => base.SetStaticDefaults();
	}
}
