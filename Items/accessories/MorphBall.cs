using System;
using System.IO;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MetroidMod.Items.accessories
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
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 1;
			item.value = 40000;
			item.rare = 2;
			//item.accessory = true;
			item.mountType = mod.MountType("MorphBallMount");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 20);
			recipe.AddIngredient(ItemID.Diamond, 3);
			recipe.AddIngredient(ItemID.GoldBar, 5);
			recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 20);
			recipe.AddIngredient(ItemID.Diamond, 3);
			recipe.AddIngredient(ItemID.PlatinumBar, 5);
			recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool CanUseItem(Player player)
		{
			return (item == player.miscEquips[3]);
		}

		public override ModItem Clone(Item item)
		{
			ModItem clone = this.NewInstance(item);
			MorphBall ballClone = (MorphBall)clone;
			ballClone.ballMods = new Item[MetroidMod.ballSlotAmount];
			for (int i = 0; i < MetroidMod.ballSlotAmount; ++i)
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
			Texture2D tex = mod.GetTexture("Items/accessories/MorphBall_Lights");
			sb.Draw(tex, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public override TagCompound Save()
		{
            TagCompound tag = new TagCompound();
            for(int i = 0; i < ballMods.Length; ++i)
            {
                tag.Add("ballItem" + i, ItemIO.Save(ballMods[i]));
            }
			return tag;
		}
		public override void Load(TagCompound tag)
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

		public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI )
		{
			DrawLights(sb, Main.player[Main.myPlayer]);
			MPlayer mp = Main.player[Main.myPlayer].GetModPlayer<MPlayer>();
			lightColor = mp.morphColorLights;
			alphaColor = lightColor;
		}
		public void DrawLights(SpriteBatch sb, Player player)
		{
			Texture2D tex = mod.GetTexture("Items/accessories/MorphBall_Lights");
			float rotation = item.velocity.X * 0.2f;
			float num3 = 1f;
			float num4 = (float)(item.height - tex.Height);
			float num5 = (float)(item.width / 2 - tex.Width / 2);
			MPlayer mp = player.GetModPlayer<MPlayer>();
			sb.Draw(tex, new Vector2(item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num5, item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num4 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), mp.morphColorLights, rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), num3, SpriteEffects.None, 0f);
		}
        
        public override void NetSend(BinaryWriter writer)
        {
            for (int i = 0; i < ballMods.Length; ++i)
            {
                writer.WriteItem(ballMods[i]);
            }
        }
        public override void NetRecieve(BinaryReader reader)
        {
            for (int i = 0; i < ballMods.Length; ++i)
            {
                ballMods[i] = reader.ReadItem();
            }
        }
    }
}
