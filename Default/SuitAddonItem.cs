using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Default
{
	[Autoload(false)]
	internal class SuitAddonItem : ModItem
	{
		public ModSuitAddon modSuitAddon;

		public override string Texture => modSuitAddon.ItemTexture;

		public override string Name => modSuitAddon.Name + "Addon";

		public SuitAddonItem(ModSuitAddon modSuitAddon)
		{
			this.modSuitAddon = modSuitAddon;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(modSuitAddon.DisplayName.GetDefault() + (!modSuitAddon.ItemNameLiteral ? " Addon" : ""));
			Tooltip.SetDefault(modSuitAddon.AddOnlyAddonItem ? $"Cannot be equipped\n{modSuitAddon.Tooltip.GetDefault()}" : $"Slot Type: {modSuitAddon.GetAddonSlotName()}\n{modSuitAddon.Tooltip.GetDefault()}");
			SacrificeTotal = 1;
		}

		public override void SetDefaults()
		{
			Item.maxStack = 1;
			modSuitAddon.SetItemDefaults(Item);
			modSuitAddon.ItemType = Type;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.width = ModContent.Request<Texture2D>(Texture).Value.Width;
			Item.height = ModContent.Request<Texture2D>(Texture).Value.Height;
			Item.vanity = false;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = modSuitAddon.TileType;
			Item.GetGlobalItem<Common.GlobalItems.MGlobalItem>().AddonType = AddonType.Suit;
		}

		public override void HoldItem(Player player)
		{
			if (player.InInteractionRange(Player.tileTargetX, Player.tileTargetY))
			{
				player.cursorItemIconEnabled = true;
				player.cursorItemIconID = Type;
			}
		}

		public override void ArmorArmGlowMask(Player drawPlayer, float shadow, ref int glowMask, ref Color color)
		{
			base.ArmorArmGlowMask(drawPlayer, shadow, ref glowMask, ref color);
		}
		public override bool IsVanitySet(int head, int body, int legs) => true;

		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) => itemGroup = ContentSamples.CreativeHelper.ItemGroup.Accessories;

		public override ModItem Clone(Item item)
		{
			SuitAddonItem obj = (SuitAddonItem)base.Clone(item);
			obj.modSuitAddon = modSuitAddon;
			return obj;
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
