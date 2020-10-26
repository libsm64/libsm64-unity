using UnityEngine;
using System.Linq;

namespace LibSM64
{
    public class SM64SurfaceObject : MonoBehaviour
    {
        Vector3 _lastPosition;
        Quaternion _lastRotation;
        Vector3 _position;
        Quaternion _rotation;
        uint _surfaceObjectId;

        void Start()
        {
            SM64Context.RegisterSurfaceObject( this );

            _position = transform.position;
            _rotation = transform.rotation;
            _lastPosition = _position;
            _lastRotation = _rotation;

            var mc = GetComponent<MeshCollider>();
            var surfaces = Utils.GetSurfacesForMesh( transform.lossyScale, mc.sharedMesh );
            _surfaceObjectId = Interop.LoadSurfaceObject( _position, _rotation, surfaces.ToArray() );
        }

        int steps = 0;

        internal void contextFixedUpdate()
        {
            _lastPosition = _position;
            _lastRotation = _rotation;

            _position += Vector3.right * 0.1f * Mathf.Sin( 0.1f * (float)(steps++) );

            Interop.MoveObject( _surfaceObjectId, _position, _rotation );
        }

        internal void contextUpdate()
        {
            float t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;

            transform.position = Vector3.LerpUnclamped( _lastPosition, _position, t );
            transform.rotation = Quaternion.SlerpUnclamped( _lastRotation, _rotation, t );
        }

        void OnDisable()
        {
            SM64Context.UnregisterSurfaceObject( this );

            Interop.UnloadObject( _surfaceObjectId );
        }
    }
}