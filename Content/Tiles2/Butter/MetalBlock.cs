using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
    internal class MetalBlock : GenericTile
    {
        public override Color MapColor => new(95, 107, 122); //updated
        public override SoundStyle HitSound => SoundID.Tink;
        public override int DustType => DustID.Stone;
		public override void AddRecipes()
		{

			Item.CreateRecipe(20)
				.AddIngredient(ItemID.IronBar, 1)
				.AddIngredient(ItemID.StoneBlock, 20)
				.AddTile(TileID.WorkBenches)
				.Register();

			Item.CreateRecipe(1)
				.AddIngredient(Mod, "MetalPlate", 1)
				.Register();

			Item.CreateRecipe(1)
				.AddIngredient(Mod, "MetalPipe", 1)
				.Register();

			Item.CreateRecipe(1)
				.AddIngredient(Mod, "MetalBrick", 1)
				.Register();

			Item.CreateRecipe(1)
				.AddIngredient(Mod, "MetalPillar", 1)
				.Register();
		}
	}
}

