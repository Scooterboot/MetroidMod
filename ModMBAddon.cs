using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MetroidModPorted.Default;
using MetroidModPorted.ID;

namespace MetroidModPorted
{
	public abstract class ModMBAddon : ModType
	{
		public int Type { get; private set; }
		internal void ChangeType(int type) => Type = type;
		/// <summary>
		/// The <see cref="ModItem"/> this addon controls.
		/// </summary>
		public ModItem Item;
		/// <summary>
		/// The <see cref="ModTile"/> this addon controls.
		/// </summary>
		public ModTile Tile;
		public int ItemType { get; internal set; }
		public int TileType { get; internal set; }

		/// <summary>
		/// The translations for the display name of this item.
		/// </summary>
		public ModTranslation DisplayName { get; internal set; }

		/// <summary>
		/// The translations for the tooltip of this item.
		/// </summary>
		public ModTranslation Tooltip { get; internal set; }

		public abstract string ItemTexture { get; }

		public abstract string TileTexture { get; }

		public bool AddOnlyAddonItem { get; set; }

		public bool ItemNameLiteral { get; set; } = true;

		public int AddonSlot { get; set; } = MorphBallAddonSlotID.None;
		public string GetAddonSlotName() => MBAddonLoader.GetAddonSlotName(AddonSlot);
		/// <summary>
		/// Determines if the addon can generate on Chozo Statues during world generation.
		/// </summary>
		public virtual bool CanGenerateOnChozoStatue(Tile tile) => false;

		public override sealed void SetupContent()
		{
			SetStaticDefaults();
			InternalStaticDefaults();
			Item.SetStaticDefaults();
		}

		internal virtual void InternalStaticDefaults() { }

		public override void Load()
		{
			Item = new MBAddonItem(this);
			Tile = new MBAddonTile(this);
			if (Item == null) { throw new Exception("WTF happened here? MissileAddonItem is null!"); }
			if (Tile == null) { throw new Exception("WTF happened here? MissileAddonTile is null!"); }
			Mod.AddContent(Item);
			Mod.AddContent(Tile);
		}
		protected override sealed void Register()
		{
			DisplayName = LocalizationLoader.CreateTranslation(Mod, $"SuitAddonName.{Name}");
			Tooltip = LocalizationLoader.CreateTranslation(Mod, $"SuitAddonTooltip.{Name}");
			if (!AddOnlyAddonItem)
			{
				Type = MBAddonLoader.AddonCount;
				if (Type > 127)
				{
					throw new Exception("Morph Ball Addons Limit Reached. (Max: 128)");
				}
				MBAddonLoader.addons.Add(this);
			}
			Mod.Logger.Info("Register new Morph Ball Addon: " + FullName + ", OnlyAddonItem: " + AddOnlyAddonItem);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}

		/// <inheritdoc cref="ModItem.SetDefaults()"/>
		public virtual void SetItemDefaults(Item item) { }

		/// <inheritdoc cref="ModItem.UpdateEquip(Player)"/>
		public virtual void UpdateEquip(Player player) { }

		/// <inheritdoc cref="ModItem.AddRecipes"/>
		public virtual void AddRecipes() { }

		public Recipe CreateRecipe(int amount = 1) => Item.CreateRecipe(amount);
	}
}
