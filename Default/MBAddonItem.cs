using MetroidMod.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Default
{
	[Autoload(false)]
	internal class MBAddonItem : ModItem
	{
		public ModMBAddon modMBAddon;

		public override string Texture => modMBAddon.ItemTexture;

		public override string Name => modMBAddon.Name + "Addon";

		public MBAddonItem(ModMBAddon modMBAddon)
		{
			this.modMBAddon = modMBAddon;
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault(modMBAddon.DisplayName.GetDefault() + (!modMBAddon.ItemNameLiteral ? " Addon" : ""));
			// Tooltip.SetDefault(modMBAddon.AddOnlyAddonItem ? $"Cannot be equipped\n{modMBAddon.Tooltip.GetDefault()}" : string.Format("[c/9696FF:Morph Ball Addon]") + $"\nSlot Type: {modMBAddon.GetAddonSlotName()}\n{modMBAddon.Tooltip.GetDefault()}");
			Item.ResearchUnlockCount = modMBAddon.SacrificeTotal;
		}

		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = Main.netMode == NetmodeID.Server ? 32 : ModContent.Request<Texture2D>(Texture).Value.Width;
			Item.height = Main.netMode == NetmodeID.Server ? 32 : ModContent.Request<Texture2D>(Texture).Value.Height;
			modMBAddon.SetItemDefaults(Item);
			modMBAddon.ItemType = Type;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.vanity = false;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = modMBAddon.TileType;
			Item.GetGlobalItem<Common.GlobalItems.MGlobalItem>().AddonType = AddonType.MorphBall;
		}

		public override void HoldItem(Player player)
		{
			if (player.InInteractionRange(Player.tileTargetX, Player.tileTargetY, default))
			{
				player.cursorItemIconEnabled = true;
				player.cursorItemIconID = Type;
			}
		}

		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) => itemGroup = ContentSamples.CreativeHelper.ItemGroup.Accessories;

		public override ModItem Clone(Item item)
		{
			MBAddonItem obj = (MBAddonItem)base.Clone(item);
			obj.modMBAddon = modMBAddon;
			return obj;
		}

		public override ModItem NewInstance(Item entity)
		{
			var inst = Clone(entity);
			return inst;
		}

		public override void AddRecipes()
		{
			if (modMBAddon.AddonSlot == MorphBallAddonSlotID.Weapon)
			{
				RecipeGroup.recipeGroups[MetroidMod.MorphBallBombsRecipeGroupID].ValidItems.Add(Type);
			}
			modMBAddon.AddRecipes();
		}
	}
}
