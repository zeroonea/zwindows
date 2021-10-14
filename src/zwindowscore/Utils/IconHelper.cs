using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace zwindowscore.Utils
{
    public static class IconHelper
    {
        
        public const int GCL_HICONSM = -34;
        public const int GCL_HICON = -14;

        public const int ICON_SMALL = 0;
        public const int ICON_BIG = 1;
        public const int ICON_SMALL2 = 2;

        public const int WM_GETICON = 0x7F;

        public static System.Drawing.Icon GetAppIcon(IntPtr hwnd)
        {
            IntPtr iconHandle = Win32Helper.SendMessage(hwnd, WM_GETICON, ICON_SMALL2, 0);
            if(iconHandle == IntPtr.Zero)
            {
                iconHandle = Win32Helper.SendMessage(hwnd, WM_GETICON, ICON_SMALL, 0);
            }
            if(iconHandle == IntPtr.Zero)
            {
                iconHandle = Win32Helper.SendMessage(hwnd, WM_GETICON, ICON_BIG, 0);
            }
            if (iconHandle == IntPtr.Zero)
            { 
                iconHandle = Win32Helper.GetClassLongPtr(hwnd, GCL_HICON);
            }
            if (iconHandle == IntPtr.Zero)
            {
                iconHandle = Win32Helper.GetClassLongPtr(hwnd, GCL_HICONSM);
            }
            if (iconHandle == IntPtr.Zero)
            {
                var img = GetModernAppLogo(hwnd);
                if(img != null)
                {
                    return IconFromImage(img);
                }
            }
            if (iconHandle == IntPtr.Zero)
            { 
                iconHandle = Win32Helper.LoadIcon(IntPtr.Zero, (IntPtr)0x7F00/*IDI_APPLICATION*/);
            }
 
            if(iconHandle == IntPtr.Zero)
            return null;

            return System.Drawing.Icon.FromHandle(iconHandle);
        }

        public static System.Drawing.Image GetModernAppLogo(IntPtr hwnd) {
            // get folder where actual app resides
            var exePath = Win32Helper.GetModernAppProcessPath(hwnd)?.MainModule.FileName; 
            if(exePath == null) return null;
            var dir = System.IO.Path.GetDirectoryName(exePath);
            var manifestPath = System.IO.Path.Combine(dir, "AppxManifest.xml");            
            if (File.Exists(manifestPath)) {
                // this is manifest file
                string pathToLogo;
                using (var fs = File.OpenRead(manifestPath)) {
                    var manifest = XDocument.Load(fs);
                    const string ns = "http://schemas.microsoft.com/appx/manifest/foundation/windows10";
                    // rude parsing - take more care here
                    pathToLogo = manifest.Root.Element(XName.Get("Properties", ns)).Element(XName.Get("Logo", ns)).Value;
                }
                // now here it is tricky again - there are several files that match logo, for example
                // black, white, contrast white. Here we choose first, but you might do differently
                string finalLogo = null;
                // serach for all files that match file name in Logo element but with any suffix (like "Logo.black.png, Logo.white.png etc)
                foreach (var logoFile in Directory.GetFiles(System.IO.Path.Combine(dir, System.IO.Path.GetDirectoryName(pathToLogo)),
                    System.IO.Path.GetFileNameWithoutExtension(pathToLogo) + "*" + System.IO.Path.GetExtension(pathToLogo))) {
                    finalLogo = logoFile;
                    break;
                }

                //return finalLogo;

                if (System.IO.File.Exists(finalLogo))
                {
                    return System.Drawing.Bitmap.FromFile(finalLogo);
                    /*using (var fs = File.OpenRead(finalLogo))
                    {
                        var img = new Bitmap()
                        {
                        };
                        img.BeginInit();
                        img.StreamSource = fs;
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.EndInit();
                        return img;
                    }*/
                }
            }
            return null;
        }

        public static System.Drawing.Icon IconFromImage(System.Drawing.Image img) {
            var ms = new System.IO.MemoryStream();
            var bw = new System.IO.BinaryWriter(ms);
            // Header
            bw.Write((short)0);   // 0 : reserved
            bw.Write((short)1);   // 2 : 1=ico, 2=cur
            bw.Write((short)1);   // 4 : number of images
            // Image directory
            var w = img.Width;
            if (w >= 256) w = 0;
            bw.Write((byte)w);    // 0 : width of image
            var h = img.Height;
            if (h >= 256) h = 0;
            bw.Write((byte)h);    // 1 : height of image
            bw.Write((byte)0);    // 2 : number of colors in palette
            bw.Write((byte)0);    // 3 : reserved
            bw.Write((short)0);   // 4 : number of color planes
            bw.Write((short)0);   // 6 : bits per pixel
            var sizeHere = ms.Position;
            bw.Write((int)0);     // 8 : image size
            var start = (int)ms.Position + 4;
            bw.Write(start);      // 12: offset of image data
            // Image data
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var imageSize = (int)ms.Position - start;
            ms.Seek(sizeHere, System.IO.SeekOrigin.Begin);
            bw.Write(imageSize);
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            // And load it
            return new System.Drawing.Icon(ms);
        }
    }
}
