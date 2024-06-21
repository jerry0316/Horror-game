using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private static Dictionary<string, int> keysCollected = new Dictionary<string, int>(); // 存儲每種鑰匙的數量

    // 獲取 GameManager 的單例實例
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    // 添加鑰匙
    public void AddKey(string keyName)
    {
        if (keysCollected.ContainsKey(keyName))
        {
            keysCollected[keyName]++;
        }
        else
        {
            keysCollected[keyName] = 1;
        }
    }

    // 檢查是否有指定名稱的鑰匙
    public bool HasKey(string keyName)
    {
        if (keysCollected.ContainsKey(keyName))
        {
            return keysCollected[keyName] > 0;
        }
        return false;
    }
}
