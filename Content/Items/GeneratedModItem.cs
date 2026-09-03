using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items
{
	/// <summary>
	/// A standard for generating and interacting with <see cref="Terraria.ModLoader.ModItem">s.
	/// </summary>
	public interface IGeneratesModItem
	{
		string Name { get; }

		GeneratedModItem GeneratedModItem { get; }

		int ItemType { get; }

		LocalizedText ItemDisplayName { get; }
		LocalizedText ItemTooltip { get; }

		string ItemTexture { get; }
		
		/// <inheritdoc cref="GeneratedModItem.AltFunctionUse(Player)"/>
		bool ItemAltFunctionUse(Player player);

		/// <inheritdoc cref="GeneratedModItem.AddRecipes"/>
		void ItemAddRecipes();

		/// <inheritdoc cref="GeneratedModItem.CanUseItem(Player)"/>
		bool ItemCanUseItem(Player player);

		/// <inheritdoc cref="GeneratedModItem.HoldItem(Player)"/>
		void ItemHoldItem(Player player);

		/// <inheritdoc cref="GeneratedModItem.SetDefaults"/>
		void ItemSetDefaults();

		/// <inheritdoc cref="GeneratedModItem.SetStaticDefaults"/>
		void ItemSetStaticDefaults();

		/// <inheritdoc cref="GeneratedModItem.UseItem(Player)"/>
		bool? ItemUseItem(Player player);
		
		IGeneratesModItem Clone(GeneratedModItem newGeneratedModItem);
	}

	/// <summary>
	/// An automatically generated ModItem. See <see cref="IGeneratesModItem"/>.
	/// </summary>
	[Autoload(false)]
	public class GeneratedModItem : ModItem
	{
		public IGeneratesModItem producer;


		public override string Name => producer.Name + "Tile";

		public override LocalizedText DisplayName => producer.ItemDisplayName;

		public override LocalizedText Tooltip => producer.ItemTooltip;

		public override string Texture => producer.ItemTexture;


		public GeneratedModItem(IGeneratesModItem producer)
		{
			this.producer = producer;
		}


		public override ModItem Clone(Item item)
		{
			GeneratedModItem obj = (GeneratedModItem)base.Clone(item);
			obj.producer = producer.Clone(obj);
			return obj;
		}

		public override ModItem NewInstance(Item entity)
		{
			ModItem inst = Clone(entity);
			return inst;
		}

		public override bool AltFunctionUse(Player player)
		{
			return producer.ItemAltFunctionUse(player);
		}

		public override void AddRecipes()
		{
			producer.ItemAddRecipes();
		}

		public override bool CanUseItem(Player player)
		{
			return producer.ItemCanUseItem(player);
		}

		public override void HoldItem(Player player)
		{
			producer.ItemHoldItem(player);
		}

		public override void SetDefaults()
		{
			producer.ItemSetDefaults();
		}

		public override void SetStaticDefaults()
		{
			producer.ItemSetStaticDefaults();
		}

		public override bool? UseItem(Player player)
		{
			return producer.ItemUseItem(player);
		}
	}
}
