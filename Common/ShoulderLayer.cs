using MetroidMod.Common.Players;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Common
{
	public class ShoulderOnLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.TryMetroidPlayer(out MPlayer mp) && mp.ShouldDrawBreastplate)
			{
				ModSuitAddon msa = MPlayer.GetPowerSuit(drawInfo.drawPlayer)[0];
				if (msa != null && msa.OnShoulderTexture != null && msa.OnShoulderTexture != "" && ModContent.RequestIfExists<Texture2D>(msa.OnShoulderTexture, out Asset<Texture2D> tex))
				{
					MPlayer.DrawTexture(ref drawInfo, tex.Value, drawInfo.drawPlayer, drawInfo.drawPlayer.bodyFrame, drawInfo.drawPlayer.fullRotation, drawInfo.drawPlayer.bodyPosition, drawInfo.bodyVect, drawInfo.colorArmorBody, drawInfo.drawPlayer.cBody);
				}
			}
		}
	}

	public class ShoulderOffLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.OffhandAcc);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.TryMetroidPlayer(out MPlayer mp) && mp.ShouldDrawBreastplate)
			{
				ModSuitAddon msa = MPlayer.GetPowerSuit(drawInfo.drawPlayer)[0];
				if (msa != null && msa.OffShoulderTexture != null && msa.OffShoulderTexture != "" && ModContent.RequestIfExists<Texture2D>(msa.OffShoulderTexture, out Asset<Texture2D> tex))
				{
					MPlayer.DrawTexture(ref drawInfo, tex.Value, drawInfo.drawPlayer, drawInfo.drawPlayer.bodyFrame, drawInfo.drawPlayer.fullRotation, drawInfo.drawPlayer.bodyPosition, drawInfo.bodyVect, drawInfo.colorArmorBody, drawInfo.drawPlayer.cBody);
				}
			}
		}
	}
}
