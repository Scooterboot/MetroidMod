using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles
{
	/// <summary>
	/// A standard for generating and interacting with <see cref="Terraria.ModLoader.ModItem">s.
	/// </summary>
	public interface IGeneratesModTile
	{
		GeneratedModTile GeneratedModTile { get; }

		int TileType { get; }

		string TileTexture { get; }

		void TileSetStaticDefaults(GeneratedModTile generatedModTile) { }

		void TileAnimateTile(GeneratedModTile generatedModTile, ref int frame, ref int frameCounter) { }
		
		bool TileCanKillTile(GeneratedModTile generatedModTile, int i, int j, ref bool blockDamaged) { return true; }

		void TileMouseOver(GeneratedModTile generatedModTile, int i, int j) { }

		void TileNumDust(GeneratedModTile generatedModTile, int i, int j, bool fail, ref int num) { }

		bool TilePreDraw(GeneratedModTile generatedModTile, int i, int j, SpriteBatch spriteBatch) { return true; }

		bool TileSlope(GeneratedModTile generatedModTile, int i, int j) { return true; }

		bool TileRightClick(GeneratedModTile generatedModTile, int i, int j);
	}

	/// <summary>
	/// An automatically generated ModItem. See <see cref="IGeneratesModTile"/>.
	/// </summary>
	[Autoload(false)]
	public class GeneratedModTile : ModTile
	{
		public IGeneratesModTile producer;

		public override string Texture => producer.TileTexture;

		public GeneratedModTile(IGeneratesModTile producer)
		{
			this.producer = producer;
		}

		public override void SetStaticDefaults()
		{
			producer.TileSetStaticDefaults(this);
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			producer.TileAnimateTile(this, ref frame, ref frameCounter);
		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return producer.TileCanKillTile(this, i, j, ref blockDamaged);
		}

		public override void MouseOver(int i, int j)
		{
			producer.TileMouseOver(this, i, j);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			producer.TileNumDust(this, i, j, fail, ref num);
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			return producer.TilePreDraw(this, i, j, spriteBatch);
		}

		public override bool RightClick(int i, int j)
		{
			return producer.TileRightClick(this, i, j);
		}

		public override bool Slope(int i, int j)
		{
			return producer.TileSlope(this, i, j);
		}
	}
}
