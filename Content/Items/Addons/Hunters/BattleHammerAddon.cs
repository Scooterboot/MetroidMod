using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;
using MetroidMod.ID;

namespace MetroidMod.Content.Items.Addons.Hunters
{
	public class BattleHammerAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("BattleHammer");
			/* Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Charge\n" +
				"Shots explode\n" +
				string.Format("[c/BE7878:+100% overheat use]\n") +
				string.Format("[c/BE7878:-40% speed]\n") +
                string.Format("[c/BE7878:Cannot pierce walls or enemies]")); */
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<VoltDriverAddon>();
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 50000;
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
			mItem.beamSlotType = BeamChangeSlotID.BattleHammer;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageBattleHammer;
			mItem.addonHeat = Common.Configs.MConfigItems.Instance.overheatBattleHammer;
		}


        public override void AddRecipes()
        {
			CreateRecipe(1)
				.AddRecipeGroup(MetroidMod.T1HMBarRecipeGroupID, 8)
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
