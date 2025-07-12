using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class CrateriaGrass : GenericTile
	{
		public override Color MapColor => new(48, 64, 24);
		public override SoundStyle HitSound => SoundID.Grass;
		public override int DustType => DustID.Grass;
		public override void AddRecipes()
		{
			Item.CreateRecipe(10)
				.AddIngredient(ItemID.DirtBlock, 10)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}

