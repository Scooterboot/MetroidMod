using System;
using MetroidMod.Default;
using MetroidMod.ID;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod
{
	public abstract class ModMBAddon : ModType
	{
		public int Type { get; private set; }
		internal void ChangeType(int type) => Type = type;
		/// <summary>
		/// The <see cref="ModItem"/> this addon controls.
		/// </summary>
		public ModItem ModItem;
		/// <summary>
		/// The <see cref="ModTile"/> this addon controls.
		/// </summary>
		public ModTile ModTile;
		/// <summary>
		/// The <see cref="Item"/> this addon controls.
		/// </summary>
		public Item Item => ModItem.Item;
		public int ItemType { get; internal set; }
		public int TileType { get; internal set; }

		/// <summary>
		/// The translations for the display name of this item.
		/// </summary>
		public LocalizedText DisplayName { get; internal set; }

		/// <summary>
		/// The translations for the tooltip of this item.
		/// </summary>
		public LocalizedText Tooltip { get; internal set; }

		public abstract string ItemTexture { get; }

		public abstract string TileTexture { get; }

		public abstract bool AddOnlyAddonItem { get; }

		public int SacrificeTotal { get; set; } = 1;

		public bool ItemNameLiteral { get; set; } = true;

		public int AddonSlot { get; set; } = MorphBallAddonSlotID.None;
		public string GetAddonSlotName() => MBAddonLoader.GetAddonSlotName(AddonSlot);
		/// <summary>
		/// Determines if the addon can generate on Chozo Statues during world generation.
		/// </summary>
		/// <param name="x">The X location of the tile</param>
		/// <param name="y">The Y location of the tile</param>
		public virtual bool CanGenerateOnChozoStatue(int x, int y) => false;
		/// <summary>
		/// Determines how likely the addon will generate on Chozo Statues.
		/// </summary>
		/// <param name="x">The X location of the tile</param>
		/// <param name="y">The Y location of the tile</param>
		public virtual double GenerationChance(int x, int y) { return 0; }

		public override sealed void SetupContent()
		{
			SetStaticDefaults();
			InternalStaticDefaults();
			ModItem.SetStaticDefaults();
		}

		internal virtual void InternalStaticDefaults() { }

		public override void Load()
		{
			ModItem = new MBAddonItem(this);
			ModTile = new MBAddonTile(this);
			if (ModItem == null) { throw new Exception("WTF happened here? MissileAddonItem is null!"); }
			if (ModTile == null) { throw new Exception("WTF happened here? MissileAddonTile is null!"); }
			Mod.AddContent(ModItem);
			Mod.AddContent(ModTile);
		}
		public override void Unload()
		{
			ModItem.Unload();
			ModTile.Unload();
			ModItem = null;
			ModTile = null;
			base.Unload();
		}
		protected override sealed void Register()
		{
			DisplayName = Language.GetOrRegister(Mod, $"SuitAddonName.{Name}");
			Tooltip = Language.GetOrRegister(Mod, $"SuitAddonTooltip.{Name}");
			if (!AddOnlyAddonItem)
			{
				Type = MBAddonLoader.AddonCount;
				if (Type > 127)
				{
					throw new Exception("Morph Ball Addons Limit Reached. (Max: 128)");
				}
				MBAddonLoader.addons.Add(this);
			}
			MetroidMod.Instance.Logger.Info("Register new Morph Ball Addon: " + FullName + ", OnlyAddonItem: " + AddOnlyAddonItem);
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

		public Recipe CreateRecipe(int amount = 1) => ModItem.CreateRecipe(amount);
	}
}
