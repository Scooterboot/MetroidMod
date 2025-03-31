using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Repository.Hierarchy;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
		public int sineDelay = 3;
		/// <summary>
		/// The lateral center of the sinewave.
		/// <br/>In other words, if the shot wasn't sinewaving, this is where it would be.
		/// </summary>
		public Vector2 sinelessCenter;
		#endregion
		public override void OnSpawn(MProjectile shot, IEntitySource source)
		{
			sinelessCenter = shot.Projectile.Center;
		}

		public override void AI(MProjectile shot)
		{
			//MetroidMod.Instance.Logger.Info("Ok so it's definitely RUNNING...");
			WaveBehavior(shot, shot.symmetry);
		}

		public override bool TileCollideStyle(MProjectile shot, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			//This code handles the Wave Beam's ability to pass through terrain.

			//Get the tile currently overlapping with the shot
			int i = (int)MathHelper.Clamp((shot.Projectile.Center.X) / 16f, 0, Main.maxTilesX - 1);
			int j = (int)MathHelper.Clamp((shot.Projectile.Center.Y) / 16f, 0, Main.maxTilesY - 1);

			if (Main.tile[i, j] != null && Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType] && !Main.tileSolidTop[Main.tile[i, j].TileType])
			{
				shot.TilesInteracted++;
				//Console.WriteLine("Tile found! \n" + wallhaxDepth + " " + wallhax);

			} //While inside of a tile, increment T.I. counter
			else if (shot.TilesInteracted > 0)
			{
				shot.TilesInteracted--;
			} //When outside of a tile, bring value back down. Prevents shots from getting eaten up from getting caught on corners.

			if (shot.TilesInteracted >= shot.TileInteract && shot.TileInteract > 0) //&& argument is included as a failsafe
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
		/// <param name="p"></param>
		/// <param name="spaze">Whether or not the sine wave should be symmetrical with other shots.</param>
		public void WaveBehavior(MProjectile p, bool spaze = false)
		{
			//This'll probably look really intimidating if you don't know too much about sinewaves but there's not all that much going on.
			//MetroidMod.Instance.Logger.Info("proj is " + p);
			float increment = (MathHelper.TwoPi / 60);

			//Consider making the following values external in the future?
			float amplitude = (p.Projectile.width * p.Projectile.height) * p.Projectile.scale;
			float wavesPerSecond = 5f;
			if (sineDelay <= 0)
			{
				if (spaze)
				{
					//TODO: waveStyle stuff
					//This is where all the stuff for multi-shot symmetrical patterns will go.
					MetroidMod.Instance.Logger.Info("Something's spazing when it shouldn't");
				}
				sineRad += increment * wavesPerSecond;
				if (sineRad >= MathHelper.TwoPi)
				{
					sineRad -= MathHelper.TwoPi;
				}
			}
			sineDelay = Math.Max(sineDelay - 1, 0);
			//If sineDir is *= to p.direction, firing left causes the shot to rapidly go back and forth between opposite wave and normal wave, making it look like two shots.
			//Consider employing if dynamic multishot does not work out.
			sineDir = p.Projectile.direction;
			
			//Set the projectile's offset from the sineless center
			float shift = amplitude * (float)Math.Sin(sineRad) * sineDir;
			sinelessCenter += p.Projectile.velocity;
			float rot = (float)Math.Atan2((p.Projectile.velocity.Y), (p.Projectile.velocity.X));
			//Update projectile's position.
			p.Projectile.position.X = sinelessCenter.X + (float)Math.Cos(rot + (MathHelper.PiOver2)) * shift;
			p.Projectile.position.Y = sinelessCenter.Y + (float)Math.Sin(rot + (MathHelper.PiOver2)) * shift;
			//MetroidMod.Instance.Logger.Info("One full wavebeh completed");
		}
		#endregion
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
