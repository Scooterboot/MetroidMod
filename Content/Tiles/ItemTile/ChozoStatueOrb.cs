using System.Collections.Generic;
using MetroidMod.Common.Systems;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
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
			TileObjectData.newTile.AnchorBottom = new AnchorData(Terraria.Enums.AnchorType.None, TileObjectData.newTile.Width, 0);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(90, 90, 90), name);
			Main.tileOreFinderPriority[Type] = 807;
			DustType = 1;
			AnimationFrameHeight = 18;
			Main.tileLavaDeath[Type] = false;
			Main.tileObsidianKill[Type] = false;
			TileID.Sets.FriendlyFairyCanLureTo[Type] = true;
		}
		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				yield return new Item(MSystem.OORB()); //oorb1
			}
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
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.tile[i, j].TileType = (ushort)MSystem.OrbItem(); //(ushort)MSystem.OORBItem1();
			}
			else if (Main.netMode != NetmodeID.SinglePlayer)
			{
				Main.tile[i, j].ClearTile();
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
				NetMessage.SendTileSquare(-1, i, j);
			}
			return true;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (Main.netMode != NetmodeID.SinglePlayer/*&& !Main.tile[i, j].HasTile*/)
			{
				if(!Main.tile[i, j].HasTile)
				{
					fail = false;
					noItem = false;
					NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
					NetMessage.SendTileSquare(-1, i, j);
				}
			}
			else
			{
				fail = true;
				noItem = true;
				Main.tile[i, j].TileType = (ushort)MSystem.OrbItem(); //(ushort)MSystem.OORBItem1();
			}
		}
	}	
	public class ChozoStatueOrb2 : ChozoStatueOrb
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}
		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				yield return new Item(MSystem.OORB()); //OORB2
			}
		}
		public override bool RightClick(int i, int j)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.tile[i, j].TileType = (ushort)MSystem.OrbItem();//(ushort)MSystem.OORBItem2();
			}
			else if (Main.netMode != NetmodeID.SinglePlayer)
			{
				Main.tile[i, j].ClearTile();
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
				NetMessage.SendTileSquare(-1, i, j);
			}
			return true;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (Main.netMode != NetmodeID.SinglePlayer/*&& !Main.tile[i, j].HasTile*/)
			{
				if (!Main.tile[i, j].HasTile)
				{
					fail = false;
					noItem = false;
					NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
					NetMessage.SendTileSquare(-1, i, j);
				}
			}
			else
			{
				fail = true;
				noItem = true;
				Main.tile[i, j].TileType = (ushort)MSystem.OrbItem(); //oorbitem2
			}
		}
	}	
	public class ChozoStatueOrb3 : ChozoStatueOrb
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			AnimationFrameHeight = 0;
		}
		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				yield return new Item(MSystem.OORB()); //oorb3
			}
		}
		public override bool RightClick(int i, int j)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.tile[i, j].TileType = (ushort)MSystem.OrbItem();//3
			}
			else if (Main.netMode != NetmodeID.SinglePlayer)
			{
				Main.tile[i, j].ClearTile();
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
				NetMessage.SendTileSquare(-1, i, j);
			}
			return true;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (Main.netMode != NetmodeID.SinglePlayer/*&& !Main.tile[i, j].HasTile*/)
			{
				if (!Main.tile[i, j].HasTile)
				{
					fail = false;
					noItem = false;
					NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
					NetMessage.SendTileSquare(-1, i, j);
				}
			}
			else
			{
				fail = true;
				noItem = true;
				Main.tile[i, j].TileType = (ushort)MSystem.OrbItem(); //oorbitem3
			}
		}
	}
}
