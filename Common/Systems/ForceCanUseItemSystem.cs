using MetroidMod.Content.Items.Tools;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Common.Systems
{
	internal class ForceCanUseItemSystem : ModSystem
	{
		public override void Load()
		{
			IL_Player.ItemCheck_CheckCanUse += HookCheckCanUse;
		}

		private void HookCheckCanUse(ILContext il)
		{
			ILCursor c = new(il);

			c.EmitLdarg0();
			c.EmitLdarg1();

			c.EmitDelegate((Player player, Item item) =>
			{
				// The item uses ammo, sure, but it can still do stuff without it...
				// Ideally we would avoid IL editing, but rn it's the quickest approach I could find
				
				if(item.type == ModContent.ItemType<ChoziteDualtool>())
				{
					return true;
				}

				return false;
			});

			ILLabel postReturn = c.DefineLabel();
			c.EmitBrfalse(postReturn);
			c.EmitLdcI4(1);
			c.EmitRet();
			c.MarkLabel(postReturn);
		}
	}
}
