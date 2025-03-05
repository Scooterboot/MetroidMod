using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.BeamAddons
{
	internal class WaveBeam : ModBeamAddon
	{
		public override bool AddOnlyAddonItem => false;

		public override Color ShotColor => MetroidMod.waveColor;
		public override int ShotDust => 59;

		#region Stat Values
		float dmg = 25f;
		float oh = 10f;
		int crit = 5;
		int wallhax = 5; //the amt of tiles it can phase before dying
		#endregion

		public override void SetStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Ion;

			#region Visual Priority
			ShapePriority = 1;
			ColorPriority = 2;
			#endregion

			#region Stat Plugin
			DamageMult = dmg;
			OverheatMult = oh;
			CritChance = crit;

			TileInteract = wallhax;
			#endregion
			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
		}
		public override void SetItemDefaults(Item item)
		{
			item.rare = ItemRarityID.Green;
			item.value = Item.buyPrice(0, 1, 98, 7); //markiplier.jpeg
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

		//This is where I put the important stuff when I figure it out
		/// <summary>
		/// Makes the Wave Beam move in a sine-wave pattern.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="space"></param>
		public void WaveBehavior(Projectile p, bool spaze = false)
		{

		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(3)
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 8)
				.AddIngredient(ItemID.Amethyst, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
