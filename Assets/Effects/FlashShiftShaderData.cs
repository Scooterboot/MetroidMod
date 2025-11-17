using MetroidMod.Common.Players;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace MetroidMod.Assets.Effecs
{
	public class FlashShiftShaderData : ArmorShaderData
	{

		private static bool isInitialized;

		private static ArmorShaderData dustShaderData;

		public FlashShiftShaderData(Ref<Effect> shader, string passName) : base(shader, passName)
		{
			if (!isInitialized)
			{
				isInitialized = true;
				dustShaderData = new ArmorShaderData(shader, passName).UseOpacity(1);
			}
		}
		public override void Apply(Entity entity, DrawData? drawData)
		{
			Player player = entity as Player;
			if (entity != null && entity is Projectile)
			{
				Projectile p = entity as Projectile;
				if (p != null)
					player = Main.player[p.owner];
			}
			if (player == null)
			{
				return;
			}
			float o = 1f;
			o = player.GetModPlayer<MPlayer>().flashShiftTime / (float)player.GetModPlayer<MPlayer>().flashShiftLength;

			float s = o;
			if (o <= 0.2 && player.GetModPlayer<MPlayer>().flashShiftGlow)
			{
				o = 0.2f;
				s = 0.4f;
			}
			base.UseColor(player.GetModPlayer<MPlayer>().flashShiftColor);
			base.UseOpacity(o);
			base.UseSaturation(s);
			base.Apply(player, drawData);
		}

		public override ArmorShaderData GetSecondaryShader(Entity entity)
		{
			Player player = entity as Player;
			if (player == null)
			{
				return dustShaderData;
			}
			return dustShaderData;
		}
	}
}
