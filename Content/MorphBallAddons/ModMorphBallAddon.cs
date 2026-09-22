using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Items;
using MetroidMod.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Content.MorphBallAddons
{
	public enum MorphBallAddonSlot : sbyte
	{
		Unassigned = -1,
		Drill = 0,
		Weapon = 1,
		Special = 2,
		Utility = 3,
		Boost = 4
	}

	public interface IMorphBallAddon : IGeneratesModItem, IGeneratesModTile
	{
		MorphBallAddonSlot AddonSlot { get; }
		
		void UpdateEquip(Player player);
	}

	public abstract class ModMorphBallAddon : ModType, IMorphBallAddon
	{
		public int Type { get; private set; }

		public virtual MorphBallAddonSlot AddonSlot => MorphBallAddonSlot.Unassigned;

		public GeneratedModItem GeneratedModItem { get; internal set; }
		public GeneratedModTile GeneratedModTile { get; internal set; }

		public int ItemType { get; internal set; }
		public int TileType { get; internal set; }

		public Item Item => GeneratedModItem.Item;

		public virtual LocalizedText ItemDisplayName => Mod.GetLocalization($"{LocalizationCategory}.{Name}.DisplayName", PrettyPrintName);
		public virtual LocalizedText ItemTooltip => Mod.GetLocalization($"{LocalizationCategory}.{Name}.Tooltip", () => "");
		public static string LocalizationCategory => "MorphBallAddons";
		
		public virtual string TexturePath => (GetType().Namespace + "." + Name).Replace('.', '/');
		public virtual string ItemTexture => TexturePath + "_Item";
		public virtual string TileTexture => TexturePath + "_Tile";

		protected override sealed void Register()
		{
			ModTypeLookup<ModMorphBallAddon>.Register(this);
			Type = MorphBallAddonLoader.AddonCount;
			MorphBallAddonLoader.addons.Add(this);
			MetroidMod.Instance.Logger.Info("Register new Suit Addon: " + FullName);
		}

		public override void Load()
		{
			GeneratedModItem = new GeneratedModItem(this);
			GeneratedModTile = new GeneratedModTile(this);
			Mod.AddContent(GeneratedModItem);
			Mod.AddContent(GeneratedModTile);
			ItemType = GeneratedModItem.Type;
			TileType = GeneratedModTile.Type;
		}


		public virtual void ItemAddRecipes() { }

		public virtual bool ItemAltFunctionUse(Player player) => false;

		public virtual bool ItemCanUseItem(Player player) => true;

		public virtual void ItemHoldItem(Player player) { }

		public virtual void ItemSetDefaults()
		{
			GeneratedModItem.Item.DefaultToPlaceableTile(TileType);

			GeneratedModItem.Item.maxStack = 1;
			GeneratedModItem.Item.width = 32;
			GeneratedModItem.Item.height = 32;
		}

		public virtual void ItemSetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public virtual bool? ItemUseItem(Player player) => true;

		public virtual void TileAnimateTile(ref int frame, ref int frameCounter) { }

		public virtual bool TileCanExplode(int i, int j) => true;

		public virtual bool TileCanKillTile(int i, int j, ref bool blockDamaged) => true;

		public virtual void TileKillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) { }

		public virtual void TileMouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ItemType;
		}

		public virtual void TileNumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public virtual bool TilePreDraw(int i, int j, SpriteBatch spriteBatch) => true;

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

		public virtual bool TileSlope(int i, int j) { return false; }

		public virtual void UpdateEquip(Player player)
		{
			
		}

		public virtual IGeneratesModItem Clone(GeneratedModItem newGeneratedModItem)
		{
			ModMorphBallAddon inst = (ModMorphBallAddon)MemberwiseClone();
			inst.GeneratedModItem = newGeneratedModItem;
			return inst;
		}
	}
}