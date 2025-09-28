using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Content.Tiles
{
	public class PhantoonTrophyTile : ModTile
	{
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
