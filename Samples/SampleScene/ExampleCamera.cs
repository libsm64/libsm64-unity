using System;
using UnityEngine;
using LibSM64;

public class ExampleCamera : MonoBehaviour
{
    [SerializeField] SM64Mario mario = null;

    void Update()
    {
        var targPos = mario.actualPosition;
        targPos.x = transform.position.x;
        targPos.y += 5;

        // TODO fix that this is framerate dependent
        transform.position += (targPos - transform.position) * .25f;
        transform.LookAt( mario.actualPosition );
    }
}
