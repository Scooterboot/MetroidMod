using MetroidMod.Common.Players;
using MetroidMod.Content.SuitAddons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Accessories
{
	//[AutoloadEquip(EquipType.Neck)]
	public class TorizoEyeNecklace : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 40000;
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.expertOnly = true;
			Item.expert = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.Eyed = true;
			//player.noKnockback = false;
		}
	}
}
