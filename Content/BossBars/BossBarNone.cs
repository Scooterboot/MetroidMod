using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.DataStructures;

namespace MetroidMod.Content.BossBars
{
	public class BossBarNone : ModBossBar
	{
		public override bool PreDraw(SpriteBatch spriteBatch, NPC npc, ref BossBarDrawParams drawParams)
		{
			return false;
		}
	}
}
