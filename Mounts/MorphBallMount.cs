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
		}

		Item[] slotItem = new Item[5];
		public override void UpdateEffects(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			
			if(player.miscEquips[3].type == mod.ItemType("MorphBall"))
			{
				MorphBall mBall = (MorphBall)player.miscEquips[3].modItem;
				if(mBall != null)
				{
					BallUI ballUI = mBall.ballUI;
					
					for(int i = 0; i < 5; i++)
					{
						slotItem[i] = ballUI.ballSlot[i].item;
					}
				}
			}
			int pb = mod.ItemType("PowerBombAddon");
			int sb = mod.ItemType("SpiderBallAddon");
			int bb = mod.ItemType("BoostBallAddon");

			mp.morphBall = true;
			mp.MorphBallBasic(player);
			if (slotItem[0] != null && !slotItem[0].IsAir)
			{
				MGlobalItem drillMItem = slotItem[0].GetGlobalItem<MGlobalItem>(mod);
				mp.Drill(player,drillMItem.drillPower);
			}
			if (slotItem[1] != null && !slotItem[1].IsAir)
			{
				MGlobalItem bombMItem = slotItem[1].GetGlobalItem<MGlobalItem>(mod);
				mp.bombDamage = (int)(player.rangedDamage * bombMItem.bombDamage);
				mp.Bomb(player);
			}
			if (slotItem[2] != null && slotItem[2].type == pb)
			{
				mp.PowerBomb(player);
			}
			if (slotItem[3] != null && slotItem[3].type == sb)
			{
				mp.SpiderBall(player);
			}
			else
			{
				mp.spiderball = false;
			}
			if (slotItem[4] != null && slotItem[4].type == bb)
			{
				mp.BoostBall(player);
			}
			else
			{
				mp.boostCharge = 0;
				mp.boostEffect = 0;
			}
			
			for(int i = 0; i < 5; i++)
			{
				slotItem[i] = null;
			}
		}
	}
}
