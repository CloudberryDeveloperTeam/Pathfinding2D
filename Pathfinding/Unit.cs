using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 20;

    Rigidbody2D rb;

    public bool canTrackPlayer;

    Vector2[] path;
    int targetIndex;


    void Start()
    {
        if (transform.position != target.position)
        {
            path = Pathfinding.RequestPath(transform.position, target.position);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }

    }

    void Update()
    {
        if (canTrackPlayer == true && transform.position != target.position)
        {
            path = Pathfinding.RequestPath(transform.position, target.position);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }

        if (transform.position == target.position)
        {
            canTrackPlayer = false;
            Debug.Log("durmali");
        }

    }

    IEnumerator FollowPath()
    {
        Vector2 currentWaypoint = path[0];

        while (true)
        {
            if ((Vector2)transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canTrackPlayer = true;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube((Vector3)path[i], Vector3.one *.5f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
