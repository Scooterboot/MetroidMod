﻿using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Common.GlobalItems
{
	public class MGlobalItem : GlobalItem
	{
		public AddonType AddonType = AddonType.None;

		#region Old code, remove later in favor of modularity maybe?
		public int addonSlotType = -1;
		public int beamSlotType = -1;
		public int missileChangeType = -1;
		public float addonChargeDmg = 1f;
		public float addonChargeHeat = 1f;
		public float addonDmg = 0f;
		public float addonSpeed = 0f;
		public float addonHeat = 0f;
		/// <summary>
		/// How much universal ammo to use per normal shot (NOT A PERCENTAGE)
		/// </summary>
		public float addonUACost = 0f;

		public int missileSlotType = -1;

		public int addonMissileCost = 5;
		public int addonMissileDrain = 5;
		#endregion

		// float because funi - ChaosInsurgent49
		public float statUA = 40f;
		public int maxUA = 400;
		public int statMissiles = 5;
		public int maxMissiles = 5;

		public int numSeekerTargets = 0;
		public int[] seekerTarget = new int[5];
		public int seekerCharge = 0;
		public static int seekerMaxCharge = 25;

		public Texture2D itemTexture;
		public bool isBeam=true;

		public override bool InstancePerEntity => true;
		protected override bool CloneNewInstances => true;

		public static float AmmoUsage(Player player, float cost)
		{
			if ((player.huntressAmmoCost90 && Main.rand.NextBool(10)) || (player.ammoBox && Main.rand.NextBool(5)) || (player.ammoPotion && Main.rand.NextBool(5)) || (player.ammoCost80 && Main.rand.NextBool(5)) || (player.ammoCost75 && Main.rand.NextBool(4)))
			{
				return Main.rand.NextFloat(cost);
			}
			else
			{
				return cost;
			}
		}
		public override GlobalItem Clone(Item item, Item itemClone)
		{
			MGlobalItem other = (MGlobalItem)MemberwiseClone();
			other.maxMissiles = maxMissiles;
			other.statMissiles = statMissiles;
			other.statUA = statUA;
			other.maxUA = maxUA;

			return other;
		}
	}
	public class VisorsGlobalItem : GlobalItem
	{
		public override bool CanUseItem(Item item, Player player)
		{
			return player.GetModPlayer<MPlayer>().VisorInUse != SuitAddonLoader.GetAddon<Content.SuitAddons.ScanVisor>().Type && player.GetModPlayer<MPlayer>().VisorInUse != SuitAddonLoader.GetAddon<Content.SuitAddons.XRayScope>().Type;
		}
	}
	public class Grab : GlobalItem
	{
		public override void GrabRange(Item item, Player player, ref int grabRange)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			grabRange += (int)(mp.statCharge * 1.6f);
		}
	}
	public class ArmorColor : GlobalItem
	{
		public override void DrawArmorColor(EquipType type, int slot, Player P, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>();
			bool pseudoScrew = (mp.statCharge >= MPlayer.maxCharge && mp.somersault);
			if (mp.hyperColors > 0 || mp.speedBoosting || mp.shineActive || (pseudoScrew && mp.psuedoScrewFlash >= 3) || (mp.shineCharge > 0 && mp.shineChargeFlash >= 4))
			{
				if (mp.hyperColors > 0)
				{
					color = P.GetImmuneAlphaPure(new Color(mp.r, mp.g, mp.b, 255), shadow);
				}
				else if (pseudoScrew && mp.psuedoScrewFlash >= 3)
				{
					color = P.GetImmuneAlphaPure(mp.chargeColor, shadow);
				}
				else if (mp.shineActive || (mp.shineCharge > 0 && mp.shineChargeFlash >= 4))
				{
					color = P.GetImmuneAlphaPure(new Color(255, 216, 0), shadow);
				}
				else if (mp.speedBoosting)
				{
					color = P.GetImmuneAlphaPure(new Color(0, 200, 255), shadow);
				}
				mp.morphColor = color;

				int dustType = 212;
				if (P.head <= 0 || P.body <= 0 || P.legs <= 0)
				{
					int dust = Dust.NewDust(new Vector2(P.position.X - P.velocity.X, P.position.Y - 2f - P.velocity.Y), P.width, P.height, dustType, -P.velocity.X * 0.25f, -P.velocity.Y * 0.25f, 100, color, 1.0f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
				}
			}
		}
	}
	public class ReservePickup : GlobalItem
	{
		public override bool OnPickup(Item item, Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if (item.type == ItemID.Heart || item.type == ItemID.CandyApple || item.type == ItemID.CandyCane)
			{
				if (mp.reserveHearts < mp.reserveTanks && player.statLife >= player.statLifeMax2)
				{
					mp.reserveHearts++;
				}
			}
			return base.OnPickup(item, player);
		}
	}
	public class BossBagAdditions : GlobalItem
	{
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (ItemID.Sets.BossBag[item.type] && !ItemID.Sets.PreHardmodeLikeBossBag[item.type])
			{
				LeadingConditionRule expertRule = new LeadingConditionRule(new Conditions.IsExpert());

				// arbitrary "288" because that's the chance each vanilla dev set has altogether
				IItemDropRule DarkHunterSet = expertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.DarkSamusHelmet>(), 288));
				DarkHunterSet.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.DarkSamusBreastplate>(), 1));
				DarkHunterSet.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.DarkSamusGreaves>(), 1));
				/*var EzloHat = */
				expertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.Contributor.EzloHat>(), 288));
				IItemDropRule DreadSuit = expertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityDreadSuitHelmet>(), 288));
				DreadSuit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityDreadSuitBreastplate>(), 1));
				DreadSuit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityDreadSuitGreaves>(), 1));
				IItemDropRule DreadVaria = expertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityVariaDreadSuitHelmet>(), 288));
				DreadVaria.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityVariaDreadSuitBreastplate>(), 1));
				DreadVaria.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityVariaDreadSuitGreaves>(), 1));
				IItemDropRule DreadGravity = expertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityGravityDreadSuitHelmet>(), 288));
				DreadGravity.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityGravityDreadSuitBreastplate>(), 1));
				DreadGravity.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityGravityDreadSuitGreaves>(), 1));
				IItemDropRule Fusion = expertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityFusionSuitHelmet>(), 288));
				Fusion.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityFusionSuitBreastplate>(), 1));
				Fusion.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityFusionSuitGreaves>(), 1));
				IItemDropRule FusionVaria = expertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityFusionVariaSuitHelmet>(), 288));
				FusionVaria.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityFusionVariaSuitBreastplate>(), 1));
				FusionVaria.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityFusionVariaSuitGreaves>(), 1));
				IItemDropRule FusionGravity = expertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityFusionGravitySuitHelmet>(), 288));
				FusionGravity.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityFusionGravitySuitBreastplate>(), 1));
				FusionGravity.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityFusionGravitySuitGreaves>(), 1));
				IItemDropRule Omega = expertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityFusionOmegaSuitHelmet>(), 288));
				Omega.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityFusionOmegaSuitBreastplate>(), 1));
				Omega.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.VanityFusionOmegaSuitGreaves>(), 1));
				IItemDropRule Retro = expertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.RetroSuitHelmet>(), 288));
				Retro.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.RetroSuitBreastplate>(), 1));
				Retro.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.RetroSuitGreaves>(), 1));
				IItemDropRule RetroVaria = expertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.RetroVariaSuitHelmet>(), 288));
				RetroVaria.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.RetroVariaSuitBreastplate>(), 1));
				RetroVaria.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Vanity.RetroVariaSuitGreaves>(), 1));

				itemLoot.Add(expertRule);
			}
		}
	}
}
