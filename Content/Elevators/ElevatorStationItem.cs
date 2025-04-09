using Terraria.ModLoader;

namespace MetroidMod.Content.Elevators
{
	internal class ElevatorStationItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<ElevatorStationTile>());
		}
	}
}
