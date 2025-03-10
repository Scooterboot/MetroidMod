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
		//TODO:
		//Make it actually inflict the debuff
		//Make ice beam shots rotate when it has shape priority

		public override bool AddOnlyAddonItem => false; //Idk why you'd ever want to enable this
		public override Color ShotColor => new(0, 255, 255); //Highly recommend making the shot texture greyscale for maximum effect
		public override int ShotDust => 59;
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

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(3)
				.AddIngredient(ItemID.IceBlock, 25)
				.AddIngredient(ItemID.Bone, 10)
				.AddIngredient(ItemID.Sapphire, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}

