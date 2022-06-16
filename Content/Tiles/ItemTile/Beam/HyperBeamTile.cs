using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Beam
{
	public class HyperBeamTile : ItemTile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hyper Beam");
			AddMapEntry(new Color(247, 0, 98), name);
			ItemDrop = ModContent.ItemType<Items.Addons.HyperBeamAddon>();
			DustType = 1;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			spriteBatch.Draw(ModContent.Request<Texture2D>("MetroidMod/Content/Tiles/ItemTile/Beam/HyperBeamTileColors").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}
}
