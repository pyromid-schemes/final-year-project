
var Map = {
    // A parent object of the game
    game: null,

    map: null,
    mapData: [],
    mapRooms: null,
    roomStructures: null,

    mapWidth: 32,
    mapHeight: 28,

    tileSize: 16,

    mapOffsetX: 16,
    mapOffsetY: 20,

    empty_tile: null,

    tilemaps: null,




    create: function(GameObject) {
        this.game = GameObject;

        this.game.graphics.beginFill(0xb5e3ea, 1);
        this.game.graphics.drawRect(this.game.mapView.x, this.game.mapView.y, this.game.mapView.w, this.game.mapView.h);
        this.game.graphics.endFill();

        this.empty_tile = game.phaser.make.sprite(0, 0, 'tile-empty-16');

        this.mapRooms = [];

        this.createRooms();
        this.createMap();


        this.tilemaps = {};
        this.createTilemap('tilemap-chengy');
    },

    createMap: function(){
        // Setup the background outline
        var map_bg_width = this.mapWidth * this.tileSize + 2;
        var map_bg_height = this.mapHeight * this.tileSize + 2;
        var mapBG_temp = game.phaser.make.bitmapData(map_bg_width, map_bg_height);

        mapBG_temp.rect(0, 0, map_bg_width, map_bg_height, '#f00');
        var mapBG = game.phaser.add.sprite(this.mapOffsetX - 1, this.mapOffsetY - 1, mapBG_temp);

        // Create the map of tiles
        this.map = game.phaser.add.renderTexture(800, 600, 'map');
        this.mapData = [];
        this.game.phaser.add.sprite(this.mapOffsetX, this.mapOffsetY, this.map);

        this.renderEmptyTiles();
    },

    renderEmptyTiles: function(){
        for(var x=0; x<this.mapWidth; x++) {
            for (var y = 0; y < this.mapHeight; y++) {
                this.map.renderRawXY(this.empty_tile, x * this.tileSize, y * this.tileSize);
                this.mapData[x + y * this.mapWidth] = '.';
            }
        }
    },

    resetMap: function(){

    },

    redrawEverything: function(){

    },

    createRooms: function(){
        this.roomStructures = {};

        this.roomStructures['room-1'] = {dimensions: chengy_room, tilemap: 'tilemap-chengy'};
        this.roomStructures['room-2'] = {dimensions: chengy_room4doors, tilemap: 'tilemap-chengy'};

        //map_rooms[0] = {room_id: 'room-1', x: 1, y: 1}
        //map_rooms[1] = {room_id: 'room-1', x: 4, y: 1}
    },

    createTilemap: function(tilemap_key){
        this.tilemaps[tilemap_key] = {};
        var tilemap_positions = ['tl', 'tm', 'tr', 'mr', 'br', 'bm', 'bl', 'ml', 'mm'];

        for(var i = 0; i < tilemap_positions.length; i++) {
            this.tilemaps[tilemap_key][tilemap_positions[i]] = game.phaser.make.sprite(0, 0, tilemap_key);
            this.tilemaps[tilemap_key][tilemap_positions[i]].frameName = tilemap_positions[i] + '.png';
        }
    },

    onDown: function(e) {
        var pointer = game.phaser.input.activePointer;

        var tile_x = Math.floor((pointer.x - this.mapOffsetX - 1) / this.tileSize); // -1 on the x because phaser coordinate system is messed up
        var tile_y = Math.floor((pointer.y - this.mapOffsetY - 2) / this.tileSize); // -2 on the y because phaser coordinate system is messed up

        if (tile_x >= 0 && tile_y >= 0 && tile_x < this.mapWidth && tile_y < this.mapHeight) {


            if(this.game.builderObject.which_tile_to_place != -1){
                var btn = this.game.buttons[this.game.builderObject.which_tile_to_place];


                if(this.canPlaceRoom(btn.room_id, tile_x, tile_y)){
                    this.placeRoom(btn.room_id, tile_x, tile_y);
                }
            }
        }
    },

    onMove: function(e){

    },


    /*
     * Room placing functions
     */

    canPlaceRoom: function(room_id, x, y){
        var room = this.roomStructures[room_id];

        if(x >= room.dimensions.center.x && y >= room.dimensions.center.y &&
            x <= (this.mapWidth - (room.dimensions.w - room.dimensions.center.x)) && y <= (this.mapHeight - (room.dimensions.h - room.dimensions.center.y))){
            for(var room_x = 0; room_x < room.dimensions.w; room_x++){
                for(var room_y = 0; room_y < room.dimensions.h; room_y++){
                    // Get the room's offset from the center (from each tile's perspective)
                    var offset_x = room_x - room.dimensions.center.x;
                    var offset_y = room_y - room.dimensions.center.y;

                    // Get the relative coordinates
                    var relative_x = x + offset_x;
                    var relative_y = y + offset_y;

                    // If the tile in the position is not empty then the room does not fit
                    if(this.mapData[relative_x + relative_y * this.mapWidth] != '.'){
                        return false;
                    }
                }
            }

            // If the room's tiles are all unoccupied then it can fit
            return true;
        }
    },

    placeRoom: function(room_id, x, y){
        var room = this.roomStructures[room_id];
        var map_room_id = this.mapRooms.length;

        this.mapRooms[map_room_id] = {room_id: room_id, x: x - room.dimensions.center.x, y: y - room.dimensions.center.y};

        console.log("sending data:");
        console.log(this.mapRooms[map_room_id]);

        this.game.sendMessage(room_id, x - room.dimensions.center.x, y - room.dimensions.center.y);

        for(var room_x = 0; room_x < room.dimensions.w; room_x++){
            for(var room_y = 0; room_y < room.dimensions.h; room_y++){
                // Get the room's offset from the center (from each tile's perspective)
                var offset_x = room_x - room.dimensions.center.x;
                var offset_y = room_y - room.dimensions.center.y;

                // Get the relative coordinates
                var relative_x = x + offset_x;
                var relative_y = y + offset_y;


                var tile_data = room.dimensions.data[room_y][room_x];
                // console.log('['+room_x+','+room_y+'] ['+tile_data+']');

                // If the tile in the position is not empty then the room does not fit
                this.mapData[relative_x + relative_y * this.mapWidth] = map_room_id;

                var tile_position = "";

                if(tile_data == '1') {
                    tile_position += (room_y == 0 ? 't' : (room_y == room.dimensions.h - 1 ? 'b' : 'm'));
                    tile_position += (room_x == 0 ? 'l' : (room_x == room.dimensions.w - 1 ? 'r' : 'm'));
                }else if(tile_data == '2'){
                    tile_position = 'mm';
                }

                this.map.renderRawXY(this.tilemaps[room.tilemap][tile_position], relative_x * this.tileSize, relative_y * this.tileSize);
            }
        }
    },


    return: {
        onDown: onDown
    }

};

