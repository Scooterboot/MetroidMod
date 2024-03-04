namespace MetroidMod
{
	/*
	public abstract class ModMissileAddon : ModType
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
		public abstract string PowerBeamTexture { get; }

		public abstract bool AddOnlyMissileItem { get; }


		public override sealed void SetupContent()
		{
			//Textures = new Asset<Texture2D>[4];
			SetStaticDefaults();
		}
		public override void Load()
		{
			Item = new MissileAddonItem(this);
			Tile = new MissileAddonTile(this);
			if (Item == null) { throw new Exception("WTF happened here? MissileAddonItem is null!"); }
			if (Tile == null) { throw new Exception("WTF happened here? MissileAddonTile is null!"); }
			Mod.AddContent(Item);
			Mod.AddContent(Tile);
		}
		protected override sealed void Register()
		{
			DisplayName = LocalizationLoader.CreateTranslation(Mod, $"SuitAddonName.{Name}");
			Tooltip = LocalizationLoader.CreateTranslation(Mod, $"SuitAddonTooltip.{Name}");
			if (!AddOnlyMissileItem)
			{
				Type = MissileLauncherLoader.MissileCount;
				if (Type > 127)
				{
					throw new Exception("Missile Limit Reached. (Max: 128)");
				}
				MissileLauncherLoader.missileAddons.Add(this);
			}
			Mod.Logger.Info("Register new Missile: " + FullName + ", OnlyMissileItem: " + AddOnlyMissileItem);
		}

		public override void SetStaticDefaults() => base.SetStaticDefaults();


		public virtual void OnHit(Entity entity)
		{

		}

		public virtual bool OnShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return true;
		}

		public virtual bool CanUse(Player player)
		{
			return true;
		}
	}
	*/
}
