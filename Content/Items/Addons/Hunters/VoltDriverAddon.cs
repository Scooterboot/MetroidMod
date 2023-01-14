using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.Addons.Hunters
{
	public class VoltDriverAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("VoltDriver");
			Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Charge\n" +
				"Charge shots home, explode, and confuse\n" +
				string.Format("[c/78BE78:+15% damage]\n") +
                string.Format("[c/BE7878:-10% speed]\n") +
                string.Format("[c/BE7878:+100% overheat use]\n") +
				string.Format("[c/BE7878:+10% noise]\n") +
				string.Format("[c/BE7878:Incompatible with ice, wave, or plasma effects]"));

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 2500;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.VoltDriverTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.addonDmg = .15f;
			mItem.addonHeat = 1f;
            mItem.addonSpeed = -.1f;
        }
	

		public override void AddRecipes()
		{
			CreateRecipe(1)
                .AddIngredient<Miscellaneous.ChoziteBar>(30)
                .AddIngredient<Miscellaneous.EnergyShard>(30)
                .AddIngredient(ItemID.CelestialMagnet, 1)
                .AddIngredient(ItemID.Topaz, 30)
                .AddIngredient(ItemID.Wire, 100)
                .AddTile(TileID.Anvils)
				.Register();
        }
	}
}
