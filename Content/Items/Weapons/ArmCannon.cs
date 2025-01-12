using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Common.Systems;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Projectiles;
using MetroidMod.Content.Projectiles.missiles;
using MetroidMod.Content.Projectiles.Paralyzer;
using MetroidMod.Default;
using MetroidMod.ID;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MetroidMod.Content.Items.Weapons
{
	internal class ArmCannon : ModItem
	{
		#region Beam and Missile addon storage

		//[Power Beam addons]
		/// <summary>
		/// The array in which active beam addons are stored.
		/// </summary>
		private Item[] beamAddons;
		/// <summary>
		/// Used to access the contents of the beam addon array.<br/>
		/// Needed because quote: "Something something data security"
		/// </summary>
		public Item[] BeamAddonAccess
		{
			get {
				if (beamAddons == null) //This is a failsafe; if the array comes up null, reset the array
				{
					beamAddons = new Item[BeamAddonSlotID.Count]; //iterate through all slots of the array
					for (int i = 0; i < beamAddons.Length; ++i)
					{
						beamAddons[i] = new Item();
						beamAddons[i].TurnToAir();
					}
				}
				return beamAddons;
			}
			set { beamAddons = value; }
		}

		/// <summary>
		/// The array in which secondary charge addons are stored.<br/>
		/// </summary>
		private Item[] chargeQuickSwap;
		/// <summary>
		/// Used to access the contents of the beam array array.
		/// </summary>
		public Item[] ChargeQuickSwapAccess
		{
			get {
				if (chargeQuickSwap == null)
				{
					chargeQuickSwap = new Item[8]; //Array is dynamic, so reset to one Item long and turn that Item into air
					for(int i = 0;i < chargeQuickSwap.Length; ++i)
					{
						chargeQuickSwap[i] = new Item();
						chargeQuickSwap[i].TurnToAir();
					}
				}
				return chargeQuickSwap;
			}
			set { chargeQuickSwap = value; }
		}

		//[Missile Launcher addons]
		/// <summary>
		/// The array in which active missile addons are stored.
		/// </summary>
		private Item[] missileAddons;
		/// <summary>
		/// Used to access the contents of the missile addon array.<br/>
		/// Needed because quote: "Something something data security"
		/// </summary>
		public Item[] MissileAddonAccess
		{
			get {
				if(missileAddons == null) //see BeamAddonAccess above
				{
					missileAddons = new Item[MissileAddonSlotID.Count];
					for (int i = 0; i < missileAddons.Length; ++i)
					{
						missileAddons[i] = new Item();
						missileAddons[i].TurnToAir();
					}
				}
				return missileAddons;
			}
			set { missileAddons = value; }
		}

		/// <summary>
		/// The array in which secondary charge combos are stored.
		/// </summary>
		private Item[] comboQuickChange;
		/// <summary>
		/// Used to access the contents of the combo quick change array.
		/// </summary>
		public Item[] ComboQuickChangeAccess
		{
			get { 
				if(comboQuickChange == null) //See BeamArrayAccess above
				{
					comboQuickChange = new Item[8];
					for (int i = 0;i < comboQuickChange.Length; ++i)
					{
						comboQuickChange[i] = new Item();
						comboQuickChange[i].TurnToAir();
					}
				}
				return comboQuickChange;
			}
			set { comboQuickChange = value; }
		}

		/// <summary>
		/// Keeps track of the addons that were selected by the <b>Visual Priority System</b>.
		/// <br/><br/><i>(VisualWinners was taken by <see cref="BeamShot"/>)</i>
		/// </summary>
		public int[] VisualDinners;
		/// <summary>
		/// Keeps track of the slot containing the currently-active holdfire. -1 means no holdfire.
		/// </summary>
		public int HoldFireSlot;

		public SoundStyle beamSound = Sounds.Items.Weapons.PowerBeamSound;
		public SoundStyle missileSound = Sounds.Items.Weapons.MissileSound;
		#endregion


		#region Power Beam stats

		/// <summary>
		/// The Power Beam's base damage, before accounting for addon multipliers.
		/// </summary>
		int BeamBaseDamage = 10;
		/// <summary>
		/// The Power Beam's base usetime, before accounting for addon multipliers.
		/// </summary>
		int BeamBaseSpeed = 14;
		/// <summary>
		/// The Power Beam's total base velocity, before accounting for addon multipliers.
		/// </summary>
		float BeamBaseVelocity = 24f;
		/// <summary>
		/// The Power Beam's base critical strike chance, before accounting for addon multipliers.
		/// </summary>
		int BeamBaseCrit = 3;
		/// <summary>
		/// The Power Beam's base Overheat use, before accounting for addon multipliers.
		/// </summary>
		int BaseOverheat = 4;

		/// <summary>
		/// Contains all of the stats added by installed addons.
		/// <br/>Each index, in order:
		/// <br/><b>[0]</b> - Added base damage (convert to <b>int</b>)
		/// <br/><b>[1]</b> - Damage multiplier
		/// <br/><b>[2]</b> - Added base usetime (convert to <b>int</b>)
		/// <br/><b>[3]</b> - Usetime multiplier
		/// <br/><b>[4]</b> - Added base velocity (convert to <b>int</b>)
		/// <br/><b>[5]</b> - Velocity multiplier
		/// <br/><b>[6]</b> - Added crit chance (convert to <b>int</b>)
		/// <br/><b>[7]</b> - Added base overheat cost (convert to <b>int</b>)
		/// <br/><b>[8]</b> - Overheat cost multiplier
		/// <br/><b>[9]</b> - Added shot count (convert to <b>int</b>)
		/// </summary>
		public float[] AdditionalBeamStats = new float[10];
		///<summary>
		///Contains all of the stats added passively by addons installed in the Primary Quick-Swap.
		/// </summary>
		//public int[] AdditionalPrimaryStats = new int[5];

		/// <summary>
		/// The final overheat value, which will be calculated in UpdateInventory.<br/>
		/// It has to be out here because there's no baked-in variable like there is for damage/velocity/whatever
		/// </summary>
		int Overheat = 0;
		#endregion


		#region Missile Launcher stats
		/// <summary>
		/// the projectile type the missile launcher will fire.
		/// <br/><br/><b>MAY NOT BE NEEDED.</b> Worry about all this shit once Charge Beam is functional.
		/// </summary>
		int missileShot = ModContent.ProjectileType<MissileShot>();
		/// <summary>
		/// The Missile Launcher's base damage, before accounting for addons.
		/// </summary>
		int MissileBaseDamage = 32;
		/// <summary>
		/// The Missile Launcher's total damage multiplier from addons.
		/// </summary>
		float MissileDamageMult = 0f;
		/// <summary>
		/// The Missile Launcher's base usetime, before accounting for addons.
		/// </summary>
		int MissileBaseSpeed = 18;
		/// <summary>
		/// The Missile Launcher's total speed multiplier from addons.
		/// </summary>
		float MissileSpeedMult = 0f;
		/// <summary>
		/// The Missile Launcher's base velocity, before accounting for addons.
		/// </summary>
		int MissileBaseVelocity = 25;
		/// <summary>
		/// The Missile Launcher's total velocity multiplier from addons.
		/// </summary>
		float MissileVelocityMult = 0f;
		/// <summary>
		/// The Missile Launcher's base critical strike chance, before accounting for addons.
		/// </summary>
		int MissileBaseCrit = 3;
		/// <summary>
		/// The Missile Launcher's base Charge Combo cost, before accounting for addons.
		/// </summary>
		int BaseComboCost = 10;
		// I was gonna have just as many stats as the PB in here but
		// there's really only one missile addon that affects your base projectile
		// it really didn't end up being necessary I think
		#endregion

		public override void SetStaticDefaults()
		{
			//Below is how display text worked before localization hjsons
			//Their introduction made these obsolete but I'm keeping this here for posterity :)        -Z
			/* DisplayName.SetDefault("Power Beam");
			   Tooltip.SetDefault("Select this item in your hotbar and open your inventory to open the Beam Addon UI");*/
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() //obviously stats are set here
		{
			MGlobalItem ac = Item.GetGlobalItem<MGlobalItem>();
			if (VisualDinners == null)
			{
				VisualDinners = new int[4]; //it's a surprise tool that'll help us later
				VisualDinners = [-1, -1, 0, 0];
				HoldFireSlot = -1;
				//Used to get alternate beam textures.
				ac.assetModifier = "";
			}
			Item.width = 40;
			Item.height = 20;
			Item.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.value = 6969;
			Item.rare = ItemRarityID.Green;
			ac.maxMissiles = 5;
			ac.statMissiles = 5;
			ac.maxUA = 40;
			ac.statUA = 40;

			if (ac.isBeam) 
			{
				Item.damage = BeamBaseDamage;
				Item.useTime = BeamBaseSpeed;
				Item.useAnimation = BeamBaseSpeed;
				Item.UseSound = beamSound;
				Item.shoot = ModContent.ProjectileType<BeamShot>(); //Most of the cool shit happens on the projectile itself
				Item.shootSpeed = BeamBaseVelocity;
				Item.crit = BeamBaseCrit;
				Item.autoReuse = true;
			}//Power Beam default stats
			else 
			{
				Item.damage = MissileBaseDamage;
				Item.useTime = MissileBaseSpeed;
				Item.useAnimation = MissileBaseSpeed;
				Item.UseSound = missileSound;
				Item.shoot = ModContent.ProjectileType<MissileShot>(); //Most of the cool shit happens on the projectile itself
				Item.shootSpeed = MissileBaseVelocity;
				Item.crit = MissileBaseCrit;
				Item.autoReuse = false;
			}//Missile Launcher default stats

		}
		public override void UseStyle(Player player, Rectangle heldItemFrame) //makes the player's arm rotate with the arm cannon
		{
			Item.TryGetGlobalItem(out MGlobalItem mi);
			float armRot = player.itemRotation - (float)(Math.PI / 2) * player.direction;
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armRot);
			Vector2 origin = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, armRot);
			origin.Y -= heldItemFrame.Height / 2f;
			player.itemLocation = origin + player.itemRotation.ToRotationVector2() * (mi.isBeam ? -16 : -14) * player.direction;
		}

		public override bool CanUseItem(Player player) //lets things properly restrict your ability to use the weapon
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			return (player.whoAmI == Main.myPlayer && mp.statOverheat < mp.maxOverheat); //Add a suit lock check here later
		}
		#region Item visual methods
		private void SetTexture(MGlobalItem ac)
		{
			if (!ac.isBeam)
			{
				ac.itemTexture = ModContent.Request<Texture2D>(Texture + "Missile").Value;
			}
			else { ac.itemTexture = ac.itemTexture = ModContent.Request<Texture2D>(Texture).Value; }
		}

		public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			if (Item == null || !Item.TryGetGlobalItem(out MGlobalItem ac)) { return true; }
			Texture2D tex = Terraria.GameContent.TextureAssets.Item[Type].Value;
			SetTexture(ac);
			if (ac.itemTexture != null)
			{
				tex = ac.itemTexture;
			}
			float num5 = Item.height - tex.Height;
			float num6 = Item.width / 2 - tex.Width / 2;
			sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + (tex.Width / 2) + num6, Item.position.Y - Main.screenPosition.Y + (tex.Height / 2) + num5 + 2f),
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), alphaColor, rotation, new Vector2(tex.Width / 2, (tex.Height / 2)), scale, SpriteEffects.None, 0f);
			return false;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (Item == null || !Item.TryGetGlobalItem(out MGlobalItem ac)) { return true; }
			Texture2D tex = Terraria.GameContent.TextureAssets.Item[Type].Value;
			SetTexture(ac);
			if (ac.itemTexture != null)
			{
				tex = ac.itemTexture;
			}
			spriteBatch.Draw(tex, new Vector2(position.X + 2f, position.Y), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), drawColor, 0f, origin, scale + 0.2f, SpriteEffects.None, 0f);
			return false;
		}
		#endregion

		//"This is where the fun begins" -Anakin Skywalker
		#region The juicy stuff
		public override void UpdateInventory(Player p)
		{
			MPlayer mp = p.GetModPlayer<MPlayer>(); //finds the current player's MPlayer data for later modification
			if (Item == null || !Item.TryGetGlobalItem(out MGlobalItem ac)) { return; }
			
			Item.autoReuse = (ac.isBeam && HoldFireSlot == -1);
			//apply the numbers to the weapon
			if (ac.isBeam) //apply to power beam
			{
				Item.damage = (int)((BeamBaseDamage + (int)AdditionalBeamStats[0] /*+ AdditionalPrimaryStats[0]*/) * ((AdditionalBeamStats[1] / 100) + 1)); //Formula for power beam base damage calc. Has to convert to int to work
				Item.useAnimation = Item.useTime = (int)Math.Max(Math.Round((360 / ((BeamBaseSpeed + (int)AdditionalBeamStats[2] /*+ AdditionalPrimaryStats[1]*/) * ((AdditionalBeamStats[3] / 100) + 1)))), 2); //Usetime calc. Can't let the usetime drop below a certain point
				Item.shootSpeed = ((BeamBaseVelocity + (int)AdditionalBeamStats[4] /*+ AdditionalPrimaryStats[2]*/) * ((AdditionalBeamStats[5] / 100) + 1)); //Velocity calc. It adds 1 and divides by 100 so the values can be easy to read
				Item.crit = BeamBaseCrit + (int)AdditionalBeamStats[6] /*+ AdditionalPrimaryStats[3]*/;
				Overheat = (int)((BaseOverheat + (int)AdditionalBeamStats[7] /*+ AdditionalPrimaryStats[4]*/) * ((AdditionalBeamStats[8] / 100) + 1));
				Item.UseSound = beamSound;
				Item.shoot = ModContent.ProjectileType<BeamShot>();
			}
			else //go missile mode
			{
				Item.damage = MissileBaseDamage;
				Item.useTime = MissileBaseSpeed;
				Item.useAnimation = MissileBaseSpeed;
				Item.UseSound = missileSound;
				Item.shootSpeed = MissileBaseVelocity;
				Item.crit = MissileBaseCrit;
				Item.shoot = ModContent.ProjectileType<MissileShot>();
			}

		}
		/// <summary>
		/// Gets all the info from installed addons and applies it to the arm cannon. Done in a separate method to prevent it from running every tick.
		/// </summary>
		public void ArrayUpdate()
		{
			Item.TryGetGlobalItem(out MGlobalItem ac);
			VisualDinners = BeamAddonLoader.VisualPriority(beamAddons); //Gets the shot visuals

			AdditionalBeamStats = BeamAddonLoader.WeaponStatStacker(beamAddons); //Gets the beam stats

			//AdditionalPrimaryStats = BeamAddonLoader.ArrayStatGrabber(primaryQuickSwap); //Gets PQS passives (doesn't exist yet)




			#region Misc. Beamstacking
			//This is gonna get a little hard to read.

			//VisualDinners[0] is the winning ShapePriority, VisualDinners[1] is the winning ColorPriority
			if (VisualDinners[0] != -1) //Make sure there's actually stuff in the array
			{
				//Iterate through the currently-active addons in search of a holdfire suppressor
				//If none are found, iterate through both the currently-active addons and the array for active holdfires
				#region Holdfire checker
				MetroidMod.Instance.Logger.Info("Holdfire Checking Time, resetting HoldFireSlot now");
				HoldFireSlot = -1;
				/*if (VisualDinners[2] == 1)
				{

				}//Check if there's a VIB*/
				ModBeamAddon currentCheck = null; //gotta assign a value to this sucker or it'll throw a fit later


				for (int i = 0; i < BeamAddonSlotID.Count - 1 + chargeQuickSwap.Length; ++i)
				{
					if (i > 4)
					{
						currentCheck = BeamAddonLoader.GetAddon(chargeQuickSwap[i - (BeamAddonSlotID.Count - 1)]);
						MetroidMod.Instance.Logger.Info("Holdfire Check Loop" + (i + 1) + ", we're in the quick-swap with" + chargeQuickSwap[i - (BeamAddonSlotID.Count - 1)]);
						if (currentCheck != null && currentCheck.HoldFire)
						{
							MetroidMod.Instance.Logger.Info("Holdfire found.");
							HoldFireSlot = i;
							break;
						}
						MetroidMod.Instance.Logger.Info("Nada");
					}
					else
					{
						currentCheck = BeamAddonLoader.GetAddon(beamAddons[i]);
						MetroidMod.Instance.Logger.Info("Holdfire Check Loop " + (i + 1) + ", we're in the main array with" + beamAddons[i]);
						if (currentCheck != null && currentCheck.HoldFire)
						{
							MetroidMod.Instance.Logger.Info("Holdfire found.");
							HoldFireSlot = i;
							break;
						}
						MetroidMod.Instance.Logger.Info("Nada");
					}
				}//Get the active holdfire
				#endregion

				//Check if the shapepriority has any special visuals for this addon combination.
				//If not, it returns blank meaning no change.
				ac.assetModifier = BeamAddonLoader.GetAddon(beamAddons[VisualDinners[0]]).SetStaticCombos(beamAddons);

				//Get the modified shot sound effect.
				//This may look fucking godawful but I assure you this is all pretty much one line
				//Basically, it's asking the ShotSoundGrabber to get an asset based on the addon with sound effect privileges
				//(shapepriority if color override is off and naturally colorpriority if it's on)
				//and the current assetmodifier, determined a function ago (is that the right terminology???)
				beamSound = BeamAddonLoader.ShotSoundGrabber
					(BeamAddonLoader.GetAddon
						(beamAddons[VisualDinners[(VisualDinners[3] == 1) ? 1 : 0]]
						).ShotSound, ac.assetModifier, "", MetroidMod.BeamShotFallbackSFX
					);


			} //All the checks and shit for if there actually ARE addons in your arm cannon. Goes through holdfires, soundoverride, etc.



			else
			{
				beamSound = Sounds.Items.Weapons.PowerBeamSound;
				HoldFireSlot = -1;
			}


			if (missileAddons != null && !missileAddons[MissileAddonSlotID.Primary].IsAir) //Missiles don't need a VPS because only one slot changes your base projectile
			{
				missileSound = new SoundStyle(MissileAddonLoader.GetAddon(missileAddons[MissileAddonSlotID.Primary]).ShotSound);
			}
			else
			{
				missileSound = Sounds.Items.Weapons.MissileSound;
			}
			#endregion

			#region Missile Launcher stat calculation
			//MissileShot = missileAddons[1]
			#endregion
		}

		public override void HoldItem(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(); //finds the current player's MPlayer data for later modification
			MGlobalItem ac = Item.GetGlobalItem<MGlobalItem>();
			ac.showChargeBar = true; //lets the charge UI show up
			ac.showOnHand = true;

			if (MSystem.ACSwitch.JustPressed)
			{
				ac.isBeam = !ac.isBeam;
				SoundEngine.PlaySound(new SoundStyle("MetroidMod/Assets/Sounds/ArmCannon/WeaponSwitch"));
			} //Swap between beam and missiles when the keybind is pressed

			//the charge beam will have to bring a method in here in order for charging to work
			//why did I want two to be active at once again???
			//may have been a relic from when holdfire meant customfire
			//or maybe it's just me trying to make sure TTNE fire beam is possible again idfk
			//whatever, fire beam is kinda lame anyway, flamethrower is literally just it but better
			//...it still would be kinda cool though
			if (CanUseItem(player) && (HoldFireSlot != -1) && (player.HeldItem != null))
			{
				if (HoldFireSlot > BeamAddonSlotID.Count - 1)
				{
					BeamAddonLoader.GetAddon(chargeQuickSwap[HoldFireSlot - (BeamAddonSlotID.Count - 1)]).HoldFireBehavior(player);
				}
				else
				{
					BeamAddonLoader.GetAddon(beamAddons[HoldFireSlot]).HoldFireBehavior(player);
				}
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(); //finds the current player's MPlayer data for later modification
			MGlobalItem ac = Item.GetGlobalItem<MGlobalItem>();

			Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
			float speedX = velocity.X;
			float speedY = velocity.Y;

			if (ac.isBeam)
			{

				if (!ac.SuppressingFire)
				{
					SpawnBeam(player, source, position, velocity, type, damage, knockback);
				}
			} //Power Beam firing procedure
			else { return true; } //Missile Launcher firing procedure

			return false;
		}

		/// <summary>
		/// Used to fire beam projectiles.
		/// <br/>Comes with an optional string overload called <i>bonusFileMod</i> which gets appended to the shot's filemod. Used for things like charge shots.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="source"></param>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		/// <param name="type"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="bonusFileMod"></param>
		public void SpawnBeam(Player player, IEntitySource source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, string bonusFileMod = "")
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(); //finds the current player's MPlayer data for later modification
			MGlobalItem ac = Item.GetGlobalItem<MGlobalItem>();
			Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
			float speedX = velocity.X;
			float speedY = velocity.Y;
			int[] edgeCaseData = [0];
			int theShootsingAmount = (int)AdditionalBeamStats[9] + 1;
			MetroidMod.Instance.Logger.Info("Beam is firing. Cannon and Addons:\n" + Item + "\n" +
											BeamAddonAccess[0] + "\n" + BeamAddonAccess[1] + "\n" +
											BeamAddonAccess[2] + "\n" + BeamAddonAccess[3] + "\n" + BeamAddonAccess[4]);

			if (ac != null && ac.isBeam)
			{
				if (VisualDinners[0] != -1)
				{
					edgeCaseData = BeamAddonLoader.GetAddon(beamAddons[VisualDinners[0]]).SpecialComboGet(ac.assetModifier + bonusFileMod);
					//Add way to add an extra shot here. It's 100% possible
				}



				for (int i = 0; i < theShootsingAmount; i++)
				{
					BeamShot beam = (Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI).ModProjectile) as BeamShot;
					MetroidMod.Instance.Logger.Info("beam spawn");
					beam.VisualWinners = VisualDinners;
					if (VisualDinners[0] != -1)
					{
						//Need to BeamAddonLoader.GetAddon() the addons because the Arm Cannon stores the associated items and not the ModBeamAddons themselves
						//There's a chance it'd prolly be more efficient to store them as the addons but I've already set everything up this way so
						beam.ModTexture = BeamAddonLoader.ShotTextureGrabber(BeamAddonLoader.GetAddon(beamAddons[VisualDinners[0]]).ShotTexture, ac.assetModifier, bonusFileMod);
						beam.Impact = BeamAddonLoader.ShotSoundGrabber(BeamAddonLoader.GetAddon(beamAddons[VisualDinners[(VisualDinners[3] == 1) ? 1 : 0]]).ImpactSound, ac.assetModifier, bonusFileMod, MetroidMod.BeamImpactFallbackSFX);
						//Okay that last line was a bit of a mouthful but essentially what that says:
						//It's attempting to set the beam impact sfx to an addon's impact sfx given the filemods.
						//If SoundOverride (VisualDinners[3]) is on, that addon is the ColorPriority, and if not, it's the ShapePriority
					}

					//The way shot textures are grabbed, explained in detail:
					//Assets are stored in BeamAddons/BeamAddonName
					//Basic shots are all named Shot
					//In order to make alternate textures modular, the textures for specific edge-cases take the standard name and append modifiers to it
					//(e.g. a charge shot should be named ShotCharged

					if (edgeCaseData[0] > 0)
					{
						beam.ShotFrames = edgeCaseData[0];
					}
					
					beam.fileMod += (ac.assetModifier + bonusFileMod);

					beam.beamAddons = BeamAddonAccess
						.Select(i => BeamAddonLoader.GetAddon(i))
						.ToArray();
					//MetroidMod.Instance.Logger.Info("New beam drawn.\nTexture path: " + (VisualDinners[0] != -1)? beam.beamAddons[beam.VisualWinners[0]].ShotTexture : "N/A" + "\nModifier stack: >" + beam.fileMod + "<");

					mp.statOverheat += MGlobalItem.AmmoUsage(player, Overheat * mp.overheatCost);
					mp.overheatDelay = (int)Math.Max(Item.useTime - 10, 2);
				}
			}
		}
		#endregion
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(8)
				//.AddIngredient<Tiles.MissileExpansion>(1)
				.AddIngredient<Miscellaneous.EnergyShard>(3)
				.AddTile(TileID.Anvils)
				.Register();
		}

		#region Data Preservation

		//This is to prevent arrays from being null on creation
		public override void OnCreated(ItemCreationContext context)
		{
			base.OnCreated(context);
			beamAddons = new Item[BeamAddonSlotID.Count];
			for (int i = 0; i < beamAddons.Length; ++i)
			{
				beamAddons[i] = new Item();
				beamAddons[i].TurnToAir();
			}
			chargeQuickSwap = new Item[8];
			for (int i = 0; i < 8; ++i)
			{
				chargeQuickSwap[i] = new Item();
				chargeQuickSwap[i].TurnToAir();
			}
			missileAddons = new Item[MissileAddonSlotID.Count];
			for(int i = 0;i < missileAddons.Length; ++i)
			{
				missileAddons[i] = new Item();
				missileAddons[i].TurnToAir();
			}
			comboQuickChange = new Item[8];
			for (int i = 0;i < 8; ++i)
			{
				comboQuickChange[i] = new Item();
				comboQuickChange[i].TurnToAir();
			}
		}
		public override ModItem Clone(Item newEntity)
		{
			//Make sure the clone has all the same addons as the original
			ArmCannon clone = (ArmCannon)base.Clone(newEntity);
			clone.beamAddons = new Item[BeamAddonSlotID.Count];
			clone.chargeQuickSwap = new Item[8];
			clone.missileAddons = new Item[MissileAddonSlotID.Count];
			clone.comboQuickChange = new Item[8];

			#region Beam Addon cloning
			for (int i = 0; i < BeamAddonSlotID.Count; ++i)
			{
				if (beamAddons == null || beamAddons[i] == null)
				{
					clone.beamAddons[i] = new Item();
					clone.beamAddons[i].TurnToAir();
				}
				else { clone.beamAddons[i] = beamAddons[i]; }
			}
			#endregion
			#region Charge Quick-Swap cloning
				for (int i = 0; i < (8); ++i)
				{
					if (chargeQuickSwap == null || chargeQuickSwap[i] == null)
					{
						clone.chargeQuickSwap[i] = new Item();
						clone.chargeQuickSwap[i].TurnToAir();
					}
					else { clone.chargeQuickSwap[i] = chargeQuickSwap[i]; }
				}
			#endregion
			#region Missile Addon cloning
			for (int i = 0; i < MissileAddonSlotID.Count; ++i)
			{
				if (missileAddons == null || missileAddons[i] == null)
				{
					clone.missileAddons[i] = new Item();
					clone.missileAddons[i].TurnToAir();
				}
				else { clone.missileAddons[i] = missileAddons[i]; }
			}
			#endregion
			#region Charge Combo Quick-Swap cloning
				for (int i = 0; (i < 8); ++i)
				{
					if (comboQuickChange == null || comboQuickChange[i] == null)
					{
						clone.comboQuickChange[i] = new Item();
						clone.comboQuickChange[i].TurnToAir();
					}
					else { clone.comboQuickChange[i] = comboQuickChange[i]; }
				}
			#endregion
			return clone;
		}
		public override void OnResearched(bool fullyResearched)
		{
			//If the player researches the arm cannon, puke out all the addons

			foreach (Item item in beamAddons)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			} //beam addons

			foreach (Item item in chargeQuickSwap)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			} //charge quick swap

			foreach (Item item in missileAddons)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			} //missile addons

			foreach (Item item in comboQuickChange)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			} //charge combo quick swap
		}

		public override void SaveData(TagCompound tag)
		{
			#region addons
			//Normal beam addons
			for (int i = 0; i < BeamAddonSlotID.Count; ++i)
			{
				//Failsafe check
				if (beamAddons[i] == null)
				{
					beamAddons[i] = new Item();
				}
				tag.Add("Beam Addon - Slot " + (i + 1), ItemIO.Save(beamAddons[i]));

			}
			//Charge Quick-Swap
			for (int i = 0; i < 8; ++i)
			{
				//Failsafe check
				if (chargeQuickSwap[i] == null)
				{
					chargeQuickSwap[i] = new Item();
				}
				tag.Add("Primary Quick-Swap - Slot " + (i + 1), ItemIO.Save(chargeQuickSwap[i]));
			}
			//Normal missile addons
			for (int i = 0; i < MissileAddonSlotID.Count; ++i)
			{
				//Failsafe check
				if (missileAddons[i] == null)
				{
					missileAddons[i] = new Item();
				}
				tag.Add("Missile Addon - Slot " + (i + 1), ItemIO.Save(missileAddons[i]));
			}
			//Combo Quick-Swap
			for (int i = 0; i < 8; ++i)
			{
				//Failsafe check
				if (comboQuickChange[i] == null)
				{
					comboQuickChange[i] = new Item();
				}
				tag.Add("Charge Combo Quick-Swap - Slot " + (i + 1), ItemIO.Save(comboQuickChange[i]));
			}
			#endregion
			//ammo
			if (Item.TryGetGlobalItem(out MGlobalItem ac))
			{
				tag.Add("Maximum UA", ac.maxUA);
				tag.Add("Current UA", ac.statUA);
				tag.Add("Maximum Missiles", ac.maxMissiles);
				tag.Add("Current Missiles", ac.statMissiles);
			}
		}
		public override void LoadData(TagCompound tag)
		{
			try
			{
				#region addons
				beamAddons = new Item[BeamAddonSlotID.Count];
				for (int i = 0; i < beamAddons.Length; i++)
				{
					Item item = tag.Get<Item>("Beam Addon - Slot " + (i + 1));
					beamAddons[i] = item;
				}
				chargeQuickSwap = new Item[8];
				for (int i = 0; i < 8; i++)
				{
					Item item = tag.Get<Item>("Primary Quick-Swap - Slot " + (i + 1));
					chargeQuickSwap[i] = item;
				}
				missileAddons = new Item[MissileAddonSlotID.Count];
				for (int i = 0; i < missileAddons.Length; i++)
				{
					Item item = tag.Get<Item>("Missile Addon - Slot " + (i + 1));
					missileAddons[i] = item;
				}
				comboQuickChange = new Item[8];
				for (int i = 0; i < 8; i++)
				{
					Item item = tag.Get<Item>("Charge Combo Quick-Swap - Slot " + (i + 1));
					comboQuickChange[i] = item;
				}
				#endregion
				MGlobalItem ac = Item.GetGlobalItem<MGlobalItem>();
				ac.maxUA = tag.GetInt("Maximum UA");
				ac.statUA = tag.GetFloat("Current UA");
				ac.maxMissiles = tag.GetInt("Maximum Missiles");
				ac.statMissiles = tag.GetInt("Current Missiles");
			}
			catch { }
			ArrayUpdate();
		}
		#endregion
	}
}
