using System;
using System.Linq;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Capture;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using MetroidMod;
using MetroidMod.NPCs;

namespace MetroidMod
{
    public class GlowTexture : ModPlayer  
    {
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			MPlayer mPlayer = player.GetModPlayer<MPlayer>();
			GlowTexture gPlayer = player.GetModPlayer<GlowTexture>();
			Player P = player;
			
			for (int k = 0; k < layers.Count; k++)
			{
				if (layers[k] == PlayerLayer.Legs)
				{
					layers.Insert(k + 1, LegsGlow);
                    LegsGlow.visible = true;
                }
				if (layers[k] == PlayerLayer.Body)
				{
					layers.Insert(k + 1, BodyGlow);
                    BodyGlow.visible = true;
                }
				if (layers[k] == PlayerLayer.Head)
				{
					layers.Insert(k + 1, HeadGlow);
                    HeadGlow.visible = true;
                }
				if (layers[k] == PlayerLayer.Arms)
				{
					layers.Insert(k + 1, ArmsGlow);
                    ArmsGlow.visible = true;
                }
			}
		}
		
		internal static Color glowColor(Color getColor, int shader)
		{
			if(shader > 0)
			{
				//System.Drawing.Color color = System.Drawing.Color.FromArgb(getColor.R,getColor.G,getColor.B);
				//return MColor.HsvColor((double)color.GetHue(),(double)color.GetSaturation()*0.5,1.0);
				
				return new Color(getColor.R,getColor.G,getColor.B,0);
			}
			return Color.White;
		}
		
		public static readonly PlayerLayer LegsGlow = new PlayerLayer("MetroidMod", "LegsGlow", PlayerLayer.Legs, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();
			
			int shader = drawInfo.legArmorShader;
			
			Item item = null;
			if(drawPlayer.armor[2] != null && !drawPlayer.armor[2].IsAir)
			{
				item = drawPlayer.armor[2];
			}
			if(drawPlayer.armor[12] != null && !drawPlayer.armor[12].IsAir)
			{
				item = drawPlayer.armor[12];
			}
			if(item != null && item.modItem != null)
			{
				string name = item.modItem.Texture + "_Legs_Glow";
				if (ModContent.TextureExists(name) && name.Contains("MetroidMod"))
				{
					Texture2D tex = ModContent.GetTexture(name);
					mPlayer.DrawTexture(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.legFrame, drawPlayer.legRotation, drawPlayer.legPosition, drawInfo.legOrigin, drawPlayer.GetImmuneAlphaPure(glowColor(drawInfo.lowerArmorColor,shader), drawInfo.shadow), shader);
				}
			}
		});
		public static readonly PlayerLayer BodyGlow = new PlayerLayer("MetroidMod", "BodyGlow", PlayerLayer.Body, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();
			
			int shader = drawInfo.bodyArmorShader;
			
			Item item = null;
			if(drawPlayer.armor[1] != null && !drawPlayer.armor[1].IsAir)
			{
				item = drawPlayer.armor[1];
			}
			if(drawPlayer.armor[11] != null && !drawPlayer.armor[11].IsAir)
			{
				item = drawPlayer.armor[11];
			}
			if(item != null && item.modItem != null)
			{
				string name = item.modItem.Texture + "_Body_Glow";
				string name2 = item.modItem.Texture + "_FemaleBody_Glow";
				if ((ModContent.TextureExists(name) && drawPlayer.Male && name.Contains("MetroidMod")) || (ModContent.TextureExists(name2) && !drawPlayer.Male && name2.Contains("MetroidMod")))
				{
					Texture2D tex = null;
					if(drawPlayer.Male)
					{
						tex = ModContent.GetTexture(name);
					}
					else
					{
						tex = ModContent.GetTexture(name2);
					}
					if(tex != null)
					{
						mPlayer.DrawTexture(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.bodyRotation, drawPlayer.bodyPosition, drawInfo.bodyOrigin, drawPlayer.GetImmuneAlphaPure(glowColor(drawInfo.middleArmorColor,shader), drawInfo.shadow), shader);
					}
				}
			}
		});
		public static readonly PlayerLayer HeadGlow = new PlayerLayer("MetroidMod", "HeadGlow", PlayerLayer.Head, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();
			
			int shader = drawInfo.headArmorShader;
			
			Item item = null;
			if(drawPlayer.armor[0] != null && !drawPlayer.armor[0].IsAir)
			{
				item = drawPlayer.armor[0];
			}
			if(drawPlayer.armor[10] != null && !drawPlayer.armor[10].IsAir)
			{
				item = drawPlayer.armor[10];
			}
			if(item != null && item.modItem != null)
			{
				string name = item.modItem.Texture + "_Head_Glow";
				if (ModContent.TextureExists(name) && name.Contains("MetroidMod"))
				{
					Texture2D tex = ModContent.GetTexture(name);
					mPlayer.DrawTexture(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.headRotation, drawPlayer.bodyPosition, drawInfo.headOrigin, drawPlayer.GetImmuneAlphaPure(glowColor(drawInfo.upperArmorColor,shader), drawInfo.shadow), shader);
				}
			}
		});
		public static readonly PlayerLayer ArmsGlow = new PlayerLayer("MetroidMod", "ArmsGlow", PlayerLayer.Arms, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();
			
			int shader = drawInfo.bodyArmorShader;
			
			Item item = null;
			if(drawPlayer.armor[1] != null && !drawPlayer.armor[1].IsAir)
			{
				item = drawPlayer.armor[1];
			}
			if(drawPlayer.armor[11] != null && !drawPlayer.armor[11].IsAir)
			{
				item = drawPlayer.armor[11];
			}
			if(item != null && item.modItem != null)
			{
				string name = item.modItem.Texture + "_Arms_Glow";
				if (ModContent.TextureExists(name) && name.Contains("MetroidMod"))
				{
					Texture2D tex = ModContent.GetTexture(name);
					mPlayer.DrawTexture(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.bodyRotation, drawPlayer.bodyPosition, drawInfo.bodyOrigin, drawPlayer.GetImmuneAlphaPure(glowColor(drawInfo.middleArmorColor,shader), drawInfo.shadow), shader);
				}
			}
		});
	}
}