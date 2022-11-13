using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    int speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //*Time.deltaTime adjusts for different speed on computers
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime * speed);
    }
}
