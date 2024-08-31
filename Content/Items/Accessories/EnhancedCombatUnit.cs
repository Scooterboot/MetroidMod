using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Content.Items.Addons;
using MetroidMod.Common.Players;
using Terraria.Localization;

namespace MetroidMod.Content.Items.Accessories
{
	public class EnhancedCombatUnit : ModItem
	{
		//All the important numbers changes need to be up here so the dynamic localization thing can access them.
		//They're written as the percent changes so it's easier for the thing to read them
		//On the plus side it'll make changing stats easier!   -Z
		public static float huntDamage = 10f; //percent increase to hunter damage
		public static int huntCrit = 10; //percent increase to hunter crit chance
		public static float overheatDown = 15f; //percent decrease to overheat cost
		public static int tankCount = 4; //amount of reserve heart tanks provided

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(overheatDown, huntDamage, tankCount);

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Supercooled Plasma Core");
			// Tooltip.SetDefault("'Strange energy core capable of producing supercooled plasma'");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 32;
			Item.height = 44;
			Item.value = 1000;
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}
		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			if (incomingItem.type == ModContent.ItemType<SupercooledEmblem>()|| incomingItem.type == ModContent.ItemType<HunterEmblem>() || incomingItem.type == ModContent.ItemType<FrozenCore>()) 
			{
				return false;
			}
			return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.overheatCost *= 1f - (overheatDown / 100); //formula to convert overheatDown to the right percentage value
			mp.UACost *= 0.85f;
			Common.Players.HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100; //formula to convert huntDamage to a percent value
			Common.Players.HunterDamagePlayer.ModPlayer(player).HunterCrit += huntCrit; //no formula needed here so neato
			mp.reserveTanks = tankCount;
			mp.reserveHeartsValue = 25;

			//mp.statOverheat -= 0.1f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<SupercooledEmblem>(1)
				.AddIngredient<ReserveTank5>(1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
