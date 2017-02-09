var Rooms = {
    room1: {
        room_id: 'room1',
        w: 128,
        h: 128,
        center: {
            x: 64,
            y: 64
        },

        scaled: {
            w: 64,
            h: 64,
            cx: 32,
            cy: 32
        },

        tile_data: [
            [1,1,1,1],
            [1,1,1,1],
            [1,1,1,1],
            [1,1,1,1]
        ],
        collision: {
            low: {x: -2, y: -2},
            high: {x: 2, y: 2}
        },

        assets: {
            normal: {
                key: 'room1',
                path: 'assets/rooms/room1/normal.png'
            },
            green: {
                key: 'room1-green',
                path: 'assets/rooms/room1/green.png',
            },
            red: {
                key: 'room1-red',
                path: 'assets/rooms/room1/red.png'
            }
        }
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
        }
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
        }
    }
};