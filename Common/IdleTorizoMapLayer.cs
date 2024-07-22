using MetroidMod.Common.Systems;
using MetroidMod.Content.NPCs.GoldenTorizo;
using MetroidMod.Content.NPCs.Torizo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;
using static System.Net.Mime.MediaTypeNames;

namespace MetroidMod.Common
{
	public class IdleTorizoMapLayer : ModMapLayer
	{
		private const float scaleIfNotSelected = 1f;
		private const float scaleIfSelected = scaleIfNotSelected * 1.2f;
		protected virtual TorizoSpawningSystem System => ModContent.GetInstance<TorizoSpawningSystem>();

		public override void Draw(ref MapOverlayDrawContext context, ref string text)
		{
			if (!System.CanShowIcon())
			{
				return;
			}

			string texturePath = $"{nameof(MetroidMod)}/Content/NPCs/Torizo/IdleTorizo_Head";
			Texture2D torizoHeadTexture = ModContent.Request<Texture2D>(texturePath, AssetRequestMode.ImmediateLoad).Value;

			var result = context.Draw(
				torizoHeadTexture,
				System.HeadLocation.ToTileCoordinates().ToVector2(),
				Color.White,
				new SpriteFrame(1, 1, 0, 0),
				scaleIfNotSelected,
				scaleIfSelected,
				Alignment.Center);

			if (result.IsMouseOver)
			{
				text = "???";
			}
		}
	}

	public class IdleGoldenTorizoMapLayer : IdleTorizoMapLayer
	{
		protected override TorizoSpawningSystem System => ModContent.GetInstance<GoldenTorizoSpawningSystem>();
	}
}
