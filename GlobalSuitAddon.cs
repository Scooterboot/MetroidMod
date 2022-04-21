using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidModPorted
{
	public abstract class GlobalSuitAddon : ModType
	{
		protected override void Register()
		{
			SuitAddonLoader.globalAddons.Add(this);
		}
	}
}
