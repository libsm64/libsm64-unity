using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LibSM64
{
    static internal class Utils
    {
        static public Interop.SM64Surface[] GetSurfacesForMesh( Vector3 scale, Mesh mesh )
        {
            var surfaces = new List<Interop.SM64Surface>();
            var tris = mesh.GetTriangles(0);
            var vertices = mesh.vertices.Select(x => Vector3.Scale( scale, x )).ToArray();

            for( int i = 0; i < tris.Length; i += 3 )
            {
                surfaces.Add(new Interop.SM64Surface {
                    force = 0,
                    type = 0,
                    v0x = (short)(Interop.SCALE_FACTOR * -vertices[tris[i  ]].x),
                    v0y = (short)(Interop.SCALE_FACTOR * vertices[tris[i  ]].y),
                    v0z = (short)(Interop.SCALE_FACTOR * vertices[tris[i  ]].z),
                    v1x = (short)(Interop.SCALE_FACTOR * -vertices[tris[i+2]].x),
                    v1y = (short)(Interop.SCALE_FACTOR * vertices[tris[i+2]].y),
                    v1z = (short)(Interop.SCALE_FACTOR * vertices[tris[i+2]].z),
                    v2x = (short)(Interop.SCALE_FACTOR * -vertices[tris[i+1]].x),
                    v2y = (short)(Interop.SCALE_FACTOR * vertices[tris[i+1]].y),
                    v2z = (short)(Interop.SCALE_FACTOR * vertices[tris[i+1]].z)
                });
            }

            return surfaces.ToArray();
        }

        static public Interop.SM64Surface[] GetAllStaticSurfaces()
        {
            var surfaces = new List<Interop.SM64Surface>();
            var meshColliders = GameObject.FindObjectsOfType<MeshCollider>();

            foreach( var mc in meshColliders )
            {
                if( mc.GetComponent<SM64SurfaceObject>() != null )
                    continue;

                var mesh = mc.sharedMesh;
                var tris = mesh.GetTriangles(0);
                var vertices = mesh.vertices.Select(x => mc.transform.TransformPoint( x )).ToArray();

                for( int i = 0; i < tris.Length; i += 3 )
                {
                    surfaces.Add(new Interop.SM64Surface {
                        force = 0,
                        type = 0,
                        v0x = (short)(Interop.SCALE_FACTOR * -vertices[tris[i  ]].x),
                        v0y = (short)(Interop.SCALE_FACTOR *  vertices[tris[i  ]].y),
                        v0z = (short)(Interop.SCALE_FACTOR *  vertices[tris[i  ]].z),
                        v1x = (short)(Interop.SCALE_FACTOR * -vertices[tris[i+2]].x),
                        v1y = (short)(Interop.SCALE_FACTOR *  vertices[tris[i+2]].y),
                        v1z = (short)(Interop.SCALE_FACTOR *  vertices[tris[i+2]].z),
                        v2x = (short)(Interop.SCALE_FACTOR * -vertices[tris[i+1]].x),
                        v2y = (short)(Interop.SCALE_FACTOR *  vertices[tris[i+1]].y),
                        v2z = (short)(Interop.SCALE_FACTOR *  vertices[tris[i+1]].z)
                    });
                }
            }

            return surfaces.ToArray();
        }
    }
}