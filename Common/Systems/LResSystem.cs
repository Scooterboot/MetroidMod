using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria;
using Terraria.ModLoader;
using MetroidMod;
using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;

namespace MetroidMod.Common.Systems //There's probably a way to do all of this inside of DrawReserveHearts but this should work too. I hope.
{
	internal class LResSystem : ModResourceOverlay //LRes makes me think of L rizz. I'm not sorry.
	{
		/* Life reserve used to just do all of the drawing inside of MUISystem.DrawReserveHearts
		   But then they added more health UI styles, all of which are built entirely different
		   So now this is here to pick the right textures and draw them. Most of this is from
		   ExampleMod so. Yeah.*/
		// This field is used to cache vanilla assets used in the CompareAssets helper method further down in this file
		private Dictionary<string, Asset<Texture2D>> vanillaAssetCache = new();

		// These fields are used to cache the result of ModContent.Request<Texture2D>()
		private Asset<Texture2D> heartTexture, fancyHeartTexture, fancyPanelTexture, barsFillingTexture, barsPanelTexture;

		private bool MkV = false; //lets me hot-swap which textures the code goes for. Default is heart because I don't trust my ability


		public override void PostDrawResource(ResourceOverlayDrawContext context)
		{
			//everything from here to the first reference to context used to be in MUISystem under DrawReserveHearts
			//Had to move it here to make all this work
			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>();

			if (mp.reserveTanks > 0) //NONE of this stuff can run if you don't have reserve tanks
			{

				//Texture2D texHeart = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/ReserveHeart").Value; //points to classic heart
				if (P.whoAmI == Main.myPlayer && P.active && !P.dead && !P.ghost) //Make sure the player isn't dead, then begin checking if they're at full health.
				{
					//section 1 - Determine the player's max health
					float lifePerHeart = 20f;
					int num = Main.player[Main.myPlayer].statLifeMax / 20; //attempt to find current number of hearts.
					int num2 = (Main.player[Main.myPlayer].statLifeMax - 400) / 5; //checks for life fruit which messes up num
					if (num2 < 0)
					{
						num2 = 0; //no life fruit found
					}
					if (num2 > 0) //life fruit found
					{
						num = Main.player[Main.myPlayer].statLifeMax / (20 + num2 / 4); //accounts for life fruit num2 found
						lifePerHeart = (float)Main.player[Main.myPlayer].statLifeMax / 20f; //life per heart given the amount of life fruits
					}
					int num3 = Main.player[Main.myPlayer].statLifeMax2 - Main.player[Main.myPlayer].statLifeMax; //checks how much extra max health the player has
					lifePerHeart += (float)(num3 / num); //get how much extra health is in each heart, and add it to the current life per heart
					int num4 = (int)((float)Main.player[Main.myPlayer].statLifeMax2 / lifePerHeart);
					if (num4 >= 10)
					{
						num4 = 10;
					}
					//section 2
					for (int i = 1; i < mp.reserveHearts + 1; i++)
					{
						float num5 = 1f;
						bool flag = false;
						int num6;
						if ((float)Main.player[Main.myPlayer].statLife >= (float)i * lifePerHeart)
						{
							num6 = 255;
							if ((float)Main.player[Main.myPlayer].statLife == (float)i * lifePerHeart)
							{
								flag = true;
							}
						}
						else
						{
							float num7 = ((float)Main.player[Main.myPlayer].statLife - (float)(i - 1) * lifePerHeart) / lifePerHeart;
							num6 = (int)(30f + 225f * num7);
							if (num6 < 30)
							{
								num6 = 30;
							}
							num5 = num7 / 4f + 0.75f;
							if ((double)num5 < 0.75)
							{
								num5 = 0.75f;
							}
							if (num7 > 0f)
							{
								flag = true;
							}
						}
						if (flag)
						{
							num5 += Main.cursorScale - 1f;
						}
						int num8 = 0;
						int num9 = 0;
						if (i > 10)
						{
							num8 -= 260;
							num9 += 26;
						}
						int a = (int)((double)((float)num6) * 0.9);
						//sb.Draw(texHeart, new Vector2((float)(500 + 26 * (i - 1) + num8 + (Main.screenWidth - 800) + Terraria.GameContent.TextureAssets.Heart.Value.Width / 2), 32f + ((float)Terraria.GameContent.TextureAssets.Heart.Value.Height - (float)Terraria.GameContent.TextureAssets.Heart.Value.Height * num5) / 2f + (float)num9 + (float)(Terraria.GameContent.TextureAssets.Heart.Value.Height / 2)), new Rectangle?(new Rectangle(0, 0, texHeart.Width, texHeart.Height)), new Color(num6, num6, num6, a), 0f, new Vector2((float)(texHeart.Width / 2), (float)(texHeart.Height / 2)), num5, SpriteEffects.None, 0f);
						//old code to draw the texture for the hearts
						Asset<Texture2D> asset = context.texture;

						string fancyFolder = "Images/UI/PlayerResourceSets/FancyClassic/";
						string barsFolder = "Images/UI/PlayerResourceSets/HorizontalBars/";

						if (context.resourceNumber >= 1 * mp.reserveHearts)
							return;

						if (mp.reserveHeartsValue == 25)
						{
							MkV = true;
						}
						else
						{
							MkV = false;
						}

					// NOTE: CompareAssets is defined below this method's body
					if (asset == TextureAssets.Heart || asset == TextureAssets.Heart2)
					{
						// Draw over the Classic hearts
						DrawClassicOverlay(context);
					}
					else if (CompareAssets(asset, fancyFolder + "Heart_Left") || CompareAssets(asset, fancyFolder + "Heart_Middle") || CompareAssets(asset, fancyFolder + "Heart_Right") || CompareAssets(asset, fancyFolder + "Heart_Right_Fancy") || CompareAssets(asset, fancyFolder + "Heart_Single_Fancy"))
					{
						// Draw over the Fancy heart panels
						DrawFancyPanelOverlay(context);
					}
					else if (CompareAssets(asset, barsFolder + "HP_Panel_Middle"))
						{
							// Draw over the Bars middle life panels
							DrawBarsPanelOverlay(context);
						}
					}
				}
			}
		}

