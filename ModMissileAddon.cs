using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria;
using MetroidMod.ID;
using MetroidMod.Default;
using Terraria.Audio;
using Terraria.ID;
using Terraria.DataStructures;
using MetroidMod.Content.Projectiles;
using Microsoft.Xna.Framework.Graphics;

//Left some notes in "type-definer variables"		-Z
namespace MetroidMod
{
	/// <summary>
	/// The base type for all Missile Launcher addons.<br/><br/>
	/// ModMissileAddons automatically generate a <see cref="Terraria.ModLoader.ModItem"/> and a <see cref="Terraria.ModLoader.ModTile"/> to access the addon in-game.<br/>
	/// Textures are grabbed automatically at this filepath:<br/>
	/// <u>(name of mod)<b>/Assets/Textures/MissileAddons/</b>(name of addon file)<b>/</b>(Item for item sprite, Tile for tile sprite, Shot for shot sprite, etc.)</u><br/>
	/// but can be overriden to point to any filepath. Sounds are also stored this way, just swap Textures for Sounds.<br/><br/>
	/// Every ModMissileAddon needs an <b>AddonSlot</b>, <b>ShapePriority</b>, and <b>ColorPriority</b>.
	/// </summary>
	public abstract class ModMissileAddon : ModType
	{
		#region Type-definer variables
		/// <summary>
		/// The numerical ID of the addon.<br/>
		/// Pretty much just like how Terraria's items all have a number ID.
		/// </summary>
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

		//PROPOSITION: built-in ModProjectiles for missile addons?
		//Can define and write its fields directly in the addon file like you can with the build-in ModItems and ModTiles
		//Would streamline and simplify the addon creation process while keeping file counts down
		//Necessary fields to begin implementation (but not to finish it) are written below, just comment them out if you decide to go with this
		//Other things this would require:
		// * A MissileAddonProjectile.cs in MetroidMod/Default
		// * A ModMissileAddon method for each Projectile method

		/// <summary>
		/// The <see cref="ModProjectile"/> this addon controls.
		/// </summary>
		public MProjectile mProjectile;

		/// <summary>
		/// The <see cref="Projectile"/> this addon controls.
		/// </summary>
		public Projectile Projectile => mProjectile.Projectile;
		/// <summary>
		/// References the ModItem previously generated
		/// </summary>
		public int ItemType { get; internal set; }
		/// <summary>
		/// References the ModTile previously generated
		/// </summary>
		public int TileType { get; internal set; }
		/// <summary>
		/// References the ModProjectile previously generated
		/// </summary>
		public int ProjectileType { get; internal set; }
		#endregion

		/// <summary>
		/// The translations for the tooltip of this item.
		/// </summary>
		public virtual LocalizedText Tooltip => ModItem.GetLocalization(nameof(Tooltip), () => "");

		/// <summary>
		/// The slot in the Addon UI that this addon uses.<br/><br/>
		/// See <see cref="MissileAddonSlotID"/> for details on the different slots.
		/// </summary> 
		public virtual int AddonSlot { get; set; } = MissileAddonSlotID.None;


		#region Appearance variables
		/// <summary>
		/// The filepath for the addon's item texture.
		/// </summary>
		public virtual string ItemTexture => $"{Mod.Name}/Assets/Textures/MissileAddons/{Name}/Item";
		/// <summary>
		/// the filepath for the addon's tile texture.
		/// </summary>
		public virtual string TileTexture => $"{Mod.Name}/Assets/Textures/MissileAddons/{Name}/Tile";
		/// <summary>
		/// The filepath for the addon's normal shot texture.
		/// </summary>
		public virtual string ShotTexture => $"{Mod.Name}/Assets/Textures/MissileAddons/{Name}/Shot";
		/// <summary>
		/// The amount of animation frames in the normal shot texture.
		/// </summary>
		public virtual int ShotFrames { get; } = 1;
		/// <summary>
		/// The filepath for the addon's shot sound effect.
		/// </summary>
		public virtual string ShotSound => $"{Mod.Name}/Assets/Sounds/MissileAddons/{Name}/Shot";
		/// <summary>
		/// The filepath for the addon's shot impact sound effect.
		/// </summary>
		public virtual string ImpactSound => $"{Mod.Name}/Assets/Sounds/MissileAddons/{Name}/Impact";
		/// <summary>
		/// The integer ID of the dust particles this addon's projectile will leave behind.
		/// <br/>Use <see cref="DustID"/> for vanilla dust and use <see cref="ModDust.Type"/> for modded ones.
		/// </summary>
		public abstract int ShotDust { get; }

