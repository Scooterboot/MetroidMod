using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Content.SuitAddons;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.Enums;
using Terraria;

namespace MetroidMod.Default
{
	[Autoload(false)]
	internal class MissileAddonTile : ModTile
	{
		public ModMissileAddon modMissileAddon;

		public override string Texture => modMissileAddon.TileTexture;
		public override string Name => modMissileAddon.Name + "Tile";

		public MissileAddonTile(ModMissileAddon modMissileAddon)
		{
			this.modMissileAddon = modMissileAddon;
		}

		public override void SetStaticDefaults()
		{
			modMissileAddon.TileType = Type;
			//ItemDrop= modSuitAddon.ItemType;
			Main.tileFrameImportant[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileOreFinderPriority[Type] = 807;
			Main.tileNoAttach[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(255, 126, 255), name);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			TileID.Sets.DisableSmartCursor[Type] = true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override bool Slope(int i, int j) => false;
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = modMissileAddon.ItemType;
		}
		public override bool RightClick(int i, int j)
		{
			if (!modMissileAddon.CanKillTile(i, j)) { return true; }
			WorldGen.KillTile(i, j, false, false, false);
			if (Main.netMode == NetmodeID.MultiplayerClient && !Main.tile[i, j].HasTile)
			{
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
			}
			return true;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!modMissileAddon.CanKillTile(i, j)) { fail = true; }
		}
	}
}
