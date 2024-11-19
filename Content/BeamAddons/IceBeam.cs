using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.ID;
using MetroidMod.Content.Buffs;
using Terraria.ID;

namespace MetroidMod.Content.BeamAddons
{
	
	public class IceBeam : ModBeamAddon
	{
		public override bool AddOnlyAddonItem => false; //Idk why you'd ever want to enable this
		public override Color ShotColor => new(0, 255, 255); //Highly recommend making the shot texture greyscale for maximum effect
		public override int ShotDust => 59;
		public override int ShapePriority => 1;
		public override int ColorPriority => 4;
		public override bool SoundOverride => true;

		public override void SetStaticDefaults()
		{
			//these values determine how the addon will interact with the dynamic visual system
			AddonSlot = BeamAddonSlotID.Ability;

			ShapePriority = 1;
			ColorPriority = 4;
			SoundOverride = true;

			BaseDamage = -5;
			InflictsBuff = ModContent.BuffType<IceFreeze>();
		}
	}
}

