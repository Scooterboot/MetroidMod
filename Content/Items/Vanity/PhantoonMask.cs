using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class PhantoonMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phantoon Mask");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = 0;
			Item.vanity = true;
		}
	}
}
