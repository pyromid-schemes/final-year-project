var Rooms = {
    chengy_room: {
        room_id: 'room1',
        w: 4,
        h: 4,
        center: {x: 1, y: 1},
        data: [
            [1, 1, 1, 1],
            [1, 1, 1, 1],
            [1, 1, 1, 1],
            [1, 1, 1, 1]
        ],
        data2: [
            ['tl', 'tm', 'tm', 'tr'],
            ['ml', 'mm', 'mm', 'mr'],
            ['ml', 'mm', 'mm', 'mr'],
            ['bl', 'bm', 'bm', 'br']
        ],
        tilemap: Tilemaps.chengy,
        builderButton: {
            image: 'assets/buttons/button4.png',
            key: 'builder-button-chengy'
        }
    },
    chengy_room_5x3: {
        room_id: 'room3',
        w: 5,
        h: 3,
        center: {x: 2, y: 1},
        data: [
            [1, 1, 1, 1, 1],
            [2, 1, 1, 1, 2],
            [1, 1, 1, 1, 1]
        ],
        data2: [
            ['tl', 'tm', 'tm', 'tm', 'tr'],
            ['mm', 'mm', 'mm', 'mm', 'mm'],
            ['bl', 'bm', 'bm', 'bm', 'br']
        ],
        tilemap: Tilemaps.lava,
        builderButton: {
            image: 'assets/buttons/lava-5x3.png',
            key: 'builder-button-lava5x3'
        }
    },
    chengy_room4doors: {
        room_id: 'room2',
        w: 4,
        h: 4,
        center: {x: 1, y: 1},
        data: [
            [1, 2, 2, 1],
            [2, 1, 1, 2],
            [2, 1, 1, 2],
            [1, 2, 2, 1]
        ],
        data2: [
            ['tl', 'mm', 'mm', 'tr'],
            ['mm', 'mm', 'mm', 'mm'],
            ['mm', 'mm', 'mm', 'mm'],
            ['bl', 'mm', 'mm', 'br']
        ],
        tilemap: Tilemaps.chengy,
        builderButton: {
            image: 'assets/buttons/chengy-room-4doors.png',
            key: 'builder-button-chengy4doors'
        }
    },
    chengy_room_door_up: {
        room_id: 'room4',
        w: 4,
        h: 4,
        center: {x: 1, y: 1},
        data: [
            [1, 2, 2, 1],
            [1, 1, 1, 1],
            [1, 1, 1, 1],
            [1, 1, 1, 1]
        ],
        data2: [
            ['tl', 'mm', 'mm', 'tr'],
            ['ml', 'mm', 'mm', 'mr'],
            ['ml', 'mm', 'mm', 'mr'],
            ['bl', 'bm', 'bm', 'br']
        ],
        tilemap: Tilemaps.chengy,
        builderButton: {
            image: 'assets/buttons/chengy-room-door-up.png',
            key: 'builder-button-chengy-door-up'
        }
    },
    l_shape_room: {
        room_id: 'room5',
        w: 4,
        h: 4,
        center: {x: 1, y: 1},
        data: [
            [1, 1, 1, 1],
            [1, 1, 1, 2],
            [1, 1, 0, 0],
            [1, 2, 0, 0]
        ],
        data2: [
            ['tl', 'tm', 'tm', 'tr'],
            ['ml', 'mm', 'bm', 'bm'],
            ['ml', 'mr', '', ''],
            ['bl', 'mr', '', '']
        ],
        tilemap: Tilemaps.chengy,
        builderButton: {
            image: 'assets/buttons/l-room.png',
            key: 'builder-button-l-room'
        }
    },
    room_6by6: {
        w: 6,
        h: 6,
        center: {x: 2, y: 2},
        data: [
            [1, 1, 1, 1, 1, 1],
            [1, 1, 1, 1, 1, 1],
            [1, 1, 1, 1, 1, 1],
            [1, 1, 1, 1, 1, 1],
            [1, 1, 1, 1, 1, 1],
            [1, 1, 1, 1, 1, 1]
        ]
    }
};