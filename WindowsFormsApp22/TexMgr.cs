﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Dynamic;
using System.IO;
using Tooling;
using WindowsFormsApp22.Entities;

namespace WindowsFormsApp22
{
    internal class TexMgr
    {
        static GraphicsPath shape_101 = new GraphicsPath(new PointF[]
        {
            (0,0).P(), (1,0).P(), (1,1).P(), (0,1).P(), (0,0).P()
        }, new byte[] {
            0, 1, 1, 1, 1
        });

        public static GraphicsPath Load(string tex)
        {
            switch(tex)
            {
                default:
                case "bullet":
                case "square":
                case "101": return (GraphicsPath)shape_101.Clone();
                case "circle":
                    var path = new GraphicsPath();
                    path.AddEllipse(0, 0, 1, 1);
                    return path;
            }
        }
    }
}
