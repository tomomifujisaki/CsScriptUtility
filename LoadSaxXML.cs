using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System;

public class LoadSaxXML {

    public string dirname = "data";    //Output Directory
    public string fname = "felicadata";      //File-Header ** '.xml' not necessary !

    private XmlReader m_xmlReader = null;


    public void ReadXML(IParsedData parsedData)
    {
        m_xmlReader = LoadPaht();
        ParseXML(m_xmlReader, parsedData);
    }


    private XmlReader LoadPaht()   //XMLファイルを読み込む
    {
        //
        string dlm = "/";
        string path;
        if (dirname != "")
        {
            path = dlm + dirname + dlm;
        }
        else
        {
            path = dlm;
        }
        string fullpath;
        if (Application.isEditor)
        {
            fullpath = Application.dataPath + path + fname; //エディタの場合, Application.dataPathは'Asset'フォルダ
        }
        else
        {
            fullpath = Application.dataPath + dlm + ".." + path + fname;    //PC/Macの場合,Application.dataPathは、'実行ファイル_data'フォルダ

        }
        FileStream fs = null;
        XmlReader xmlRead = null;
        try
        {
            Debug.Log("Load File : " + fullpath);
            //fs = new FileStream(fullpath, FileMode.Open);
            XmlReaderSettings settings = null;
            
            settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            xmlRead = XmlReader.Create(fullpath, settings);  //XMLデータをロード
        }
        catch (Exception e)
        {
            Debug.Log("Load File Error : " + fullpath);
            Debug.Log("Exception: " + e.StackTrace);
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
            }
        }

        return xmlRead;  //読み込んだXmlDocumentを返す
    }

    public void ParseXML(XmlReader xmlReader, IParsedData parsedData)
    {
        string prop = "";
        try
        {
            while (xmlReader.Read() == true)
            {
                XmlNodeType nType = xmlReader.NodeType;
                prop = "";
                prop += "NodeType: " + nType.ToString() + "\r\n";

                prop += "LocalName: " + xmlReader.LocalName + "\r\n";
                prop += "Depth: " + Convert.ToString(xmlReader.Depth) + "\r\n";
                prop += "Name: " + xmlReader.Name + "\r\n";

                if (XmlNodeType.Element == nType)
                {
                    parsedData.AddNode(xmlReader.Name, xmlReader.Depth);
                }

                if (xmlReader.HasValue == true)
                {
                    Type valueType = xmlReader.ValueType;
                    prop += "ValueType: " + valueType.ToString() +"\r\n";
                    prop += "Value: " + xmlReader.Value + "\r\n";
                    parsedData.SetValue(xmlReader.Value);
                }

                //属性がある場合
                if (xmlReader.HasAttributes == true)
                {
                    for (int i = 0; i < xmlReader.AttributeCount; i++)
                    {
                        xmlReader.MoveToAttribute(i);
                        prop += "Attribute Name: " + xmlReader.Name + "\r\n";
                        if (xmlReader.HasValue == true)
                        {
                            Type valueType = xmlReader.ValueType;
                            prop += "ValueType: " + valueType.ToString() + "\r\n";
                            prop += "Attribute Value: " + xmlReader.Value + "\r\n";
                            parsedData.AddAttribute(xmlReader.Name, xmlReader.Value);
                        }
                    }
                    xmlReader.MoveToElement();
                }
                //Debug.Log(prop);
                
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.StackTrace);
        }
        finally
        {
            if (xmlReader != null)
            {
                xmlReader.Close();
            }
        }
    }
}
