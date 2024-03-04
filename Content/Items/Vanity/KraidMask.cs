using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class KraidMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Kraid Mask");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.value = 0;
			Item.vanity = true;
		}
	}
}
