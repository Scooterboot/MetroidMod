using MetroidMod.Assets.Effecs;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Aeion
{
	public class FlashShiftShader : ModItem
	{
		public override void SetStaticDefaults()
		{
			if (!Main.dedServ)
			{

				GameShaders.Armor.BindShader(
					Item.type,
					new FlashShiftShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Assets/Effects/FlashShiftShader", AssetRequestMode.ImmediateLoad).Value), "FlashShaderPass").UseColor(0f, 1f, 1f).UseSecondaryColor(1f, 1f, 1f) // Be sure to update the effect path and pass name here.
				);
			}
		}

		public override void SetDefaults()
		{
			// Item.dye will already be assigned to this item prior to SetDefaults because of the above GameShaders.Armor.BindShader code in Load().
			// This code here remembers Item.dye so that information isn't lost during CloneDefaults.
			int dye = Item.dye;

			Item.CloneDefaults(ItemID.GelDye);

			Item.dye = dye;
		}
	}
}
