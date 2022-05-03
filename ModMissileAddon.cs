using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidModPorted
{
	public abstract class ModMissileAddon : ModType
	{
		public int Type { get; private set; }
		internal void ChangeType(int type) => Type = type;
		public int ItemType { get; internal set; }
		public int TileType { get; internal set; }

		public ModTranslation DisplayName { get; internal set; }
		public abstract string PowerBeamTexture { get; }

		public abstract bool AddOnlyMissileItem { get; }


		public override sealed void SetupContent()
		{
			//Textures = new Asset<Texture2D>[4];
			SetStaticDefaults();
		}
		protected override sealed void Register()
		{
			if (!AddOnlyMissileItem)
			{
				Type = MissileLauncherLoader.MissileCount;
				if (Type > 127)
				{
					throw new Exception("Missile Limit Reached. (Max: 128)");
				}
				MissileLauncherLoader.missileAddons.Add(this);
			}
			//Mod.AddContent(new MissileItem(this));
			//Mod.AddContent(new MissileTile(this));
			MetroidModPorted.Instance.Logger.Info("Register new Missile: " + FullName + ", OnlyMissileItem: " + AddOnlyMissileItem);
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
}
