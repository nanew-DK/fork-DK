using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName; // ��ȭ�ϴ� ĳ���� �̸�
    public Sprite speakerImage; // ĳ���� �̹��� 1
    public Sprite speakerImage2; // ĳ���� �̹��� 2 �߰�
    [TextArea(3, 5)] public string dialogueText; // ��ȭ ����
}

// ScriptableObject�� ���� ��ȭ �����͸� ������ �� �ְ� ����
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines; // ���� ���� ��� ����
}
