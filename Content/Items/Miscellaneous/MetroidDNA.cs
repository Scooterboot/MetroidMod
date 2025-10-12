using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class MetroidAlphaDNA : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 10;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 34;
			Item.height = 32;
			Item.value = 1000;
			Item.rare = ItemRarityID.LightRed;
		}
	}

	public class MetroidGammaDNA : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 10;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 34;
			Item.height = 32;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;
		}
	}

	public class MetroidZetaDNA : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 10;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 34;
			Item.height = 32;
			Item.value = 100000;
			Item.rare = ItemRarityID.LightRed;
		}
	}
	
	public class MetroidOmegaDNA : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 10;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 34;
			Item.height = 32;
			Item.value = 1000000;
			Item.rare = ItemRarityID.LightRed;
		}
	}
}
