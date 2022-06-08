using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.Items.Vanity.Suits
{
	[AutoloadEquip(EquipType.Body)]
	public class VanityVortexGravitySuitBreastplate : VanityTerraGravitySuitBreastplate
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentBreastplate";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Vortex Gravity Suit Breastplate");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanityVortexGravitySuitHelmet>() && body.type == ModContent.ItemType<VanityVortexGravitySuitBreastplate>() && legs.type == ModContent.ItemType<VanityVortexGravitySuitGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(67, 255, 255);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VanityVortexGravitySuitGreaves : VanityTerraGravitySuitGreaves
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentGreaves";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Vortex Gravity Suit Greaves");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VanityVortexGravitySuitHelmet : VanityTerraGravitySuitHelmet
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/VortexAugment/VortexAugmentHelmet";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Vortex Gravity Suit Helmet");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
	}
}
