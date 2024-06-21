using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private static Dictionary<string, int> keysCollected = new Dictionary<string, int>(); // �s�x�C���_�ͪ��ƶq

    // ��� GameManager ����ҹ��
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

    // �K�[�_��
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

    // �ˬd�O�_�����w�W�٪��_��
    public bool HasKey(string keyName)
    {
        if (keysCollected.ContainsKey(keyName))
        {
            return keysCollected[keyName] > 0;
        }
        return false;
    }
}
