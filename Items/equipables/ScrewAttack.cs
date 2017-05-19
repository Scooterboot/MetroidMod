using Terraria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Items.equipables
{
	public class ScrewAttack : ModItem
	{
		bool screwAttack = false;
		int screwAttackSpeed = 0;
		int screwSpeedDelay = 0;
		int proj = -1;
		public override void SetDefaults()
		{
			item.name = "Screw Attack";
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.toolTip = "Allows the user to double jump\n" + 
			"Allows somersaulting\n" + 
			"Damage enemies while someraulting\n" + 
			"Hold Left/Right and double jump to do a 'boost' ability";
			item.value = 40000;
			item.rare = 7;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			mp.screwAttackSpeedEffect = screwAttackSpeed;
			mp.screwAttack = screwAttack;
			if(mp.somersault /*&& mp.spaceJumped*/)
			{
				screwAttack = false;
				player.longInvince = true;
				int screwAttackID = mod.ProjectileType("ScrewAttackProj");
				foreach(Projectile P in Main.projectile)
				{
					if(P.active && P.owner==player.whoAmI && P.type == screwAttackID)
					{
						screwAttack = true;
						break;
					}
				}
				if(!screwAttack)
				{
					proj = Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,screwAttackID,mp.specialDmg,0,player.whoAmI);
				}
			}
			if(screwSpeedDelay <= 0 && !mp.ballstate && player.grappling[0] == -1 && player.velocity.Y != 0f && !player.mount.Active)
			{
				if(player.controlJump && player.releaseJump && System.Math.Abs(player.velocity.X) > 2.5f)
				{
					screwSpeedDelay = 20;
				}
			}
			if(screwSpeedDelay > 0)
			{
				if(player.jump > 1 && ((player.velocity.Y < 0 && player.gravDir == 1) || (player.velocity.Y > 0 && player.gravDir == -1)) && screwSpeedDelay >= 19 && mp.somersault)
				{
					screwAttackSpeed = 60;
				}
				screwSpeedDelay--;
			}
			if(screwAttackSpeed > 0)
			{
				if (player.controlLeft)
				{
					if (player.velocity.X < -2 && player.velocity.X > -8*player.moveSpeed)
					{
						player.velocity.X -= 0.2f;
						player.velocity.X -= (float) 0.02+((player.moveSpeed-1f)/10);
					}
				}
				else if (player.controlRight)
				{
					if (player.velocity.X > 2 && player.velocity.X < 8*player.moveSpeed)
					{
						player.velocity.X += 0.2f;
						player.velocity.X += (float) 0.02+((player.moveSpeed-1f)/10);
					}
				}
				for(int i = 0; i < (screwAttackSpeed/20); i++)
				{
					if(proj != -1)
					{
						Projectile P = Main.projectile[proj];
						if(P.active && P.owner == player.whoAmI && P.type == mod.ProjectileType("ScrewAttackProj"))
						{
							Color color = new Color();
							int dust = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 57, -player.velocity.X * 0.5f, -player.velocity.Y * 0.5f, 100, color, 2f);
							Main.dust[dust].noGravity = true;
							if(i == ((screwAttackSpeed/20)-1) && screwAttackSpeed == 59)
							{
								Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/ScrewAttackSpeedSound"));
							}
						}
					}
				}
				screwAttackSpeed--;
			}
			mp.AddSpaceJump(player);
		}
	}
}