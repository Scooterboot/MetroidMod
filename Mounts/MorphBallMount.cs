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
        
		public override void UpdateEffects(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			
			MorphBall mBall = (MorphBall)player.miscEquips[3].modItem;

			int pb = mod.ItemType("PowerBombAddon");
			int sb = mod.ItemType("SpiderBallAddon");
			int bb = mod.ItemType("BoostBallAddon");

			mp.morphBall = true;
			mp.MorphBallBasic(player);
			if (!mBall.ballMods[0].IsAir)
			{
				MGlobalItem drillMItem = mBall.ballMods[0].GetGlobalItem<MGlobalItem>(mod);
				mp.Drill(player,drillMItem.drillPower);
			}
			if (!mBall.ballMods[1].IsAir)
			{
				MGlobalItem bombMItem = mBall.ballMods[1].GetGlobalItem<MGlobalItem>(mod);
				mp.bombDamage = (int)(player.rangedDamage * bombMItem.bombDamage);
				mp.Bomb(player);
			}
			if (mBall.ballMods[2].type == pb)
				mp.PowerBomb(player);

			if (mBall.ballMods[3].type == sb)
				mp.SpiderBall(player);
			else
				mp.spiderball = false;

			if (mBall.ballMods[4].type == bb)
				mp.BoostBall(player);
			else
			{
				mp.boostCharge = 0;
				mp.boostEffect = 0;
			}
		}
	}
}
