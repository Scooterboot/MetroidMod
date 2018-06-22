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
using MetroidMod.Items;
using MetroidMod.Items.equipables;

namespace MetroidMod.Mounts
{
	public class MorphBallMount : ModMountData
	{
		public override void SetDefaults()
		{
			mountData.spawnDust = 299;//63;
			mountData.spawnDustNoGravity = true;
			mountData.buff = mod.BuffType("MorphBallMountBuff");
			mountData.heightBoost = -(42 - MPlayer.morphSize);
			
			mountData.runSpeed = 3f;
			mountData.acceleration = 0.08f;
			mountData.dashSpeed = 3f;
			
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 5.01f;
			
			mountData.fallDamage = 0f;
			mountData.blockExtraJumps = true;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.constantJump = true;
			
			mountData.totalFrames = 6;
			int[] array = new int[mountData.totalFrames];
			mountData.playerYOffsets = array;
			mountData.playerHeadOffset = mountData.heightBoost;
			/*if (Main.netMode != 2)
			{
				mountData.textureWidth = mountData.backTexture.Width + 20;
				mountData.textureHeight = mountData.backTexture.Height;
			}*/
		}

		public override void UpdateEffects(Player player)
		{
			/*mountData.runSpeed = 3f;
			mountData.acceleration = 0.08f;
			mountData.dashSpeed = 3f;
			mountData.acceleration *= player.moveSpeed;
			mountData.runSpeed *= player.moveSpeed;
			
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 5.01f;
			if (player.jumpBoost)
			{
				mountData.jumpHeight = 20;
				mountData.jumpSpeed = 6.51f;
			}
			if (player.wereWolf)
			{
				mountData.jumpHeight += 2;
				mountData.jumpSpeed += 0.2f;
			}
			mountData.jumpSpeed += player.jumpSpeedBoost;*/
			
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			
			MorphBall mBall = (MorphBall)player.miscEquips[3].modItem;
			if(mBall != null)
			{
				BallUI ballUI = mBall.ballUI;
				
				int pb = mod.ItemType("PowerBombAddon");
				int sb = mod.ItemType("SpiderBallAddon");
				int bb = mod.ItemType("BoostBallAddon");

				Item slotDrill = ballUI.ballSlot[0].item;
				Item slotBomb = ballUI.ballSlot[1].item;
				Item slotSpecial = ballUI.ballSlot[2].item;
				Item slotUtility = ballUI.ballSlot[3].item;
				Item slotBoost = ballUI.ballSlot[4].item;


				mp.morphBall = true;
				mp.MorphBallBasic(player);
				if (!slotDrill.IsAir)
				{
					MGlobalItem drillMItem = slotDrill.GetGlobalItem<MGlobalItem>(mod);
					mp.Drill(player,drillMItem.drillPower);
				}
				if (!slotBomb.IsAir)
				{
					MGlobalItem bombMItem = slotBomb.GetGlobalItem<MGlobalItem>(mod);
					mp.bombDamage = (int)(player.rangedDamage * bombMItem.bombDamage);
					mp.Bomb(player);
				}
				if (slotSpecial.type == pb)
				{
					mp.PowerBomb(player);
				}
				if (slotUtility.type == sb)
				{
					mp.SpiderBall(player);
				}
				else
				{
					mp.spiderball = false;
				}
				if (slotBoost.type == bb)
				{
					mp.BoostBall(player);
				}
				else
				{
					mp.boostCharge = 0;
					mp.boostEffect = 0;
				}
			}
		}
	}
}