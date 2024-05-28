using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomDestory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("BoomDestroy", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BoomDestroy()
    {
            Destroy(gameObject);
    }
}
