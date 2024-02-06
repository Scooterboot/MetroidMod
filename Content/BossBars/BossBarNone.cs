using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

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
