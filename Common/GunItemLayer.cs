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
			drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Weapons.PowerBeam>() ||
			drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Weapons.MissileLauncher>() ||
			drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Tools.NovaLaserDrill>();
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player P = drawInfo.drawPlayer;
			Item I = P.inventory[P.selectedItem];
			MPlayer mPlayer = P.GetModPlayer<MPlayer>();
			if (drawInfo.shadow != 0f || P.frozen || ((P.itemAnimation <= 0 || I.useStyle == 0) && (I.holdStyle <= 0 || P.pulley)) || I.type <= 0 || P.dead || I.noUseGraphic || (P.wet && I.noWet) || mPlayer.somersault)
			{
				return;
			}

			if (I.type == ModContent.ItemType<Content.Items.Weapons.PowerBeam>() || I.type == ModContent.ItemType<Content.Items.Weapons.MissileLauncher>() || I.type == ModContent.ItemType<Content.Items.Tools.NovaLaserDrill>())
			{
				Texture2D tex = Terraria.GameContent.TextureAssets.Item[I.type].Value;
				MGlobalItem mi = I.GetGlobalItem<MGlobalItem>();
				if (mi.itemTexture != null)
				{
					tex = mi.itemTexture;
				}
				Color currentColor = Lighting.GetColor((int)((double)drawInfo.Position.X + (double)P.width * 0.5) / 16, (int)(((double)drawInfo.Position.Y + (double)P.height * 0.5) / 16.0));

				int num80 = 10;
				Vector2 vector7 = new(tex.Width / 2, tex.Height / 2);
				Vector2 vector8 = new(24f / 2, tex.Height / 2);
				num80 = (int)vector8.X;
				vector7.Y = vector8.Y;
				Vector2 origin4 = new(-num80, tex.Height / 2);
				if (P.direction == -1)
				{
					origin4 = new Vector2(tex.Width + num80, tex.Height / 2);
				}
				DrawData item2 = new(tex, new Vector2((int)(drawInfo.ItemLocation.X - Main.screenPosition.X + vector7.X), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y + vector7.Y)), new Rectangle(0, 0, tex.Width, tex.Height), drawInfo.colorArmorBody, P.itemRotation, origin4, I.scale, drawInfo.itemEffect, 0);
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
			drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Weapons.PowerBeam>() ||
			drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Weapons.MissileLauncher>() ||
			drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Tools.NovaLaserDrill>();
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player P = drawInfo.drawPlayer;
			MPlayer mPlayer = P.GetModPlayer<MPlayer>();
			Item I = P.inventory[P.selectedItem];
			int frame = (int)(P.bodyFrame.Y / P.bodyFrame.Height);
			if ((I.type == ModContent.ItemType<Content.Items.Weapons.PowerBeam>() || I.type == ModContent.ItemType<Content.Items.Weapons.MissileLauncher>() || I.type == ModContent.ItemType<Content.Items.Tools.NovaLaserDrill>()) && ((P.itemAnimation == 0 && (frame < 1 || frame > 4)) || (mPlayer.statCharge > 0 && mPlayer.somersault)) && !P.dead)
			{
				Texture2D tex = Terraria.GameContent.TextureAssets.Item[I.type].Value;//Main.itemTexture[I.type];
				MGlobalItem mi = I.GetGlobalItem<MGlobalItem>();
				if (mi.itemTexture != null)
				{
					tex = mi.itemTexture;
				}

				if (tex != null)
				{
					Vector2 origin = new(14f, (float)((int)(tex.Height / 2)));
					if (P.direction == -1)
					{
						origin.X = tex.Width - 14;
					}
					Vector2 pos = new(0f, 0f);
					float rot = 0f;
					float rotate = 0f;
					float posX = 0f;
					float posY = 0f;
					if (frame == 0)
					{
						rotate = 1.3625f;
						posX = -7f;
						posY = 13f;
					}
					else if (frame == 5)
					{
						rotate = -1.75f;
						posX = -8f;
						posY = -12f;
					}
					else if (frame == 6 || frame == 18 || frame == 19 || (frame >= 11 && frame <= 13))
					{
						posX = 0f;
						posY = 5f;
					}
					else if (frame >= 7 && frame <= 9)
					{
						posX = -2f;
						posY = 3f;
					}
					else if (frame == 10)
					{
						posX = -2f;
						posY = 5f;
					}
					else if (frame == 14)
					{
						posX = 2f;
						posY = 3f;
					}
					else if (frame == 15 || frame == 16)
					{
						posX = 4f;
						posY = 3f;
					}
					else if (frame == 17)
					{
						posX = 2f;
						posY = 5f;
					}
					rot = rotate * P.direction * P.gravDir;
					pos.X += ((float)P.bodyFrame.Width * 0.5f) + posX * P.direction;
					pos.Y += ((float)P.bodyFrame.Height * 0.5f) + 4f + posY * P.gravDir;

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
					Color color = Lighting.GetColor((int)((double)drawInfo.Position.X + (double)P.width * 0.5) / 16, (int)((double)drawInfo.Position.Y + (double)P.height * 0.5) / 16);

					DrawData item = new(tex, new Vector2((float)((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(P.bodyFrame.Width / 2) + (float)(P.width / 2))), (float)((int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)P.height - (float)P.bodyFrame.Height + 4f))) + new Vector2((float)((int)pos.X), (float)((int)pos.Y)), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), drawInfo.colorArmorBody, rot, origin, I.scale, effects, 0);
					item.shader = drawInfo.cBody;
					drawInfo.DrawDataCache.Add(item);
				}
			}
		}
	}
}
