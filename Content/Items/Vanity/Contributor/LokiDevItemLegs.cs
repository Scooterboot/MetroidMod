using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity.Contributor
{
	[AutoloadEquip(EquipType.Legs)]
	public class LokiDevItemLegs : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(0, 25);
			Item.vanity = true;
		}
	}
}
