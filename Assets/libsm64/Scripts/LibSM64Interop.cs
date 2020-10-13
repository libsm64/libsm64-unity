using System;
using UnityEngine;
using System.Runtime.InteropServices;

public static class LibSM64Interop
{
    public const float SCALE_FACTOR = 100.0f;

    public const int SM64_TEXTURE_WIDTH  = 64 * 11;
    public const int SM64_TEXTURE_HEIGHT = 64;
    public const int SM64_GEO_MAX_TRIANGLES = 1024;

    [StructLayout(LayoutKind.Sequential)]
    public struct SM64Surface
    {
        public short type;
        public short force;
        public short v0x, v0y, v0z;
        public short v1x, v1y, v1z;
        public short v2x, v2y, v2z;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct SM64MarioInputs
    {
        public float camLookX, camLookZ;
        public float stickX, stickY;
        public byte buttonA, buttonB, buttonZ;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct SM64MarioState
    {
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] position;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] velocity;
        public float faceAngle;
        public short health;
    };

    [StructLayout(LayoutKind.Sequential)]
    struct SM64MarioGeometryBuffers
    {
        public IntPtr position;
        public IntPtr normal;
        public IntPtr color;
        public IntPtr uv;
        public ushort numTrianglesUsed;
    };

    [DllImport("sm64")]
    static extern void sm64_global_init( IntPtr rom, IntPtr outTexture, IntPtr debugPrintFunctionPtr );
    [DllImport("sm64")]
    static extern void sm64_load_surfaces( ushort terrainType, SM64Surface[] surfaces, ulong numSurfaces );
    [DllImport("sm64")]
    static extern void sm64_mario_reset( short marioX, short marioY, short marioZ );
    [DllImport("sm64")]
    static extern void sm64_mario_tick( ref SM64MarioInputs inputs, ref SM64MarioState outState, ref SM64MarioGeometryBuffers outBuffers );
    [DllImport("sm64")]
    static extern void sm64_global_terminate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void DebugPrintFuncDelegate(string str);

    static void debugPrintCallback(string str)
    {
        Debug.Log("libsm64: " + str);
    }

    public static void InitWithROM( byte[] rom )
    {
        var callbackDelegate = new DebugPrintFuncDelegate( debugPrintCallback );
        var romHandle = GCHandle.Alloc( rom, GCHandleType.Pinned );
        var textureData = new byte[ 4 * SM64_TEXTURE_WIDTH * SM64_TEXTURE_HEIGHT ];
        var textureDataHandle = GCHandle.Alloc( textureData, GCHandleType.Pinned );

        sm64_global_terminate();
        sm64_global_init( romHandle.AddrOfPinnedObject(), textureDataHandle.AddrOfPinnedObject(), Marshal.GetFunctionPointerForDelegate( callbackDelegate ));

        romHandle.Free();
        textureDataHandle.Free();
    }

    public static void LoadSurfaces( SM64TerrainType terrainType, SM64Surface[] surfaces )
    {
        sm64_load_surfaces( (ushort)terrainType, surfaces, (ulong)surfaces.Length );
    }

    public static void MarioReset( Vector3 marioPos )
    {
        sm64_mario_reset( (short)marioPos.x, (short)marioPos.y, (short)marioPos.z );
    }

    public static SM64MarioState MarioTick( SM64MarioInputs inputs, Vector3[] positionBuffer, Vector3[] normalBuffer, Vector3[] colorBuffer, Vector2[] uvBuffer )
    {
        SM64MarioState outState = new SM64MarioState();

        var posHandle = GCHandle.Alloc( positionBuffer, GCHandleType.Pinned );
        var normHandle = GCHandle.Alloc( normalBuffer, GCHandleType.Pinned );
        var colorHandle = GCHandle.Alloc( colorBuffer, GCHandleType.Pinned );
        var uvHandle = GCHandle.Alloc( uvBuffer, GCHandleType.Pinned );

        SM64MarioGeometryBuffers buff = new SM64MarioGeometryBuffers
        {
            position = posHandle.AddrOfPinnedObject(),
            normal = normHandle.AddrOfPinnedObject(),
            color = colorHandle.AddrOfPinnedObject(),
            uv = uvHandle.AddrOfPinnedObject()
        };

        sm64_mario_tick( ref inputs, ref outState, ref buff );

        posHandle.Free();
        normHandle.Free();
        colorHandle.Free();
        uvHandle.Free();

        return outState;
    }
}
