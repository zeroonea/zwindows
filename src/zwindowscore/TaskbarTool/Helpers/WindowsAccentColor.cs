﻿using System;
using System.Windows.Media;

namespace TaskbarTool
{
    public static class WindowsAccentColor
    {
        private static Color accentColor = Color.FromArgb(255, 0, 0, 0);

        public static Int32 GetColorAsInt()
        {
            UpdateColor();

            return BitConverter.ToInt32(new byte[] { accentColor.R, accentColor.G, accentColor.B, accentColor.A }, 0);
        }

        public static Int32 GetColorAsInt(byte alpha)
        {
            UpdateColor();

            return BitConverter.ToInt32(new byte[] { accentColor.R, accentColor.G, accentColor.B, alpha }, 0);
        }

        private static void UpdateColor()
        {
            string keyName = "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Accent";
            int keyColor = (int)Microsoft.Win32.Registry.GetValue(keyName, "StartColorMenu", 00000000);

            byte[] bytes = BitConverter.GetBytes(keyColor);

            accentColor = Color.FromArgb(bytes[3], bytes[0], bytes[1], bytes[2]);
        }
    }
}