window.reset = Map.resetMap;

// function refreshMap(){
//     for(var xy = 0; xy < mapWidth * mapHeight; xy++){
//         var x = (xy % mapWidth);
//         var y = Math.floor((xy / mapWidth));
//         var tile_str = findTileStrFromKey(mapData[xy]);
//         updateMapTile(x, y, tile_str);
//     }
// }

// // Update the view with
// function updateMap(x, y, tile_str){
//     var tile_data = hashmap[tile_str];
//
//     // DEBUG --- THIS IS NOT PRODUCTION CODE
//     if(tile_data.key == '.') {
//         map.renderRawXY(tiles_16_hash['empty'].tile, x * tileSize, y * tileSize);
//     }else if(tile_data.key == '1') {
//         if (checkRoomCanFit(x, y, green_4by3_room)) {
//             placeRoom(x, y, green_4by3_room, '1', 'green-16');
//         }
//     }else if(tile_data.key == '2') {
//         if (checkRoomCanFit(x, y, purple_3by3_room)) {
//             placeRoom(x, y, purple_3by3_room, '2', 'purple-16');
//         }
//     }else if(tile_data.key == '3'){
//         if(checkRoomCanFit(x, y, yellow_3by5_room)){
//             placeRoom(x, y, yellow_3by5_room, '3', 'yellow-16');
//         }
//     }else{
//         map.renderRawXY(tile_data.tile, x * tileSize, y * tileSize);
//         mapData[x + y * mapWidth] = tile_data.key;
//     }
// }
// function updateMapTile(x, y, tile_str){
//     var tile_data = hashmap[tile_str];
//     mapData[x + y * mapWidth] = tile_data.key;
//     map.renderRawXY(tile_data.tile, x * tileSize, y * tileSize);
//     // map.renderRawXY(tile_data.tile, x * tileSize, y * tileSize);
// }


/**
 * Debug methods
 * **/

// Save the mapData and output it
function save(){
    var data = {w: mapWidth, h: mapHeight};
    data['map'] = mapData;

    console.log('[save]');
    console.log(JSON.stringify(data));
}
// Load a map
function load(map_data){
    mapWidth = map_data.w;
    mapHeight = map_data.h;

    mapData = map_data.map;
    // refreshMap(); //Refresh the map by drawing all the things
}
window.save = save;
window.load = load;


function print_map_data(){
    console.log("map_data:");
    console.log(JSON.stringify(Map.mapData));
}
window.map_print = print_map_data;

window.map = Map;

function pretty_print(){
    var log_counter = 0;
    for(var y=0; y<Map.mapHeight; y++){
        var str = "";
        for (var x=0; x<Map.mapWidth; x++){
            var md = Map.mapData[x + y * Map.mapWidth];

            if(md == '.'){ str += '..'; }
            if(md >= 0 && md <= 9) str += '0' + md;
            else if(md > 9) str += md; 
        }
        log_counter++;
        console.log(str + (log_counter % 2 ? '\u200B' : ''));
    }
}
window.pretty_print = pretty_print;