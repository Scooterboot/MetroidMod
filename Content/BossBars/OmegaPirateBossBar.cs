using MetroidMod.Content.NPCs.OmegaPirate;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ModLoader;

namespace MetroidMod.Content.BossBars
{
	public class OmegaPirateBossBar : ModBossBar
	{
		private int bossHeadIndex = -1;

		public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame)
		{
			if (bossHeadIndex != -1)
			{
				return TextureAssets.NpcHeadBoss[bossHeadIndex];
			}
			return null;
		}

		public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax)/* tModPorter Note: life and shield current and max values are now separate to allow for hp/shield number text draw */
		{
			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (!npc.active)
				return false;

			bossHeadIndex = npc.GetBossHeadTextureIndex();

			life = npc.life;
			lifeMax = (float)npc.lifeMax;
			shieldMax = 20000f;

			if (npc.ModNPC is OmegaPirate body1)
			{
				life = npc.life;
				if (npc.ai[0] != 2)
				{
					shield = body1.NPCArmorHP;
					shieldMax = body1.NPCArmorHPMax;
				}
			}

			return true;
		}
	}
}
