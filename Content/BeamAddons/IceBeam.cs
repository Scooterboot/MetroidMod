using MetroidMod.Content.Buffs;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.BeamAddons
{
	public class IceBeam : ModBeamAddon
	{
		//TODO:
		//Make it actually inflict the debuff (it does that now)
		/// <summary>
		/// If true, the projectile will rotate.
		/// </summary>
		public bool bananasRotatE = false;

		public int iceDustTimer = 4;

		public override bool AddOnlyAddonItem => false; //Idk why you'd ever want to enable this
		public override Color PrimaryColor => new(0, 255, 255); //Highly recommend making the shot texture greyscale for maximum effect
		public override Color SecondaryColor => MetroidMod.iceSecondaryColor;
		public override string ImpactSound => $"{Mod.Name}/Assets/Sounds/BeamAddons/IceBeam/Impact";
		public override int ShotDust => DustID.IceGolem;

		public override void SetStaticDefaults()
		{
			//these values determine how the addon will interact with the dynamic visual system
			AddonSlot = BeamAddonSlotID.Ability;

			ShapePriority = 1;
			ColorPriority = 4;
			SoundOverride = true;
			bananasRotatE = false;

			BaseDamage = -5;
			VelocityMult = -25f;
			InflictsBuff = ModContent.BuffType<IceFreeze>();
		}

		public override int[] ComboVisualsGet(string modifier)
		{
			if (modifier == "Charged" || modifier == "")
			{
				bananasRotatE = true;
				return base.ComboVisualsGet(modifier);
			}
			else
			{
				bananasRotatE = false;
				return base.ComboVisualsGet(modifier);
			}
		}

		public override void ShapeBehavior(MProjectile mpshot)
		{
			//MetroidMod.Instance.Logger.Info("rotat e? " + bananasRotatE);
			if (bananasRotatE)
			{
				//MetroidMod.Instance.Logger.Info("go..... g    O..........");
				mpshot.Projectile.rotation += 0.6f * mpshot.Projectile.direction;
			}
		}

		public override void AI(MProjectile mpshot)
		{
			if (iceDustTimer <= 0)
			{
				Dust.NewDust(mpshot.Projectile.position, mpshot.Projectile.width, mpshot.Projectile.height, ShotDust, 0, 1, 0, default, 1f);
				iceDustTimer = 5;
			}
			iceDustTimer--;
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