		private bool CompareAssets(Asset<Texture2D> existingAsset, string compareAssetPath)
		{
			// This is a helper method for checking if a certain vanilla asset was drawn
			if (!vanillaAssetCache.TryGetValue(compareAssetPath, out var asset))
				asset = vanillaAssetCache[compareAssetPath] = Main.Assets.Request<Texture2D>(compareAssetPath);

			return existingAsset == asset;
		}

		private void DrawClassicOverlay(ResourceOverlayDrawContext context)
		{
			// Draw over the Classic hearts
			// "context" contains information used to draw the resource
			// If you want to draw directly on top of the vanilla hearts, just replace the texture and have the context draw the new texture
			context.position += new Vector2(-2, -2);
			context.texture = heartTexture ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/UI/LifeReserve/ResClassic");
			if (MkV == true){ context.color = Color.LimeGreen;}

			else { context.color = Color.DarkViolet;}
			context.Draw();
		}

		/*private void DrawFancyOverlay(ResourceOverlayDrawContext context)
		{
			// Draw over the Fancy hearts
			// "context" contains information used to draw the resource
			// If you want to draw directly on top of the vanilla hearts, just replace the texture and have the context draw the new texture
			context.texture = fancyHeartTexture ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/UI/LifeReserve/LifeOverlay");
			context.Draw(); Also entirely unnecessary!
		}*/

		// Drawing over the panel backgrounds is very required, actually, that's the entire reason we're here.
		private void DrawFancyPanelOverlay(ResourceOverlayDrawContext context)
		{
			// Draw over the Fancy heart panels
			string fancyFolder = "Images/UI/PlayerResourceSets/FancyClassic/";

			// The original position refers to the entire panel slice.
			// However, since this overlay only modifies the "inner" portion of the slice (aka the part behind the heart),
			// the position should be modified to compensate for the sprite size difference

			if (context.resourceNumber == context.snapshot.AmountOfLifeHearts - 1)
			{
					// Other panels existed in this panel's row
					// Vanilla texture is "Heart_Right_Fancy"
					context.texture = fancyPanelTexture ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/UI/LifeReserve/ResDXRight");
			}
			else if (CompareAssets(context.texture, fancyFolder + "Heart_Left"))
			{
				// First panel in this row
				context.texture = fancyPanelTexture ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/UI/LifeReserve/ResDXLeft");
			}
			else if (CompareAssets(context.texture, fancyFolder + "Heart_Middle"))
			{
				// Any panel that has a panel to its left AND right
				context.position += new Vector2(-4, 0);
				context.texture = fancyPanelTexture ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/UI/LifeReserve/ResDXMiddle");
			}
			else
			{
				// Failsafe in case something else is wonky
				context.texture = fancyPanelTexture ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/UI/LifeReserve/ResDXSingle");
			}

			// "context" contains information used to draw the resource
			// If you need to adjust the positioning of the entire batch just drop a [[context.position = new Vector2()]] here
			if (MkV == true) { context.color = Color.LimeGreen; }

			else { context.color = Color.DarkViolet; }
			context.Draw();
		}

		/*private void DrawBarsOverlay(ResourceOverlayDrawContext context)
		{
			// Draw over the Bars life bars
			// "context" contains information used to draw the resource
			// If you want to draw directly on top of the vanilla bars, just replace the texture and have the context draw the new texture
			context.texture = barsFillingTexture ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/UI/LifeReserve/BarsLifeOverlay_Fill");
			context.Draw(); We also do not need this.
		} */

		private void DrawBarsPanelOverlay(ResourceOverlayDrawContext context)
		{
			// Draw over the Bars middle life panels
			// "context" contains information used to draw the resource
			// If you want to draw directly on top of the vanilla bar panels, just replace the texture and have the context draw the new texture

			//I gave it a good shot but I dunno how to make endcaps and startcaps and whatevercaps work right.
			context.texture = barsPanelTexture ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/UI/LifeReserve/ResBar");
			if (MkV == true) { context.color = Color.LimeGreen; }

			else { context.color = Color.DarkViolet; }
			context.Draw();
		}

	}
}
