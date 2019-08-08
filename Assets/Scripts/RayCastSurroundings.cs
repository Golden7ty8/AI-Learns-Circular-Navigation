using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastSurroundings : MonoBehaviour
{

    public float yOriginOffset;

    public List<Vector2> rayDirXZ;
    public List<float> rayDirY;

    public List<Ray> rays;

    // Start is called before the first frame update
    void Start()
    {
        rays = new List<Ray>();

        for(int i = 0; i < rayDirXZ.Count; i++)
        {
            for (int j = 0; j < rayDirY.Count; j++)
            {
                rays.Add(new Ray(new Vector3(transform.position.x, transform.position.y + yOriginOffset, transform.position.z), new Vector3(rayDirXZ[i].x, rayDirXZ[i].y, rayDirY[j])));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*for(int i = 0; i < rays.Count; i++)
        {
            Debug.Log(rays[i].direction);
        }*/

        DrawRays();
    }

    void UpdateRays()
    {
        for (int i = 0; i < rayDirXZ.Count; i++)
        {
            for (int j = 0; j < rayDirY.Count; j++)
            {
                rays[i] = new Ray(new Vector3(transform.position.x, transform.position.y + yOriginOffset, transform.position.z), new Vector3(rayDirXZ[i].x, rayDirXZ[i].y, rayDirY[j]));
            }
        }
    }

    void DrawRays() {
        for(int i = 0; i < rays.Count; i++)
        {
            Debug.DrawRay(rays[i].origin, rays[i].direction, Color.blue);
        }
    }
}
