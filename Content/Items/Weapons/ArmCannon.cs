using System;
using System.Collections.Generic;
using System.IO;
using MetroidMod.Common.Configs;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Items.MissileAddons;
using MetroidMod.Content.Items.MissileAddons.BeamCombos;
using MetroidMod.Content.Projectiles;
using MetroidMod.Content.Projectiles.hyperbeam;
using MetroidMod.Content.Projectiles.missilecombo;
using MetroidMod.Content.Projectiles.missiles;
using MetroidMod.Content.Projectiles.powerbeam;
using MetroidMod.Content.Projectiles.VoltDriver;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI.Chat;
using Terraria.Utilities;

namespace MetroidMod.Content.Items.Weapons
{
	public class ArmCannon : ModItem
	{
		// Failsaves.
		private Item[] _beamMods;
		private Item[] _beamchangeMods;
		private bool isHunter = false;
		public Item[] BeamMods
		{
			get {
				if (_beamMods == null)
				{
					_beamMods = new Item[BeamAddonSlotID.Count];
					for (int i = 0; i < _beamMods.Length; ++i)
					{
						_beamMods[i] = new Item();
						_beamMods[i].TurnToAir();
					}
				}

				return _beamMods;
			}
			set { _beamMods = value; }
		}
		public Item[] BeamChange
		{
			get {
				if (_beamchangeMods == null)
				{
					_beamchangeMods = new Item[BeamChangeSlotID.Count];
					for (int i = 0; i < _beamchangeMods.Length; ++i)
					{
						_beamchangeMods[i] = new Item();
						_beamchangeMods[i].TurnToAir();
					}
				}

				return _beamchangeMods;
			}
			set { _beamchangeMods = value; }
		}
		private Item[] _missileMods;
		private Item[] _missileChange;
		public Item[] MissileMods
		{
			get {
				if (_missileMods == null)
				{
					_missileMods = new Item[MetroidMod.missileSlotAmount];
					for (int i = 0; i < _missileMods.Length; ++i)
					{
						_missileMods[i] = new Item();
						_missileMods[i].TurnToAir();
					}
				}

				return _missileMods;
			}
			set { _missileMods = value; }
		}
		public Item[] MissileChange
		{
			get {
				if (_missileChange == null)
				{
					_missileChange = new Item[13];
					for (int i = 0; i < _missileChange.Length; ++i)
					{
						_missileChange[i] = new Item();
						_missileChange[i].TurnToAir();
					}
				}

				return _missileChange;
			}
			set { _missileChange = value; }
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Power Beam");
			// Tooltip.SetDefault("Select this item in your hotbar and open your inventory to open the Beam Addon UI");
			Item.ResearchUnlockCount = 1;

			BeamMods = new Item[BeamAddonSlotID.Count];
			BeamChange = new Item[BeamChangeSlotID.Count];
		}
		public override void SetDefaults()
		{
			MGlobalItem pb = Item.GetGlobalItem<MGlobalItem>();
			if (pb.isBeam) 
			{
				Item.damage = MConfigItems.Instance.damagePowerBeam;
				Item.DamageType = ModContent.GetInstance<HunterDamageClass>();
				Item.width = 24;
				Item.height = 12;
				Item.scale = 0.8f;
				Item.useTime = MConfigItems.Instance.useTimePowerBeam;
				Item.useAnimation = 14;
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noMelee = true;
				Item.knockBack = 4f;
				Item.value = 20000;
				Item.rare = ItemRarityID.Green;
				Item.UseSound = ShotSound;//mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/PowerBeamSound");
				Item.shoot = ModContent.ProjectileType<PowerBeamShot>();
				Item.shootSpeed = 8f;
				Item.crit = 3;
			}
			else
			{
				Item.damage = Common.Configs.MConfigItems.Instance.damageMissileLauncher;
				Item.DamageType = ModContent.GetInstance<HunterDamageClass>();
				Item.width = 24;
				Item.height = 16;
				Item.scale = 0.8f;
				Item.useTime = Common.Configs.MConfigItems.Instance.useTimeMissileLauncher;
				Item.useAnimation = Common.Configs.MConfigItems.Instance.useTimeMissileLauncher;
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noMelee = true;
				Item.knockBack = 5.5f;
				Item.value = 20000;
				Item.rare = ItemRarityID.Green;
				Item.UseSound = Sounds.Items.Weapons.MissileShoot;
				Item.autoReuse = false;
				Item.shoot = ModContent.ProjectileType<MissileShot>();
				Item.shootSpeed = 8f;
				Item.crit = 10;
			}
			Item.autoReuse = pb.isBeam;
			pb.statUA = MConfigItems.Instance.ammoPowerBeam;
			pb.maxUA = MConfigItems.Instance.ammoPowerBeam;
			pb.statMissiles = MConfigItems.Instance.ammoMissileLauncher;
			pb.maxMissiles = MConfigItems.Instance.ammoMissileLauncher;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(8)
				.AddIngredient<Tiles.MissileExpansion>(1)
				.AddIngredient<Miscellaneous.EnergyShard>(3)
				.AddTile(TileID.Anvils)
				.Register();
		}
		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			Item.TryGetGlobalItem(out MGlobalItem mi);
			float armRot = player.itemRotation - (float)(Math.PI / 2) * player.direction;
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armRot);
			Vector2 origin = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, armRot);
			origin.Y -= heldItemFrame.Height / 2f;
			player.itemLocation = origin + player.itemRotation.ToRotationVector2() * (mi.isBeam ? -16 : -14) * player.direction;
		}

		public override bool CanUseItem(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if (player.whoAmI == Main.myPlayer && Item.type == Main.mouseItem.type)
			{
				return false;
			}
			if (BeamMods[0].type == ModContent.ItemType<Addons.PhazonBeamAddon>() && !mp.canUsePhazonBeam)
			{
				return false;
			}
			if (BeamMods[0].type == ModContent.ItemType<Addons.HyperBeamAddon>() && !mp.canUseHyperBeam)
			{
				return false;
			}
			if (isHunter && BeamMods[0].type != ModContent.ItemType<Addons.Hunters.OmegaCannonAddon>() && Item.TryGetGlobalItem(out MGlobalItem ac) && ac.isBeam)
			{
				return player.whoAmI == Main.myPlayer && Math.Floor(ac.statUA) > 0 && mp.statOverheat < mp.maxOverheat /*&& pb.statUA > BeamMods[0].GetGlobalItem<MGlobalItem>().addonUACost*/;
			}
			return Item.TryGetGlobalItem(out MGlobalItem mi) && (mi.isBeam? (player.whoAmI == Main.myPlayer && mp.statOverheat < mp.maxOverheat) : (player.whoAmI == Main.myPlayer && mi.statMissiles > 0));// && BeamLoader.CanShoot(player, BeamMods);
		}

		public override int ChoosePrefix(UnifiedRandom rand)
		{
			int output = Item.prefix;
			switch (rand.Next(14))
			{
				case 0: output = 36; break;
				case 1: output = 37; break;
				case 2: output = 38; break;
				case 3: output = 53; break;
				case 4: output = 54; break;
				case 5: output = 55; break;
				case 6: output = 39; break;
				case 7: output = 40; break;
				case 8: output = 56; break;
				case 9: output = 41; break;
				case 10: output = 57; break;
				case 11: output = 59; break;
				case 12: output = 60; break;
				case 13: output = 61; break;
			}
			//PrefixLoader.Roll(Item, ref output, 14, rand, new PrefixCategory[] { PrefixCategory.AnyWeapon, PrefixCategory.Custom });
			return output;
		}

		public override void OnResearched(bool fullyResearched)
		{
			foreach (Item item in BeamMods)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			}
			foreach (Item item in BeamChange)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			}
			foreach (Item item in MissileMods)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			}
			foreach (Item item in MissileChange)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			}
		}
		public override bool AltFunctionUse(Player player)
		{
			Item.TryGetGlobalItem(out MGlobalItem mi);
			if (!Stealth || Stealth && player.velocity != Vector2.Zero)
			{
				mi.isBeam = !mi.isBeam;
			}
			return false;
		}

		/*public override bool CanReforge()// tModPorter Note: Use CanReforge instead for logic determining if a reforge can happen. 
		{
			foreach (Item item in BeamMods)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			}
			BeamMods = new Item[BeamAddonSlotID.Count];
			foreach (Item item in BeamChange)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			}
			BeamChange = new Item[BeamChangeSlotID.Count];
			return base.CanReforge();
		}
		/*public override bool RangedPrefix()
		{
			return true;
		}*/

		private float iceDmg = 0f;
		private float waveDmg = 0f;
		private float spazDmg = 0f;
		private float plasDmg = 0f;
		private float hunterDmg = 0f;

		private float iceHeat = 0f;
		private float waveHeat = 0f;
		private float spazHeat = 0f;
		private float plasHeat = 0f;
		private float hunterHeat = 0f;

		private float iceSpeed = 0f;
		private float waveSpeed = 0f;
		private float spazSpeed = 0f;
		private float plasSpeed = 0f;
		private float cooldown = 0f;
		public float impStealth = 0f;

		private int finalDmg = MConfigItems.Instance.damagePowerBeam;

		private float chargeDmgMult = 3f;
		private float chargeCost = 2f;

		private int overheat = MConfigItems.Instance.overheatPowerBeam;
		private double useTime = MConfigItems.Instance.useTimePowerBeam;

		private string shot = "PowerBeamShot";
		private string chargeShot = "PowerBeamChargeShot";
		private string shotSound = "PowerBeamSound";
		private Mod shotSoundMod = MetroidMod.Instance;
		private string chargeShotSound = "PowerBeamChargeSound";
		private Mod chargeShotSoundMod = MetroidMod.Instance;
		private string chargeUpSound = "ChargeStartup_Power";
		private Mod chargeUpSoundMod = MetroidMod.Instance;
		private string chargeTex = "ChargeLead";
		private Mod chargeTexMod = MetroidMod.Instance;
		private int dustType = 64;
		private Color dustColor = default(Color);
		private Color lightColor = MetroidMod.powColor;
		public int shotAmt = 1;
		private int chargeShotAmt = 1;
		public string shotEffect = "";
		private float comboKnockBack = 5.5f;
		private bool isHoming = false;
		private bool isCharge = false;
		private bool isSeeker = false;
		private int isHeldCombo = 0;
		private int chargeCostMi = 5;
		private int comboSound = 0;
		private float comboDrain = 5f;
		private bool useFlameSounds = false;
		private bool useVortexSounds = false;

		private bool isShotgun = false;
		private int shotgunAmt = 5;

		private bool isMiniGun = false;
		private int miniRateIncr = 2;
		private int miniGunCostReduct = 2;
		private int miniGunAmt = 1;

		private int comboUseTime = 4;
		private int comboCostUseTime = 12;
		private int comboShotAmt = 1;
		private float chargeMult = 1f;

		private float leadAimSpeed = 0f;

		public SoundStyle? ShotSound;
		public SoundStyle? ChargeShotSound;

		private int waveDir = -1;

		public bool Lum = false;
		public bool Diff = false;

		private bool isJud = false;
		private bool isSpray = false;
		private bool isChargeSpray = false;
		private bool isShock = false;
		private bool isHyper = false;
		private bool isPhazon = false;
		private bool Stealth = false;

		public bool comboError1, comboError2, comboError3, comboError4;
		private bool noSomersault = false;

		private string altTexture => texture + "_alt";
		private string texture = "";

		private readonly int ch = ModContent.ItemType<Addons.ChargeBeamAddon>();
		private readonly int ic = ModContent.ItemType<Addons.IceBeamAddon>();
		private readonly int wa = ModContent.ItemType<Addons.WaveBeamAddon>();
		private readonly int sp = ModContent.ItemType<Addons.SpazerAddon>();
		private readonly int plR = ModContent.ItemType<Addons.PlasmaBeamRedAddon>();
		private readonly int plG = ModContent.ItemType<Addons.PlasmaBeamGreenAddon>();

		private readonly int ch2 = ModContent.ItemType<Addons.V2.ChargeBeamV2Addon>();
		private readonly int ic2 = ModContent.ItemType<Addons.V2.IceBeamV2Addon>();
		private readonly int wa2 = ModContent.ItemType<Addons.V2.WaveBeamV2Addon>();
		private readonly int wi = ModContent.ItemType<Addons.V2.WideBeamAddon>();
		private readonly int nv = ModContent.ItemType<Addons.V2.NovaBeamAddon>();

		private readonly int ch3 = ModContent.ItemType<Addons.V3.LuminiteBeamAddon>();
		private readonly int sd = ModContent.ItemType<Addons.V3.StardustBeamAddon>();
		private readonly int nb = ModContent.ItemType<Addons.V3.NebulaBeamAddon>();
		private readonly int vt = ModContent.ItemType<Addons.V3.VortexBeamAddon>();
		private readonly int sl = ModContent.ItemType<Addons.V3.SolarBeamAddon>();

		private readonly int hy = ModContent.ItemType<Addons.HyperBeamAddon>();
		private readonly int ph = ModContent.ItemType<Addons.PhazonBeamAddon>();

		private readonly int vd = ModContent.ItemType<Addons.Hunters.VoltDriverAddon>();
		private readonly int jd = ModContent.ItemType<Addons.Hunters.JudicatorAddon>();
		private readonly int bh = ModContent.ItemType<Addons.Hunters.BattleHammerAddon>();
		private readonly int mm = ModContent.ItemType<Addons.Hunters.MagMaulAddon>();
		private readonly int imp = ModContent.ItemType<Addons.Hunters.ImperialistAddon>();
		private readonly int sc = ModContent.ItemType<Addons.Hunters.ShockCoilAddon>();
		private readonly int oc = ModContent.ItemType<Addons.Hunters.OmegaCannonAddon>();

		//Mod modBeamTextureMod = null;
		public override void UpdateInventory(Player P)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>();
			if (Item == null || !Item.TryGetGlobalItem(out MGlobalItem ac)) { return; }
			if (ac.isBeam)
			{
				Item slot1 = BeamMods[0];
				Item slot2 = BeamMods[1];
				Item slot3 = BeamMods[2];
				Item slot4 = BeamMods[3];
				Item slot5 = BeamMods[4];
				Item UA = BeamMods[5];

				int damage = MConfigItems.Instance.damagePowerBeam;
				overheat = MConfigItems.Instance.overheatPowerBeam;
				useTime = MConfigItems.Instance.useTimePowerBeam;
				shot = "PowerBeamShot";
				chargeShot = "PowerBeamChargeShot";
				shotAmt = 1;
				chargeShotAmt = 1;
				shotSound = "PowerBeamSound";
				shotSoundMod = MetroidMod.Instance;
				chargeShotSound = "PowerBeamChargeSound";
				chargeShotSoundMod = MetroidMod.Instance;
				chargeUpSound = "ChargeStartup_Power";
				chargeUpSoundMod = MetroidMod.Instance;
				chargeTex = "ChargeLead";
				chargeTexMod = MetroidMod.Instance;
				dustType = 64;
				dustColor = default(Color);
				lightColor = MetroidMod.powColor;

				texture = "";
				//shotEffect = "";

				//modBeamTextureMod = null;

				Item.autoReuse = true;
				Lum = !BeamChange[11].IsAir || (!BeamChange[10].IsAir && mp.PrimeHunter);
				Diff = (!BeamChange[10].IsAir || mp.PrimeHunter) && BeamChange[11].IsAir;
				ShotSound = null;
				ChargeShotSound = null;
				noSomersault = false;
				isSpray = false;
				isChargeSpray = false;
				isShock = false;
				Stealth = false;
				isJud = !Lum && !Diff && (slot1.type == jd) && !mp.PrimeHunter;
				isCharge = (slot1.type == ch || slot1.type == ch2 || slot1.type == ch3);
				isHyper = (slot1.type == hy);
				isPhazon = (slot1.type == ph);
				isHunter = (slot1.type == oc) || (slot1.type == sc) || (slot1.type == imp) || (slot1.type == mm) || (slot1.type == bh) || (slot1.type == jd) || (slot1.type == vd);

				comboError1 = false;
				comboError2 = false;
				comboError3 = false;
				comboError4 = false;

				bool chargeV1 = slot1.type == ch,
					chargeV2 = slot1.type == ch2,
					chargeV3 = slot1.type == ch3;

				bool addonsV1 = (slot2.type == ic || slot3.type == wa || slot4.type == sp || ((slot5.type == plG || slot5.type == plR) && !chargeV2 && !chargeV3));
				bool addonsV2 = (slot2.type == ic2 || slot3.type == wa2 || slot4.type == wi || slot5.type == nv);
				addonsV2 |= ((slot5.type == plG || slot5.type == plR) && (chargeV2 || chargeV3) && !addonsV1);
				bool addonsV3 = (slot2.type == sd || slot3.type == nb || slot4.type == vt || slot5.type == sl);
				ac.maxUA = MConfigItems.Instance.ammoPowerBeam + (MConfigItems.Instance.ammoUA * Math.Min(UA.stack, 12));
				if (ac.statUA > ac.maxUA)
				{
					ac.statUA = ac.maxUA;
				}
				if (ac.statUA <= 0)
				{
					ac.statUA = 0;
				}
				int versionType = 1;
				float GetCharge()
				{
					if (!Lum)
					{
						return MConfigItems.Instance.damageLuminiteBeam;
					}
					else if (!Diff)
					{
						return MConfigItems.Instance.damageChargeBeamV2;
					}
					else
					{
						return MConfigItems.Instance.damageChargeBeam;
					}
				}
				if (addonsV3 || (chargeV3 && !addonsV1 && !addonsV2))
				{
					versionType = 3;
				}
				else if (addonsV2 || (chargeV2 && !addonsV1))
				{
					versionType = 2;
				}
				if (versionType == 1)
				{
					if (slot3.type == wa && slot5.IsAir && slot4.type != sp)
					{
						chargeShotAmt = 2;
					}
					if (slot4.type == sp)
					{
						shotAmt = 3;
						chargeShotAmt = 3;
					}
				}
				if (versionType == 3)
				{
					if (slot3.type == nb && slot4.type != vt)
					{
						shotAmt = 2;
						chargeShotAmt = 2;
					}
					if (slot4.type == vt)
					{
						shotAmt = 5;
						chargeShotAmt = 5;
					}
					if (slot2.type == ic || slot2.type == ic2)
					{
						comboError1 = true;
					}
					if (slot3.type == wa || slot3.type == wa2)
					{
						comboError2 = true;
					}
					if (slot4.type == sp || slot4.type == wi)
					{
						comboError3 = true;
					}
					if (slot5.type == plR || slot5.type == plG || slot5.type == nv)
					{
						comboError4 = true;
					}
				}
				if (versionType == 2)
				{
					if (slot2.type == ic)
					{
						comboError1 = true;
					}
					if (slot3.type == wa)
					{
						comboError2 = true;
					}
					if (slot4.type == sp)
					{
						comboError3 = true;
					}
					if (slot4.type != wi && slot3.type == wa2)
					{
						shotAmt = 2;
						chargeShotAmt = 2;
					}
					if (slot4.type == wi)
					{
						shotAmt = 3;
						chargeShotAmt = 3;
					}
				}
				
				// Default Combos
				if (!isHyper && !isPhazon && !isHunter)
				{
					if (versionType == 1)
					{
						// Ice
						if (slot2.type == ic)
						{
							shot = "IceBeamShot";
							chargeShot = "IceBeamChargeShot";
							shotSound = "IceBeamSound";
							chargeShotSound = "IceBeamChargeSound";
							chargeUpSound = "ChargeStartup_Ice";
							chargeTex = "ChargeLead_Ice";
							dustType = 59;
							lightColor = MetroidMod.iceColor;
							texture = "IceBeam";
							if (slot4.type == sp)
							{
								shot = "IceSpazerShot";
								chargeShot = "IceSpazerChargeShot";
								shotSound = "IceComboSound";
							}
							if (slot5.type == plG)
							{
								shot = "IcePlasmaBeamGreenShot";
								chargeShot = "IcePlasmaBeamGreenChargeShot";
							}
							// Ice Plasma (Red)
							if (slot5.type == plR)
							{
								shot = "IcePlasmaBeamRedShot";
								chargeShot = "IcePlasmaBeamRedChargeShot";
								dustType = 135;
							}
						}
						else
						{
							// Wave
							if (slot3.type == wa && slot5.IsAir)
							{
								shot = "WaveBeamShot";
								chargeShot = "WaveBeamChargeShot";
								shotSound = "WaveBeamSound";
								chargeShotSound = "WaveBeamChargeSound";
								chargeUpSound = "ChargeStartup_Wave";
								chargeTex = "ChargeLead_Wave";
								dustType = 62;
								lightColor = MetroidMod.waveColor;
								texture = "WaveBeam";

								// Wave Spazer
								if (slot4.type == sp)
								{
									shot = "WaveSpazerShot";
									chargeShot = "WaveSpazerChargeShot";
									shotSound = "SpazerSound";
								}
							}
							else
							{
								// Spazer
								if (slot4.type == sp && slot5.IsAir)
								{
									shot = "SpazerShot";
									chargeShot = "SpazerChargeShot";
									shotSound = "SpazerSound";
									chargeShotSound = "SpazerChargeSound";
									chargeTex = "ChargeLead_Spazer";
									texture = "Spazer";
								}
								if (slot5.type == plG)
								{
									shot = "PlasmaBeamGreenShot";
									chargeShot = "PlasmaBeamGreenChargeShot";
									shotSound = "PlasmaBeamGreenSound";
									chargeShotSound = "PlasmaBeamGreenChargeSound";
									chargeTex = "ChargeLead_PlasmaGreen";
									dustType = 61;
									lightColor = MetroidMod.plaGreenColor;
									texture = "PlasmaBeamG";
								}
								if (slot5.type == plR)
								{
									shot = "PlasmaBeamRedShot";
									chargeShot = "PlasmaBeamRedChargeShot";
									shotSound = "PlasmaBeamRedSound";
									chargeShotSound = "PlasmaBeamRedChargeSound";
									chargeUpSound = "ChargeStartup_PlasmaRed";
									chargeTex = "ChargeLead_PlasmaRed";
									dustType = 6;
									lightColor = MetroidMod.plaRedColor;
									texture = "PlasmaBeamR";
								}
							}
						}
					}
					// Charge V2
					else if (versionType == 2)
					{
						shot = "PowerBeamV2Shot";
						chargeShot = "PowerBeamV2ChargeShot";
						shotSound = "PowerBeamV2Sound";

						// Ice V2
						if (slot2.type == ic2)
						{
							shot = "IceBeamV2Shot";
							chargeShot = "IceBeamV2ChargeShot";
							shotSound = "IceBeamV2Sound";
							chargeShotSound = "IceBeamChargeSound";
							chargeUpSound = "ChargeStartup_Ice";
							chargeTex = "ChargeLead_Ice";
							dustType = 59;
							lightColor = MetroidMod.iceColor;
							texture = "IceBeam";

							// Ice Wave V2
							if (slot3.type == wa2)
							{
								shot = "IceWaveBeamV2Shot";
								chargeShot = "IceWaveBeamV2ChargeShot";

								// Ice Wave Wide
								if (slot4.type == wi)
								{
									shot = "IceWaveWideBeamShot";
									chargeShot = "IceWaveWideBeamChargeShot";
									//shotAmt = 3;
									//chargeShotAmt = 3;

									// Ice Wave Wide Nova
									if (slot5.type == nv)
									{
										shot = "IceWaveWideNovaBeamShot";
										chargeShot = "IceWaveWideNovaBeamChargeShot";
										shotSound = "IceWaveNovaBeamV2Sound";
									}
									// Ice Wave Wide Plasma (Green)
									if (slot5.type == plG)
									{
										shot = "IceWaveWidePlasmaBeamGreenV2Shot";
										chargeShot = "IceWaveWidePlasmaBeamGreenV2ChargeShot";
										shotSound = "FinalBeamSound";
										chargeShotSound = "FinalBeamChargeSound";
										chargeUpSound = "ChargeStartup_Final";
									}
									// Ice Wave Wide Plasma (Red)
									if (slot5.type == plR)
									{
										shot = "IceWaveWidePlasmaBeamRedV2Shot";
										chargeShot = "IceWaveWidePlasmaBeamRedV2ChargeShot";
										shotSound = "FinalBeamSound";
										chargeShotSound = "FinalBeamChargeSound";
										chargeUpSound = "ChargeStartup_Final";
										dustType = 135;
									}
								}
								else
								{
									//shotAmt = 2;
									// Ice Wave Nova
									if (slot5.type == nv)
									{
										shot = "IceWaveNovaBeamShot";
										chargeShot = "IceWaveNovaBeamChargeShot";
									}
									// Ice Wave Plasma (Green)
									if (slot5.type == plG)
									{
										shot = "IceWavePlasmaBeamGreenV2Shot";
										chargeShot = "IceWavePlasmaBeamGreenV2ChargeShot";
									}
									// Ice Wave Plasma (Red)
									if (slot5.type == plR)
									{
										shot = "IceWavePlasmaBeamRedV2Shot";
										chargeShot = "IceWavePlasmaBeamRedV2ChargeShot";
										dustType = 135;
									}
								}
							}
							else
							{
								// Ice Wide
								if (slot4.type == wi && slot5.type != nv)
								{
									shot = "IceWideBeamShot";
									chargeShot = "IceWideBeamChargeShot";
									//chargeShotAmt = 3;

									// Ice Wide Plasma (Green)
									if (slot5.type == plG)
									{
										shot = "IceWidePlasmaBeamGreenV2Shot";
										chargeShot = "IceWidePlasmaBeamGreenV2ChargeShot";
									}
									// Ice Wide Plasma (Red)
									if (slot5.type == plR)
									{
										shot = "IceWidePlasmaBeamRedV2Shot";
										chargeShot = "IceWidePlasmaBeamRedV2ChargeShot";
										dustType = 135;
									}
								}
								if (slot5.type == nv)
								{
									shot = "IceNovaBeamShot";
									chargeShot = "IceNovaBeamChargeShot";
									if (slot4.type == wi)
									{
										//shotAmt = 3;
										//chargeShotAmt = 3;
									}
								}
								else
								{
									// Ice Plasma (Green)
									if (slot5.type == plG)
									{
										shot = "IcePlasmaBeamGreenV2Shot";
										chargeShot = "IcePlasmaBeamGreenV2ChargeShot";
									}
									// Ice Plasma (Red)
									if (slot5.type == plR)
									{
										shot = "IcePlasmaBeamRedV2Shot";
										chargeShot = "IcePlasmaBeamRedV2ChargeShot";
										dustType = 135;
									}
								}
							}
						}
						else
						{
							// Wave V2
							if (slot3.type == wa2)
							{
								shot = "WaveBeamV2Shot";
								chargeShot = "WaveBeamV2ChargeShot";
								shotSound = "WaveBeamV2Sound";
								chargeShotSound = "WaveBeamChargeSound";
								chargeUpSound = "ChargeStartup_Wave";
								chargeTex = "ChargeLead_WaveV2";
								dustType = 62;
								lightColor = MetroidMod.waveColor2;
								texture = "WaveBeam";
								// Wave Wide
								if (slot4.type == wi && slot5.IsAir)
								{
									shot = "WaveWideBeamShot";
									chargeShot = "WaveWideBeamChargeShot";
								}
								if (slot5.type == nv)
								{
									shot = "WaveNovaBeamShot";
									chargeShot = "WaveNovaBeamChargeShot";
									shotSound = "NovaBeamSound";
									chargeShotSound = "NovaBeamChargeSound";
									chargeUpSound = "ChargeStartup_Nova";
									chargeTex = "ChargeLead_Nova";
									dustType = 75;
									lightColor = MetroidMod.novColor;
									texture = "NovaBeam";
								}
								// Wave Plasma (Green)
								if (slot5.type == plG)
								{
									shot = "WavePlasmaBeamGreenV2Shot";
									chargeShot = "WavePlasmaBeamGreenV2ChargeShot";
									shotSound = "WavePlasmaBeamGreenSound";
									chargeShotSound = "PlasmaBeamGreenChargeSound";
									chargeUpSound = "ChargeStartup_Power";
									chargeTex = "ChargeLead_PlasmaGreenV2";
									dustType = 15;
									lightColor = MetroidMod.plaGreenColor;
									texture = "PlasmaBeamG";
								}
								// Wave Plasma (Red)
								if (slot5.type == plR)
								{
									shot = "WavePlasmaBeamRedV2Shot";
									chargeShot = "WavePlasmaBeamRedV2ChargeShot";
									shotSound = "PlasmaBeamRedV2Sound";
									chargeShotSound = "PlasmaBeamRedChargeSound";
									chargeUpSound = "ChargeStartup_PlasmaRed";
									chargeTex = "ChargeLead_PlasmaRed";
									dustType = 6;
									lightColor = MetroidMod.plaRedColor;
									texture = "PlasmaBeamR";
								}
							}
							else
							{
								// Wide
								if (slot4.type == wi && slot5.type != nv)
								{
									shot = "WideBeamShot";
									chargeShot = "WideBeamChargeShot";
									shotSound = "WideBeamSound";
									chargeShotSound = "SpazerChargeSound";
									chargeTex = "ChargeLead_Wide";
									dustType = 63;
									lightColor = MetroidMod.wideColor;
									dustColor = MetroidMod.wideColor;
									texture = "WideBeam";

									if (slot5.type == plG)
									{
										shot = "WidePlasmaBeamGreenV2Shot";
										chargeShot = "WidePlasmaBeamGreenV2ChargeShot";
										shotSound = "WidePlasmaBeamGreenSound";
										chargeShotSound = "PlasmaBeamGreenChargeSound";
										chargeTex = "ChargeLead_PlasmaGreen";
										dustType = 61;
										lightColor = MetroidMod.plaGreenColor;
										dustColor = default(Color);
										texture = "PlasmaBeamG";
									}
									// Wide Plasma (Red)
									if (slot5.type == plR)
									{
										shot = "WidePlasmaBeamRedV2Shot";
										chargeShot = "WidePlasmaBeamRedV2ChargeShot";
										shotSound = "PlasmaBeamRedSound";
										chargeShotSound = "PlasmaBeamRedChargeSound";
										chargeUpSound = "ChargeStartup_PlasmaRed";
										chargeTex = "ChargeLead_PlasmaRed";
										dustType = 6;
										lightColor = MetroidMod.plaRedColor;
										dustColor = default(Color);
										texture = "PlasmaBeamR";
									}
								}
								if (slot5.type == nv)
								{
									shot = "NovaBeamShot";
									chargeShot = "NovaBeamChargeShot";
									shotSound = "NovaBeamSound";
									chargeShotSound = "NovaBeamChargeSound";
									chargeUpSound = "ChargeStartup_Nova";
									chargeTex = "ChargeLead_Nova";
									dustType = 75;
									lightColor = MetroidMod.novColor;
									dustColor = default(Color);
									texture = "NovaBeam";
								}
								else
								{
									// Plasma (Green)
									if (slot5.type == plG)
									{
										shot = "PlasmaBeamGreenV2Shot";
										chargeShot = "PlasmaBeamGreenV2ChargeShot";
										shotSound = "PlasmaBeamGreenSound";
										chargeShotSound = "PlasmaBeamGreenChargeSound";
										chargeTex = "ChargeLead_PlasmaGreen";
										dustType = 61;
										lightColor = MetroidMod.plaGreenColor;
										texture = "PlasmaBeamG";
									}
									// Plasma (Red)
									if (slot5.type == plR)
									{
										shot = "PlasmaBeamRedV2Shot";
										chargeShot = "PlasmaBeamRedV2ChargeShot";
										shotSound = "PlasmaBeamRedSound";
										chargeShotSound = "PlasmaBeamRedChargeSound";
										chargeUpSound = "ChargeStartup_PlasmaRed";
										chargeTex = "ChargeLead_PlasmaRed";
										dustType = 6;
										lightColor = MetroidMod.plaRedColor;
										texture = "PlasmaBeamR";
									}
								}
							}
						}

						if (slot2.type == ic)
						{
							comboError1 = true;
						}
						if (slot3.type == wa)
						{
							comboError2 = true;
						}
						if (slot4.type == sp)
						{
							comboError3 = true;
						}
					}
					// Charge V3
					//else if(slot1.type == ch3 || slot2.type == sd || slot3.type == nb || slot4.type == vt || slot5.type == sl)
					else if (versionType == 3)
					{
						shot = "LuminiteBeamShot";
						chargeShot = "LuminiteBeamChargeShot";
						shotSound = "PowerBeamV2Sound";
						chargeTex = "ChargeLead_Luminite";
						dustType = 229;
						lightColor = MetroidMod.lumColor;
						// Stardust
						if (slot2.type == sd && slot5.IsAir)
						{
							shot = "StardustBeamShot";
							chargeShot = "StardustBeamChargeShot";
							shotSound = "IceBeamV2Sound";
							chargeShotSound = "IceBeamChargeSound";
							chargeUpSound = "ChargeStartup_Ice";
							chargeTex = "ChargeLead_Stardust";
							dustType = 87;
							lightColor = MetroidMod.iceColor;
							texture = "StardustBeam";

							// Stardust Nebula
							if (slot4.type == vt)
							{
								shot = "StardustVortexBeamShot";
								chargeShot = "StardustVortexBeamChargeShot";
							}
						}
						if (slot5.type == sl)
						{
							shot = "SolarBeamShot";
							chargeShot = "SolarBeamChargeShot";
							shotSound = "PlasmaBeamRedV2Sound";
							chargeShotSound = "PlasmaBeamRedChargeSound";
							chargeUpSound = "ChargeStartup_PlasmaRed";
							chargeTex = "ChargeLead_Solar";
							dustType = 6;
							lightColor = MetroidMod.plaRedColor;
							texture = "SolarBeam";

							if (slot4.type == vt)
							{
								shot = "VortexSolarBeamShot";
								chargeShot = "VortexSolarBeamChargeShot";
							}
						}
						else
						{
							// Nebula
							if (slot3.type == nb)
							{
								shot = "NebulaBeamShot";
								chargeShot = "NebulaBeamChargeShot";
								shotSound = "WaveBeamV2Sound";
								chargeShotSound = "WaveBeamChargeSound";
								chargeUpSound = "ChargeStartup_Wave";
								chargeTex = "ChargeLead_Nebula";
								dustType = 255;
								lightColor = MetroidMod.waveColor;
								texture = "NebulaBeam";

								// Nebula Vortex
								if (slot4.type == vt)
								{
									shot = "NebulaVortexBeamShot";
									chargeShot = "NebulaVortexBeamChargeShot";
									shotSound = "WideBeamSound";
								}
							}
							else
							{
								// Vortex
								if (slot4.type == vt)
								{
									shot = "VortexBeamShot";
									chargeShot = "VortexBeamChargeShot";
									shotSound = "WideBeamSound";
									chargeShotSound = "SpazerChargeSound";
									chargeTex = "ChargeLead_Vortex";
									texture = "VortexBeam";

								}
							}
						}
					}
				}
				else if (isHunter)
				{
					if (slot1.type == vd)
					{
						isCharge = true;
						shot = "VoltDriverShot";
						chargeShot = "VoltDriverChargeShot";
						shotSound = "VoltDriverSound";
						chargeShotSound = "VoltDriverChargeSound";
						chargeUpSound = "VoltDriverCharge";
						texture = "VoltDriver";
						chargeTex = "ChargeLead_Spazer";
						MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
						mItem.addonChargeDmg = GetCharge();
						//mItem.addonChargeHeat = GetHeat();
						useTime = MConfigItems.Instance.useTimeVoltDriver;
						if (shotAmt > 1)
						{
							isSpray = true;
						}
						if (chargeShotAmt > 1)
						{
							isChargeSpray = true;
						}
					}
					if (slot1.type == jd)
					{
						isCharge = true;
						shot = "JudicatorShot";
						chargeShot = Lum || Diff ? "JudicatorChargeShot" : "JudicatorShot";
						shotSound = "JudicatorSound";
						chargeShotSound = "JudicatorChargeSound";
						chargeUpSound = Lum || Diff ? "ChargeStartup_JudicatorAffinity" : "ChargeStartup_Judicator";
						texture = "Judicator";
						chargeTex = "ChargeLead_Ice";
						useTime = MConfigItems.Instance.useTimeJudicator;
						MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
						mItem.addonChargeDmg = Lum || Diff ? GetCharge() : 1f;
						//mItem.addonChargeHeat = GetHeat();
						if (shotAmt > 1)
						{
							isSpray = true;
						}
						if (chargeShotAmt > 1)
						{
							isChargeSpray = true;
						}
						if (!slot3.IsAir)
						{
							comboError2 = true;
						}
					}

					if (slot1.type == bh)
					{
						shot = "BattleHammerShot";
						shotSound = Lum || Diff ? "BattleHammerAffinitySound" : "BattleHammerSound";
						texture = "BattleHammer";
						useTime = MConfigItems.Instance.useTimeBattleHammer;
						if (shotAmt > 1)
						{
							isSpray = true;
						}
						if (!slot3.IsAir)
						{
							comboError2 = true;
						}

						if (slot5.type == plG)
						{
							comboError4 = true;
						}
					}

					if (slot1.type == imp)
					{
						shot = "ImperialistShot";
						shotSound = "ImperialistSound";
						texture = "Imperialist";
						useTime = MConfigItems.Instance.useTimeImperialist;
						Stealth = true;
					}
					if (slot1.type == mm)
					{
						isCharge = true;
						shot = "MagMaulShot";
						chargeShot = "MagMaulChargeShot";
						shotSound = "MagMaulSound";
						chargeShotSound = "MagMaulChargeSound";
						chargeUpSound = "ChargeStartup_MagMaul";
						texture = "MagMaul";
						chargeTex = "ChargeLead_PlasmaRed";
						MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
						mItem.addonChargeDmg = GetCharge();
						//mItem.addonChargeHeat = GetHeat();
						useTime = MConfigItems.Instance.useTimeMagMaul;
						if (shotAmt > 1)
						{
							isSpray = true;
						}
						if (chargeShotAmt > 1)
						{
							isChargeSpray = true;
						}
						if (!slot2.IsAir)
						{
							comboError1 = true;
						}
						if (!slot3.IsAir)
						{
							comboError2 = true;
						}
						if (slot5.type == plG)
						{
							comboError4 = true;
						}
					}
					if (slot1.type == sc)
					{
						isShock = true;
						noSomersault = true;
						shot = "ShockCoilShot";
						shotSound = "ShockCoilStartupSound";
						texture = "ShockCoil";
						chargeUpSound = "ShockCoilStartupSound";
						chargeShotSound = "";
						chargeShot = "ShockCoilChargeShot";
						chargeTex = "ChargeLead_Stardust";
						useTime = MConfigItems.Instance.useTimeShockCoil;
						//shotAmt = 1;
						if (slot5.type == plG)
						{
							comboError4 = true;
						}
					}
					if (slot1.type == oc)
					{
						shot = "OmegaCannonShot";
						shotSound = "OmegaCannonShotSound";
						texture = "OmegaCannon";
						useTime = MConfigItems.Instance.useTimeOmegaCannon;
						if (shotAmt > 1)
						{
							isSpray = true;
						}
						if (!slot3.IsAir)
						{
							comboError2 = true;
						}
						if (slot5.type == plG)
						{
							comboError4 = true;
						}
					}
				}
				// Hyper
				else if (isHyper)
				{
					shot = "HyperBeamShot";
					shotSound = "HyperBeamSound";
					useTime = MConfigItems.Instance.useTimeHyperBeam;

					damage = MConfigItems.Instance.damageHyperBeam;
					overheat = MConfigItems.Instance.overheatHyperBeam;

					texture = "HyperBeam";

					// Wave / Nebula
					if (shot.Contains("plasmagreen") || shot.Contains("nova") || shot.Contains("solar"))
					{
						shot = "PlasmaHyperBeamShot";
					}
				}
				// Phazon
				else if (isPhazon)
				{

					shot = "PhazonBeamShot";
					shotSound = "PhazonBeamSound";
					useTime = MConfigItems.Instance.useTimePhazonBeam;

					damage = MConfigItems.Instance.damagePhazonBeam;
					overheat = MConfigItems.Instance.overheatPhazonBeam;

					texture = "PhazonBeam";
				}

				iceDmg = 0f;
				waveDmg = 0f;
				spazDmg = 0f;
				plasDmg = 0f;
				hunterDmg = 0f;

				iceHeat = 0f;
				waveHeat = 0f;
				spazHeat = 0f;
				plasHeat = 0f;
				hunterHeat = 0f;

				iceSpeed = 0f;
				waveSpeed = 0f;
				spazSpeed = 0f;
				plasSpeed = 0f;

				if (!slot3.IsAir)
				{
					MGlobalItem mItem = slot3.GetGlobalItem<MGlobalItem>();
					waveDmg = mItem.addonDmg;
					waveHeat = mItem.addonHeat;
					waveSpeed = mItem.addonSpeed;
				}

				if (!slot4.IsAir)
				{
					MGlobalItem mItem = slot4.GetGlobalItem<MGlobalItem>();
					spazDmg = mItem.addonDmg;
					spazHeat = mItem.addonHeat;
					spazSpeed = mItem.addonSpeed;
				}
				if (!slot2.IsAir)
				{
					MGlobalItem mItem = slot2.GetGlobalItem<MGlobalItem>();
					iceDmg = mItem.addonDmg;
					iceHeat = mItem.addonHeat;
					iceSpeed = mItem.addonSpeed;
				}
				if (!slot5.IsAir)
				{
					MGlobalItem mItem = slot5.GetGlobalItem<MGlobalItem>();
					plasDmg = mItem.addonDmg;
					plasHeat = mItem.addonHeat;
					plasSpeed = mItem.addonSpeed;
				}

				if (!slot1.IsAir)
				{
					MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
					chargeDmgMult = mItem.addonChargeDmg;
					chargeCost = mItem.addonChargeHeat;
					hunterDmg = mItem.addonDmg;
					hunterHeat = mItem.addonHeat;
				}

				finalDmg = (int)Math.Round((double)(damage * (1f + iceDmg + waveDmg + spazDmg + plasDmg + hunterDmg)));
				overheat = (int)Math.Max(Math.Round((double)(overheat * (1 + iceHeat + waveHeat + spazHeat + plasHeat + hunterHeat))), 1);

				double shotsPerSecond = 60 / useTime * (1f + iceSpeed + waveSpeed + spazSpeed + plasSpeed);

				useTime = (int)Math.Max(Math.Round(60.0 / (double)shotsPerSecond), 2);

				float oof = 1f + (impStealth / 126f);

				Item.damage = (int)(finalDmg * oof);
				Item.useTime = (int)useTime;
				Item.useAnimation = (int)useTime;
				Item.shoot = ModContent.Find<ModProjectile>(Mod.Name, shot).Type;
				//ShotSound = new SoundStyle($"{shotSoundMod.Name}/Assets/Sounds/{shotSound}");
				ChargeShotSound = new SoundStyle($"{Mod.Name}/Assets/Sounds/{chargeShotSound}");
				Item.UseSound = new($"{Mod.Name}/Assets/Sounds/{shotSound}");//ShotSound; //TESTING

				//Item.autoReuse = (!slot1.IsAir);//(isCharge);

				Item.shootSpeed = slot1.type == oc || Item.shoot == ModContent.ProjectileType<VoltDriverChargeShot>() ? 2f : Item.shoot == ModContent.ProjectileType<VoltDriverShot>() ? 11f : 8f;
				Item.reuseDelay = 0;
				Item.mana = 0;
				Item.knockBack = slot1.type == bh ? 6f : slot1.type == sc ? 0f : 4f;
				Item.scale = 0.8f;
				Item.crit = 3;
				Item.value = 20000;

				Item.rare = ItemRarityID.Green;

				Item.Prefix(Item.prefix);

				if (isPhazon)
				{
					Item.useAnimation = 9;
					Item.useTime = 3;
					Item.UseSound = new SoundStyle($"{Mod.Name}/Assets/Sounds/PhazonBeamSound");
				}
				else
				{
					Item.UseSound = new($"{Mod.Name}/Assets/Sounds/{shotSound}");
				}
			}
			else
			{

				int ic = ModContent.ItemType<IceMissileAddon>();
				int sm = ModContent.ItemType<SuperMissileAddon>();
				int icSm = ModContent.ItemType<IceSuperMissileAddon>();
				int st = ModContent.ItemType<StardustMissileAddon>();
				int ne = ModContent.ItemType<NebulaMissileAddon>();

				int se = ModContent.ItemType<SeekerMissileAddon>();

				Item slot1 = MissileMods[0];
				Item slot2 = MissileMods[1];
				Item exp = MissileMods[2];

				int damage = Common.Configs.MConfigItems.Instance.damageMissileLauncher;
				useTime = Common.Configs.MConfigItems.Instance.useTimeMissileLauncher;
				shot = "MissileShot";
				chargeShot = "";
				shotSound = "MissileShoot";
				chargeShotSound = "SuperMissileSound";
				chargeUpSound = "";
				chargeTex = "";
				dustType = 0;
				dustColor = default(Color);
				lightColor = Color.White;

				texture = "";

				comboKnockBack = Item.knockBack;

				isSeeker = slot1.type == se;
				isCharge = !slot1.IsAir && !isSeeker;
				isHeldCombo = 0;
				chargeCost = 5;
				comboSound = 0;
				comboDrain = 5f;
				noSomersault = false;
				useFlameSounds = false;
				useVortexSounds = false;

				isHoming = false;
				isShotgun = false;
				shotgunAmt = 5;

				isMiniGun = false;
				miniRateIncr = 2;
				miniGunCostReduct = 2;
				miniGunAmt = 1;

				comboUseTime = 4;
				comboCostUseTime = 12;
				comboShotAmt = 1;

				leadAimSpeed = 0f;

				ac.maxMissiles = MConfigItems.Instance.ammoMissileLauncher + (MConfigItems.Instance.ammoMissileTank * Math.Min(exp.stack, 50));
				if (ac.statMissiles > ac.maxMissiles)
				{
					ac.statMissiles = ac.maxMissiles;
				}

				// Default Combos

				if (slot2.type == sm)
				{
					shot = "SuperMissileShot";
					shotSound = "SuperMissileShoot";
				}
				else if (slot2.type == ic)
				{
					shot = "IceMissileShot";
					shotSound = "MissileShoot";
				}
				else if (slot2.type == icSm)
				{
					shot = "IceSuperMissileShot";
					shotSound = "IceMissileShoot";
				}
				else if (slot2.type == st)
				{
					shot = "StardustMissileShot";
					shotSound = "SuperMissileShoot";
				}
				else if (slot2.type == ne)
				{
					shot = "NebulaMissileShot";
					shotSound = "SuperMissileShoot";
				}

				int wb = ModContent.ItemType<WavebusterAddon>();
				int icSp = ModContent.ItemType<IceSpreaderAddon>();
				int sp = ModContent.ItemType<SpazerComboAddon>();
				int ft = ModContent.ItemType<FlamethrowerAddon>();
				int pl = ModContent.ItemType<PlasmaMachinegunAddon>();
				int nv = ModContent.ItemType<NovaComboAddon>();
				int hm = ModContent.ItemType<HomingMissileAddon>();

				int di = ModContent.ItemType<DiffusionMissileAddon>();

				// Charge Combos
				if (slot1.type == wb)
				{
					isHeldCombo = 1;
					comboSound = 1;
					noSomersault = true;
					chargeShot = "WavebusterShot";
					chargeUpSound = "ChargeStartup_Wave";
					chargeTex = "ChargeLead_WaveV2";
					dustType = 62;
					lightColor = MetroidMod.waveColor2;
					comboKnockBack = 0f;
					texture = "Wavebuster";
				}
				if (slot1.type == hm)
				{
					isHoming = true;
					chargeCost = 2;
					chargeShot = shot;
					chargeUpSound = "ChargeStartup_HomingMissile";
					chargeShotSound = "HomingMissileShoot";
					chargeTex = "ChargeLead_Spazer";
					dustType = 64;
					lightColor = MetroidMod.powColor;
					texture = "SpazerCombo";
				}
				if (slot1.type == icSp)
				{
					chargeShot = "IceSpreaderShot";
					chargeShotSound = "IceSpreaderSound";
					chargeUpSound = "ChargeStartup_Ice";
					chargeTex = "ChargeLead_Ice";
					dustType = 59;
					lightColor = MetroidMod.iceColor;
					texture = "IceSpreader";
				}
				if (slot1.type == sp)
				{
					isShotgun = true;
					chargeShot = shot;
					chargeUpSound = "ChargeStartup_Power";
					chargeTex = "ChargeLead_Spazer";
					dustType = 64;
					lightColor = MetroidMod.powColor;
					texture = "SpazerCombo";
				}
				if (slot1.type == ft)
				{
					isHeldCombo = 2;
					comboSound = 1;
					noSomersault = true;
					useFlameSounds = true;
					chargeShot = "FlamethrowerShot";
					chargeUpSound = "ChargeStartup_PlasmaRed";
					chargeTex = "ChargeLead_PlasmaRed";
					dustType = 6;
					lightColor = MetroidMod.plaRedColor;
					texture = "Flamethrower";
				}
				if (slot1.type == pl)
				{
					isHeldCombo = 2;
					comboSound = 2;
					noSomersault = true;
					isMiniGun = true;
					chargeShot = "PlasmaMachinegunShot";
					chargeShotSound = "PlasmaMachinegunSound";
					chargeUpSound = "ChargeStartup_Power";
					chargeTex = "ChargeLead_PlasmaGreen";
					dustType = 61;
					lightColor = MetroidMod.plaGreenColor;
					texture = "PlasmaMachinegun";
				}
				if (slot1.type == nv)
				{
					isHeldCombo = 1;
					comboSound = 1;
					noSomersault = true;
					leadAimSpeed = 0.85f;
					chargeShot = "NovaLaserShot";
					chargeUpSound = "ChargeStartup_Nova";
					chargeTex = "ChargeLead_Nova";
					dustType = 75;
					lightColor = MetroidMod.novColor;
					texture = "NovaLaser";
				}

				if (slot1.type == di)
				{
					chargeShot = "DiffusionMissileShot";
					chargeUpSound = "ChargeStartup_Power";
					chargeTex = "ChargeLead_PlasmaRed";
					dustType = 6;
					lightColor = MetroidMod.plaRedColor;
					texture = "DiffusionMissile";

					if (slot2.type == ic || slot2.type == icSm)
					{
						chargeShot = "IceDiffusionMissileShot";
						chargeUpSound = "ChargeStartup_Ice";
						chargeTex = "ChargeLead_Ice";
						dustType = 135;
						lightColor = MetroidMod.iceColor;
					}
					if (slot2.type == st)
					{
						chargeShot = "StardustDiffusionMissileShot";
						chargeUpSound = "ChargeStartup_Ice";
						chargeTex = "ChargeLead_Stardust";
						dustType = 87;
						lightColor = MetroidMod.iceColor;
					}
					if (slot2.type == ne)
					{
						chargeShot = "NebulaDiffusionMissileShot";
						chargeUpSound = "ChargeStartup_Wave";
						chargeTex = "ChargeLead_Nebula";
						dustType = 255;
						lightColor = MetroidMod.waveColor;
					}
				}
				if (isSeeker)
				{
					texture = "SeekerMissile";
				}

				int sd = ModContent.ItemType<StardustComboAddon>();
				int nb = ModContent.ItemType<NebulaComboAddon>();
				int vt = ModContent.ItemType<VortexComboAddon>();
				int sl = ModContent.ItemType<SolarComboAddon>();

				if (slot1.type == sd)
				{
					chargeShot = "StardustComboShot";
					chargeShotSound = "IceSpreaderSound";
					chargeUpSound = "ChargeStartup_Ice";
					chargeTex = "ChargeLead_Stardust";
					dustType = 87;
					lightColor = MetroidMod.iceColor;
					texture = "StardustCombo";
				}
				if (slot1.type == nb)
				{
					isHeldCombo = 1;
					comboSound = 1;
					noSomersault = true;
					chargeShot = "NebulaComboShot";
					chargeUpSound = "ChargeStartup_Wave";
					chargeTex = "ChargeLead_Nebula";
					dustType = 255;
					lightColor = MetroidMod.waveColor;
					texture = "NebulaCombo";
				}
				if (slot1.type == vt)
				{
					isHeldCombo = 2;
					comboSound = 2;
					noSomersault = true;

					comboUseTime = 10;
					comboShotAmt = 6;

					useVortexSounds = true;

					chargeShot = "VortexComboShot";
					chargeShotSound = "PlasmaMachinegunSound";
					chargeUpSound = "ChargeStartup_Power";
					chargeTex = "ChargeLead_Vortex";
					dustType = 229;
					lightColor = MetroidMod.lumColor;
					texture = "VortexCombo";
				}
				if (slot1.type == sl)
				{
					isHeldCombo = 1;
					comboSound = 1;
					noSomersault = true;
					leadAimSpeed = 0.9f;
					chargeShot = "SolarLaserShot";
					chargeUpSound = "ChargeStartup_PlasmaRed";
					chargeTex = "ChargeLead_SolarCombo";
					dustType = 6;
					lightColor = MetroidMod.plaRedColor;
					texture = "SolarCombo";
				}

				if (!slot1.IsAir)
				{
					MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
					chargeMult = mItem.addonChargeDmg;
					chargeCost = mItem.addonMissileCost;
					comboDrain = mItem.addonMissileDrain;
				}
				comboCostUseTime = (int)Math.Round(60.0 / (double)comboDrain);

				float addonDmg = 0f;
				float addonSpeed = 0f;
				if (!slot2.IsAir)
				{
					MGlobalItem mItem = slot2.GetGlobalItem<MGlobalItem>();
					addonDmg = mItem.addonDmg;
					addonSpeed = mItem.addonSpeed;
				}
				finalDmg = (int)Math.Round((double)(damage * (1f + addonDmg)));

				float shotsPerSecond = (float)(60f / useTime) * (1f + addonSpeed);
				useTime = (int)Math.Max(Math.Round(60.0 / (double)shotsPerSecond), 2);

				Item.damage = finalDmg;
				Item.useTime = (int)useTime;
				Item.useAnimation = (int)useTime;
				Item.shoot = Mod.Find<ModProjectile>(shot).Type;
				Item.UseSound = new($"{Mod.Name}/Assets/Sounds/{shotSound}");

				Item.shootSpeed = 8f;
				Item.reuseDelay = 0;
				Item.mana = 0;
				Item.knockBack = 5.5f;
				Item.scale = 0.8f;
				Item.crit = 10;
				Item.value = 20000;

				Item.rare = ItemRarityID.Green;

				Item.Prefix(Item.prefix);
			}
			
		}
		public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			if (Item == null || !Item.TryGetGlobalItem(out MGlobalItem mi)) { return true; }
			Texture2D tex = Terraria.GameContent.TextureAssets.Item[Type].Value;
			SetTexture(mi);
			if (mi.itemTexture != null)
			{
				tex = mi.itemTexture;
			}
			float num5 = Item.height - tex.Height;
			float num6 = Item.width / 2 - tex.Width / 2;
			sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + (tex.Width / 2) + num6, Item.position.Y - Main.screenPosition.Y + (tex.Height / 2) + num5 + 2f),
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), alphaColor, rotation, new Vector2(tex.Width / 2, (tex.Height / 2)), scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (Item == null || !Item.TryGetGlobalItem(out MGlobalItem mi)) { return true; }
			Texture2D tex = Terraria.GameContent.TextureAssets.Item[Type].Value;
			SetTexture(mi);
			if (mi.itemTexture != null)
			{
				tex = mi.itemTexture;
			}
			sb.Draw(tex, position, new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}

		private void SetTexture(MGlobalItem mi)
		{
			if (texture != "")
			{
				string alt = "";
				if (MetroidMod.UseAltWeaponTextures)
				{
					alt = "_alt";
				}
				mi.itemTexture = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/ArmCannon/{texture + alt}").Value;
			}
			else
			{
				if (mi.isBeam)
				{
					if (MetroidMod.UseAltWeaponTextures)
					{
						mi.itemTexture = ModContent.Request<Texture2D>(Texture + "_alt").Value;
					}
					else
					{
						mi.itemTexture = ModContent.Request<Texture2D>(Texture).Value;
					}
				}
				else
				{
					if (MetroidMod.UseAltWeaponTextures)
					{
						mi.itemTexture = ModContent.Request<Texture2D>(Texture + "miss_alt").Value;
					}
					else
					{
						mi.itemTexture = ModContent.Request<Texture2D>(Texture + "miss").Value;
					}
				}
			}
			if (mi.itemTexture != null)
			{
				Item.width = mi.itemTexture.Width;
				Item.height = mi.itemTexture.Height;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			Item.TryGetGlobalItem(out MGlobalItem mi);
			Player player = Main.player[Main.myPlayer];
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if (mi.isBeam)
			{
				if (Item == Main.HoverItem)
				{
					Item.ModItem.UpdateInventory(player);
				}

				int dmg = player.GetWeaponDamage(Item);
				int chDmg = (int)(dmg * chargeDmgMult);
				TooltipLine chDmgLine = new(Mod, "ChargeDamage", chDmg + " Charge Shot damage");

				int oh = (int)(overheat * mp.overheatCost);
				TooltipLine ohLine = new(Mod, "Overheat", "Overheats by " + oh + " points per use");
				int chOh = (int)(oh * chargeCost);
				TooltipLine chOhLine = new(Mod, "ChargeOverheat", "Overheats by " + chOh + " points on Charge Shot");

				for (int k = 0; k < tooltips.Count; k++)
				{
					if (tooltips[k].Name == "Damage" && isCharge)
					{
						tooltips.Insert(k + 1, chDmgLine);
					}
					if (tooltips[k].Name == "Knockback")
					{
						tooltips.Insert(k + 1, ohLine);
						if (isCharge)
						{
							tooltips.Insert(k + 2, chOhLine);
						}
					}
					if (tooltips[k].Name == "PrefixDamage")
					{
						double num19 = (double)((float)Item.damage - (float)finalDmg);
						num19 = num19 / (double)((float)finalDmg) * 100.0;
						num19 = Math.Round(num19);
						if (num19 > 0.0)
						{
							tooltips[k].Text = "+" + num19 + Lang.tip[39].Value;
						}
						else
						{
							tooltips[k].Text = num19 + Lang.tip[39].Value;
						}
					}
					if (tooltips[k].Name == "PrefixSpeed")
					{
						double num20 = (double)((float)Item.useAnimation - (float)useTime);
						num20 = num20 / (double)((float)useTime) * 100.0;
						num20 = Math.Round(num20);
						num20 *= -1.0;
						if (num20 > 0.0)
						{
							tooltips[k].Text = "+" + num20 + Lang.tip[40].Value;
						}
						else
						{
							tooltips[k].Text = num20 + Lang.tip[40].Value;
						}
					}
				}
			}
			else 
			{
				if (Item == Main.HoverItem)
				{
					Item.ModItem.UpdateInventory(Main.player[Main.myPlayer]);
				}

				int cost = (int)(chargeCost * (mp.missileCost + 0.001f));
				string ch = "Charge shot consumes " + cost + " missiles";
				if (isHeldCombo > 0)
				{
					ch = "Charge initially costs " + cost + " missiles";
				}
				TooltipLine mCost = new(Mod, "ChargeMissileCost", ch);

				float drain = (float)Math.Round(comboDrain * mp.missileCost, 2);
				TooltipLine mDrain = new(Mod, "ChargeMissileDrain", "Drains " + drain + " missiles per second");

				for (int k = 0; k < tooltips.Count; k++)
				{
					if (tooltips[k].Name == "Knockback" && !MissileMods[0].IsAir && !isSeeker)
					{
						tooltips.Insert(k + 1, mCost);
						if (isHeldCombo > 0)
						{
							tooltips.Insert(k + 2, mDrain);
						}
					}

					if (tooltips[k].Name == "PrefixDamage")
					{
						double num19 = (double)((float)Item.damage - (float)finalDmg);
						num19 = num19 / (double)((float)finalDmg) * 100.0;
						num19 = Math.Round(num19);
						if (num19 > 0.0)
						{
							tooltips[k].Text = "+" + num19 + Lang.tip[39].Value;
						}
						else
						{
							tooltips[k].Text = num19 + Lang.tip[39].Value;
						}
					}
					if (tooltips[k].Name == "PrefixSpeed")
					{
						double num20 = (double)((float)Item.useAnimation - (float)useTime);
						num20 = num20 / (double)((float)useTime) * 100.0;
						num20 = Math.Round(num20);
						num20 *= -1.0;
						if (num20 > 0.0)
						{
							tooltips[k].Text = "+" + num20 + Lang.tip[40].Value;
						}
						else
						{
							tooltips[k].Text = num20 + Lang.tip[40].Value;
						}
					}
				}
			}
		}

		/*public override void GetWeaponDamage(Player P, ref int dmg)
		{
			dmg = (int)((float)dmg*baseDmg * (1f + iceDmg + waveDmg + spazDmg + plasDmg));
		}*/

		public override ModItem Clone(Item item)
		{
			ModItem clone = base.Clone(item);
			ArmCannon clone2 = (ArmCannon)clone;
			clone2._beamMods = new Item[MetroidMod.beamSlotAmount];
			clone2._beamchangeMods = new Item[MetroidMod.beamChangeSlotAmount];
			for (int i = 0; i < MetroidMod.beamSlotAmount; ++i)
			{
				if (_beamMods == null || _beamMods[i] == null)
				{
					clone2._beamMods[i] = new Item();
					clone2._beamMods[i].TurnToAir();
				}
				else
				{
					clone2._beamMods[i] = _beamMods[i];
				}
			}
			for (int i = 0; i < MetroidMod.beamChangeSlotAmount; ++i)
			{
				if (_beamchangeMods == null || _beamchangeMods[i] == null)
				{
					clone2._beamchangeMods[i] = new Item();
					clone2._beamchangeMods[i].TurnToAir();
				}
				else
				{
					clone2._beamchangeMods[i] = _beamchangeMods[i];
				}
			}
			clone2._missileMods = new Item[MetroidMod.missileSlotAmount];
			clone2._missileChange = new Item[MetroidMod.missileChangeSlotAmount];
			for (int i = 0; i < MetroidMod.missileSlotAmount; ++i)
			{
				clone2.MissileMods[i] = this.MissileMods[i];
				if (_missileMods == null || _missileMods[i] == null)
				{
					clone2._missileMods[i] = new Item();
					clone2._missileMods[i].TurnToAir();
				}
				else
				{
					clone2._missileMods[i] = _missileMods[i];
				}
			}
			for (int i = 0; i < MetroidMod.missileChangeSlotAmount; ++i)
			{
				clone2.MissileChange[i] = this.MissileChange[i];
				if (_missileChange == null || _missileChange[i] == null)
				{
					clone2._missileChange[i] = new Item();
					clone2._missileChange[i].TurnToAir();
				}
				else
				{
					clone2._missileChange[i] = _missileChange[i];
				}
			}
			return clone2;
		}

		int chargeLead = -1;
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
			float speedX = velocity.X;
			float speedY = velocity.Y;
			if (!Item.TryGetGlobalItem(out MGlobalItem pb)) { return true; }
			if (pb.isBeam)
			{
				if (isCharge || isShock)
				{
					int ch = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ChargeLead>(), damage, knockback, player.whoAmI);
					ChargeLead cl = (ChargeLead)Main.projectile[ch].ModProjectile;
					cl.ChargeUpSound = chargeUpSound;
					cl.ChargeUpSoundMod = chargeUpSoundMod;
					cl.ChargeTex = chargeTex;
					cl.ChargeTexMod = chargeTexMod;
					cl.ChargeShotAmt = chargeShotAmt;
					cl.DustType = dustType;
					cl.DustColor = dustColor;
					cl.LightColor = lightColor;
					cl.canPsuedoScrew = mp.psuedoScrewActive;
					cl.ShotSound = shotSound;
					cl.ShotSoundMod = shotSoundMod;
					cl.ChargeShotSound = chargeShotSound;
					cl.ChargeShotSoundMod = chargeShotSoundMod;
					cl.Projectile.netUpdate = true;
					cl.noSomersault = noSomersault;

					chargeLead = ch;
					if (isShock)
					{
						cl.extraScale = .1f;
					}
				}
				if (isHyper)
				{
					int hyperProj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, Item.shoot, damage, knockback, player.whoAmI);

					if (shotAmt > 1)
					{
						for (int i = 0; i < shotAmt; i++)
						{
							if (i != 2)
							{
								int extraProj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ExtraHyperBeamShot>(), damage, knockback, player.whoAmI, 0, i);
								MProjectile mProj = (MProjectile)Main.projectile[extraProj].ModProjectile;
								mProj.waveDir = waveDir;
								mProj.shot = shotEffect.ToString();
								Main.projectile[extraProj].netUpdate = true;
							}
						}
					}
					mp.hyperColors = 23;
				}
				else
				{
					for (int i = 0; i < shotAmt; i++)
					{
						int shotProj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, Item.shoot, damage, knockback, player.whoAmI, 0, i);
						MProjectile mProj = (MProjectile)Main.projectile[shotProj].ModProjectile;
						mProj.waveDir = waveDir;
						mProj.shot = shotEffect.ToString();
						Main.projectile[shotProj].netUpdate = true;
						if (isSpray && shotAmt > 1)
						{
							Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
							Main.projectile[shotProj].velocity = newVelocity;
						}
					}
				}
				waveDir *= -1;

				mp.statOverheat += MGlobalItem.AmmoUsage(player, overheat * mp.overheatCost);//(int)(pb.AmmoUse(player) ? (overheat * mp.overheatCost) : 0);
				mp.overheatDelay = (int)Math.Max(useTime - 10, 2);
				/* Sound & Sound Networking */ //TODO causes audio crashes
				if (Main.netMode != NetmodeID.SinglePlayer && mp.Player.whoAmI == Main.myPlayer)
				{
					//NetMessage.SendData(132);
					// Send a packet to have the sound play on all clients.
					/*ModPacket packet = Mod.GetPacket();
					packet.Write((byte)MetroidMessageType.PlaySyncedSound);
					packet.Write((byte)player.whoAmI);
					packet.Write(shotSound);
					packet.Send();*/
				}
				// Play the shot sound for the local player, not really --Dr
				if (!isPhazon)
				{
					SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/{shotSound}"), player.position);// SoundEngine.PlaySound(new SoundStyle($"{shotSoundMod.Name}/Assets/Sounds/{shotSound}"), oPos);
					if (Main.netMode != NetmodeID.SinglePlayer && mp.Player.whoAmI == Main.myPlayer)
					{
						NetMessage.SendData(132);
						// Send a packet to have the sound play on all clients.
						/*ModPacket packet = Mod.GetPacket();
						packet.Write((byte)MetroidMessageType.PlaySyncedSound);
						packet.Write((byte)player.whoAmI);
						packet.Write(shotSound);
						packet.Send();*/
					}
				}
				// Does UA math, and doesn't subtract for normal shots (they have cost set to 0)
				if (!isShock && isHunter)
				{
					pb.statUA -= MGlobalItem.AmmoUsage(player, BeamMods[0].GetGlobalItem<MGlobalItem>().addonUACost * mp.UACost);
				}
			}
			else
			{
				if (isCharge)
				{
					int ch = Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ModContent.ProjectileType<ChargeLead>(), damage, knockback, player.whoAmI);
					ChargeLead cl = (ChargeLead)Main.projectile[ch].ModProjectile;
					cl.ChargeUpSound = chargeUpSound;
					cl.ChargeTex = chargeTex;
					cl.DustType = dustType;
					cl.DustColor = dustColor;
					cl.LightColor = lightColor;
					cl.ShotSound = shotSound;
					cl.ChargeShotSound = chargeShotSound;
					cl.Projectile.netUpdate = true;
					cl.missile = true;
					cl.comboSound = comboSound;
					cl.noSomersault = noSomersault;
					cl.aimSpeed = leadAimSpeed;

					chargeLead = ch;
					return false;
				}
				else if (isSeeker)
				{
					int ch = Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ModContent.ProjectileType<SeekerMissileLead>(), damage, knockback, player.whoAmI);
					chargeLead = ch;
					return false;
				}
				else
				{
					pb.statMissiles -= (int)Math.Round(MGlobalItem.AmmoUsage(player, 1));
					SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/{shotSound}"), player.position);
				}
				return true;
			}
			return false;
		}
		private bool LeadActive(Player player, int type)
		{
			return (chargeLead != -1 && Main.projectile[chargeLead].active && Main.projectile[chargeLead].owner == player.whoAmI && Main.projectile[chargeLead].type == type);
		}
		private bool initialShot = false;
		private int comboTime = 0;
		private int comboCostTime = 0;
		private float scalePlus = 0f;
		private int targetingDelay = 0;
		private int targetNum = 0;
		public override void HoldItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				MPlayer mp = player.GetModPlayer<MPlayer>();
				MGlobalItem pb = Item.GetGlobalItem<MGlobalItem>();
				float MY = Main.mouseY + Main.screenPosition.Y;
				float MX = Main.mouseX + Main.screenPosition.X;
				Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
				float targetrotation = (float)Math.Atan2(MY - oPos.Y, MX - oPos.X);
				int damage = player.GetWeaponDamage(Item);
				Vector2 velocity = targetrotation.ToRotationVector2() * Item.shootSpeed;
				if (pb.isBeam) 
				{
					shotEffect = "";
					Item.UseSound = ShotSound;
					Item slot1 = BeamMods[0];
					Item slot2 = BeamMods[1];
					Item slot3 = BeamMods[2];
					Item slot4 = BeamMods[3];
					Item slot5 = BeamMods[4];
					if (Common.Systems.MSystem.SwitchKey.JustPressed)
					{
						mp.beamChangeActive = !mp.beamChangeActive;
						if (mp.beamChangeActive)
						{
							SoundEngine.PlaySound(Sounds.Items.Weapons.BeamSelect);
						}
						if (!mp.beamChangeActive)
						{
							SoundEngine.PlaySound(Sounds.Items.Weapons.BeamSelectFail);
						}
					}
					float oHeat = overheat * mp.overheatCost;
					if (comboError3 != true)
					{
						if (slot4.type == vt)
						{
							shotEffect += "vortex";
						}
						if (slot4.type == sp)
						{
							shotEffect += "spazer";
						}
						if (slot4.type == wi)
						{
							shotEffect += "wide";
						}
					}
					if (comboError2 != true || chargeShot == "JudicatorChargeShot")
					{
						if (slot3.type == wa)
						{
							shotEffect += "wave";
						}
						if (slot3.type == wa2)
						{
							shotEffect += "waveV2";
						}
						if (slot3.type == nb)
						{
							shotEffect += "nebula";
						}
					}

					if (comboError4 != true)
					{
						if (slot5.type == plR)
						{
							shotEffect += "plasmared";
						}
						if (slot5.type == plG)
						{
							shotEffect += "plasmagreen";
						}
						if (slot5.type == nv)
						{
							shotEffect += "nova";
						}
						if (slot5.type == sl)
						{
							shotEffect += "solar";
						}
					}
					if (comboError1 != true)
					{
						if (slot2.type == ic)
						{
							shotEffect += "ice";
						}
						if (slot2.type == ic2)
						{
							shotEffect += "iceV2";
						}
						if (slot2.type == sd)
						{
							shotEffect += "stardust";
						}
					}
					if (isJud)
					{
						chargeShotAmt *= 3;
						isChargeSpray = true;
					}
					if (player.gravDir == -1f)
					{
						MY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
					}

					if (isCharge)
					{

						if (!mp.ballstate && !mp.shineActive && !player.dead && !player.noItems)
						{
							if (player.controlUseItem && chargeLead != -1 && Main.projectile[chargeLead].active && Main.projectile[chargeLead].owner == player.whoAmI && Main.projectile[chargeLead].type == ModContent.ProjectileType<ChargeLead>())
							{
								if (mp.statCharge < MPlayer.maxCharge && mp.statOverheat < mp.maxOverheat)
								{
									mp.statCharge = Math.Min(mp.statCharge + 1, MPlayer.maxCharge);
								}
							}
							else
							{

								float dmgMult = 1f + (chargeDmgMult - 1f) / MPlayer.maxCharge * mp.statCharge;
								double sideangle = Math.Atan2(velocity.Y, velocity.X) + (Math.PI / 2);

								if ((mp.statCharge >= (MPlayer.maxCharge * 0.5) && !isHunter) || (mp.statCharge >= MPlayer.maxCharge && isHunter))
								{
									for (int i = 0; i < chargeShotAmt; i++)
									{
										int chargeProj = Projectile.NewProjectile(player.GetSource_ItemUse(Item), oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(chargeShot).Type, (int)(damage * dmgMult), Item.knockBack, player.whoAmI, 0, i);
										MProjectile mProj = (MProjectile)Main.projectile[chargeProj].ModProjectile;
										mProj.waveDir = waveDir;
										mProj.shot = shotEffect.ToString();
										mProj.canDiffuse = mp.statCharge >= (MPlayer.maxCharge * 0.9) && (Lum || Diff);
										Main.projectile[chargeProj].netUpdate = true;
										if (isChargeSpray /*&& chargeShotAmt > 1*/)
										{
											Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
											Main.projectile[chargeProj].velocity = newVelocity;
										}
									}
									SoundEngine.PlaySound(new SoundStyle($"{chargeShotSoundMod.Name}/Assets/Sounds/{chargeShotSound}"), oPos);

									mp.statOverheat += MGlobalItem.AmmoUsage(player, oHeat * mp.overheatCost);
									mp.overheatDelay = (int)useTime - 10;
									pb.statUA -= MGlobalItem.AmmoUsage(player, BeamMods[0].GetGlobalItem<MGlobalItem>().addonUACost * mp.UACost);
								}
								else if (mp.statCharge > 0)
								{
									if (mp.statCharge >= 30 && mp.statCharge <= (!isHunter ? MPlayer.maxCharge * 0.5 : MPlayer.maxCharge))
									{
										for (int i = 0; i < shotAmt; i++)
										{
											int shotProj = Projectile.NewProjectile(player.GetSource_ItemUse(Item), oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(shot).Type, damage, Item.knockBack, player.whoAmI, 0, i);
											MProjectile mProj = (MProjectile)Main.projectile[shotProj].ModProjectile;
											mProj.waveDir = waveDir;
											mProj.shot = shotEffect.ToString();
											Main.projectile[shotProj].netUpdate = true;
											if (isSpray && shotAmt > 1)
											{
												Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
												Main.projectile[shotProj].velocity = newVelocity;
											}
										}

										SoundEngine.PlaySound(new SoundStyle($"{shotSoundMod.Name}/Assets/Sounds/{shotSound}"), oPos);

										mp.statOverheat += MGlobalItem.AmmoUsage(player, oHeat * mp.overheatCost);
										mp.overheatDelay = (int)useTime - 10;
									}
								}
								if (chargeLead == -1 || !Main.projectile[chargeLead].active || Main.projectile[chargeLead].owner != player.whoAmI || Main.projectile[chargeLead].type != ModContent.ProjectileType<ChargeLead>())
								{
									mp.statCharge = 0;
								}
							}
						}
						else if (!mp.ballstate)
						{
							mp.statCharge = 0;
						}
					}
					if (isShock && player.controlUseItem && mp.statOverheat < mp.maxOverheat)
					{
						cooldown--;
						mp.overheatDelay = (int)cooldown / 3;
						if (cooldown <= 0)
						{
							pb.statUA -= MGlobalItem.AmmoUsage(player, BeamMods[0].GetGlobalItem<MGlobalItem>().addonUACost * mp.UACost);
							if (mp.statCharge >= MPlayer.maxCharge)
							{
								mp.statOverheat += MGlobalItem.AmmoUsage(player, oHeat);
							}
							cooldown = (int)useTime;
						}
					}
					if (Stealth)
					{
						if (!player.mount.Active)
						{
							player.scope = true;
						}
						if (impStealth < 126f)
						{
							impStealth += 1.5f;
						}
						if (Diff || Lum)
						{
							player.shroomiteStealth = true;
							player.stealth -= impStealth / 126f;
							player.aggro -= (int)(impStealth * 4f);
							if (Main.netMode != NetmodeID.SinglePlayer && mp.Player.whoAmI == Main.myPlayer)
							{
								NetMessage.SendData(84);
							}
						}
						if (player.velocity != Vector2.Zero || player.controlUseItem)
						{
							player.shroomiteStealth = false;
							impStealth = 0f;
						}
						DamageClass damageClass = ModContent.GetInstance<HunterDamageClass>();
						player.GetCritChance(damageClass) += (int)impStealth / (Lum ? 3f : Diff ? 5f : 10f);
					}
					if (isHunter && pb.statUA <= 0f && player.controlUseItem && BeamMods[0].type != ModContent.ItemType<Addons.Hunters.OmegaCannonAddon>())
					{
						if (!BeamChange[11].IsAir)
						{
							BeamMods[0] = BeamChange[11].Clone();
							BeamChange[11].type = ModContent.ItemType<Addons.V3.LuminiteBeamAddon>();
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonUACost = 0f;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonDmg = 0f;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonChargeDmg = MConfigItems.Instance.damageLuminiteBeam;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonChargeHeat = MConfigItems.Instance.overheatLuminiteBeam;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonHeat = 0f;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonSpeed = 0f;
						}
						else if (!BeamChange[10].IsAir)
						{
							BeamMods[0] = BeamChange[10].Clone();
							BeamChange[10].type = ModContent.ItemType<Addons.V2.ChargeBeamV2Addon>();
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonUACost = 0f;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonDmg = 0f;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonChargeDmg = MConfigItems.Instance.damageChargeBeamV2;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonChargeHeat = MConfigItems.Instance.overheatChargeBeamV2;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonHeat = 0f;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonSpeed = 0f;
						}
						else if (!BeamChange[0].IsAir)
						{
							BeamMods[0] = BeamChange[0].Clone();
							BeamChange[0].type = ModContent.ItemType<Addons.ChargeBeamAddon>();
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonUACost = 0f;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonDmg = 0f;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonChargeDmg = MConfigItems.Instance.damageChargeBeam;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonChargeHeat = MConfigItems.Instance.overheatChargeBeam;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonHeat = 0f;
							BeamMods[0].GetGlobalItem<MGlobalItem>().addonSpeed = 0f;
						}
						else
						{
							BeamMods[0].TurnToAir();
						}
					}
					
				}
				else
				{
					if (Common.Systems.MSystem.SwitchKey.JustPressed)
					{
						mp.missileChangeActive = !mp.missileChangeActive;
						//SoundEngine.PlaySound(Sounds.Items.Weapons.BeamSelectFail);
						if (mp.missileChangeActive)
						{
							SoundEngine.PlaySound(Sounds.Items.Weapons.BeamSelect);
						}
						if (!mp.missileChangeActive)
						{
							SoundEngine.PlaySound(Sounds.Items.Weapons.BeamSelectFail);
						}
					}

					int chCost = (int)(chargeCost * (mp.missileCost + 0.001f));
					comboCostUseTime = (int)Math.Round(60.0 / (double)(comboDrain * mp.missileCost));
					isCharge &= (pb.statMissiles >= chCost || (isHeldCombo > 0 && initialShot));

					Item.autoReuse = (isCharge || isSeeker);

					if (isCharge)
					{
						if (!mp.ballstate && !mp.shineActive && !player.dead && !player.noItems)
						{
							if (player.gravDir == -1f)
								MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;


							float dmgMult = chargeMult;

							//if (player.controlUseItem && chargeLead != -1 && Main.projectile[chargeLead].active && Main.projectile[chargeLead].owner == player.whoAmI && Main.projectile[chargeLead].type == mod.ProjectileType("ChargeLead"))
							if (player.controlUseItem && LeadActive(player, ModContent.ProjectileType<ChargeLead>()))
							{
								if (mp.statCharge < MPlayer.maxCharge)
								{
									mp.statCharge = Math.Min(mp.statCharge + 1, MPlayer.maxCharge);
								}
								if (isHeldCombo > 0)
								{
									if (pb.statMissiles > 0)
									{
										if (mp.statCharge >= MPlayer.maxCharge)
										{
											if (isMiniGun)
											{
												this.MiniGunShoot(player, Item, Main.projectile[chargeLead], Mod.Find<ModProjectile>(chargeShot).Type, (int)((float)damage * dmgMult), comboKnockBack, chargeShotSound);
											}
											else
											{
												if (comboTime <= 0)
												{
													var entitySource = player.GetSource_ItemUse(Item);
													for (int i = 0; i < comboShotAmt; i++)
													{
														int proj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(chargeShot).Type, (int)((float)damage * dmgMult), comboKnockBack, player.whoAmI);
														Main.projectile[proj].ai[0] = chargeLead;
													}
													comboTime = comboUseTime;
												}

												if (isHeldCombo == 2 && comboTime > 0)
												{
													comboTime--;
												}
											}

											if (!initialShot)
											{
												if (useFlameSounds || useVortexSounds)
												{
													int type = ModContent.ProjectileType<FlamethrowerLead>();
													if (useVortexSounds)
													{
														type = ModContent.ProjectileType<VortexComboLead>();
													}
													int proj = Projectile.NewProjectile(player.GetSource_ItemUse(Item), oPos.X, oPos.Y, velocity.X, velocity.Y, type, 0, 0, player.whoAmI);
													Main.projectile[proj].ai[0] = chargeLead;
												}

												pb.statMissiles = Math.Max(pb.statMissiles - (int)Math.Round(MGlobalItem.AmmoUsage(player, chCost)), 0);

												initialShot = true;
											}

											if (comboCostUseTime > 0)
											{
												//if(comboCostTime <= 0)
												if (comboCostTime > comboCostUseTime)
												{
													pb.statMissiles = Math.Max(pb.statMissiles -= (int)Math.Round(MGlobalItem.AmmoUsage(player, 1)), 0);
													//comboCostTime = comboCostUseTime;
													comboCostTime = 0;
												}
												else
												{
													//comboCostTime--;
													comboCostTime++;
												}
											}
										}
									}
									else
									{
										Main.projectile[chargeLead].Kill();
									}
								}
							}
							else
							{
								if (mp.statCharge <= 0 && LeadActive(player, ModContent.ProjectileType<ChargeLead>()))
								{
									mp.statCharge++;
								}
								if (isHeldCombo <= 0 || mp.statCharge < MPlayer.maxCharge)
								{
									var entitySource = player.GetSource_ItemUse(Item);
									if (mp.statCharge >= MPlayer.maxCharge && pb.statMissiles >= chCost)
									{
										if (isShotgun)
										{
											for (int i = 0; i < shotgunAmt; i++)
											{
												int k = i - (shotgunAmt / 2);
												Vector2 shotGunVel = Vector2.Normalize(velocity) * (Item.shootSpeed + 4f);
												double rot = Angle.ConvertToRadians(4.0 * k);
												shotGunVel = shotGunVel.RotatedBy(rot, default(Vector2));
												if (float.IsNaN(shotGunVel.X) || float.IsNaN(shotGunVel.Y))
												{
													shotGunVel = -Vector2.UnitY;
												}
												int chargeProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, shotGunVel.X, shotGunVel.Y, Mod.Find<ModProjectile>(chargeShot).Type, (int)((float)damage * dmgMult), Item.knockBack, player.whoAmI);
											}
										}
										if (isHoming)
										{
											int shotProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(shot).Type, damage * 2, Item.knockBack, player.whoAmI);
											MProjectile mProj = (MProjectile)Main.projectile[shotProj].ModProjectile;
											mProj.homing = true;
											//mProj.Projectile.netUpdate2 = true;
											//mi.statMissiles = Math.Max(mi.statMissiles -= (int)Math.Round(MGlobalItem.AmmoUsage(player, 2)), 0);
										}
										else if (!isShotgun) //dont know why this needs to be this way but it do
										{
											int chargeProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(chargeShot).Type, (int)((float)damage * dmgMult), Item.knockBack, player.whoAmI);
										}
										pb.statMissiles -= (int)Math.Round(MGlobalItem.AmmoUsage(player, chCost));
									}
									else if (mp.statCharge > 0)
									{
										int shotProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(shot).Type, damage, Item.knockBack, player.whoAmI);
										pb.statMissiles -= (int)Math.Round(MGlobalItem.AmmoUsage(player, 1));
									}
								}

								if (!LeadActive(player, ModContent.ProjectileType<ChargeLead>()))
								{
									mp.statCharge = 0;
								}

								comboTime = 0;
								comboCostTime = 0;
								scalePlus = 0f;
								initialShot = false;
							}
						}
						else if (!mp.ballstate)
						{
							mp.statCharge = 0;
							comboTime = 0;
							comboCostTime = 0;
							scalePlus = 0f;
							initialShot = false;
						}
					}
					else
					{
						mp.statCharge = 0;
						comboTime = 0;
						comboCostTime = 0;
						scalePlus = 0f;
						initialShot = false;
					}

					if (targetingDelay > 0)
						targetingDelay--;

					if (isSeeker && !mp.ballstate && !mp.shineActive && !player.dead && !player.noItems)
					{
						if (player.gravDir == -1f)
						{
							MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
						}
						Rectangle mouse = new Rectangle((int)MX - 1, (int)MY - 1, 2, 2);
						//if (player.controlUseItem && chargeLead != -1 && Main.projectile[chargeLead].active && Main.projectile[chargeLead].owner == player.whoAmI && Main.projectile[chargeLead].type == mod.ProjectileType("SeekerMissileLead"))
						if (player.controlUseItem && LeadActive(player, ModContent.ProjectileType<SeekerMissileLead>()))
						{
							if (pb.seekerCharge < MGlobalItem.seekerMaxCharge)
							{
								pb.seekerCharge = Math.Min(pb.seekerCharge + 1, MGlobalItem.seekerMaxCharge);
							}
							else
							{
								for (int i = 0; i < Main.maxNPCs; i++)
								{
									NPC npc = Main.npc[i];
									if (npc.active && npc.chaseable && !npc.dontTakeDamage && !npc.friendly)// && !npc.immortal)
									{
										Rectangle npcRect = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
										bool flag = false;
										for (int j = 0; j < pb.seekerTarget.Length; j++)
										{
											if (pb.seekerTarget[j] == npc.whoAmI)
											{
												flag = true;
											}
										}

										Vector2 delta = new Vector2(MX, MY);
										delta.X -= MathHelper.Clamp(MX, npcRect.X, npcRect.X + npcRect.Width);
										delta.Y -= MathHelper.Clamp(MY, npcRect.Y, npcRect.Y + npcRect.Height);
										bool colFlag = (delta.Length() < 50);
										if (colFlag && pb.seekerTarget[targetNum] <= -1 && ((targetingDelay <= 0 && mouse.Intersects(npcRect)) || !flag) && pb.statMissiles > pb.numSeekerTargets)
										{
											pb.seekerTarget[targetNum] = npc.whoAmI;
											targetNum++;
											if (targetNum > 4)
											{
												targetNum = 0;
											}
											targetingDelay = 40;
											SoundEngine.PlaySound(Sounds.Items.Weapons.SeekerLockSound, oPos);
										}
									}
								}

								int num = 10;
								while (pb.seekerTarget[targetNum] > -1 && num > 0)
								{
									targetNum++;
									if (targetNum > 4)
									{
										targetNum = 0;
									}
									num--;
								}

								pb.numSeekerTargets = 0;
								for (int i = 0; i < pb.seekerTarget.Length; i++)
								{
									if (pb.seekerTarget[i] > -1)
									{
										pb.numSeekerTargets++;

										if (!Main.npc[pb.seekerTarget[i]].active)
										{
											pb.seekerTarget[i] = -1;
										}
									}
								}
							}
						}
						else
						{
							if (pb.seekerCharge <= 0 && LeadActive(player, ModContent.ProjectileType<SeekerMissileLead>()))
							{
								pb.seekerCharge++;
							}
							if (pb.seekerCharge >= MGlobalItem.seekerMaxCharge && pb.numSeekerTargets > 0)
							{
								var entitySource = player.GetSource_ItemUse(Item);
								for (int i = 0; i < pb.seekerTarget.Length; i++)
								{
									if (pb.seekerTarget[i] > -1)
									{
										int shotProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(shot).Type, damage, Item.knockBack, player.whoAmI);
										MProjectile mProj = (MProjectile)Main.projectile[shotProj].ModProjectile;
										mProj.seekTarget = pb.seekerTarget[i];
										mProj.seeking = true;
										mProj.Projectile.netUpdate2 = true;
										pb.statMissiles = Math.Max(pb.statMissiles -= (int)Math.Round(MGlobalItem.AmmoUsage(player, 1)), 0);
									}
								}

								SoundEngine.PlaySound(Sounds.Items.Weapons.SeekerMissileSound, oPos);
							}
							else if (pb.seekerCharge > 0)
							{
								var entitySource = player.GetSource_ItemUse(Item);
								int shotProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(shot).Type, damage, Item.knockBack, player.whoAmI);
								SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/{shotSound}"), oPos);

								pb.statMissiles -= 1;
							}
							if (!LeadActive(player, ModContent.ProjectileType<SeekerMissileLead>()))
							{
								pb.seekerCharge = 0;
							}
							pb.numSeekerTargets = 0;
							for (int k = 0; k < pb.seekerTarget.Length; k++)
							{
								pb.seekerTarget[k] = -1;
							}
							targetNum = 0;
							targetingDelay = 0;
						}
					}
					else
					{
						pb.seekerCharge = 0;
						pb.numSeekerTargets = 0;
						for (int k = 0; k < pb.seekerTarget.Length; k++)
						{
							pb.seekerTarget[k] = -1;
						}
						targetNum = 0;
						targetingDelay = 0;
					}
					if(pb.statMissiles <= 0f && player.controlUseItem)
					{
						pb.isBeam = !pb.isBeam;
					}
				}

			}
		}
		SoundEffectInstance soundInstance;
		public void MiniGunShoot(Player player, Item item, Projectile Lead, int projType, int damage, float knockBack, string sound)
		{
			if (comboTime <= 0)
			{
				SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/{sound}"), player.Center), out ActiveSound result);
				soundInstance = result.Sound;
				if (soundInstance != null)
				{
					soundInstance.Volume *= 1f - 0.25f * (scalePlus / 20f);
				}

				float spray = 1f * (scalePlus / 20f);

				float scaleFactor2 = 14f;

				var entitySource = Lead.GetSource_FromAI();
				for (int i = 0; i < miniGunAmt; i++)
				{
					float rot = Lead.velocity.ToRotation() + (float)Angle.ConvertToRadians(Main.rand.Next(18) * 10) - (float)Math.PI / 2f;
					Vector2 vector3 = Lead.Center + rot.ToRotationVector2() * 7f * spray;
					Vector2 vector5 = Vector2.Normalize(Lead.velocity) * scaleFactor2;
					vector5 = vector5.RotatedBy((Main.rand.NextDouble() * 0.12 - 0.06) * spray, default(Vector2));
					if (float.IsNaN(vector5.X) || float.IsNaN(vector5.Y))
					{
						vector5 = -Vector2.UnitY;
					}
					int proj = Projectile.NewProjectile(entitySource, vector3.X, vector3.Y, vector5.X, vector5.Y, projType, damage, knockBack, player.whoAmI, 0f, 0f);
					Main.projectile[proj].ai[0] = Lead.whoAmI;
					MProjectile mProj = (MProjectile)Main.projectile[proj].ModProjectile;
					mProj.waveDir = waveDir;
				}

				waveDir *= -1;

				comboTime = comboUseTime;
			}
			else
			{
				comboTime--;
			}
			scalePlus = Math.Min(scalePlus + (2f / comboUseTime), 20f);
			ChargeLead chLead = (ChargeLead)Lead.ModProjectile;
			chLead.extraScale = 0.3f * (scalePlus / 20f);
		}
		public override void SaveData(TagCompound tag)
		{
			for (int i = 0; i < BeamMods.Length; ++i)
			{
				// Failsave check.
				if (BeamMods[i] == null)
				{
					BeamMods[i] = new Item();
				}
				tag.Add("BeamItem" + i, ItemIO.Save(BeamMods[i]));
			}
			for (int i = 0; i < BeamChange.Length; ++i)
			{
				// Failsave check.
				if (BeamChange[i] == null)
				{
					BeamChange[i] = new Item();
				}
				tag.Add("BeamChange" + i, ItemIO.Save(BeamChange[i]));
			}
			if (Item.TryGetGlobalItem(out MGlobalItem pb))
			{
				tag.Add("statUA", pb.statUA);
				tag.Add("maxUA", pb.maxUA);
			}
			else
			{
				tag.Add("statUA", 0);
				tag.Add("maxUA", 0);
			}
			for (int i = 0; i < MissileMods.Length; ++i)
			{
				tag.Add("missileItem" + i, ItemIO.Save(MissileMods[i]));
			}

			if (Item.TryGetGlobalItem(out MGlobalItem mi))
			{
				tag.Add("statMissiles", mi.statMissiles);
				tag.Add("maxMissiles", mi.maxMissiles);
			}
			else
			{
				tag.Add("statMissiles", 0);
				tag.Add("maxMissiles", 0);
			}
			for (int i = 0; i < MissileChange.Length; ++i)
			{
				// Failsave check.
				if (MissileChange[i] == null)
				{
					MissileChange[i] = new Item();
				}
				tag.Add("MissileChange" + i, ItemIO.Save(MissileChange[i]));
			}
		}
		public override void LoadData(TagCompound tag)
		{
			try
			{
				BeamMods = new Item[MetroidMod.beamSlotAmount];
				for (int i = 0; i < BeamMods.Length; i++)
				{
					Item item = tag.Get<Item>("BeamItem" + i);
					BeamMods[i] = item;
				}
				BeamChange = new Item[MetroidMod.beamChangeSlotAmount];
				for (int i = 0; i < BeamChange.Length; i++)
				{
					Item item = tag.Get<Item>("BeamChange" + i);
					BeamChange[i] = item;
				}
				MGlobalItem pb = Item.GetGlobalItem<MGlobalItem>();
				pb.statUA = tag.GetFloat("statUA");
				pb.maxUA = tag.GetInt("maxUA");
				MissileMods = new Item[MetroidMod.missileSlotAmount];
				for (int i = 0; i < MissileMods.Length; i++)
				{
					Item item = tag.Get<Item>("missileItem" + i);
					MissileMods[i] = item;
				}

				MGlobalItem mi = Item.GetGlobalItem<MGlobalItem>();
				mi.statMissiles = tag.GetInt("statMissiles");
				mi.maxMissiles = tag.GetInt("maxMissiles");
				MissileChange = new Item[MetroidMod.missileChangeSlotAmount];
				for (int i = 0; i < MissileChange.Length; i++)
				{
					Item item = tag.Get<Item>("MissileChange" + i);
					MissileChange[i] = item;
				}
			}
			catch { }
		}

		public override void OnCreated(ItemCreationContext context)
		{
			base.OnCreated(context);
			_beamMods = new Item[BeamAddonSlotID.Count];
			_beamchangeMods = new Item[BeamChangeSlotID.Count];
			for (int i = 0; i < _beamMods.Length; ++i)
			{
				_beamMods[i] = new Item();
				_beamMods[i].TurnToAir();
			}
			for (int i = 0; i < _beamchangeMods.Length; ++i)
			{
				_beamchangeMods[i] = new Item();
				_beamchangeMods[i].TurnToAir();
			}
		}
		public override void NetSend(BinaryWriter writer)
		{
			for (int i = 0; i < BeamMods.Length; ++i)
			{
				ItemIO.Send(BeamMods[i], writer,true);
			}
			for (int i = 0; i < BeamChange.Length; ++i)
			{
				ItemIO.Send(BeamChange[i], writer);
			}
			for (int i = 0; i < MissileMods.Length; ++i)
			{
				ItemIO.Send(MissileMods[i], writer, true);
			}
			for (int i = 0; i < MissileChange.Length; ++i)
			{
				ItemIO.Send(MissileChange[i], writer);
			}
			writer.Write(chargeLead);
		}
		public override void NetReceive(BinaryReader reader)
		{
			for (int i = 0; i < BeamMods.Length; ++i)
			{
				BeamMods[i] = ItemIO.Receive(reader, true);
			}
			for (int i = 0; i < BeamChange.Length; ++i)
			{
				BeamChange[i] = ItemIO.Receive(reader);
			}
			for (int i = 0; i < MissileMods.Length; ++i)
			{
				MissileMods[i] = ItemIO.Receive(reader, true);
			}
			for (int i = 0; i < MissileChange.Length; ++i)
			{
				MissileChange[i] = ItemIO.Receive(reader);
			}
			chargeLead = reader.ReadInt32();
		}
	}
}
