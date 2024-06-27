#region Using directives

using System;
using MetroidMod.Content.Items.Accessories;
using MetroidMod.Content.Items.Weapons;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

#endregion

namespace MetroidMod.Content.Items
{
	internal sealed class ItemIL
	{
		public static void Load()
		{
			Terraria.IL_Player.ItemFitsItemFrame += ItemFitsItemFrame_DisableForMetroidItems;
			//IL.Terraria.UI.ItemSlot.PickItemMovementAction += PickItemMovementAction_NoSuitAddons;
		}

		public static void Unload()
		{
			Terraria.IL_Player.ItemFitsItemFrame -= ItemFitsItemFrame_DisableForMetroidItems;
			//IL.Terraria.UI.ItemSlot.PickItemMovementAction -= PickItemMovementAction_NoSuitAddons;
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
					item.type == ModContent.ItemType<ArmCannon>() ||
					item.type == ModContent.ItemType<MissileLauncher>())
				{
					return (false);
				}

				return (stackCheck);
			});
		}
		/*private static void PickItemMovementAction_NoSuitAddons(ILContext il)
		{
			var c = new ILCursor(il);

			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(30)))
			{
				return;
			}

			//c.Emit(OpCodes.Ldarg_1);

			c.EmitDelegate<Func<int, Item[], int, int, Item, int>>((returnValue, inv, context, slot, checkItem) =>
			{
				if (SuitAddonLoader.TryGetAddon(checkItem, out _)) { return -1; }
				return returnValue;
			});
		}*/
	}
}
