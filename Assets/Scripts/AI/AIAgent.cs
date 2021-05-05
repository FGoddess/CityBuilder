using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AIAgent : MonoBehaviour
{
    public event Action OnDeath;

    private Animator animator;
    private float speed = 0.2f;
    private float rotationSpeed = 10f;

    //TODO: rename pathToGO -> Waypoints; moveFlag -> isMoving; endPos -> nextPoint
    private List<Vector3> pathToGo = new List<Vector3>();
    private bool moveFlag = false;
    private int index = 0;
    private Vector3 endPos;

    public void Initialize(List<Vector3> path)
    {
        pathToGo = path;
        index = 1;
        moveFlag = true;
        endPos = pathToGo[index];
        animator = GetComponent<Animator>();
        animator.SetTrigger("Walk");
    }

    private void Update()
    {
        if (moveFlag)
        {
            PerformMovement();
        }
    }

    private void PerformMovement()
    {
        if (pathToGo.Count > index)
        {
            float distanceToGo = MoveAgent();
            if (distanceToGo < 0.05f) //Mathf.Epsilon
            {
                index++;
                if (index >= pathToGo.Count)
                {
                    moveFlag = false;
                    Destroy(gameObject);
                    return;
                }
                endPos = pathToGo[index];
            }
        }
    }

    private float MoveAgent()
    {
        float step = speed * Time.deltaTime;
        Vector3 endPosCorrect = new Vector3(endPos.x, transform.position.y, endPos.z);
        transform.position = Vector3.MoveTowards(transform.position, endPosCorrect, step);
        var lookDir = endPosCorrect - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * rotationSpeed);

        return Vector3.Distance(transform.position, endPosCorrect);
    }

    private void OnDestroy()
    {
        OnDeath?.Invoke();
    }
}
