using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Common
{
	public class LargeTailLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.WaistAcc);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawInfo.drawPlayer.GetModPlayer<MPlayer>();
			if (mPlayer.largeTailTex != null)
			{
				if (ModContent.RequestIfExists<Texture2D>(mPlayer.largeTailTex, out Asset<Texture2D> tex))
				{
					Rectangle frame = new Rectangle(drawPlayer.legFrame.X, drawPlayer.legFrame.Y, tex.Width(), tex.Height() / 20);
					MPlayer.DrawTexture(ref drawInfo, tex.Value, drawPlayer, frame, drawPlayer.fullRotation, drawPlayer.bodyPosition, drawInfo.bodyVect, drawInfo.colorArmorBody, mPlayer.largeTailDye);
				}
			}
		}
	}
}
