using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.ID;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Content.Projectiles;
using rail;

namespace MetroidMod.Content.BeamAddons
{
	public class ChargeBeam : ModBeamAddon
	{
		//So fun fact, this is the first ModBeamAddon ever made!      -Z
		public override bool AddOnlyAddonItem => false; //Idk why you'd ever want to enable this

		#region Projectile visuals
		public override Color ShotColor => new(248, 248, 110); //This should hopefully only be for light color in the future, assuming I make shaders
		public override int ShotDust => 64;

		public override string ShotSound => $"{Mod.Name}/Assets/Sounds/ArmCannon/Shot";

		public override string ImpactSound => $"{Mod.Name}/Assets/Sounds/ArmCannon/BeamImpactSound";

		#endregion

		public float chargeMultiplier = 3f;

		public override void SetStaticDefaults()
		{
			
			AddonSlot = BeamAddonSlotID.Primary;
			//these values determine how the addon will interact with the dynamic visual system
			ShapePriority = 0;
			ColorPriority = 0;
			SoundOverride = false;

			//This is where you set your numbers
			BaseDamage = 5;
			DamageMult = 5f;
			BaseOverheat = 5;
			OverheatMult = 0f;
			BaseSpeed = 0;
			SpeedMult = 0f;
			BaseVelocity = 0;
			VelocityMult = 0f;
			CritChance = 0;
		}

		public override void SetItemDefaults(Item item)
		{
			item.rare = ItemRarityID.Blue;
		}
		/*
		public override void HoldFireBehavior(Player player, Item item)
		{
			//

			//Get all the relevant data
			MPlayer mp = player.GetModPlayer<MPlayer>();
			MGlobalItem ac = item.GetGlobalItem<MGlobalItem>();
			ArmCannon wepon = ModContent.GetModItem(item.type) as ArmCannon;
			Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
			float currentMultiplier;
			float chargeDelay = item.useTime;
			#region Charge Thresholds
			if (mp.statCharge == 5 && player.controlUseItem)
			{
				//spawn the charge lead
				//play the charging noise
			} //Player has held Fire long enough to be recognized as trying to charge the beam
			
			if (mp.statCharge == 75 && player.controlUseItem)
			{
				ac.assetModB = "Charged";
				//If beam, change arm cannon's use sound effect to itself plus "Charged"
				//Enable pseudo-screw
			} //Player has built up enough charge to create a charged beam shot

			if (mp.statCharge == 99.9f && player.controlUseItem) //can't do 100 otherwise it'll spam this every tick
			{
				//play charging complete sound

				//Don't have any of it yet but if it's charging a held combo spawn that projectile now
			} //Player has maxed out their charge

			#endregion

			if (player.controlUseItem && !mp.ballstate && !mp.shineActive && !player.dead && !player.noItems)
			{
				if (ac.isBeam || wepon.MissileAddonAccess[MissileAddonSlotID.Charge] != null)
				{
					if (chargeDelay > 0)
					{
						chargeDelay -= 0.5f;
						if (chargeDelay < 0) { chargeDelay = 0; }
					}
					else
					{
						MetroidMod.Instance.Logger.Info("Charging!");
						mp.statCharge += 0.1f;
						currentMultiplier = (chargeMultiplier / 100) * (mp.statCharge / 2);
					}
				}

			}//Stuff that happens while fire is held and the player is legally allowed to shoot
			else if (!mp.ballstate && !mp.shineActive && !player.dead && !player.noItems && mp.statCharge > 5)
			{
				MetroidMod.Instance.Logger.Info("Releasing charge!");
				if (mp.statCharge == 100)
				{
					if (ac.isBeam) 
					{
						wepon.Shoot(player, player.whoAmI, oPos, item.shootSpeed, item.shoot, (int)(item.damage * chargeMultiplier), item.knockBack);
					} //If beam, release the kraken
					 //Else, check if the installed charge missile is not a holdfire
					 //If it isn't, launch the nuke
				}
				else if (mp.statCharge >= 75)
				{
					//If beam, release a slightly less cool kraken
					//Else, fire a boring normal missile
				}
				else
				{
					//fire the weapon normally and boringly
				}
				//kill the charge lead
				mp.statCharge = 0;
			}//Stuff that happens once the player releases fire with an existent amount of charge
			else
			{
				ac.assetModB = "NADA";
				mp.statCharge = 0;
			}
		}
		*/
	}



	//What I am about to do is probably incredibly stupid. Be warned.
	public class ChargeLead : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/ChargeLead";
		public string ChargingSound => $"{Mod.Name}/Assets/Sounds/ArmCannon/BeamChargingSound";
		public string MaxChargeSound => $"{Mod.Name}/Assets/Sounds/ArmCannon/ChargeMax";

		public Color ballColor = MetroidMod.powColor;

		public Item sourceItem;

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.timeLeft = 8800;
			Projectile.ownerHitCheck = true;
			Projectile.friendly = false; //Keeps it from hurting enemies
			Projectile.hostile = false; //Keeps it from hurting friends
			Projectile.tileCollide = false;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			MPlayer mp = player.GetModPlayer<MPlayer>();

			//THE BARE MINIMUM OF WHAT I WANT THIS TO DO:
			//* Increase in size with charge stat
			//* Glue itself to the end of the arm cannon
			//* Delete itself upon releasing fire
			//* Function with both the beam and missiles
			//* Color itself to either match the current beam or the current charge combo, depending on the context
		}
	}
}

