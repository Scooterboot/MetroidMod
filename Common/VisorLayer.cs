using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MetroidMod.Common.Players;

namespace MetroidMod.Common
{
	public class VisorLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();
			if (mPlayer.isLegacySuit && !mPlayer.ballstate)
			{
				Texture2D tex = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/VisorGlow").Value;
				drawInfo.DrawDataCache.Add(new DrawData(
					tex,
					drawInfo.Position,
					null,
					drawPlayer.GetImmuneAlphaPure(mPlayer.visorGlowColor, drawInfo.shadow),
					drawPlayer.headRotation,
					tex.Size()*0.5f,
					1f,
					SpriteEffects.None,
					0
				));
				//mPlayer.DrawTexture(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.headRotation, drawPlayer.bodyPosition, drawInfo.headOrigin, drawPlayer.GetImmuneAlphaPure(mPlayer.visorGlowColor, drawInfo.shadow), 0);
			}
		}
	}
}
