using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.ID;
using MetroidMod.Default;
using MetroidMod.Content.Projectiles;
using Terraria.DataStructures;

namespace MetroidMod
{
	/// <summary>
	/// The base type for all Missile Launcher addons.
	/// </summary>
	public abstract class ModMissileAddon : ModType
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

		public MProjectile MProjectile;
		public int ItemType { get; internal set; }
		public int TileType { get; internal set; }

		//TODO: When you gut the old system remove the "NewFormat/" from the path directories
		public virtual string ItemTexture => $"{Mod.Name}/Assets/Textures/MissileAddons/NewFormat/{Name}/Item";
		public virtual string TileTexture => $"{Mod.Name}/Assets/Textures/MissileAddons/NewFormat/{Name}/Tile";
		public virtual int TileFrames { get; } = 1;
		public virtual string ShotTexture => $"{Mod.Name}/Assets/Textures/MissileAddons/NewFormat/{Name}/Shot";
		public virtual int ShotFrames { get; } = 1;
		public virtual string ShotSound => $"{Mod.Name}/Assets/Sounds/MissileAddons/NewFormat/{Name}/Shot";
		public virtual string ImpactSound => $"{Mod.Name}/Assets/Sounds/MissileAddons/NewFormat/{Name}/Impact";

		public virtual LocalizedText Tooltip => ModItem.GetLocalization(nameof(Tooltip), () => "");

		#region Addon stats and properties
		/// <summary>
		/// The addon's base damage.
		/// </summary>
		public virtual int BaseDamage { get; } = 0;
		/// <summary>
		/// The addon's base usetime.
		/// </summary>
		public virtual int BaseSpeed { get; } = 0;
		/// <summary>
		/// The addon's base velocity.
		/// </summary>
		public virtual int BaseVelocity { get; } = 0;
		/// <summary>
		/// The slot in the Missile Launcher this addon goes in.<br/><br/>
		/// See <see cref="MissileAddonSlotID"/> for details on each slot.
		/// </summary>
		public virtual int AddonSlot { get; set; } = MissileAddonSlotID.None;
		/// <summary>
		/// The Beam Addon that must be installed in order to use the addon.
		/// <br/><br/>Intended for use with <b>Charge Combos</b>.
		/// </summary>
		public virtual ModBeamAddon RequiredBeam { get; set; }
		#endregion

		public abstract bool AddOnlyAddonItem { get; }


		public override sealed void SetupContent()
		{
			//Textures = new Asset<Texture2D>[4];
			SetStaticDefaults();
		}
		public override void Load()
		{
			ModItem = new MissileAddonItem(this);
			ModTile = new MissileAddonTile(this);
			if (ModItem == null) { throw new Exception("WTF happened here? MissileAddonItem is null!"); }
			if (ModTile == null) { throw new Exception("WTF happened here? MissileAddonTile is null!"); }
			Mod.AddContent(ModItem);
			Mod.AddContent(ModTile);
		}
		protected override sealed void Register()
		{
			if (!AddOnlyAddonItem)
			{
				Type = MissileAddonLoader.AddonCount;
				if (Type > 127)
				{
					throw new Exception("Missile Addon Limit Reached. (Max: 128)");
				}
				MissileAddonLoader.addons.Add(this);
			}
			Mod.Logger.Info("Register new Missile: " + FullName + ", OnlyMissileItem: " + AddOnlyAddonItem);
		}

		public override void SetStaticDefaults() => base.SetStaticDefaults();

		/// <inheritdoc cref="ModItem.SetDefaults()"/>
		public virtual void SetItemDefaults(Item item) { }
		/// <inheritdoc cref="ModItem.AddRecipes"/>
		public virtual void AddRecipes() { }
		public virtual bool ShowTileHover(Player player) => player.InInteractionRange(Player.tileTargetX, Player.tileTargetY, default);
		/// <inheritdoc cref="ModTile.CanKillTile(int, int, ref bool)"/>
		public virtual bool CanKillTile(int i, int j) { return true; }
		/// <inheritdoc cref="ModMBAddon.CanExplodeTile(int, int)"/>
		public virtual bool CanExplodeTile(int i, int j) { return true; }
	}
}
