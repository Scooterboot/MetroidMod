#region Using directives

using MetroidMod.Common.Systems;
using MetroidMod.ID;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

#endregion

namespace MetroidMod.Content.Items.Tools
{
	public class ChoziteCutter : ModItem
	{

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chozite Cutter");
			// Tooltip.SetDefault("Removes weapon-destructable blocks. \nDoes not break wires.");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 1;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useTime = 5;
			Item.useAnimation = 14;
			Item.useStyle = 1;
			Item.rare = 1;
			Item.tileBoost = 20;
			Item.value = Item.buyPrice(0, 2, 50);
		}

		// Netsyncing ?
		public override bool? UseItem(Player player)
		{
			if (MUtils.CanReachWiring(player, Item))
			{
				return RemoveBlockAt(player, Player.tileTargetX, Player.tileTargetY);
			}

			return false;
		}

		public override void HoldItem(Player player)
		{
			if (MUtils.CanReachWiring(player, Item))
			{
				player.cursorItemIconEnabled = true;
				player.cursorItemIconID = Type;
			}
		}

		public static bool RemoveBlockAt(Player player, int i, int j)
		{
			if (Main.tile[i, j].Get<FakeBlockSystem>().Type == BreakableTileID.None)
			{
				return false;
			}

			IEntitySource source = new EntitySource_Parent(player);
			if (player == Main.LocalPlayer)
			{
				switch (Main.tile[i, j].Get<FakeBlockSystem>().Type)
				{
					case BreakableTileID.CrumbleInstant:
						player.QuickSpawnItem(source, ModContent.ItemType<Tiles.Destroyable.CrumbleBlock>());
						break;

					case BreakableTileID.CrumbleSpeed:
						player.QuickSpawnItem(source, ModContent.ItemType<Tiles.Destroyable.CrumbleBlockSpeed>());
						break;

					case BreakableTileID.Bomb:
						player.QuickSpawnItem(source, ModContent.ItemType<Tiles.Destroyable.BombBlock>());
						break;

					case BreakableTileID.Missile:
						player.QuickSpawnItem(source, ModContent.ItemType<Tiles.Destroyable.MissileBlock>());
						break;

					case BreakableTileID.Fake:
						player.QuickSpawnItem(source, ModContent.ItemType<Tiles.Destroyable.FakeBlock>());
						break;

					case BreakableTileID.Boost:
						player.QuickSpawnItem(source, ModContent.ItemType<Tiles.Destroyable.BoostBlock>());
						break;

					case BreakableTileID.PowerBomb:
						player.QuickSpawnItem(source, ModContent.ItemType<Tiles.Destroyable.PowerBombBlock>());
						break;

					case BreakableTileID.SuperMissile:
						player.QuickSpawnItem(source, ModContent.ItemType<Tiles.Destroyable.SuperMissileBlock>());
						break;

					case BreakableTileID.ScrewAttack:
						player.QuickSpawnItem(source, ModContent.ItemType<Tiles.Destroyable.ScrewAttackBlock>());
						break;

					case BreakableTileID.FakeHint:
						player.QuickSpawnItem(source, ModContent.ItemType<Tiles.Destroyable.FakeBlockHint>());
						break;

					case BreakableTileID.CrumbleSlow:
						player.QuickSpawnItem(source, ModContent.ItemType<Tiles.Destroyable.CrumbleBlockSlow>());
						break;

					case BreakableTileID.BombChain:
						player.QuickSpawnItem(source, ModContent.ItemType<Tiles.Destroyable.BombBlockChain>());
						break;

					default:
						MetroidMod.Instance.Logger.Info("Rolled a non-value. " + Main.tile[i, j].Get<FakeBlockSystem>().Type);
						break;
				}
			}


			Main.tile[i, j].Get<FakeBlockSystem>().Type = BreakableTileID.None;
			MSystem.dontRegen[i, j] = false;
			MSystem.hit[i, j] = false;
			SoundEngine.PlaySound(SoundID.Dig, Main.MouseWorld);
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				ModPacket removeBreakableBlock = ModContent.GetInstance<MetroidMod>().GetPacket();
				removeBreakableBlock.Write((byte)MetroidMessageType.BreakableBlockUpdate);
				removeBreakableBlock.Write7BitEncodedInt(i);
				removeBreakableBlock.Write7BitEncodedInt(j);
				removeBreakableBlock.Write(true);
				removeBreakableBlock.Send(-1, player.whoAmI);
			}

			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.WireCutter)
				.AddIngredient<Miscellaneous.ChoziteBar>(5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
