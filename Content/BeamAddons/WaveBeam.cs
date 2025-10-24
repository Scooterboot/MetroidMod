using System;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace MetroidMod.Content.BeamAddons
{
	internal class WaveBeam : ModBeamAddon
	{
		public override bool AddOnlyAddonItem => false;

		public override Color PrimaryColor => MetroidMod.waveColor;

		public override Color SecondaryColor => MetroidMod.waveSecondaryColor;
		public override int ShotDust => DustID.FireworkFountain_Pink;

		#region Stat Values
		private readonly float dmg = 25f;
		private readonly float oh = 10f;
		private readonly int crit = 5;
		private readonly int wallhax = 5; //the amt of tiles it can phase before dying
		#endregion

		/// <summary>
		/// If true, add an extra shot when charged.
		/// </summary>
		private readonly bool doubleUp = false;

		#region Item properties
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
			AddShots = 0;

			TileInteract = wallhax;
			#endregion
			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
		}

		public override void SetItemDefaults(Item item)
		{
			item.rare = ItemRarityID.Green;
			item.value = Item.buyPrice(0, 1, 98, 7); //markiplier.jpeg
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
		#endregion

		#region Addon interaction fields
		public override int[] ComboVisualsGet(string modifier)
		{
			switch (modifier)
			{
				case "Charged":
					//Check if doubleUp is true
					//If true, add an extra projectile to the shot.
					//If not, don't
					return [2, -1];

				default:
					return [4, -1];
			}
		}

		public override float[] EdgeCaseData(ModBeamAddon[] addons, float[] statVals, string bonusMod)
		{
			//WELCOME ONE AND ALL TO THE ENTIRE REASON THIS METHOD EXISTS
			//Because hardcoding it is just too below me or some shit I guess			-Z
			//MetroidMod.Instance.Logger.Info("edgecase bullshit  " + bonusMod);
			if (bonusMod == "Charged" && statVals[9] == 0)
			{
				MetroidMod.Instance.Logger.Info("HEY WAVE BEAM'S DOING THE THING!!!");
				return [0, 0, 0, 0, 1];
			}
			else { return base.EdgeCaseData(addons, statVals, bonusMod); }

		}
		#endregion

		#region Behavior modification

		#region Vital sinewave variables
		//There's a nonzero chance some of these may have to be moved somewhere else for spazing/netcoding reasons.
		//Thankfully, this is not my problem. My condolences!		-Past Z

		/// <summary>
		/// Radian value used to calculate the offset used for the Wave Beam's trademark sinewave.
		/// <br/>Loops from 0 to 2*pi.
		/// </summary>
		public float sineRad = 0f;
		/// <summary>
		/// The direction the sine wave is... sine-ing in.
		/// </summary>
		public float sineDir = -1;
		/// <summary>
		/// The time in game ticks until the sine wave starts.
		/// </summary>
		public float sineTimer = 0;
		#endregion

		public override void OnSpawn(MProjectile mpshot, IEntitySource source)
		{
			if (source is EntitySource_Parent parent && parent.Entity is Player player && player.whoAmI == Main.myPlayer && (!mpshot.symmetry))
			{
				MGlobalItem ac = player.HeldItem.GetGlobalItem<MGlobalItem>();
				//if (ac.inverter != 1 && ac.inverter != -1) { ac.inverter = 1; } //Potential failsafe to guard against bad values, revisit after making spazed waves
				if (!mpshot.symmetry)
				{
					sineDir = ac.inverter;
					ac.inverter *= -1;
				}
				MetroidMod.Instance.Logger.Info("Fleeped that sheet");
			} //Check if the shot is asymmetrical. If it is, flip the Arm Cannon's inverter. This allows for the sinewave direction to flip between shots.

		}

		//public override void AI(MProjectile mpshot)
		//{
		//	//MetroidMod.Instance.Logger.Info("Ok so it's definitely RUNNING...");
		//	WaveBehavior(mpshot, mpshot.symmetry);
		//}

		public override void PostAI(MProjectile mpshot)
		{
			//MetroidMod.Instance.Logger.Info("Ok so it's definitely RUNNING...");
			WaveBehavior(mpshot, mpshot.symmetry);
		}

		public override bool TileCollideStyle(MProjectile mpshot, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			//This code handles the Wave Beam's ability to pass through terrain.

			//Get the tile currently overlapping with the shot
			int i = (int)MathHelper.Clamp(mpshot.Projectile.Center.X / 16f, 0, Main.maxTilesX - 1);
			int j = (int)MathHelper.Clamp(mpshot.Projectile.Center.Y / 16f, 0, Main.maxTilesY - 1);

			if (Main.tile[i, j] != null && Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType] && !Main.tileSolidTop[Main.tile[i, j].TileType])
			{
				mpshot.TilesInteracted++;
				//Console.WriteLine("Tile found! \n" + wallhaxDepth + " " + wallhax);

			} //While inside of a tile, increment T.I. counter
			else if (mpshot.TilesInteracted > 0)
			{
				mpshot.TilesInteracted--;
			} //When outside of a tile, bring value back down. Prevents shots from getting eaten up from getting caught on corners.

			if (mpshot.TilesInteracted >= mpshot.TileInteract && mpshot.TileInteract > 0) //&& argument is included as a failsafe
			{
				MetroidMod.Instance.Logger.Info("Yo the thing shoulda despawned by now");
				return true;
			} //Destroy shot if alloted interactions has been used up
			else
			{
				return false;
			} //Shot may freely pass through tiles.
		}

		/// <summary>
		/// Makes the Wave Beam move in a sine-wave pattern.
		/// </summary>
		/// <param name="mpshot"></param>
		/// <param name="spaze">Whether or not the sine wave should be symmetrical with other shots.</param>
		public void WaveBehavior(MProjectile mpshot, bool spaze = false)
		{
			//This'll probably look really intimidating if you don't know too much about sinewaves but there's not all that much going on.
			//MetroidMod.Instance.Logger.Info("proj is " + p);
			float increment = MathHelper.TwoPi / 60;
			float sineDelay = mpshot.Projectile.height / mpshot.Projectile.velocity.Length();
			float sineDelay2 = MathHelper.Lerp(mpshot.Projectile.height, mpshot.Projectile.velocity.Length(), 2f);

			//Consider making the following values external in the future?
			float amplitude = mpshot.Projectile.width * mpshot.Projectile.scale * 4;
			float ampMultiplier = 1; //Used for larger multishots to space out shots
			float wavesPerSecond = 5f - ((mpshot.groupSize - 1) / 2);
			if (sineTimer >= sineDelay)
			{
				if (spaze)
				{
					//TODO: waveStyle stuff

					//Must set ampMultiplier dynamically based on groupID.
					//Half of the projectiles will have the inverse of the other half's values
					//(e.g: 4 projectiles could have -2, -1, 1, 2 in that order)
					//However it may be best to use half-steps on even amounts so the overal amount of space is consistent
					float midpoint = (((float)mpshot.groupSize - 1) / 2) + 1; //This equation should do that automatically. Emphasis on "should".

					ampMultiplier = mpshot.groupID + 1 - midpoint; //Subtract by the midpoint to create an offset. Must add 1 to ID so values line up properly.
																   //If odd, the middle projectile will have a multiplier of 0.

				}
				else
				{
					ampMultiplier = sineDir;
				}
				sineRad += increment * wavesPerSecond;
				if (sineRad >= MathHelper.TwoPi)
				{
					sineRad -= MathHelper.TwoPi;
				}
			}
			sineTimer = Math.Min(sineTimer + 1, sineDelay);
			//If sineDir is *= to p.direction, firing left causes the shot to rapidly go back and forth between opposite wave and normal wave, making it look like two shots.
			//Consider employing if dynamic multishot does not work out.
			//sineDir = p.Projectile.direction;

			//Set the projectile's offset from the sineless center
			float shift = amplitude * (float)Math.Sin(sineRad) * ampMultiplier;
			float rot = (float)Math.Atan2(mpshot.Projectile.velocity.Y, mpshot.Projectile.velocity.X);
			//Update projectile's position.
			mpshot.Projectile.position.X = mpshot.corePosition.X + ((float)Math.Cos(rot + MathHelper.PiOver2) * shift);
			mpshot.Projectile.position.Y = mpshot.corePosition.Y + ((float)Math.Sin(rot + MathHelper.PiOver2) * shift);
			//MetroidMod.Instance.Logger.Info("One full wavebeh completed");
		}
		#endregion
	}
}
