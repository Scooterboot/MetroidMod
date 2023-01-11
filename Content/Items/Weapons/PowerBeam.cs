using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Projectiles;
using MetroidMod.Content.Projectiles.powerbeam;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Default;
using Terraria.Utilities;

namespace MetroidMod.Content.Items.Weapons
{
	public class PowerBeam : ModItem
	{
		// Failsaves.
		private Item[] _beamMods;
		public Item[] BeamMods
		{
			get
			{
				if (_beamMods == null)
				{
					_beamMods = new Item[5];
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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Beam");
			Tooltip.SetDefault("Select this item in your hotbar and open your inventory to open the Beam Addon UI");
			SacrificeTotal = 1;

			BeamMods = new Item[5];
		}
		public override void SetDefaults()
		{
			Item.damage = Common.Configs.MConfigItems.Instance.damagePowerBeam;
			Item.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Item.width = 24;
			Item.height = 12;
			Item.scale = 0.8f;
			Item.useTime = Common.Configs.MConfigItems.Instance.useTimePowerBeam;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 20000;
			Item.rare = ItemRarityID.Green;
			//Item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/PowerBeamSound");
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<PowerBeamShot>();
			Item.shootSpeed = 8f;
			Item.crit = 3;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(8)
				.AddIngredient<Miscellaneous.EnergyShard>(3)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 8);
			recipe.AddIngredient(null, "EnergyShard", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
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
			return mp.statOverheat < mp.maxOverheat;// && BeamLoader.CanShoot(player, BeamMods);
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
			PrefixLoader.Roll(Item, ref output, 14, rand, new PrefixCategory[] { PrefixCategory.AnyWeapon, PrefixCategory.Custom });
			return output;
		}

		public override void OnResearched(bool fullyResearched)
		{
			foreach(Item item in BeamMods)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnClonedItem(itemSource_OpenItem, item, item.stack);
			}
		}

		public override bool PreReforge()
		{
			foreach (Item item in BeamMods)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnClonedItem(itemSource_OpenItem, item, item.stack);
			}
			BeamMods = new Item[5];
			return base.PreReforge();
		}

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
		private float hunterSpeed = 0f;

		private int finalDmg = Common.Configs.MConfigItems.Instance.damagePowerBeam;

		private float chargeDmgMult = 3f;
		private float chargeCost = 2f;

		private int overheat = Common.Configs.MConfigItems.Instance.overheatPowerBeam;
		private int useTime = Common.Configs.MConfigItems.Instance.useTimePowerBeam;

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
		private int shotAmt = 1;
		private int chargeShotAmt = 1;

		public SoundStyle? ShotSound;
		public SoundStyle? ChargeShotSound;

		private int waveDir = -1;

		private bool isImp = false;
		private bool isCharge = false;
		private bool isHyper = false;
		private bool isPhazon = false;

		public bool comboError1, comboError2, comboError3, comboError4;

		private string altTexture => texture + "_alt";
		private string texture = "";
		//Mod modBeamTextureMod = null;

