using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.Utils
{
    public static class TextUtils
    {
        public static readonly string ThreeDots = "...";

        public static string Fit(string text, int Limit)
        {
            if(text.Length > Limit)
            {
                var fitText = text.Substring(0, Limit - ThreeDots.Length);
                fitText += ThreeDots;
                return fitText;
            }
            return text;
        }
    }
}
