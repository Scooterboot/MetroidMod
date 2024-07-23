using MetroidMod.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class TorizoRelocator : ModItem
	{
		public virtual TorizoSpawningSystem System => ModContent.GetInstance<TorizoSpawningSystem>();
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.LifeCrystal);
			Item.consumable = false;
			Item.maxStack = 1;
		}

		public override bool? UseItem(Player player)
		{
			Vector2 position = player.Bottom.ToTileCoordinates().ToWorldCoordinates(0, 0);
			int direction = player.direction;
			System.UpdateLocation(position, position, direction);
			return true;
		}
	}

	public class GoldenTorizoRelocator : TorizoRelocator
	{
		public override TorizoSpawningSystem System => ModContent.GetInstance<GoldenTorizoSpawningSystem>();
	}
}
