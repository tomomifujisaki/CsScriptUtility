using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {


    void OnGUI()
    {

        // フォントサイズ
        Util.SetFontSize(32);
        // 中央揃え
        Util.SetFontAlignment(TextAnchor.MiddleCenter);

        // フォントの位置
        float w = 128; // 幅
        float h = 32; // 高さ
        float px = Screen.width / 2 - w / 2;
        float py = Screen.height / 2 - h / 2;

        // フォント描画
        Util.GUILabel(px, py, w, h, "MINI GAME");
        Util.GUILabel(px, py - 32, w, h, "TEST GAME");

        // ボタンは少し下にずらす
        py += 60;
        if (GUI.Button(new Rect(px, py, w, h), "START"))
        {
            // XML読み込み
            LoadSaxXML xml = new LoadSaxXML();
            ImageData imgDat = new ImageData();
            //xml.isFileInput = true;
            xml.dirname = "Resources/xml";
            xml.fname = "sources.xml";
            xml.ReadXML(imgDat);
            GetComponent<SpriteRenderer>().sprite = imgDat.GetSprite();
        }
    }
}
