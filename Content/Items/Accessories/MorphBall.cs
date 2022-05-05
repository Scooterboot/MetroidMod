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

using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.Items.Accessories
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
					_ballMods = new Item[MetroidModPorted.ballSlotAmount];
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
				.AddRecipeGroup(MetroidModPorted.GoldPlatinumBarRecipeGroupID, 5)
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
			ballClone.ballMods = new Item[MetroidModPorted.ballSlotAmount];
			for (int i = 0; i < MetroidModPorted.ballSlotAmount; ++i)
			{
				ballClone.ballMods[i] = this.ballMods[i];
			}

			return clone;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			Player player = Main.player[Main.myPlayer];
			MPlayer mp = player.GetModPlayer<MPlayer>();
			return mp.morphItemColor;
		}
		public override void PostDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale )
		{
			Player player = Main.player[Main.myPlayer];
			MPlayer mp = player.GetModPlayer<MPlayer>();
			itemColor = mp.morphItemColor;
			drawColor = mp.morphColorLights;
			Texture2D tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Items/Accessories/MorphBall_Lights").Value;
			sb.Draw(tex, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
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
				ballMods = new Item[MetroidModPorted.ballSlotAmount];
				for (int i = 0; i< ballMods.Length; ++i)
				{
					Item item = tag.Get<Item>("ballItem" + i);
					ballMods[i] = item;
				}
			}
			catch{}
		}

		public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI )
		{
			DrawLights(sb, Main.player[Main.myPlayer]);
			MPlayer mp = Main.player[Main.myPlayer].GetModPlayer<MPlayer>();
			lightColor = mp.morphColorLights;
			alphaColor = lightColor;
		}
		public void DrawLights(SpriteBatch sb, Player player)
		{
			Texture2D tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Items/Accessories/MorphBall_Lights").Value;
			float rotation = Item.velocity.X * 0.2f;
			float num3 = 1f;
			float num4 = (float)(Item.height - tex.Height);
			float num5 = (float)(Item.width / 2 - tex.Width / 2);
			MPlayer mp = player.GetModPlayer<MPlayer>();
			sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num5, Item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num4 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), mp.morphColorLights, rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), num3, SpriteEffects.None, 0f);
		}
		
		public override void NetSend(BinaryWriter writer)
		{
			for (int i = 0; i < ballMods.Length; ++i)
			{
				writer.WriteVarInt(ballMods[i].type);
			}
		}
		public override void NetReceive(BinaryReader reader)
		{
			for (int i = 0; i < ballMods.Length; ++i)
			{
				ballMods[i].type = reader.ReadInt32();//.CloneDefaults(reader.ReadInt32());
			}
		}
	}
}
