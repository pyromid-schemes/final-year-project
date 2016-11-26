
var Map = {
    // A parent object of the game
    game: null,

    map: null,
    mapDimensions: null,
    mapData: [],
    mapRooms: [],
    roomStructures: null,

    mapWidth: 32,
    mapHeight: 28,

    tileSize: 16,

    mapOffsetX: 16,
    mapOffsetY: 20,

    empty_tile: null,

    tilemaps: {},

    mapScrollOffsetX: 0,
    mapScrollOffsetY: 0,

    dragCurrentX: 0,
    dragCurrentY: 0,
    isDragging: false,


    /* Utils */
    black_bg: null,


    create: function(GameObject) {
        //Reference the main game object
        this.game = GameObject;

        // Set the background colour for the section
        this.game.graphics.beginFill(0xb5e3ea, 1);
        this.game.graphics.drawRect(this.game.mapView.x, this.game.mapView.y, this.game.mapView.w, this.game.mapView.h);
        this.game.graphics.endFill();

        // Set the empty tile sprite - the lonely initialized sprite
        this.empty_tile = game.phaser.make.sprite(0, 0, 'tile-empty-16');

        // 
        this.setupRooms();
        this.createMap();
        this.createTilemap('tilemap-chengy');
        this.createTilemap('tilemap-lava');
    },

    createMap: function(){

        // Setup the background blackness
        var map_world_width = this.mapWidth * this.tileSize;
        var map_world_height = this.mapHeight * this.tileSize;
        var map_bg_black = game.phaser.make.bitmapData(map_world_width, map_world_height);
        map_bg_black.rect(0, 0, map_world_width, map_world_height, '#000');

        this.mapDimensions = {x: this.mapOffsetX, y: this.mapOffsetY, w: map_world_width, h: map_world_height};

        // Setup the background outline
        var map_bg_width = map_world_width + 2;
        var map_bg_height = map_world_height + 2;
        var map_bg_red_border = game.phaser.make.bitmapData(map_bg_width, map_bg_height);
        map_bg_red_border.rect(0, 0, map_bg_width, map_bg_height, '#f00');

        // Add the coloured rects to the world
        game.phaser.add.sprite(this.mapOffsetX - 1, this.mapOffsetY - 1, map_bg_red_border);
        game.phaser.add.sprite(this.mapOffsetX, this.mapOffsetY, map_bg_black);



        // Create the map of tiles
        this.map = game.phaser.add.renderTexture(map_bg_width - 2, map_bg_height - 2, 'map');
        this.mapData = [];
        this.game.phaser.add.sprite(this.mapOffsetX, this.mapOffsetY, this.map);

        this.renderEmptyTiles();
    },

    renderEmptyTiles: function() {
        // Clear the drawn rooms
        this.map.clear();

        // Get the map offset (mod tileSize)
        var offset = this.getOffsetXY();

        // Draw a grid of empty tiles
        for (var x = -1; x < this.mapWidth + 1; x++) {
            for (var y = -1; y < this.mapHeight + 1; y++) {
                this.map.renderRawXY(this.empty_tile, offset.x + x * this.tileSize, offset.y + y * this.tileSize);
                // this.mapData[x + y * this.mapWidth] = '.'; // I think mapData is irrelevant?
            }
        }
    },

    renderAllRooms: function(){
        // Render rooms now
        for(var i = 0; i < this.mapRooms.length; i++){
            var r = this.mapRooms[i];

            var render_x = r.x2 * this.tileSize + this.mapScrollOffsetX;
            var render_y = r.y2 * this.tileSize + this.mapScrollOffsetY;

            this.renderRoomXY(r.room_id, render_x, render_y);
        }
    },

    resetMap: function(){
    },

    redrawEverything: function(){
        this.renderEmptyTiles();
        this.renderAllRooms();
    },

    setupRooms: function(){
        this.roomStructures = {};

        this.roomStructures['room-1'] = {dimensions: chengy_room, tilemap: 'tilemap-chengy' };
        this.roomStructures['room-2'] = {dimensions: chengy_room4doors, tilemap: 'tilemap-chengy'};
        this.roomStructures['room-3'] = {dimensions: chengy_room_5x3, tilemap: 'tilemap-lava'};
        this.roomStructures['room-4'] = {dimensions: chengy_room_door_up, tilemap: 'tilemap-chengy'};
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
        var pointer = this.game.phaser.input.activePointer;

        var tile_x = Math.floor((pointer.x - this.mapOffsetX - 1 - this.mapScrollOffsetX) / this.tileSize); // -1 on the x because phaser coordinate system is messed up
        var tile_y = Math.floor((pointer.y - this.mapOffsetY - 2 - this.mapScrollOffsetY) / this.tileSize); // -2 on the y because phaser coordinate system is messed up


        var mouse_x = pointer.x - 1;
        var mouse_y = pointer.y - 2;
        var is_mouse_inside_map = (mouse_x >= this.mapOffsetX && mouse_y >= this.mapOffsetY && mouse_x < this.mapOffsetX + this.mapDimensions.w && mouse_y < this.mapOffsetY + this.mapDimensions.h);

        // If the mouse is inside the map when the user clicks?
        if(is_mouse_inside_map){
            // If there is a room-tile selected inside the [game.builderObject]
            if(this.game.builderObject.which_tile_to_place != -1){
                var btn = this.game.builderObject.buttons[this.game.builderObject.which_tile_to_place];

                if(this.canPlaceRoom(btn.room_id, tile_x, tile_y)){
                    this.placeRoom(btn.room_id, tile_x, tile_y);
                }
            }else {
                // Can only navigate the map if you're not placing objects
                this.isDragging = true;

                // Set the current drag position to the mouse position
                this.dragCurrentX = pointer.x;
                this.dragCurrentY = pointer.y;
            }
        }



        var relative_click_x = (pointer.x - this.mapOffsetX - 1);
        var relative_click_y = (pointer.y - this.mapOffsetY - 2);

        var relative_tile_x = Math.floor(relative_click_x / this.tileSize);
        var relative_tile_y = Math.floor(relative_click_y / this.tileSize);

        // console.log("Relative coords ["+relative_click_x+","+relative_click_y+"]");
        // console.log("Relative tile xy ["+relative_tile_x+","+relative_tile_y+"]");

        // console.log(game.phaser.input.activePointer);
        // if(game.phaser.input.mouse.button == Phaser.Mouse.RIGHT_BUTTON){
        //     console.log("Right click");
        // }
    },

    onUp: function(e){

        if(this.isDragging) {
            this.isDragging = false;
        }
    },

    onMove: function(e){

        if(this.isDragging) {
            var pointer = this.game.phaser.input.activePointer;

            var difference_x = this.dragCurrentX - pointer.x;
            var difference_y = this.dragCurrentY - pointer.y;

            this.dragCurrentX = pointer.x;
            this.dragCurrentY = pointer.y;

            this.mapScrollOffsetX -= difference_x;
            this.mapScrollOffsetY -= difference_y;

            this.redrawEverything();
        }
    },


    /*
     * Room placing functions
     */

    canPlaceRoom: function(room_id, x, y){
        var room = this.roomStructures[room_id];
        var bounding_box = this.getBoundingBoxForRoom(x, y, room);

        for(var i = 0; i < this.mapRooms.length; i++){
            var r = this.mapRooms[i];

            if(this.doesBoundingBoxesCollide(bounding_box, r.bb)){
                //ToDo: "per-tile" collision, so for odd shaped rooms which don't occupy all of a square they can not collide if their bounding boxes do
                return false;
            }
        }
        return true;
    },

    placeRoom: function(room_id, x, y){
        // Get the room data from the roomStructures array
        var room = this.roomStructures[room_id];
        // Basic way of getting the next index
        var map_room_id = this.mapRooms.length;

        // Get the bounding box from the current x/y and room data
        var bounding_box = this.getBoundingBoxForRoom(x, y, room);

        // Add the room into the world room array
        this.mapRooms[map_room_id] = {room_id: room_id,
            x: x - room.dimensions.center.x,
            y: y - room.dimensions.center.y,
            x2: x,
            y2: y,
            bb: bounding_box,
            room: room};

        // Broadcast a message to the VR client
        this.game.sendMessage(room_id, x - room.dimensions.center.x, y - room.dimensions.center.y);

        // Repaint the world with the new room
        this.redrawEverything();
    },

    // So this function just blits out a room at a specific location...
    renderRoomXY: function(room_id, x, y){
        var room = this.roomStructures[room_id];
        var map_room_id = this.mapRooms.length;

        for(var room_x = 0; room_x < room.dimensions.w; room_x++){
            for(var room_y = 0; room_y < room.dimensions.h; room_y++){
                // Get the room's offset from the center (from each tile's perspective)
                var offset_x = room_x - room.dimensions.center.x;
                var offset_y = room_y - room.dimensions.center.y;

                // Get the relative coordinates
                var relative_x = x + offset_x * this.tileSize;
                var relative_y = y + offset_y * this.tileSize;

                // Get the tile data for the room
                var tile_data = room.dimensions.data[room_y][room_x];

                // If the tile in the position is not empty then the room does not fit
                // this.mapData[relative_x + relative_y * this.mapWidth] = map_room_id; //mapData is irrelevant??

                var tile_position = "";

                // This is... some piece of code to draw the correct tiles for the room... surprised it actually somewhat works.
                if(tile_data == '1') {
                    tile_position += (room_y == 0 ? 't' : (room_y == room.dimensions.h - 1 ? 'b' : 'm'));
                    tile_position += (room_x == 0 ? 'l' : (room_x == room.dimensions.w - 1 ? 'r' : 'm'));
                }else if(tile_data == '2'){
                    tile_position = 'mm';
                }

                // Render the specific tile piece from the tilemap at the correct position
                this.map.renderRawXY(this.tilemaps[room.tilemap][tile_position], relative_x , relative_y );
            }
        }
    },

    // This gets the current map scroll offset mod the tileSize - so it only returns from [0 up to tileSize]
    getOffsetXY: function(){
        return {
            x: this.mapScrollOffsetX % this.tileSize,
            y: this.mapScrollOffsetY % this.tileSize
        };
    },

    // Do two bounding boxes collide?
    doesBoundingBoxesCollide: function(bb1, bb2){
        return !((bb2.x1 > bb1.x2) || (bb2.x2 < bb1.x1) || (bb2.y1 > bb1.y2) || (bb2.y2 < bb1.y1));
    },

    // Returns a bounding box for a room at a specific location
    getBoundingBoxForRoom: function(x, y, room){
        var top_left_x = x - room.dimensions.center.x;
        var top_left_y = y - room.dimensions.center.y;

        var bottom_right_x = top_left_x + room.dimensions.w - 1;
        var bottom_right_y = top_left_y + room.dimensions.h - 1;

        return {
            x1: top_left_x,
            y1: top_left_y,
            x2: bottom_right_x,
            y2: bottom_right_y
        };
    },

    return: {
        onDown: this.onDown
    }
};