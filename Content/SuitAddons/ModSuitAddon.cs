using System;
using MetroidMod.Content.Items;
using MetroidMod.Content.Tiles;
using MetroidMod.Default;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Content.SuitAddons
{
	public interface ISuitAddon : IGeneratesModItem, IGeneratesModTile
	{

	}
	/// <summary>
	/// Base class for Suit Addons. Don't use, use <see cref="ModSuitUpgrade"> and <see cref="ModVisorAddon"> instead. 
	/// For more advanced use-cases, extend this class and implement <see cref="IHelmetAddon"> or <see cref="IBreastplateAddon">.
	/// </summary>
	public abstract class ModSuitAddon : ModType, ISuitAddon
	{
		public int Type { get; private set; }
		internal void ChangeType(int type) => Type = type;

		public GeneratedModItem GeneratedModItem { get; internal set; }

		public GeneratedModTile GeneratedModTile { get; internal set; }
		public int ItemType { get; internal set; }
		public int TileType { get; internal set; }

		/// <summary>
		/// <inheritdoc cref="ModItem.DisplayName"/>
		/// </summary>
		public virtual LocalizedText ItemDisplayName => Mod.GetLocalization($"{LocalizationCategory}.{Name}.DisplayName", PrettyPrintName);

		/// <summary>
		/// <inheritdoc cref="ModItem.Tooltip"/>
		/// </summary>
		public virtual LocalizedText ItemTooltip => Mod.GetLocalization($"{LocalizationCategory}.{Name}.Tooltip", () => "");

		public static string LocalizationCategory => "SuitAddons";
		
		public virtual string TexturePath => (GetType().Namespace + "." + Name).Replace('.', '/');

		public virtual string ItemTexture => TexturePath + "_Item";

		public virtual string TileTexture => TexturePath + "_Tile";

		/// <summary>
		/// Determines if the addon can generate on Chozo Statues during world generation.
		/// </summary>
		/// <param name="x">The X location of the tile</param>
		/// <param name="y">The Y location of the tile</param>
		public virtual bool CanGenerateOnChozoStatue() => false;
		/// <summary>
		/// Determines how likely the addon will generate on Chozo Statues.
		/// </summary>
		/// <param name="x">The X location of the tile</param>
		/// <param name="y">The Y location of the tile</param>
		public virtual double GenerationChance() { return 0; }
		public override sealed void SetupContent()
		{
			ItemType = GeneratedModItem.Type;
			TileType = GeneratedModTile.Type;

			SetStaticDefaults();
			// SetupDrawing();
		}
		public override void Load()
		{
			GeneratedModItem = new GeneratedModItem(this);
			GeneratedModTile = new GeneratedModTile(this);
			Mod.AddContent(GeneratedModItem);
			Mod.AddContent(GeneratedModTile);
			ItemType = GeneratedModItem.Type;
			TileType = GeneratedModTile.Type;
			// if (Main.netMode != NetmodeID.Server)
			// {
			// 	EquipLoader.AddEquipTexture(Mod, ArmorTextureHead, EquipType.Head, name: Name);
			// 	EquipLoader.AddEquipTexture(Mod, ArmorTextureTorso, EquipType.Body, name: Name);
			// 	EquipLoader.AddEquipTexture(Mod, ArmorTextureLegs, EquipType.Legs, name: Name);
			// }
		}

		// private void SetupDrawing()
		// {
		// 	if (Main.netMode == NetmodeID.Server || !IsArmor) { return; }
		// 	int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
		// 	int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
		// 	int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

		// 	ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
		// 	ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
		// 	//ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
		// 	ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
		// }

		protected override sealed void Register()
		{
			ModTypeLookup<ModSuitAddon>.Register(this);
			Type = SuitAddonLoader.AddonCount;
			SuitAddonLoader.addons.Add(this);
			MetroidMod.Instance.Logger.Info("Register new Suit Addon: " + FullName);
		}

		/// <inheritdoc cref="GeneratedModItem.AddRecipes"/>
		public virtual void ItemAddRecipes(GeneratedModItem generatedModItem) { }
		
		public virtual void ItemSetDefaults(GeneratedModItem generatedModItem)
		{
			GeneratedModItem.Item.DefaultToPlaceableTile(TileType);

			GeneratedModItem.Item.maxStack = 1;
			GeneratedModItem.Item.width = 32;
			GeneratedModItem.Item.height = 32;
		}

		public virtual void ItemSetStaticDefaults(GeneratedModItem generatedModItem)
		{
			generatedModItem.Item.ResearchUnlockCount = 1;
		}

		public virtual void TileSetStaticDefaults()
		{
			Main.tileFrameImportant[TileType] = true;
			Main.tileBlockLight[TileType] = true;
			Main.tileSpelunker[TileType] = true;
			Main.tileOreFinderPriority[TileType] = 807;
			Main.tileNoAttach[TileType] = true;
			LocalizedText name = GeneratedModTile.CreateMapEntryName();
			GeneratedModTile.AddMapEntry(new Color(255, 126, 255), name);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(TileType);
			TileID.Sets.DisableSmartCursor[TileType] = true;
		}

		public virtual void TileAnimateTile(ref int frame, ref int frameCounter) { }

		public bool TileCanExplode(int i, int j) { return true; }

		public virtual bool TileCanKillTile(int i, int j, ref bool blockDamaged) { return true; }

		public virtual void TileKillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) { }

		public virtual void TileMouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ItemType;
		}

		public virtual void TileNumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public bool TilePreDraw(int i, int j, SpriteBatch spriteBatch) => true;

		public virtual bool TileRightClick(int i, int j)
		{
			bool blockDamaged = false;
			if (!TileCanKillTile(i, j, ref blockDamaged)) { return true; }
			WorldGen.KillTile(i, j, false, false, false);
			if (Main.netMode == NetmodeID.MultiplayerClient && !Main.tile[i, j].HasTile)
			{
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
			}
			return true;
		}

		public virtual bool TileSlope(int i, int j) => false;

		public virtual bool ShowTileHover(Player player) => player.InInteractionRange(Player.tileTargetX, Player.tileTargetY, default);

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

		/// <inheritdoc cref="ModItem.HoldItem(Player)"/>
		public virtual void HoldItem(Player player) { }

		/// <inheritdoc cref="ModMBAddon.CanExplodeTile(int, int)"/>
		public virtual bool CanExplodeTile(int i, int j) { return true; }

		public virtual IGeneratesModItem Clone(GeneratedModItem newGeneratedModItem)
		{
			ModSuitAddon inst = (ModSuitAddon)MemberwiseClone();
			inst.GeneratedModItem = newGeneratedModItem;
			return inst;
		}
	}
}
