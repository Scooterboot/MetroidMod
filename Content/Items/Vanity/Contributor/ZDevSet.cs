using MetroidMod.Common.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity.Contributor
{
	[AutoloadEquip(EquipType.Head)]
	public class ZDevHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			if (!Main.dedServ)
			{
				ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
			}
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(0, 25);
			Item.vanity = true;
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class ZDevArmour : ModItem
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

	[AutoloadEquip(EquipType.Legs)]
	public class ZDevLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(0, 25);
			Item.vanity = true;
		}
	}

	[AutoloadEquip(EquipType.Wings)]
	public class ZDevWings : ModItem
	{
		public override void SetStaticDefaults()
		{
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(150, 7f, 1f, true, 10f, 10f);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(0, 25);
			Item.accessory = true;
		}
		public override void UpdateVisibleAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<MPlayer>().wingsGlowmaskTex = Item.ModItem.Texture + "_Wings_Glow";
		}
		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			if (player.TryingToHoverDown)
			{
				if (player.velocity.Y != 0)
				{
					player.velocity.Y *= 0.9f;
					if (player.velocity.Y > -2f && player.velocity.Y < 1f)
					{
						player.velocity.Y = 1E-05f;
						ascentWhenRising = 0;
						ascentWhenFalling = 0;
						constantAscend = 0;
					}
				}

				if (!player.controlLeft && !player.controlRight)
				{
					player.wingTime += 0.5f;
				}
			}
		}
	}

	//[AutoloadEquip(EquipType.Waist)]
	public class ZDevTail : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(0, 25);
			Item.vanity = true;
			Item.accessory = true;
		}
		public override void UpdateVisibleAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<MPlayer>().largeTailTex = Item.ModItem.Texture + "_Waist";
			player.GetModPlayer<MPlayer>().largeTailGlowmaskTex = Item.ModItem.Texture + "_Waist_Glow";
		}
	}
}
