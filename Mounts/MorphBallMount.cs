#region Using directives

using Terraria;
using Terraria.ModLoader;

using MetroidMod.Items;
using MetroidMod.Items.accessories;

#endregion

namespace MetroidMod.Mounts
{
	public class MorphBallMount : ModMountData
	{
		public override void SetDefaults()
		{
			mountData.spawnDust = 299;//63;
			mountData.spawnDustNoGravity = true;
			mountData.buff = ModContent.BuffType<Buffs.MorphBallMountBuff>();
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

		MorphBall mBall;
		public override void UpdateEffects(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();

			if (player.grappling[0] != -1)
			{
				return;
			}

			if(player.miscEquips[3].type == mod.ItemType("MorphBall"))
			{
				mBall = (MorphBall)player.miscEquips[3].modItem;
			}
			else
			{
				mBall = null;
			}

			int sb = mod.ItemType("SpiderBallAddon");
			int bb = mod.ItemType("BoostBallAddon");

			mp.morphBall = true;
			mp.MorphBallBasic(player);
			if(mBall != null)
			{
				if (!mBall.ballMods[0].IsAir)
				{
					MGlobalItem drillMItem = mBall.ballMods[0].GetGlobalItem<MGlobalItem>();
					mp.Drill(player,drillMItem.drillPower);
				}
				if (!mBall.ballMods[1].IsAir)
				{
					MGlobalItem bombMItem = mBall.ballMods[1].GetGlobalItem<MGlobalItem>();
					//mp.bombDamage = (int)(player.rangedDamage * bombMItem.bombDamage);
					mp.bombDamage = player.GetWeaponDamage(mBall.ballMods[1]);
					mp.Bomb(player, bombMItem.bombType);
				}
				if(!mBall.ballMods[2].IsAir)
				{
					MGlobalItem pbMItem = mBall.ballMods[2].GetGlobalItem<MGlobalItem>();
					mp.PowerBomb(player,pbMItem.powerBombType,player.GetWeaponDamage(mBall.ballMods[2]));
				}

				if (mBall.ballMods[3].type == sb)
				{
					mp.SpiderBall(player);
				}
				else
				{
					mp.spiderball = false;
				}

				if (mBall.ballMods[4].type == bb)
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
