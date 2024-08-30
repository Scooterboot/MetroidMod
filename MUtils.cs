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
			foreach (NPC who in Main.ActiveNPCs)
			{
				NPC npc = Main.npc[who.whoAmI];
				if (npc.boss && npc.active && npc.type != ModContent.NPCType<IdleTorizo>() && npc.type != ModContent.NPCType<IdleGoldenTorizo>())
				{
					return true;
				}
			}
			return false;
		}

		public static bool CanReachWiring(Player player, Item item)
		{
			// Adapted from the mess that is decompiled vanilla code (specifically wire range because the copypaste in the disassembly goes hard)... like come on this feature ain't even that important why am I doing this?
			float rangeX = Player.tileRangeX + item.tileBoost + player.blockRange;
			float rangeY = Player.tileRangeY + item.tileBoost + player.blockRange;

			bool reachFromLeft = rangeX > ((player.Left.X / 16f) - Player.tileTargetX);
			bool reachFromRight = rangeX >= (Player.tileTargetX - (player.Right.X / 16f) + 1);
			bool reachFromTop = rangeY > ((player.Top.Y / 16f) - Player.tileTargetY);
			bool reachFromBottom = rangeY >= (Player.tileTargetY - (player.Bottom.Y / 16f) + 2);

			bool reaches = reachFromLeft && reachFromRight && reachFromTop && reachFromBottom;
			return reaches;
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

		public static Recipe AddSuitAddon<T>(this Recipe recipe, int stack = 1) where T : ModSuitAddon => recipe.AddIngredient(SuitAddonLoader.GetAddon<T>().ItemType, stack);
	}
}
