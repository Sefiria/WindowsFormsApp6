﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btTile_Click(object sender, EventArgs e)
        {
            new WindowsFormsApp7.Form1().ShowDialog(this);
        }

        private void btMap_Click(object sender, EventArgs e)
        {
            new WindowsFormsApp8.Form1().ShowDialog(this);
        }
    }
}
