using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Common
{
	public class BallLayer : PlayerDrawLayer
	{
		//TODO spiderball lags multiplayer like crazy
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.FrontAccFront);
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.GetModPlayer<MPlayer>().morphBall;

		public override bool IsHeadLayer => false;

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mp = drawPlayer.GetModPlayer<MPlayer>();

			if (!drawPlayer.active || drawPlayer.outOfRange || Main.gameMenu) return;

			Texture2D tex =/* mp.spiderball ? ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Morphball_Spiderball").Value :*/ ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Morphball").Value;
			Texture2D tex3 = /*mp.spiderball ? ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Morphball_Spiderball_Dye").Value :*/ ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Morphball_Dye").Value;
			Texture2D boost = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Boostball").Value;
			Texture2D tex2 = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Morphball_Light").Value;
			Texture2D spiderTex = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Spiderball").Value;
			Texture2D trail = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Morphball_Trail").Value;

			drawInfo.DrawDataCache.Clear();

			float thisx = (int)(drawInfo.Position.X + (drawPlayer.width / 2));
			float thisy = (int)(drawInfo.Position.Y + (drawPlayer.height / 2));

			Vector2 ballDims = new Vector2(28f, 28f);
			Vector2 thispos = new Vector2(thisx, thisy) - Main.screenPosition;

			if (drawInfo.shadow == 0f)
			{
				int timez = (int)(mp.Time % 60) / 10;

				SpriteEffects effects = SpriteEffects.None;
				if (drawPlayer.direction == -1)
					effects = SpriteEffects.FlipHorizontally;
				if (drawPlayer.gravDir == -1f)
					effects |= SpriteEffects.FlipVertically;

				float ballrotoffset = 0f;
				if (drawPlayer.velocity.Y != Vector2.Zero.Y)
				{
					if (drawPlayer.velocity.X != 0f)
						ballrotoffset += 0.05f * drawPlayer.velocity.X;
					else
						ballrotoffset += 0.25f * drawPlayer.direction;
				}
				else if (drawPlayer.velocity.X < 0f)
					ballrotoffset -= 0.2f;
				else if (drawPlayer.velocity.X > 0f)
					ballrotoffset += 0.2f;

				if (drawPlayer.velocity.X != 0f)
					ballrotoffset += 0.025f * drawPlayer.velocity.X;
				else
					ballrotoffset += 0.125f * drawPlayer.direction;

				if (mp.spiderball && mp.CurEdge != Edge.None)
					mp.ballrot += mp.spiderSpeed * 0.085f;
				else
					mp.ballrot += ballrotoffset;

				if (Mount.currentShader > 0)
				{
					mp.morphColor = Color.White;
					tex = tex3;
				}
				Color mColor = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.Position.Y + (double)drawPlayer.height * 0.5) / 16, mp.morphColor), 0f);
				float scale = 0.57f;
				int offset = 4;
				if (mp.ballstate && !drawPlayer.dead)
				{
					DrawData data;
					for (int i = 0; i < mp.oldPos.Length; i++)
					{
						Color color23 = mp.morphColorLights;
						if (mp.shineActive)// || (mp.shineCharge > 0 && mp.shineChargeFlash >= 4))
						{
							color23 = new Color(255, 216, 0, 255);
						}
						else if (mp.speedBoosting)
						{
							color23 = new Color(0, 200, 255, 255);
						}
						if (mp.boostEffect > 0)
						{
							Color gold = new Color(255, 255, 0, 255);
							color23 = Color.Lerp(color23, gold, (float)mp.boostEffect / 60f);
						}
						color23 *= (mp.oldPos.Length - (i)) / 15f;

						Vector2 drawPos = mp.oldPos[i] - Main.screenPosition + new Vector2((int)(drawPlayer.width / 2), (int)(drawPlayer.height / 2));

						if (drawPos != thispos)
						{
							data = new DrawData(trail, drawPos, new Rectangle?(new Rectangle(0, 0, trail.Width, trail.Height)), color23, mp.ballrot, ballDims / 2, scale, effects, 0);

							drawInfo.DrawDataCache.Add(data);
						}
					}

					data = new DrawData(tex, thispos, new Rectangle?(new Rectangle(0, ((int)ballDims.Y + offset) * timez, (int)ballDims.X, (int)ballDims.Y)), mColor, mp.ballrot, ballDims / 2, scale, effects, 0)
					{
						//shader = drawPlayer.mount.currentShader;
						shader = Mount.currentShader
					};
					drawInfo.DrawDataCache.Add(data);

					data = new DrawData(tex2, thispos, new Rectangle?(new Rectangle(0, ((int)ballDims.Y + offset) * timez, (int)ballDims.X, (int)ballDims.Y)), mp.morphColorLights, mp.ballrot, ballDims / 2, scale, effects, 0);
					drawInfo.DrawDataCache.Add(data);

					for (int i = 0; i < mp.boostEffect; i++)
					{
						data = new DrawData(boost, thispos, new Rectangle?(new Rectangle(0, 0, boost.Width, boost.Height)), mp.boostGold * 0.5f, mp.ballrot, ballDims / 2, scale, effects, 0);
						drawInfo.DrawDataCache.Add(data);
					}
					for (int i = 0; i < mp.boostCharge; i++)
					{
						data = new DrawData(boost, thispos, new Rectangle?(new Rectangle(0, 0, boost.Width, boost.Height)), mp.boostYellow * 0.5f, mp.ballrot, ballDims / 2, scale, effects, 0);
						drawInfo.DrawDataCache.Add(data);
					}

					if (mp.spiderball)
					{
						data = new DrawData(spiderTex, thispos, new Rectangle?(new Rectangle(0, 0, spiderTex.Width, spiderTex.Height)), mp.morphColorLights * 0.5f, mp.ballrot, new Vector2(spiderTex.Width / 2, spiderTex.Height / 2), scale, effects, 0);
						drawInfo.DrawDataCache.Add(data);
					}
				}
			}
		}
	}
}
