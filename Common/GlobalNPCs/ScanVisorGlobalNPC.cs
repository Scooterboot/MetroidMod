using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidMod.Common.Players;

namespace MetroidMod.Common.GlobalNPCs
{
	internal class ScanVisorGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		int timer = 0;
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
			boundingBox.X -= 25;
			boundingBox.Y -= 25;
			boundingBox.Width += 50;
			boundingBox.Height += 50;
			mp.ScanProgress = 0f;
			if (boundingBox.Contains(new Point((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y)))
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
