using System;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace MetroidMod.Content.MissileAddons.BeamCombos
{
	internal class HomingMissile : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override float DamageMult
		{
			get {
				return 2f;
			}

			set {
				base.DamageMult = value;
			}
		}
		public override Color PrimaryColor => MetroidMod.powColor;
		public override Color SecondaryColor => MetroidMod.powSecondaryColor;
		public override int ShotDust => DustID.YellowTorch;
		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Charge;

			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
			base.SetStaticDefaults();
		}
		public override void AI(MProjectile mpshot)
		{
			if (mpshot is MissileShot oof)
			{
				if (oof.fileMod.Contains("Charge"))
				{
					mpshot.HomingBehavior(mpshot.Projectile);
				}
			}
		}
		public override void SetItemDefaults(Item item) //TO DO SOMETHING WITH THIS
		{
			item.value = 30000;
			item.rare = ItemRarityID.LightRed;
			base.SetItemDefaults(item);
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(8)
				.AddIngredient(ItemID.IceBlock, 25)
				.AddIngredient(ItemID.Bone, 10)
				.AddIngredient(ItemID.Sapphire)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
