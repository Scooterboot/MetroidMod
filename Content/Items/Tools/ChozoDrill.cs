using MetroidMod.Content.Items.Miscellaneous;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tools
{
	public class ChozoDrill : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.IsDrill[Type] = true;
		}

		public override void SetDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 3));
			Item.damage = 27;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.width = 20;
			Item.height = 12;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0.5f;
			Item.value = Item.buyPrice(gold: 2, silver: 60);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item23;
			Item.shoot = ModContent.ProjectileType<Projectiles.ChozoDrillProjectile>();
			Item.shootSpeed = 32f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.tileBoost = -1;

			Item.pick = 100;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<ChoziteBar>(12)
				.AddIngredient(ItemID.HellstoneBar, 20)
				.AddIngredient<EnergyShard>(3)
				.AddTile(TileID.Hellforge)
				.Register();
			/*CreateRecipe()
				.AddIngredient<ChoziteBar>(12)
				.AddIngredient(ItemID.MoltenPickaxe)
				.AddIngredient<EnergyShard>(3)
				.AddTile(TileID.Hellforge)
				.Register();
			CreateRecipe()
				.AddIngredient<ChozitePickaxe>()
				.AddIngredient(ItemID.MoltenPickaxe)
				.AddIngredient<EnergyShard>(3)
				.AddTile(TileID.Hellforge)
				.Register();
			CreateRecipe()
				.AddIngredient<ChozitePickaxe>()
				.AddIngredient(ItemID.HellstoneBar, 20)
				.AddIngredient<EnergyShard>(3)
				.AddTile(TileID.Hellforge)
				.Register();*/
		}
	}
}
