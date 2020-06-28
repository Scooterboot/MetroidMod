using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace MetroidMod.Items.damageclass
{
	public abstract class HunterDamageItem : ModItem
	{
		//public override bool CloneNewInstances => true;

		public virtual void SafeSetDefaults() {}

		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			add += HunterDamagePlayer.ModPlayer(player).hunterDamageAdd;
			mult *= HunterDamagePlayer.ModPlayer(player).hunterDamageMult;
		}

		public override void GetWeaponKnockback(Player player, ref float knockback)
		{
			knockback += HunterDamagePlayer.ModPlayer(player).hunterKnockback;
		}

		public override void GetWeaponCrit(Player player, ref int crit)
		{
			crit += HunterDamagePlayer.ModPlayer(player).hunterCrit;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " hunter " + damageWord;
			}
			
			if(item.accessory || item.GetGlobalItem<MGlobalItem>().ballSlotType >= 0)
			{
				for (int k = 0; k < tooltips.Count; k++)
				{
					if(tooltips[k].Name == "Knockback" && item.knockBack <= 0)
					{
						tooltips[k].text = "";
					}
					if(tooltips[k].Name == "CritChance" && item.crit <= 0)
					{
						tooltips[k].text = "";
					}
					if(tooltips[k].Name == "Speed")
					{
						tooltips[k].text = "";
					}
				}
			}
		}
		
		public override bool AllowPrefix(int pre)
		{
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
			if(mi.ballSlotType >= 0 || mi.addonSlotType >= 0 || mi.missileSlotType >= 0)
			{
				return false;
			}
			return true;
		}
		
		// by default, items with custom damage types cannot get prefixes (tml bug or feature?)
		// so i'm just gonna manually allow it to use any ranged weapon prefix by literally copy-pasting vanilla code
		// except... modded prefixes for ranged weapons won't work because internal shenanigans
		public override int ChoosePrefix(UnifiedRandom rand)
		{
			int num = -1;
			
			int expr_155F = rand.Next(36);
			if (expr_155F == 0)
			{
				num = 16;
			}
			if (expr_155F == 1)
			{
				num = 17;
			}
			if (expr_155F == 2)
			{
				num = 18;
			}
			if (expr_155F == 3)
			{
				num = 19;
			}
			if (expr_155F == 4)
			{
				num = 20;
			}
			if (expr_155F == 5)
			{
				num = 21;
			}
			if (expr_155F == 6)
			{
				num = 22;
			}
			if (expr_155F == 7)
			{
				num = 23;
			}
			if (expr_155F == 8)
			{
				num = 24;
			}
			if (expr_155F == 9)
			{
				num = 25;
			}
			if (expr_155F == 10)
			{
				num = 58;
			}
			if (expr_155F == 11)
			{
				num = 36;
			}
			if (expr_155F == 12)
			{
				num = 37;
			}
			if (expr_155F == 13)
			{
				num = 38;
			}
			if (expr_155F == 14)
			{
				num = 53;
			}
			if (expr_155F == 15)
			{
				num = 54;
			}
			if (expr_155F == 16)
			{
				num = 55;
			}
			if (expr_155F == 17)
			{
				num = 39;
			}
			if (expr_155F == 18)
			{
				num = 40;
			}
			if (expr_155F == 19)
			{
				num = 56;
			}
			if (expr_155F == 20)
			{
				num = 41;
			}
			if (expr_155F == 21)
			{
				num = 57;
			}
			if (expr_155F == 22)
			{
				num = 42;
			}
			if (expr_155F == 23)
			{
				num = 43;
			}
			if (expr_155F == 24)
			{
				num = 44;
			}
			if (expr_155F == 25)
			{
				num = 45;
			}
			if (expr_155F == 26)
			{
				num = 46;
			}
			if (expr_155F == 27)
			{
				num = 47;
			}
			if (expr_155F == 28)
			{
				num = 48;
			}
			if (expr_155F == 29)
			{
				num = 49;
			}
			if (expr_155F == 30)
			{
				num = 50;
			}
			if (expr_155F == 31)
			{
				num = 51;
			}
			if (expr_155F == 32)
			{
				num = 59;
			}
			if (expr_155F == 33)
			{
				num = 60;
			}
			if (expr_155F == 34)
			{
				num = 61;
			}
			if (expr_155F == 35)
			{
				num = 82;
			}
			
			/*ModPrefix.Roll(this, ref num, 36, new PrefixCategory[]
			{
				PrefixCategory.AnyWeapon,
				PrefixCategory.Ranged
			});*/
			// this is where mod prefixes are handled, and this would normally work
			// except that "ModPrefix.Roll" is marked as internal and not public
			// so i'd have to do a hacky workaround to actually make it work
			
			return num;
		}
	}
}