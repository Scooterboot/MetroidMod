#region Using directives

using Terraria;
using Terraria.ModLoader;

using MetroidModPorted.Common.GlobalItems;
using MetroidModPorted.Common.Players;
using MetroidModPorted.Content.Items;
using MetroidModPorted.Content.Items.Accessories;

#endregion

namespace MetroidModPorted.Content.Mounts
{
	public class MorphBallMount : ModMount//ModMountData
	{
		public override void SetStaticDefaults()
		{
			MountData.spawnDust = 299;//63;
			MountData.spawnDustNoGravity = true;
			MountData.buff = ModContent.BuffType<Buffs.MorphBallMountBuff>();
			MountData.heightBoost = -(42 - MPlayer.morphSize);
			MountData.runSpeed = 3f;
			MountData.acceleration = 0.08f;
			MountData.dashSpeed = 3f;

			MountData.jumpHeight = 15;
			MountData.jumpSpeed = 5.01f;

			MountData.fallDamage = 0f;
			MountData.blockExtraJumps = true;
			MountData.flightTimeMax = 0;
			MountData.fatigueMax = 0;
			MountData.constantJump = true;

			MountData.totalFrames = 6;
			int[] array = new int[MountData.totalFrames];
			MountData.playerYOffsets = array;
			MountData.playerHeadOffset = MountData.heightBoost;
		}

		private MorphBall mBall;
		public override void UpdateEffects(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();

			if (player.grappling[0] != -1)
			{
				return;
			}

			player.armorEffectDrawShadowLokis = false;
			player.armorEffectDrawOutlines = false;

			if (player.miscEquips[3].type == ModContent.ItemType<MorphBall>())
			{
				mBall = (MorphBall)player.miscEquips[3].ModItem;
			}
			else
			{
				mBall = null;
			}

			//int sb = ModContent.Find<ModItem>("SpiderBallAddon").Type;
			//int bb = ModContent.Find<ModItem>("BoostBallAddon").Type;

			mp.morphBall = true;
			mp.MorphBallBasic(player);
			if(mBall != null)
			{
				ModMBAddon modMBAddon;
				if (!mBall.ballMods[0].IsAir)
				{
					//MGlobalItem drillMItem = mBall.ballMods[0].GetGlobalItem<MGlobalItem>();
					if (MBAddonLoader.TryGetAddon(mBall.ballMods[0], out modMBAddon)) { modMBAddon.UpdateEquip(player); }
					//mp.Drill(player,drillMItem.drillPower);
				}
				if (!mBall.ballMods[1].IsAir)
				{
					//MGlobalItem bombMItem = mBall.ballMods[1].GetGlobalItem<MGlobalItem>();
					if (MBAddonLoader.TryGetAddon(mBall.ballMods[1], out modMBAddon)) { modMBAddon.UpdateEquip(player); }
					//mp.bombDamage = (int)(player.rangedDamage * bombMItem.bombDamage);
					mp.bombDamage = player.GetWeaponDamage(mBall.ballMods[1]);
					//mp.Bomb(player, bombMItem.bombType, mBall.ballMods[1]);
				}
				if(!mBall.ballMods[2].IsAir)
				{
					//MGlobalItem pbMItem = mBall.ballMods[2].GetGlobalItem<MGlobalItem>();
					if (MBAddonLoader.TryGetAddon(mBall.ballMods[2], out modMBAddon)) { modMBAddon.UpdateEquip(player); }
					//mp.PowerBomb(player,pbMItem.powerBombType,player.GetWeaponDamage(mBall.ballMods[2]), mBall.ballMods[2]);
				}

				/*if (mBall.ballMods[3].type == sb)
				{
					mp.SpiderBall(player);
				}
				else
				{*/
					if (MBAddonLoader.TryGetAddon(mBall.ballMods[3], out modMBAddon))
					{
						modMBAddon.UpdateEquip(player);
					}
					else
					{
						mp.spiderball = false;
					}
				/*}*/

				/*if (mBall.ballMods[4].type == bb)
				{
					mp.BoostBall(player);
				}
				else
				{*/
					if (MBAddonLoader.TryGetAddon(mBall.ballMods[4], out modMBAddon))
					{
						modMBAddon.UpdateEquip(player);
					}
					else
					{
						mp.boostCharge = 0;
						mp.boostEffect = 0;
					}
				/*}*/
			}
		}
	}
}
