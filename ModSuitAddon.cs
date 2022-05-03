using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.ID;

namespace MetroidModPorted
{
	public abstract class ModSuitAddon : ModType
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
		/// The translations for the display name of this tooltip.
		/// </summary>
		public ModTranslation Tooltip { get; internal set; }

		public abstract string ItemTexture { get; }

		public virtual string ArmorTextureHead { get; set; }
		public virtual string ArmorTextureTorso { get; set; }
		public virtual string ArmorTextureLegs { get; set; }

		public abstract string TileTexture { get; }

		public bool AddOnlyAddonItem { get; set; }

		public bool ItemNameLiteral { get; set; } = true;

		public virtual int AddonSlot { get; set; } = SuitAddonSlotID.None;
		internal bool IsArmor => ArmorTextureHead != null && ArmorTextureHead != "" && ArmorTextureTorso != null && ArmorTextureTorso != "" && ArmorTextureLegs != null && ArmorTextureLegs != "";
		public string GetAddonSlotName()
		{
			return AddonSlot switch
			{
				SuitAddonSlotID.Tanks_Energy => "Energy Tank",
				SuitAddonSlotID.Tanks_Reserve => "Reserve Tank",
				SuitAddonSlotID.Suit_Varia => "Varia",
				SuitAddonSlotID.Suit_Utility => "Utility",
				SuitAddonSlotID.Suit_Augment => "Augmentation",
				SuitAddonSlotID.Suit_LunarAugment => "Secondary Augmentation",
				SuitAddonSlotID.Misc_Grip => "Hand",
				SuitAddonSlotID.Misc_Attack => "Attack",
				SuitAddonSlotID.Boots_JumpHeight => "Boots",
				SuitAddonSlotID.Boots_Jump => "Jump",
				SuitAddonSlotID.Boots_Speed => "Speed Augmentation",
				_ => "Unknown"
			};
		}
		public override sealed void SetupContent()
		{
			SetStaticDefaults();
			Item.SetStaticDefaults();
			SetupDrawing();
		}
		public override void Load()
		{
			Item = new SuitAddonItem(this);
			Tile = new SuitAddonTile(this);
			if (Item == null) { throw new Exception("WTF happened here? SuitAddonItem is null!"); }
			if (Tile == null) { throw new Exception("WTF happened here? SuitAddonTile is null!"); }
			Mod.AddContent(Item);
			Mod.AddContent(Tile);
			if (IsArmor && Main.netMode != NetmodeID.Server)
			{
				Mod.AddEquipTexture(Item, EquipType.Head, ArmorTextureHead);
				Mod.AddEquipTexture(Item, EquipType.Body, ArmorTextureTorso);
				Mod.AddEquipTexture(Item, EquipType.Legs, ArmorTextureLegs);
			}
		}

		private void SetupDrawing()
		{
			if (!IsArmor) { return; }
			int equipSlotHead = Mod.GetEquipSlot(Item.Name, EquipType.Head);
			int equipSlotBody = Mod.GetEquipSlot(Item.Name, EquipType.Body);
			int equipSlotLegs = Mod.GetEquipSlot(Item.Name, EquipType.Legs);

			ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
			ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
			ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
			ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
		}

		protected override sealed void Register()
		{
			DisplayName = LocalizationLoader.CreateTranslation(Mod, $"SuitAddonName.{Name}");
			Tooltip = LocalizationLoader.CreateTranslation(Mod, $"SuitAddonTooltip.{Name}");
			if (!AddOnlyAddonItem)
			{
				Type = SuitAddonLoader.AddonCount;
				if (Type > 127)
				{
					throw new Exception("Suit Addons Limit Reached. (Max: 128)");
				}
				SuitAddonLoader.addons.Add(this);
			}
			Mod.Logger.Info("Register new Suit Addon: " + FullName + ", OnlyAddonItem: " + AddOnlyAddonItem);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}

		public virtual void SetItemDefaults(Item item) { }

		/// <inheritdoc cref="ModItem.UpdateAccessory(Player, bool)"/>
		public virtual void UpdateAccessory(Player player, bool hideVisual) { }

		/// <inheritdoc cref="ModItem.UpdateArmorSet(Player)"/>
		public virtual void OnUpdateArmorSet(Player player) { }

		/// <inheritdoc cref="ModItem.UpdateVanitySet(Player)"/>
		public virtual void OnUpdateVanitySet(Player player) { }

		/// <inheritdoc cref="ModItem.ArmorSetShadows(Player)"/>
		public virtual void ArmorSetShadows(Player player) { }

		public virtual string GetSetBonusText()
		{
			return null;
		}

		/// <inheritdoc cref="ModItem.AddRecipes"/>
		public virtual void AddRecipes() { }

		public Recipe CreateRecipe(int amount = 1) => Item.CreateRecipe(amount);
	}
}
