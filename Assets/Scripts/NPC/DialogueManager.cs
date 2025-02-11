using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // ��ȭâ �г�
    public Text nameText; // ĳ���� �̸� �ؽ�Ʈ
    public Image speakerImage; // ĳ���� �̹��� (UI Image)
    public Text dialogueText; // ��ȭ ���� �ؽ�Ʈ

    public DialogueData dialogueData; // ��ȭ ������
    private int currentLineIndex = 0; // ���� ��� �ε���
    private bool isDialogueActive = false; // ��ȭ Ȱ��ȭ ���� (private ����)

    public bool IsDialogueActive => isDialogueActive; // isDialogueActive ������Ƽ �߰�

    public Transform player; // �÷��̾��� Transform (�̰ɷ� �Ÿ� üũ)
    public Transform npc; // NPC�� Transform
    public float dialogueTriggerDistance = 2f; // ��ȭ�� ���۵� �Ÿ� (�⺻�� 2f)

    void Start()
    {
        dialoguePanel.SetActive(false); // ��ȭâ�� �ʱ⿡�� ��Ȱ��ȭ
    }

    void Update()
    {
        // �÷��̾�� NPC ������ �Ÿ��� ���
        float distance = Vector3.Distance(player.position, npc.position);

        if (distance <= dialogueTriggerDistance) // ���� �Ÿ� �̳��� ���� ���� ��ȭ ���� ����
        {
            if (Input.GetKeyDown(KeyCode.Space)) // ��ȭ �� Space Ű�� ������ ��
            {
                if (!isDialogueActive)
                {
                    // ��ȭ�� ���۵��� �ʾҴٸ� ��ȭ ����
                    StartDialogue(dialogueData);
                }
                else
                {
                    // ��ȭ ���̶�� ��� ����
                    ShowNextDialogue();
                }
            }
        }
    }

    // ��ȭ ����
    public void StartDialogue(DialogueData newDialogue)
    {
        dialogueData = newDialogue;
        dialoguePanel.SetActive(true); // ��ȭâ Ȱ��ȭ
        isDialogueActive = true;
        currentLineIndex = 0;
        ShowNextDialogue(); // ù ��° ��� �����ֱ�
    }

    // ��� ����
    public void ShowNextDialogue()
    {
        if (currentLineIndex < dialogueData.lines.Length)
        {
            DialogueLine line = dialogueData.lines[currentLineIndex];
            nameText.text = line.speakerName; // ĳ���� �̸�
            speakerImage.sprite = line.speakerImage; // ĳ���� �̹���
            dialogueText.text = line.dialogueText; // ��ȭ ����

            currentLineIndex++; // ���� ���� �Ѿ��
        }
        else
        {
            EndDialogue(); // ��ȭ�� �������� ����
        }
    }

    // ��ȭ ����
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false); // ��ȭâ ��Ȱ��ȭ
        isDialogueActive = false; // ��ȭ ���� ����
        currentLineIndex = 0; // �ε��� �ʱ�ȭ
    }
}
