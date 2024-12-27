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

//gonna document as much of the code as I can to make it easy to follow
namespace MetroidMod
{
	/// <summary>
	/// The base type for all Power Beam addons.<br/><br/>
	/// ModBeamAddons automatically generate a <see cref="Terraria.ModLoader.ModItem"/> and a <see cref="Terraria.ModLoader.ModTile"/> to access the addon in-game.<br/>
	/// Textures are grabbed automatically at this filepath:<br/>
	/// <u>(name of mod)<b>/Assets/Textures/BeamAddons/</b>(name of addon file)<b>/</b>(Item for item sprite, Tile for tile sprite, Shot for shot sprite, etc.)</u><br/>
	/// but can be overriden to point to any filepath. Sounds are also stored this way, just swap Textures for Sounds.<br/><br/>
	/// Every ModBeamAddon needs an <b>AddonSlot</b>, <b>ShapePriority</b>, and <b>ColorPriority</b>.
	/// </summary>
	public abstract class ModBeamAddon : ModType
	{
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
		/// <summary>
		/// References the ModItem previously generated
		/// </summary>
		public int ItemType { get; internal set; }
		/// <summary>
		/// References the ModTile previously generated
		/// </summary>
		public int TileType { get; internal set; }

		/// <summary>
		/// The translations for the tooltip of this item.
		/// </summary>
		public virtual LocalizedText Tooltip => ModItem.GetLocalization(nameof(Tooltip), () => "");

		#region Appearance variables
		/// <summary>
		/// The filepath for the addon's item texture.
		/// </summary>
		public virtual string ItemTexture => $"{Mod.Name}/Assets/Textures/BeamAddons/{Name}/Item";
		/// <summary>
		/// the filepath for the addon's tile texture.
		/// </summary>
		public virtual string TileTexture => $"{Mod.Name}/Assets/Textures/BeamAddons/{Name}/Tile";
		/// <summary>
		/// The filepath for the addon's normal shot texture.
		/// </summary>
		public virtual string ShotTexture => $"{Mod.Name}/Assets/Textures/BeamAddons/{Name}/Shot";
		/// <summary>
		/// The amount of animation frames in the normal shot texture.
		/// </summary>
		public virtual int ShotFrames { get; } = 1;
		/// <summary>
		/// The filepath for the addon's shot sound effect.
		/// </summary>
		public virtual string ShotSound => $"{Mod.Name}/Assets/Sounds/BeamAddons/{Name}/Shot";
		/// <summary>
		/// The filepath for the addon's shot impact sound effect.
		/// </summary>
		public virtual string ImpactSound => $"{Mod.Name}/Assets/Sounds/BeamAddons/{Name}/Impact";
		/// <summary>
		/// The color of the addon's projectile.
		/// </summary>
		public abstract Color ShotColor { get; }
		/// <summary>
		/// The integer ID of the dust particles this addon's projectile will leave behind.
		/// <br/>Use <see cref="DustID"/> for vanilla dust and use <see cref="ModDust.Type"/> for modded ones.
		/// </summary>
		public abstract int ShotDust { get; }
		#endregion

		#region Visual Priority System variables
		/// <summary>
		/// Determines the level of priority of the addon's <b>shot texture</b>.<br />
		/// 0 is the lowest, 5 is the highest<br />
		/// If the addon has the <i>highest shape priority currently installed</i>, its shot graphics will be used.<br />
		/// In the case of a tie, graphics are decided by slot priority.<br/>
		/// Slot shape priority highest to lowest: Secondary(4), Spread(3), Ion(2), Ability(1), Primary(0)
		/// </summary>
		public virtual int ShapePriority { get; set; } = 0;

		/// <summary>
		/// Determines the level of priority of the addon's <b>shot color</b>.<br />
		/// 0 is the lowest, 5 is the highest<br />
		/// If the addon has the <i>highest color priority currently installed</i>, its shot color will be used.<br />
		/// In the case of a tie, color is decided by slot priority.<br />
		/// Slot color priority highest to lowest: Ability(1), Secondary(4), Ion(2), Spread(3), Primary(0)
		/// </summary>
		public virtual int ColorPriority { get; set; } =  0;
		/// <summary>
		/// If true, this addon's sounds will play instead of the sounds from the current shape priority.
		/// <br/>Requires this addon to have color priority.
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		public virtual bool SoundOverride { get; set; } = false;

