using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

using sfColor = SFML.Graphics.Color;
using sfImage = SFML.Graphics.Image;

namespace Framework
{
    [Serializable]
    public class EntityProperties
    {
        public string Name;
        public string EntityPath;
        public bool autoTiled;
        public bool indestructible, trigger;
        public byte ID, layer;
        public int HP;
        public (byte R, byte G, byte B) TransparentColor;
        public BehaviorScript behaviorScript = null;
        public byte behaviorIDToLoad = 0;
        public bool gravityEffect = false;


        private string m_image;
        private List<string> m_AutoTilesRaws = new List<string>();

        [NonSerialized]
        public sfImage image;
        [NonSerialized]
        public List<sfImage> AutoTilesRaws = new List<sfImage>();

        public EntityProperties()
        {
            Name = EntityPath = "";
            autoTiled = false;
            image = new sfImage((uint)Tools.TileSize, (uint)Tools.TileSize, sfColor.White);
            for (int i = 0; i < 12; i++)
            {
                AutoTilesRaws.Add(new sfImage((uint)Tools.TileSize, (uint)Tools.TileSize, sfColor.White));
                for (int j = 0; j < 4; j++)
                    m_AutoTilesRaws.Add("");
            }
            indestructible = false;
            HP = 1;
            ID = layer = 0;
            behaviorScript = new BehaviorScript();
        }
        public void Save()
        {
            string imgUrl = Directory.GetCurrentDirectory() + "/" + EntityPath + "/" + Name + ".png";
            string propertiesUrl = imgUrl.Replace(imgUrl.Split('.').Last(), "properties");

            m_image = Tools.GetSfImageEncoded(image);
            for (int i = 0; i < 12 * 4; i+=4)
            {
                var splitted = Tools.Split4SfImage(AutoTilesRaws[i/4]);
                for (int j = 0; j < 4; j++)
                    m_AutoTilesRaws[i + j] = Tools.GetSfImageEncoded(splitted[j]);
            }

            BinaryFormatter bf = new BinaryFormatter();
            using (Stream stream = File.Open(propertiesUrl, FileMode.Create))
                bf.Serialize(stream, this);
        }
        static public EntityProperties Load(string filename)
        {
            var entity = new EntityProperties();
            BinaryFormatter bf = new BinaryFormatter();

            using (Stream stream = File.Open(filename, FileMode.Open))
                entity = bf.Deserialize(stream) as EntityProperties;

            entity.image = new sfImage(Tools.GetSfImageDecoded(entity.m_image));
            entity.AutoTilesRaws = new List<sfImage>();
            for (int i = 0; i < 12 * 4; i+=4)
            {
                var unsplitted = Tools.UnSplit4SfImage( Tools.GetSfImageDecoded(entity.m_AutoTilesRaws[i + 0]),
                                                        Tools.GetSfImageDecoded(entity.m_AutoTilesRaws[i + 1]),
                                                        Tools.GetSfImageDecoded(entity.m_AutoTilesRaws[i + 2]),
                                                        Tools.GetSfImageDecoded(entity.m_AutoTilesRaws[i + 3]));
                entity.AutoTilesRaws.Add(unsplitted);
            }

            return entity;
        }
        public List<Sprite> GetAutoTilesRawsSpriteList()
        {
            var result = new List<Sprite>();
            for (int i = 0; i < 48; i++)
            {
                var img = Tools.GetSfImageDecoded(m_AutoTilesRaws[i]);
                img.CreateMaskFromColor(new sfColor(TransparentColor.R, TransparentColor.G, TransparentColor.B));
                result.Add(new Sprite(new Texture(img)));
            }
            return result;
        }
    }
}
