using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private GridMap g;
    void Start()
    {
        g = new GridMap(3, 5, 10f, gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            g.SetValue(2, 2, 12);
        }
    }
}
