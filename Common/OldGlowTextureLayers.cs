using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;

namespace MetroidMod.Common
{
	internal class OldGlowTextureLayer_Legs : PlayerDrawLayer
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

	internal class OldGlowTextureLayer_Body : PlayerDrawLayer
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
				string name2 = item.ModItem.Texture + "_FemaleBody_Glow";
				if ((drawPlayer.Male && ModContent.RequestIfExists(name, out Asset<Texture2D> asset) && name.Contains("MetroidMod")) || (!drawPlayer.Male && ModContent.RequestIfExists(name2, out asset) && name2.Contains("MetroidMod")))
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
						MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.bodyRotation, drawPlayer.bodyPosition, drawInfo.bodyVect, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorBody, shader), drawInfo.shadow), shader);
					}
				}
			}
		}
	}

	internal class OldGlowTextureLayer_Head : PlayerDrawLayer
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

	internal class OldGlowTextureLayer_Arms : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HandOnAcc);

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
				string name = item.ModItem.Texture + "_Arms_Glow";
				if (ModContent.RequestIfExists(name, out Asset<Texture2D> asset) && name.Contains("MetroidMod"))
				{
					Texture2D tex = asset.Value;
					MPlayer.DrawTexture(ref drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.bodyRotation, drawPlayer.bodyPosition, drawInfo.bodyVect, drawPlayer.GetImmuneAlphaPure(VanityGlowTexture.glowColor(drawInfo.colorArmorBody, shader), drawInfo.shadow), shader);
				}
			}
		}
	}
}
