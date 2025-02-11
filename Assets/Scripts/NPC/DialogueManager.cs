using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // 대화창 패널
    public Text nameText; // 캐릭터 이름 텍스트
    public Image speakerImage; // 캐릭터 이미지 (UI Image)
    public Text dialogueText; // 대화 내용 텍스트

    public DialogueData dialogueData; // 대화 데이터
    private int currentLineIndex = 0; // 현재 대사 인덱스
    private bool isDialogueActive = false; // 대화 활성화 상태 (private 유지)

    public bool IsDialogueActive => isDialogueActive; // isDialogueActive 프로퍼티 추가

    public Transform player; // 플레이어의 Transform (이걸로 거리 체크)
    public Transform npc; // NPC의 Transform
    public float dialogueTriggerDistance = 2f; // 대화가 시작될 거리 (기본값 2f)

    void Start()
    {
        dialoguePanel.SetActive(false); // 대화창을 초기에는 비활성화
    }

    void Update()
    {
        // 플레이어와 NPC 사이의 거리를 계산
        float distance = Vector3.Distance(player.position, npc.position);

        if (distance <= dialogueTriggerDistance) // 일정 거리 이내에 있을 때만 대화 시작 가능
        {
            if (Input.GetKeyDown(KeyCode.Space)) // 대화 중 Space 키를 눌렀을 때
            {
                if (!isDialogueActive)
                {
                    // 대화가 시작되지 않았다면 대화 시작
                    StartDialogue(dialogueData);
                }
                else
                {
                    // 대화 중이라면 대사 진행
                    ShowNextDialogue();
                }
            }
        }
    }

    // 대화 시작
    public void StartDialogue(DialogueData newDialogue)
    {
        dialogueData = newDialogue;
        dialoguePanel.SetActive(true); // 대화창 활성화
        isDialogueActive = true;
        currentLineIndex = 0;
        ShowNextDialogue(); // 첫 번째 대사 보여주기
    }

    // 대사 진행
    public void ShowNextDialogue()
    {
        if (currentLineIndex < dialogueData.lines.Length)
        {
            DialogueLine line = dialogueData.lines[currentLineIndex];
            nameText.text = line.speakerName; // 캐릭터 이름
            speakerImage.sprite = line.speakerImage; // 캐릭터 이미지
            dialogueText.text = line.dialogueText; // 대화 내용

            currentLineIndex++; // 다음 대사로 넘어가기
        }
        else
        {
            EndDialogue(); // 대화가 끝났으면 종료
        }
    }

    // 대화 종료
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false); // 대화창 비활성화
        isDialogueActive = false; // 대화 종료 상태
        currentLineIndex = 0; // 인덱스 초기화
    }
}
