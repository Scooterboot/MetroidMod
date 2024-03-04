using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;
using MetroidMod.ID;

namespace MetroidMod.Content.Items.Addons.V3
{
	public class StardustBeamAddon : ModItem, IBeamAddon
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Beam");
			/* Tooltip.SetDefault(string.Format("[c/FF9696:Power Beam Addon V3]\n") +
				"Slot Type: Secondary\n" +
				"Shots freeze enemies\n" + 
				"~Each time the enemy is shot, they will become 20% slower\n" + 
				"~After 5 shots the enemy will become completely frozen\n" + 
				string.Format("[c/78BE78:+260% damage]\n") +
				string.Format("[c/BE7878:+50% overheat use]\n") +
				string.Format("[c/BE7878:-30% speed]")); */

			Item.ResearchUnlockCount = 1;
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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.StardustBeamTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
   			this.beamID = BeamID.Beam.Stardust;
      			this.slotType = BeamID.SlotType.Secondary;
			this.ver = 3;
    			this.itemID = ModContent.ItemType<StardustBeamAddon>();
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageStardustBeam;
			mItem.addonHeat = Common.Configs.MConfigItems.Instance.overheatStardustBeam;
			mItem.addonSpeed = Common.Configs.MConfigItems.Instance.speedStardustBeam;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.FragmentStardust, 18)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
