using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private Transform player;
    private float skeletonMoveSpeed = 3.0f;
    private float skeletonRotateSpeed = 5.0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;

            if (direction.sqrMagnitude > 0)
            {
                transform.position += direction.normalized * skeletonMoveSpeed * Time.deltaTime;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * skeletonRotateSpeed);
            }
        }
    }
}