		#region Charge Colors
		//Relevant only for charge addons. Determines the color of the charge lead.

		/// <summary>
		/// The primary color of the addon's projectile.
		/// </summary>
		public virtual Color PrimaryColor { get; set; } = MetroidMod.powColor;
		/// <summary>
		/// The secondary color of the addon's projectile, used for dark shading.
		/// <br/>For example, Ice Missile's secondary color is dark blue.
		/// </summary>
		public virtual Color SecondaryColor => PrimaryColor;
		/// <summary>
		/// The brightness of the projectile's "core" when this addon has color priority.
		/// <br/>Default is <b>1f</b>, or full brightness. In conjunction with <see cref="CoreSaturation"/>'s default value, the "core" appears pure white by default.
		/// </summary>
		public virtual float CoreBrightness => 1f;
		/// <summary>
		/// The saturation of the projectile's "core" when this addon has color priority.
		/// <br/>Default is <b>0</b>, or fully greyscale.
		/// </summary>
		public virtual float CoreSaturation => 0f;
		#endregion

		#endregion

		#region Addon stat variables
		//These stats are plugged into the WEAPON, not the projectile.
		/// <summary>
		/// The base damage value this addon adds.<br/>
		/// NOTE: Not to be confused with DamageMult, which is applied after this variable.
		/// </summary>
		public virtual int BaseDamage { get; set; } = 0;
		/// <summary>
		/// The damage multiplier value this addon adds.<br/>
		/// NOTE: Input the value as you would see it on the item's <i>tooltip</i>. It will be converted later.<br/>
		/// (i.e. if the addon should have a 50% damage increase, put 50f instead of 1.5f)
		/// </summary>
		public virtual float DamageMult { get; set; } = 0f;
		/// <summary>
		/// The base usetime value this addon adds.<br/>
		/// NOTE: Not to be confused with SpeedMult, which is applied after this variable.
		/// </summary>
		public virtual int BaseSpeed { get; set; } = 0;
		/// <summary>
		/// The usetime multiplier value this addon adds.<br/>
		/// NOTE: Input the value as you would see it on the item's <i>tooltip</i>. It will be converted later.<br/>
		/// (i.e. if the addon should have a 50% speed increase, put 50f instead of 1.5f)
		/// </summary>
		public virtual float SpeedMult { get; set; } = 0f;
		/// <summary>
		/// The amount of extra projectiles this addon will make the player fire.
		/// </summary>
		public virtual int AddShots { get; set; } = 0;
		#endregion

		#region Shot Behavior/Compatibility Variables
		//These stats get plugged into the PROJECTILE, not the weapon.
		/// <summary>
		/// The buff that this addon will inflict on hit.
		/// </summary>
		public virtual int InflictsBuff { get; set; }
		/// <summary>
		/// If true, this addon will continue to perform an action for as long as Fire is held.
		/// <br/>For advanced Missile shenanigans. Assemble said shenanigans over in <see cref="HoldFireBehavior(Player)"/>.
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		public virtual bool HoldFire { get; set; } = false;

		/// <summary>
		/// If false, this addon does not require Charge Beam to use.
		/// <br/>Applies only to charge addons.
		/// <br/><br/>Defaults to <b>true</b>.
		/// </summary>
		public virtual bool NeedsCharging { get; set; } = true; //thanks seeker missiles

		/// <summary>
		/// If true, this addon will <i>not</i> use its own projectile, and will instead inject its behavior into your primary addon.
		/// <br/>Does not affect Primary addons. Defaults to <b>false</b>.
		/// </summary>
		public virtual bool IgnoreProjectile => false;

