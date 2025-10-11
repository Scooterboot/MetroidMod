using System;
using System.Linq;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Common.Systems;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
					chargeQuickSwap = new Item[MetroidMod.beamChangeSlotAmount];
					for (int i = 0; i < chargeQuickSwap.Length; ++i)
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
				if (missileAddons == null) //see BeamAddonAccess above
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
				if (comboQuickChange == null) //See BeamArrayAccess above
				{
					comboQuickChange = new Item[MetroidMod.missileChangeSlotAmount];
					for (int i = 0; i < comboQuickChange.Length; ++i)
					{
						comboQuickChange[i] = new Item();
						comboQuickChange[i].TurnToAir();
					}
				}
				return comboQuickChange;
			}
			set { comboQuickChange = value; }
		}
		#endregion


		#region Data pointers
		/// <summary>
		/// Keeps track of the addons that were selected by the <b>Visual Priority System</b>.
		/// <br/><br/><i>(VisualWinners was taken by <see cref="BeamShot"/>)</i>
		/// </summary>
		public int[] VisualDinners;
		/// <summary>
		/// Keeps track of the slot containing the currently-active holdfire. -1 means no holdfire.
		/// </summary>
		public int HoldFireSlot;

		/// <summary>
		/// The slot in the Beam Array that the beam Charge slot is currently mapped to.
		/// <br/>Defaults to <b>0</b>.
		/// </summary>
		public int activeBeamArraySlot = 0;
		/// <summary>
		/// The slot in the Charge Combo Array that the missile Charge slot is currently mapped to.
		/// <br/>Defaults to <b>0</b>.
		/// </summary>
		public int activeMissileArray = 0;

		public SoundStyle beamSound = Sounds.Items.Weapons.PowerBeamSound;
		public SoundStyle missileSound = Sounds.Items.Weapons.MissileShoot;
		#endregion

		#region stats

		#region Power Beam stats

		/// <summary>
		/// The Power Beam's base damage, before accounting for addon multipliers.
		/// </summary>
		private readonly int BeamBaseDamage = 10;
		/// <summary>
		/// The Power Beam's base usetime, before accounting for addon multipliers.
		/// </summary>
		private readonly int BeamBaseSpeed = 12;
		/// <summary>
		/// The Power Beam's total base velocity, before accounting for addon multipliers.
		/// </summary>
		private readonly float BeamBaseVelocity = 18f;
		/// <summary>
		/// The Power Beam's base critical strike chance, before accounting for addon multipliers.
		/// </summary>
		private readonly int BeamBaseCrit = 3;
		/// <summary>
		/// The Power Beam's base Overheat use, before accounting for addon multipliers.
		/// </summary>
		private readonly int BaseOverheat = 4;

		/// <summary>
		/// The final overheat value, which will be calculated in UpdateInventory.<br/>
		/// It has to be out here because there's no baked-in variable like there is for damage/velocity/whatever
		/// </summary>
		private int Overheat = 0;
		#endregion


		#region Missile Launcher stats
		/// <summary>
		/// the projectile type the missile launcher will fire.
		/// <br/><br/><b>MAY NOT BE NEEDED.</b> Worry about all this shit once Charge Beam is functional.
		/// </summary>
		private int missileShot = ModContent.ProjectileType<MissileShot>();
		/// <summary>
		/// The Missile Launcher's base damage, before accounting for addons.
		/// </summary>
		private readonly int MissileBaseDamage = 32;
		/// <summary>
		/// The Missile Launcher's total damage multiplier from addons.
		/// </summary>
		private readonly float MissileDamageMult = 0f;
		/// <summary>
		/// The Missile Launcher's base usetime, before accounting for addons.
		/// </summary>
		private readonly int MissileBaseSpeed = 18;
		/// <summary>
		/// The Missile Launcher's total speed multiplier from addons.
		/// </summary>
		private readonly float MissileSpeedMult = 0f;
		/// <summary>
		/// The Missile Launcher's base velocity, before accounting for addons.
		/// </summary>
		private readonly float MissileBaseVelocity = 8f;
		/// <summary>
		/// The Missile Launcher's total velocity multiplier from addons.
		/// </summary>
		private readonly float MissileVelocityMult = 0f;
		/// <summary>
		/// The Missile Launcher's base critical strike chance, before accounting for addons.
		/// </summary>
		private readonly int MissileBaseCrit = 3;
		/// <summary>
		/// The Missile Launcher's base Charge Combo cost, before accounting for addons.
		/// </summary>
		private readonly int BaseComboCost = 10;
		// I was gonna have just as many stats as the PB in here but
		// there's really only one missile addon that affects your base projectile
		// it really didn't end up being necessary I think
		#endregion

		//TODO: See about converting these into StatModifiers?

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
		/// <summary>
		/// Contains all of the stats added by installed addons.
		/// <br/>Each index, in order:
		/// <br/><b>[0]</b> - Added base damage (convert to <b>int</b>)
		/// <br/><b>[1]</b> - Damage multiplier
		/// <br/><b>[2]</b> - Added base usetime (convert to <b>int</b>)
		/// <br/><b>[3]</b> - Usetime multiplier
		///</summary>
		public float[] AdditionalMissileStats = new float[5];
		///<summary>
		///Contains all of the stats added passively by addons installed in the Primary Quick-Swap.
		/// </summary>
		public float[] AdditionalPrimaryStats = new float[5];
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
			ac.barrelOffset = 20f;

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
			}//Power Beam default stat assignment
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
			}//Missile Launcher default stat assignment
		}
		public override void UseStyle(Player player, Rectangle heldItemFrame) //makes the player's arm rotate with the arm cannon
		{
			Item.TryGetGlobalItem(out MGlobalItem mi);
			float armRot = player.itemRotation - ((float)(Math.PI / 2) * player.direction);
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, player.gravDir < 0 ? MathHelper.Pi - armRot : armRot);
			Vector2 origin = SetCannonPos(player, armRot);
			origin.Y -= heldItemFrame.Height / 2f;
			player.itemLocation = origin + (player.itemRotation.ToRotationVector2() * -20 * player.direction);
		}
		private Vector2 SetCannonPos(Player player, float rotation)
		{
			float num = rotation + MathHelper.PiOver2;
			Vector2 vector = new Vector2((float)Math.Cos((double)num), (float)Math.Sin((double)num));
			vector *= 10f;

			vector += new Vector2(-4f * player.direction, -2f * player.gravDir);
			vector += new Vector2(0f, 3f * player.direction * player.gravDir).RotatedBy((double)(rotation + MathHelper.PiOver2), default(Vector2));

			return player.MountedCenter + vector;
		}

		public override bool CanUseItem(Player player) //lets things properly restrict your ability to use the weapon
		{
			//MPlayer mp = player.GetModPlayer<MPlayer>();
			if (Item == null || !Item.TryGetGlobalItem(out MGlobalItem ac) || ac == null || !player.TryGetModPlayer(out MPlayer mp)) { return false; }
			if (ac.isBeam)
			{
				return player.whoAmI == Main.myPlayer && mp.statOverheat < mp.maxOverheat; //Add a suit lock check here later (as well as missile --DR);
			}
			else
				return ac.statMissiles > 0;
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
			float num6 = (Item.width / 2) - (tex.Width / 2);
			sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + (tex.Width / 2) + num6, Item.position.Y - Main.screenPosition.Y + (tex.Height / 2) + num5 + 2f),
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), alphaColor, rotation, new Vector2(tex.Width / 2, tex.Height / 2), scale, SpriteEffects.None, 0f);
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
		{   //MPlayer mp = player.GetModPlayer<MPlayer>();//finds the current player's MPlayer data for later modification
			if (Item == null || !Item.TryGetGlobalItem(out MGlobalItem ac) || ac == null || !p.TryGetModPlayer(out MPlayer mp)) { return; }

			Item.autoReuse = ac.isBeam && HoldFireSlot == -1;
			//apply the numbers to the weapon
			if (ac.isBeam) //apply to power beam
			{
				Item.damage = (int)((int)(BeamBaseDamage + AdditionalBeamStats[0] + AdditionalPrimaryStats[0]) * ((AdditionalBeamStats[1] / 100) + 1)); //Formula for power beam base damage calc. Has to convert to int to work
				Item.useAnimation = Item.useTime = (int)Math.Max(Math.Round(360 / ((BeamBaseSpeed + AdditionalBeamStats[2] + AdditionalPrimaryStats[1]) * ((AdditionalBeamStats[3] / 100) + 1))), 2); //Usetime calc. Can't let the usetime drop below a certain point
				Item.shootSpeed = (BeamBaseVelocity + AdditionalBeamStats[4] /*+ AdditionalPrimaryStats[2]*/) * ((AdditionalBeamStats[5] / 100f) + 1f); //Velocity calc. It adds 1 and divides by 100 so the values can be easy to read
				Item.crit = (int)(BeamBaseCrit + AdditionalBeamStats[6] + AdditionalPrimaryStats[3]);
				Overheat = (int)((BaseOverheat + AdditionalBeamStats[7] + AdditionalPrimaryStats[4]) * ((AdditionalBeamStats[8] / 100) + 1));
				Item.UseSound = beamSound;
				Item.shoot = ModContent.ProjectileType<BeamShot>();
			}
			else //go missile mode
			{
				Item.damage = (int)((MissileBaseDamage + AdditionalMissileStats[0]) * ((AdditionalMissileStats[1] / 100) + 1)); //Formula for power Missile base damage calc. Has to convert to int to work
				Item.useAnimation = Item.useTime = (int)Math.Max(Math.Round(360 / ((MissileBaseSpeed + AdditionalMissileStats[2]) * ((AdditionalMissileStats[3] / 100) + 1))), 2); //Usetime calc. Can't let the usetime drop below a certain point
				Item.shootSpeed = MissileBaseVelocity;
				Item.crit = MissileBaseCrit;
				Item.UseSound = missileSound;
				Item.shoot = missileAddons[MissileAddonSlotID.Primary].IsAir
					? ModContent.ProjectileType<MissileShot>() //fallback if no missile addon (may consider revoking?	-Z)
					: missileShot;
			}
		}
		/// <summary>
		/// Gets all the pre-shot info from installed addons and applies it to the arm cannon.
		/// <br/>Done in a separate method to prevent it from running every tick.
		/// </summary>
		public void ArrayUpdate()
		{
			Item.TryGetGlobalItem(out MGlobalItem ac);

			VisualDinners = BeamAddonLoader.VisualPriority(beamAddons); //Gets the shot visuals and checks for VIBs

			AdditionalBeamStats = BeamAddonLoader.WeaponStatStacker(beamAddons); //Gets the beam stats
			AdditionalMissileStats = MissileAddonLoader.WeaponStatStacker(missileAddons);


			chargeQuickSwap[activeBeamArraySlot] = beamAddons[0];

			AdditionalPrimaryStats = BeamAddonLoader.ArrayStatGrabber(chargeQuickSwap, activeBeamArraySlot); //Gets PQS passives (doesn't exist yet)

			#region Misc. Beamstacking
			//This is gonna get a little hard to read.

			//VisualDinners[0] is the winning ShapePriority, VisualDinners[1] is the winning ColorPriority
			if (VisualDinners[0] != -1) //This makes sure there's actually stuff in the array
			{

				//Suitlock checker goes here

				//Compatibility checker goes here

				#region Holdfire Checker
				//Originally this was pretty much just to check for holdfires, but this is the perfect place to do a shitton of stuff

				//Initialize important variables
				ModBeamAddon currentCheck = null; //gotta assign a value to this sucker or it'll throw a fit later //but do you tho? --DR
				bool HelpImBeingSuppressed = false; //no basis for a form of government			-Z
				HoldFireSlot = -1;

				if (VisualDinners[2] == 2)
				{
					ac.SuppressingFire = true;
					HelpImBeingSuppressed = true;
				}//Check if there's bespoke VIB projectile
				else
				{
					ac.SuppressingFire = false;
				}


				//First, check for holdfire suppressors.

				MetroidMod.Instance.Logger.Info("Checking the anarcho-cynicallist commune for supreme executive power");
				if (!HelpImBeingSuppressed)
				{
					for (int i = 0; i < BeamAddonSlotID.Count - 1; ++i)
					{
						currentCheck = BeamAddonLoader.GetAddon(beamAddons[i]);
						if (currentCheck != null && currentCheck.SuppressHoldFire)
						{
							MetroidMod.Instance.Logger.Info("Slot " + i + " had a sword thrown at them by a watery tart");
							HelpImBeingSuppressed = true;
							break;
						}
						//MetroidMod.Instance.Logger.Info("Nothing on " + i);
					}
				}

				//Holdfire checker.
				//Goes through all installed addons (including quick-swap) to check for holdfires.
				//Only runs if no holdfire suppressors are installed in the main array.
				MetroidMod.Instance.Logger.Info("Holdfire Checking Time");

				if (!HelpImBeingSuppressed)
				{
					currentCheck = null; //Clear value for next check
					for (int i = 0; i < BeamAddonSlotID.Count - 1 + chargeQuickSwap.Length; ++i)
					{
						if (i > 4) //Quickswap Array
						{
							currentCheck = BeamAddonLoader.GetAddon(chargeQuickSwap[i - (BeamAddonSlotID.Count - 1)]);
							//MetroidMod.Instance.Logger.Info("Holdfire Check Loop" + (i + 1) + ", we're in the quick-swap with" + chargeQuickSwap[i - (BeamAddonSlotID.Count - 1)]);
							if (currentCheck != null && currentCheck.HoldFire)
							{
								MetroidMod.Instance.Logger.Info("Holdfire found at quick-swap slot " + (i - 3) + ".");
								HoldFireSlot = i;
								break;
							}
							//MetroidMod.Instance.Logger.Info("Nada");
						}
						else //Normal Addons
						{
							currentCheck = BeamAddonLoader.GetAddon(beamAddons[i]);
							//MetroidMod.Instance.Logger.Info("Holdfire Check Loop " + (i + 1) + ", we're in the main array with" + beamAddons[i]);
							if (currentCheck != null && currentCheck.HoldFire)
							{
								MetroidMod.Instance.Logger.Info("Holdfire found at slot " + (1 + 1) + ".");
								HoldFireSlot = i;
								break;
							}
							//MetroidMod.Instance.Logger.Info("Nada");
						}
					}//Get the active holdfire
					if (HoldFireSlot == -1) { MetroidMod.Instance.Logger.Info("No holdfires found"); }
				}

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
				ac.SuppressingFire = false;
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

			#region Missile Launcher
			if (!missileAddons[MissileAddonSlotID.Primary].IsAir)
			{
				missileShot = MissileAddonLoader.GetAddon(missileAddons[MissileAddonSlotID.Primary]).ProjectileType;
			}
			#endregion

			//TODO: Netsync thing here
			Item.NetStateChanged();

			ac.barrelOffset = 20f;
		}

		public override void HoldItem(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(); //finds the current player's MPlayer data for later modification
			MGlobalItem ac = Item.GetGlobalItem<MGlobalItem>();
			ac.showChargeBar = true;
			ac.showOnHand = true;

			if (MSystem.ACSwitch.JustPressed && player.itemTime <= 0)
			{
				ac.isBeam = !ac.isBeam;
				SoundEngine.PlaySound(new SoundStyle("MetroidMod/Assets/Sounds/ArmCannon/WeaponSwitch"));
			} //Swap between beam and missiles when the keybind is pressed

			//the charge beam will have to bring a method in here in order for charging to work
			if (CanUseItem(player) && (HoldFireSlot != -1) && (player.HeldItem.type == ModContent.ItemType<ArmCannon>()) && (ac.isBeam || (!ac.isBeam && missileAddons[MissileAddonSlotID.Charge] != null)))
			{
				//note: if charge combos depend on charge and charge is being overridden by a different holdfire
				//that'll make it so you can't shoot charge combos while it's equipped
				//Look into later			-Z
				if (HoldFireSlot >= BeamAddonSlotID.Count - 1)
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
				//if (ac.SuppressingFire) { return false; }
				//if (ac.SuppressingFire && VisualDinners[2] != 2) { return false; }
				if (VisualDinners[2] == 2 && ac.SuppressingFire)
				{
					MetroidMod.Instance.Logger.Info("TASTE THE RAINBOW MOTHERFUCKER");
					BeamAddonLoader.GetAddon(beamAddons[VisualDinners[0]]).VIBShoot(Item, player, source, position, velocity, type, damage, knockback);
					mp.statOverheat += MGlobalItem.AmmoUsage(player, Overheat * mp.overheatCost);
					mp.overheatDelay = Math.Max(Item.useTime - 10, 2);
				}
				else if (ac.SuppressingFire) { return false; }
				else
				{
					SpawnBeam(player, source, position, velocity, type, damage, knockback);
				}
			} //Power Beam firing procedure
			else
			{
				Launch(player, source, position, velocity, type, damage, knockback);
			} //Missile Launcher firing procedure
			return false;
		}

		/// <summary>
		/// Used to fire beam projectiles.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="source"></param>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		/// <param name="type"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="bonusFileMod">Appended to the shot's filemod for on-the-fly modifications.
		/// <br/>Things like charge shots take advantage of this.</param>
		/// <param name="multiplier">Allows for on-the-fly modifying of the Interact values.</param>
		public void SpawnBeam(Player player, IEntitySource source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, string bonusFileMod = "", float multiplier = 1f)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(); //finds the current player's MPlayer data for later modification
			MGlobalItem ac = Item.GetGlobalItem<MGlobalItem>();
			Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
			float speedX = velocity.X;
			float speedY = velocity.Y;
			int[] visualData = [0, -1];
			float[] edgeCaseStuff = [0, 0, 0, 0, 0];
			int theShootsingAmount = (int)AdditionalBeamStats[9] + 1;
			MetroidMod.Instance.Logger.Info("Beam is firing. Cannon and Addons:\n" + Item + "\n" +
											BeamAddonAccess[0] + "\n" + BeamAddonAccess[1] + "\n" +
											beamAddons[2] + "\n" + BeamAddonAccess[3] + "\n" + BeamAddonAccess[4]);

			if (ac != null && ac.isBeam)
			{
				if (VisualDinners[0] != -1)
				{
					visualData = BeamAddonLoader.GetAddon(beamAddons[VisualDinners[0]]).ComboVisualsGet(ac.assetModifier + bonusFileMod);
				}
				//MetroidMod.Instance.Logger.Info("Wave Beam Bullshit Time");
				edgeCaseStuff = BeamAddonLoader.EdgeCaseStacker(beamAddons, AdditionalBeamStats, bonusFileMod);
				AdditionalBeamStats[1] += edgeCaseStuff[0];
				AdditionalBeamStats[3] += edgeCaseStuff[1];
				AdditionalBeamStats[5] += edgeCaseStuff[2];
				AdditionalBeamStats[8] += edgeCaseStuff[3];
				theShootsingAmount += (int)edgeCaseStuff[4];
				//need way to slap on a bonus projectile here.


				for (int i = 0; i < theShootsingAmount; i++) //Assign i's value to projectile & include shootsingamount in there too
				{
					BeamShot beam = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI).ModProjectile as BeamShot;
					MetroidMod.Instance.Logger.Info("beam spawn || " + (i + 1) + " " + theShootsingAmount + " || " + source);
					beam.VisualWinners = VisualDinners;
					if (VisualDinners[0] != -1)
					{
						//Need to BeamAddonLoader.GetAddon() the addons because the Arm Cannon stores the associated items and not the ModBeamAddons themselves
						//There's a chance it'd prolly be more efficient to store them as the addons but I've already set everything up this way so
						beam.ModTexture = BeamAddonLoader.ShotTextureGrabber(BeamAddonLoader.GetAddon(beamAddons[VisualDinners[0]]).ShotTexture, ac.assetModifier, bonusFileMod);
						beam.beamDust = (visualData[1] < 0) ? BeamAddonLoader.GetAddon(beamAddons[VisualDinners[1]]).ShotDust : visualData[1];
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

					//TODO: Character limit on modifiers? Don't want someone to make a 5000000000000 letter long one

					if (visualData[0] > 0)
					{
						beam.ShotFrames = visualData[0];
					}

					beam.groupSize = theShootsingAmount;
					beam.groupID = i;

					beam.fileMod += ac.assetModifier + bonusFileMod;

					beam.beamAddons = beamAddons
						.Select(i => BeamAddonLoader.GetAddon(i))
						.Select(i => i?.Clone())
						.ToArray();

					beam.OnInitialized(source);
					mp.statOverheat += MGlobalItem.AmmoUsage(player, Overheat * mp.overheatCost);
					mp.overheatDelay = Math.Max(Item.useTime - 10, 2);
				}
			}
		}
		/// <summary>
		/// Used to fire missile projectiles.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="source"></param>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		/// <param name="type"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="bonusFileMod">Appended to the shot's filemod for on-the-fly modifications.
		/// <br/>Things like charge shots take advantage of this.</param>
		/// <param name="multiplier">Allows for on-the-fly modifying of the Interact values.</param>
		public void Launch(Player player, IEntitySource source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, bool isCharged = false)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(); //finds the current player's MPlayer data for later modification
			MGlobalItem ac = Item.GetGlobalItem<MGlobalItem>();
			float[] edgeCaseStuff;
			if (ac != null && !ac.isBeam)
			{
				if (isCharged && MissileAddonLoader.GetAddon(missileAddons[MissileAddonSlotID.Charge]).IgnoreProjectile)
				{
					MProjectile miss = (MProjectile)Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback).ModProjectile;
					miss.Override = MissileAddonLoader.GetAddon(missileAddons[MissileAddonSlotID.Charge]);
				}
				else if (isCharged)
				{
					Projectile.NewProjectileDirect(source, position, velocity, MissileAddonLoader.GetAddon(missileAddons[MissileAddonSlotID.Charge]).Projectile.type, damage, knockback);
				}
				else
				{
					MProjectile miss = (MProjectile)Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback).ModProjectile;
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
			for (int i = 0; i < missileAddons.Length; ++i)
			{
				missileAddons[i] = new Item();
				missileAddons[i].TurnToAir();
			}
			comboQuickChange = new Item[8];
			for (int i = 0; i < 8; ++i)
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
			for (int i = 0; i < 8; ++i)
			{
				if (chargeQuickSwap == null || chargeQuickSwap[i] == null)
				{
					clone.chargeQuickSwap[i] = new Item();
					clone.chargeQuickSwap[i].TurnToAir();
				}
				else { clone.chargeQuickSwap[i] = chargeQuickSwap[i]; }
			}
			clone.activeBeamArraySlot = activeBeamArraySlot;
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
			for (int i = 0; i < 8; ++i)
			{
				if (comboQuickChange == null || comboQuickChange[i] == null)
				{
					clone.comboQuickChange[i] = new Item();
					clone.comboQuickChange[i].TurnToAir();
				}
				else { clone.comboQuickChange[i] = comboQuickChange[i]; }
			}
			clone.activeMissileArray = activeMissileArray;
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
		}//TODO this dumps addons galore with hyperresearch iirc --DR

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
			tag.Add("Selected Primary Quick-Swap Slot", activeBeamArraySlot);
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
			tag.Add("Selected Combo Quick-Swap Slot", activeMissileArray);
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
				activeBeamArraySlot = tag.GetInt("Selected Primary Quick-Swap Slot");
				activeMissileArray = tag.GetInt("Selected Combo Quick-Swap Slot");
			}
			catch { }
			ArrayUpdate();
		}
		#endregion
	}
}
