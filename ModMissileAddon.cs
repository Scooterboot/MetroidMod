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
		public virtual string ItemTexture => $"{Mod.Name}/Assets/Textures/MissileAddons/{Name}/Item";
		public virtual string TileTexture => $"{Mod.Name}/Assets/Textures/MissileAddons/{Name}/Tile";
		public virtual int TileFrames { get; } = 1;
		public virtual string ShotTexture => $"{Mod.Name}/Assets/Textures/MissileAddons/{Name}/Shot";
		public virtual int ShotFrames { get; } = 1;
		public virtual string ShotSound => $"{Mod.Name}/Assets/Sounds/MissileAddons/{Name}/Shot";
		public virtual string ImpactSound => $"{Mod.Name}/Assets/Sounds/MissileAddons/{Name}/Impact";
		/// <summary>
		/// Extension of <see cref="AI(MProjectile)"/> that only runs when the addon has Shape Priority.
		/// <br/>Example: Ice Beam uses this to make its projectile rotate.
		/// </summary>
		/// <param name="shot"></param>
		public virtual void ShapeBehavior(MProjectile shot) { }
		/// <summary>
		/// The buff that this addon will inflict on hit.
		/// </summary>
		public virtual int InflictsBuff { get; set; }

		public virtual LocalizedText Tooltip => ModItem.GetLocalization(nameof(Tooltip), () => "");
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
		/// The integer ID of the dust particles this addon's projectile will leave behind.
		/// <br/>Use <see cref="DustID"/> for vanilla dust and use <see cref="ModDust.Type"/> for modded ones.
		/// </summary>
		public abstract int ShotDust { get; }

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
		/// Used to instantiate copies of beam addons.
		/// <br/> Useful for being able to have more than one beam shot active at once.
		/// </summary>
		/// <returns></returns>
		public ModMissileAddon Clone()
		{
			return (ModMissileAddon)this.MemberwiseClone();
		}
		/// <summary>
		/// The Beam Addon that must be installed in order to use the addon.
		/// <br/><br/>Intended for use with <b>Charge Combos</b>.
		/// </summary>
		public virtual ModMissileAddon RequiredMissile { get; set; }
		#endregion

		public abstract bool AddOnlyAddonItem { get; }


		public override sealed void SetupContent()
		{
			//Textures = new Asset<Texture2D>[4];
			SetStaticDefaults();
		}
		public override void Load()
		{
			//ModMissileAddons automatically generate their items and tiles on load, based on the MissileAddonItem and MissileAddonTile templates.
			//This is the code that facilitates this.

			//Assigns a new M.A.I. and M.A.T. instance to the current M.M.A.
			ModItem = new MissileAddonItem(this);
			ModTile = new MissileAddonTile(this);
			if (ModItem == null) { throw new Exception("WTF happened here? MissileAddonItem is null!"); }
			if (ModTile == null) { throw new Exception("WTF happened here? MissileAddonTile is null!"); }
			//Adds the content to the game.
			Mod.AddContent(ModItem);
			Mod.AddContent(ModTile);
			//Assigns the Type values to the appropriate fields.
			//If you forget this part, you can't call the addons through their Type, which breaks things like Shimmer recipes.
			ItemType = ModItem.Type;
			TileType = ModTile.Type;
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
		/// <summary>
		/// The amount of extra tiles this addon allows the beam to interact with before being destroyed.
		/// <br/><br/>Example: The amount of tiles the Wave Beam allows the shot to phase through.
		/// </summary>
		public virtual int TileInteract { get; set; } = 0;
		/// <summary>
		/// The amount of extra NPCs this addon allows the beam to hit before being destroyed.
		/// </summary>
		public virtual int EntityInteract { get; set; } = 0;

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
	}
}
