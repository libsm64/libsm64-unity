public enum SM64TerrainType
{
    TERRAIN_GRASS  = 0x0000,
    TERRAIN_STONE  = 0x0001,
    TERRAIN_SNOW   = 0x0002,
    TERRAIN_SAND   = 0x0003,
    TERRAIN_SPOOKY = 0x0004,
    TERRAIN_WATER  = 0x0005,
    TERRAIN_SLIDE  = 0x0006,
    TERRAIN_MASK   = 0x0007
}

public enum SM64SurfaceType
{
    SURFACE_DEFAULT                    = 0x0000,// Environment default
    SURFACE_BURNING                    = 0x0001,// Lava / Frostbite (in SL), but is used mostly for Lava
    SURFACE_0004                       = 0x0004,// Unused, has no function and has parameters
    SURFACE_HANGABLE                   = 0x0005,// Ceiling that Mario can climb on
    SURFACE_SLOW                       = 0x0009,// Slow down Mario, unused
    SURFACE_DEATH_PLANE                = 0x000A,// Death floor
    SURFACE_CLOSE_CAMERA               = 0x000B,// Close camera
    SURFACE_WATER                      = 0x000D,// Water, has no action, used on some waterboxes below
    SURFACE_FLOWING_WATER              = 0x000E,// Water (flowing), has parameters
    SURFACE_INTANGIBLE                 = 0x0012,// Intangible (Separates BBH mansion from merry-go-round, for room usage)
    SURFACE_VERY_SLIPPERY              = 0x0013,// Very slippery, mostly used for slides
    SURFACE_SLIPPERY                   = 0x0014,// Slippery
    SURFACE_NOT_SLIPPERY               = 0x0015,// Non-slippery, climbable
    SURFACE_TTM_VINES                  = 0x0016,// TTM vines, has no action defined
    SURFACE_MGR_MUSIC                  = 0x001A,// Plays the Merry go round music, see handle_merry_go_round_music in bbh_merry_go_round.inc.c for more details
    SURFACE_INSTANT_WARP_1B            = 0x001B,// Instant warp to another area, used to warp between areas in WDW and the endless stairs to warp back
    SURFACE_INSTANT_WARP_1C            = 0x001C,// Instant warp to another area, used to warp between areas in WDW
    SURFACE_INSTANT_WARP_1D            = 0x001D,// Instant warp to another area, used to warp between areas in DDD, SSL and TTM
    SURFACE_INSTANT_WARP_1E            = 0x001E,// Instant warp to another area, used to warp between areas in DDD, SSL and TTM
    SURFACE_SHALLOW_QUICKSAND          = 0x0021,// Shallow Quicksand (depth of 10 units)
    SURFACE_DEEP_QUICKSAND             = 0x0022,// Quicksand (lethal, slow, depth of 160 units)
    SURFACE_INSTANT_QUICKSAND          = 0x0023,// Quicksand (lethal, instant)
    SURFACE_DEEP_MOVING_QUICKSAND      = 0x0024,// Moving quicksand (flowing, depth of 160 units)
    SURFACE_SHALLOW_MOVING_QUICKSAND   = 0x0025,// Moving quicksand (flowing, depth of 25 units)
    SURFACE_QUICKSAND                  = 0x0026,// Moving quicksand (60 units)
    SURFACE_MOVING_QUICKSAND           = 0x0027,// Moving quicksand (flowing, depth of 60 units)
    SURFACE_WALL_MISC                  = 0x0028,// Used for some walls, Cannon to adjust the camera, and some objects like Warp Pipe
    SURFACE_NOISE_DEFAULT              = 0x0029,// Default floor with noise
    SURFACE_NOISE_SLIPPERY             = 0x002A,// Slippery floor with noise
    SURFACE_HORIZONTAL_WIND            = 0x002C,// Horizontal wind, has parameters
    SURFACE_INSTANT_MOVING_QUICKSAND   = 0x002D,// Quicksand (lethal, flowing)
    SURFACE_ICE                        = 0x002E,// Slippery Ice, in snow levels and THI's water floor
    SURFACE_LOOK_UP_WARP               = 0x002F,// Look up and warp (Wing cap entrance)
    SURFACE_HARD                       = 0x0030,// Hard floor (Always has fall damage)
    SURFACE_WARP                       = 0x0032,// Surface warp
    SURFACE_TIMER_START                = 0x0033,// Timer start (Peach's secret slide)
    SURFACE_TIMER_END                  = 0x0034,// Timer stop (Peach's secret slide)
    SURFACE_HARD_SLIPPERY              = 0x0035,// Hard and slippery (Always has fall damage)
    SURFACE_HARD_VERY_SLIPPERY         = 0x0036,// Hard and very slippery (Always has fall damage)
    SURFACE_HARD_NOT_SLIPPERY          = 0x0037,// Hard and Non-slippery (Always has fall damage)
    SURFACE_VERTICAL_WIND              = 0x0038,// Death at bottom with vertical wind
    SURFACE_BOSS_FIGHT_CAMERA          = 0x0065,// Wide camera for BOB and WF bosses
    SURFACE_CAMERA_FREE_ROAM           = 0x0066,// Free roam camera for THI and TTC
    SURFACE_THI3_WALLKICK              = 0x0068,// Surface where there's a wall kick section in THI 3rd area, has no action defined
    SURFACE_CAMERA_8_DIR               = 0x0069,// Surface that enables far camera for platforms, used in THI
    SURFACE_CAMERA_MIDDLE              = 0x006E,// Surface camera that returns to the middle, used on the 4 pillars of SSL
    SURFACE_CAMERA_ROTATE_RIGHT        = 0x006F,// Surface camera that rotates to the right (Bowser 1 & THI)
    SURFACE_CAMERA_ROTATE_LEFT         = 0x0070,// Surface camera that rotates to the left (BOB & TTM)
    SURFACE_CAMERA_BOUNDARY            = 0x0072,// Intangible Area, only used to restrict camera movement
    SURFACE_NOISE_VERY_SLIPPERY_73     = 0x0073,// Very slippery floor with noise, unused
    SURFACE_NOISE_VERY_SLIPPERY_74     = 0x0074,// Very slippery floor with noise, unused
    SURFACE_NOISE_VERY_SLIPPERY        = 0x0075,// Very slippery floor with noise, used in CCM
    SURFACE_NO_CAM_COLLISION           = 0x0076,// Surface with no cam collision flag
    SURFACE_NO_CAM_COLLISION_77        = 0x0077,// Surface with no cam collision flag, unused
    SURFACE_NO_CAM_COL_VERY_SLIPPERY   = 0x0078,// Surface with no cam collision flag, very slippery with noise (THI)
    SURFACE_NO_CAM_COL_SLIPPERY        = 0x0079,// Surface with no cam collision flag, slippery with noise (CCM, PSS and TTM slides)
    SURFACE_SWITCH                     = 0x007A,// Surface with no cam collision flag, non-slippery with noise, used by switches and Dorrie
    SURFACE_VANISH_CAP_WALLS           = 0x007B // Vanish cap walls, pass through them with Vanish Cap
}