using Android.Content.Res;
using Android.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Alansa.Droid.Utils
{
    public class ColorManager
    {
        private static bool IsColorsAlreadySet;
        private static int count;

        private static readonly List<Color> Colors = new List<Color>();

        public struct Indicators
        {
            public static Color Red => Color.ParseColor("#d32f2f");
            public static Color Yellow => Color.ParseColor("#FFA000");
            public static Color Green => Color.ParseColor("#388E3C");
            public static Color Gray => Color.ParseColor("#8C000000");
        }

        public static void Initialize(AssetManager manager)
        {
            if (!IsColorsAlreadySet)
            {
                var text = new StreamReader(manager.Open("colors.txt")).ReadToEnd();
                var colors = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var color in colors)
                    Colors.Add(Color.ParseColor($"#{color}"));
                IsColorsAlreadySet = true;
            }
        }

        public static Color GetColor()
        {
            count++;
            if (count >= Colors.Count - 1)
                count = 0;

            if (count > 16)
                count = 0;

            return Colors[count];
        }
    }
}