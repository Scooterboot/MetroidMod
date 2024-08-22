using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace MetroidMod.Content.Tiles2
{
	public abstract class GenericTile : ModType
	{
		public BaseItem Item;
		public BaseTile Tile;

		public virtual string ItemTexture => (GetType().Namespace + ".Item." + Name).Replace('.', '/');
		public virtual string TileTexture => (GetType().Namespace + ".Tile" + Name).Replace('.', '/');

		public abstract Color MapColor { get; }
		public abstract SoundStyle HitSound { get; }
		public abstract int DustType { get; }

		protected override sealed void Register()
		{
			Item = new(this);
			Mod.AddContent(Item);

			Tile = new(this);
			Mod.AddContent(Tile);
		}

		/// <summary>
		/// Allows customizing defaults that are specific to each tile.
		/// </summary>
		public virtual void SetExtraTileDefaults()
		{

		}

		public class BaseItem(GenericTile tile) : ModItem
		{
			public override string Name => tile.Name;
			public override string Texture => tile.ItemTexture;
			protected override bool CloneNewInstances => true;

			public override void SetStaticDefaults()
			{
				Item.ResearchUnlockCount = 100;
			}
			public override void SetDefaults()
			{
				Item.width = 16;
				Item.height = 16;
				Item.maxStack = 9999;
				Item.useTurn = true;
				Item.autoReuse = true;
				Item.useAnimation = 15;
				Item.useTime = 10;
				Item.useStyle = ItemUseStyleID.Swing;
				Item.consumable = true;
				Item.createTile = tile.Tile.Type;
			}
		}

		public class BaseTile(GenericTile tile) : ModTile
		{
			public override string Name => tile.Name;
			public override string Texture => tile.TileTexture;
			public override void SetStaticDefaults()
			{
				Main.tileSolid[Type] = true;
				Main.tileBlendAll[Type] = true;
				Main.tileMergeDirt[Type] = true;
				Main.tileBlockLight[Type] = true;

				HitSound = tile.HitSound;
				DustType = tile.DustType;
				MinPick = 35;

				AddMapEntry(tile.MapColor);

				tile.SetExtraTileDefaults();
			}

			// If i left this in, i forgot to remove it bruh
			public override bool RightClick(int i, int j)
			{
				SetStaticDefaults();
				return true;
			}
		}
	}
}
