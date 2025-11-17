using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Content.Tiles
{
	public class PhantoonTrophyTile : ModTile
	{
		public static Asset<Texture2D> pupilTexture => ModContent.Request<Texture2D>("MetroidMod/Content/Tiles/PhantoonTrophyTile_Eye");
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.StyleWrapLimit = 9;
			AnimationFrameHeight = TileObjectData.newTile.CoordinateFullHeight;
			TileObjectData.addTile(Type);
			DustType = 7;
			TileID.Sets.DisableSmartCursor[Type] = true;//disableSmartCursor = true;
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Phantoon Trophy");
			AddMapEntry(new Color(149, 133, 77), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			new EntitySource_TileBreak(i, j); //Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<Items.Tiles.PhantoonTrophy>());
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX > 18 && tile.TileFrameY > 18)
			{
				i--;
				j--;
			}
			else
			{
				return;
			}
			if (!TileDrawing.IsVisible(Main.tile[i, j])) { return; } //Don't do any of this if the tile's invisible (i.e. echo paint)

			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 eyeDistance = new Vector2(Main.LocalPlayer.Center.X - (i * 16), Main.LocalPlayer.Center.Y - (j * 16));
			Vector2 eyeAim = Vector2.Zero;

			if (eyeDistance.Length() > 450)
			{
				int c = 50;
				if (eyeDistance.X < -c)
				{
					eyeAim.X = -2;
				}
				if (eyeDistance.X > c)
				{
					eyeAim.X = 2;
				}
				if (eyeDistance.Y < -c)
				{
					eyeAim.Y = -2;
				}
				if (eyeDistance.Y > c)
				{
					eyeAim.Y = 2;
				}
			}
			Rectangle pupilRectangle = pupilTexture.Frame();
			Vector2 offset = new Vector2(2, -4) + eyeAim;
			spriteBatch.Draw(pupilTexture.Value, new Vector2((i * 16) - (int)Main.screenPosition.X, (j * 16) - (int)Main.screenPosition.Y) + zero + offset, pupilRectangle, Color.White);

		}

		//public override void AnimateTile(ref int frame, ref int frameCounter)
		//{
		//	float targetRot = (float)Math.Atan2(Main.LocalPlayer.Center.Y - (Main.tile.Y + 22), Main.Main.LocalPlayer.Center.X - tile.X);
		//	if (targetRot >= (float)(Math.PI * 2))
		//	{
		//		targetRot -= (float)(Math.PI * 2);
		//	}
		//	if (targetRot < 0)
		//	{
		//		targetRot += (float)(Math.PI * 2);
		//	}
		//}
	}
}
