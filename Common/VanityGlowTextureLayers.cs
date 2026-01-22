using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using static MetroidMod.Sounds;

namespace MetroidMod.Common
{
	internal class VanityGlowTextureLayer_Legs : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;

			int shader = drawInfo.cLegs;

			Item item = null;
			if (drawPlayer.armor[2] != null && !drawPlayer.armor[2].IsAir && drawPlayer.legs == drawPlayer.armor[2].legSlot)
			{
				item = drawPlayer.armor[2];
			}
			if (drawPlayer.armor[12] != null && !drawPlayer.armor[12].IsAir && drawPlayer.legs == drawPlayer.armor[12].legSlot)
			{
				item = drawPlayer.armor[12];
			}
			if (item != null && item.ModItem != null)
			{
				string name = item.ModItem.Texture + "_Legs_Glow";
				if (ModContent.RequestIfExists(name, out Asset<Texture2D> asset) && name.Contains("MetroidMod"))
				{
					Texture2D tex = asset.Value;
					MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, drawPlayer.legFrame, drawPlayer.legRotation, drawPlayer.legPosition, drawInfo.legVect, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorLegs, shader), drawInfo.shadow), shader);
				}
			}
		}
	}

	internal class VanityGlowTextureLayer_Body : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();

			int shader = drawInfo.cBody;

			Item item = null;
			if (drawPlayer.armor[1] != null && !drawPlayer.armor[1].IsAir && drawPlayer.body == drawPlayer.armor[1].bodySlot)
			{
				item = drawPlayer.armor[1];
			}
			if (drawPlayer.armor[11] != null && !drawPlayer.armor[11].IsAir && drawPlayer.body == drawPlayer.armor[11].bodySlot)
			{
				item = drawPlayer.armor[11];
			}
			if (item != null && item.ModItem != null)
			{
				string name = item.ModItem.Texture + "_Body_Glow";
				//string name2 = item.ModItem.Texture + "_FemaleBody_Glow";
				if (ModContent.RequestIfExists(name, out Asset<Texture2D> asset) && name.Contains("MetroidMod"))
				{
					Texture2D tex = asset.Value;
					/*if (drawPlayer.Male)
					{
						tex = ModContent.GetTexture(name);
					}
					else
					{
						tex = ModContent.GetTexture(name2);
					}*/
					if (tex != null)
					{
						Vector2 pos = drawPlayer.bodyPosition;
						Vector2 offset = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
						offset.Y -= 2f;
						pos += offset * (float)-(float)drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
						MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, drawInfo.compTorsoFrame, drawPlayer.bodyRotation, pos, drawInfo.bodyVect, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorBody, shader), drawInfo.shadow), shader);
					}
				}
			}
		}
	}

	internal class VanityGlowTextureLayer_Head : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();

			int shader = drawInfo.cHead;

			Item item = null;
			if (drawPlayer.armor[0] != null && !drawPlayer.armor[0].IsAir && drawPlayer.head == drawPlayer.armor[0].headSlot)
			{
				item = drawPlayer.armor[0];
			}
			if (drawPlayer.armor[10] != null && !drawPlayer.armor[10].IsAir && drawPlayer.head == drawPlayer.armor[10].headSlot)
			{
				item = drawPlayer.armor[10];
			}
			if (item != null && item.ModItem != null)
			{
				string name = item.ModItem.Texture + "_Head_Glow";
				if (ModContent.RequestIfExists(name, out Asset<Texture2D> asset) && name.Contains("MetroidMod"))
				{
					Texture2D tex = asset.Value;
					MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.headRotation, drawPlayer.bodyPosition, drawInfo.headVect, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorHead, shader), drawInfo.shadow), shader);
				}
			}
		}
	}
	internal class VanityGlowTextureLayer_BackArm : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Torso);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();

			int shader = drawInfo.cBody;

			Item item = null;
			if (drawPlayer.armor[1] != null && !drawPlayer.armor[1].IsAir && drawPlayer.body == drawPlayer.armor[1].bodySlot)
			{
				item = drawPlayer.armor[1];
			}
			if (drawPlayer.armor[11] != null && !drawPlayer.armor[11].IsAir && drawPlayer.body == drawPlayer.armor[11].bodySlot)
			{
				item = drawPlayer.armor[11];
			}
			if (item != null && item.ModItem != null)
			{
				string name = item.ModItem.Texture + "_Body_Glow";
				if (ModContent.RequestIfExists(name, out Asset<Texture2D> asset) && name.Contains("MetroidMod"))
				{
					Texture2D tex = asset.Value;

					Rectangle frame = drawInfo.compBackArmFrame;
					Rectangle shoulderFrame = drawInfo.compBackShoulderFrame;
					Vector2 origin = drawInfo.bodyVect;
					Vector2 pos = drawPlayer.bodyPosition;
					float rot = drawPlayer.bodyRotation;
					rot += drawInfo.compositeBackArmRotation;

					Vector2 compositeOffset_BackArm = new Vector2(6 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : -1), 2 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : -1));
					origin += compositeOffset_BackArm;

					Vector2 offset = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
					offset.Y -= 2f;
					pos += offset * (float)-(float)drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
					Vector2 shoulderPos = pos + drawInfo.backShoulderOffset;

					if (drawInfo.compBackArmFrame.X / drawInfo.compBackArmFrame.Width >= 7)
					{
						pos += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : -1, (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : -1);
					}

					if (!drawInfo.hideCompositeShoulders)
					{
						MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, shoulderFrame, drawPlayer.bodyRotation, shoulderPos, origin, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorBody, shader), drawInfo.shadow), shader);

					}
					MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, frame, rot, pos, origin, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorBody, shader), drawInfo.shadow), shader);

				}
			}
		}
	}
	internal class VanityGlowTextureLayer_FrontArm : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HandOnAcc);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();

			int shader = drawInfo.cBody;

			Item item = null;
			if (drawPlayer.armor[1] != null && !drawPlayer.armor[1].IsAir && drawPlayer.body == drawPlayer.armor[1].bodySlot)
			{
				item = drawPlayer.armor[1];
			}
			if (drawPlayer.armor[11] != null && !drawPlayer.armor[11].IsAir && drawPlayer.body == drawPlayer.armor[11].bodySlot)
			{
				item = drawPlayer.armor[11];
			}
			if (item != null && item.ModItem != null)
			{
				string name = item.ModItem.Texture + "_Body_Glow";
				//string name2 = item.ModItem.Texture + "_Shoulders_Glow";
				if (ModContent.RequestIfExists(name, out Asset<Texture2D> asset) && name.Contains("MetroidMod"))
				{
					Texture2D tex = asset.Value;

					Rectangle frame = drawInfo.compFrontArmFrame;
					Rectangle shoulderFrame = drawInfo.compFrontShoulderFrame;
					Vector2 origin = drawInfo.bodyVect;
					Vector2 pos = drawPlayer.bodyPosition;
					float rot = drawPlayer.bodyRotation;
					rot += drawInfo.compositeFrontArmRotation;
					Vector2 compositeOffset_FrontArm = new Vector2(-5 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : -1), 0f);
					origin += compositeOffset_FrontArm;
					Vector2 offset = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
					offset.Y -= 2f;
					pos += offset * (float)-(float)drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
					Vector2 shoulderPos = pos + drawInfo.frontShoulderOffset;

					if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
					{
						pos += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : -1, (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : -1);
					}

					bool holdingCannon = drawPlayer.inventory[drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Weapons.PowerBeam>() ||
										 drawPlayer.inventory[drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Weapons.MissileLauncher>() ||
										 drawPlayer.inventory[drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Tools.NovaLaserDrill>() ||
										 drawPlayer.inventory[drawPlayer.selectedItem].type == ModContent.ItemType<Content.Items.Weapons.ArmCannon>();
					if (!holdingCannon)
					{
						MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, frame, rot, pos, origin, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorBody, shader), drawInfo.shadow), shader);
					}
					if (!drawInfo.hideCompositeShoulders && drawInfo.compShoulderOverFrontArm)
					{
						MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, shoulderFrame, drawPlayer.bodyRotation, shoulderPos, origin, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorBody, shader), drawInfo.shadow), shader);
					}

					//DrawDataInfo info = PowerArmorDrawLayer.GetFrontArmDrawDataInfo(drawInfo, tex);
					//MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, (Rectangle)info.Frame, info.Rotation, info.Position, info.Origin, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorBody, shader), drawInfo.shadow), shader);
					//MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.bodyRotation, drawPlayer.bodyPosition, drawInfo.bodyVect, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorBody, shader), drawInfo.shadow), shader);
				}
				//if (ModContent.RequestIfExists(name, out asset) && name.Contains("MetroidMod"))
				//{
				//	Texture2D tex = asset.Value;
				//	MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.bodyRotation, drawPlayer.bodyPosition, drawInfo.bodyVect, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorBody, shader), drawInfo.shadow), shader);
				//}
			}
		}
	}
	internal class VanityGlowTextureLayer_Wings : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Wings);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();

			int shader = drawInfo.cWings;

			if (mPlayer.wingsGlowmaskTex != null)
			{
				string name = mPlayer.wingsGlowmaskTex;
				if (ModContent.RequestIfExists(name, out Asset<Texture2D> asset) && name.Contains("MetroidMod"))
				{
					Texture2D tex = asset.Value;
					Rectangle drawRect = new Rectangle(0, TextureAssets.Wings[drawPlayer.wings].Height() / 4 * drawPlayer.wingFrame, TextureAssets.Wings[drawPlayer.wings].Width(), TextureAssets.Wings[drawPlayer.wings].Height() / 4);
					Vector2 drawOrigin = new Vector2((float)(TextureAssets.Wings[drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawPlayer.wings].Height() / 8));
					Vector2 drawPos = drawPlayer.bodyPosition + new Vector2(0, drawOrigin.Y) + new Vector2(drawPlayer.Directions.X * -9, drawPlayer.Directions.Y * -23);
					MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, drawRect, drawPlayer.bodyRotation, drawPos, drawOrigin, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorBody, shader), drawInfo.shadow), shader);
				}
			}
		}
	}
	internal class VanityGlowTextureLayer_LargeTail : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.WaistAcc);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();

			int shader = mPlayer.largeTailDye;

			if (mPlayer.largeTailGlowmaskTex != null)
			{
				string name = mPlayer.largeTailGlowmaskTex;
				if (ModContent.RequestIfExists(name, out Asset<Texture2D> asset) && name.Contains("MetroidMod"))
				{
					Texture2D tex = asset.Value;
					Rectangle drawRect = new Rectangle(drawPlayer.legFrame.X, drawPlayer.legFrame.Y, tex.Width, tex.Height / 20);
					Vector2 drawOrigin = new Vector2(tex.Width / 2, tex.Height / 40);
					Vector2 drawPos = drawPlayer.bodyPosition;
					MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, drawRect, drawPlayer.bodyRotation, drawPos, drawOrigin, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorBody, shader), drawInfo.shadow), shader);
				}
			}
		}
	}
}
