using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;

namespace MetroidMod
{
	internal static class DebugAssist
	{
		/// <summary>
		/// Print a message in the chat from any netmode, while also printing the netmode
		/// </summary>
		public static void NewTextMP(object message)
		{
			// TODO disabled for now. maybe should re-check logging best practices to see what to do with these statements
			return;

			string label = Main.netMode switch
			{
				NetmodeID.SinglePlayer => "SP",
				NetmodeID.MultiplayerClient => $"Client #{Main.myPlayer}",
				NetmodeID.Server => "Server",
				_ => "???",
			};

			string text = $"[{label}] {message}";
			
			if(Main.netMode == NetmodeID.Server)
			{
				ChatHelper.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral(text), Color.White);
			}
			else
			{
				Main.NewText(text);
			}
		}
	}
}
