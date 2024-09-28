﻿using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Default
{
	[Autoload(false)]
	internal class SuitAddonItem : ModItem
	{
		public ModSuitAddon modSuitAddon;

		public override string Texture => modSuitAddon.ItemTexture;

		public override string Name => modSuitAddon.Name + "Addon";

		public override LocalizedText Tooltip => modSuitAddon.Tooltip ?? base.Tooltip;

		public SuitAddonItem(ModSuitAddon modSuitAddon)
		{
			this.modSuitAddon = modSuitAddon;
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault(modSuitAddon.DisplayName.GetDefault() + (!modSuitAddon.ItemNameLiteral ? " Addon" : ""));
			// Tooltip.SetDefault(modSuitAddon.AddOnlyAddonItem ? $"Cannot be equipped\n{modSuitAddon.Tooltip.GetDefault()}" : string.Format("[c/9696FF:Suit Addon]") + $"\nSlot Type: {modSuitAddon.GetAddonSlotName()}\n{modSuitAddon.Tooltip.GetDefault()}");
			Item.ResearchUnlockCount = modSuitAddon.SacrificeTotal;
		}

		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = Main.netMode == NetmodeID.Server ? 32 : ModContent.Request<Texture2D>(Texture).Value.Width;
			Item.height = Main.netMode == NetmodeID.Server ? 32 : ModContent.Request<Texture2D>(Texture).Value.Height;
			modSuitAddon.SetItemDefaults(Item);
			modSuitAddon.ItemType = Type;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.vanity = false;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = modSuitAddon.TileType;
			Item.GetGlobalItem<Common.GlobalItems.MGlobalItem>().AddonType = AddonType.Suit;
		}

		public override void HoldItem(Player player)
		{
			if (modSuitAddon.ShowTileHover(player))
			{
				player.cursorItemIconEnabled = true;
				player.cursorItemIconID = Type;
			}
		}
		public override bool IsVanitySet(int head, int body, int legs) => true;

		public override bool AltFunctionUse(Player player) => modSuitAddon.AltFunctionUse(player);

		public override bool CanUseItem(Player player) => modSuitAddon.CanUseItem(player);

		public override bool? UseItem(Player player) => modSuitAddon.UseItem(player);

		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) => itemGroup = ContentSamples.CreativeHelper.ItemGroup.Accessories;

		public override ModItem Clone(Item item)
		{
			SuitAddonItem obj = (SuitAddonItem)base.Clone(item);
			obj.modSuitAddon = modSuitAddon;
			return obj;
		}

		public override ModItem NewInstance(Item entity)
		{
			var inst = Clone(entity);
			return inst;
		}

		public override void AddRecipes()
		{
			modSuitAddon.AddRecipes();
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			modSuitAddon.UpdateAccessory(player, hideVisual);
		}

		public override void UpdateInventory(Player player)
		{
			modSuitAddon.UpdateInventory(player);
		}
	}
}
