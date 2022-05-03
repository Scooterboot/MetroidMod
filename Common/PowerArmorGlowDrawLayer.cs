using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using MetroidModPorted.Common.GlobalItems;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Common
{
	internal class DrawDataInfo
	{
		public Vector2 Position;
		public Rectangle? Frame;
		public float Rotation;
		public Texture2D Texture;
		public Vector2 Origin;
	}
	internal abstract class PowerArmorDrawLayer : PlayerDrawLayer
	{
		protected static int? ShaderId;

		public abstract DrawDataInfo GetData(PlayerDrawSet info);

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.shadow == 0f && !drawInfo.drawPlayer.invis;// && drawInfo.drawPlayer.GetModPlayer<MPlayer>().isPowerSuit;

		public static DrawDataInfo GetHeadDrawDataInfo(PlayerDrawSet drawInfo, Texture2D texture)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			Vector2 pos = drawPlayer.headPosition + drawInfo.headVect + new Vector2(
				(int)(drawInfo.Position.X + drawPlayer.width / 2f - drawPlayer.bodyFrame.Width / 2f - Main.screenPosition.X),
				(int)(drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4f - Main.screenPosition.Y)
			);

			return new DrawDataInfo
			{
				Position = pos,
				Frame = drawPlayer.bodyFrame,
				Origin = drawInfo.headVect,
				Rotation = drawPlayer.headRotation,
				Texture = texture
			};
		}

		public static DrawDataInfo GetBodyDrawDataInfo(PlayerDrawSet drawInfo, Texture2D texture)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			Vector2 pos = drawPlayer.bodyPosition + drawInfo.bodyVect + new Vector2(
				 (int)(drawInfo.Position.X - Main.screenPosition.X - drawPlayer.bodyFrame.Width / 2f + drawPlayer.width / 2f),
				 (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4f)
			);

			return new DrawDataInfo
			{
				Position = pos,
				Frame = drawPlayer.bodyFrame,
				Origin = drawInfo.bodyVect,
				Rotation = drawPlayer.bodyRotation,
				Texture = texture
			};
		}

		public static DrawDataInfo GetLegDrawDataInfo(PlayerDrawSet drawInfo, Texture2D texture)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			Vector2 pos = drawPlayer.legPosition + drawInfo.legVect + new Vector2(
				(int)(drawInfo.Position.X - Main.screenPosition.X - drawPlayer.legFrame.Width / 2f + drawPlayer.width / 2f),
				(int)(drawInfo.Position.Y - Main.screenPosition.Y + drawPlayer.height - drawPlayer.legFrame.Height + 4f)
			);

			return new DrawDataInfo
			{
				Position = pos,
				Frame = drawPlayer.legFrame,
				Origin = drawInfo.legVect,
				Rotation = drawPlayer.legRotation,
				Texture = texture
			};
		}
	}
	internal abstract class PowerArmorGlowLayer : PowerArmorDrawLayer
	{
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			DrawDataInfo drawDataInfo = GetData(drawInfo);
			Player drawPlayer = drawInfo.drawPlayer;
			//MPlayer modPlayer = drawPlayer.GetModPlayer<MPlayer>();
			SpriteEffects effects = SpriteEffects.None;

			if (drawPlayer.direction == -1)
			{
				effects |= SpriteEffects.FlipHorizontally;
			}

			if (drawPlayer.gravDir == -1)
			{
				effects |= SpriteEffects.FlipVertically;
			}

			DrawData data = new(
				drawDataInfo.Texture,
				drawDataInfo.Position,
				drawDataInfo.Frame,
				Color.White * Main.essScale,
				drawDataInfo.Rotation,
				drawDataInfo.Origin,
				1f,
				effects,
				0
			);

			drawInfo.DrawDataCache.Add(data);
		}
	}
	internal abstract class PowerArmorShaderLayer : PowerArmorDrawLayer
	{
		public const int ShaderNumSegments = 8;
		public const int ShaderDrawOffset = 2;

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			DrawDataInfo drawDataInfo = GetData(drawInfo);
			Player drawPlayer = drawInfo.drawPlayer;
			//MPlayer modPlayer = drawPlayer.GetModPlayer<MPlayer>();
			SpriteEffects effects = SpriteEffects.None;

			if (drawPlayer.direction == -1) {
				effects |= SpriteEffects.FlipHorizontally;
			}

			if (drawPlayer.gravDir == -1) {
				effects |= SpriteEffects.FlipVertically;
			}

			DrawData data = new(
				drawDataInfo.Texture,
				drawDataInfo.Position,
				drawDataInfo.Frame,
				Color.White * Main.essScale,// * modPlayer.LayerStrength * modPlayer.ShaderStrength,
				drawDataInfo.Rotation,
				drawDataInfo.Origin,
				1f,
				effects,
				0);

			BeginShaderBatch(Main.spriteBatch);

			ShaderId ??= GameShaders.Armor.GetShaderIdFromItemId(Terraria.ID.ItemID.LivingRainbowDye);

			GameShaders.Armor.Apply(ShaderId.Value, drawPlayer, data);

			Vector2 centerPos = data.position;

			for (int i = 0; i < ShaderNumSegments; i++) {
				data.position = centerPos + GetDrawOffset(i);
				data.Draw(Main.spriteBatch);
			}

			data.position = centerPos;
		}

		protected static Vector2 GetDrawOffset(int i) => new Vector2(0, ShaderDrawOffset).RotatedBy((float)i / ShaderNumSegments * MathHelper.TwoPi);

		private static void BeginShaderBatch(SpriteBatch batch) {
			batch.End();
			RasterizerState rasterizerState = Main.LocalPlayer.gravDir == 1f ? RasterizerState.CullCounterClockwise : RasterizerState.CullClockwise;
			batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, rasterizerState, null, Main.GameViewMatrix.TransformationMatrix);
		}
	}
	/*public class PowerArmorHelmetDrawLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			//throw new NotImplementedException();
		}
	}*/
	internal class PAHelmetGlow : PowerArmorGlowLayer
	{
		private static Asset<Texture2D> _glowTexture;

		public override bool IsHeadLayer => true;

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
			=> drawInfo.drawPlayer.head == MPlayer.GetHelmet(drawInfo.drawPlayer) && base.GetDefaultVisibility(drawInfo);

		public override DrawDataInfo GetData(PlayerDrawSet info)
		{
			_glowTexture = MPlayer.GetHelmetGlow(info);

			return GetBodyDrawDataInfo(info, _glowTexture.Value);
		}

		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
	}
	/*public class PowerArmorBreastplateDrawLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			//throw new NotImplementedException();
		}
	}*/
	internal class PABreastplateGlow : PowerArmorGlowLayer
	{
		private static Asset<Texture2D> _glowTexture;

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
			=> drawInfo.drawPlayer.body == MPlayer.GetBreastplate(drawInfo.drawPlayer) && base.GetDefaultVisibility(drawInfo);

		public override DrawDataInfo GetData(PlayerDrawSet info)
		{
			_glowTexture = MPlayer.GetBreastplateGlow(info);

			return GetBodyDrawDataInfo(info, _glowTexture.Value);
		}

		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);
	}
	internal class PAArmsGlow : PowerArmorGlowLayer
	{
		private static Asset<Texture2D> _glowTexture;

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
			=> drawInfo.drawPlayer.body == MPlayer.GetBreastplate(drawInfo.drawPlayer) && base.GetDefaultVisibility(drawInfo);

		public override DrawDataInfo GetData(PlayerDrawSet info)
		{
			_glowTexture = MPlayer.GetArmsGlow(info);

			return GetBodyDrawDataInfo(info, _glowTexture.Value);
		}

		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.OffhandAcc);
	}
	/*public class PowerArmorGreavesDrawLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			//throw new NotImplementedException();
		}
	}*/
	internal class PAGreavesGlow : PowerArmorGlowLayer
	{
		private static Asset<Texture2D> _glowTexture;

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
			=> drawInfo.drawPlayer.legs == MPlayer.GetGreaves(drawInfo.drawPlayer) && base.GetDefaultVisibility(drawInfo);

		public override DrawDataInfo GetData(PlayerDrawSet info)
		{
			_glowTexture = MPlayer.GetGreavesGlow(info);

			return GetBodyDrawDataInfo(info, _glowTexture.Value);
		}

		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);
	}
}