		public override void UpdateInventory(Player P)
		{
			//MPlayer mp = P.GetModPlayer<MPlayer>();

			int ch = ModContent.ItemType<Addons.ChargeBeamAddon>();
			int ic = ModContent.ItemType<Addons.IceBeamAddon>();
			int wa = ModContent.ItemType<Addons.WaveBeamAddon>();
			int sp = ModContent.ItemType<Addons.SpazerAddon>();
			int plR = ModContent.ItemType<Addons.PlasmaBeamRedAddon>();
			int plG = ModContent.ItemType<Addons.PlasmaBeamGreenAddon>();

			int ch2 = ModContent.ItemType<Addons.V2.ChargeBeamV2Addon>();
			int ic2 = ModContent.ItemType<Addons.V2.IceBeamV2Addon>();
			int wa2 = ModContent.ItemType<Addons.V2.WaveBeamV2Addon>();
			int wi = ModContent.ItemType<Addons.V2.WideBeamAddon>();
			int nv = ModContent.ItemType<Addons.V2.NovaBeamAddon>();

			int ch3 = ModContent.ItemType<Addons.V3.LuminiteBeamAddon>();
			int sd = ModContent.ItemType<Addons.V3.StardustBeamAddon>();
			int nb = ModContent.ItemType<Addons.V3.NebulaBeamAddon>();
			int vt = ModContent.ItemType<Addons.V3.VortexBeamAddon>();
			int sl = ModContent.ItemType<Addons.V3.SolarBeamAddon>();

			int hy = ModContent.ItemType<Addons.HyperBeamAddon>();
			int ph = ModContent.ItemType<Addons.PhazonBeamAddon>();

			int vd = ModContent.ItemType<Addons.Hunters.VoltDriverAddon>();
			int jd = ModContent.ItemType<Addons.Hunters.JudicatorAddon>();
			int bh = ModContent.ItemType<Addons.Hunters.BattleHammerAddon>();
			int mm = ModContent.ItemType<Addons.Hunters.MagMaulAddon>();
			int imp = ModContent.ItemType<Addons.Hunters.ImperialistAddon>();
			int sc = ModContent.ItemType<Addons.Hunters.ShockCoilAddon>();
			int oc = ModContent.ItemType<Addons.Hunters.OmegaCannonAddon>();

			Item slot1 = BeamMods[0];
			Item slot2 = BeamMods[1];
			Item slot3 = BeamMods[2];
			Item slot4 = BeamMods[3];
			Item slot5 = BeamMods[4];

			int damage = Common.Configs.MConfigItems.Instance.damagePowerBeam;
			overheat = Common.Configs.MConfigItems.Instance.overheatPowerBeam;
			useTime = Common.Configs.MConfigItems.Instance.useTimePowerBeam;
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
			//modBeamTextureMod = null;

			ShotSound = null;
			ChargeShotSound = null;

			isCharge = (slot1.type == ch || slot1.type == ch2 || slot1.type == ch3);
			isHyper = (slot1.type == hy);
			isPhazon = (slot1.type == ph);

			comboError1 = false;
			comboError2 = false;
			comboError3 = false;
			comboError4 = false;

			bool chargeV1 = (slot1.type == ch),
				chargeV2 = (slot1.type == ch2),
				chargeV3 = (slot1.type == ch3);

			bool addonsV1 = (slot2.type == ic || slot3.type == wa || slot4.type == sp || ((slot5.type == plG || slot5.type == plR) && !chargeV2 && !chargeV3));
			bool addonsV2 = (slot2.type == ic2 || slot3.type == wa2 || slot4.type == wi || slot5.type == nv);
			addonsV2 |= ((slot5.type == plG || slot5.type == plR) && (chargeV2 || chargeV3) && !addonsV1);
			bool addonsV3 = (slot2.type == sd || slot3.type == nb || slot4.type == vt || slot5.type == sl);

			int versionType = 1;
			if (addonsV3 || (chargeV3 && !addonsV1 && !addonsV2))
			{
				versionType = 3;
			}
			else if (addonsV2 || (chargeV2 && !addonsV1))
			{
				versionType = 2;
			}


			// Default Combos
			if (!isHyper && !isPhazon)
			{
				//if(slot1.type != ch2 && slot1.type != ch3 && (slot1.type == ch || slot2.type == ic || slot3.type == wa || slot4.type == sp || slot5.type == plG || slot5.type == plR))
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

						// Ice Wave
						if (slot3.type == wa)
						{
							shot = "IceWaveBeamShot";
							chargeShot = "IceWaveBeamChargeShot";
							chargeShotAmt = 2;

							// Ice Wave Spazer
							if (slot4.type == sp)
							{
								shot = "IceWaveSpazerShot";
								chargeShot = "IceWaveSpazerChargeShot";
								shotSound = "IceComboSound";
								shotAmt = 3;
								chargeShotAmt = 3;

								// Ice Wave Spazer Plasma (Green)
								if (slot5.type == plG)
								{
									shot = "IceWaveSpazerPlasmaBeamGreenShot";
									chargeShot = "IceWaveSpazerPlasmaBeamGreenChargeShot";
									shotSound = "IceComboSound";
								}
								// Ice Wave Spazer Plasma (Red)
								if (slot5.type == plR)
								{
									shot = "IceWaveSpazerPlasmaBeamRedShot";
									chargeShot = "IceWaveSpazerPlasmaBeamRedChargeShot";
									shotSound = "IceComboSound";
									dustType = 135;
								}
							}
							else
							{
								// Ice Wave Plasma (Green)
								if (slot5.type == plG)
								{
									shot = "IceWavePlasmaBeamGreenShot";
									chargeShot = "IceWavePlasmaBeamGreenChargeShot";
									shotSound = "IceComboSound";
									shotAmt = 2;
								}
								// Ice Wave Plasma (Red)
								if (slot5.type == plR)
								{
									shot = "IceWavePlasmaBeamRedShot";
									chargeShot = "IceWavePlasmaBeamRedChargeShot";
									shotSound = "IceComboSound";
									shotAmt = 2;
									dustType = 135;
								}
							}
						}
						else
						{
							// Ice Spazer
							if (slot4.type == sp)
							{
								shot = "IceSpazerShot";
								chargeShot = "IceSpazerChargeShot";
								shotSound = "IceComboSound";
								shotAmt = 3;
								chargeShotAmt = 3;

								// Ice Spazer Plasma (Green)
								if (slot5.type == plG)
								{
									shot = "IceSpazerPlasmaBeamGreenShot";
									chargeShot = "IceSpazerPlasmaBeamGreenChargeShot";
									shotSound = "IceComboSound";
								}
								// Ice Spazer Plasma (Red)
								if (slot5.type == plR)
								{
									shot = "IceSpazerPlasmaBeamRedShot";
									chargeShot = "IceSpazerPlasmaBeamRedChargeShot";
									shotSound = "IceComboSound";
									dustType = 135;
								}
							}
							else
							{
								// Ice Plasma (Green)
								if (slot5.type == plG)
								{
									shot = "IcePlasmaBeamGreenShot";
									chargeShot = "IcePlasmaBeamGreenChargeShot";
									shotSound = "IceComboSound";
								}
								// Ice Plasma (Red)
								if (slot5.type == plR)
								{
									shot = "IcePlasmaBeamRedShot";
									chargeShot = "IcePlasmaBeamRedChargeShot";
									shotSound = "IceComboSound";
									dustType = 135;
								}
							}
						}
					}
					else
					{
						// Wave
						if (slot3.type == wa)
						{
							shot = "WaveBeamShot";
							chargeShot = "WaveBeamChargeShot";
							shotSound = "WaveBeamSound";
							chargeShotSound = "WaveBeamChargeSound";
							chargeUpSound = "ChargeStartup_Wave";
							chargeTex = "ChargeLead_Wave";
							dustType = 62;
							lightColor = MetroidMod.waveColor;
							chargeShotAmt = 2;
							texture = "WaveBeam";

							// Wave Spazer
							if (slot4.type == sp)
							{
								shot = "WaveSpazerShot";
								chargeShot = "WaveSpazerChargeShot";
								shotSound = "SpazerSound";
								shotAmt = 3;
								chargeShotAmt = 3;

								// Wave Spazer Plasma (Green)
								if (slot5.type == plG)
								{
									shot = "WaveSpazerPlasmaBeamGreenShot";
									chargeShot = "WaveSpazerPlasmaBeamGreenChargeShot";
									shotSound = "PlasmaBeamGreenSound";
									chargeShotSound = "PlasmaBeamGreenChargeSound";
									chargeUpSound = "ChargeStartup_Power";
									chargeTex = "ChargeLead_PlasmaGreen";
									dustType = 61;
									lightColor = MetroidMod.plaGreenColor;
									texture = "PlasmaBeamG";
								}
								// Wave Spazer Plasma (Red)
								if (slot5.type == plR)
								{
									shot = "WaveSpazerPlasmaBeamRedShot";
									chargeShot = "WaveSpazerPlasmaBeamRedChargeShot";
									shotSound = "PlasmaBeamRedSound";
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
								// Wave Plasma (Green)
								if (slot5.type == plG)
								{
									shot = "WavePlasmaBeamGreenShot";
									chargeShot = "WavePlasmaBeamGreenChargeShot";
									shotSound = "PlasmaBeamGreenSound";
									chargeShotSound = "PlasmaBeamGreenChargeSound";
									chargeUpSound = "ChargeStartup_Power";
									chargeTex = "ChargeLead_PlasmaGreen";
									dustType = 61;
									lightColor = MetroidMod.plaGreenColor;
									shotAmt = 2;
									texture = "PlasmaBeamG";
								}
								// Wave Plasma (Red)
								if (slot5.type == plR)
								{
									shot = "WavePlasmaBeamRedShot";
									chargeShot = "WavePlasmaBeamRedChargeShot";
									shotSound = "PlasmaBeamRedSound";
									chargeShotSound = "PlasmaBeamRedChargeSound";
									chargeUpSound = "ChargeStartup_PlasmaRed";
									chargeTex = "ChargeLead_PlasmaRed";
									dustType = 6;
									lightColor = MetroidMod.plaRedColor;
									shotAmt = 2;
									texture = "PlasmaBeamR";
								}
							}
						}
						else
						{
							// Spazer
							if (slot4.type == sp)
							{
								shot = "SpazerShot";
								chargeShot = "SpazerChargeShot";
								shotSound = "SpazerSound";
								chargeShotSound = "SpazerChargeSound";
								chargeTex = "ChargeLead_Spazer";
								shotAmt = 3;
								chargeShotAmt = 3;
								texture = "Spazer";

								// Spazer Plasma (Green)
								if (slot5.type == plG)
								{
									shot = "SpazerPlasmaBeamGreenShot";
									chargeShot = "SpazerPlasmaBeamGreenChargeShot";
									shotSound = "PlasmaBeamGreenSound";
									chargeShotSound = "PlasmaBeamGreenChargeSound";
									chargeTex = "ChargeLead_PlasmaGreen";
									dustType = 61;
									lightColor = MetroidMod.plaGreenColor;
									texture = "PlasmaBeamG";
								}
								// Spazer Plasma (Red)
								if (slot5.type == plR)
								{
									shot = "SpazerPlasmaBeamRedShot";
									chargeShot = "SpazerPlasmaBeamRedChargeShot";
									shotSound = "PlasmaBeamRedSound";
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
								// Plasma (Green)
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
								// Plasma (Red)
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
				}
				// Charge V2
				//else if(slot1.type != ch3 && (slot1.type == ch2 || slot2.type == ic2 || slot3.type == wa2 || slot4.type == wi || slot5.type == nv || slot5.type == plG || slot5.type == plR))
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
							//shotAmt = 2;
							chargeShotAmt = 2;

							// Ice Wave Wide
							if (slot4.type == wi)
							{
								shot = "IceWaveWideBeamShot";
								chargeShot = "IceWaveWideBeamChargeShot";
								shotAmt = 3;
								chargeShotAmt = 3;

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
								// Ice Wave Nova
								if (slot5.type == nv)
								{
									shot = "IceWaveNovaBeamShot";
									chargeShot = "IceWaveNovaBeamChargeShot";
									shotAmt = 2;
								}
								// Ice Wave Plasma (Green)
								if (slot5.type == plG)
								{
									shot = "IceWavePlasmaBeamGreenV2Shot";
									chargeShot = "IceWavePlasmaBeamGreenV2ChargeShot";
									shotAmt = 2;
								}
								// Ice Wave Plasma (Red)
								if (slot5.type == plR)
								{
									shot = "IceWavePlasmaBeamRedV2Shot";
									chargeShot = "IceWavePlasmaBeamRedV2ChargeShot";
									shotAmt = 2;
									dustType = 135;
								}
							}
						}
						else
						{
							// Ice Wide
							if (slot4.type == wi)
							{
								shot = "IceWideBeamShot";
								chargeShot = "IceWideBeamChargeShot";
								shotAmt = 3;
								chargeShotAmt = 3;

								// Ice Wide Nova
								if (slot5.type == nv)
								{
									shot = "IceWideNovaBeamShot";
									chargeShot = "IceWideNovaBeamChargeShot";
								}
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
							else
							{
								// Ice Nova
								if (slot5.type == nv)
								{
									shot = "IceNovaBeamShot";
									chargeShot = "IceNovaBeamChargeShot";
								}
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
							//shotAmt = 2;
							chargeShotAmt = 2;
							texture = "WaveBeam";

							// Wave Wide
							if (slot4.type == wi)
							{
								shot = "WaveWideBeamShot";
								chargeShot = "WaveWideBeamChargeShot";
								shotAmt = 3;
								chargeShotAmt = 3;

								// Wave Wide Nova
								if (slot5.type == nv)
								{
									shot = "WaveWideNovaBeamShot";
									chargeShot = "WaveWideNovaBeamChargeShot";
									shotSound = "NovaBeamSound";
									chargeShotSound = "NovaBeamChargeSound";
									chargeUpSound = "ChargeStartup_Nova";
									chargeTex = "ChargeLead_Nova";
									dustType = 75;
									lightColor = MetroidMod.novColor;
									texture = "NovaBeam";
								}
								// Wave Wide Plasma (Green)
								if (slot5.type == plG)
								{
									shot = "WaveWidePlasmaBeamGreenV2Shot";
									chargeShot = "WaveWidePlasmaBeamGreenV2ChargeShot";
									shotSound = "WavePlasmaBeamGreenSound";
									chargeShotSound = "PlasmaBeamGreenChargeSound";
									chargeUpSound = "ChargeStartup_Power";
									chargeTex = "ChargeLead_PlasmaGreenV2";
									dustType = 15;
									lightColor = MetroidMod.plaGreenColor;
									texture = "PlasmaBeamG";
								}
								// Wave Wide Plasma (Red)
								if (slot5.type == plR)
								{
									shot = "WaveWidePlasmaBeamRedV2Shot";
									chargeShot = "WaveWidePlasmaBeamRedV2ChargeShot";
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
								// Wave Nova
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
									shotAmt = 2;
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
									shotAmt = 2;
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
									shotAmt = 2;
									texture = "PlasmaBeamR";
								}
							}
						}
						else
						{
							// Wide
							if (slot4.type == wi)
							{
								shot = "WideBeamShot";
								chargeShot = "WideBeamChargeShot";
								shotSound = "WideBeamSound";
								chargeShotSound = "SpazerChargeSound";
								chargeTex = "ChargeLead_Wide";
								dustType = 63;
								lightColor = MetroidMod.wideColor;
								dustColor = MetroidMod.wideColor;
								shotAmt = 3;
								chargeShotAmt = 3;
								texture = "WideBeam";

								// Wide Nova
								if (slot5.type == nv)
								{
									shot = "WideNovaBeamShot";
									chargeShot = "WideNovaBeamChargeShot";
									shotSound = "NovaBeamSound";
									chargeShotSound = "NovaBeamChargeSound";
									chargeUpSound = "ChargeStartup_Nova";
									chargeTex = "ChargeLead_Nova";
									dustType = 75;
									lightColor = MetroidMod.novColor;
									dustColor = default(Color);
									texture = "NovaBeam";
								}
								// Wide Plasma (Green)
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
							else
							{
								// Nova
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
									texture = "NovaBeam";
								}
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
					//ShotSound = SoundID.Item91;
					//chargeShotSound = "IceBeamChargeSound";
					//chargeUpSound = "ChargeStartup_Ice";
					chargeTex = "ChargeLead_Luminite";
					dustType = 229;
					lightColor = MetroidMod.lumColor;

					// Stardust
					if (slot2.type == sd)
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
						if (slot3.type == nb)
						{
							shot = "StardustNebulaBeamShot";
							chargeShot = "StardustNebulaBeamChargeShot";
							shotAmt = 2;
							chargeShotAmt = 2;

							// Stardust Nebula Vortex
							if (slot4.type == vt)
							{
								shot = "StardustNebulaVortexBeamShot";
								chargeShot = "StardustNebulaVortexBeamChargeShot";
								shotAmt = 5;
								chargeShotAmt = 5;

								// Stardust Nebula Vortex Solar
								if (slot5.type == sl)
								{
									shot = "StardustNebulaVortexSolarBeamShot";
									chargeShot = "StardustNebulaVortexSolarBeamChargeShot";
									shotSound = "PlasmaBeamRedV2Sound";
									chargeShotSound = "PlasmaBeamRedChargeSound";
									chargeUpSound = "ChargeStartup_PlasmaRed";
									chargeTex = "ChargeLead_Solar";
									lightColor = MetroidMod.plaRedColor;
									texture = "SolarBeam";
								}
							}
							else
							{
								// Stardust Nebula Solar
								if (slot5.type == sl)
								{
									shot = "StardustNebulaSolarBeamShot";
									chargeShot = "StardustNebulaSolarBeamChargeShot";
									shotSound = "PlasmaBeamRedV2Sound";
									chargeShotSound = "PlasmaBeamRedChargeSound";
									chargeUpSound = "ChargeStartup_PlasmaRed";
									chargeTex = "ChargeLead_Solar";
									lightColor = MetroidMod.plaRedColor;
									texture = "SolarBeam";
								}
							}
						}
						else
						{
							// Stardust Vortex
							if (slot4.type == vt)
							{
								shot = "StardustVortexBeamShot";
								chargeShot = "StardustVortexBeamChargeShot";
								shotAmt = 5;
								chargeShotAmt = 5;

								// Stardust Vortex Solar
								if (slot5.type == sl)
								{
									shot = "StardustVortexSolarBeamShot";
									chargeShot = "StardustVortexSolarBeamChargeShot";
									shotSound = "PlasmaBeamRedV2Sound";
									chargeShotSound = "PlasmaBeamRedChargeSound";
									chargeUpSound = "ChargeStartup_PlasmaRed";
									chargeTex = "ChargeLead_Solar";
									lightColor = MetroidMod.plaRedColor;
									texture = "SolarBeam";
								}
							}
							else
							{
								// Stardust Solar
								if (slot5.type == sl)
								{
									shot = "StardustSolarBeamShot";
									chargeShot = "StardustSolarBeamChargeShot";
									shotSound = "PlasmaBeamRedV2Sound";
									chargeShotSound = "PlasmaBeamRedChargeSound";
									chargeUpSound = "ChargeStartup_PlasmaRed";
									chargeTex = "ChargeLead_Solar";
									lightColor = MetroidMod.plaRedColor;
									texture = "SolarBeam";
								}
							}
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
							shotAmt = 2;
							chargeShotAmt = 2;
							texture = "NebulaBeam";

							// Nebula Vortex
							if (slot4.type == vt)
							{
								shot = "NebulaVortexBeamShot";
								chargeShot = "NebulaVortexBeamChargeShot";
								shotSound = "WideBeamSound";
								shotAmt = 5;
								chargeShotAmt = 5;

								// Nebula Vortex Solar
								if (slot5.type == sl)
								{
									shot = "NebulaVortexSolarBeamShot";
									chargeShot = "NebulaVortexSolarBeamChargeShot";
									shotSound = "PlasmaBeamRedV2Sound";
									chargeShotSound = "PlasmaBeamRedChargeSound";
									chargeUpSound = "ChargeStartup_PlasmaRed";
									chargeTex = "ChargeLead_Solar";
									dustType = 6;
									lightColor = MetroidMod.plaRedColor;
									texture = "SolarBeam";
								}
							}
							else
							{
								// Nebula Solar
								if (slot5.type == sl)
								{
									shot = "NebulaSolarBeamShot";
									chargeShot = "NebulaSolarBeamChargeShot";
									shotSound = "PlasmaBeamRedV2Sound";
									chargeShotSound = "PlasmaBeamRedChargeSound";
									chargeUpSound = "ChargeStartup_PlasmaRed";
									chargeTex = "ChargeLead_Solar";
									dustType = 6;
									lightColor = MetroidMod.plaRedColor;
									texture = "SolarBeam";
								}
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
								shotAmt = 5;
								chargeShotAmt = 5;
								texture = "VortexBeam";

								// Vortex Solar
								if (slot5.type == sl)
								{
									shot = "VortexSolarBeamShot";
									chargeShot = "VortexSolarBeamChargeShot";
									shotSound = "PlasmaBeamRedV2Sound";
									chargeShotSound = "PlasmaBeamRedChargeSound";
									chargeUpSound = "ChargeStartup_PlasmaRed";
									chargeTex = "ChargeLead_Solar";
									dustType = 6;
									lightColor = MetroidMod.plaRedColor;
									texture = "SolarBeam";
								}
							}
							else
							{
								// Solar
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
								}
							}
						}
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
			}
			if (slot1.type == vd)
			{
				isCharge= true;
				shot = "VoltDriverShot";
				chargeShot = "VoltDriverChargeShot";
				shotSound = "VoltDriverSound";
				chargeShotSound = "VoltDriverChargeSound";
				chargeUpSound = "VoltDriverCharge";
				texture = "VoltDriver";
				MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
				mItem.addonChargeDmg = Common.Configs.MConfigItems.Instance.damageChargeBeam;
				mItem.addonChargeHeat = Common.Configs.MConfigItems.Instance.overheatChargeBeam;

				if (!slot2.IsAir)
				{
					comboError1 = true;
				}
				if(!slot3.IsAir)
				{
					comboError1 = true;
				}
				if (!slot5.IsAir)
				{
					comboError4 = true;
				}
                if (slot4.type == sp || slot4.type == wi)
                {
                    shot = "SpazerVoltDriverShot";
                    chargeShot = "SpazerVoltDriverChargeShot";
                }
                if (slot4.type == vt)
                {
                    shot = "VortexVoltDriverShot";
                    chargeShot = "VortexVoltDriverChargeShot";
                }
            }
			if (slot1.type == jd)
			{
				isCharge= true;
				shot = "JudicatorShot";
				chargeShot = "JudicatorChargeShot";
				shotSound = "JudicatorSound";
				chargeShotSound = "JudicatorChargeSound";
				chargeUpSound = "ChargeStartup_JudicatorAffinity";
				texture = "Judicator";
                chargeTex = "ChargeLead_Ice";
                useTime = 15;
				MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
				mItem.addonChargeDmg = Common.Configs.MConfigItems.Instance.damageChargeBeam;
				mItem.addonChargeHeat = Common.Configs.MConfigItems.Instance.overheatChargeBeam;

				if (!slot2.IsAir)
				{
					comboError1 = true;
				}
				if (!slot3.IsAir)
				{
					comboError1 = true;
				}
				if (!slot5.IsAir)
				{
					comboError4 = true;
				}
                if (slot4.type == sp || slot4.type == wi)
                {
                    shot = "SpazerJudicatorShot";
                    chargeShot = "SpazerJudicatorChargeShot";
                }
                if (slot4.type == vt)
                {
                    shot = "VortexJudicatorShot";
                    chargeShot = "VortexJudicatorChargeShot";
                }
            }

			if (slot1.type == bh)
			{
				shot = "BattleHammerShot";
				shotSound = "BattleHammerAffinitySound";
				texture = "BattleHammer";
                MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
				useTime = 15;

				if (!slot2.IsAir)
				{
					comboError1 = true;
				}
				if (!slot3.IsAir)
				{
					comboError1 = true;
				}
				if (!slot5.IsAir)
				{
					comboError4 = true;
				}
                if (slot4.type == sp || slot4.type == wi)
                {
					shot = "SpazerBattleHammerShot";

                }
                if (slot4.type == vt)
                {
					shot = "VortexBattleHammerShot";

                }
            }

			if (slot1.type == imp)
			{
				shot = "ImperialistShot";
				shotSound = "ImperialistSound";
				texture = "Imperialist";
				useTime = 60;
				MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();

				if (!slot2.IsAir)
				{
					comboError1 = true;
				}
				if (!slot3.IsAir)
				{
					comboError1 = true;
				}
				if (!slot5.IsAir)
				{
					comboError4 = true;
				}
                if (slot4.type == sp || slot4.type == wi)
                {
                    shot = "SpazerImperialistShot";
                }
                if (slot4.type == vt)
                {
                    shot = "VortexImperialistShot";
                }
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
				mItem.addonChargeDmg = Common.Configs.MConfigItems.Instance.damageChargeBeam;
				mItem.addonChargeHeat = Common.Configs.MConfigItems.Instance.overheatChargeBeam;
				useTime = 20;

				if (!slot2.IsAir)
				{
					comboError1 = true;
				}
				if (!slot3.IsAir)
				{
					comboError1 = true;
				}
				if (!slot5.IsAir)
				{
					comboError4 = true;
				}
                if (slot4.type == sp || slot4.type == wi)
                {
                    shot = "SpazerMagMaulShot";
                    chargeShot = "SpazerMagMaulChargeShot";
                }
                if (slot4.type == vt)
                {
                    shot = "VortexMagMaulShot";
                    chargeShot = "VortexMagMaulChargeShot";
                }
            }
			if (slot1.type == sc)
			{
				isCharge = true;
				shot = "ShockCoilShot";
				shotSound = "ShockCoilAffinity1";
				texture = "ShockCoil";
                chargeUpSound = "ShockCoilStartupSound";
                chargeShotSound = "ShockCoilLoad";
                chargeShot = "ShockCoilShot";
                chargeTex = "ChargeLead_Stardust";
                MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
                mItem.addonChargeDmg = Common.Configs.MConfigItems.Instance.damageChargeBeam;
                mItem.addonChargeHeat = Common.Configs.MConfigItems.Instance.overheatChargeBeam;

                if (!slot2.IsAir)
				{
					comboError1 = true;
				}
				if (!slot3.IsAir)
				{
					comboError1 = true;
				}
				if (!slot5.IsAir)
				{
					comboError4 = true;
				}
			}
			if (slot1.type == oc)
			{
				shot = "OmegaCannonShot";
				shotSound = "OmegaCannonShotSound";
				texture = "OmegaCannon";
				MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
				useTime = 60;

				if (!slot2.IsAir)
				{
					comboError1 = true;
				}
				if (!slot3.IsAir)
				{
					comboError1 = true;
				}
				if (!slot5.IsAir)
				{
					comboError4 = true;
				}
                if (slot4.type == sp || slot4.type == wi)
                {
                    shot = "SpazerOmegaCannonShot";
                }
                if (slot4.type == vt)
                {
                    shot = "VortexOmegaCannonShot";
                }
            }
			// Hyper
			else if (isHyper)
			{
				shot = "HyperBeamShot";
				shotSound = "HyperBeamSound";
				useTime = Common.Configs.MConfigItems.Instance.useTimeHyperBeam;

				damage = Common.Configs.MConfigItems.Instance.damageHyperBeam;
				overheat = Common.Configs.MConfigItems.Instance.overheatHyperBeam;

				texture = "HyperBeam";

				// Wave / Nebula
				if (slot3.type == wa || slot3.type == wa2 || slot3.type == nb)
				{
					string wave = "Wave";
					if (slot3.type == nb)
					{
						wave = "Nebula";
					}
					shot = wave + "HyperBeamShot";

					// Wave Spazer
					if (slot4.type == sp || slot4.type == wi || slot4.type == vt)
					{
						shot = wave + "SpazerHyperBeamShot";
						shotAmt = 3;
						if (slot4.type == vt)
						{
							shotAmt = 5;
						}

						// Wave Spazer Plasma
						if (slot5.type == plG || slot5.type == nv || slot5.type == sl)
						{
							shot = wave + "SpazerPlasmaHyperBeamShot";
						}
					}
					// Wave Plasma
					else if (slot5.type == plG || slot5.type == nv || slot5.type == sl)
					{
						shot = wave + "PlasmaHyperBeamShot";
					}
				}
				// Spazer
				else if (slot4.type == sp || slot4.type == wi || slot4.type == vt)
				{
					shot = "SpazerHyperBeamShot";
					shotAmt = 3;
					if (slot4.type == vt)
					{
						shotAmt = 5;
					}

					// Spazer Plasma
					if (slot5.type == plG || slot5.type == nv || slot5.type == sl)
					{
						shot = "SpazerPlasmaHyperBeamShot";
					}
				}
				// Plasma
				else if (slot5.type == plG || slot5.type == nv || slot5.type == sl)
				{
					shot = "PlasmaHyperBeamShot";
				}
			}
			// Phazon
			else if (isPhazon)
			{
				shot = "PhazonBeamShot";
				shotSound = "PhazonBeamSound";
				useTime = Common.Configs.MConfigItems.Instance.useTimePhazonBeam;

				damage = Common.Configs.MConfigItems.Instance.damagePhazonBeam;
				overheat = Common.Configs.MConfigItems.Instance.overheatPhazonBeam;

				texture = "PhazonBeam";

				// Wave / Nebula
				if (slot3.type == wa || slot3.type == wa2 || slot3.type == nb)
				{
					string wave = "Wave";
					if (slot3.type == nb)
					{
						wave = "Nebula";
					}
					shot = wave + "PhazonBeamShot";

					// Wave Spazer
					if (slot4.type == sp || slot4.type == wi || slot4.type == vt)
					{
						shot = wave + "SpazerPhazonBeamShot";
						shotAmt = 3;
						if (slot4.type == vt)
						{
							shotAmt = 5;
						}

						// Wave Spazer Plasma
						if (slot5.type == plG || slot5.type == nv || slot5.type == sl)
						{
							shot = wave + "SpazerPlasmaPhazonBeamShot";
						}
					}
					// Wave Plasma
					else if (slot5.type == plG || slot5.type == nv || slot5.type == sl)
					{
						shot = wave + "PlasmaPhazonBeamShot";
					}
				}
				// Spazer
				else if (slot4.type == sp || slot4.type == wi || slot4.type == vt)
				{
					shot = "SpazerPhazonBeamShot";
					shotAmt = 3;
					if (slot4.type == vt)
					{
						shotAmt = 5;
					}

					// Spazer Plasma
					if (slot5.type == plG || slot5.type == nv || slot5.type == sl)
					{
						shot = "SpazerPlasmaPhazonBeamShot";
					}
				}
				// Plasma
				else if (slot5.type == plG || slot5.type == nv || slot5.type == sl)
				{
					shot = "PlasmaPhazonBeamShot";
				}
			}

			iceDmg = 0f;
			waveDmg = 0f;
			spazDmg = 0f;
			plasDmg = 0f;
			hunterDmg= 0f;

			iceHeat = 0f;
			waveHeat = 0f;
			spazHeat = 0f;
			plasHeat = 0f;
			hunterHeat = 0f;

			iceSpeed = 0f;
			waveSpeed = 0f;
			spazSpeed = 0f;
			plasSpeed = 0f;
			hunterSpeed= 0f;

			if (!slot3.IsAir)
			{
				MGlobalItem mItem = slot3.GetGlobalItem<MGlobalItem>();
				waveDmg = mItem.addonDmg;
				waveHeat = mItem.addonHeat;
				waveSpeed = mItem.addonSpeed;
				/*if (BeamLoader.TryGetValue(BeamLoader.beams, slot3, out ModBeam modBeam))
				{
					waveDmg = modBeam.AddonDamageMult;
					waveHeat = modBeam.AddonHeat;
					waveSpeed = modBeam.AddonSpeed;
					if (((ModUtilityBeam)modBeam).ShotAmount > 1)
					{
						shotAmt = ((ModUtilityBeam)modBeam).ShotAmount;
						chargeShotAmt = ((ModUtilityBeam)modBeam).ShotAmount;
					}
					if (modBeam.PowerBeamTexture != null && modBeam.PowerBeamTexture != "")
					{
						texture = modBeam.PowerBeamTexture;
						//modBeamTextureMod = modBeam.Mod;
					}
					if (modBeam.ShotSound != null && modBeam.ShotSound != "" && modBeam.ChargingSound != null && modBeam.ChargingSound != "" && modBeam.ChargeShotSound != null && modBeam.ChargeShotSound != "")
					{
						shotSound = modBeam.ShotSound;
						shotSoundMod = modBeam.Mod;
						chargeUpSound = modBeam.ChargingSound;
						chargeUpSoundMod = modBeam.Mod;
						chargeShotSound = modBeam.ChargeShotSound;
						chargeShotSoundMod = modBeam.Mod;
					}
				}*/
			}

			if (!slot4.IsAir)
			{
				MGlobalItem mItem = slot4.GetGlobalItem<MGlobalItem>();
				spazDmg = mItem.addonDmg;
				spazHeat = mItem.addonHeat;
				spazSpeed = mItem.addonSpeed;
				/*if (BeamLoader.TryGetValue(BeamLoader.beams, slot4, out ModBeam modBeam))
				{
					spazDmg = modBeam.AddonDamageMult;
					spazHeat = modBeam.AddonHeat;
					spazSpeed = modBeam.AddonSpeed;
					if (((ModPrimaryABeam)modBeam).ShotAmount > 1)
					{
						shotAmt = ((ModPrimaryABeam)modBeam).ShotAmount;
						chargeShotAmt = ((ModPrimaryABeam)modBeam).ShotAmount;
					}
					if (modBeam.PowerBeamTexture != null && modBeam.PowerBeamTexture != "")
					{
						texture = modBeam.PowerBeamTexture;
						//modBeamTextureMod = modBeam.Mod;
					}
				}*/
			}
			if (!slot2.IsAir)
			{
				MGlobalItem mItem = slot2.GetGlobalItem<MGlobalItem>();
				iceDmg = mItem.addonDmg;
				iceHeat = mItem.addonHeat;
				iceSpeed = mItem.addonSpeed;
				/*if (BeamLoader.TryGetValue(BeamLoader.beams, slot2, out ModBeam modBeam))
				{
					iceDmg = modBeam.AddonDamageMult;
					iceHeat = modBeam.AddonHeat;
					iceSpeed = modBeam.AddonSpeed;
					if (modBeam.PowerBeamTexture != null && modBeam.PowerBeamTexture != "")
					{
						texture = modBeam.PowerBeamTexture;
						//modBeamTextureMod = modBeam.Mod;
					}
				}*/
			}
			if (!slot5.IsAir)
			{
				MGlobalItem mItem = slot5.GetGlobalItem<MGlobalItem>();
				plasDmg = mItem.addonDmg;
				plasHeat = mItem.addonHeat;
				plasSpeed = mItem.addonSpeed;
				/*if (BeamLoader.TryGetValue(BeamLoader.beams, slot5, out ModBeam modBeam))
				{
					plasDmg = modBeam.AddonDamageMult;
					plasHeat = modBeam.AddonHeat;
					plasSpeed = modBeam.AddonSpeed;
					if (modBeam.PowerBeamTexture != null && modBeam.PowerBeamTexture != "")
					{
						texture = modBeam.PowerBeamTexture;
						//modBeamTextureMod = modBeam.Mod;
					}
				}*/
			}

			if (!slot1.IsAir)
			{
				MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
				chargeDmgMult = mItem.addonChargeDmg;
				chargeCost = mItem.addonChargeHeat;
				hunterDmg = mItem.addonDmg;
				hunterHeat = mItem.addonHeat;
				hunterSpeed = mItem.addonSpeed;
				/*if (BeamLoader.TryGetValue(BeamLoader.beams, slot1, out ModBeam modBeam))
				{
					//isCharge = ((ModChargeBeam)modBeam).IsTraditionalCharge;
					//chargeDmgMult = modBeam.AddonChargeDamage;
					//chargeCost = modBeam.AddonChargeHeat;
					if (modBeam.PowerBeamTexture != null && modBeam.PowerBeamTexture != "")
					{
						texture = modBeam.PowerBeamTexture;
						//modBeamTextureMod = modBeam.Mod;
					}
				}*/
			}

			finalDmg = (int)Math.Round((double)((float)damage * (1f + iceDmg + waveDmg + spazDmg + plasDmg + hunterDmg)));
			overheat = (int)Math.Max(Math.Round((double)((float)overheat * (1 + iceHeat + waveHeat + spazHeat + plasHeat + hunterHeat))), 1);

			float shotsPerSecond = (60 / useTime) * (1f + iceSpeed + waveSpeed + spazSpeed + plasSpeed + hunterSpeed);

			useTime = (int)Math.Max(Math.Round(60.0 / (double)shotsPerSecond), 2);

			Item.damage = finalDmg;
			Item.useTime = useTime;
			Item.useAnimation = useTime;
			Item.shoot = ModContent.Find<ModProjectile>(Mod.Name, shot).Type;
			if (ShotSound == null)
			{
				ShotSound = new SoundStyle($"{shotSoundMod.Name}/Assets/Sounds/{shotSound}");
			}
			if (ChargeShotSound == null)
			{
				ChargeShotSound = new SoundStyle($"{chargeShotSoundMod.Name}/Assets/Sounds/{chargeShotSound}");
			}
			//Item.UseSound = ShotSound;

			//Item.autoReuse = (!slot1.IsAir);//(isCharge);

			Item.shootSpeed = 8f;
			Item.reuseDelay = 0;
			Item.mana = 0;
			Item.knockBack = 4f;
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
				Item.UseSound = null;
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
			float num5 = (float)(Item.height - tex.Height);
			float num6 = (float)(Item.width / 2 - tex.Width / 2);
			sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num6, Item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num5 + 2f),
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), alphaColor, rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), scale, SpriteEffects.None, 0f);
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
				mi.itemTexture = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/PowerBeam/{texture+alt}").Value;
			}
			else
			{
				if (MetroidMod.UseAltWeaponTextures)
				{
					mi.itemTexture = ModContent.Request<Texture2D>(Texture+"_alt").Value;
				}
				else
				{
					mi.itemTexture = ModContent.Request<Texture2D>(Texture).Value;
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

			Player player = Main.player[Main.myPlayer];
			MPlayer mp = player.GetModPlayer<MPlayer>();

			if (Item == Main.HoverItem)
			{
				Item.ModItem.UpdateInventory(player);
			}

			int dmg = player.GetWeaponDamage(Item);
			int chDmg = (int)((float)dmg * chargeDmgMult);
			TooltipLine chDmgLine = new(Mod, "ChargeDamage", chDmg + " Charge Shot damage");

			int oh = (int)((float)overheat * mp.overheatCost);
			TooltipLine ohLine = new(Mod, "Overheat", "Overheats by " + oh + " points per use");
			int chOh = (int)((float)oh * chargeCost);
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

		/*public override void GetWeaponDamage(Player P, ref int dmg)
		{
			dmg = (int)((float)dmg*baseDmg * (1f + iceDmg + waveDmg + spazDmg + plasDmg));
		}*/

		public override ModItem Clone(Item item)
		{
			ModItem clone = base.Clone(item);
			PowerBeam beamClone = (PowerBeam)clone;
			beamClone._beamMods = new Item[MetroidMod.beamSlotAmount];
			for (int i = 0; i < MetroidMod.beamSlotAmount; ++i)
			{
				if (_beamMods == null || _beamMods[i] == null)
				{
					beamClone._beamMods[i] = new Item();
					beamClone._beamMods[i].TurnToAir();
				}
				else
				{
					beamClone._beamMods[i] = _beamMods[i];
				}
			}

			return clone;
		}
			

		int chargeLead = -1;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if (isCharge)
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

				chargeLead = ch;
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
							int extraProj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>("Extra" + shot).Type, damage, knockback, player.whoAmI, 0, i);
							MProjectile mProj = (MProjectile)Main.projectile[extraProj].ModProjectile;
							mProj.waveDir = waveDir;
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
					Main.projectile[shotProj].netUpdate = true;
				}
			}
			waveDir *= -1;

