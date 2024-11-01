using TMPro;
using UnityEngine;
using UnityEngine.AI; // �T�O�ޤJ NavMesh �R�W�Ŷ�

public class MedicinePickup : MonoBehaviour
{
    public int sanityRestoreAmount = 20; // ��_���z���ȶq
    private SanityManager sanityManager; // �ѦҨ�z���޲z��
    private bool isCollected = false; // �Ω�P�_�Ī��O�_�w�Q�߰_

    public TextMeshProUGUI MedicinePickupText; // �Ω���ܴ��ܪ��奻

    private void Start()
    {
        sanityManager = FindObjectOfType<SanityManager>();
        if (MedicinePickupText != null)
        {
            MedicinePickupText.gameObject.SetActive(false); // ��l�Ʈ����ô��ܤ奻
        }
        else
        {
            Debug.LogError("MedicinePickupText �����T�]�m�I�нT�O���w�b Inspector ���s���C");
        }
    }

    private void Update()
    {
        // �ˬd���a���U 'E' ��åB�Ī����Q�߰_
        if (!isCollected && Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f); // �ϥζ�νd���˴����a�O�_�b����
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    CollectMedicine(); // �߰_�Ī�
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MedicinePickupText.text = "�� E �߰_�Ī�"; // ��ܴ��ܤ奻
            MedicinePickupText.gameObject.SetActive(true); // ��ܴ���
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MedicinePickupText.gameObject.SetActive(false); // ���ô���
        }
    }

    private void CollectMedicine()
    {
        if (sanityManager != null)
        {
            sanityManager.IncreaseSanity(sanityRestoreAmount); // ��_�z����
        }

        // ���ô��ܤ奻
        if (MedicinePickupText != null)
        {
            MedicinePickupText.gameObject.SetActive(false); // ���ô��ܤ奻
        }

        isCollected = true; // �]�m���w�������A
        gameObject.SetActive(false); // �����Ī�����A�קK�h���߰_

        // �i��G�z�i�H�b�o��Ĳ�o�@�Ӧ^���έ��s�ͦ����ƥ�
        Invoke("Respawn", 3f); // 30��᭫�s�ͦ��Ī�
    }


    private void Respawn()
    {
        // �H����ܤ@�ӷs����m�ӥͦ��Ī�
        Vector3 newPosition = GetRandomNavMeshPosition();
        if (newPosition != Vector3.zero) // �T�O�ͦ���m����
        {
            transform.position = newPosition;
            gameObject.SetActive(true); // ����Ī�����
            isCollected = false; // ���m�������A
        }
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        Vector3 randomPosition = Random.insideUnitSphere * 10; // �H����m�d��
        randomPosition.y = 0; // �վ� y �b�H�ŦX�a��

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position; // ��^�b�ɯ����W���H����m
        }

        return Vector3.zero; // �Y�S���i�Φ�m�A��^�s�V�q
    }
}
