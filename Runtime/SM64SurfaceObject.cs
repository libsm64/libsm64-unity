using UnityEngine;
using System.Linq;

namespace LibSM64
{
    public class SM64SurfaceObject : MonoBehaviour
    {
        uint _surfaceObjectId;

        void Start()
        {
            SM64Context.RegisterSurfaceObject( this );

            var mc = GetComponent<MeshCollider>();
            var surfaces = Utils.GetSurfacesForMesh( transform.lossyScale, mc.sharedMesh );
            _surfaceObjectId = Interop.LoadSurfaceObject( transform, surfaces.ToArray() );
        }

        public void contextFixedUpdate()
        {
            Interop.MoveObject( _surfaceObjectId, transform );
        }

        void OnDisable()
        {
            SM64Context.UnregisterSurfaceObject( this );

            Interop.UnloadObject( _surfaceObjectId );
        }
    }
}