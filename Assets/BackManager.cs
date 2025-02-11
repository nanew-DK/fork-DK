using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    private Transform player; // 플레이어 위치 참조
    public GameObject[] background; // 배경 오브젝트 배열 (Empty 부모 오브젝트)
    public GameObject[] cloudGroup; // 구름 그룹 배열 (각 그룹마다 아침, 낮, 저녁, 밤 포함)

    public Vector2 parallaxFactor = new Vector2(0.05f, 0.05f); // 패럴랙스 효과
    public Vector2 offset = new Vector2(0, 0); // 배경 전체 오프셋
    public float cloudSpeed = 1.0f; // 구름 이동 속도
    public float cloudResetX = -230f; // 구름이 사라진 후 다시 생성될 X 좌표
    public float cloudLimitX = 160f; // 구름이 왼쪽으로 이동할 최대 X 좌표

    private Vector3[] initialPositions; // 배경 초기 위치 저장

    private float[,] transitionRanges = {
        { 78f, 98f },   // 첫 번째 배경 → 두 번째 배경 전환
        { 263f, 283f }, // 두 번째 배경 → 세 번째 배경 전환
        { 437f, 467f }  // 세 번째 배경 → 네 번째 배경 전환
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
            Debug.LogError("BackgroundManager: 'Player' 태그를 가진 오브젝트를 찾을 수 없습니다!");
        }

        // 배경 초기 위치 저장
        initialPositions = new Vector3[background.Length];
        for (int i = 0; i < background.Length; i++)
        {
            if (background[i] != null)
            {
                initialPositions[i] = background[i].transform.position;
            }
        }

        // 모든 배경을 투명하게 설정 후, 첫 번째 배경만 보이게 설정
        for (int i = 1; i < background.Length; i++)
        {
            SetAlpha(background[i], 0f);
        }
        SetAlpha(background[0], 1f);

        // 구름 그룹도 동일하게 첫 번째 그룹만 보이게 설정
        for (int i = 1; i < cloudGroup.Length; i++)
        {
            SetAlpha(cloudGroup[i], 0f);
        }
        SetAlpha(cloudGroup[0], 1f);

        // 저장지점에서 태어날 경우 즉시 배경 및 구름 적용
        SetInitialBackground();
        SetInitialCloud();
    }

    void Update()
    {
        if (player == null || background.Length == 0) return;

        // 배경 이동 (패럴랙스 효과)
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

        // 배경 및 구름 상태 조절
        AdjustBackgroundAlpha();
        SwitchCloudTime();

        // 구름 이동 (무한 루프)
        MoveClouds();
    }

    void MoveClouds()
    {
        for (int i = 0; i < cloudGroup.Length; i++)
        {
            if (cloudGroup[i] != null)
            {
                cloudGroup[i].transform.position += Vector3.left * cloudSpeed * Time.deltaTime;

                // 구름이 특정 위치를 벗어나면 다시 오른쪽에서 등장
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

            // 현재 구름 그룹에서 해당 시간대의 구름만 활성화
            ActivateCloudByTime(cloudGroup[i], currentTimeIndex);
        }
    }

    int GetTimeIndex(float playerX)
    {
        if (playerX < transitionRanges[0, 0]) return 0; // 아침
        if (playerX < transitionRanges[1, 0]) return 1; // 낮
        if (playerX < transitionRanges[2, 0]) return 2; // 저녁
        return 3; // 밤
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
