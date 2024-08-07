using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Switches
{
	[Autoload(false)]
	public class ModBubbleSwitchItem(ModBubbleSwitch bubbleSwitch) : ModItem
	{
		public override string Name => bubbleSwitch.Name;
		public override string Texture => $"{nameof(MetroidMod)}/Content/Switches/Variants/{Name}Item";
		protected override bool CloneNewInstances => true;
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 5;
		}
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = bubbleSwitch.Tile.Type;
		}
	}
}
