using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace oneHandleInput
{
    internal class FileNameValidator
    {
        public static bool IsValid(string fileName)
        {
            if (Path.GetInvalidFileNameChars().Any(fileName.Contains)) return false;

            try
            {
                _ = Path.GetFullPath(fileName);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
