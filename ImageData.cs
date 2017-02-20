using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public class ImageData : IParsedData {
    const string ELEMENT_CHARACTOR = "charactor";
    const string ELEMENT_FILE = "file";
    const string ELEMENT_WIDTH_SPLIT = "width_split";
    const string ELEMENT_HEIGHT_SPLIT = "height_split";

    const string ATTRIBUTE_NAME = "name";

    enum Element{
        NONE = 0,
        CHARACTOR,
        FILE,
        WIDTH_SPLIT,
        HEIGHT_SPLIT
    };

    private Dictionary<int, ImageProperty> m_properties = new Dictionary<int, ImageProperty>();
    private int m_counter = 0;
    private Element m_elm = Element.NONE;


    class ImageProperty
    {
        public string m_name;
        public string m_file;
        public int m_width;
        public int m_height;
        public int m_width_split;
        public int m_height_split;

        // コンストラクタ
        public ImageProperty()
        {
            this.m_name = "";
            this.m_file = "";
            this.m_width = 0;
            this.m_height = 0;
            this.m_width_split = 0;
            this.m_height_split = 0;
        }
    }

    public void AddAttribute(string p_name, string p_value)
    {
        string nodeName = "";
        ImageProperty imgProp = new ImageProperty();

        Debug.Log("AddAttribute " + p_name + " " + p_value);
        if (0 == ATTRIBUTE_NAME.CompareTo(p_name.ToLower()))
        {
            if (0 < m_counter)
            {
                bool flag = m_properties.TryGetValue(m_counter - 1, out imgProp);
                nodeName = flag ? imgProp.m_name : "";
            }

            if (0 != nodeName.CompareTo(p_value))
            {
                imgProp = new ImageProperty();
                imgProp.m_name = p_value;
                m_properties.Add(m_counter, imgProp);
                m_counter++;
            }
        }

        return;
    }

    public void AddNode(string p_name, int p_depth)
    {
        if (0 == ELEMENT_CHARACTOR.CompareTo(p_name.ToLower()))
        {
            m_elm = Element.CHARACTOR;
        }
        else if (0 == ELEMENT_FILE.CompareTo(p_name.ToLower()))
        {
            m_elm = Element.FILE;
        }
        else if (0 == ELEMENT_WIDTH_SPLIT.CompareTo(p_name.ToLower()))
        {
            m_elm = Element.WIDTH_SPLIT;
        }
        else if (0 == ELEMENT_HEIGHT_SPLIT.CompareTo(p_name.ToLower()))
        {
            m_elm = Element.HEIGHT_SPLIT;
        }
        else
        {
            m_elm = Element.NONE;
        }

        return;
    }

    public void SetValue(string p_value)
    {
        ImageProperty imgProp = new ImageProperty();

        if (0 < m_counter)
        {
            bool flag = m_properties.TryGetValue(m_counter - 1, out imgProp);
            if (flag)
            {
                switch (m_elm)
                {
                    case Element.FILE:
                        {
                                imgProp.m_file = p_value;
                        }
                        break;
                    case Element.WIDTH_SPLIT:
                        {
                            imgProp.m_width_split = Int32.Parse(p_value);
                        }
                        break;
                    case Element.HEIGHT_SPLIT:
                        {
                            imgProp.m_height_split = Int32.Parse(p_value);
                        }
                        break;
                    default:
                        break;
                }
                string prop = "";
                prop = imgProp.m_name + " " + imgProp.m_file + " " + imgProp.m_width_split + " " + imgProp.m_height_split;
                Debug.Log(prop);
            }
        }
    }

    public Sprite GetSprite()
    {
        Sprite result = null;
        ImageProperty imgProp = new ImageProperty();
        if (0 < m_counter)
        {
            bool flag = m_properties.TryGetValue(m_counter - 1, out imgProp);
            if (flag)
            {
                result = Resources.Load<Sprite>(imgProp.m_file);
            }
        }
                
        return result;
    }

    private static void DividSprite(string texturePath, int horizontalCount, int verticalCount)
    {
        TextureImporter importer = TextureImporter.GetAtPath(texturePath) as TextureImporter;

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.filterMode = FilterMode.Point;
        EditorUtility.SetDirty(importer);
        AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);

        Texture texture = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture)) as Texture;
        importer.spritePixelsPerUnit = Mathf.Max(texture.width / horizontalCount, texture.height / verticalCount);
        importer.spritesheet = CreateSpriteMetaDataArray(texture, horizontalCount, verticalCount);

        EditorUtility.SetDirty(importer);
        AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
    }

    private static SpriteMetaData[] CreateSpriteMetaDataArray(Texture texture, int horizontalCount, int verticalCount)
    {
        float spriteWidth = texture.width / horizontalCount;
        float spriteHeight = texture.height / verticalCount;

        return Enumerable
            .Range(0, horizontalCount * verticalCount)
            .Select(index => {
                int x = index % horizontalCount;
                int y = index / horizontalCount;

                return new SpriteMetaData
                {
                    name = string.Format("{0}_{1}", texture.name, index),
                    rect = new Rect(spriteWidth * x, texture.height - spriteHeight * (y + 1), spriteWidth, spriteHeight)
                };
            })
            .ToArray();
    }
}
