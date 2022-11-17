using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLightController : MonoBehaviour
{
    Light _light;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        _light.intensity = Mathf.PingPong(Time.time*1.6f, 0.4f)+0.4f;
        //_light.intensity = (Random.Range(.5f, 1));
        //_light.intensity = (Mathf.PingPong(Time.time * 1.6f, 0.4f) + 0.4f) + ((Random.Range(0, 0.1f)));
        //Debug.Log(_light.intensity);
    }
}
