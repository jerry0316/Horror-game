using UnityEngine;
using UnityEngine.UI; // �ޤJUI�R�W�Ŷ�

public class JumpscareTrigger : MonoBehaviour
{
    public GameObject scareImage;   // ���̪����ƹϤ� (UI Image)
    public AudioSource scareSound;  // ���ƭ���
    private bool hasTriggered = false;  // ����uĲ�o�@��

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered) // �ˬd���aTag�ýT�O�uĲ�o�@��
        {
            hasTriggered = true;  // �]�m���wĲ�o
            scareSound.Play();  // ���񮣩ƭ���
            scareImage.SetActive(true);  // ��ܥ��̮��ƹϤ�
           
            Invoke("StopScare", 2f);  // 2���ե� StopScare ��k
        }
    }

    // 2��ᰱ��Ϥ���ܩM����
    private void StopScare()
    {
        scareImage.SetActive(false);  // ���î��ƹϤ�
        scareSound.Stop();  // ����ƭ���
    }
}
