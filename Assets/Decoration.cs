using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration : MonoBehaviour
{
    public Transform backGround; // 부모 오브젝트
    public float[] speeds = new float[3] { 1.0f, 0.5f, 2.0f }; // 각 자식 오브젝트의 개별 속도
    public float resetX = 10f;  // 오브젝트가 다시 오른쪽에서 시작할 위치
    public float destroyX = -10f; // 왼쪽 끝 (이 좌표를 지나면 다시 오른쪽으로 이동)

    public float[] stopPositions = new float[3] { -5f, -3f, -7f }; // 각 자식 오브젝트의 멈출 x 좌표

    private Transform[] children; // 자식 오브젝트 배열
    private bool[] stopped; // 각 자식 오브젝트의 멈춤 여부

    void Start()
    {
        if (backGround != null)
        {
            int childCount = backGround.childCount;
            children = new Transform[childCount];
            stopped = new bool[childCount]; // 초기에는 모두 이동 상태

            for (int i = 0; i < childCount; i++)
            {
                children[i] = backGround.GetChild(i);
                stopped[i] = false; // 초기 상태: 이동 가능
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i] != null && !stopped[i]) // 멈춘 오브젝트는 이동하지 않음
            {
                // 왼쪽으로 이동
                children[i].position += Vector3.left * speeds[i] * Time.deltaTime;

                // 특정 좌표에 도달하면 멈춤
                if (children[i].position.x <= stopPositions[i])
                {
                    stopped[i] = true; // 이동 중지
                }

                // 왼쪽 끝을 넘어서면 오른쪽으로 다시 이동 & 멈춤 상태 해제
                if (children[i].position.x <= destroyX)
                {
                    children[i].position = new Vector3(resetX, children[i].position.y, children[i].position.z);
                    stopped[i] = false; // 이동 다시 시작
                }
            }
        }
    }
}
