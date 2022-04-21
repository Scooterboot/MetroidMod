using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.DamageClasses
{
	public class HunterDamageClass : DamageClass
	{
		public override void SetStaticDefaults()
		{
			ClassName.SetDefault("hunter damage");
		}
		protected override float GetBenefitFrom(DamageClass damageClass)
		{
			if (damageClass == Generic)
				return 1f;
			return 0;
		}
		public override bool CountsAs(DamageClass damageClass)
		{
			return false;
		}
	}
}
