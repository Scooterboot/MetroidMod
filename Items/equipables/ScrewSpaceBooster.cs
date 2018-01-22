using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.equipables
{
    public class ScrewSpaceBooster : ModItem
    {
        bool screwAttack = false;
        int screwAttackSpeed = 0;
        int screwSpeedDelay = 0;
        int proj = -1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Space Boosted Screw Attack");
            Tooltip.SetDefault("Allows the user to run insanely fast\n" +
            "Allows somersaulting\n" +
            "Damage enemies while running or somersaulting\n" +
            "Damage increases with enemy's damage\n" +
            "Allows the user to jump up to 10 times in a row\n" +
            "Jumps recharge mid-air\n" +
            "Holding left/right while jumping midair gives a boost");
        }
        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 38;
            item.maxStack = 1;
            item.value = 80000;
            item.rare = 8;
            item.accessory = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ScrewAttack");
            recipe.AddIngredient(null, "SpaceBooster");
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.speedBooster = true;
            mp.AddSpeedBoost(player);
            mp.AddSpaceJumping(player);
            mp.screwAttackSpeedEffect = screwAttackSpeed;
            mp.screwAttack = screwAttack;
            if (mp.somersault /*&& mp.spaceJumped*/)
            {
                screwAttack = false;
                player.longInvince = true;
                int screwAttackID = mod.ProjectileType("ScrewAttackProj");
                foreach (Projectile P in Main.projectile)
                {
                    if (P.active && P.owner == player.whoAmI && P.type == screwAttackID)
                    {
                        screwAttack = true;
                        break;
                    }
                }
                if (!screwAttack)
                {
                    proj = Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y + player.height / 2, 0, 0, screwAttackID, mp.specialDmg, 0, player.whoAmI);
                }
            }
            if (screwSpeedDelay <= 0 && !mp.ballstate && player.grappling[0] == -1 && player.velocity.Y != 0f && !player.mount.Active)
            {
                if (player.controlJump && player.releaseJump && System.Math.Abs(player.velocity.X) > 2.5f)
                {
                    screwSpeedDelay = 20;
                }
            }
            if (screwSpeedDelay > 0)
            {
                if (player.jump > 1 && ((player.velocity.Y < 0 && player.gravDir == 1) || (player.velocity.Y > 0 && player.gravDir == -1)) && screwSpeedDelay >= 19 && mp.somersault)
                {
                    screwAttackSpeed = 60;
                }
                screwSpeedDelay--;
            }
            if (screwAttackSpeed > 0)
            {
                if (player.controlLeft)
                {
                    if (player.velocity.X < -2 && player.velocity.X > -8 * player.moveSpeed)
                    {
                        player.velocity.X -= 0.2f;
                        player.velocity.X -= (float)0.02 + ((player.moveSpeed - 1f) / 10);
                    }
                }
                else if (player.controlRight)
                {
                    if (player.velocity.X > 2 && player.velocity.X < 8 * player.moveSpeed)
                    {
                        player.velocity.X += 0.2f;
                        player.velocity.X += (float)0.02 + ((player.moveSpeed - 1f) / 10);
                    }
                }
                for (int i = 0; i < (screwAttackSpeed / 20); i++)
                {
                    if (proj != -1)
                    {
                        Projectile P = Main.projectile[proj];
                        if (P.active && P.owner == player.whoAmI && P.type == mod.ProjectileType("ScrewAttackProj"))
                        {
                            Color color = new Color();
                            int dust = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 57, -player.velocity.X * 0.5f, -player.velocity.Y * 0.5f, 100, color, 2f);
                            Main.dust[dust].noGravity = true;
                            if (i == ((screwAttackSpeed / 20) - 1) && screwAttackSpeed == 59)
                            {
                                Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/ScrewAttackSpeedSound"));
                            }
                        }
                    }
                }
                screwAttackSpeed--;
            }
        }
        public override bool CanEquipAccessory(Player player, int slot)
        {
            for (int k = 3; k < 8 + player.extraAccessorySlots; k++)
            {
                if (player.armor[k].type == mod.ItemType("SpeedBooster") || player.armor[k].type == mod.ItemType("SpaceJump") || player.armor[k].type == mod.ItemType("SpaceBooster") || player.armor[k].type == mod.ItemType("ScrewAttack") || player.armor[k].type == mod.ItemType("TerraBooster"))
                {
                    return false;
                }
            }
            return true;

        }

    }
}