		//Fake Block-related stuff
		/// <summary>
		/// The level of strength this missile addon has in regard to missile blocks.
		/// <br/><i>(e.g. Missile blocks are tier 1, Super Missile blocks are tier 2, etc. etc.)</i>
		/// <br/><br/>Defaults to <b>1</b>.
		/// </summary>
		public virtual int MissileTier { get; set; } = 1;

		/// <summary>
		/// If true, this missile addon cannot break standard tiered missile blocks.
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		public virtual bool TechnicallyNotAMissile { get; set; } = false;


		/// <summary>
		/// Lets you make the Arm Cannon do things while Fire is held down.
		/// <br/>To be used with <see cref="HoldFire"/>.
		/// </summary>
		public virtual void HoldFireBehavior(Player player) { }


		//Compatibility-related variables
		/// <summary>
		/// If true, this addon will not apply its properties to the Arm Cannon.
		/// <br/>Used to create incompatibilites between addons.
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		public virtual bool Overridden => false;
		/// <summary>
		/// If true, this addon will prevent the Arm Cannon it's installed in from firing.
		/// <br/>Used primarily in Suitlocking.
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		public virtual bool Locked => false;
		#endregion

		#region Data-handling methods
		/// <summary>
		/// Makes the addon in question only add the item and tile, not the Missile properties.<br/>
		/// Good for... something, I think   -Z
		/// </summary>
		public abstract bool AddOnlyAddonItem { get; }

		public override sealed void SetupContent()
		{
			SetStaticDefaults();
			ModItem.SetStaticDefaults();
		}

		public override void Load()
		{
			//ModMissileAddons automatically generate their items and tiles on load, based on the MissileAddonItem and MissileAddonTile templates.
			//This is the code that facilitates this.



			//Assigns a new M.A.I., M.A.T., and (usually) M.A.P. instance to the current M.M.A.
			ModItem = new MissileAddonItem(this);
			ModTile = new MissileAddonTile(this);
			if (!IgnoreProjectile) { mProjectile = new MissileAddonProjectile(this); }
			if (ModItem == null) { throw new Exception("WTF happened here? MissileAddonItem is null!"); }
			if (ModTile == null) { throw new Exception("WTF happened here? MissileAddonTile is null!"); }
			//Adds the content to the game.
			Mod.AddContent(ModItem);
			Mod.AddContent(ModTile);
			if (!IgnoreProjectile) { Mod.AddContent(mProjectile); }
			//Assigns the Type values to the appropriate fields.
			//If you forget this part, you can't call the addons through their Type, which breaks things like Shimmer recipes.
			ItemType = ModItem.Type;
			TileType = ModTile.Type;
			if (!IgnoreProjectile) { ProjectileType = mProjectile.Type; }

		}

		public override void Unload()
		{
			ModItem.Unload();
			ModTile.Unload();
			if (!IgnoreProjectile) { mProjectile.Unload(); }
			ModItem = null;
			ModTile = null;
			if (!IgnoreProjectile) { mProjectile = null; }
			base.Unload();
		}

		protected sealed override void Register()
		{
			if (!AddOnlyAddonItem && MissileAddonLoader.AddonCount <= 127)
			{
				Type = MissileAddonLoader.AddonCount;
				if (Type > 127)
				{
					throw new Exception("Missile Addons Limit Reached. (Max: 128)");
				}
				MissileAddonLoader.addons.Add(this);
			}
			MetroidMod.Instance.Logger.Info("Register new Missile Addon: " + FullName + ", OnlyAddonItem: " + AddOnlyAddonItem + ", IgnoreProjectile: " + IgnoreProjectile);
		}
		/// <summary>
		/// Used to instantiate copies of Missile addons.
		/// <br/> Useful for being able to have more than one Missile shot active at once.
		/// </summary>
		/// <returns></returns>
		public ModMissileAddon Clone()
		{
			return (ModMissileAddon)this.MemberwiseClone();
		}
		#endregion


		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[TileType] = true;
			Main.tileOreFinderPriority[Type] = 806;
			base.SetStaticDefaults();
		}

