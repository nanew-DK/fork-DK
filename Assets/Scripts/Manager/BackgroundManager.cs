using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    private Transform player; // �÷��̾� ��ġ ����
    public GameObject[] background; // ��� ������Ʈ �迭
    public GameObject[] cloudGroup; // ���� �׷� �迭 (�� �׷츶�� ��ħ, ��, ����, �� ����)

    public Vector2 parallaxFactor = new Vector2(1.5f, 1.2f); // �з����� ȿ��
    public Vector2 offset = new Vector2(0, 0); // ��� ��ü ������
    public float baseCloudSpeed = 2.0f; // �⺻ ���� �̵� �ӵ�
    public float cloudResetX = 230f; // ������ �ٽ� ������ X ��ǥ
    public float cloudLimitX = -160f; // ������ �̵��� �ִ� X ��ǥ

    private Vector3[] initialPositions; // ��� �ʱ� ��ġ ����
    private float[] cloudSpeeds; // �� ���� �׷��� ���� �ӵ�

    private int currentBackgroundIndex = 0; // ���� Ȱ��ȭ�� ��� �ε���

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
        for (int i = 0; i < background.Length; i++)
        {
            SetAlpha(background[i], i == 0 ? 1f : 0f);
        }

        // ���� ��ġ�� �缳������ ����!
        for (int i = 0; i < cloudGroup.Length; i++)
        {
            cloudGroup[i].SetActive(true);
        }

        // �� ���� �׷��� �ӵ��� �����ϰ� ����
        cloudSpeeds = new float[cloudGroup.Length];
        for (int i = 0; i < cloudGroup.Length; i++)
        {
            cloudSpeeds[i] = baseCloudSpeed + Random.Range(-0.5f, 1.5f);
        }
    }


    void Update()
    {
        if (player == null || background.Length == 0) return;

        // �з����� ȿ�� ����
        for (int i = 0; i < background.Length; i++)
        {
            if (background[i] != null)
            {
                float distanceFactor = Mathf.Lerp(1f, 2.5f, i / (float)background.Length);
                Vector3 newPosition = new Vector3(
                    initialPositions[i].x - (player.position.x * parallaxFactor.x * distanceFactor) + offset.x,
                    initialPositions[i].y + offset.y,
                    background[i].transform.position.z
                );
                background[i].transform.position = newPosition;
            }
        }

        // ��� �� ���� ���� ����
        AdjustBackgroundAlpha();
        MoveClouds();
    }

    void MoveClouds()
    {
        for (int i = 0; i < cloudGroup.Length; i++)
        {
            if (cloudGroup[i] != null)
            {
                cloudGroup[i].transform.position += Vector3.left * cloudSpeeds[i] * Time.deltaTime;

                // ������ ȭ�� ������ ������ ���� ��ġ�� �̵� (x�� ����)
                if (cloudGroup[i].transform.position.x <= cloudLimitX)
                {
                    cloudGroup[i].transform.position = new Vector3(cloudResetX, cloudGroup[i].transform.position.y, cloudGroup[i].transform.position.z);
                }
            }
        }
    }

    void AdjustBackgroundAlpha()
    {
        bool isTransitioning = false;

        for (int i = 0; i < transitionRanges.GetLength(0); i++)
        {
            float startX = transitionRanges[i, 0];
            float endX = transitionRanges[i, 1];

            if (player.position.x > startX && player.position.x < endX)
            {
                float t = Mathf.SmoothStep(0f, 1f, (player.position.x - startX) / (endX - startX));

                SetAlpha(background[i], 1.0f);
                SetAlpha(background[i + 1], t);

                isTransitioning = true;
            }

            if (player.position.x >= endX + 5f)
            {
                SetAlpha(background[i], 0f);
                currentBackgroundIndex = i + 1; // ���� ��� ������Ʈ
            }
        }

        // ��ȯ ���� �ƴ� �� ���� ��� ����
        if (!isTransitioning)
        {
            for (int i = 0; i < background.Length; i++)
            {
                SetAlpha(background[i], i == currentBackgroundIndex ? 1f : 0f);
            }
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
