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
	public class GunItemLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Mod mod = MetroidModPorted.Instance;
			Player P = drawInfo.drawPlayer;
			Item I = P.inventory[P.selectedItem];
			MPlayer mPlayer = P.GetModPlayer<MPlayer>();
			if (drawInfo.shadow != 0f || P.frozen || ((P.itemAnimation <= 0 || I.useStyle == 0) && (I.holdStyle <= 0 || P.pulley)) || I.type <= 0 || P.dead || I.noUseGraphic || (P.wet && I.noWet) || mPlayer.somersault)
			{
				return;
			}

			if (I.type == ModContent.ItemType<Content.Items.Weapons.PowerBeam>() || I.type == ModContent.ItemType<Content.Items.Weapons.MissileLauncher>() || I.type == ModContent.ItemType<Content.Items.Tools.NovaLaserDrill>())
			{
				Texture2D tex = Terraria.GameContent.TextureAssets.Item[I.type].Value;//Main.itemTexture[I.type];
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
}
