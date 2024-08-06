using MetroidMod.Content.Hatches.Variants;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MetroidMod.Content.Hatches
{
	internal class HatchTileEntity : ModTileEntity, IHatchProvider, IHatchOpenController, IHatchVisualController
	{
		private HatchTile HatchTile => ModContent.GetModTile(Main.tile[Position].TileType) as HatchTile;

		/// <summary>
		/// The ModHatch that this tile entity is controlling.
		/// </summary>
		public ModHatch Hatch => HatchTile.Hatch;


		private HatchBehavior _behavior;
		public HatchBehavior Behavior
		{
			get {
				_behavior ??= new(this, this, this);
				return _behavior;
			}
		}


		public bool IsOpen => HatchTile.Open;

		private HatchAnimation animation;
		public HatchAnimation Animation
		{
			get {
				animation ??= new HatchAnimation(IsOpen);
				return animation;
			}
		}

		private HatchVisualState _visualState;
		private IHatchAppearance _appearance;
		public IHatchAppearance Appearance
		{
			get {
				if (_appearance == null) SetVisualState(_visualState);
				return _appearance;
			}
		}

		private HatchAutoclose autoclose;
		private HatchAutoclose Autoclose
		{
			get {
				autoclose ??= new HatchAutoclose(this);
				return autoclose;
			}
		}

		private Vector2 Center => (Position + new Terraria.DataStructures.Point16(2, 2)).ToWorldCoordinates(0, 0);

		public void Open()
		{
			if (IsOpen) return;
			UpdateTiles(true);
			Animation.Open();
			Autoclose.Open();
			SoundEngine.PlaySound(Sounds.Tiles.HatchOpen, Center);
		}

		public void Close()
		{
			if (!IsOpen) return;
			UpdateTiles(false);
			Animation.Close();
			SoundEngine.PlaySound(Sounds.Tiles.HatchClose, Center);
		}

		public override void Update()
		{
			Animation.Update();
			Appearance.Update();
			Autoclose.Update();
		}

		public override void SaveData(TagCompound tag)
		{
			if (Behavior.BlueConversion != default) tag["BlueConversion"] = (int)Behavior.BlueConversion;
			if (Behavior.Locked) tag["Locked"] = Behavior.Locked;
			if (_visualState != default) tag["_visualState"] = (int)_visualState;
		}

		public override void LoadData(TagCompound tag)
		{
			Behavior.BlueConversion = (HatchBlueConversionStatus)tag.Get<int>("BlueConversion");
			Behavior.Locked = tag.Get<bool>("Locked");
			SetVisualState((HatchVisualState)tag.Get<int>("_visualState"));
		}


		public override bool IsTileValidForEntity(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return tile.HasTile && ModContent.GetModTile(tile.TileType) is HatchTile;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 4, 4);

				// Sync the placement of the tile entity with other clients
				// The "type" parameter refers to the tile type which placed the tile entity, so "Type" (the type of the tile entity) needs to be used here instead
				NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
				return -1;
			}

			// ModTileEntity.Place() handles checking if the entity can be placed, then places it for you
			int placedEntity = Place(i, j);
			return placedEntity;
		}

		public override void OnNetPlace()
		{
			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
			}
		}

		private void UpdateTiles(bool toOpenTiles)
		{
			ushort type = (ushort)Hatch.GetTileType(toOpenTiles, HatchTile.Vertical);
			HatchTilePlacement.SetHatchTilesAt(type, Position.X, Position.Y);
		}

		private IHatchAppearance CurrentAppearance => Behavior.IsTurnedBlue ? 
			ModContent.GetInstance<BlueHatch>().DefaultAppearance : Hatch.DefaultAppearance;

		public void SetVisualState(HatchVisualState state)
		{
			_visualState = state;

			switch (state)
			{
				default:
				case HatchVisualState.Current:
					_appearance = CurrentAppearance;
					break;
				case HatchVisualState.Locked:
					_appearance = new HatchAppearance("LockedHatch");
					break;
				case HatchVisualState.Blinking:
					_appearance = new HatchBlinkingAppearance(CurrentAppearance, new HatchAppearance("LockedHatch"));
					break;
			}
		}
	}
}