		/// <summary>
		/// If true, this addon will <b>completely override</b> the visual priority system and custom firing system. <br/>
		/// Intended for use on Special Beams, like Hyper and Phazon.<br/>
		/// Checks each addon in sequential order; 1, 2, yadda yadda.<br/>
		/// Defaults to <b>false.</b><br/>
		/// <i>(stands for Very Important Beam)</i>
		/// </summary>
		public virtual bool VIB { get; set; } = false;
		#endregion

		#region Addon stat variables
		/// <summary>
		/// The slot in the Addon UI that this addon uses.<br/><br/>
		/// See <see cref="BeamAddonSlotID"/> for details on the different slots.
		/// </summary> 
		public virtual int AddonSlot { get; set; } = BeamAddonSlotID.None;

		//These stats are plugged into the WEAPON, not the projectile.
		/// <summary>
		/// The base damage value this addon adds.<br/>
		/// NOTE: Not to be confused with DamageMult, which is applied after this variable.
		/// </summary>
		public virtual int BaseDamage { get; set; } = 0;
		/// <summary>
		/// The damage multiplier value this addon adds.<br/>
		/// NOTE: Input the value as you would see it on the item's tooltip. It will be converted later.<br/>
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
		/// NOTE: Input the value as you would see it on the item's tooltip. It will be converted later.<br/>
		/// (i.e. if the addon should have a 50% speed increase, put 50f instead of 1.5f)
		/// </summary>
		public virtual float SpeedMult { get; set; } = 0f;
		/// <summary>
		/// The base velocity value this addon adds.<br/>
		/// NOTE: Not to be confused with VelocityMult, which is applied after this variable.
		/// </summary>
		public virtual float BaseVelocity { get; set; } = 0f;
		/// <summary>
		/// The velocity multiplier value this addon adds.<br/>
		/// NOTE: Input the value as you would see it on the item's tooltip. It will be converted later.<br/>
		/// (i.e. if the addon should have a 50% speed increase, put 50f instead of 1.5f)
		/// </summary>
		public virtual float VelocityMult { get; set; } = 0f;
		/// <summary>
		/// The critical strike chance this addon adds.<br/>
		/// NOTE: due to how crits work this one does NOT have a respective Mult value.
		/// </summary>
		public virtual int CritChance { get; set; } = 0;
		/// <summary>
		/// The base overheat value this addon adds.<br/>
		/// NOTE: Not to be confused with OverheatMult, which is applied after this variable.
		/// </summary>
		public virtual int BaseOverheat { get; set; } = 0;
		/// <summary>
		/// The overheat multiplier value this addon adds.<br/>
		/// NOTE: Input the value as you would see it on the item's tooltip. It will be converted later.<br/>
		/// (i.e. if the addon should have a -50% overheat multiplier, put -50f instead of 0.5f)
		/// </summary>
		public virtual float OverheatMult { get; set; } = 0f;
		/// <summary>
		/// The amount of extra projectiles this addon will make the player fire.
		/// </summary>
		public virtual int AddShots { get; set; } = 0;


		//These stats get plugged into the PROJECTILE, not the weapon.
		/// <summary>
		/// The buff that this addon will inflict on hit.
		/// </summary>
		public virtual int InflictsBuff { get; set; }
		/// <summary>
		/// The amount of extra tiles this addon allows the beam to interact with before being destroyed.
		/// <br/><br/>Example: The amount of tiles the Wave Beam allows the shot to phase through.
		/// </summary>
		public virtual int TileInteract { get; set; } = 0;
		/// <summary>
		/// The amount of extra NPCs this addon allows the beam to hit before being destroyed.
		/// </summary>
		public virtual int NPCInteract { get; set; } = 0;
		/// <summary>
		/// If true, this addon will continue to perform an action for as long as Fire is held.
		/// <br/>For advanced beam shenanigans. Assemble said shenanigans over in HoldFireBehavior().
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		public virtual bool HoldFire { get; set; } = false;
		/// <summary>
		/// If true, <b>all holdfire behavior</b> is disabled for as long as this beam is installed.
		/// <br/>Useful if you don't want your addon to be able to be charged. Leave it off if your addon has a holdfire itself.
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		public virtual bool SuppressHoldFire { get; set;} = false;
		#endregion



