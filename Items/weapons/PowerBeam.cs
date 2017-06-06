using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using MetroidMod.Projectiles;
using MetroidMod.Projectiles.chargelead;

namespace MetroidMod.Items.weapons
{
	public class PowerBeam : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Power Beam";
			item.damage = 7;
			item.ranged = true;
			item.width = 24;
			item.height = 12;
			item.scale = 0.8f;
			item.toolTip = "Overheats by 3 points per use\n" +
				"Select this item in your hotbar and open your inventory to open the Beam Addon UI";
			item.useTime = 6;
			item.useAnimation = 6;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = 20000;
			item.rare = 2;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/PowerBeamSound");
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("PowerBeamShot");
			item.shootSpeed = 8f;
			item.crit = 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		public override void UseStyle(Player P)
		{
			P.itemLocation.X = P.MountedCenter.X - (float)Main.itemTexture[item.type].Width * 0.5f;
			P.itemLocation.Y = P.MountedCenter.Y - (float)Main.itemTexture[item.type].Height * 0.5f;
		}
		
		public override bool CanUseItem(Player player)
		{
			if(player.whoAmI == Main.myPlayer && item.type == Main.mouseItem.type)
			{
				return false;
			}
			return true;
		}

		public MetroidModUI metroidUI;
		
		int finalDmg = 7;
		
		string shot = "PowerBeamShot";
		string chargeShot = "PowerBeamChargeShot";
		string shotSound = "PowerBeamSound";
		string chargeShotSound = "PowerBeamChargeSound";
		string chargeUpSound = "ChargeStartup_Power";
		string chargeTex = "ChargeLead";
		int dustType = 64;
		Color dustColor = default(Color);
		Color lightColor = MetroidMod.powColor;
		int shotAmt = 1;
		int chargeShotAmt = 1;
		
		int waveDir = -1;

