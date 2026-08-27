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
		GeneratedModItem GeneratedModItem { get; }

		int ItemType { get; }

		LocalizedText ItemDisplayName { get; }
		LocalizedText ItemTooltip { get; }

		string ItemTexture { get; }
		
		/// <inheritdoc cref="GeneratedModItem.AddRecipes"/>
		void ItemAddRecipes(GeneratedModItem generatedModItem) { }

		/// <inheritdoc cref="GeneratedModItem.SetDefaults"/>
		void ItemSetDefaults(GeneratedModItem generatedModItem) { }

		/// <inheritdoc cref="GeneratedModItem.SetStaticDefaults"/>
		void ItemSetStaticDefaults(GeneratedModItem generatedModItem) { }

	}

	/// <summary>
	/// An automatically generated ModItem. See <see cref="IGeneratesModItem"/>.
	/// </summary>
	[Autoload(false)]
	public class GeneratedModItem : ModItem
	{
		public IGeneratesModItem producer;


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
			obj.producer = producer;
			return obj;
		}

		public override ModItem NewInstance(Item entity)
		{
			var inst = Clone(entity);
			return inst;
		}


		public override void AddRecipes()
		{
			producer.ItemAddRecipes(this);
		}

		public override void SetDefaults()
		{
			producer.ItemSetDefaults(this);
		}

		public override void SetStaticDefaults()
		{
			producer.ItemSetStaticDefaults(this);
		}
	}
}
