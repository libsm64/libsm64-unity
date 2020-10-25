using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LibSM64
{
    internal class SM64Context
    {
        static SM64Context s_instance;

        static public SM64Context Instance
        {
            get
            {
                if( s_instance == null )
                {
                    var contextGo = new GameObject( "SM64_CONTEXT" );
                    s_instance = new SM64Context( contextGo.AddComponent<SM64ContextObject>() );
                }
                return s_instance;
            }
        }

        SM64ContextObject _contextObject;
        List<SM64Mario> _marios = new List<SM64Mario>();
        List<SM64SurfaceObject> _surfaceObjects = new List<SM64SurfaceObject>();
        bool _registeredMeshColliders = false;

        SM64Context( SM64ContextObject contextObject )
        {
            _contextObject = contextObject;
            _contextObject.BindUpdateListeners( onUpdate, onFixedUpdate );

            Interop.InitWithROM( File.ReadAllBytes( Application.dataPath + "/../baserom.us.z64" ));

            RegisterStaticMeshColliders();
        }

        void onFixedUpdate()
        {
            foreach( var o in _surfaceObjects )
                o.contextFixedUpdate();

            foreach( var o in _marios )
                o.contextFixedUpdate();
        }

        void onUpdate()
        {
            foreach( var o in _marios )
                o.contextUpdate();
        }

        public void RegisterStaticMeshColliders()
        {
            var surfaces = new List<Interop.SM64Surface>();
            var meshColliders = GameObject.FindObjectsOfType<MeshCollider>();

            foreach( var mc in meshColliders )
            {
                if( mc.GetComponent<SM64SurfaceObject>() != null )
                    continue;

                var mesh = mc.sharedMesh;
                var tris = mesh.GetTriangles(0);

                var vertices = mesh.vertices.Select(x => {
                    return mc.transform.TransformPoint( x );
                }).ToArray();

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

            Interop.LoadSurfaces( SM64TerrainType.TERRAIN_STONE, surfaces.ToArray() );

            _registeredMeshColliders = true;
        }

        public void RegisterMario( SM64Mario mario )
        {
            if( !_registeredMeshColliders )
                RegisterStaticMeshColliders();

            if( !_marios.Contains( mario ))
            {
                _marios.Add( mario );

                var pos = mario.transform.position;
                Interop.MarioReset( new Vector3( -pos.x, pos.y, pos.z ) * Interop.SCALE_FACTOR );
            }
        }

        public void UnregisterMario( SM64Mario mario )
        {
            if( _marios.Contains( mario ))
                _marios.Remove( mario );
        }

        public void RegisterSurfaceObject( SM64SurfaceObject surfaceObject )
        {
            if( !_surfaceObjects.Contains( surfaceObject ))
            {
                _surfaceObjects.Add( surfaceObject );
            }
        }

        public void UnregisterSurfaceObject( SM64SurfaceObject surfaceObject )
        {
            if( _surfaceObjects.Contains( surfaceObject ))
                _surfaceObjects.Remove( surfaceObject );
        }
    }
}