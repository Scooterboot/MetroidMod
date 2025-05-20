using MetroidMod.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.HandsOff)]
	public class GrappleLasso : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			Item.vanity = true;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(2)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
