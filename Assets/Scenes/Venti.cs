using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venti : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, 120f) * Time.deltaTime);
    }
}
