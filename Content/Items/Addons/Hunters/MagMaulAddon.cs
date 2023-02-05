using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.Addons.Hunters
{
	public class MagMaulAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("MagMaul");
			Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Charge\n" +
				"Shots bounce and roll\n" +
				"Charge shots explode and ignite\n" +
				string.Format("[c/78BE78:+10% damage]\n") +
				string.Format("[c/BE7878:-45% speed]\n") +
                string.Format("[c/BE7878:Incompatible with wave, ice, or green plasma effects]"));

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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.MagMaulTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.addonDmg = 0.10f;
			mItem.addonHeat = 1.25f;
			mItem.addonSpeed = -.45f;
		}


        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<Miscellaneous.ChoziteBar>(30)
                .AddIngredient<Miscellaneous.EnergyShard>(30)
                .AddIngredient(ItemID.HellstoneBar, 30)
                .AddIngredient(ItemID.FallenStar, 30)
                .AddIngredient(ItemID.Amber, 30)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}
