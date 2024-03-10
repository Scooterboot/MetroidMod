using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Content.Items.Addons;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Accessories
{
	public class FrozenCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Supercooled Plasma Core");
			// Tooltip.SetDefault("'Strange energy core capable of producing supercooled plasma'");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 18;
			Item.height = 16;
			Item.value = 1000;
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//There's almost definitely a better way to do this but heck if I know what it is
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.statOverheat -= 0.1f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SpectreBar, 8)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
