using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using SteelSeries.GameSense;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using static MetroidMod.Sounds;

namespace MetroidMod.Content.BeamAddons
{
    class SpazerBeam : ModBeamAddon
    {
		int bd = 50;

		int extraShots = 2;
		public override bool AddOnlyAddonItem => false;
		public override Color PrimaryColor => MetroidMod.powColor;

		public override Color SecondaryColor => MetroidMod.powSecondaryColor;

		public override int ShotDust => DustID.YellowTorch;


		bool spazed = false;
		float spazeRad = 0f;
		float spazeTimer = 0;

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

		public override int[] ComboVisualsGet(string modifier)
		{
			switch (modifier)
			{
				case "Charged":
					return [2, -1];
				default:
					return base.ComboVisualsGet(modifier);
			}
		}

		public override void OnSpawn(MProjectile mpshot, IEntitySource source)
		{
			mpshot.symmetry = true;
			spazed = false;
		}

		public override bool PreAI(MProjectile mpshot)
		{
			return base.PreAI(mpshot);
		}

		public override void AI(MProjectile mpshot)
		{
			if (!spazed)
			{
				SpazeBehavior(mpshot);
			}
		}

		public void SpazeBehavior(MProjectile mpshot)
		{
			//Spazer uses a sine wave for a nice clean spazing.
			//See wave beam for a more thorough documentation of this.
			float increment = (MathHelper.TwoPi / 60);
			float SPAZE_DELAY = mpshot.Projectile.height / mpshot.Projectile.velocity.Length();
			float amplitude = mpshot.Projectile.width * mpshot.Projectile.scale * 4;
			float frequency = 5f - ((mpshot.groupSize - 1) / 2);

			//Must account for an arbitrary amount of projectiles. Any addon could just randomly add an extra shot, after all.
			float midpoint = (((float)mpshot.groupSize - 1) / 2) + 1; //This equation should do that automatically.

			float ampMultiplier = (mpshot.groupID + 1) - midpoint; //Subtract by the midpoint to create an offset. Must add 1 to ID so values line up properly.
																   //If odd, the middle projectile will have a multiplier of 0.

			if (spazeTimer >= SPAZE_DELAY)
			{
				//Increment the radian value toward Pi over 2 and then keep it there.
				spazeRad = Math.Min(spazeRad + increment * frequency, MathHelper.PiOver2);
			}
			//Delay is to make it look nicer.
			spazeTimer = Math.Min(spazeTimer + 1, SPAZE_DELAY);

			float shift = amplitude * (float)Math.Sin(spazeRad) * ampMultiplier;
			float rot = (float)Math.Atan2((mpshot.Projectile.velocity.Y), (mpshot.Projectile.velocity.X));
			//Update projectile's position.
			mpshot.Projectile.position.X = mpshot.corePosition.X + (float)Math.Cos(rot + (MathHelper.PiOver2)) * shift;
			mpshot.Projectile.position.Y = mpshot.corePosition.Y + (float)Math.Sin(rot + (MathHelper.PiOver2)) * shift;

			if (spazeRad == MathHelper.PiOver2)
			{
				spazed = true;
			}

		}
	}
}
