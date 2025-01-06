using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private GameObject tail;//마지막 원으로 
    [SerializeField] private int numOfNodes = 1;
    [SerializeField] private GameObject nodePrefab;
    GameObject beforePrefab;
    private float prefabDistance = 0f;
    static public Rigidbody2D FindBefore(HingeJoint2D linkedHinge)
    {
        HingeJoint2D cur = linkedHinge;
        Rigidbody2D connectedRigidbody = linkedHinge.connectedBody;
        Rope headRope;// 유니티에서 Static Rope의 Ropescript
        
        while (true)//Rope 스크립트가 들어간 오브젝트를 찾을 때 까지 루프
        {
            connectedRigidbody = cur.connectedBody;
            if (connectedRigidbody.GetComponent<Rope>() != null) break;
            cur = connectedRigidbody.GetComponent<HingeJoint2D>();
        }//Found head node

        headRope = connectedRigidbody.gameObject.GetComponent<Rope>();
        HingeJoint2D tailHj = headRope.tail.GetComponent<HingeJoint2D>();
        Rigidbody2D prevObj = headRope.tail.GetComponent<Rigidbody2D>();
        if (linkedHinge.GetComponent<Rigidbody2D>() == tailHj.connectedBody) return linkedHinge.GetComponent<Rigidbody2D>();
        while (tailHj != linkedHinge)//linkedHinge의 전에 있는 오브젝트를 찾을 때 까지 루프
        {
            prevObj = connectedRigidbody;
            connectedRigidbody = tailHj.connectedBody;
            tailHj = connectedRigidbody.GetComponent<HingeJoint2D>();
        }
        return prevObj;
    }
    static public Rigidbody2D FindHead(HingeJoint2D linkedHinge)
    //FindBefore부분의 Rope스크립트가 들어간 오브젝트를 찾는 구간의 스크립트
    {
        HingeJoint2D cur = linkedHinge;
        Rigidbody2D connectedRigidbody = linkedHinge.connectedBody;

        while (true)
        {
            connectedRigidbody = cur.connectedBody;
            if (connectedRigidbody.GetComponent<Rope>() != null) break;
            cur = connectedRigidbody.GetComponent<HingeJoint2D>();
        }//Found head node
        return connectedRigidbody;
    }
    public GameObject GetTail()
    {
        return tail;
    }

    private void Start()
    {
        beforePrefab = this.gameObject;
        prefabDistance = nodePrefab.transform.localScale.y * 2;
        for (int i = 0; i < numOfNodes; i++)
        {
            GameObject Node = Instantiate(nodePrefab,new Vector2(beforePrefab.transform.position.x, beforePrefab.transform.position.y- prefabDistance),Quaternion.identity,this.transform);
            HingeJoint2D NodeHj=Node.GetComponent<HingeJoint2D>();
            NodeHj.connectedBody = beforePrefab.GetComponent<Rigidbody2D>();
            NodeHj.anchor = new Vector2(0, 0.5f);
            NodeHj.connectedAnchor = new Vector2(0, -0.5f);
            beforePrefab = Node;
        }
        HingeJoint2D tailHj=tail.GetComponent<HingeJoint2D>();
        tail.transform.position = new Vector2(beforePrefab.transform.position.x, beforePrefab.transform.position.y - prefabDistance);
        tailHj.connectedBody= beforePrefab.GetComponent<Rigidbody2D>();
        tailHj.anchor = new Vector2(0, 0.5f);
        tailHj.connectedAnchor = new Vector2(0, -0.5f);
    }
}
