using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Content.Items.Addons;
using MetroidMod.Common.Players;
using Terraria.Localization;

namespace MetroidMod.Content.Items.Accessories
{
	[AutoloadEquip(EquipType.Back)]
	public class SupercooledEmblem : ModItem
	{
		//All the important numbers changes need to be up here so the dynamic localization thing can access them.
		//They're written as the percent changes so it's easier for the thing to read them
		//On the plus side it'll make changing stats easier!   -Z
		public static float huntDamage = 10f; //percent increase to hunter damage
		public static int huntCrit = 10; //percent increase to hunter crit chance
		public static float overheatDown = 15f; //percent decrease to overheat cost

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(overheatDown, huntDamage, huntCrit);
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Thermal Regulator");
			// Tooltip.SetDefault("'I mean it's not really an emblem anymore y'know'");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 32;
			Item.height = 44;
			Item.value = 1000;

			Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}
		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			if (incomingItem.type == ModContent.ItemType<HunterEmblem>() || incomingItem.type == ModContent.ItemType<FrozenCore>() || incomingItem.type == ModContent.ItemType<EnhancedCombatUnit>())
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
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += huntDamage / 100; //formula to convert huntDamage to a percent value
			HunterDamagePlayer.ModPlayer(player).HunterCrit += huntCrit; //no formula needed here so neato
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<FrozenCore>(1)
				.AddIngredient(ItemID.DestroyerEmblem, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
