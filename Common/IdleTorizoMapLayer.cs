using MetroidMod.Common.Systems;
using MetroidMod.Content.NPCs.GoldenTorizo;
using MetroidMod.Content.NPCs.Torizo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;

namespace MetroidMod.Common
{
	public class IdleTorizoMapLayer : ModMapLayer
	{
		public static IdleTorizoMapLayer Instance;

		public IdleTorizoMapLayer()
		{
			Instance = this;
		}

		public override void Draw(ref MapOverlayDrawContext context, ref string text)
		{
			const float scaleIfNotSelected = 1f;
			const float scaleIfSelected = scaleIfNotSelected * 1.2f;

			var torizoHeadTexture = ModContent.Request<Texture2D>($"{nameof(MetroidMod)}/Content/NPCs/Torizo/IdleTorizo_Head", AssetRequestMode.ImmediateLoad);

			Rectangle room = MSystem.TorizoRoomLocation;
			if (!MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo) && !NPC.AnyNPCs(ModContent.NPCType<Torizo>()) && context.Draw(torizoHeadTexture.Value, new Vector2(room.Center.X, room.Center.Y), Color.White, new SpriteFrame(1, 1, 0, 0), scaleIfNotSelected, scaleIfSelected, Alignment.Center).IsMouseOver)
			{
				text = "???";//Language.GetTextValue("");
			}
			else if (NPC.downedGolemBoss && MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo) && !MSystem.bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo) && !NPC.AnyNPCs(ModContent.NPCType<GoldenTorizo>()) && MSystem.TorizoRoomLocation.X > 0 && MSystem.TorizoRoomLocation.Y > 0 && context.Draw(torizoHeadTexture.Value, new Vector2(room.Left + (room.Width / 2f), room.Top + (room.Height / 2f)), Color.White, new SpriteFrame(1, 1, 0, 0), scaleIfNotSelected, scaleIfSelected, Alignment.Center).IsMouseOver)
			{
				text = "???";//Language.GetTextValue("");
			}
		}
	}
}
