using DOSBOX.Suggestions.plants;
using DOSBOX.Suggestions.plants.Fruits;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DOSBOX.Suggestions
{
    public class Labs : IState
    {
        public class button
        {
            class DispClass : Disp
            {
                public DispClass(byte[,] g, vec vec) : base() { this.g = g; this.vec = new vec(vec); }
            }

            public string text;
            public vec vec;
            public byte graphicId;
            public Action Do;
            public Func<bool> EnableCondition;
            public int w_img => GraphicImages.W(graphicId);
            public int h_img => GraphicImages.H(graphicId);
            public int w_txt => text.Length * 5;
            public int h_txt => 3;
            public int w => w_img + w_txt;
            public int h => h_img + h_txt;
            public button(string text, vec vec, byte graphicId, Action Do, Func<bool> enableCondition)
            {
                this.text = text;
                this.vec = new vec(vec);
                this.graphicId = graphicId;
                this.Do = Do;
                EnableCondition = enableCondition;
            }
            public bool Check(vecf cursor) => Check(cursor.i);
            public bool Check(vec cursor)
            {
                return new Rectangle(vec.x, vec.y, w, h).Contains(cursor.x, cursor.y);
            }
            public void CheckAndDo(vecf cursor) => CheckAndDo(cursor.i);
            public void CheckAndDo(vec cursor)
            {
                if (!EnableCondition()) return;
                if (KB.IsKeyPressed(KB.Key.Space) && Check(cursor))
                    Do();
            }
            public void Display()
            {
                new DispClass ( GraphicImages.Images[graphicId], vec ).Display(2);
                Text.DisplayText(text, vec.x + w_img + 2, vec.y + h / 2 - 2, 2);
                if (!EnableCondition())
                {
                    Graphic.DisplayHorizontalLine(
                        vec.x + w_img + 2,
                        vec.x + w_img + 2 + w_txt,
                        vec.y + h / 2 - 2 + 1,
                        3, 1, 2);
                    Graphic.DisplayHorizontalLine(
                        vec.x + w_img + 2,
                        vec.x + w_img + 2 + w_txt,
                        vec.y + h / 2 - 2 + 3,
                        3, 1, 2);
                }
            }
        }

        button Add, Seed;
        vecf cursor;
        LabsAddIngredient LabsAddIngredient;
        List<string> Ingredients;
        Fiole Fiole;
        public static bool ShowAddIngredient = false;
        public static string IngredientToAdd = null;

        public void Init()
        {
            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
            Core.Layers.Add(new byte[64, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI

            cursor = new vecf(32, 32);

            Add = new button("add", new vec(2, 2), 1, () => ShowAddIngredient = true, () => Ingredients.Count < Fiole.MaxIngredients);
            Seed = new button("seed", new vec(2, 64 - 2 - GraphicImages.W(2)), 2, () => CreateSeed(), () => Ingredients.Count > 0);

            LabsAddIngredient = new LabsAddIngredient();
            LabsAddIngredient.Init();
            Ingredients = new List<string>();

            Fiole = new Fiole(new vec(32 - Fiole.w / 2, 32 - Fiole.h / 2));
        }

        public void Update()
        {
            if (ShowAddIngredient)
            {
                LabsAddIngredient.Update();
            }
            else
            {
                if (KB.IsKeyPressed(KB.Key.Escape))
                {
                    Plants.Instance.CurrentState = null;
                    return;
                }

                if (!string.IsNullOrWhiteSpace(IngredientToAdd))
                {
                    Ingredients.Add(IngredientToAdd);
                    IngredientToAdd = null;
                }

                UserInputs();
                Add.CheckAndDo(cursor);
                Seed.CheckAndDo(cursor);

                DisplayUI();
            }
        }

        private void UserInputs()
        {
            float speed = 2F;

            if (KB.IsKeyDown(KB.Key.Left))
                cursor.x -= speed;
            if (KB.IsKeyDown(KB.Key.Right))
                cursor.x += speed;
            if (KB.IsKeyDown(KB.Key.Up))
                cursor.y -= speed;
            if (KB.IsKeyDown(KB.Key.Down))
                cursor.y += speed;

            if (cursor.x < 0) cursor.x = 0;
            if (cursor.x > 63) cursor.x = 63;
            if (cursor.y < 0) cursor.y = 0;
            if (cursor.y > 63) cursor.y = 63;
        }
        private void UserDisplay()
        {
            bool chk(int v) => v >= 0 && v < 64;

            int x = cursor.i.x;
            int y = cursor.i.y;
            for (int i = -2; i < 3; i++)
            {
                if (chk(x + i) && chk(y))
                    Core.Layers[2][x + i, y] = 3;
                if (chk(x) && chk(y + i))
                    Core.Layers[2][x, y + i] = 3;
            }
        }

        private void DisplayUI()
        {
            Add.Display();
            Seed.Display();
            Fiole.Display(Ingredients);
            UserDisplay();
        }

        public class GraphicImages
        {
            public static List<byte[,]> Images = new List<byte[,]>();
            static GraphicImages()
            {
                byte[,] img;

                // 0
                img = new byte[8, 8];
                Images.Add(img);

                // 1
                img = new byte[8, 8];
                for (int x = 0; x < 8; x+=2)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        img[x, y] = 2;
                    }
                }
                Images.Add(img);

                // 2
                img = new byte[8, 8];
                for (int y = 0; y < 8; y+=2)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        img[x, y] = 2;
                    }
                }
                Images.Add(img);
            }
            public static int W(int id) => Images[SafeId(id)].GetLength(0);
            public static int H(int id) => Images[SafeId(id)].GetLength(1);
            private static int SafeId(int NotSafeId) => NotSafeId < 0 || NotSafeId >= Images.Count() ? 0 : NotSafeId;
        }

        void CreateSeed()
        {
            Data.Instance.Seeds.Add($"OGM_{OGM.EncodeGenetic(Ingredients)}", 1);
            Ingredients.Clear();
        }
    }
}
