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
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.GetModPlayer<MPlayer>().isPowerSuit || drawInfo.drawPlayer.GetModPlayer<MPlayer>().isLegacySuit;

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			//Mod mod = MetroidModPorted.Instance;
			//SpriteBatch spriteBatch = Main.spriteBatch;
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
				if (ModContent.RequestIfExists(name, out Asset<Texture2D> tex) && name.Contains("MetroidModPorted"))
				{
					if ((drawPlayer.wings == 0 && drawPlayer.back == -1) || drawPlayer.velocity.Y == 0f || mPlayer.shineDirection != 0)
					{
						MPlayer.DrawTexture(ref drawInfo, tex.Value, drawPlayer, drawPlayer.bodyFrame, drawPlayer.bodyRotation, drawPlayer.bodyPosition, drawInfo.bodyVect, drawInfo.colorArmorBody, drawInfo.cBody);
					}
				}
			}
		}
	}
	public class JetLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.GetModPlayer<MPlayer>().isPowerSuit || drawInfo.drawPlayer.GetModPlayer<MPlayer>().isLegacySuit;

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();
			if (mPlayer.jet && !drawPlayer.sandStorm && drawInfo.shadow == 0f)
			{
				if ((drawPlayer.wings == 0 && drawPlayer.back == -1) || drawPlayer.velocity.Y == 0f || mPlayer.shineDirection != 0)
				{
					Texture2D tex = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/thrusterFlameNew").Value;
					if (mPlayer.shineDirection != 0 || mPlayer.SMoveEffect > 15)
					{
						tex = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/thrusterFlameNew_Spark").Value;
					}
					if (mPlayer.thrusters)
					{
						tex = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/thrusterFlame").Value;
					}
					mPlayer.DrawThrusterJet(ref drawInfo, tex, drawPlayer, drawPlayer.bodyRotation, drawPlayer.bodyPosition);
				}
			}
		}
	}
}
