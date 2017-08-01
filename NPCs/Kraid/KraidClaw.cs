using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Kraid
{
    public class KraidClaw : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kraid");
		}

		public override void SetDefaults()
		{
			npc.width = 26;
			npc.height = 26;
			npc.scale = 1f;
			npc.damage = 30;
			npc.defense = 0;
			npc.lifeMax = 1;
			npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.noGravity = true;
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.behindTiles = false;
			npc.aiStyle = -1;
			npc.npcSlots = 1;
		}

		float rotation = 0f;
		public override void AI()
		{
			rotation += 0.25f*npc.direction;
		}
		
		/*public override void NPCLoot()
		{
			Player player = Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)];
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			bool flag = false;
            for(int i = 0; i < player.inventory.Length; i++)
			{
				if(player.inventory[i].type == mod.ItemType("MissileLauncher"))
				{
					flag = true;
				}
			}
			if(flag)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MissilePickup"), 1 + Main.rand.Next(5));
			}
		}*/
		
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			Texture2D tex = Main.npcTexture[npc.type];
			SpriteEffects effects = SpriteEffects.None;
			if (npc.direction == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			sb.Draw(tex, npc.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), npc.GetAlpha(Color.White), rotation, new Vector2((float)tex.Width/2f, (float)tex.Height/2f), npc.scale, effects, 0f);
			return false;
		}
	}
}