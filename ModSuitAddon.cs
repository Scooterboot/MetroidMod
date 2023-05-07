using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using MetroidMod.Default;
using MetroidMod.ID;

namespace MetroidMod
{
	public abstract class ModSuitAddon : ModType
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
		public Item Item;
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

		public virtual string ArmorTextureHead { get; }
		public virtual string ArmorTextureTorso { get; }
		public virtual string ArmorTextureArmsGlow { get; }
		public virtual string ArmorTextureLegs { get; }

		public abstract string TileTexture { get; }

		/// <summary>
		/// The path to the texture shown for the visor in the visor select. <br />
		/// Note: This is only used for Visors, such as the X-Ray Scope.
		/// </summary>
		public virtual string VisorSelectIcon { get; }

		/// <summary>
		/// The Sound to be played when the visor is in use. <br />
		/// Note: This is only used for Visors, such as the Scan Visor.
		/// </summary>
		public virtual SoundStyle? VisorBackgroundNoise => null;

		/// <summary>
		/// The Color to set the hud to when the visor is in use. <br />
		/// Note: This is only used for Visors.
		/// </summary>
		public virtual Color VisorColor { get; } = Color.LightBlue;

		public abstract bool AddOnlyAddonItem { get; }

		public int SacrificeTotal { get; set; } = 1;

		public bool ItemNameLiteral { get; set; } = true;

		public virtual int AddonSlot { get; set; } = SuitAddonSlotID.None;
		internal bool IsArmor => ArmorTextureHead != null && ArmorTextureHead != "" && ArmorTextureTorso != null && ArmorTextureTorso != "" && ArmorTextureLegs != null && ArmorTextureLegs != "";
		public string GetAddonSlotName() => SuitAddonLoader.GetAddonSlotName(AddonSlot);
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
			ModItem.SetStaticDefaults();
			SetupDrawing();
		}
		public override void Load()
		{
			ModItem = new SuitAddonItem(this);
			ModTile = new SuitAddonTile(this);
			if (ModItem == null) { throw new Exception("WTF happened here? SuitAddonItem is null!"); }
			if (ModTile == null) { throw new Exception("WTF happened here? SuitAddonTile is null!"); }
			Mod.AddContent(ModItem);
			Mod.AddContent(ModTile);
			if (IsArmor && Main.netMode != NetmodeID.Server)
			{
				EquipLoader.AddEquipTexture(Mod, ArmorTextureHead, EquipType.Head, name: Name);
				EquipLoader.AddEquipTexture(Mod, ArmorTextureTorso, EquipType.Body, name: Name);
				EquipLoader.AddEquipTexture(Mod, ArmorTextureLegs, EquipType.Legs, name: Name);
			}
		}

		private void SetupDrawing()
		{
			if (Main.netMode == NetmodeID.Server || !IsArmor) { return; }
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
			int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

			ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
			ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
			//ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
			ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
		}
		public override void Unload()
		{
			ModItem.Unload();
			ModTile.Unload();
			ModItem = null;
			ModTile = null;
			Item = null;
			base.Unload();
		}

		protected override sealed void Register()
		{
			DisplayName = Language.GetOrRegister(Mod, $"SuitAddonName.{Name}");
			Tooltip = Language.GetOrRegister(Mod, $"SuitAddonTooltip.{Name}");
			if (!AddOnlyAddonItem)
			{
				Type = SuitAddonLoader.AddonCount;
				if (Type > 127)
				{
					throw new Exception("Suit Addons Limit Reached. (Max: 128)");
				}
				SuitAddonLoader.addons.Add(this);
			}
			MetroidMod.Instance.Logger.Info("Register new Suit Addon: " + FullName + ", OnlyAddonItem: " + AddOnlyAddonItem);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}

		/// <inheritdoc cref="ModItem.SetDefaults()"/>
		public virtual void SetItemDefaults(Item item) { }

		public virtual bool ShowTileHover(Player player) => player.InInteractionRange(Player.tileTargetX, Player.tileTargetY);

		/// <inheritdoc cref="ModItem.UpdateAccessory(Player, bool)"/>
		public virtual void UpdateAccessory(Player player, bool hideVisual) { UpdateInventory(player); }

		/// <inheritdoc cref="ModItem.UpdateInventory(Player)"/>
		public virtual void UpdateInventory(Player player) { }

		/// <inheritdoc cref="ModItem.UpdateArmorSet(Player)"/>
		public virtual void OnUpdateArmorSet(Player player, int stack) { }

		/// <inheritdoc cref="ModItem.UpdateVanitySet(Player)"/>
		public virtual void OnUpdateVanitySet(Player player) { }

		/// <inheritdoc cref="ModItem.ArmorSetShadows(Player)"/>
		public virtual void ArmorSetShadows(Player player) { }

		/// <inheritdoc cref="ModItem.AltFunctionUse(Player)"/>
		public virtual bool AltFunctionUse(Player player) { return false; }

		/// <inheritdoc cref="ModItem.CanUseItem(Player)"/>
		public virtual bool CanUseItem(Player player) { return true; }

		/// <inheritdoc cref="ModItem.UseItem(Player)"/>
		public virtual bool? UseItem(Player player) { return null; }

		/// <summary>
		/// Allows you to do things when this visor is equipped and in use. <br />
		/// Note: This is only called for Visors, such as the X-Ray Scope.
		/// </summary>
		/// <param name="player">The player.</param>
		public virtual void DrawVisor(Player player) { }

		public virtual string GetSetBonusText()
		{
			return null;
		}

		/// <inheritdoc cref="ModItem.AddRecipes"/>
		public virtual void AddRecipes() { }

		public Recipe CreateRecipe(int amount = 1) => ModItem.CreateRecipe(amount);
	}
}
