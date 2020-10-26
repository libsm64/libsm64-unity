using UnityEngine;
using LibSM64;

public class ExamplePlatformMover : MonoBehaviour
{
    SM64DynamicTerrain platform;
    Quaternion rotStep;
    
    void Start()
    {
        platform = GetComponent<SM64DynamicTerrain>();
        rotStep = Quaternion.AngleAxis( 1f, Random.onUnitSphere );
    }

    void FixedUpdate()
    {
        platform.SetPosition( Vector3.right * 50.0f * Mathf.Cos( 0.1f * Time.fixedTime + Mathf.PI ));
        platform.SetRotation( platform.rotation * rotStep );
    }
}
