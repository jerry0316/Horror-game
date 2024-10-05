using UnityEngine;
using System.Collections;

public class MonsterAppear : MonoBehaviour
{
    public GameObject monster; // 怪物的遊戲物件
    public Transform spawnPoint; // 怪物出現的位子
    public float delayTime = 2f; // 出現前的延遲時間
    public float duration = 3f; // 怪物顯示的持續時間
    public AudioSource monsterSound; // 怪物出現時播放的音效

    private bool hasAppeared = false; // 紀錄怪物是否已經出現過

    private void Start()
    {
        // 開始時隱藏怪物
        if (monster != null)
        {
            monster.SetActive(false); // 隱藏怪物
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasAppeared) // 檢查怪物是否已經出現過
        {
            StartCoroutine(SpawnMonster());
        }
    }

    private IEnumerator SpawnMonster()
    {
        hasAppeared = true; // 設定怪物已經出現過
        yield return new WaitForSeconds(delayTime);
        monster.SetActive(true); // 啟用怪物
        if (monsterSound != null)
        {
            monsterSound.Play(); // 播放怪物音效
        }
        Debug.Log("Monster has appeared!");

        // 等待怪物顯示的持續時間
        yield return new WaitForSeconds(duration);

        monster.SetActive(false); // 隱藏怪物
        Debug.Log("Monster has disappeared!");
    }
}
