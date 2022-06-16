using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.GameContent.UI.BigProgressBar;
using MetroidMod.Content.NPCs.OmegaPirate;

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

		public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float lifePercent, ref float shieldPercent)
		{
			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (!npc.active)
				return false;

			bossHeadIndex = npc.GetBossHeadTextureIndex();

			lifePercent = Utils.Clamp(npc.life / (float)npc.lifeMax, 0f, 1f);

			if (npc.ModNPC is OmegaPirate body1)
			{
				lifePercent = Utils.Clamp(npc.life / (float)npc.lifeMax, 0f, 1f);
				shieldPercent = Utils.Clamp((float)body1.NPCArmorHP / 20000f, 0f, 1f);
			}

			return true;
		}
	}
}
