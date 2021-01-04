#region Using directives

using System;

using Terraria;
using Terraria.ModLoader;

using MonoMod.Cil;
using Mono.Cecil.Cil;

using MetroidMod.Items.weapons;
using MetroidMod.Items.accessories;

#endregion

namespace MetroidMod.Items
{
	internal sealed class ItemIL
	{
		public static void Load()
		{
			IL.Terraria.Player.ItemFitsItemFrame += ItemFitsItemFrame_DisableForMetroidItems;
		}

		private static void ItemFitsItemFrame_DisableForMetroidItems(ILContext il)
		{
			var c = new ILCursor(il);

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchCgt()))
			{
				return;
			}

			c.Emit(OpCodes.Ldarg_1);

			c.EmitDelegate<Func<bool, Item, bool>>((stackCheck, item) =>
			{					
				if (item.type == ModContent.ItemType<MorphBall>() || 
					item.type == ModContent.ItemType<PowerBeam>() ||
					item.type == ModContent.ItemType<MissileLauncher>())
				{
					return (false);
				}

				return (stackCheck);
			});
		}
	}
}
