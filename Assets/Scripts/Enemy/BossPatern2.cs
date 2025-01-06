using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatern2 : MonoBehaviour
{
    public GameObject leg;
    public GameObject dangerArea;
    private GameObject legAttack;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float diff = 3.7f;
    [SerializeField] private float attackTurm = 3f;
    [SerializeField] private int page = 1;

    private int legWhere;
    private int currentLegWhere = 0;
    private int moveCount = 0;
    private int direction = 1;
    private float randomX;
    private float randomY;

    private bool isWaiting = true;
    private bool threating = false;
    private bool gidare = true;
    
    private Vector3 startPos;
    private Vector3 targetPos;
    
    float[,] legPositions = {
        { -22f,  3f, 1 },
        { -22f, -2f, 1 },
        { -22f, -7f, 1 },
        {  22f,  3f, -1 },
        {  22f, -2f, -1 },
        {  22f, -7f, -1 }
    };

    void Update()
    {
        //여기서 보스의 명령을 받을 예정 Update에서는 나가고
        if (Input.GetKey(KeyCode.A) && gidare)
        {
            Debug.Log("공격");
            moveCount = 0;
            StartCoroutine("Wait");
            Attack();
        }
    }
    private IEnumerator Wait()
    {
        gidare = false;
        yield return new WaitForSeconds(1f); // 대기 시간
        gidare = true;
    }
    public void Attack()
    {
        Debug.Log("패턴2");
        List<int> usedIndices = new List<int>();
        for (int i = 0;i < page; i++)
        {
            // 랜덤 인덱스 선택
            do
            {
               legWhere = Random.Range(0, legPositions.GetLength(0));
            } while (usedIndices.Contains(legWhere)); // 이미 사용된 인덱스는 제외
            usedIndices.Add(legWhere);

            // 값 할당
            randomX = legPositions[legWhere, 0];
            randomY = legPositions[legWhere, 1];
            direction = (int)legPositions[legWhere, 2];

            currentLegWhere = legWhere;

            // 방향 결정
            Vector3 legDirection = leg.transform.localScale;
            if (direction == 1)
            {
                legDirection.x = Mathf.Abs(legDirection.x);
            }
            else
            {
                legDirection.x = -Mathf.Abs(legDirection.x);
            }

            // 다리 공격 생성
            Vector2 SpawnPos = new Vector2(randomX, randomY);
            legAttack = Instantiate(leg, SpawnPos, transform.rotation);
            legAttack.transform.localScale = legDirection;

            // 왕복할 목표 위치 설정
            startPos = legAttack.transform.position;
            targetPos = new Vector3(randomX + 20f * direction, randomY, startPos.z);  // 왕복 거리 설정
            GameObject dangerByLeg = Instantiate(dangerArea, new Vector3(0f, randomY + diff, 0f), transform.rotation);

            usedIndices.Clear();

            StartCoroutine(MoveLeg(legAttack, startPos, targetPos));
            Destroy(legAttack, 4f);  // 일정 시간 후에 다리 공격 삭제
            Destroy(dangerByLeg, 2f);
        }
    }
    private IEnumerator MoveLeg(GameObject legAttack, Vector3 startPos, Vector3 targetPos)
    {
        bool isMovingForward = true;
        yield return new WaitForSeconds(2f);
        while (legAttack != null)
        {
            if (isMovingForward)
            {
                legAttack.transform.position = Vector3.MoveTowards(legAttack.transform.position, targetPos, speed * Time.deltaTime);
                if (Vector3.Distance(legAttack.transform.position, targetPos) < 0.1f)
                {
                    isMovingForward = false;
                    yield return new WaitForSeconds(1f); // 대기 시간
                }
            }
            else
            {
                legAttack.transform.position = Vector3.MoveTowards(legAttack.transform.position, startPos, speed * Time.deltaTime);
                if (Vector3.Distance(legAttack.transform.position, startPos) < 0.1f)
                {
                    break; // 시작 위치로 돌아오면 이동 종료
                }
            }
            yield return null;
        }
    }
}
