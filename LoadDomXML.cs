using UnityEngine;
using System.Collections;
using System.Xml;

public class LoadDomXML {


    //Public Variables
    public bool isFileInput = false;
    public string dirname = "data";    //Output Directory
    public string fname = "felicadata";      //File-Header ** '.xml' not necessary !

    //Private Variables
    private XmlDocument xmlDoc;

    //Public Functions

    //--------------
    // Initialization
    public void Start()
    {
        if (isFileInput)
        {
            xmlDoc = ReadXML();
            ParseXML(xmlDoc);
        }
    }


    private XmlDocument ReadXML()   //XMLファイルを読み込む
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
        XmlDocument xmlDoc = new XmlDocument(); //XmlDocumentクラス
        xmlDoc.Load(fullpath); // load the file.    //XMLデータをロード

        return xmlDoc;  //読み込んだXmlDocumentを返す
    }

    private void ParseXML(XmlDocument xmlDoc)
    {  //XMLをパースする
       //Parse XML List
        XmlNode all = xmlDoc.FirstChild;    //最初のノード　'FelicaData'タグ
        Debug.Log("FirstChild " + all.InnerText);   //子ノードをふくむ、すべてのタグのテキストが表示される
                                       

        XmlNodeList header = all.NextSibling.ChildNodes; //最初のノード＝'Header'タグの、子ノードのリスト
        foreach (XmlNode node in header)
        {
            Debug.Log(node.Name + ", " + node.InnerText);   //タグ名と、テキストを表示
        }
        //
        XmlNodeList models = xmlDoc.GetElementsByTagName("Model");  //'Model'タグのリストを作る
        foreach (XmlNode model in models)
        {
            Debug.Log(model.Attributes["id"].Value + ", " + model.InnerText);   //属性'id'と、テキストを表示
        }
        //
        XmlNodeList actions = xmlDoc.GetElementsByTagName("Action");    //'Action'タグのリストを作る
        foreach (XmlNode action in actions)
        {
            Debug.Log(action.Attributes["id"].Value + ", " + action.InnerText); //属性'id'と、テキストを表示
        }
    }
}
