using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace MetroidMod.Content.BeamAddons
{
    class SpazerBeam : ModBeamAddon
    {
		int bd = 50;

		int extraShots = 2;
		public override bool AddOnlyAddonItem => false;
		public override Color ShotColor => MetroidMod.powColor;

		public override int ShotDust => DustID.YellowTorch;

		public override void SetStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Spread;

			ShapePriority = 3;
			ColorPriority = 1;

			BaseDamage = bd;
			AddShots = extraShots;
			TileInteract = 3;
		}

		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.rare = ItemRarityID.Green;
			item.value = Item.buyPrice(0, 2, 50, 7);
		}

		public override int[] SpecialComboGet(string modifier)
		{
			switch (modifier)
			{
				case "Charged":
					return [2];
				default:
					return base.SpecialComboGet(modifier);
			}
		}
	}
}
