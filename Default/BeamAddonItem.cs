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
	
	internal class BeamAddonItem : ModItem
	{
		public ModBeamAddon modBeamAddon;

		public override string Texture => modBeamAddon.ItemTexture;
		public override string Name => modBeamAddon.Name + "Addon";
		public override LocalizedText Tooltip => modBeamAddon.Tooltip ?? base.Tooltip;

		public BeamAddonItem(ModBeamAddon modBeamAddon)
		{
			this.modBeamAddon = modBeamAddon;
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
			modBeamAddon.SetItemDefaults(Item);
			modBeamAddon.ItemType = Type;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.vanity = false;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = modBeamAddon.TileType;
		}

		public override void HoldItem(Player player)
		{
			if (modBeamAddon.ShowTileHover(player))
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
			BeamAddonItem obj = (BeamAddonItem)base.Clone(item);
			obj.modBeamAddon = modBeamAddon;
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
			modBeamAddon.AddRecipes();
		}
	}
}
