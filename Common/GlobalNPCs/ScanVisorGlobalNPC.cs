using MetroidMod.Common.Players;
using MetroidMod.Content.SuitAddons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Common.GlobalNPCs
{
	internal class ScanVisorGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		private int timer = 0;
		internal static bool soundIsPlaying = false;
		internal static bool soundShouldPlay = false;
		internal static ActiveSound sound;
		public override void ModifyHoverBoundingBox(NPC npc, ref Rectangle boundingBox)
		{
			if (!Main.LocalPlayer.TryGetModPlayer(out MPlayer mp) ||
				!SuitAddonLoader.TryGetAddon<Content.SuitAddons.ScanVisor>(out ModSuitAddon scanMsa) ||
				mp.VisorInUse != scanMsa.Type)
			{
				if (sound != null && sound.IsPlaying)
				{
					sound.Sound.Stop(true);
					soundIsPlaying = false;
				}
				return;
			}

			float MX = Main.screenPosition.X + ((Main.mouseX + ((Main.mouseX - (Main.screenWidth * 0.5f)) * ((1 / Main.GameViewMatrix.Zoom.X) - 1f))) * Main.UIScale);
			float MY = Main.screenPosition.Y + ((Main.mouseY + ((Main.mouseY - (Main.screenHeight * 0.5f)) * ((1 / Main.GameViewMatrix.Zoom.Y) - 1f))) * Main.UIScale);
			if (Main.LocalPlayer.gravDir == -1f)
			{
				MY = Main.screenPosition.Y + ((Main.screenHeight - (Main.mouseY + ((Main.mouseY - (Main.screenHeight * 0.5f)) * ((1 / Main.GameViewMatrix.Zoom.Y) - 1f)))) * Main.UIScale);
			}

			boundingBox.X -= 25;
			boundingBox.Y -= 25;
			boundingBox.Width += 50;
			boundingBox.Height += 50;
			mp.ScanProgress = 0f;
			if (boundingBox.Contains(new Point((int)MX, (int)MY)))
			{
				soundShouldPlay = true;
				if (npc.friendly)
				{
					if (npc.townNPC)
					{
						mp.ScanProgress = 1f;
						if (!Main.BestiaryTracker.Chats.GetWasChatWith(npc))
						{
							StartPlayingSFX();
							mp.ScanProgress = timer / 30f;
							if (timer >= 30)
							{
								Main.BestiaryTracker.Chats.RegisterChatStartWith(npc);
								SendUpdateBestiary(npc, 1);

								timer = 0;
								sound.Sound.Stop(true);
								soundIsPlaying = false;
							}
						}
					}
					else
					{
						mp.ScanProgress = 1f;
						if (!Main.BestiaryTracker.Sights.GetWasNearbyBefore(npc))
						{
							StartPlayingSFX();
							mp.ScanProgress = timer / 30f;
							if (timer >= 30)
							{
								Main.BestiaryTracker.Sights.RegisterWasNearby(npc);
								SendUpdateBestiary(npc, 2);
								timer = 0;
								sound.Sound.Stop(true);
								soundIsPlaying = false;
							}
						}
					}
				}
				else
				{
					StartPlayingSFX();
					int killCount = Main.BestiaryTracker.Kills.GetKillCount(npc);
					int killTotalNeeded = npc.boss ? 1 : 50;
					mp.ScanProgress = Utils.Clamp(1f * killCount / killTotalNeeded, 0f, 1f);
					if (killCount < killTotalNeeded && timer >= (killTotalNeeded == 1 ? 60 : 1.2))
					{
						Main.BestiaryTracker.Kills.RegisterKill(npc);
						SendUpdateBestiary(npc, 3);
						timer = 0;
					}
					if (killCount >= killTotalNeeded)
					{
						sound.Sound.Stop(true);
						soundIsPlaying = false;
					}
				}
				timer++;
			}
		}
		private void StartPlayingSFX()
		{
			if (!soundIsPlaying || !sound.IsPlaying)
			{
				SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Suit.Visors.ScanVisorScanning), out sound);
				soundIsPlaying = true;
			}
		}
		private void SendUpdateBestiary(NPC npc, byte hostilityType)
		{
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)MetroidMessageType.BestiaryUpdate);
				packet.Write(npc.type);
				packet.Write(hostilityType);
				packet.Send();
			}
		}
	}
}
