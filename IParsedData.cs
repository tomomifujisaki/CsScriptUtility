using UnityEngine;
using System.Collections;

public interface IParsedData {
    // ノードの追加
    void AddNode(string p_name, int p_depth);
    void AddAttribute(string p_name, string p_value);
    void SetValue(string p_value);
}
