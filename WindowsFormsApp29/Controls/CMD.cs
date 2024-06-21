using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Tooling;

namespace WindowsFormsApp29.Controls
{
    internal class CMD : UIElement
    {
        Rectangle Bounds;
        Font Font = new Font("Consolas", 16F);
        float  w = 0F, h;
        int cw => (int)(Bounds.Width / w);

        public CMD()
        {
            var sz = TextRenderer.MeasureText("_", Font);
            w = sz.Width / 2;
            h = sz.Height / 2 + 2;
            KB.OnKeyPressed += OnKeyPressed;
        }

        #region KeysManagement
        Regex KeysManagement_Regex = new Regex(@"^[a-zA-Z0-9\-\+\*_\/\?\!]$");
        string KeysManagement_Command = "";
        private void OnKeyPressed(System.Windows.Input.Key key)
        {
            string keyString = key.ToString();
            if ((keyString.StartsWith("D") || keyString.StartsWith("NumPad")) && keyString.Length > 1)
            {
                keyString = keyString.Substring(keyString.Length - 1);
            }
            if (key == System.Windows.Input.Key.Back)
            {
                KeysManagement_Command = KeysManagement_Command.Substring(0, KeysManagement_Command.Length - 1);
            }
            else if (key == System.Windows.Input.Key.Enter)
            {
                // To : Do command
                HistoryCommands_Add(KeysManagement_Command);
                KeysManagement_Command = "";
            }
            else if (KeysManagement_Regex.IsMatch(keyString))
            {
                KeysManagement_Command += keyString;
            }
        }
        #endregion

        public override void Update()
        {
        }

        public override void Draw(Graphics g)
        {
            Bounds = new Rectangle(Point.Empty, MainForm.Ref.Size);
            g.FillRectangle(new SolidBrush(Color.FromArgb(30, 30, 30)), Bounds);

            Write(g, ">" + KeysManagement_Command);
        }
        void Write(Graphics g, string text, int x = 0, int y = 0)
        {
            for(int i= 0; i < text.Length; i++)
            {
                g.DrawString(text[i].ToString(), Font, Brushes.LightGray, x + (i % cw) * w, y + (i / cw) * h);
            }
        }

        #region history
        List<string> HistoryCommands_Commands = new List<string>();
        int HistoryCommands_SelectionIndex = -1;
        void HistoryCommands_Add(string command)
        {
            HistoryCommands_Commands.Insert(0, command);
            while(HistoryCommands_Commands.Count > 10) HistoryCommands_Commands.RemoveAt(HistoryCommands_Commands.Count-1);
        }
        /// <summary>
        /// Get history command from index of var CommandsHistorySelectionIndex
        /// </summary>
        /// <returns>Returns history command or null if index out of bounds</returns>
        string HistoryCommands_Get()
        {
            return HistoryCommands_SelectionIndex == -1 || HistoryCommands_SelectionIndex > 9 ? null : HistoryCommands_Commands[HistoryCommands_SelectionIndex];
        }
        void HistoryCommands_ResetIndex() => HistoryCommands_SelectionIndex = -1;
        #endregion
    }
}
