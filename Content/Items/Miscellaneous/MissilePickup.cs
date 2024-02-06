using MetroidMod.Common.GlobalItems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class MissilePickup : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Missile");
			ItemID.Sets.ItemNoGravity[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 4));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.IgnoresEncumberingStone[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 255;
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
				if (player.inventory[i].type == ModContent.ItemType<Weapons.MissileLauncher>())
				{
					MGlobalItem mi = player.inventory[i].GetGlobalItem<MGlobalItem>();
					mi.statMissiles += Item.stack;
				}
			}
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, player.position);
			CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.White, Item.stack, false, false);
			return false;
		}
	}
}
