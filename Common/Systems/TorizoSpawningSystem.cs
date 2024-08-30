using System.IO;
using MetroidMod.Content.NPCs.GoldenTorizo;
using MetroidMod.Content.NPCs.Torizo;
using Microsoft.Xna.Framework;
using MonoMod.Core.Platforms;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MetroidMod.Common.Systems
{
	public class TorizoSpawningSystem : ModSystem
	{
		public bool Initialized;
		public Vector2 SpawnLocation;
		public Vector2 HeadLocation;
		public int Direction;
		private int spawnCounter;

		public virtual int NpcType => ModContent.NPCType<IdleTorizo>();
		public virtual bool CanShowIcon()
		{
			bool torizoIdle = NPC.AnyNPCs(ModContent.NPCType<IdleTorizo>());
			return torizoIdle;
		}

		public virtual bool CanSpawn()
		{
			bool torizoDowned = MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo);
			bool torizoAlive = NPC.AnyNPCs(ModContent.NPCType<Torizo>()) || NPC.AnyNPCs(ModContent.NPCType<IdleTorizo>());
			return Initialized && !torizoDowned && !torizoAlive;
		}

		public void SetLocationFromLegacy(Point location)
		{
			Rectangle roomArea = new(location.X, location.Y, 80, 40);
			Vector2 headLocation = roomArea.Center.ToWorldCoordinates();
			int direction = (roomArea.X < Main.maxTilesX / 2).ToDirectionInt();
			float spawnX = (direction == 1) ? (roomArea.X + 8) : (roomArea.Right - 8);
			Vector2 spawnLocation = new Vector2(spawnX, roomArea.Bottom - 4).ToWorldCoordinates(0, 0);
			UpdateLocation(spawnLocation, headLocation, direction);
		}

		public void UpdateLocation(Vector2 spawn, Vector2 head, int direction)
		{
			Initialized = true;
			SpawnLocation = spawn;
			HeadLocation = head;
			Direction = direction;
		}

		public void UpdateNpcAttributes(NPC npc)
		{
			npc.direction = Direction;
			npc.spriteDirection = npc.direction;

			npc.position.X = SpawnLocation.X - npc.width / 2;
			npc.position.Y = SpawnLocation.Y - npc.height;

		}

		public override void PostUpdateEverything()
		{
			if(!CanSpawn())
			{
				spawnCounter = 300;
				return;
			}

			if(spawnCounter > 0)
			{
				spawnCounter -= 1;
				return;
			}

			NPC.NewNPC(Entity.GetSource_NaturalSpawn(), (int)SpawnLocation.X, (int)SpawnLocation.Y, NpcType);
		}

		public override void LoadWorldData(TagCompound tag)
		{
			tag.TryGet("Initialized", out Initialized);
			tag.TryGet("SpawnLocation", out SpawnLocation);
			tag.TryGet("HeadLocation", out HeadLocation);
			tag.TryGet("Direction", out Direction);
		}

		public override void SaveWorldData(TagCompound tag)
		{
			tag.Set("Initialized", Initialized);
			tag.Set("SpawnLocation", SpawnLocation);
			tag.Set("HeadLocation", HeadLocation);
			tag.Set("Direction", Direction);
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.WriteVector2(SpawnLocation);
			writer.WriteVector2(HeadLocation);
			writer.Write(Direction);
		}

		public override void NetReceive(BinaryReader reader)
		{
			SpawnLocation = reader.ReadVector2();
			HeadLocation = reader.ReadVector2();
			Direction = reader.ReadInt32();
		}
	}

	public class GoldenTorizoSpawningSystem : TorizoSpawningSystem
	{
		public override int NpcType => ModContent.NPCType<IdleGoldenTorizo>();
		public override bool CanShowIcon()
		{
			bool goldenTorizoIdle = NPC.AnyNPCs(ModContent.NPCType<IdleGoldenTorizo>());
			return goldenTorizoIdle;
		}

		public override bool CanSpawn()
		{
			bool canGoldenSpawn = NPC.downedGolemBoss;
			bool goldenTorizoDowned = MSystem.bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo);
			bool goldenTorizoAlive = NPC.AnyNPCs(ModContent.NPCType<GoldenTorizo>()) || NPC.AnyNPCs(ModContent.NPCType<IdleGoldenTorizo>());
			return canGoldenSpawn && !goldenTorizoDowned && !goldenTorizoAlive;
		}
	}
}
