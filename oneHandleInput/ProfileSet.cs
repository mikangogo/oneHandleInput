using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace oneHandleInput
{
    public class ProfileSet
    {
        private const string SaveFileName = "lastLoaded.txt";

        private readonly Dictionary<string, ConfigProfile> m_profiles;

        public bool isInitialSetting { get; }
        public ICollection<string> keys => m_profiles.Keys;
        public string currentKey { get; set; }
        public ConfigProfile currentProfile => m_profiles[currentKey];

        private ProfileSet(Dictionary<string, ConfigProfile> profiles, string defaultKey, bool isInitialSetting)
        {
            if (defaultKey == null)
            {
                defaultKey = profiles.Keys.First();
            }

            m_profiles = profiles;
            currentKey = defaultKey;
            this.isInitialSetting = isInitialSetting;
        }

        public static ProfileSet load(string configDirectory)
        {
            string[] fileNames = Directory.GetFiles(configDirectory, "*.xml");
            Dictionary<string, ConfigProfile> profiles = fileNames.ToDictionary(fileName => Path.GetFileNameWithoutExtension(fileName), ConfigProfile.fromFile);

            string defaultKey;
            bool isInitialSetting = false;
            if (profiles.Count == 0)
            {
                profiles.Add("(既定)", ConfigProfile.Empty);
                defaultKey = "(既定)";
                isInitialSetting = true;
            }
            else
            {
                defaultKey = Path.GetFileNameWithoutExtension(fileNames[0]);
            }

            try
            {
                StreamReader sr = new StreamReader(Path.Combine(configDirectory, SaveFileName));
                defaultKey = sr.ReadToEnd();
                sr.Close();
            }
            catch
            {
            }

            return new ProfileSet(profiles, defaultKey, isInitialSetting);
        }

        public void save(string configDirectory)
        {
            File.WriteAllText(Path.Combine(configDirectory, SaveFileName), currentKey);
        }
    }
}
