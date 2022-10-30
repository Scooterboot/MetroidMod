using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.GlobalNPCs;
using MetroidMod.Common.GlobalProjectiles;
using MetroidMod.Common.Players;
using MetroidMod.Content.NPCs.GoldenTorizo;
using MetroidMod.Content.NPCs.Torizo;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod
{
	public static class MUtils
	{
		/*public static string ConcatBeamNames(params ModBeam[] modBeams)
		{
			string result = "";
			for (int i = 0; i < modBeams.Length; i++)
			{
				result += modBeams[i].Name;//[propertyName];
			}
			return result;
		}*/

		public static bool AnyBossesActive()
		{
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].boss && Main.npc[i].active && Main.npc[i].type != ModContent.NPCType<IdleTorizo>() && Main.npc[i].type != ModContent.NPCType<IdleGoldenTorizo>())
				{
					return true;
				}
			}
			return false;
		}

		public static bool CalamityActive() => ModLoader.TryGetMod("CalamityMod", out _);
		public static bool CalamityMod(out Mod calamityMod) => ModLoader.TryGetMod("CalamityMod", out calamityMod);

		public static MPlayer MetroidPlayer(this Player p) => p.GetModPlayer<MPlayer>();
		public static bool TryMetroidPlayer(this Player p, out MPlayer mp) => p.TryGetModPlayer(out mp);

		public static MGlobalNPC MetroidGlobal(this NPC npc) => npc.GetGlobalNPC<MGlobalNPC>();
		public static bool TryMetroidGlobal(this NPC npc, out MGlobalNPC mgn) => npc.TryGetGlobalNPC(out mgn);

		public static MGlobalItem MetroidGlobal(this Item item) => item.GetGlobalItem<MGlobalItem>();
		public static bool TryMetroidGlobal(this Item item, out MGlobalItem mgn) => item.TryGetGlobalItem(out mgn);

		public static MGlobalProjectile MetroidGlobal(this Projectile proj) => proj.GetGlobalProjectile<MGlobalProjectile>();
		public static bool MetroidGlobal(this Projectile proj, out MGlobalProjectile mgp) => proj.TryGetGlobalProjectile(out mgp);
	}
}
