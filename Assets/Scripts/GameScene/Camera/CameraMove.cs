using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;    //目标对象

    public Vector3 offsetPos;   //摄像机偏移位置

    public float bodyHeight;    //y偏移值

    public float moveSpeed;     //移动速度
    public float rotateSpeed;   //旋转速度

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        Vector3 targetPos = target.position + target.forward * offsetPos.z;
        targetPos += Vector3.up * offsetPos.y;
        targetPos += target.right * offsetPos.x;

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed*Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(target.position + Vector3.up * bodyHeight - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation, rotateSpeed*Time.deltaTime);
    }

    public void SetTarget(Transform player)
    {
        target = player;
    }
}
