using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles
{
	/// <summary>
	/// A standard for generating and interacting with <see cref="Terraria.ModLoader.ModItem">s.
	/// </summary>
	public interface IGeneratesModTile
	{
		string Name { get; }

		GeneratedModTile GeneratedModTile { get; }

		int TileType { get; }

		string TileTexture { get; }

		void TileSetStaticDefaults() { }

		void TileAnimateTile(ref int frame, ref int frameCounter);
		
		bool TileCanKillTile(int i, int j, ref bool blockDamaged);

		bool TileCanExplode(int i, int j);

		void TileKillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem);

		void TileMouseOver(int i, int j);

		void TileNumDust(int i, int j, bool fail, ref int num);

		bool TilePreDraw(int i, int j, SpriteBatch spriteBatch);

		bool TileSlope(int i, int j);

		bool TileRightClick(int i, int j);
	}

	/// <summary>
	/// An automatically generated ModTile. See <see cref="IGeneratesModTile"/>.
	/// </summary>
	[Autoload(false)]
	public class GeneratedModTile : ModTile
	{
		public IGeneratesModTile producer;

		public override string Name => producer.Name + "Tile";

		public override string Texture => producer.TileTexture;

		public GeneratedModTile(IGeneratesModTile producer)
		{
			this.producer = producer;
		}

		public override void SetStaticDefaults()
		{
			producer.TileSetStaticDefaults();
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			producer.TileAnimateTile(ref frame, ref frameCounter);
		}

		public override bool CanExplode(int i, int j)
		{
			return producer.TileCanExplode(i, j);
		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return producer.TileCanKillTile(i, j, ref blockDamaged);
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			producer.TileKillTile(i, j, ref fail, ref effectOnly, ref noItem);
		}

		public override void MouseOver(int i, int j)
		{
			producer.TileMouseOver(i, j);
		}


		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			producer.TileNumDust(i, j, fail, ref num);
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			return producer.TilePreDraw(i, j, spriteBatch);
		}

		public override bool RightClick(int i, int j)
		{
			return producer.TileRightClick(i, j);
		}

		public override bool Slope(int i, int j)
		{
			return producer.TileSlope(i, j);
		}
	}
}
