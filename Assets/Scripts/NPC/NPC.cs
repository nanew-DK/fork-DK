using UnityEngine;

public class NPC : MonoBehaviour
{
    public DialogueData dialogue; // 이 NPC의 대화 데이터
    private bool playerInRange = false; // 플레이어가 NPC 근처에 있는지 체크
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>(); // 씬에서 DialogueManager 찾기
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space)) // 플레이어가 가까운 상태에서 Space 키를 누르면
        {
            if (!dialogueManager.IsDialogueActive) // 대화가 시작되지 않았다면
            {
                dialogueManager.StartDialogue(dialogue); // 대화 시작
            }
            else
            {
                dialogueManager.ShowNextDialogue(); // 대화 중이라면 대사 진행
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어가 NPC 범위에 들어오면
        {
            playerInRange = true; // 범위 내 플레이어 감지
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어가 NPC 범위에서 벗어나면
        {
            playerInRange = false; // 범위 내 플레이어 제거
        }
    }
}