using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Common
{
	// "Aiming" sprite
	public class GunItemLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) =>
			drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Tools.NovaLaserDrill>() ||
			drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Weapons.ArmCannon>();
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player P = drawInfo.drawPlayer;
			Item I = P.inventory[P.selectedItem];
			MPlayer mPlayer = P.GetModPlayer<MPlayer>();
			if (drawInfo.shadow != 0f || P.frozen || ((P.itemAnimation <= 0 || I.useStyle == 0) && (I.holdStyle <= 0 || P.pulley)) || I.type <= 0 || P.dead || I.noUseGraphic || (P.wet && I.noWet) || mPlayer.somersault)
			{
				return;
			}

			if (I.type == ModContent.ItemType<Content.Items.Weapons.ArmCannon>() || I.type == ModContent.ItemType<Content.Items.Tools.NovaLaserDrill>())
			{
				Texture2D tex = Terraria.GameContent.TextureAssets.Item[I.type].Value;
				MGlobalItem mi = I.GetGlobalItem<MGlobalItem>();
				if (mi.itemTexture != null)
				{
					tex = mi.itemTexture;
				}
				Color currentColor = Lighting.GetColor((int)(drawInfo.Position.X + (P.width * 0.5)) / 16, (int)((drawInfo.Position.Y + (P.height * 0.5)) / 16.0));

				int num80 = 10;
				Vector2 vector7 = new(tex.Width / 2, tex.Height / 2);
				Vector2 vector8 = Main.DrawPlayerItemPos(P.gravDir, I.type);
				num80 = (int)vector8.X;
				vector7.Y = vector8.Y;
				Vector2 origin4 = new(-num80, tex.Height / 2);
				if (P.direction == -1)
				{
					origin4 = new Vector2(tex.Width + num80, tex.Height / 2);
				}
				DrawData item2 = new(tex, new Vector2((int)(drawInfo.ItemLocation.X - Main.screenPosition.X), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y + vector7.Y)), new Rectangle(0, 0, tex.Width, tex.Height), drawInfo.colorArmorBody, P.itemRotation, origin4, I.scale, drawInfo.itemEffect, 0);
				item2.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(item2);
			}
		}
	}
	// Idle
	public class GunLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HandOnAcc);
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) =>
			drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Tools.NovaLaserDrill>() ||
			drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Weapons.ArmCannon>();
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player P = drawInfo.drawPlayer;
			MPlayer mPlayer = P.GetModPlayer<MPlayer>();
			Item I = P.inventory[P.selectedItem];
			int frame = P.bodyFrame.Y / P.bodyFrame.Height;
			if ((I.type == ModContent.ItemType<Content.Items.Weapons.ArmCannon>() || I.type == ModContent.ItemType<Content.Items.Tools.NovaLaserDrill>()) && ((P.itemAnimation == 0 && (frame < 1 || frame > 4)) || (mPlayer.statCharge > 0 && mPlayer.somersault)) && !P.dead)
			{
				Texture2D tex = Terraria.GameContent.TextureAssets.Item[I.type].Value;//Main.itemTexture[I.type];
				MGlobalItem mi = I.GetGlobalItem<MGlobalItem>();
				if (mi.itemTexture != null)
				{
					tex = mi.itemTexture;
				}

				if (tex != null)
				{
					Vector2 origin = new(14f, tex.Height / 2);
					if (P.direction == -1)
					{
						origin.X = tex.Width - 14;
					}
					Vector2 pos = new(0f, 0f);
					float rot = 0f;
					float rotate = 0f;
					float posX = 0f;
					float posY = 0f;
					switch (frame)
					{
						case 0:
							rotate = 1.3625f;
							posX = -7f;
							posY = 11f;
							break;

						case 5:
							rotate = -1.75f;
							posX = -8f;
							posY = -13f;
							break;

						case 6 or 18 or 19 or (>= 11 and <= 13):
							posX = 0f;
							posY = 5f;
							break;

						case >= 7 and <= 9:
							posX = -2f;
							posY = 3f;
							break;

						case 10:
							posX = -2f;
							posY = 5f;
							break;

						case 14:
							posX = 2f;
							posY = 3f;
							break;

						case 15 or 16:
							posX = 4f;
							posY = 3f;
							break;

						case 17:
							posX = 2f;
							posY = 5f;
							break;

					}
					rot = rotate * P.direction * P.gravDir;
					pos.X += (P.bodyFrame.Width * 0.5f) + (posX * P.direction);
					pos.Y += (P.bodyFrame.Height * 0.5f) + 4f + (posY * P.gravDir);

					SpriteEffects effects = SpriteEffects.None;
					if (P.direction == -1)
					{
						effects = SpriteEffects.FlipHorizontally;
					}
					if (P.gravDir == -1f)
					{
						effects |= SpriteEffects.FlipVertically;
						pos.Y -= 2;
					}
					Color color = Lighting.GetColor((int)(drawInfo.Position.X + (P.width * 0.5)) / 16, (int)(drawInfo.Position.Y + (P.height * 0.5)) / 16);

					DrawData item = new(tex, new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (P.bodyFrame.Width / 2) + (P.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + P.height - P.bodyFrame.Height + 4f)) + new Vector2((int)pos.X, (int)pos.Y), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), drawInfo.colorArmorBody, rot, origin, I.scale, effects, 0);
					item.shader = drawInfo.cBody;
					drawInfo.DrawDataCache.Add(item);
				}
			}
		}
	}
}
