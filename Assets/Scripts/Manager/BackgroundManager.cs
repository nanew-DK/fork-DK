using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    private Transform player; // 플레이어 위치 참조
    public GameObject[] background; // 배경 오브젝트 배열
    public GameObject[] cloudGroup; // 구름 그룹 배열 (각 그룹마다 아침, 낮, 저녁, 밤 포함)

    public Vector2 parallaxFactor = new Vector2(1.5f, 1.2f); // 패럴랙스 효과
    public Vector2 offset = new Vector2(0, 0); // 배경 전체 오프셋
    public float baseCloudSpeed = 2.0f; // 기본 구름 이동 속도
    public float cloudResetX = 230f; // 구름이 다시 생성될 X 좌표
    public float cloudLimitX = -160f; // 구름이 이동할 최대 X 좌표

    private Vector3[] initialPositions; // 배경 초기 위치 저장
    private float[] cloudSpeeds; // 각 구름 그룹의 개별 속도

    private int currentBackgroundIndex = 0; // 현재 활성화된 배경 인덱스

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
        for (int i = 0; i < background.Length; i++)
        {
            SetAlpha(background[i], i == 0 ? 1f : 0f);
        }

        // 구름 위치를 재설정하지 않음!
        for (int i = 0; i < cloudGroup.Length; i++)
        {
            cloudGroup[i].SetActive(true);
        }

        // 각 구름 그룹의 속도를 랜덤하게 설정
        cloudSpeeds = new float[cloudGroup.Length];
        for (int i = 0; i < cloudGroup.Length; i++)
        {
            cloudSpeeds[i] = baseCloudSpeed + Random.Range(-0.5f, 1.5f);
        }
    }


    void Update()
    {
        if (player == null || background.Length == 0) return;

        // 패럴랙스 효과 적용
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

        // 배경 및 구름 상태 조절
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

                // 구름이 화면 밖으로 나가면 원래 위치로 이동 (x만 조정)
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
                currentBackgroundIndex = i + 1; // 현재 배경 업데이트
            }
        }

        // 전환 중이 아닐 때 현재 배경 유지
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
