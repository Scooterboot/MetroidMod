using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Common
{
	public class ScrewAttackLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.FrontAccFront);
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.GetModPlayer<MPlayer>().screwAttack && drawInfo.drawPlayer.GetModPlayer<MPlayer>().somersault;
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player P = drawInfo.drawPlayer;
			MPlayer mPlayer = P.GetModPlayer<MPlayer>();
			if (mPlayer.somersault && mPlayer.screwAttack && drawInfo.shadow == 0f && !mPlayer.ballstate)
			{
				Texture2D tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/ScrewAttackProj").Value;
				Texture2D tex2 = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/ScrewAttack_Yellow").Value;
				for (int i = 0; i < 255; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (projectile.active && projectile.owner == P.whoAmI && projectile.type == ModContent.ProjectileType<Content.Projectiles.ScrewAttackProj>())
					{
						SpriteEffects effects = SpriteEffects.None;
						if (projectile.spriteDirection == -1)
						{
							effects = SpriteEffects.FlipHorizontally;
						}
						if (P.gravDir == -1f)
						{
							effects |= SpriteEffects.FlipVertically;
						}
						Color alpha = Lighting.GetColor((int)((double)projectile.position.X + (double)projectile.width * 0.5) / 16, (int)(((double)projectile.position.Y + (double)projectile.height * 0.5) / 16.0));
						int num121 = tex.Height / Main.projFrames[projectile.type];
						int y9 = num121 * projectile.frame;
						drawInfo.DrawDataCache.Add(new DrawData(tex, drawInfo.Position + P.fullRotationOrigin - Main.screenPosition, new Rectangle?(new Rectangle(0, y9, tex.Width, num121 - 1)), alpha, -mPlayer.rotation, new Vector2((float)(tex.Width / 2), (float)(num121 / 2)), projectile.scale, effects, 0));
						if (mPlayer.screwAttackSpeedEffect > 0)
						{
							Color color21 = alpha * ((float)Math.Min(mPlayer.screwAttackSpeedEffect, 30) / 30f);
							drawInfo.DrawDataCache.Add(new DrawData(tex2, drawInfo.Position + P.fullRotationOrigin - Main.screenPosition, new Rectangle?(new Rectangle(0, y9, tex2.Width, num121 - 1)), color21, -mPlayer.rotation, new Vector2((float)(tex2.Width / 2), (float)(num121 / 2)), projectile.scale, effects, 0));
							Texture2D tex3 = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/ScrewAttack_YellowPlayerGlow").Value;
							drawInfo.DrawDataCache.Add(new DrawData(tex3, drawInfo.Position + (P.Center - P.position) - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, tex3.Width, tex3.Height)), color21, 0f, new Vector2((float)(tex3.Width / 2), (float)(tex3.Height / 2)), projectile.scale, effects, 0));
						}
					}
				}
			}
		}
	}
}
