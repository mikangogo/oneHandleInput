using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace oneHandleInput
{
    public class ConfigProfile
    {
        public static ConfigProfile Empty;

        public Guid guid;

        public int reverserPosFront;
        public int reverserPosBack;
        public int reverserAxis;
        public bool reverserAxisNegative;

        public int brakePosEmr;
        public int brakePosMax;
        public int brakePosNeutral;
        public int brakeNotches;
        public int brakeChatter;
        public int brakeAxis;
        public bool brakeAxisNegative;

        public int powerPosNeutral;
        public int powerPosMax;
        public int powerNotches;
        public int powerAxis;
        public bool powerAxisNegative;

        public int ssbPosMax;
        public int ssbPosNeutral;
        public int ssbNotches;
        public int ssbAxis;
        public bool ssbAxisNegative;

        public int switchS;
        public int switchA1;
        public int switchA2;
        public int switchB1;
        public int switchB2;
        public int switchC1;
        public int switchC2;
        public int switchD;
        public int switchE;
        public int switchF;
        public int switchG;
        public int switchH;
        public int switchI;
        public int switchJ;
        public int switchK;
        public int switchL;
        public int switchReverserFront;
        public int switchReverserNeutral;
        public int switchReverserBack;
        public int switchHorn1;
        public int switchHorn2;
        public int switchMusicHorn;
        public int switchConstSpeed;

        static ConfigProfile()
        {
            FormStringConverter converter = new FormStringConverter();
            Empty = new ConfigProfile()
            {
                switchS = converter.fromSwitchString("OFF"),
                switchA1 = converter.fromSwitchString("OFF"),
                switchA2 = converter.fromSwitchString("OFF"),
                switchB1 = converter.fromSwitchString("OFF"),
                switchB2 = converter.fromSwitchString("OFF"),
                switchC1 = converter.fromSwitchString("OFF"),
                switchC2 = converter.fromSwitchString("OFF"),
                switchD = converter.fromSwitchString("OFF"),
                switchE = converter.fromSwitchString("OFF"),
                switchF = converter.fromSwitchString("OFF"),
                switchG = converter.fromSwitchString("OFF"),
                switchH = converter.fromSwitchString("OFF"),
                switchI = converter.fromSwitchString("OFF"),
                switchJ = converter.fromSwitchString("OFF"),
                switchK = converter.fromSwitchString("OFF"),
                switchL = converter.fromSwitchString("OFF"),
                switchReverserFront = converter.fromSwitchString("OFF"),
                switchReverserNeutral = converter.fromSwitchString("OFF"),
                switchReverserBack = converter.fromSwitchString("OFF"),
                switchHorn1 = converter.fromSwitchString("OFF"),
                switchHorn2 = converter.fromSwitchString("OFF"),
                switchMusicHorn = converter.fromSwitchString("OFF"),
                switchConstSpeed = converter.fromSwitchString("OFF"),
            };
        }

        public static ConfigProfile fromFile(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigProfile));
            FileStream fs = new FileStream(path, FileMode.Open);
            ConfigProfile profile = (ConfigProfile)serializer.Deserialize(fs);
            fs.Close();

            return profile;
        }

        public void save(string path)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(ConfigProfile));
            FileStream fs = new FileStream(path, FileMode.Create);
            serializer.Serialize(fs, this);
            fs.Close();
        }
    }
}
