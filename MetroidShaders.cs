using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using ReLogic.Content;

namespace MetroidMod
{
	public static class MetroidShaders
	{
		public const string path = "Assets/Effects/";

		internal const string prefix = "MetroidMod:";

		public static Effect DarkVisorEffect;

		// methods from calamity mod (thanks lol)
		private static void RegisterMiscShader(Effect shader, string passName, string registrationName)
		{
			MiscShaderData passParamRegistration = new MiscShaderData(new Ref<Effect>(shader), passName);
			GameShaders.Misc[prefix + registrationName] = passParamRegistration;
		}

		private static void RegisterSceneFilter(ScreenShaderData passReg, string registrationName, EffectPriority priority = EffectPriority.High)
		{
			string prefixedRegistrationName = prefix + registrationName;
			Filters.Scene[prefixedRegistrationName] = new Filter(passReg, priority);
			Filters.Scene[prefixedRegistrationName].Load();
		}

		private static void RegisterScreenShader(Effect shader, string passName, string registrationName, EffectPriority priority = EffectPriority.High)
		{
			RegisterSceneFilter(new ScreenShaderData(new Ref<Effect>(shader), passName), registrationName, priority);
		}

		public static void LoadShaders()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				DarkVisorEffect = MetroidMod.Instance.Assets.Request<Effect>(path + "DarkVisorEffect", AssetRequestMode.ImmediateLoad).Value;
				RegisterScreenShader(DarkVisorEffect, "DarkVisorEffect", "DarkVisor", EffectPriority.High);
			}
		}
	}
}
