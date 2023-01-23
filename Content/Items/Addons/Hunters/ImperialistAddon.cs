using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.Addons.Hunters
{
	public class ImperialistAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperialist");
			Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Charge\n" +
				string.Format("[c/78BE78:+200% damage]\n") +
				string.Format("[c/BE7878:+500% overheat use]\n") +
				string.Format("[c/BE7878:Massive speed reduction]\n"));

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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.ImperialistTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.addonDmg = 3f;
			mItem.addonHeat = 5f;
			mItem.addonSpeed = -.25f;
		}


        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<Miscellaneous.ChoziteBar>(30)
                .AddIngredient<Miscellaneous.EnergyShard>(30)
                .AddIngredient(ItemID.ZapinatorGray, 1)
                .AddIngredient(ItemID.SoulofFright, 20)
                .AddIngredient(ItemID.Ruby, 20)
                .AddTile(TileID.Hellforge)
                .Register();
			
			CreateRecipe(1)
                .AddIngredient<Miscellaneous.ChoziteBar>(30)
                .AddIngredient<Miscellaneous.EnergyShard>(30)
                .AddIngredient(ItemID.ZapinatorOrange, 1)
                .AddIngredient(ItemID.SoulofFright, 20)
                .AddIngredient(ItemID.Ruby, 20)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}
