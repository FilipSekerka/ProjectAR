using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantable : MonoBehaviour
{
    public float min_scale = 0.8f;
    public float max_scale = 1.2f;
    public float max_tilt_angle = 5.0f;
    public float scale_modifier = 1.0f;
    private float scale = 0.0f;

    void Start()
    {
        transform.localScale = new Vector3(0, 0, 0);
        scale = Random.Range(min_scale, max_scale);

        transform.eulerAngles = new Vector3(
             Random.Range(-max_tilt_angle, max_tilt_angle),
            Random.Range(0.0f, 360.0f),
            Random.Range(-max_tilt_angle, max_tilt_angle)
        );
    }

    void Update()
    {
        transform.localScale = scale_modifier * new Vector3(scale, scale, scale);
    }
}