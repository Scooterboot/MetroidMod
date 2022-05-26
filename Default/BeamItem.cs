using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Default
{
	/*
	[Autoload(false)]
	public class BeamItem : ModItem
	{
		public ModBeam modBeam;

		public override string Texture => modBeam.ItemTexture;

		public override string Name => modBeam.Name + "Addon";

		public BeamItem(ModBeam modBeam)
		{
			this.modBeam = modBeam;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(modBeam.DisplayName.GetDefault());
			Tooltip.SetDefault(modBeam.AddOnlyBeamItem ? "Cannot be equipped\n" + (modBeam.Tooltip.GetDefault() != modBeam.Tooltip.Key ? modBeam.Tooltip.GetDefault() : "") : string.Format($"{modBeam.GetAddonVersionName()}\n") + $"Slot Type: {modBeam.GetAddonSlotName()}\n" + (modBeam.Tooltip.GetDefault() != modBeam.Tooltip.Key ? modBeam.Tooltip.GetDefault() + "\n" : "") + string.Format($"[c/{(modBeam.AddonDamageMult >= 0f ? "78BE78" : "BE7878")}:{(modBeam.AddonDamageMult >= 0f ? "+" : "") + modBeam.AddonDamageMult * 100}% damage]\n") +
				string.Format($"[c/{(modBeam.AddonHeat <= 0f ? "78BE78" : "BE7878")}:{(modBeam.AddonHeat >= 0f ? "+" : "") + modBeam.AddonHeat * 100}% overheat use]\n") +
				string.Format($"[c/{(modBeam.AddonSpeed >= 0f ? "78BE78" : "BE7878")}:{(modBeam.AddonSpeed >= 0f ? "+" : "") + modBeam.AddonSpeed * 100}% speed]"));
			SacrificeTotal = modBeam.SacrificeTotal;
		}

		public override void SetDefaults()
		{
			Item.maxStack = 1;
			modBeam.SetItemDefaults(Item);
			modBeam.ItemType = Type;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.width = ModContent.Request<Texture2D>(Texture).Value.Width;
			Item.height = ModContent.Request<Texture2D>(Texture).Value.Height;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = modBeam.TileType;
			Item.GetGlobalItem<Common.GlobalItems.MGlobalItem>().AddonType = AddonType.PowerBeam;
		}

		public override void HoldItem(Player player)
		{
			if (player.InInteractionRange(Player.tileTargetX, Player.tileTargetY))
			{
				player.cursorItemIconEnabled = true;
				player.cursorItemIconID = Type;
			}
		}

		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
		{
			itemGroup = ContentSamples.CreativeHelper.ItemGroup.Accessories;
		}

		public override ModItem Clone(Item item)
		{
			BeamItem obj = (BeamItem)base.Clone(item);
			obj.modBeam = modBeam;
			return obj;
		}

		public override ModItem NewInstance(Item entity)
		{
			var inst = Clone(entity);
			return inst;
		}
	}
	*/
}
