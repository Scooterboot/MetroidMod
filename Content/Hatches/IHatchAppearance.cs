using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches
{
	public interface IHatchAppearance
	{
		void Update();
		string GetTexturePath(bool vertical);
	}

	internal class HatchAppearance(string name) : IHatchAppearance
	{
		public void Update() { }
		public string GetTexturePath(bool vertical)
		{
			return $"Content/Hatches/Variants/{name}{(vertical ? "Vertical" : string.Empty)}Door";
		}
	}

	internal class HatchBlinkingAppearance(IHatchAppearance main, IHatchAppearance alt) : IHatchAppearance
	{
		private bool useAlt;
		private int counter;

		public void Update()
		{
			if (counter++ >= 10)
			{
				counter = 0;
				useAlt = !useAlt;
			}
		}

		public string GetTexturePath(bool vertical)
		{
			return (useAlt ? alt : main).GetTexturePath(vertical);
		}
	}
}
