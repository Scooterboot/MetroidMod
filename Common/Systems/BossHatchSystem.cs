using MetroidMod.Content.Hatches;
using MetroidMod.Content.Hatches.Variants;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Common.Systems
{
	internal class BossHatchSystem : ModSystem
	{
		private bool oldAnyBossAlive;
		private bool anyBossAlive;
		private bool bossWasKilled;

		public override void PreUpdateNPCs()
		{
			anyBossAlive = false;
		}
		public override void PostUpdateNPCs()
		{
			if (anyBossAlive != oldAnyBossAlive)
			{
				oldAnyBossAlive = anyBossAlive;
				foreach (TileEntity tileEntity in TileEntity.ByID.Values)
				{
					if (tileEntity is not HatchTileEntity hatchTE)
					{
						continue;
					}

					if (hatchTE.Hatch is BlueHatch && hatchTE.Behavior.BlueConversion != HatchBlueConversionStatus.Disabled)
					{
						// We can't turn a blue hatch blue! Activate secret boss functionality.
						if(anyBossAlive)
						{
							hatchTE.Behavior.Lock();
						}
						else
						{
							hatchTE.Behavior.Unlock(bossWasKilled);
						}
					}
				}

				if(!anyBossAlive)
				{
					bossWasKilled = false;
				}
			}
		}
		
		private class BossHatchGlobalNPC : GlobalNPC
		{
			public override void AI(NPC npc)
			{
				if(npc.boss)
				{
					ModContent.GetInstance<BossHatchSystem>().anyBossAlive = true;
				}
			}

			public override void OnKill(NPC npc)
			{
				if (npc.boss)
				{
					ModContent.GetInstance<BossHatchSystem>().bossWasKilled = true;
				}
			}
		}
	}
}
