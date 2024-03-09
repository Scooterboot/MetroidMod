using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ObjectData;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public class ChozoStatueOrb : ItemTile
	{
		public override void SetStaticDefaults()
		{
			//base.SetStaticDefaults();
			TileObjectData.newTile.LavaDeath = false;
			Main.tileSpelunker[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(90, 90, 90), name);
			Main.tileOreFinderPriority[Type] = 807;
			DustType = 1;
			AnimationFrameHeight = 18;
			Main.tileLavaDeath[Type] = false;
			Main.tileObsidianKill[Type] = false;
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 4)
			{
				frame++;
				if (frame > 3)
					frame = 0;
				frameCounter = 0;
			}
		}
		public override bool RightClick(int i, int j)
		{
			WorldGen.KillTile(i, j, false, false, true);
			if ((Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server) && !Main.tile[i, j].HasTile)
			{
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
			}
			return true;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!fail && !effectOnly)
			{
				noItem = true;
				base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
				/*if ((Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server) && !Main.tile[i, j].HasTile)
				{
					NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
				}*/
				Main.tile[i, j].TileType = (ushort)Common.Systems.MSystem.OrbItem(i, j);
				fail = true;
				//Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, Common.Systems.MSystem.OrbItem(i, j));
			}
		}
	}
}
