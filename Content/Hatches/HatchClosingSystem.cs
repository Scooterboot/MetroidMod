using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches
{
	/// <summary>
	/// This system focuses on ensuring that hatches that should be closed, do as such
	/// as soon as it is possible for them. This can not happen immediately when the hatch
	/// is obstructed, by a player or another entity.
	/// 
	/// This code is put in its own ModSystem to ensure it runs both on clients and servers
	/// (TileEntity.Update only runs on server for some reason. There could be other ways
	/// to work around this, but for now this is what we went with)
	/// 
	/// Currently, hatch closing is only synced in intent (i.e. we indicate that a hatch
	/// wants to close, but we don't actually close it until it ensures there's nothing in the way)
	/// </summary>
	internal class HatchClosingSystem : ModSystem
	{
		public override void PostUpdateEverything()
		{
			foreach(HatchTileEntity hatch in HatchTileEntity.GetAll())
			{
				if(hatch.NeedsToClose && CanClose(hatch.Position))
				{
					hatch.PhysicallyClose();
				}
			}
		}

		private bool CanClose(Point16 position)
		{
			for (int j = 0; j < 4; j++)
			{
				for (int i = 0; i < 4; i++)
				{
					if (!Collision.EmptyTile(position.X + i, position.Y + j, true))
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}
