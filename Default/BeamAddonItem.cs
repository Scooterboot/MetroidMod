using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Default
{
	[Autoload(false)]
	///<summary>
	///The template off of which <see cref="ModBeamAddon"/>s generate their items.
	///</summary>
	internal class BeamAddonItem(ModBeamAddon modBeamAddon) : ModItem
	{
		/// <summary>
		/// <inheritdoc cref="BeamAddonTile.modBeamAddon"/>
		/// </summary>
		public ModBeamAddon modBeamAddon = modBeamAddon;

		public override string Texture => modBeamAddon.ItemTexture;
		public override string Name => modBeamAddon.Name + "Addon";
		public override LocalizedText Tooltip => modBeamAddon.Tooltip ?? base.Tooltip;

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

		public override void RightClick(Player player) => modBeamAddon.RightClick(player);

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) => modBeamAddon.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => modBeamAddon.PostDrawInWorld(Item, spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);

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


		public override void AddRecipes() => modBeamAddon.AddRecipes();
	}
}
