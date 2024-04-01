using MetroidMod.Common.GlobalItems;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class UAPickup : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("UA Pickup");
			ItemID.Sets.ItemNoGravity[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 6));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;	
			ItemID.Sets.IgnoresEncumberingStone[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 200;
			Item.width = 20;
			Item.height = 26;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
		}
		public override bool ItemSpace(Player player) => true;
		public override bool OnPickup(Player player)
		{
			for (int i = 0; i < player.inventory.Length; i++)
			{
				if (player.inventory[i].type == ModContent.ItemType<Weapons.PowerBeam>())
				{
					MGlobalItem mi = player.inventory[i].GetGlobalItem<MGlobalItem>();
					mi.statUA += Item.stack;
				}
			}

			Terraria.Audio.SoundEngine.PlaySound(Sounds.Suit.UAPickup, player.position);
			CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.Green, Item.stack, false, false);
			return false;
		}
	}
}