		bool isCharge = false;
		bool isChargeV2 = false;
		public override void UpdateInventory(Player P)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);

			if(metroidUI == null)
			{
				metroidUI = new MetroidModUI();
			}

			int ch = mod.ItemType("ChargeBeamAddon");
			int ch2 = mod.ItemType("ChargeBeamV2Addon");

			int ic = mod.ItemType("IceBeamAddon");
			int wa = mod.ItemType("WaveBeamAddon");
			int sp = mod.ItemType("SpazerAddon");
			int plR = mod.ItemType("PlasmaBeamRedAddon");
			int plG = mod.ItemType("PlasmaBeamGreenAddon");
			int nv = mod.ItemType("NovaBeamAddon");

			int hy = mod.ItemType("HyperBeamAddon");
			int ph = mod.ItemType("PhazonBeamAddon");
			
			Item slot1 = metroidUI.beamSlot[0].item;
			Item slot2 = metroidUI.beamSlot[1].item;
			Item slot3 = metroidUI.beamSlot[2].item;
			Item slot4 = metroidUI.beamSlot[3].item;
			Item slot5 = metroidUI.beamSlot[4].item;
			
			string name = "Power Beam";
			int damage = 7;
			int overheat = 3;
			int useTime = 6;
			shot = "PowerBeamShot";
			chargeShot = "PowerBeamChargeShot";
			shotAmt = 1;
			chargeShotAmt = 1;
			shotSound = "PowerBeamSound";
			chargeShotSound = "PowerBeamChargeSound";
			chargeUpSound = "ChargeStartup_Power";
			chargeTex = "ChargeLead";
			dustType = 64;
			dustColor = default(Color);
			lightColor = MetroidMod.powColor;
			
			isCharge = (slot1.type == ch || slot1.type == ch2);
			isChargeV2 = (slot1.type == ch2);

			// Default Combos
			if(slot1.IsAir || slot1.type == ch)
			{
				// Ice
				if(slot2.type == ic)
				{
					shot = "IceBeamShot";
					chargeShot = "IceBeamChargeShot";
					shotSound = "IceBeamSound";
					chargeShotSound = "IceBeamChargeSound";
					chargeUpSound = "ChargeStartup_Ice";
					chargeTex = "ChargeLead_Ice";
					dustType = 59;
					lightColor = MetroidMod.iceColor;

					// Ice Wave
					if(slot3.type == wa)
					{
						shot = "IceWaveBeamShot";
						chargeShot = "IceWaveBeamChargeShot";
						chargeShotAmt = 2;

						// Ice Wave Spazer
						if(slot4.type == sp)
						{
							shot = "IceWaveSpazerShot";
							chargeShot = "IceWaveSpazerChargeShot";
							shotSound = "IceComboSound";
							shotAmt = 3;
							chargeShotAmt = 3;
							
							// Ice Wave Spazer Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "IceWaveSpazerPlasmaBeamGreenShot";
								chargeShot = "IceWaveSpazerPlasmaBeamGreenChargeShot";
								shotSound = "IceComboSound";
							}
							// Ice Wave Spazer Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "IceWaveSpazerPlasmaBeamRedShot";
								chargeShot = "IceWaveSpazerPlasmaBeamRedChargeShot";
								shotSound = "IceComboSound";
								dustType = 135;
							}
							// Ice Wave Spazer Nova
							if(slot5.type == nv)
							{
								shot = "IceWaveSpazerNovaBeamShot";
								chargeShot = "IceWaveSpazerNovaBeamChargeShot";
								shotSound = "IceComboSound";
								dustType = 135;
							}
						}
						else
						{
							// Ice Wave Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "IceWavePlasmaBeamGreenShot";
								chargeShot = "IceWavePlasmaBeamGreenChargeShot";
								shotSound = "IceComboSound";
								shotAmt = 2;
							}
							// Ice Wave Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "IceWavePlasmaBeamRedShot";
								chargeShot = "IceWavePlasmaBeamRedChargeShot";
								shotSound = "IceComboSound";
								shotAmt = 2;
								dustType = 135;
							}
							// Ice Wave Nova
							if(slot5.type == nv)
							{
								shot = "IceWaveNovaBeamShot";
								chargeShot = "IceWaveNovaBeamChargeShot";
								shotSound = "IceComboSound";
								shotAmt = 2;
								dustType = 135;
							}
						}
					}
					else
					{
						// Ice Spazer
						if(slot4.type == sp)
						{
							shot = "IceSpazerShot";
							chargeShot = "IceSpazerChargeShot";
							shotSound = "IceComboSound";
							shotAmt = 3;
							chargeShotAmt = 3;

							// Ice Spazer Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "IceSpazerPlasmaBeamGreenShot";
								chargeShot = "IceSpazerPlasmaBeamGreenChargeShot";
								shotSound = "IceComboSound";
							}
							// Ice Spazer Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "IceSpazerPlasmaBeamRedShot";
								chargeShot = "IceSpazerPlasmaBeamRedChargeShot";
								shotSound = "IceComboSound";
								dustType = 135;
							}
							// Ice Spazer Nova
							if(slot5.type == nv)
							{
								shot = "IceSpazerNovaBeamShot";
								chargeShot = "IceSpazerNovaBeamChargeShot";
								shotSound = "IceComboSound";
								dustType = 135;
							}
						}
						else
						{
							// Ice Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "IcePlasmaBeamGreenShot";
								chargeShot = "IcePlasmaBeamGreenChargeShot";
								shotSound = "IceComboSound";
							}
							// Ice Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "IcePlasmaBeamRedShot";
								chargeShot = "IcePlasmaBeamRedChargeShot";
								shotSound = "IceComboSound";
								dustType = 135;
							}
							// Ice Nova
							if(slot5.type == nv)
							{
								shot = "IceNovaBeamShot";
								chargeShot = "IceNovaBeamChargeShot";
								shotSound = "IceComboSound";
								dustType = 135;
							}
						}
					}
				}
				else
				{
					// Wave
					if(slot3.type == wa)
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

						// Wave Spazer
						if(slot4.type == sp)
						{
							shot = "WaveSpazerShot";
							chargeShot = "WaveSpazerChargeShot";
							shotSound = "SpazerSound";
							shotAmt = 3;
							chargeShotAmt = 3;

							// Wave Spazer Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "WaveSpazerPlasmaBeamGreenShot";
								chargeShot = "WaveSpazerPlasmaBeamGreenChargeShot";
								shotSound = "PlasmaBeamGreenSound";
								chargeShotSound = "PlasmaBeamGreenChargeSound";
								chargeUpSound = "ChargeStartup_Power";
								chargeTex = "ChargeLead_PlasmaGreen";
								dustType = 61;
								lightColor = MetroidMod.plaGreenColor;
							}
							// Wave Spazer Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "WaveSpazerPlasmaBeamRedShot";
								chargeShot = "WaveSpazerPlasmaBeamRedChargeShot";
								shotSound = "PlasmaBeamRedSound";
								chargeShotSound = "PlasmaBeamRedChargeSound";
								chargeUpSound = "ChargeStartup_PlasmaRed";
								chargeTex = "ChargeLead_PlasmaRed";
								dustType = 6;
								lightColor = MetroidMod.plaRedColor;
							}
							// Wave Spazer Nova
							if(slot5.type == nv)
							{
								shot = "WaveSpazerNovaBeamShot";
								chargeShot = "WaveSpazerNovaBeamChargeShot";
								shotSound = "NovaBeamSound";
								chargeShotSound = "NovaBeamChargeSound";
								chargeUpSound = "ChargeStartup_Nova";
								chargeTex = "ChargeLead_Nova";
								dustType = 75;
								lightColor = MetroidMod.novColor;
							}
						}
						else
						{
							// Wave Plasma (Green)
							if(slot5.type == plG)
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
							}
							// Wave Plasma (Red)
							if(slot5.type == plR)
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
							}
							// Wave Nova
							if(slot5.type == nv)
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
							}
						}
					}
					else
					{
						// Spazer
						if(slot4.type == sp)
						{
							shot = "SpazerShot";
							chargeShot = "SpazerChargeShot";
							shotSound = "SpazerSound";
							chargeShotSound = "SpazerChargeSound";
							chargeTex = "ChargeLead_Spazer";
							shotAmt = 3;
							chargeShotAmt = 3;

							// Spazer Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "SpazerPlasmaBeamGreenShot";
								chargeShot = "SpazerPlasmaBeamGreenChargeShot";
								shotSound = "PlasmaBeamGreenSound";
								chargeShotSound = "PlasmaBeamGreenChargeSound";
								chargeTex = "ChargeLead_PlasmaGreen";
								dustType = 61;
								lightColor = MetroidMod.plaGreenColor;
							}
							// Spazer Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "SpazerPlasmaBeamRedShot";
								chargeShot = "SpazerPlasmaBeamRedChargeShot";
								shotSound = "PlasmaBeamRedSound";
								chargeShotSound = "PlasmaBeamRedChargeSound";
								chargeUpSound = "ChargeStartup_PlasmaRed";
								chargeTex = "ChargeLead_PlasmaRed";
								dustType = 6;
								lightColor = MetroidMod.plaRedColor;
							}
							// Spazer Nova
							if(slot5.type == nv)
							{
								shot = "SpazerNovaBeamShot";
								chargeShot = "SpazerNovaBeamChargeShot";
								shotSound = "NovaBeamSound";
								chargeShotSound = "NovaBeamChargeSound";
								chargeUpSound = "ChargeStartup_Nova";
								chargeTex = "ChargeLead_Nova";
								dustType = 75;
								lightColor = MetroidMod.novColor;
							}
						}
						else
						{
							// Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "PlasmaBeamGreenShot";
								chargeShot = "PlasmaBeamGreenChargeShot";
								shotSound = "PlasmaBeamGreenSound";
								chargeShotSound = "PlasmaBeamGreenChargeSound";
								chargeTex = "ChargeLead_PlasmaGreen";
								dustType = 61;
								lightColor = MetroidMod.plaGreenColor;
							}
							// Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "PlasmaBeamRedShot";
								chargeShot = "PlasmaBeamRedChargeShot";
								shotSound = "PlasmaBeamRedSound";
								chargeShotSound = "PlasmaBeamRedChargeSound";
								chargeUpSound = "ChargeStartup_PlasmaRed";
								chargeTex = "ChargeLead_PlasmaRed";
								dustType = 6;
								lightColor = MetroidMod.plaRedColor;
							}
							// Nova
							if(slot5.type == nv)
							{
								shot = "NovaBeamShot";
								chargeShot = "NovaBeamChargeShot";
								shotSound = "NovaBeamSound";
								chargeShotSound = "NovaBeamChargeSound";
								chargeUpSound = "ChargeStartup_Nova";
								chargeTex = "ChargeLead_Nova";
								dustType = 75;
								lightColor = MetroidMod.novColor;
							}
						}
					}
				}
			}
			// Charge V2
			else if(slot1.type == ch2)
			{
				shot = "PowerBeamV2Shot";
				chargeShot = "PowerBeamV2ChargeShot";
				shotSound = "PowerBeamV2Sound";

				// Ice
				if(slot2.type == ic)
				{
					shot = "IceBeamV2Shot";
					chargeShot = "IceBeamV2ChargeShot";
					shotSound = "IceBeamV2Sound";
					chargeShotSound = "IceBeamChargeSound";
					chargeUpSound = "ChargeStartup_Ice";
					chargeTex = "ChargeLead_Ice";
					dustType = 59;
					lightColor = MetroidMod.iceColor;

					// Ice Wave
					if(slot3.type == wa)
					{
						shot = "IceWaveBeamV2Shot";
						chargeShot = "IceWaveBeamV2ChargeShot";
						shotAmt = 2;
						chargeShotAmt = 2;

						// Ice Wave Spazer/Wide
						if(slot4.type == sp)
						{
							shot = "IceWaveWideBeamShot";
							chargeShot = "IceWaveWideBeamChargeShot";
							shotAmt = 3;
							chargeShotAmt = 3;

							// Ice Wave Spazer/Wide Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "IceWaveWidePlasmaBeamGreenV2Shot";
								chargeShot = "IceWaveWidePlasmaBeamGreenV2ChargeShot";
								shotSound = "FinalBeamSound";
								chargeShotSound = "FinalBeamChargeSound";
								chargeUpSound = "ChargeStartup_Final";
							}
							// Ice Wave Spazer/Wide Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "IceWaveWidePlasmaBeamRedV2Shot";
								chargeShot = "IceWaveWidePlasmaBeamRedV2ChargeShot";
								shotSound = "FinalBeamSound";
								chargeShotSound = "FinalBeamChargeSound";
								chargeUpSound = "ChargeStartup_Final";
							}
							// Ice Wave Spazer/Wide Nova
							if(slot5.type == nv)
							{
								shot = "IceWaveWideNovaBeamV2Shot";
								chargeShot = "IceWaveWideNovaBeamV2ChargeShot";
								shotSound = "IceWaveNovaBeamV2Sound";
							}
						}
						else
						{
							// Ice Wave Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "IceWavePlasmaBeamGreenV2Shot";
								chargeShot = "IceWavePlasmaBeamGreenV2ChargeShot";
								shotSound = "FinalBeamSound";
								chargeShotSound = "FinalBeamChargeSound";
								chargeUpSound = "ChargeStartup_Final";
							}
							// Ice Wave Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "IceWavePlasmaBeamRedV2Shot";
								chargeShot = "IceWavePlasmaBeamRedV2ChargeShot";
								shotSound = "FinalBeamSound";
								chargeShotSound = "FinalBeamChargeSound";
								chargeUpSound = "ChargeStartup_Final";
							}
							// Ice Wave Nova
							if(slot5.type == nv)
							{
								shot = "IceWaveNovaBeamV2Shot";
								chargeShot = "IceWaveNovaBeamV2ChargeShot";
								shotSound = "IceWaveNovaBeamV2Sound";
							}
						}
					}
					else
					{
						// Ice Spazer/Wide
						if(slot4.type == sp)
						{
							shot = "IceWideBeamShot";
							chargeShot = "IceWideBeamChargeShot";
							shotAmt = 3;
							chargeShotAmt = 3;

							// Ice Spazer/Wide Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "IceWidePlasmaBeamGreenV2Shot";
								chargeShot = "IceWidePlasmaBeamGreenV2ChargeShot";
								shotSound = "IceComboSound";
							}
							// Ice Spazer/Wide Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "IceWidePlasmaBeamRedV2Shot";
								chargeShot = "IceWidePlasmaBeamRedV2ChargeShot";
							}
							// Ice Spazer/Wide Nova
							if(slot5.type == nv)
							{
								shot = "IceWideNovaBeamV2Shot";
								chargeShot = "IceWideNovaBeamV2ChargeShot";
							}
						}
						else
						{
							// Ice Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "IcePlasmaBeamGreenV2Shot";
								chargeShot = "IcePlasmaBeamGreenV2ChargeShot";
								shotSound = "IceComboSound";
							}
							// Ice Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "IcePlasmaBeamRedV2Shot";
								chargeShot = "IcePlasmaBeamRedV2ChargeShot";
							}
							// Ice Nova
							if(slot5.type == nv)
							{
								shot = "IceNovaBeamV2Shot";
								chargeShot = "IceNovaBeamV2ChargeShot";
							}
						}
					}
				}
				else
				{
					// Wave
					if(slot3.type == wa)
					{
						shot = "WaveBeamV2Shot";
						chargeShot = "WaveBeamV2ChargeShot";
						shotSound = "WaveBeamV2Sound";
						chargeShotSound = "WaveBeamChargeSound";
						chargeUpSound = "ChargeStartup_Wave";
						chargeTex = "ChargeLead_WaveV2";
						dustType = 62;
						lightColor = MetroidMod.waveColor2;
						shotAmt = 2;
						chargeShotAmt = 2;

						// Wave Spazer/Wide
						if(slot4.type == sp)
						{
							shot = "WaveWideBeamShot";
							chargeShot = "WaveWideBeamChargeShot";
							shotAmt = 3;
							chargeShotAmt = 3;

							// Wave Spazer/Wide Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "WaveWidePlasmaBeamGreenV2Shot";
								chargeShot = "WaveWidePlasmaBeamGreenV2ChargeShot";
								shotSound = "WavePlasmaBeamGreenSound";
								chargeShotSound = "PlasmaBeamGreenChargeSound";
								chargeUpSound = "ChargeStartup_Power";
								chargeTex = "ChargeLead_PlasmaGreenV2";
								dustType = 15;
								lightColor = MetroidMod.plaGreenColor;
							}
							// Wave Spazer/Wide Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "WaveWidePlasmaBeamRedV2Shot";
								chargeShot = "WaveWidePlasmaBeamRedV2ChargeShot";
								shotSound = "PlasmaBeamRedV2Sound";
								chargeShotSound = "PlasmaBeamRedChargeSound";
								chargeUpSound = "ChargeStartup_PlasmaRed";
								chargeTex = "ChargeLead_PlasmaRed";
								dustType = 6;
								lightColor = MetroidMod.plaRedColor;
							}
							// Wave Spazer/Wide Nova
							if(slot5.type == nv)
							{
								shot = "WaveWideNovaBeamV2Shot";
								chargeShot = "WaveWideNovaBeamV2ChargeShot";
								shotSound = "NovaBeamSound";
								chargeShotSound = "NovaBeamChargeSound";
								chargeUpSound = "ChargeStartup_Nova";
								chargeTex = "ChargeLead_Nova";
								dustType = 75;
								lightColor = MetroidMod.novColor;
							}
						}
						else
						{
							// Wave Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "WavePlasmaBeamGreenV2Shot";
								chargeShot = "WavePlasmaBeamGreenV2ChargeShot";
								shotSound = "WavePlasmaBeamGreenSound";
								chargeShotSound = "PlasmaBeamGreenChargeSound";
								chargeUpSound = "ChargeStartup_Power";
								chargeTex = "ChargeLead_PlasmaGreenV2";
								dustType = 15;
								lightColor = MetroidMod.plaGreenColor;
							}
							// Wave Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "WavePlasmaBeamRedV2Shot";
								chargeShot = "WavePlasmaBeamRedV2ChargeShot";
								shotSound = "PlasmaBeamRedV2Sound";
								chargeShotSound = "PlasmaBeamRedChargeSound";
								chargeUpSound = "ChargeStartup_PlasmaRed";
								chargeTex = "ChargeLead_PlasmaRed";
								dustType = 6;
								lightColor = MetroidMod.plaRedColor;
							}
							// Wave Nova
							if(slot5.type == nv)
							{
								shot = "WaveNovaBeamV2Shot";
								chargeShot = "WaveNovaBeamV2ChargeShot";
								shotSound = "NovaBeamSound";
								chargeShotSound = "NovaBeamChargeSound";
								chargeUpSound = "ChargeStartup_Nova";
								chargeTex = "ChargeLead_Nova";
								dustType = 75;
								lightColor = MetroidMod.novColor;
							}
						}
					}
					else
					{
						// Spazer/Wide
						if(slot4.type == sp)
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

							// Spazer/Wide Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "WidePlasmaBeamGreenV2Shot";
								chargeShot = "WidePlasmaBeamGreenV2ChargeShot";
								shotSound = "WidePlasmaBeamGreenSound";
								chargeShotSound = "PlasmaBeamGreenChargeSound";
								chargeTex = "ChargeLead_PlasmaGreen";
								dustType = 61;
								lightColor = MetroidMod.plaGreenColor;
								dustColor = default(Color);
							}
							// Spazer/Wide Plasma (Red)
							if(slot5.type == plR)
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
							}
							// Spazer/Wide Nova
							if(slot5.type == nv)
							{
								shot = "WideNovaBeamV2Shot";
								chargeShot = "WideNovaBeamV2ChargeShot";
								shotSound = "NovaBeamSound";
								chargeShotSound = "NovaBeamChargeSound";
								chargeUpSound = "ChargeStartup_Nova";
								chargeTex = "ChargeLead_Nova";
								dustType = 75;
								lightColor = MetroidMod.novColor;
								dustColor = default(Color);
							}
						}
						else
						{
							// Plasma (Green)
							if(slot5.type == plG)
							{
								shot = "PlasmaBeamGreenV2Shot";
								chargeShot = "PlasmaBeamGreenV2ChargeShot";
								shotSound = "PlasmaBeamGreenSound";
								chargeShotSound = "PlasmaBeamGreenChargeSound";
								chargeTex = "ChargeLead_PlasmaGreen";
								dustType = 61;
								lightColor = MetroidMod.plaGreenColor;
							}
							// Plasma (Red)
							if(slot5.type == plR)
							{
								shot = "PlasmaBeamRedV2Shot";
								chargeShot = "PlasmaBeamRedV2ChargeShot";
								shotSound = "PlasmaBeamRedSound";
								chargeShotSound = "PlasmaBeamRedChargeSound";
								chargeUpSound = "ChargeStartup_PlasmaRed";
								chargeTex = "ChargeLead_PlasmaRed";
								dustType = 6;
								lightColor = MetroidMod.plaRedColor;
							}
							// Nova
							if(slot5.type == nv)
							{
								shot = "NovaBeamV2Shot";
								chargeShot = "NovaBeamV2ChargeShot";
								shotSound = "NovaBeamSound";
								chargeShotSound = "NovaBeamChargeSound";
								chargeUpSound = "ChargeStartup_Nova";
								chargeTex = "ChargeLead_Nova";
								dustType = 75;
								lightColor = MetroidMod.novColor;
							}
						}
					}
				}
			}
			// Hyper
			else if(slot1.type == hy)
			{
				
			}
			// Phazon
			else if(slot1.type == ph)
			{
				
			}
			
			if(slot1.type == hy || slot1.type == ph)
			{
				name = slot1.name;
			}
			else
			{
				if(!slot5.IsAir)
				{
					if(slot5.type == plR || slot5.type == plG)
					{
						name = "Plasma Beam";
					}
					else
					{
						name = slot5.name;
					}
				}
				else if(!slot4.IsAir)
				{
					if(slot1.type == ch2 && slot4.type == sp)
					{
						name = "Wide Beam";
					}
					else
					{
						name = slot4.name;
					}
				}
				else if(!slot3.IsAir)
				{
					name = slot3.name;
				}
				else if(!slot2.IsAir)
				{
					name = slot2.name;
				}
			}
			
			float iceDmg = 0f;
			float waveDmg = 0f;
			float spazDmg = 0f;
			float plasDmg = 0f;

			float iceHeat = 0f;
			float waveHeat = 0f;
			float spazHeat = 0f;
			float plasHeat = 0f;

			if(slot1.type == ch2)
			{
				damage = 10;
				overheat = 5;
			}
			else if(slot1.type != hy && slot1.type != ph)
			{
				damage = 7;
				overheat = 3;
			}
			if(slot1.type != hy && slot1.type != ph)
			{
				if(slot2.type == ic)
				{
					iceDmg = 1.25f;
					iceHeat = 0.1f;
				}
				if(slot3.type == wa)
				{
					waveDmg = 1f;
					waveHeat = 0.5f;
				}
				if(slot4.type == sp)
				{
					spazDmg = 0.25f;
					spazHeat = 0.25f;
				}
				if(slot5.type == plR)
				{
					plasDmg = 3f;
					plasHeat = 1f;
				}
				else if(slot5.type == plG)
				{
					plasDmg = 3f;
					plasHeat = 1f;
				}
				else if(slot5.type == nv)
				{
					plasDmg = 5f;
					plasHeat = 2f;
				}
			}
			
			finalDmg = (int)((float)damage * (1f + iceDmg + waveDmg + spazDmg + plasDmg));
			
			item.name = name;
			item.damage = finalDmg;
			item.useTime = useTime;
			item.useAnimation = useTime;
			item.shoot = mod.ProjectileType(shot);
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/"+shotSound);
			
			item.autoReuse = (isCharge);
			item.channel = (isCharge);

			item.shootSpeed = 8f;
			item.reuseDelay = 0;
			item.mana = 0;
			item.knockBack = 4f;
			item.scale = 0.8f;
			item.crit = 3;
			item.value = 20000;
			
			item.rare = 2;
			
			item.Prefix(item.prefix);
		}
		
		/*public override void GetWeaponDamage(Player P, ref int dmg)
		{
			dmg = (int)((float)dmg*baseDmg * (1f + iceDmg + waveDmg + spazDmg + plasDmg));
		}*/
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(isCharge)
			{
				int ch = Projectile.NewProjectile(position.X,position.Y,speedX,speedY,mod.ProjectileType("ChargeLead"),damage,knockBack,player.whoAmI);
				ChargeLead cl = (ChargeLead)Main.projectile[ch].modProjectile;
				cl.Shot = shot;
				cl.ChargeShot = chargeShot;
				cl.ShotAmt = shotAmt;
				cl.ChargeShotAmt = chargeShotAmt;
				cl.ShotSound = shotSound;
				cl.ChargeShotSound = chargeShotSound;
				cl.ChargeUpSound = chargeUpSound;
				cl.ChargeTex = chargeTex;
				cl.DustType = dustType;
				cl.DustColor = dustColor;
				cl.LightColor = lightColor;
				cl.waveDir = waveDir;
				cl.IsChargeV2 = isChargeV2;
			}
			
			if(isChargeV2 && shotAmt <= 1)
			{
				double sideangle = Math.Atan2(speedY, speedX) + (Math.PI/2);

				int shotProj1 = Projectile.NewProjectile(position.X,position.Y,speedX,speedY,item.shoot,damage,knockBack,player.whoAmI);
				int shotProj2 = Projectile.NewProjectile(position.X,position.Y,speedX,speedY,item.shoot,damage,knockBack,player.whoAmI);
				Projectile proj = Main.projectile[shotProj1];
				float offset = ((float)Main.projectileTexture[proj.type].Width/2f)*proj.scale - 0.5f;
				proj.position.X -= (float)Math.Cos(sideangle) * offset;
				proj.position.Y -= (float)Math.Sin(sideangle) * offset;
				proj.ai[0] = 1f;
				MProjectile mProj = (MProjectile)proj.modProjectile;
				mProj.waveDir = waveDir;
				proj = Main.projectile[shotProj2];
				proj.position.X += (float)Math.Cos(sideangle) * offset;
				proj.position.Y += (float)Math.Sin(sideangle) * offset;
				proj.ai[0] = -1f;
				mProj = (MProjectile)proj.modProjectile;
				mProj.waveDir = waveDir;
			}
			else
			{
				for(int i = 0; i < shotAmt; i++)
				{
					int shotProj = Projectile.NewProjectile(position.X,position.Y,speedX,speedY,item.shoot,damage,knockBack,player.whoAmI);
					MProjectile mProj = (MProjectile)Main.projectile[shotProj].modProjectile;
					mProj.waveStyle = i;
					mProj.waveDir = waveDir;
				}
			}
			waveDir *= -1;
			return false;
		}
		
		public override void HoldItem(Player player)
		{
			if(isCharge)
			{
				MPlayer mp = player.GetModPlayer<MPlayer>(mod);

				if(!mp.ballstate && !mp.shineActive && !player.dead && !player.noItems)
				{
					if(player.controlUseItem || player.channel)
					{
						if(mp.statCharge < MPlayer.maxCharge)
						{
							if(mp.SMoveEffect > 0)
							{
								mp.statCharge = Math.Min(mp.statCharge + 15, MPlayer.maxCharge);
							}
							else
							{
								mp.statCharge = Math.Min(mp.statCharge + 1, MPlayer.maxCharge);
							}
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
						float targetrotation = (float)Math.Atan2((MY-oPos.Y),(MX-oPos.X));
						
						Vector2 velocity = targetrotation.ToRotationVector2()*item.shootSpeed;

						float dmgMult = (1f+((float)mp.statCharge*0.02f));
						int damage = (int)((float)item.damage*player.rangedDamage);
						
						double sideangle = Math.Atan2(velocity.Y, velocity.X) + (Math.PI/2);

						if(mp.statCharge >= (MPlayer.maxCharge*0.5))
						{
							if(isChargeV2 && chargeShotAmt <= 1)
							{
								int chargeProj1 = Projectile.NewProjectile(oPos.X,oPos.Y,velocity.X,velocity.Y,mod.ProjectileType(chargeShot),(int)((float)damage*dmgMult),item.knockBack,player.whoAmI);
								int chargeProj2 = Projectile.NewProjectile(oPos.X,oPos.Y,velocity.X,velocity.Y,mod.ProjectileType(chargeShot),(int)((float)damage*dmgMult),item.knockBack,player.whoAmI);
								Projectile proj = Main.projectile[chargeProj1];
								float offset = ((float)Main.projectileTexture[proj.type].Width/2f)*proj.scale - 0.5f;
								proj.position.X -= (float)Math.Cos(sideangle) * offset;
								proj.position.Y -= (float)Math.Sin(sideangle) * offset;
								proj.ai[0] = 1f;
								MProjectile mProj = (MProjectile)proj.modProjectile;
								mProj.canDiffuse = (mp.statCharge >= (MPlayer.maxCharge*0.9));
								mProj.waveDir = waveDir;
								proj = Main.projectile[chargeProj2];
								proj.position.X += (float)Math.Cos(sideangle) * offset;
								proj.position.Y += (float)Math.Sin(sideangle) * offset;
								proj.ai[0] = -1f;
								mProj = (MProjectile)proj.modProjectile;
								mProj.canDiffuse = (mp.statCharge >= (MPlayer.maxCharge*0.9));
								mProj.waveDir = waveDir;
							}
							else
							{
								for(int i = 0; i < chargeShotAmt; i++)
								{
									int chargeProj = Projectile.NewProjectile(oPos.X,oPos.Y,velocity.X,velocity.Y,mod.ProjectileType(chargeShot),(int)((float)damage*dmgMult),item.knockBack,player.whoAmI);
									MProjectile mProj = (MProjectile)Main.projectile[chargeProj].modProjectile;
									mProj.waveStyle = i;
									mProj.waveDir = waveDir;
									mProj.canDiffuse = (mp.statCharge >= (MPlayer.maxCharge*0.9));
								}
							}
							
							Main.PlaySound(SoundLoader.customSoundType, (int)oPos.X, (int)oPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/"+chargeShotSound));
							
							player.itemTime = (item.useTime*3);
							player.itemAnimation = (item.useAnimation*3);
						}
						else if(mp.statCharge > 0)
						{
							if(mp.statCharge >= 30)
							{
								if(isChargeV2 && shotAmt <= 1)
								{
									int shotProj1 = Projectile.NewProjectile(oPos.X,oPos.Y,velocity.X,velocity.Y,mod.ProjectileType(shot),damage,item.knockBack,player.whoAmI);
									int shotProj2 = Projectile.NewProjectile(oPos.X,oPos.Y,velocity.X,velocity.Y,mod.ProjectileType(shot),damage,item.knockBack,player.whoAmI);
									Projectile proj = Main.projectile[shotProj1];
									float offset = ((float)Main.projectileTexture[proj.type].Width/2f)*proj.scale - 0.5f;
									proj.position.X -= (float)Math.Cos(sideangle) * offset;
									proj.position.Y -= (float)Math.Sin(sideangle) * offset;
									proj.ai[0] = 1f;
									MProjectile mProj = (MProjectile)proj.modProjectile;
									mProj.waveDir = waveDir;
									proj = Main.projectile[shotProj2];
									proj.position.X += (float)Math.Cos(sideangle) * offset;
									proj.position.Y += (float)Math.Sin(sideangle) * offset;
									proj.ai[0] = -1f;
									mProj = (MProjectile)proj.modProjectile;
									mProj.waveDir = waveDir;
								}
								else
								{
									for(int i = 0; i < shotAmt; i++)
									{
										int shotProj = Projectile.NewProjectile(oPos.X,oPos.Y,velocity.X,velocity.Y,mod.ProjectileType(shot),damage,item.knockBack,player.whoAmI);
										MProjectile mProj = (MProjectile)Main.projectile[shotProj].modProjectile;
										mProj.waveStyle = i;
										mProj.waveDir = waveDir;
									}
								}
								
								Main.PlaySound(SoundLoader.customSoundType, (int)oPos.X, (int)oPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/"+shotSound));
							}
							
							player.itemTime = item.useTime;
							player.itemAnimation = item.useAnimation;
						}
						mp.statCharge = 0;
					}
				}
				else
				{
					mp.statCharge = 0;
				}
			}
		}
		
		int selectedItem = 0;
        	public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        	{
			Player player = Main.player[Main.myPlayer];
			if(player.selectedItem < 10)
			{
				selectedItem = player.selectedItem;
			}
			if(metroidUI != null)
			{
				if(player.inventory[selectedItem] == item)
				{
					metroidUI.Draw(spriteBatch);
				}
				else
				{
					metroidUI.BeamUIOpen = false;
				}
			}
        	}
		
		public override TagCompound Save()
		{
			if(metroidUI != null)
			{
				return new TagCompound
				{
					{"beamItem0", ItemIO.Save(metroidUI.beamSlot[0].item)},
					{"beamItem1", ItemIO.Save(metroidUI.beamSlot[1].item)},
					{"beamItem2", ItemIO.Save(metroidUI.beamSlot[2].item)},
					{"beamItem3", ItemIO.Save(metroidUI.beamSlot[3].item)},
					{"beamItem4", ItemIO.Save(metroidUI.beamSlot[4].item)}
				};
			}
			return null;
		}
		public override void Load(TagCompound tag)
		{
			try
			{
				if(metroidUI == null)
				{
					metroidUI = new MetroidModUI();
				}
				for(int i = 0; i < metroidUI.beamSlot.Length ; i++)
				{
					Item item = tag.Get<Item>("beamItem"+i);
					metroidUI.beamSlot[i].item = item;
				}
			}
			catch{}
		}
	}
}
