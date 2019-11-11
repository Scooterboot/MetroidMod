using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;

using MetroidMod.Items;
using MetroidMod.Items.weapons;

namespace MetroidMod.NewUI
{
    public class MissileLauncherUI : UIState
    {
        public static bool visible
        {
            get { return Main.playerInventory && Main.LocalPlayer.inventory[((MetroidMod)(MetroidMod.Instance)).selectedItem].type == ModLoader.GetMod("MetroidMod").ItemType("MissileLauncher"); }
        }

        MissileLauncherPanel missileLauncherPanel;

        public override void OnInitialize()
        {
            missileLauncherPanel = new MissileLauncherPanel();
            missileLauncherPanel.Initialize();

            this.Append(missileLauncherPanel);
        }
    }

    public class MissileLauncherPanel : UIPanel
    {
        Texture2D panelTexture;

        public MissileLauncherItemBox[] missileSlots;

        public Rectangle drawRectangle
        {
            get { return new Rectangle((int)this.Left.Pixels, (int)this.Top.Pixels, (int)this.Width.Pixels, (int)this.Height.Pixels); }
        }

        public Vector2[] itemBoxPositionValues = new Vector2[MetroidMod.missileSlotAmount]
        {
            new Vector2(106, 94),
            new Vector2(38, 42),
            new Vector2(178, 42)
        };

        public override void OnInitialize()
        {
            panelTexture = ModContent.GetTexture("MetroidMod/Textures/UI/MissileLauncher_Border");

            this.SetPadding(0);
            this.Left.Pixels = 160;
            this.Top.Pixels = 260;
            this.Width.Pixels = panelTexture.Width;
            this.Height.Pixels = panelTexture.Height;

            missileSlots = new MissileLauncherItemBox[MetroidMod.missileSlotAmount];
            for (int i = 0; i < MetroidMod.missileSlotAmount; ++i)
            {
                missileSlots[i] = new MissileLauncherItemBox();
                missileSlots[i].Top.Pixels = itemBoxPositionValues[i].Y;
                missileSlots[i].Left.Pixels = itemBoxPositionValues[i].X;
                missileSlots[i].missileSlotType = i;
                missileSlots[i].SetCondition();

                this.Append(missileSlots[i]);
            }

            this.Append(new MissileLauncherFrame());
            this.Append(new MissileLauncherLines());
        }

        public override void Update(GameTime gameTime)
        {
            this.Top.Pixels = 260;
            if (Main.LocalPlayer.chest != -1 || Main.npcShop != 0)
			{
                this.Top.Pixels += 170;
			}

            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(panelTexture, drawRectangle, Color.White);
        }
    }

    public class MissileLauncherItemBox : UIPanel
    {
        Texture2D itemBoxTexture;

        public Condition condition;

        public int missileSlotType;

        public Rectangle drawRectangle
        {
            get { return new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels); }
        }

        public delegate bool Condition(Item item);
        public override void OnInitialize()
        {
            itemBoxTexture = ModContent.GetTexture("MetroidMod/Textures/UI/ItemBox");

            Width.Pixels = itemBoxTexture.Width; Height.Pixels = itemBoxTexture.Height;
            this.OnClick += ItemBoxClick;
        }

