using Terraria.ModLoader;

namespace MetroidMod
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
