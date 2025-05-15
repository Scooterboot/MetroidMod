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
		public override Color ShotColor => MetroidMod.powColor;

		public override int ShotDust => DustID.YellowTorch;


		bool spazed = false;

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

		public override void OnSpawn(MProjectile shot, IEntitySource source)
		{
			shot.symmetry = true;

			MetroidMod.Instance.Logger.Info(shot.groupID + " " + shot.groupSize)
;			float totalSpaze = shot.Projectile.height * shot.groupSize * 5;
			float rot = (float)Math.Atan2((shot.Projectile.velocity.Y), (shot.Projectile.velocity.X));
			float spazPos;
			if (shot.groupID <= (shot.groupSize / 2))
			{

			}
			else if (shot.groupID == ((shot.groupSize - 1) / 2) + 1 && shot.groupSize % 2 != 0)
			{

			}
			else if (shot.groupID > (shot.groupSize / 2))


				shot.Projectile.position.X = shot.corePosition.X + (float)Math.Cos(rot + (Math.Sin(MathHelper.PiOver2) * shot.Projectile.direction) * (totalSpaze / shot.groupID)) * 10;
			shot.Projectile.position.Y = shot.corePosition.Y + (float)Math.Sin(rot + (Math.Sin(MathHelper.PiOver2) * shot.Projectile.direction) * (totalSpaze / shot.groupID)) * 10;
		}

		public override bool PreAI(MProjectile shot)
		{
			return base.PreAI(shot);
		}

		public override void AI(MProjectile shot)
		{
			if (!spazed)
			{
				SpazeBehavior(shot);
			}
		}

		public void SpazeBehavior(MProjectile p)
		{
			//Must account for an arbitrary amount of projectiles

			//first, check if the number of projectiles is even or odd.
			if (p.groupSize % 2 == 0)
			{
				for (int i = 0; i < p.groupSize; i++)
				{
					if (i <= (p.groupSize / 2))
					{

					}
				}
			}//Total projectile number is even
			else
			{

			}//Total projectile number is odd


		}
	}
}
