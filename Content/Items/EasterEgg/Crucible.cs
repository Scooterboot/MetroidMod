using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Content.Projectiles.EasterEgg;

namespace MetroidMod.Content.Items.EasterEgg
{
	public class Crucible : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Slayer's Crucible");
			/* Tooltip.SetDefault("A hellish blade of unknown origin.\n" +
			"'Rip and Tear until it is done.'"); */

			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.DamageType = DamageClass.Melee;
			Item.width = 52;
			Item.height = 52;
			Item.scale = 1.15f;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CrucibleProj>();
			Item.shootSpeed = 11f;
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
		/*public override bool OnlyShootOnSwing
		{
			get {
				return true;
			}
		}*/
		public override bool CanShoot(Player player) => player.ItemAnimationJustStarted;
		int lastSound = -1;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (!player.ItemAnimationJustStarted) { return false; }
			int n = 1 + Main.rand.Next(4);
			if (n == lastSound)
			{
				n++;
				if (n > 4)
				{
					n = 1;
				}
			}
			SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/EasterEgg/Crucible_{n}") { Volume = 0.5f });
			lastSound = n;
			return true;
		}
		public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Items/EasterEgg/Crucible_Glow").Value;
			float num4 = Item.velocity.X * 0.2f;
			float scale2 = 1f;
			float num5 = (float)(Item.height - tex.Height);
			float num6 = (float)(Item.width / 2 - tex.Width / 2);
			sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num6, Item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num5 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), new Color(250, 250, 250, Item.alpha), num4, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), scale2, SpriteEffects.None, 0f);
		}
	}
}
