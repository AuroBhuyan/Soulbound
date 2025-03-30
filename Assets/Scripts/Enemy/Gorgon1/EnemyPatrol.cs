using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform patrolPoint1;
    public Transform patrolPoint2;
    public float speed = 2f;
    private Vector3 target;
    public float threshold = 0.2f;

    void Start()
    {
        target = patrolPoint1.position;
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < threshold)
        {
            if (target == patrolPoint1.position)
            {
                target = patrolPoint2.position;
                Flip(true);
            }
            else
            {
                target = patrolPoint1.position;
                Flip(false);
            }
        }
    }

    void Flip(bool faceRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = faceRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