		/// <summary>
		/// Makes the addon in question only add the item and tile, not the beam properties.<br/>
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
			ModItem = new BeamAddonItem(this);
			ModTile = new BeamAddonTile(this);
			if (ModItem == null) { throw new Exception("WTF happened here? BeamAddonItem is null!"); }
			if (ModTile == null) { throw new Exception("WTF happened here? BeamAddonTile is null!"); }
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

		protected sealed override void Register()
		{
			if (!AddOnlyAddonItem && BeamAddonLoader.AddonCount <= 127)
			{
				Type = BeamAddonLoader.AddonCount;
				if (Type > 127)
				{
					throw new Exception("Beam Addons Limit Reached. (Max: 128)");
				}
				BeamAddonLoader.addons.Add(this);
			}
			MetroidMod.Instance.Logger.Info("Register new Beam Addon: " + FullName + ", OnlyAddonItem: " + AddOnlyAddonItem);
		}

		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[TileType] = true;
			Main.tileOreFinderPriority[Type] = 806;
			base.SetStaticDefaults();
		}

		/// <inheritdoc cref="ModItem.SetDefaults()"/>
		public virtual void SetItemDefaults(Item item) { }

		/// <inheritdoc cref="ModItem.AddRecipes"/>
		public virtual void AddRecipes() { }

		#region Advanced addon properties
		/// <summary>
		/// Allows VIB addons to completely commandeer the shot-firing process.
		/// <br/><br/>TODO: make this optional
		/// </summary>
		/// <param name="addons"></param>
		/// <returns></returns>
		public virtual void VIBOverride(Item[] addons, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
		/// <summary>
		/// Lets you make the Arm Cannon do things while Fire is held down.
		/// </summary>
		public virtual void HoldFireBehavior(Player player) { }
		/// <summary>
		/// Changes how the projectiles shot are distributed.
		/// </summary>
		/// <param name="shot"></param>
		public virtual void ModifyShotSpread(Projectile shot) { }
		/// <summary>
		/// Changes the way projectiles behave with this addon installed.
		/// </summary>
		/// <param name="shot"></param>
		public virtual void ModifyShotAI(Projectile shot) { }
		/// <summary>
		/// Changes how projectiles interact with tiles they collide with.
		/// </summary>
		/// <param name="shot"></param>
		public virtual void ModifyShotHitTile(Projectile shot) { }
		/// <summary>
		/// Changes how projectiles interact with tiles they hit.
		/// </summary>
		/// <param name="shot"></param>
		public virtual void ModifyShotHitEntity(Projectile shot) { }
		/// <summary>
		/// Changes how projectiles interact with players they hit.
		/// </summary>
		/// <param name="shot"></param>
		public virtual void ModifyShotHitPlayer(Projectile shot) { }
		/// <summary>
		/// Changes what projectiles do upon destruction.
		/// </summary>
		/// <param name="shot"></param>
		public virtual void ModifyShotKill(Projectile shot) { }
		/// <summary>
		/// Allows this addon to detect if specific addons are installed with it and use unique visuals to accomodate.
		/// <br/>Requires this addon to have shape priority.
		/// <br/><br/>Should return a <b>blank string</b> if a special combo is not selected.
		/// </summary>
		/// <param name="addons"></param>
		/// <returns></returns>
		public virtual string SpecialComboSet(Item[] addons) { return ""; }
		/// <summary>
		/// Defines special properties the beam shot will undertake with special combos defined in <see cref="SpecialComboSet()"/> (i.e. frame count).
		/// <br/>Requires that special combos be defined.
		/// <br/><br/>Should return all <b>zeroes</b> if a special combo is not identified.
		/// </summary>
		/// <param name="modifier"></param>
		/// <returns></returns>
		public virtual int[] SpecialComboGet(string modifier) { return [0]; }
		#endregion

		public virtual bool ShowTileHover(Player player) => player.InInteractionRange(Player.tileTargetX, Player.tileTargetY, default);
		/// <inheritdoc cref="ModTile.CanKillTile(int, int, ref bool)"/>
		public virtual bool CanKillTile(int i, int j) { return true; }
		/// <inheritdoc cref="ModMBAddon.CanExplodeTile(int, int)"/>
		public virtual bool CanExplodeTile(int i, int j) { return true; }
	}
}
