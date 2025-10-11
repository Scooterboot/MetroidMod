using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Default
{
	[Autoload(false)]

	///<summary>
	///The template off of which <see cref="ModBeamAddon"/>s generate their item tiles.
	///</summary>
	internal class BeamAddonTile(ModBeamAddon modBeamAddon) : ModTile
	{
		//This is essentially just a dummy tile file that allows for the built-in ModTiles to be defined directly within the main class.

		/// <summary>
		/// The <see cref="ModBeamAddon"/> this template is generating for.
		/// </summary>
		public ModBeamAddon modBeamAddon = modBeamAddon;

		public override string Texture => modBeamAddon.TileTexture;
		public override string Name => modBeamAddon.Name + "Tile";


		public override void SetStaticDefaults()
		{
			modBeamAddon.TileType = Type;
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

		//All the basic consistencies between addon tiles are pre-defined.
		//For everything else, all that needs to be done is to just connect the methods to their corresponding ModBeamAddon method.
		//Square peg, square hole. Simple as that.		-Z
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch) => modBeamAddon.PostDrawTile(i, j, spriteBatch);

		public override void PostDrawPlacementPreview(int i, int j, SpriteBatch spriteBatch, Rectangle frame, Vector2 position, Color color, bool validPlacement, SpriteEffects spriteEffects) => modBeamAddon.PostDrawPlacementPreview(i, j, spriteBatch, frame, position, color, validPlacement, spriteEffects);

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override bool Slope(int i, int j) => false;
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = modBeamAddon.ItemType;
		}
		public override bool RightClick(int i, int j)
		{
			if (!modBeamAddon.CanKillTile(i, j)) { return true; }
			WorldGen.KillTile(i, j, false, false, false);
			if (Main.netMode == NetmodeID.MultiplayerClient && !Main.tile[i, j].HasTile)
			{
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
			}
			return true;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!modBeamAddon.CanKillTile(i, j)) { fail = true; }
		}
	}
}
