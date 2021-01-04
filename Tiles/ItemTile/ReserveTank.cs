using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
    public class ReserveTank : ItemTile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Reserve Tank");
            AddMapEntry(new Color(113, 130, 146), name);
            drop = mod.ItemType("ReserveTank");
            dustType = 1;
		}
	}
}