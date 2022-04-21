using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace MetroidModPorted.Common.Systems
{
	internal class MUISystem : ModSystem
	{
		public static MUISystem Instance { get; private set; }
		internal static UserInterface pbUserInterface;
		//internal static UI.PowerBeamUI powerBeamUI;
		//internal static UI.AddonsUI addonsUI;

		//internal static UserInterface mlUserInterface;
		//internal static UI.MissileLauncherUI missileLauncherUI;

		//internal static UserInterface mbUserInterface;
		//internal static UI.MorphBallUI morphBallUI;

		//internal static UserInterface smUserInterface;
		//internal static UI.SenseMoveUI senseMoveUI;

		internal bool isPBInit = false;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				//addonsUI = new UI.AddonsUI();
				pbUserInterface = new UserInterface();

				/*powerBeamUI = new UI.PowerBeamUI();
				powerBeamUI.Activate();
				pbUserInterface = new UserInterface();
				pbUserInterface.SetState(powerBeamUI);*/

				/*missileLauncherUI = new UI.MissileLauncherUI();
				missileLauncherUI.Activate();
				mlUserInterface = new UserInterface();
				mlUserInterface.SetState(missileLauncherUI);

				morphBallUI = new UI.MorphBallUI();
				morphBallUI.Activate();
				mbUserInterface = new UserInterface();
				mbUserInterface.SetState(morphBallUI);

				senseMoveUI = new UI.SenseMoveUI();
				senseMoveUI.Activate();
				smUserInterface = new UserInterface();
				smUserInterface.SetState(senseMoveUI);*/
			}
		}

		public override void Unload()
		{
			pbUserInterface = null;
			//powerBeamUI = null;
			//addonsUI = null;
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (true && isPBInit == false)
			{
				pbUserInterface.SetState(new UI.PowerBeamUI());
				isPBInit = true;
			}
			if (pbUserInterface != null && UI.PowerBeamUI.Visible)
			{
				pbUserInterface.Update(gameTime);
			}
		}

		//int lastSeenScreenWidth;
		//int lastSeenScreenHeight;
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			// Draws the Music Player UI.
			int index = layers.FindIndex((GameInterfaceLayer layer) => layer.Name.Equals("Vanilla: Mouse Text"));
			if (index != -1)
			{
				layers.Insert(index, new LegacyGameInterfaceLayer(
					"MetroidModPorted: Power Beam UI",
					delegate {
						if (UI.PowerBeamUI.Visible)// && !Main.recBigList)
						{
							if (Main.hasFocus) { pbUserInterface.Recalculate(); }
							pbUserInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						}

						return true;
					},
					InterfaceScaleType.UI)
				);
				/*layers.Insert(index, new LegacyGameInterfaceLayer(
					"MetroidModPorted: Missile Launcher UI",
					delegate {
						if (UI.MissileLauncherUI.visible)
						{
							if (Main.hasFocus) { mlUserInterface.Recalculate(); }
							mlUserInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);*/
				/*layers.Insert(index, new LegacyGameInterfaceLayer(
					"MetroidModPorted: Missile Launcher UI",
					delegate {
						if (UI.MorphBallUI.visible)
						{
							if (Main.hasFocus) { mbUserInterface.Recalculate(); }
							mbUserInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);*/
			}
		}
	}
}
