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
			/* this isn't animated.
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 4));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			*/
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
			// TODO: Add original UA Pickup noise (currently uses MissilePickup noise
			Terraria.Audio.SoundEngine.PlaySound(Sounds.Suit.MissilePickup/*UAPickup*/, player.position);
			CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.White, Item.stack, false, false);
			return false;
		}
	}
}
