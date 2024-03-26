using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Csharp_SFML
{
    public partial class NumericUpDownCustom : UserControl
    {
        [Serializable]
        class NUDCustom : NumericUpDown
        {
            public override void UpButton()
            {
                base.UpButton();
                UpButtonClicked(this, null);
            }
            public override void DownButton()
            {
                base.DownButton();
                DownButtonClicked(this, null);
            }

            public event EventHandler UpButtonClicked;
            public event EventHandler DownButtonClicked;
        }

        public NumericUpDownCustom()
        {
            InitializeComponent();

            nudc = new NUDCustom();
            nudc.Bounds = new System.Drawing.Rectangle(0, 0, 80, 20);
            nudc.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            Controls.Add(nudc);
            nudc.UpButtonClicked += UpButtonClicked;
            nudc.DownButtonClicked += DownButtonClicked;
            nudc.ValueChanged += ValueChanged;
        }

        private NUDCustom nudc;

        [Category("Arrows")]
        public event EventHandler UpButtonClicked;
        [Category("Arrows")]
        public event EventHandler DownButtonClicked;
        [Category("Action")]
        public event EventHandler ValueChanged;

        [Category("NumericUpDown")]
        public decimal Minimum { get => nudc.Minimum; set => nudc.Minimum = value; }
        [Category("NumericUpDown")]
        public decimal Maximum { get => nudc.Maximum; set => nudc.Maximum = value; }
        [Category("NumericUpDown")]
        public decimal Value { get => nudc.Value; set => nudc.Value = value; }
    }
}
