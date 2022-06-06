using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MetroidModPorted.Common.GlobalItems;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.Items.Miscellaneous
{
	public class EnergyPickup : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy");
			ItemID.Sets.ItemNoGravity[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(20, 3));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.IgnoresEncumberingStone[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 255;
			Item.width = 30;
			Item.height = 30;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
		}
		public override bool ItemSpace(Player player) => true;
		public override bool OnPickup(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.Energy += Item.stack;
			Terraria.Audio.SoundEngine.PlaySound(Sounds.Suit.EnergyPickup, player.position);
			CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.White, Item.stack, false, false);
			return false;
		}
	}
}
