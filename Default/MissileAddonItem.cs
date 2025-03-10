using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using MetroidMod.Content.SuitAddons;

namespace MetroidMod.Default
{
	[Autoload(false)]
	
	internal class MissileAddonItem : ModItem
	{
		public ModMissileAddon modMissileAddon;

		public override string Texture => modMissileAddon.ItemTexture;
		public override string Name => modMissileAddon.Name + "Addon";
		public override LocalizedText Tooltip => modMissileAddon.Tooltip ?? base.Tooltip;

		public MissileAddonItem(ModMissileAddon modMissileAddon)
		{
			this.modMissileAddon = modMissileAddon;
		}

		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = Main.netMode == NetmodeID.Server ? 32 : ModContent.Request<Texture2D>(Texture).Value.Width;
			Item.height = Main.netMode == NetmodeID.Server ? 32 : ModContent.Request<Texture2D>(Texture).Value.Height;
			modMissileAddon.SetItemDefaults(Item);
			modMissileAddon.ItemType = Type;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.vanity = false;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = modMissileAddon.TileType;
		}

		public override void HoldItem(Player player)
		{
			if (modMissileAddon.ShowTileHover(player))
			{
				player.cursorItemIconEnabled = true;
				player.cursorItemIconID = Type;
			}
		}

		//You need the next two methods in here or else it will just NOT WORK
		//And it'll take ages to figure out the problem
		//For future reference for other addon systems I guess??
		public override ModItem Clone(Item item)
		{
			MissileAddonItem obj = (MissileAddonItem)base.Clone(item);
			obj.modMissileAddon = modMissileAddon;
			return obj;
		}

		public override ModItem NewInstance(Item entity)
		{
			var inst = Clone(entity);
			return inst;
		}
		//Again, don't forget those two up there
		//You'll be kicking yourself over it
		public override void AddRecipes()
		{
			modMissileAddon.AddRecipes();
		}
	}
}
