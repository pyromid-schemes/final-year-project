var DOOR_UP = { x: 0, y: -32 };   // DOOR UP
var DOOR_RIGHT = { x: 32, y: 0 }; // DOOR RIGHT
var DOOR_DOWN = { x: 0, y: 32 };  // DOOR DOWN
var DOOR_LEFT = { x: -32, y: 0 }; // DOOR LEFT

var DOORS = [
    DOOR_UP,
    DOOR_RIGHT,
    DOOR_DOWN,
    DOOR_LEFT
];

var Rooms = {
    room1: {
        room_id: 'room1',
        w: 128,
        h: 128,
        center: { x: 64, y: 64 },
        scaled: { w: 64, h: 64, cx: 32, cy: 32 },
        assets: {
            normal: { key: 'room1', path: 'assets/rooms/room1/normal.png' },
            // normal: { key: 'room1', path: 'assets/rooms/room1/normal3.abc' },
            green: { key: 'room1-green', path: 'assets/rooms/room1/green.png' },
            red: { key: 'room1-red', path: 'assets/rooms/room1/red.png' }
        },
        door_positions: [
            DOOR_UP
        ]
    },
    room2: {
        room_id: 'room2',
        w: 128,
        h: 128,
        center: { x: 64, y: 64 },
        scaled: { w: 64, h: 64, cx: 32, cy: 32},
        tile_data: [[1,1,1,1], [1,1,1,1], [1,1,1,1], [1,1,1,1]],
        assets: {
            normal: { key: 'room2', path: 'assets/rooms/room2/normal.png' },
            green: { key: 'room2-green', path: 'assets/rooms/room2/green.png' },
            red: { key: 'room2-red', path: 'assets/rooms/room2/red.png' }
        },
        door_positions: [
            DOOR_UP,
            DOOR_RIGHT,
            DOOR_DOWN,
            DOOR_LEFT
        ]
    },
    room3: {
        room_id: 'room3',
        w: 128,
        h: 128,
        center: { x: 64, y: 64 },
        scaled: { w: 64, h: 64, cx: 32, cy: 32},
        tile_data: [[1,1,1,1], [1,1,1,1], [1,1,1,1], [1,1,1,1]],
        assets: {
            normal: { key: 'room3', path: 'assets/rooms/room3/normal.png' },
            green: { key: 'room3-green', path: 'assets/rooms/room3/green.png' },
            red: { key: 'room3-red', path: 'assets/rooms/room3/red.png' }
        },
        door_positions: [
            DOOR_UP,
            DOOR_RIGHT
        ]
    },
    room1_new: {
        room_id: 'room1new',
        w: 480,
        h: 480,
        center: { x: 240, y: 240 },
        scaled: { w: 240, h: 240, cx: 120, cy: 120},
        assets: {
            normal: { key: 'room3', path: 'assets/rooms/room3/normal.png' },
            green: { key: 'room3-green', path: 'assets/rooms/room3/green.png' },
            red: { key: 'room3-red', path: 'assets/rooms/room3/red.png' }
        },
        door_positions: [
            DOOR_UP,
            DOOR_RIGHT
        ]
    }
};