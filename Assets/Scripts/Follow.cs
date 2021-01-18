using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    MazeGenerator maze;
    [SerializeField]
    float defaultCameraSize = 6f;
    [SerializeField]
    float speed = 6f;

    GameObject target;
    float cameraSize;
    float lerpTime = 0f;

    void Start()
    {
        cameraSize = Mathf.Max(maze.width / 2, maze.height / 2) + 1f;
        transform.position = new Vector3(maze.width / 2, maze.height / 2, transform.position.z);
        Camera.main.orthographicSize = cameraSize;
    }

    void Update()
    {
        if (!maze.mazeBuilt)
        { return; }
        target = FindObjectOfType<Player>().gameObject;
        if (cameraSize != defaultCameraSize)
        {
            Camera.main.orthographicSize = Mathf.Lerp(cameraSize, defaultCameraSize, lerpTime);
            lerpTime += .5f * Time.deltaTime;
        }
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z), speed * Time.deltaTime);

    }
}