		#region ModItem fields

		/// <inheritdoc cref="ModItem.SetDefaults()"/>
		public virtual void SetItemDefaults(Item item) { }

		/// <inheritdoc cref="ModItem.PostDrawInInventory(SpriteBatch, Vector2, Rectangle, Color, Color, Vector2, float)"/>
		public virtual void PostDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) { }

		///<inheritdoc cref="ModItem.PostDrawInWorld(SpriteBatch, Color, Color, float, float, int)"/>
		public virtual void PostDrawInWorld(Item item, SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) { }
		///<inheritdoc cref="ModItem.RightClick(Player)"/>
		public virtual void RightClick(Player player)
		{

		}
		/// <inheritdoc cref="ModItem.AddRecipes"/>
		public virtual void AddRecipes() { }
		public Recipe CreateRecipe(int amount = 1) => ModItem.CreateRecipe(amount);

		#endregion

		#region Advanced addon properties


		#endregion

		#region ModTile fields
		/// <summary>
		/// Whether or not this addon's tile will display its ModItem texture when hovered over with the mouse.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool ShowTileHover(Player player) => player.InInteractionRange(Player.tileTargetX, Player.tileTargetY, default);
		/// <inheritdoc cref="ModTile.CanKillTile(int, int, ref bool)"/>
		public virtual bool CanKillTile(int i, int j) { return true; }
		/// <inheritdoc cref="ModMBAddon.CanExplodeTile(int, int)"/>
		public virtual bool CanExplodeTile(int i, int j) { return true; }

		///<inheritdoc cref="ModBlockType.PostDraw(int, int, SpriteBatch)"/>
		public virtual void PostDrawTile(int i, int j, SpriteBatch sb) { }
		#endregion

		#region ModProjectile fields

		/// <inheritdoc cref="ModProjectile.SetDefaults()"/>
		public virtual void SetProjectileDefaults(MProjectile mProjectile) { }


		/// <inheritdoc cref="ModProjectile.OnSpawn(IEntitySource)"/>
		public virtual void OnSpawn(MProjectile mProjectile, IEntitySource source) { }

		/// <inheritdoc cref="ModProjectile.PreAI()"/>
		public virtual bool PreAI(MProjectile mProjectile) { return true; }
		///<inheritdoc cref="ModProjectile.AI()"/>
		public virtual void AI(MProjectile mProjectile) { }
		///<inheritdoc cref="ModProjectile.PostAI()"/>
		public virtual void PostAI(MProjectile mProjectile) { }

		///<inheritdoc cref="ModProjectile.TileCollideStyle(ref int, ref int, ref bool, ref Vector2)"/>
		public virtual bool TileCollideStyle(MProjectile mProjectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) { return true; }
		///<inheritdoc cref="ModProjectile.OnTileCollide(Vector2)"/>
		public virtual bool OnTileCollide (MProjectile mProjectile, Vector2 oldVelocity) { return true; }

		/// <inheritdoc cref="ModProjectile.OnHitNPC(NPC, NPC.HitInfo, int)"/>
		public virtual void OnHitNPC(MProjectile mProjectile, NPC target, NPC.HitInfo hit, int damageDone) { }
		/// <inheritdoc cref="ModProjectile.OnHitPlayer(Player, Player.HurtInfo)"/>
		public virtual void OnHitPlayer(MProjectile mProjectile, Player target, Player.HurtInfo info) { }

		///<inheritdoc cref="ModProjectile.OnKill(int)"/>
		public virtual void OnKill(MProjectile mProjectile, int timeLeft) { }


		///<inheritdoc cref="ModProjectile.PreDraw(ref Color)"/>
		public virtual bool PreDrawProjectile(MProjectile mProjectile, ref Color lightColor) { return true; }

		///<inheritdoc cref="ModProjectile.PostDraw(Color)"/>
		public virtual void PostDrawProjectile(MProjectile mProjectile, Color lightColor) { }
		#endregion
	}
}
