using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    private Transform player; // �÷��̾� ��ġ ����
    public GameObject[] background; // ��� ������Ʈ �迭 (Empty �θ� ������Ʈ)
    public GameObject[] cloudGroup; // ���� �׷� �迭 (�� �׷츶�� ��ħ, ��, ����, �� ����)

    public Vector2 parallaxFactor = new Vector2(0.05f, 0.05f); // �з����� ȿ��
    public Vector2 offset = new Vector2(0, 0); // ��� ��ü ������
    public float cloudSpeed = 1.0f; // ���� �̵� �ӵ�
    public float cloudResetX = -230f; // ������ ����� �� �ٽ� ������ X ��ǥ
    public float cloudLimitX = 160f; // ������ �������� �̵��� �ִ� X ��ǥ

    private Vector3[] initialPositions; // ��� �ʱ� ��ġ ����

    private float[,] transitionRanges = {
        { 78f, 98f },   // ù ��° ��� �� �� ��° ��� ��ȯ
        { 263f, 283f }, // �� ��° ��� �� �� ��° ��� ��ȯ
        { 437f, 467f }  // �� ��° ��� �� �� ��° ��� ��ȯ
    };

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("BackgroundManager: 'Player' �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�!");
        }

        // ��� �ʱ� ��ġ ����
        initialPositions = new Vector3[background.Length];
        for (int i = 0; i < background.Length; i++)
        {
            if (background[i] != null)
            {
                initialPositions[i] = background[i].transform.position;
            }
        }

        // ��� ����� �����ϰ� ���� ��, ù ��° ��游 ���̰� ����
        for (int i = 1; i < background.Length; i++)
        {
            SetAlpha(background[i], 0f);
        }
        SetAlpha(background[0], 1f);

        // ���� �׷쵵 �����ϰ� ù ��° �׷츸 ���̰� ����
        for (int i = 1; i < cloudGroup.Length; i++)
        {
            SetAlpha(cloudGroup[i], 0f);
        }
        SetAlpha(cloudGroup[0], 1f);

        // ������������ �¾ ��� ��� ��� �� ���� ����
        SetInitialBackground();
        SetInitialCloud();
    }

    void Update()
    {
        if (player == null || background.Length == 0) return;

        // ��� �̵� (�з����� ȿ��)
        for (int i = 0; i < background.Length; i++)
        {
            if (background[i] != null)
            {
                Vector3 newPosition = new Vector3(
                    initialPositions[i].x - (player.position.x * parallaxFactor.x) + offset.x,
                    initialPositions[i].y + offset.y,
                    background[i].transform.position.z
                );
                background[i].transform.position = newPosition;
            }
        }

        // ��� �� ���� ���� ����
        AdjustBackgroundAlpha();
        SwitchCloudTime();

        // ���� �̵� (���� ����)
        MoveClouds();
    }

    void MoveClouds()
    {
        for (int i = 0; i < cloudGroup.Length; i++)
        {
            if (cloudGroup[i] != null)
            {
                cloudGroup[i].transform.position += Vector3.left * cloudSpeed * Time.deltaTime;

                // ������ Ư�� ��ġ�� ����� �ٽ� �����ʿ��� ����
                if (cloudGroup[i].transform.position.x <= cloudLimitX)
                {
                    cloudGroup[i].transform.position = new Vector3(cloudResetX, cloudGroup[i].transform.position.y, cloudGroup[i].transform.position.z);
                }
            }
        }
    }

    void AdjustBackgroundAlpha()
    {
        for (int i = 0; i < transitionRanges.GetLength(0); i++)
        {
            float startX = transitionRanges[i, 0];
            float endX = transitionRanges[i, 1];

            if (player.position.x > startX && player.position.x < endX)
            {
                float t = Mathf.SmoothStep(0f, 1f, (player.position.x - startX) / (endX - startX));

                SetAlpha(background[i], 1.0f);
                SetAlpha(background[i + 1], t);
            }

            if (player.position.x >= endX + 5f)
            {
                SetAlpha(background[i], 0f);
            }
        }
    }

    void SwitchCloudTime()
    {
        for (int i = 0; i < cloudGroup.Length; i++)
        {
            int currentTimeIndex = GetTimeIndex(player.position.x);

            // ���� ���� �׷쿡�� �ش� �ð����� ������ Ȱ��ȭ
            ActivateCloudByTime(cloudGroup[i], currentTimeIndex);
        }
    }

    int GetTimeIndex(float playerX)
    {
        if (playerX < transitionRanges[0, 0]) return 0; // ��ħ
        if (playerX < transitionRanges[1, 0]) return 1; // ��
        if (playerX < transitionRanges[2, 0]) return 2; // ����
        return 3; // ��
    }

    void ActivateCloudByTime(GameObject cloudParent, int activeIndex)
    {
        if (cloudParent == null) return;

        for (int i = 0; i < cloudParent.transform.childCount; i++)
        {
            GameObject subCloud = cloudParent.transform.GetChild(i).gameObject;
            subCloud.SetActive(i == activeIndex);
        }
    }

    void SetInitialBackground()
    {
        for (int i = 0; i < transitionRanges.GetLength(0); i++)
        {
            float startX = transitionRanges[i, 0];
            float endX = transitionRanges[i, 1];

            if (player.position.x >= startX && player.position.x <= endX)
            {
                SetAlpha(background[i], 1.0f);
                SetAlpha(background[i + 1], 0.0f);
                return;
            }
        }
    }

    void SetInitialCloud()
    {
        for (int i = 0; i < cloudGroup.Length; i++)
        {
            int timeIndex = GetTimeIndex(player.position.x);
            ActivateCloudByTime(cloudGroup[i], timeIndex);
        }
    }

    void SetAlpha(GameObject parent, float alpha)
    {
        if (parent == null) return;

        SpriteRenderer[] spriteRenderers = parent.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                Color color = sr.color;
                color.a = alpha;
                sr.color = color;
            }
        }
    }
}
