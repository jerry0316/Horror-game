using UnityEngine;

public class ChairFallTrigger : MonoBehaviour
{
    public Animator chairAnimator; // �ѦҴȤl�� Animator

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �T�{Ĳ�o������O�_�O���a
        {
            Debug.Log("Player has entered the trigger area!"); // �ոհT��
            if (chairAnimator != null)
            {
                chairAnimator.Play("ChairFall"); // ��������Ȥl�ˤU���ʵe
                Debug.Log("Chair fall animation triggered!"); // �ոհT��
            }
            else
            {
                Debug.LogError("Chair animator is not assigned!"); // ���~�T��
            }
        }
    }
}