        public override void Update(GameTime gameTime)
        {
            // Ignore mouse input.
            if (base.IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;
        }

        public void SetCondition()
        {
            this.condition = delegate (Item addonItem)
            {
                Mod mod = ModLoader.GetMod("MetroidMod");
                if (addonItem.modItem != null && addonItem.modItem.mod == mod)
                {
                    MGlobalItem mItem = addonItem.GetGlobalItem<MGlobalItem>();
                    Main.NewText("LOOKING FOR TYPE: " + this.missileSlotType + " GOT TYPE: " + mItem.missileSlotType, Color.Orange);
                    return (addonItem.type <= 0 || mItem.missileSlotType == this.missileSlotType);
                }
                return (addonItem.type <= 0 || (addonItem.modItem != null && addonItem.modItem.mod == mod));
            };
        }

        // Clicking functionality.
        private void ItemBoxClick(UIMouseEvent evt, UIElement e)
        {
            // No failsafe. Should maybe be implemented?
            MissileLauncher missileLauncherTarget = Main.LocalPlayer.inventory[((MetroidMod)MetroidMod.Instance).selectedItem].modItem as MissileLauncher;

            if (missileLauncherTarget.missileMods[missileSlotType] != null && !missileLauncherTarget.missileMods[missileSlotType].IsAir)
            {
                if (Main.mouseItem.IsAir)
                {
                    Main.PlaySound(SoundID.Grab);
                    Main.mouseItem = missileLauncherTarget.missileMods[missileSlotType].Clone();

                    missileLauncherTarget.missileMods[missileSlotType].TurnToAir();
                }
                else if (condition == null || (condition != null && condition(Main.mouseItem)))
                {
                    Main.PlaySound(SoundID.Grab);

                    if (Main.mouseItem.type == missileLauncherTarget.missileMods[missileSlotType].type)
                    {
                        int stack = Main.mouseItem.stack + missileLauncherTarget.missileMods[missileSlotType].stack;

                        if(missileLauncherTarget.missileMods[missileSlotType].maxStack >= stack)
                        {
                            missileLauncherTarget.missileMods[missileSlotType].stack = stack;
                            Main.mouseItem.TurnToAir();
                        }
                        else
                        {
                            int stackDiff = stack - missileLauncherTarget.missileMods[missileSlotType].maxStack;
                            missileLauncherTarget.missileMods[missileSlotType].stack = missileLauncherTarget.missileMods[missileSlotType].maxStack;
                            Main.mouseItem.stack = stackDiff;
                        }
                    }
                    else
                    {
                        Item tempBoxItem = missileLauncherTarget.missileMods[missileSlotType].Clone();
                        Item tempMouseItem = Main.mouseItem.Clone();

                        missileLauncherTarget.missileMods[missileSlotType] = tempMouseItem;
                        Main.mouseItem = tempBoxItem;
                    }
                }
            }
            else if (!Main.mouseItem.IsAir)
            {
                if (condition == null || (condition != null && condition(Main.mouseItem)))
                {
                    Main.PlaySound(SoundID.Grab);
                    missileLauncherTarget.missileMods[missileSlotType] = Main.mouseItem.Clone();
                    Main.mouseItem.TurnToAir();
                }
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            MissileLauncher missileLauncherTarget = Main.LocalPlayer.inventory[((MetroidMod)MetroidMod.Instance).selectedItem].modItem as MissileLauncher;

            spriteBatch.Draw(itemBoxTexture, drawRectangle, Color.White);

            // Item drawing.
            if (missileLauncherTarget.missileMods[missileSlotType].IsAir) return;

            Color itemColor = missileLauncherTarget.missileMods[missileSlotType].GetAlpha(Color.White);
            Texture2D itemTexture = Main.itemTexture[missileLauncherTarget.missileMods[missileSlotType].type];
            CalculatedStyle innerDimensions = base.GetDimensions();

            if (base.IsMouseHovering)
            {
                Main.hoverItemName = missileLauncherTarget.missileMods[missileSlotType].Name;
                Main.HoverItem = missileLauncherTarget.missileMods[missileSlotType].Clone();
            }

            var frame = Main.itemAnimations[missileLauncherTarget.missileMods[missileSlotType].type] != null
                        ? Main.itemAnimations[missileLauncherTarget.missileMods[missileSlotType].type].GetFrame(itemTexture)
                        : itemTexture.Frame(1, 1, 0, 0);

            float drawScale = 1f;
            if ((float)frame.Width > innerDimensions.Width || (float)frame.Height > innerDimensions.Width)
            {
                if (frame.Width > frame.Height)
                    drawScale = innerDimensions.Width / (float)frame.Width;
                else
                    drawScale = innerDimensions.Width / (float)frame.Height;
            }

            var unreflectedScale = drawScale;
            var tmpcolor = Color.White;

            ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, missileLauncherTarget.missileMods[missileSlotType].type);

            Vector2 drawPosition = new Vector2(innerDimensions.X, innerDimensions.Y);

            drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
            drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

            spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
                Vector2.Zero, drawScale, SpriteEffects.None, 0f);

            if(missileLauncherTarget.missileMods[missileSlotType].stack > 1)
            {
                Utils.DrawBorderStringFourWay(
                    spriteBatch,
                    Main.fontItemStack,
                    Math.Min(9999, missileLauncherTarget.missileMods[missileSlotType].stack).ToString(),
                    innerDimensions.Position().X + 10f,
                    innerDimensions.Position().Y + 26f,
                    Color.White,
                    Color.Black,
                    Vector2.Zero,
                    unreflectedScale * 0.8f);
            }
        }
    }

    /*
     * The classes in the following section do not have any functionality besides visual aesthetics.
     */
    public class MissileLauncherFrame : UIPanel
    {
        Texture2D missileLauncherFrame;

        public Rectangle drawRectangle
        {
            get { return new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels); }
        }

        public override void OnInitialize()
        {
            missileLauncherFrame = ModContent.GetTexture("MetroidMod/Textures/UI/MissileLauncher_Frame");

            this.Width.Pixels = missileLauncherFrame.Width;
            this.Height.Pixels = missileLauncherFrame.Height;

            // Hardcoded position values.
            this.Top.Pixels = 52;
            this.Left.Pixels = 118;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(missileLauncherFrame, drawRectangle, Color.White);
        }
    }
    public class MissileLauncherLines : UIPanel
    {
        Texture2D missileLauncherLines;

        public Rectangle drawRectangle
        {
            get { return new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels); }
        }

        public override void OnInitialize()
        {
            missileLauncherLines = ModContent.GetTexture("MetroidMod/Textures/UI/MissileLauncher_Lines");

            this.Width.Pixels = missileLauncherLines.Width;
            this.Height.Pixels = missileLauncherLines.Height;

            // Hardcoded position values.
            this.Top.Pixels = 0;
            this.Left.Pixels = 0;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(missileLauncherLines, drawRectangle, Color.White);
        }
    }
}
