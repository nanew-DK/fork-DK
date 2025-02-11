using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName; // 대화하는 캐릭터 이름
    public Sprite speakerImage; // 캐릭터 이미지 1
    public Sprite speakerImage2; // 캐릭터 이미지 2 추가
    [TextArea(3, 5)] public string dialogueText; // 대화 내용
}

// ScriptableObject로 여러 대화 데이터를 저장할 수 있게 설정
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines; // 여러 개의 대사 저장
}
