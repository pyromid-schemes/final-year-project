var DOOR_UP = { x: 0, y: -80 };
var DOOR_RIGHT = { x: 80, y: 0 };
var DOOR_DOWN = { x: 0, y: 80 };
var DOOR_LEFT = { x: -80, y: 0 };

var DOORS = [DOOR_UP, DOOR_RIGHT, DOOR_DOWN, DOOR_LEFT];

var SCALE_THIRD = 1/3;
var CENTER_480 = { x: 240, y: 240 };
var SCALED_480 = { w: 160, h: 160, cx: 80, cy: 80 };

var Rooms = {
    room1: generate_room(1, [DOOR_UP]),
    room2: generate_room(2, [DOOR_UP, DOOR_RIGHT, DOOR_DOWN, DOOR_LEFT]),
    room3: generate_room(3, [DOOR_UP, DOOR_DOWN]),
    room4: generate_room(4, [DOOR_UP, DOOR_RIGHT, DOOR_DOWN]),
    room5: generate_room(5, [DOOR_UP, DOOR_RIGHT]),
    room6: generate_room(6, [DOOR_UP])
};

function generate_room(room_id, doors){
    var room_key = 'room' + room_id;
    return {
        room_id: room_key,
        w: 480,
        h: 480,
        scale: SCALE_THIRD,
        center: CENTER_480,
        scaled: SCALED_480,
        assets: {
            normal: { key: room_key, path: 'assets/rooms/' + room_key + '/normal.png' },
            green: { key: room_key + '-green', path: 'assets/rooms/' + room_key +'/green.png' },
            red: { key:  room_key + '-red', path: 'assets/rooms/' + room_key + '/red.png' }
        },
        door_positions: doors
    }
}