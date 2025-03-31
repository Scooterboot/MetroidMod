using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat.Commands;
using Terraria.ID;

namespace MetroidMod.Content.BeamAddons
{
    class PlasmaBeam : ModBeamAddon
    {
		public override bool AddOnlyAddonItem => false;

		public override int ShotDust => DustID.KryptonMoss;
		public override Color ShotColor => MetroidMod.plaGreenColor;


		int die = 100;
		float hot = 50f;
		int pierce = 255;

		public override void SetStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Secondary;
			ShapePriority = 4;
			ColorPriority = 3;

			BaseDamage = die;
			OverheatMult = hot;

			EntityInteract = pierce;
		}

		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.buyPrice(0, 10, 1, 67);
		}

		public override int[] SpecialComboGet(string modifier)
		{
			switch(modifier)
			{
				case "Charged":
					return [2];
				default:
					return base.SpecialComboGet(modifier);
			}
		}
	}
}
