using UnityEngine;
using System.Collections;

public class MonsterAppear : MonoBehaviour
{
    public GameObject monster; // �Ǫ����C������
    public Transform spawnPoint; // �Ǫ��X�{����l
    public float delayTime = 2f; // �X�{�e������ɶ�
    public float duration = 3f; // �Ǫ���ܪ�����ɶ�
    public AudioSource monsterSound; // �Ǫ��X�{�ɼ��񪺭���

    private bool hasAppeared = false; // �����Ǫ��O�_�w�g�X�{�L

    private void Start()
    {
        // �}�l�����éǪ�
        if (monster != null)
        {
            monster.SetActive(false); // ���éǪ�
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasAppeared) // �ˬd�Ǫ��O�_�w�g�X�{�L
        {
            StartCoroutine(SpawnMonster());
        }
    }

    private IEnumerator SpawnMonster()
    {
        hasAppeared = true; // �]�w�Ǫ��w�g�X�{�L
        yield return new WaitForSeconds(delayTime);
        monster.SetActive(true); // �ҥΩǪ�
        if (monsterSound != null)
        {
            monsterSound.Play(); // ����Ǫ�����
        }
        Debug.Log("Monster has appeared!");

        // ���ݩǪ���ܪ�����ɶ�
        yield return new WaitForSeconds(duration);

        monster.SetActive(false); // ���éǪ�
        Debug.Log("Monster has disappeared!");
    }
}
