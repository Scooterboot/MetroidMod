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
	public abstract class GlobalMBAddon : ModType
	{
		protected override sealed void Register()
		{
			MBAddonLoader.globalAddons.Add(this);
		}
		public override sealed void SetupContent() => SetStaticDefaults();

		public override void SetStaticDefaults() => base.SetStaticDefaults();
	}
}
