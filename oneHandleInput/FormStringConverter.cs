using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oneHandleInput
{
    internal class FormStringConverter
    {
        public string toSwitchString(int n)
        {
            switch (n)
            {
                case -1: return "OFF";
                default: return toString(n);
            }
        }

        public int fromSwitchString(string s)
        {
            switch (s)
            {
                case "OFF": return -1;
                default: return fromString(s);
            }
        }

        public string toString(int n)
        {
            return n.ToString();
        }

        public int fromString(string s)
        {
            int n = 0;

            try
            {
                n = int.Parse(s);
            }
            catch
            {
                n = 0;
            }

            return n;
        }
    }
}
