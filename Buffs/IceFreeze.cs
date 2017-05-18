using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria.ModLoader;
using Terraria;

namespace MetroidMod.Buffs
{
    public class IceFreeze : ModBuff
    {     
		//int oldNPCai = -1;
		//int oldNPCdmg = 0;
		//Color oldColor = default(Color);
		float speedDecrease = 0.8f;
		//bool oldGravity = false;
		//bool oldCollision = false;
		int oldDir = 1;
		bool canFreeze = true;
		bool isSkeletronArm = false;
		public override void SetDefaults()
		{
			Main.buffName[Type] = "Froze";
			Main.buffTip[Type] = "You Got Ice Beam'd!";
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
		/*public override void Start(NPC N,int buffIndex)
		{
			oldNPCai = N.aiStyle;
			oldNPCdmg = N.damage;
			oldColor = N.color;
			oldGravity = N.noGravity;
			oldCollision = N.noTileCollide;
			oldDir = N.direction;
			isSkeletronArm = (N.aiStyle == 12 || N.aiStyle == 33 || N.aiStyle == 34 || N.aiStyle == 35 || N.aiStyle == 36);
			canFreeze = (!N.dontTakeDamage && !N.boss && N.lifeMax < 3000 && !isSkeletronArm &&
			N.type != 143 && N.type != 144 && N.type != 145 && N.type != 146 && N.aiStyle != 6 && !N.buffImmune[44]);
			if(canFreeze)
			{
				int dustID = Dust.NewDust(N.position, N.width, N.height, 59, N.velocity.X * 0.2f, N.velocity.Y * 0.2f, 100, new Color(), 2f);
			}
			else
			{
				N.buffImmune[BuffDef.byName["MetroidMod:IceFreeze"]] = true;
				N.buffImmune[BuffDef.byName["MetroidMod:InstantFreeze"]] = true;
			}
			if(N.buffType[buffIndex] == BuffDef.byName["MetroidMod:InstantFreeze"])
			{
				speedDecrease = 0f;
			}
			if(N.type == NPCDef.byName["MetroidMod:LarvalMetroid"].type)
			{
				speedDecrease = 1f;
			}
	}*/
		public override bool ReApply(NPC N, int time, int buffIndex)
		{
			isSkeletronArm = (N.aiStyle == 12 || N.aiStyle == 33 || N.aiStyle == 34 || N.aiStyle == 35 || N.aiStyle == 36);
			canFreeze = (!N.dontTakeDamage && !N.boss && N.lifeMax < 3000 && !isSkeletronArm &&	N.type != 143 && N.type != 144 && N.type != 145 && N.type != 146 && N.aiStyle != 6 && !N.buffImmune[44]);
			if(canFreeze)
			{
				if(speedDecrease > 0)
				{
					speedDecrease -= 0.2f;
				}
				else
				{
					speedDecrease = 0;
				}
				if(N.type == mod.NPCType("LarvalMetroid"))
				{
					speedDecrease = 1f;
				}
				int dustID = Dust.NewDust(N.position, N.width, N.height, 59, N.velocity.X * 0.2f, N.velocity.Y * 0.2f, 100, new Color(), 2f);
			}
			if(!canFreeze)
			{
				N.buffImmune[mod.BuffType("MetroidMod:IceFreeze")] = true;
				N.buffImmune[mod.BuffType("MetroidMod:InstantFreeze")] = true;
			}
			return false;
		}
		public override void Update(NPC N,ref int buffIndex)
		{
			isSkeletronArm = (N.aiStyle == 12 || N.aiStyle == 33 || N.aiStyle == 34 || N.aiStyle == 35 || N.aiStyle == 36);
			canFreeze = (!N.dontTakeDamage && !N.boss && N.lifeMax < 3000 && !isSkeletronArm &&	N.type != 143 && N.type != 144 && N.type != 145 && N.type != 146 && N.aiStyle != 6 && !N.buffImmune[44]);
			if(canFreeze)
			{
				N.velocity.X = N.velocity.X * speedDecrease;
				if(N.noGravity)
				{
					N.velocity.Y = N.velocity.Y * speedDecrease;
				}
				if(speedDecrease <= 0 || N.type == mod.NPCType("LarvalMetroid"))
				{
					N.damage = 0;
					N.aiStyle = 0;
					N.frame.Y = 0;
					N.noGravity = false;
					N.noTileCollide = false;
					N.direction = oldDir;
					N.spriteDirection = oldDir;
				}
				else
				{
					oldDir = N.direction;
				}
				Color color = new Color(0, 144, 255, 100);
				N.color = color;
			}
			else
			{
				N.buffImmune[mod.BuffType("MetroidMod:IceFreeze")] = true;
				N.buffImmune[mod.BuffType("MetroidMod:InstantFreeze")] = true;
			}
		}
		/*public override void End(NPC N,int buffIndex)
		{
			if(canFreeze || N.type == NPCDef.byName["MetroidMod:LarvalMetroid"].type)
			{
				N.color = oldColor;
				N.aiStyle = oldNPCai;
				N.damage = oldNPCdmg;
				N.noGravity = oldGravity;
				N.noTileCollide = oldCollision;
			}
			else
			{
				N.buffImmune[mod.BuffType("MetroidMod:IceFreeze")] = true;
				N.buffImmune[mod.BuffType("MetroidMod:InstantFreeze")] = true;
			}
		}*/
    }
}