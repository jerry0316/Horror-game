using UnityEngine;

public class ChairFallTrigger : MonoBehaviour
{
    public Animator chairAnimator; // 參考椅子的 Animator

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 確認觸發的物件是否是玩家
        {
            Debug.Log("Player has entered the trigger area!"); // 調試訊息
            if (chairAnimator != null)
            {
                chairAnimator.Play("ChairFall"); // 直接播放椅子倒下的動畫
                Debug.Log("Chair fall animation triggered!"); // 調試訊息
            }
            else
            {
                Debug.LogError("Chair animator is not assigned!"); // 錯誤訊息
            }
        }
    }
}
