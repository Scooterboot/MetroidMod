using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;


namespace MetroidMod.Content.Tiles2.Butter
{
	internal class MetalBrick : GenericTile
	{
		public override Color MapColor => new(95, 107, 122); //updated
		public override SoundStyle HitSound => SoundID.Tink;
		public override int DustType => DustID.Stone;
		public override void AddRecipes()
		{
			Item.CreateRecipe(1)
				.AddIngredient(Mod, "MetalBlock", 1)
				.Register();
		}
	}
}