			mp.statOverheat += (int)((float)overheat * mp.overheatCost);
			mp.overheatDelay = Math.Max(useTime - 10, 2);

			/* Sound & Sound Networking */
			if (Main.netMode != NetmodeID.SinglePlayer && mp.Player.whoAmI == Main.myPlayer)
			{
				// Send a packet to have the sound play on all clients.
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)MetroidMessageType.PlaySyncedSound);
				packet.Write((byte)player.whoAmI);
				packet.Write(shotSound);
				packet.Send();
			}

			// Play the shot sound for the local player.
			if (!isPhazon)
			{
				SoundEngine.PlaySound(new SoundStyle($"{shotSoundMod.Name}/Assets/Sounds/{shotSound}"), player.position);
			}

			return false;
		}

		public override void HoldItem(Player player)
		{
			if (isCharge && player.whoAmI == Main.myPlayer)
			{
				MPlayer mp = player.GetModPlayer<MPlayer>();

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
						Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);

						float MY = Main.mouseY + Main.screenPosition.Y;
						float MX = Main.mouseX + Main.screenPosition.X;
						if (player.gravDir == -1f)
						{
							MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
						}

						float targetrotation = (float)Math.Atan2((MY - oPos.Y), (MX - oPos.X));

						Vector2 velocity = targetrotation.ToRotationVector2() * Item.shootSpeed;

						float dmgMult = 1f + ((chargeDmgMult - 1f) / MPlayer.maxCharge) * mp.statCharge;
						int damage = player.GetWeaponDamage(Item);

						int oHeat = (int)((float)overheat * mp.overheatCost);

						double sideangle = Math.Atan2(velocity.Y, velocity.X) + (Math.PI / 2);

						if (mp.statCharge >= (MPlayer.maxCharge * 0.5))
						{
							for (int i = 0; i < chargeShotAmt; i++)
							{
								int chargeProj = Projectile.NewProjectile(Item.GetSource_ItemUse(Item), oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(chargeShot).Type, (int)((float)damage * dmgMult), Item.knockBack, player.whoAmI, 0, i);
								MProjectile mProj = (MProjectile)Main.projectile[chargeProj].ModProjectile;
								mProj.waveDir = waveDir;
								mProj.canDiffuse = (mp.statCharge >= (MPlayer.maxCharge * 0.9));
								mProj.Projectile.netUpdate2 = true;
							}

							SoundEngine.PlaySound(new SoundStyle($"{chargeShotSoundMod.Name}/Assets/Sounds/{chargeShotSound}"), oPos);

							mp.statOverheat += (int)((float)oHeat * chargeCost);
							mp.overheatDelay = useTime - 10;
						}
						else if (mp.statCharge > 0)
						{
							if (mp.statCharge >= 30)
							{
								for (int i = 0; i < shotAmt; i++)
								{
									int shotProj = Projectile.NewProjectile(Item.GetSource_ItemUse(Item), oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(shot).Type, damage, Item.knockBack, player.whoAmI, 0, i);
									MProjectile mProj = (MProjectile)Main.projectile[shotProj].ModProjectile;
									mProj.waveDir = waveDir;
									mProj.Projectile.netUpdate = true;
								}

								SoundEngine.PlaySound(new SoundStyle($"{shotSoundMod.Name}/Assets/Sounds/{shotSound}"), oPos);

								mp.statOverheat += oHeat;
								mp.overheatDelay = useTime - 10;
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
			}
			catch { }
		}

		public override void OnCreate(ItemCreationContext context)
		{
			base.OnCreate(context);
			_beamMods = new Item[5];
			for (int i = 0; i < _beamMods.Length; ++i)
			{
				_beamMods[i] = new Item();
				_beamMods[i].TurnToAir();
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			for (int i = 0; i < BeamMods.Length; ++i)
			{
				ItemIO.Send(BeamMods[i], writer);
			}
			writer.Write(chargeLead);
		}
		public override void NetReceive(BinaryReader reader)
		{
			for (int i = 0; i < BeamMods.Length; ++i)
			{
				BeamMods[i] = ItemIO.Receive(reader);
			}
			chargeLead = reader.ReadInt32();
		}
    }
}
