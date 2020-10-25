using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SM64SurfaceObject : MonoBehaviour
{
    static float[] vecToArr( Vector3 v )
    {
        return new float[] { v.x, v.y, v.z };
    }

    static float fmod( float a, float b )
    {
        return a - b * Mathf.Floor( a / b );
    }
    
    static float fixAngle( float a )
    {
        return fmod( a + 180.0f, 360.0f ) - 180.0f;
    }

    uint _surfaceObjectId;

    LibSM64Interop.SM64ObjectTransform GetTransform()
    {
        var pos = LibSM64Interop.SCALE_FACTOR * Vector3.Scale( transform.position, new Vector3( -1, 1, 1 ));
        var rot = Vector3.Scale( transform.rotation.eulerAngles, new Vector3( -1, 1, 1 ));

        rot.x = fixAngle( rot.x );
        rot.y = fixAngle( rot.y );
        rot.z = fixAngle( rot.z );

        return new LibSM64Interop.SM64ObjectTransform {
            position = vecToArr( pos ),
            eulerRotation = vecToArr( rot )
        };
    }

    void Start()
    {
        var surfaces = new List<LibSM64Interop.SM64Surface>();

        var mc = GetComponent<MeshCollider>();

        var mesh = mc.sharedMesh;
        var tris = mesh.GetTriangles(0);

        var vertices = mesh.vertices.Select(x => Vector3.Scale( mc.transform.localScale, x )).ToArray();

        for( int i = 0; i < tris.Length; i += 3 )
        {
            surfaces.Add(new LibSM64Interop.SM64Surface {
                force = 0,
                type = 0,
                v0x = (short)(LibSM64Interop.SCALE_FACTOR * -vertices[tris[i  ]].x),
                v0y = (short)(LibSM64Interop.SCALE_FACTOR * vertices[tris[i  ]].y),
                v0z = (short)(LibSM64Interop.SCALE_FACTOR * vertices[tris[i  ]].z),
                v1x = (short)(LibSM64Interop.SCALE_FACTOR * -vertices[tris[i+2]].x),
                v1y = (short)(LibSM64Interop.SCALE_FACTOR * vertices[tris[i+2]].y),
                v1z = (short)(LibSM64Interop.SCALE_FACTOR * vertices[tris[i+2]].z),
                v2x = (short)(LibSM64Interop.SCALE_FACTOR * -vertices[tris[i+1]].x),
                v2y = (short)(LibSM64Interop.SCALE_FACTOR * vertices[tris[i+1]].y),
                v2z = (short)(LibSM64Interop.SCALE_FACTOR * vertices[tris[i+1]].z)
            });
        }

        var trans = GetTransform();
        _surfaceObjectId = LibSM64Interop.LoadSurfaceObject( trans, surfaces.ToArray() );
    }

    public void Tock()
    {
        //transform.position += Mathf.Sin( Time.time ) * Vector3.forward * 10.0f / LibSM64Interop.SCALE_FACTOR;
        //transform.rotation *= Quaternion.Euler( 1.5f, 1, 0 );

        LibSM64Interop.MoveObject( _surfaceObjectId, GetTransform() );
    }
}