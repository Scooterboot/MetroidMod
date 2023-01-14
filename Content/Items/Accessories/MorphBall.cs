using System;
using System.IO;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Accessories
{
	public class MorphBall : ModItem
	{
		private Item[] _ballMods;
		public Item[] ballMods
		{
			get
			{
				if (_ballMods == null)
				{
					_ballMods = new Item[MetroidMod.ballSlotAmount];
					for (int i = 0; i < _ballMods.Length; ++i)
					{
						_ballMods[i] = new Item();
						_ballMods[i].TurnToAir();
					}
				}

				return _ballMods;
			}
			set { _ballMods = value; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball");
			Tooltip.SetDefault("Equip and press the mount key to roll into a ball\n" + 
			"Morph Ball's colors are based on your shirt and undershirt colors");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 1;
			Item.value = 40000;
			Item.rare = ItemRarityID.Green;
			//Item.accessory = true;
			Item.mountType = ModContent.MountType<Mounts.MorphBallMount>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(12)
				.AddRecipeGroup(MetroidMod.GoldPlatinumBarRecipeGroupID, 5)
				.AddIngredient(ItemID.FallenStar, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override bool CanUseItem(Player player)
		{
			return Item == player.miscEquips[3];
		}

		public override ModItem Clone(Item item)
		{
			ModItem clone = base.Clone(item);
			MorphBall ballClone = (MorphBall)clone;
			ballClone.ballMods = new Item[MetroidMod.ballSlotAmount];
			for (int i = 0; i < MetroidMod.ballSlotAmount; ++i)
			{
				ballClone.ballMods[i] = this.ballMods[i];
			}

			return clone;
		}

		public override void SaveData(TagCompound tag)
		{
			//TagCompound tag = new TagCompound();
			for(int i = 0; i < ballMods.Length; ++i)
			{
				tag.Add("ballItem" + i, ItemIO.Save(ballMods[i]));
			}
			//return tag;
		}
		public override void LoadData(TagCompound tag)
		{
			try
			{
				ballMods = new Item[MetroidMod.ballSlotAmount];
				for (int i = 0; i< ballMods.Length; ++i)
				{
					Item item = tag.Get<Item>("ballItem" + i);
					ballMods[i] = item;
				}
			}
			catch{}
		}
		
		public override void NetSend(BinaryWriter writer)
		{
			for (int i = 0; i < ballMods.Length; ++i)
			{
				ItemIO.Send(ballMods[i], writer);
			}
		}
		public override void NetReceive(BinaryReader reader)
		{
			for (int i = 0; i < ballMods.Length; ++i)
			{
				ballMods[i] = ItemIO.Receive(reader);
			}
		}
	}
}
