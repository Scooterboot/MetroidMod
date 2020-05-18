using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.easteregg
{
	public class Crucible : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slayer's Crucible");
			Tooltip.SetDefault("A hellish blade of unknown origin.\n" +
			"'Rip and Tear until it is done.'");
		}
		
		public override void SetDefaults()
		{
			item.damage = 100;
			item.melee = true;
			item.width = 52;
			item.height = 52;
			item.scale = 1.15f;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = Item.buyPrice(gold: 1);
			item.rare = 10;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("CrucibleProj");
			item.shootSpeed = 11f;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(2))
			{
				Dust dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 183);
				dust.noGravity = true;
				dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6);
				dust.noGravity = true;
			}
		}
		public override bool OnlyShootOnSwing
		{
			get
			{
				return true;
			}
		}
		int lastSound = -1;
		public override bool Shoot(Player P, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int n = 1 + Main.rand.Next(4);
			if(n == lastSound)
			{
				n++;
				if(n > 4)
				{
					n = 1;
				}
			}
			SoundEffectInstance soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)P.Center.X, (int)P.Center.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/easteregg/Crucible_"+n));
			soundInstance.Volume *= 0.5f;
			lastSound = n;
			return true;
		}
		public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D tex = mod.GetTexture("Items/easteregg/Crucible_Glow");
			float num4 = item.velocity.X * 0.2f;
			float scale2 = 1f;
			float num5 = (float)(item.height - tex.Height);
			float num6 = (float)(item.width / 2 - tex.Width / 2);
			sb.Draw(tex, new Vector2(item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num6, item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num5 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), new Color(250, 250, 250, item.alpha), num4, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), scale2, SpriteEffects.None, 0f);
		}
	}
}