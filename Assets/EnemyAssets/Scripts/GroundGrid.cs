using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGrid : MonoBehaviour
{
    private GridMap g;
    // Start is called before the first frame update
    void Start()
    {
        g = new GridMap(50, 33, 3.0f, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
