using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Transparency
{
    static class Program
    {
        // ########## TRANSPARENCY ##########
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;
            foreach (var arg in args)
            {
                if (File.Exists(arg))
                {
                    var img = Image.FromFile(arg) as Bitmap;
                    if (img == null) continue;
                    img.MakeTransparent();
                    string filename = $"{Path.GetDirectoryName(arg)}{Path.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(arg)}.png";
                    img.Save(filename);
                }
            }
        }


        // ########## SPLIT 32x32 ##########
        //        [STAThread]
        //        static void Main(string[] args)
        //        {
        //            bool fast_execution = args.Length > 0 && (args[0].CompareTo("--fast") == 0 || args[0].CompareTo("-f") == 0);
        //            int sz = 48;
        //            if (args.Length == 0)
        //                return;
        //            foreach (var arg in args)
        //            {
        //                if (File.Exists(arg))
        //                {
        //                    var img = Image.FromFile(arg) as Bitmap;
        //                    if (img == null) continue;
        //                    var name = Path.GetFileNameWithoutExtension(arg);
        //                    if (!Directory.Exists(name))
        //                        Directory.CreateDirectory(name);
        //                    img.MakeTransparent();
        //                    int qw = img.Width / sz;
        //                    int qh = img.Height / sz;
        //                    for(int x=0; x < qw; x++)
        //                    {
        //                        for (int y = 0; y < qh; y++)
        //                        {
        //                            var b = new Bitmap(sz, sz);
        //                            using (Graphics g = Graphics.FromImage(b))
        //                                g.DrawImage(img, new RectangleF(0, 0, sz, sz), new RectangleF(x * sz, y * sz, sz, sz), GraphicsUnit.Pixel);
        //                            if (fast_execution || !IsImageEmpty(b))
        //                            {
        //                                b.MakeTransparent(Color.White);
        //                                b.Save($"{name}\\{name} {x} {y}.png", ImageFormat.Png);
        //                            }
        //                        }
        //                    }
        //                    Console.WriteLine($"done : {arg}");
        //                }
        //            }
        ////            Console.WriteLine("Press Enter to leave program...");
        ////            Console.ReadLine();
        //        }
        //        static bool IsImageEmpty(Bitmap img)
        //        {
        //            for (int x = 0; x < img.Width; x++)
        //                for (int y = 0; y < img.Height; y++)
        //                    if (!new List<int>() { Color.FromArgb(0,0,0,0).ToArgb(), Color.Transparent.ToArgb(), Color.White.ToArgb(), Color.Black.ToArgb() }.Contains(img.GetPixel(x, y).ToArgb()))
        //                        return false;
        //            return true;
        //        }


        // ########## Convert 32x32 to 48x48 ##########
        //static List<int> listEmptyColors = new List<int>() { Color.FromArgb(0, 0, 0, 0).ToArgb(), Color.Transparent.ToArgb(), Color.White.ToArgb(), Color.Black.ToArgb() };
        //[STAThread]
        //static void Main(string[] args)
        //{
        //    //args = new string[] { "E:\\Users\\sauve\\RPG MAker MZ projects\\Moq\\img\\characters\\crops2.png" };
        //    int max_w = 12;
        //    int max_h = 8;
        //    try
        //    {
        //        if (args.Length == 0)
        //            return;
        //        foreach (var arg in args)
        //        {
        //            if (File.Exists(arg))
        //            {
        //                var img = Image.FromFile(arg) as Bitmap;
        //                if (img == null) continue;
        //                img.MakeTransparent();
        //                int qw = img.Width / 32;
        //                int qh = img.Height / 32;
        //                bool twofloors_left = false, twofloors_right = false;
        //                bool twofloors_top = false, twofloors_bottom = false;
        //                var line = Enumerable.Range(0, 32).Cast<int?>().ToList();
        //                bool check_bounds(int __x, int __y) => __x >= 0 && __x < img.Width && __y >= 0 && __y < img.Height;
        //                for (int j=0; j < img.Height / 32 / max_h; j++)
        //                {
        //                    for (int i=0; i < img.Width / 32 / max_w; i++)
        //                    {
        //                        Bitmap result = new Bitmap(max_w * 48, max_h * 48);
        //                        Graphics g = Graphics.FromImage(result);
        //                        for (int x = 0; x < max_w; x++)
        //                        {
        //                            for (int y = 0; y < max_h; y++)
        //                            {
        //                                twofloors_left = line.FirstOrDefault(n => { var _x = i * max_w * 48 + (x + 1) * 32; var _y = j * max_h * 48 + y * 32 + n.Value; return check_bounds(_x,_y) && !listEmptyColors.Contains(img.GetPixel(_x, _y).ToArgb()); }) != null;
        //                                twofloors_right = line.FirstOrDefault(n => { var _x = i * max_w * 48 + (x - 1) * 32 + 31; var _y = j * max_h * 48 + y * 32 + n.Value; return check_bounds(_x, _y) && !listEmptyColors.Contains(img.GetPixel(_x, _y).ToArgb()); }) != null;
        //                                twofloors_top = line.FirstOrDefault(n => { var _x = i * max_w * 48 + x * 32 + n.Value; var _y = j * max_h * 48 + (y + 1) * 32; return check_bounds(_x, _y) && !listEmptyColors.Contains(img.GetPixel(_x, _y).ToArgb()); }) != null;
        //                                twofloors_bottom = line.FirstOrDefault(n => { var _x = i * max_w * 48 + x * 32 + n.Value; var _y = j * max_h * 48 + (y - 1) * 32 + 31; return check_bounds(_x, _y) && !listEmptyColors.Contains(img.GetPixel(_x, _y).ToArgb()); }) != null;
        //                                int u = twofloors_left ? 16 : (twofloors_right ? 0 : 8);
        //                                int v = twofloors_top ? 16 : (twofloors_bottom ? 0 : 8);
        //                                g.DrawImage(img, new RectangleF(u + x * 48, v + y * 48, 32, 32), new RectangleF(i * max_w * 48 + x * 32, j * max_h * 48 + y * 32, 32, 32), GraphicsUnit.Pixel);
        //                            }
        //                        }
        //                        g.Dispose();
        //                        string filename = $"{Path.GetDirectoryName(arg)}{Path.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(arg)}_px48x48_id{i}x{j}.png";
        //                        Console.WriteLine($"Saving bitmap to {filename}");
        //                        if (!IsImageEmpty(result))
        //                        {
        //                            result.MakeTransparent();
        //                            result.Save(filename, ImageFormat.Png);
        //                        }
        //                        qw -= max_w;
        //                        qh -= max_h;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        Console.Read();
        //    }
        //}
        //static bool IsImageEmpty(Bitmap img)
        //{
        //    for (int x = 0; x < img.Width; x++)
        //        for (int y = 0; y < img.Height; y++)
        //            if (!listEmptyColors.Contains(img.GetPixel(x, y).ToArgb()))
        //                return false;
        //    return true;
        //}

        // ########## Resize 32x32 to 48x48 ##########
        //static List<int> listEmptyColors = new List<int>() { Color.FromArgb(0, 0, 0, 0).ToArgb(), Color.Transparent.ToArgb(), Color.White.ToArgb(), Color.Black.ToArgb() };
        //[STAThread]
        //static void Main(string[] args)
        //{
        //    //args = new string[] { "E:\\Users\\sauve\\RPG MAker MZ projects\\Moq\\img\\characters\\crops2.png" };
        //    int max_w = 12;
        //    int max_h = 8;
        //    try
        //    {
        //        if (args.Length == 0)
        //            return;
        //        foreach (var arg in args)
        //        {
        //            if (File.Exists(arg))
        //            {
        //                var img = Image.FromFile(arg) as Bitmap;
        //                if (img == null) continue;
        //                img.MakeTransparent();
        //                int qw = img.Width / 32;
        //                int qh = img.Height / 32;
        //                for (int j = 0; j < img.Height / 32 / max_h; j++)
        //                {
        //                    for (int i = 0; i < img.Width / 32 / max_w; i++)
        //                    {
        //                        Bitmap result = new Bitmap(max_w * 48, max_h * 48);
        //                        Graphics g = Graphics.FromImage(result);
        //                        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        //                        for (int x = 0; x < max_w; x++)
        //                        {
        //                            for (int y = 0; y < max_h; y++)
        //                            {
        //                                g.DrawImage(img, new RectangleF(x * 48, y * 48, 48, 48), new RectangleF(i * max_w * 48 + x * 32, j * max_h * 48 + y * 32, 32, 32), GraphicsUnit.Pixel);
        //                            }
        //                        }
        //                        g.Dispose();
        //                        string filename = $"{Path.GetDirectoryName(arg)}{Path.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(arg)}_res48x48_id{i}x{j}.png";
        //                        Console.WriteLine($"Saving bitmap to {filename}");
        //                        if (!IsImageEmpty(result))
        //                        {
        //                            result.MakeTransparent();
        //                            result.Save(filename, ImageFormat.Png);
        //                        }
        //                        qw -= max_w;
        //                        qh -= max_h;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        Console.Read();
        //    }
        //}
        //static bool IsImageEmpty(Bitmap img)
        //{
        //    for (int x = 0; x < img.Width; x++)
        //        for (int y = 0; y < img.Height; y++)
        //            if (!listEmptyColors.Contains(img.GetPixel(x, y).ToArgb()))
        //                return false;
        //    return true;
        //}
    }
}