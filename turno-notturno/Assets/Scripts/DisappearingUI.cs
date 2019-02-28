using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingUI : MonoBehaviour
{

    public float lifeTime;

    private int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        lifeTime = lifeTime * 60;
    }

    // Update is called once per frame
    void Update()
    {
        counter++;
        if(counter > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
