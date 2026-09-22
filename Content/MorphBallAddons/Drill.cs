using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class Drill : ModMorphBallAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/Drill/DrillItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/Drill/DrillTile";

		public override MorphBallAddonSlot AddonSlot => MorphBallAddonSlot.Drill;

		public override void ItemSetDefaults()
		{
			Item.value = Item.buyPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Blue;
		}

		public override void UpdateEquip(Terraria.Player player)
		{
			player.GetModPlayer<Common.Players.MPlayer>().Drill(player);
		}

		public override void ItemAddRecipes()
		{
			GeneratedModItem.CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(12)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
