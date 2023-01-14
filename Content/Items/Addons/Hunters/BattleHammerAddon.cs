using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.Addons.Hunters
{
	public class BattleHammerAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BattleHammer");
			Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Charge\n" +
				"Shots explode\n" +
				string.Format("[c/78BE78:+25% damage]\n") +
				string.Format("[c/BE7878:+100% overheat use]\n") +
				string.Format("[c/BE7878:-50% speed]\n") +
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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.BattleHammerTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.addonDmg = .25f;
			mItem.addonHeat = 1.0f;
			mItem.addonSpeed = -0.5f;
		}


        public override void AddRecipes()
        {
			CreateRecipe(1)
                .AddIngredient<Miscellaneous.ChoziteBar>(30)
                .AddIngredient<Miscellaneous.EnergyShard>(30)
                .AddIngredient(ItemID.MeteoriteBar, 30)
                .AddIngredient(ItemID.Emerald, 30)
                .AddIngredient(ItemID.Grenade, 99)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}
