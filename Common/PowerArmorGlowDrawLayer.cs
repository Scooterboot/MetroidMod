using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Common
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
		public int shader = -1;
		public Color color = Color.White;

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
				drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(color, shader), drawInfo.shadow),
				drawDataInfo.Rotation,
				drawDataInfo.Origin,
				1f,
				effects,
				0
			);
			data.shader = shader;

			drawInfo.DrawDataCache.Add(data);
		}
	}
	internal class PAHelmetGlow : PowerArmorGlowLayer
	{
		private static Asset<Texture2D> _glowTexture;

		public override bool IsHeadLayer => true;

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
			=> drawInfo.drawPlayer.head == MPlayer.GetHelmet(drawInfo.drawPlayer) && base.GetDefaultVisibility(drawInfo);

		public override DrawDataInfo GetData(PlayerDrawSet info)
		{
			_glowTexture = MPlayer.GetHelmetGlow(info);
			shader = info.cHead;
			color = info.colorArmorHead;

			return GetBodyDrawDataInfo(info, _glowTexture.Value);
		}

		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
	}
	internal class PABreastplateGlow : PowerArmorGlowLayer
	{
		private static Asset<Texture2D> _glowTexture;

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
			=> drawInfo.drawPlayer.body == MPlayer.GetBreastplate(drawInfo.drawPlayer) && base.GetDefaultVisibility(drawInfo);

		public override DrawDataInfo GetData(PlayerDrawSet info)
		{
			_glowTexture = MPlayer.GetBreastplateGlow(info);
			shader = info.cBody;
			color = info.colorArmorBody;

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
			shader = info.cBody;
			color = info.colorArmorBody;

			return GetBodyDrawDataInfo(info, _glowTexture.Value);
		}

		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HandOnAcc);
	}
	internal class PAShouldersGlow : PowerArmorGlowLayer
	{
		private static Asset<Texture2D> _glowTexture;

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
			=> drawInfo.drawPlayer.body == MPlayer.GetBreastplate(drawInfo.drawPlayer) && base.GetDefaultVisibility(drawInfo);

		public override DrawDataInfo GetData(PlayerDrawSet info)
		{
			_glowTexture = MPlayer.GetShouldersGlow(info);
			shader = info.cBody;
			color = info.colorArmorBody;

			return GetBodyDrawDataInfo(info, _glowTexture.Value);
		}

		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HandOnAcc);
	}
	internal class PAGreavesGlow : PowerArmorGlowLayer
	{
		private static Asset<Texture2D> _glowTexture;

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
			=> drawInfo.drawPlayer.legs == MPlayer.GetGreaves(drawInfo.drawPlayer) && base.GetDefaultVisibility(drawInfo);

		public override DrawDataInfo GetData(PlayerDrawSet info)
		{
			_glowTexture = MPlayer.GetGreavesGlow(info);
			shader = info.cLegs;
			color = info.colorArmorLegs;

			return GetBodyDrawDataInfo(info, _glowTexture.Value);
		}

		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);
	}
}
