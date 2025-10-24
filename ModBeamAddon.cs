using System;
using System.Collections.Generic;
using MetroidMod.Content.Projectiles;
using MetroidMod.Default;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

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
		/// <summary>
		/// References the ModItem previously generated
		/// </summary>
		public int ItemType { get; internal set; }
		/// <summary>
		/// References the ModTile previously generated
		/// </summary>
		public int TileType { get; internal set; }
		#endregion

		/// <summary>
		/// The translations for the tooltip of this item.
		/// </summary>
		public virtual LocalizedText Tooltip => ModItem.GetLocalization(nameof(Tooltip), () => "");

		/// <summary>
		/// The slot in the Addon UI that this addon uses.<br/><br/>
		/// See <see cref="BeamAddonSlotID"/> for details on the different slots.
		/// </summary> 
		public virtual int AddonSlot { get; set; } = BeamAddonSlotID.None;


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
		/// The primary color of the addon's projectile.
		/// </summary>
		public abstract Color PrimaryColor { get; }
		/// <summary>
		/// The secondary color of the addon's projectile, used for dark shading.
		/// <br/>For example, Ice Beam's secondary color is dark blue.
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
		/// Sets to true when this addon has shape priority.
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		//public bool ShapePrioritized = false;
		/// <summary>
		/// Determines the level of priority of the addon's <b>shot color</b>.<br />
		/// 0 is the lowest, 5 is the highest<br />
		/// If the addon has the <i>highest color priority currently installed</i>, its shot color will be used.<br />
		/// In the case of a tie, color is decided by slot priority.<br />
		/// Slot color priority highest to lowest: Ability(1), Secondary(4), Ion(2), Spread(3), Primary(0)
		/// </summary>
		public virtual int ColorPriority { get; set; } = 0;
		/// <summary>
		/// Sets to true when this addon has color priority.
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		//public bool ColorPrioritized = false;
		/// <summary>
		/// If true, this addon's sounds will play instead of the sounds from the current shape priority.
		/// <br/>Requires this addon to have color priority.
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		public virtual bool SoundOverride { get; set; } = false;

		/// <summary>
		/// If true, this addon will <b>completely override</b> the visual priority system. <br/>
		/// Making an addon a VIB also allows you to <b>create your own custom projectile</b> for your addon's shot, if you so choose. <br/>
		/// If not, you can simply leave <see cref="vibOverride"/> as null. <br/>
		/// Intended for use on Special Beams, such as the <b>Hyper Beam</b> and <b>Phazon Beam</b>.<br/>
		/// Checks each addon in sequential order; slot 0, slot 1, yadda yadda.<br/>
		/// Defaults to <b>false.</b><br/>
		/// <i>(stands for Very Important Beam)</i>
		/// </summary>
		public virtual bool VIB { get; set; } = false;
		/// <summary>
		/// Determines the custom projectile the VIB will fire instead of the standard beam shot.
		/// <br/>Leave at -1 to use the standard beam shot projectile.
		/// <br/><b>For advanced use ONLY. Not recommended for beginners.</b>
		/// </summary>
		public int vibOverride = -1;
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
		/// The base velocity value this addon adds.<br/>
		/// NOTE: Not to be confused with VelocityMult, which is applied after this variable.
		/// </summary>
		public virtual float BaseVelocity { get; set; } = 0f;
		/// <summary>
		/// The velocity multiplier value this addon adds.<br/>
		/// NOTE: Input the value as you would see it on the item's <i>tooltip</i>. It will be converted later.<br/>
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
		/// NOTE: Input the value as you would see it on the item's <i>tooltip</i>. It will be converted later.<br/>
		/// (i.e. if the addon should have a -50% overheat multiplier, put -50f instead of 0.5f)
		/// </summary>
		public virtual float OverheatMult { get; set; } = 0f;
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
		/// The amount of extra tiles this addon allows the beam to interact with before being destroyed.
		/// <br/><br/>Example: The amount of tiles the Wave Beam allows the shot to phase through.
		/// </summary>
		public virtual int TileInteract { get; set; } = 0;
		/// <summary>
		/// The amount of extra NPCs this addon allows the beam to hit before being destroyed.
		/// </summary>
		public virtual int EntityInteract { get; set; } = 0;
		/// <summary>
		/// If true, this addon will continue to perform an action for as long as Fire is held.
		/// <br/>For advanced beam shenanigans. Assemble said shenanigans over in <see cref="HoldFireBehavior(Player)"/>.
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		public virtual bool HoldFire { get; set; } = false;
		/// <summary>
		/// If true, any base stat changes this addon applies will continue to apply within the array.
		/// <br/>Defaults to <b>false</b>.
		/// </summary>
		public virtual bool ArrayPassive { get; set; } = false;
		/// <summary>
		/// If true, this addon's HoldFire functionality can still be accessed if it is in the array.
		/// <br/>Defaults to <b>True</b>.
		/// </summary>
		public virtual bool HoldfirePassive { get; set; } = true;

		//Compatibility-related variables
		/// <summary>
		/// If true, <b>all holdfire behavior</b> is disabled for as long as this beam is installed.
		/// <br/>Useful if you don't want your addon to be able to be charged. Leave it off if your addon has a holdfire itself.
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		public virtual bool SuppressHoldFire { get; set; } = false;
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

		public virtual List<string> Overrides => new();
		#endregion

		#region Data-handling methods
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
			//ModBeamAddons automatically generate their items and tiles on load, based on the BeamAddonItem and BeamAddonTile templates.
			//This is the code that facilitates this.

			//Assigns a new B.A.I. and B.A.T. instance to the current M.B.A.
			ModItem = new BeamAddonItem(this);
			ModTile = new BeamAddonTile(this);
			if (ModItem == null) { throw new Exception("WTF happened here? BeamAddonItem is null!"); }
			if (ModTile == null) { throw new Exception("WTF happened here? BeamAddonTile is null!"); }
			//Adds the content to the game.
			Mod.AddContent(ModItem);
			Mod.AddContent(ModTile);
			//Assigns the Type values to the appropriate fields.
			//If you forget this part, you can't call the addons through their Type, which breaks things like Shimmer recipes.
			ItemType = ModItem.Type;
			TileType = ModTile.Type;

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
			if (!AddOnlyAddonItem && BeamAddonLoader.AddonCount <= 127)
			{
				Type = BeamAddonLoader.AddonCount;
				if (Type > 127)
				{
					throw new Exception("Beam Addons Limit Reached. (Max: 128)");
				}
				BeamAddonLoader.addons.Add(this);
			}
			MetroidMod.Instance.Logger.Info("Register new Beam Addon: " + FullName + ", OnlyAddonItem: " + AddOnlyAddonItem + ", Visual Priority: " + ShapePriority + "S/" + ColorPriority + "C, VIB: " + VIB);
		}
		/// <summary>
		/// Used to instantiate copies of beam addons.
		/// <br/> Useful for being able to have more than one beam shot active at once.
		/// </summary>
		/// <returns></returns>
		public ModBeamAddon Clone()
		{
			return (ModBeamAddon)this.MemberwiseClone();
		}
		#endregion

		#region ModItem fields
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[TileType] = true;
			Main.tileOreFinderPriority[Type] = 806;
			base.SetStaticDefaults();
		}

		/// <inheritdoc cref="ModItem.SetDefaults()"/>
		public virtual void SetItemDefaults(Item item) { }

		/// <inheritdoc cref="ModItem.PostDrawInInventory(SpriteBatch, Vector2, Rectangle, Color, Color, Vector2, float)"/>
		public virtual void PostDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) { }

		///<inheritdoc cref="ModItem.PostDrawInWorld(SpriteBatch, Color, Color, float, float, int)"/>
		public virtual void PostDrawInWorld(Item item, SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) { }

		///<inheritdoc cref="ModItem.RightClick(Player)"/>
		public virtual void RightClick(Player player) { }
		/// <inheritdoc cref="ModItem.AddRecipes"/>
		public virtual void AddRecipes() { }
		public Recipe CreateRecipe(int amount = 1) => ModItem.CreateRecipe(amount);

		#endregion

		#region Advanced addon properties
		/// <summary>
		/// Allows this addon to define <b>static combos</b>, allowing for specific addon combinations to have unique properties.
		/// <br/>Each static combo needs a corresponding keyword, which the method will return. <b>Keywords must not contain spaces.</b>
		/// <br/>An addon's static combos will only trigger if it has shape priority.
		/// <br/>To apply special data to a combo (such as animation frame count), use <see cref="ComboVisualsGet(string)"/>.
		/// <br/><br/>Should return a <b>blank string</b> if a static combo is not selected.
		/// </summary>
		/// <param name="addons"></param>
		/// <returns></returns>
		public virtual string SetStaticCombos(Item[] addons) { return ""; }
		/// <summary>
		/// Defines special visual properties the beam shot will undertake when certain combos are detected.
		/// <br/>This can include <b>static combos</b> defined in <see cref="SetStaticCombos(Item[])"/>, <b>dynamic combos</b> applied at the time of firing (such as "Charged"), as well as combinations of both.
		/// <br/><b>ReturnValue[0]</b>: Combo texture's animation framecount. Count starts at <b>1</b>.
		/// <br/><b>ReturnValue[1]</b>: Combo's unique dust ID, if any. <b>-1</b> enables default dust-grabbing behavior and <b>-2</b> disables default dust generation entirely.
		/// <br/><br/>Should return <b>[1, -1]</b> if a special combo is not identified.
		/// </summary>
		/// <param name="modifier"></param>
		/// <returns></returns>
		public virtual int[] ComboVisualsGet(string modifier) { return [1, -1]; }
		//Dynamic combos are applied on the shot's firing.
		//The best example of this would be charged shots, for which the keyword is "Charged".

		/// <summary>
		/// Allows beam addons to apply some <i>highly</i> specific edge-case values in edge-case scenarios.
		/// <br/>Example: The Wave Beam uses this method to spawn a second projectile on a charged shot without any other extra projectiles.
		/// <br/>Array values are as follows:
		/// <br/>[0]: Damage Multiplier
		/// <br/>[1]: Speed Multiplier
		/// <br/>[2]: Velocity Multiplier
		/// <br/>[3]: Overheat Multiplier
		/// <br/>[4]: Add Shots
		/// </summary>
		/// <param name="addons"></param>
		/// <param name="statVals"></param>
		/// <param name="bonusMod"></param>
		/// <returns></returns>
		public virtual float[] EdgeCaseData(ModBeamAddon[] addons, float[] statVals, string bonusMod) { return [0, 0, 0, 0, 0]; }

		/// <summary>
		/// Lets you make the Arm Cannon do things while Fire is held down.
		/// <br/>To be used with <see cref="HoldFire"/>.
		/// </summary>
		public virtual void HoldFireBehavior(Player player) { }
		/// <summary>
		/// Extension of <see cref="AI(MProjectile)"/> that only runs when the addon has Shape Priority.
		/// <br/>Example: Ice Beam uses this to make its projectile rotate.
		/// </summary>
		/// <param name="shot"></param>
		public virtual void ShapeBehavior(MProjectile shot) { }
		/// <summary>
		/// Allows VIBs with custom projectiles to take over the firing logic.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="source"></param>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		/// <param name="type"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		public virtual void VIBShoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, string bonusFileMod = "", float multiplier = 1f) { }
		//TODO: Make this return a bool? Make it like a preAI() type deal?
		//Too drained to figure this shit out rn		-Z

		#region Projectile behavior injectors
		//These methods are designed to line up with the ones inside of ModProjectiles, allowing for ModProjectile code to be injected into beam shots.
		//Making them check for an MProjectile as opposed to a standard Projectile makes it easier to use beam-specific variables.


		///<summary> Gets called when your projectile spawns in world.
		///<br/><br/>...except it's not <i>technically</i> on spawn since onboarding addons happens after the projectile spawns, so uh...
		///<br/>Let's just say 'yes' and pretend a 'yes', because it might as well be, but acknowledge that... also... 'no'??</summary>
		public virtual void OnSpawn(MProjectile mpshot, IEntitySource source) { }
		/// <inheritdoc cref="ModProjectile.PreAI"/>
		public virtual bool PreAI(MProjectile mpshot) { return true; }
		/// <inheritdoc cref="ModProjectile.AI"/>
		public virtual void AI(MProjectile mpshot) { }
		/// /// <inheritdoc cref="ModProjectile.PostAI"/>
		public virtual void PostAI(MProjectile mpshot) { }
		/// <inheritdoc cref="ModProjectile.OnHitNPC(NPC, NPC.HitInfo, int)"/>
		public virtual void OnHitNPC(MProjectile mpshot, NPC target, NPC.HitInfo hit, int damageDone) { }
		/// <inheritdoc cref="ModProjectile.OnHitPlayer(Player, Player.HurtInfo)"/>
		public virtual void OnHitPlayer(MProjectile mpshot, Player target, Player.HurtInfo info) { }
		/// <inheritdoc cref="ModProjectile.TileCollideStyle(ref int, ref int, ref bool, ref Vector2)"/>
		public virtual bool TileCollideStyle(MProjectile mpshot, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) { return true; }
		/// <inheritdoc cref="ModProjectile.OnTileCollide(Vector2)"/>
		public virtual bool OnTileCollide(MProjectile mpshot, Vector2 oldVelocity) { return true; }
		/// <inheritdoc cref="ModProjectile.OnKill(int)"/>
		public virtual void OnKill(MProjectile mpshot, int timeLeft) { }
		#endregion

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

		///<inheritdoc cref="ModTile.PostDrawPlacementPreview(int, int, SpriteBatch, Rectangle, Vector2, Color, bool, SpriteEffects)"/>
		public virtual void PostDrawPlacementPreview(int i, int j, SpriteBatch spriteBatch, Rectangle frame, Vector2 position, Color color, bool validPlacement, SpriteEffects spriteEffects) { }
		#endregion
	}
}
