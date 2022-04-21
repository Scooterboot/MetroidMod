using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MetroidModPorted.Common.GlobalItems;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Common
{
	public class ThrustLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Leggings);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Mod mod = MetroidModPorted.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();

			Item item = null;
			if (drawPlayer.armor[1] != null && !drawPlayer.armor[1].IsAir && drawPlayer.armor[1].ModItem != null)
			{
				item = drawPlayer.armor[1];
			}
			if (drawPlayer.armor[11] != null && !drawPlayer.armor[11].IsAir && drawPlayer.armor[11].ModItem != null)
			{
				item = drawPlayer.armor[11];
			}
			if (mPlayer.thrusters && item != null)
			{
				string name = item.ModItem.Texture + "_Thrusters";
				if (ModContent.RequestIfExists<Texture2D>(name, out Asset<Texture2D> tex) && name.Contains("MetroidMod"))
				{
					if ((drawPlayer.wings == 0 && drawPlayer.back == -1) || drawPlayer.velocity.Y == 0f || mPlayer.shineDirection != 0)
					{
						MPlayer.DrawTexture(spriteBatch, drawInfo, tex.Value, drawPlayer, drawPlayer.bodyFrame, drawPlayer.bodyRotation, drawPlayer.bodyPosition, drawInfo.bodyVect, drawInfo.colorArmorBody, drawInfo.cBody);
					}
				}
			}
		}
	}
}
