using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class SM64Mario : MonoBehaviour
{
    Vector3 lastMarioPos = Vector3.zero;

    Vector3[][] positionBuffers;
    Vector3[][] normalBuffers;

    Vector3[] lerpPositionBuffer;
    Vector3[] lerpNormalBuffer;
    Vector3[] colorBuffer;
    Vector2[] uvBuffer;

    Mesh mesh1;
    int buffIndex;

    void Awake()
    {
        LibSM64Interop.InitWithROM(File.ReadAllBytes(Application.dataPath + "/../baserom.us.z64"));

        var surfaces = new List<LibSM64Interop.SM64Surface>();
        var meshColliders = FindObjectsOfType<MeshCollider>();

        foreach( var mc in meshColliders )
        {
            var mesh = mc.sharedMesh;
            var tris = mesh.GetTriangles(0);

            var vertices = mesh.vertices.Select(x => {
                return mc.transform.TransformPoint( x );
            }).ToArray();

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
        }

        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", LibSM64Interop.marioTexture);

        var arr = surfaces.ToArray();

        LibSM64Interop.LoadSurfaces( SM64TerrainType.TERRAIN_STONE, surfaces.ToArray() );
        LibSM64Interop.MarioReset( new Vector3( -transform.position.x, transform.position.y, transform.position.z ) * LibSM64Interop.SCALE_FACTOR );

        transform.parent = null;
        transform.localScale = new Vector3( -1, 1, 1 ) / LibSM64Interop.SCALE_FACTOR;
        transform.localPosition = Vector3.zero;

        lerpPositionBuffer = new Vector3[3 * LibSM64Interop.SM64_GEO_MAX_TRIANGLES];
        lerpNormalBuffer = new Vector3[3 * LibSM64Interop.SM64_GEO_MAX_TRIANGLES];
        positionBuffers = new Vector3[][] { new Vector3[3 * LibSM64Interop.SM64_GEO_MAX_TRIANGLES], new Vector3[3 * LibSM64Interop.SM64_GEO_MAX_TRIANGLES] };
        normalBuffers = new Vector3[][] { new Vector3[3 * LibSM64Interop.SM64_GEO_MAX_TRIANGLES], new Vector3[3 * LibSM64Interop.SM64_GEO_MAX_TRIANGLES] };
        colorBuffer = new Vector3[3 * LibSM64Interop.SM64_GEO_MAX_TRIANGLES];
        uvBuffer = new Vector2[3 * LibSM64Interop.SM64_GEO_MAX_TRIANGLES];

        mesh1 = new Mesh();
        mesh1.vertices = lerpPositionBuffer;
        mesh1.triangles = Enumerable.Range(0, 3*1024).ToArray();
        GetComponent<MeshFilter>().sharedMesh = mesh1;
    }

    void FixedUpdate() 
    {
        var cam = FindObjectOfType<Camera>();

        var inputs = new LibSM64Interop.SM64MarioInputs();
        var look = lastMarioPos - cam.transform.position;
        look.y = 0;
        look = look.normalized;

        inputs.camLookX = -look.x;
        inputs.camLookZ = look.z;
        inputs.stickX = Input.GetAxis("Horizontal");
        inputs.stickY = -Input.GetAxis("Vertical");
        inputs.buttonA = Input.GetButton("Jump") ? (byte)1 : (byte)0;
        inputs.buttonB = Input.GetButton("Kick") ? (byte)1 : (byte)0;
        inputs.buttonZ = Input.GetButton("Z") ? (byte)1 : (byte)0;

        var state = LibSM64Interop.MarioTick( inputs, positionBuffers[buffIndex], normalBuffers[buffIndex], colorBuffer, uvBuffer );

        buffIndex = 1 - buffIndex;
    }

    void Update()
    {
        var cam = FindObjectOfType<Camera>();

        float t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        int j = (buffIndex + 1) % 2;
        for( int i = 0; i < lerpPositionBuffer.Length; ++i )
        {
            lerpPositionBuffer[i] = Vector3.LerpUnclamped( positionBuffers[buffIndex][i], positionBuffers[j][i], t );
            lerpNormalBuffer[i] = Vector3.LerpUnclamped( normalBuffers[buffIndex][i], normalBuffers[j][i], t );
        }

        mesh1.vertices = lerpPositionBuffer;
        mesh1.normals = lerpNormalBuffer;
        mesh1.colors = colorBuffer.Select( x => new Color( x.x, x.y, x.z, 1 )).ToArray(); // TODO Don't use linq
        mesh1.uv = uvBuffer;

        mesh1.RecalculateBounds();
        mesh1.RecalculateTangents();

        var targPos = lastMarioPos;
        targPos.x = cam.transform.position.x;
        targPos.y += 5;

        cam.transform.position += (targPos - cam.transform.position) * .25f;
        cam.transform.LookAt( lastMarioPos );
    }
}
