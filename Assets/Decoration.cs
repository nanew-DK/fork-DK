using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration : MonoBehaviour
{
    public Transform backGround; // �θ� ������Ʈ
    public float[] speeds = new float[3] { 1.0f, 0.5f, 2.0f }; // �� �ڽ� ������Ʈ�� ���� �ӵ�
    public float resetX = 10f;  // ������Ʈ�� �ٽ� �����ʿ��� ������ ��ġ
    public float destroyX = -10f; // ���� �� (�� ��ǥ�� ������ �ٽ� ���������� �̵�)

    public float[] stopPositions = new float[3] { -5f, -3f, -7f }; // �� �ڽ� ������Ʈ�� ���� x ��ǥ

    private Transform[] children; // �ڽ� ������Ʈ �迭
    private bool[] stopped; // �� �ڽ� ������Ʈ�� ���� ����

    void Start()
    {
        if (backGround != null)
        {
            int childCount = backGround.childCount;
            children = new Transform[childCount];
            stopped = new bool[childCount]; // �ʱ⿡�� ��� �̵� ����

            for (int i = 0; i < childCount; i++)
            {
                children[i] = backGround.GetChild(i);
                stopped[i] = false; // �ʱ� ����: �̵� ����
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i] != null && !stopped[i]) // ���� ������Ʈ�� �̵����� ����
            {
                // �������� �̵�
                children[i].position += Vector3.left * speeds[i] * Time.deltaTime;

                // Ư�� ��ǥ�� �����ϸ� ����
                if (children[i].position.x <= stopPositions[i])
                {
                    stopped[i] = true; // �̵� ����
                }

                // ���� ���� �Ѿ�� ���������� �ٽ� �̵� & ���� ���� ����
                if (children[i].position.x <= destroyX)
                {
                    children[i].position = new Vector3(resetX, children[i].position.y, children[i].position.z);
                    stopped[i] = false; // �̵� �ٽ� ����
                }
            }
        }
    }
}
