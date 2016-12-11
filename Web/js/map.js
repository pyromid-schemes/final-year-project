
var Map = {
    // A parent object of the game
    game: null,

    // This is the texture which all the rooms draw onto
    map: null,
    // The X/Y W/H of the map (excluding the border)
    mapDimensions: null,
    // Holds all the actual rooms in the world [room-id, x, y, bounding-box]
    mapRooms: [],
    // Holds all the types of rooms available
    roomTypes: {},
    // This contains all the rooms built from single tiles
    // - so that each room only has to make 1 draw call instead of [w * h] tile draw calls
    roomRendersCache: {},
    // Every tileMap type
    tileMapTypes: {},

    // The empty tile texture for the map background
    emptyTile: null,
    // The background texture made from drawing many emptyTiles onto it
    emptyTileBackground: null,

    // How much the world has been scrolled
    mapScrollOffsetX: 0,
    mapScrollOffsetY: 0,

    // Used for calculating the mapScrollOffsetX/Y when dragging the map
    dragCurrentX: 0,
    dragCurrentY: 0,
    isDragging: false,

    // Used to draw a ghostRoom at a location
    ghostRoomX: -1,
    ghostRoomY: -1,


    /** Player data **/
    player: null,

    /** Mob debug data **/
    mobTypes: {},
    mapMobs: [],

    debug_mob_ant: null,


    /**
     * Constants
     * **/

    // How many tiles to show on the width and height
    MAP_WIDTH: 33,
    MAP_HEIGHT: 30,

    // Each world tile is 16px x 16px
    TILE_SIZE: 16,

    // The offset to draw the map by
    MAP_DRAW_OFFSET_X: 10,
    MAP_DRAW_OFFSET_Y: 10,


    create: function(GameObject) {
        //Reference the main game object
        this.game = GameObject;

        // Set the background colour for the section
        this.game.graphics.beginFill(0xb5e3ea, 1);
        this.game.graphics.drawRect(this.game.mapView.x, this.game.mapView.y, this.game.mapView.w, this.game.mapView.h);
        this.game.graphics.endFill();

        
        // Setup the world map
        this.setupMap();

        // Create the empty tile background
        this.setupTiledBackground();

        // Create tileMap types
        this.createTilemap(Tilemaps.chengy.tilemapKey);
        this.createTilemap(Tilemaps.lava.tilemapKey);
        this.createTilemap(Tilemaps.ghostRoom.placeable.tilemapKey);
        this.createTilemap(Tilemaps.ghostRoom.nonplaceable.tilemapKey);

        // Setup the rooms
        this.addRoom(Rooms.chengy_room);
        this.addRoom(Rooms.chengy_room4doors);
        this.addRoom(Rooms.chengy_room_5x3);
        this.addRoom(Rooms.chengy_room_door_up);
        this.addRoom(Rooms.l_shape_room);

        // Setup mobs
        this.addMob(Mobs.ant);
        this.addMob(Mobs.bear);

        // Setup the player
        this.setupPlayer(Player);


        this.mapScrollOffsetX += (17 * 16);
        this.mapScrollOffsetY += (15 * 16);
        this.placeRoomAtTopLeft(Rooms.chengy_room4doors.room_id, -1, -1, false);

        this.game.invalidated = true;
    },


    /**
     * Setup Functions
     * **/

    // This setups the map border and default background
    setupMap: function(){
        // Set the background to a black colour as a default
        var map_world_width = this.MAP_WIDTH * this.TILE_SIZE;
        var map_world_height = this.MAP_HEIGHT * this.TILE_SIZE;
        var map_bg_black = game.phaser.make.bitmapData(map_world_width, map_world_height);
        map_bg_black.rect(0, 0, map_world_width, map_world_height, '#000');

        // Puts the map dimensions into a variable for future use (is the mouse inside the map?)
        this.mapDimensions = {x: this.MAP_DRAW_OFFSET_X, y: this.MAP_DRAW_OFFSET_Y, w: map_world_width, h: map_world_height};

        // Setup the background outline
        var map_bg_width = map_world_width + 2;
        var map_bg_height = map_world_height + 2;
        var map_bg_red_border = game.phaser.make.bitmapData(map_bg_width, map_bg_height);
        map_bg_red_border.rect(0, 0, map_bg_width, map_bg_height, '#f00');

        // Draw the border outside the map (a pixel to the left and up)
        game.phaser.add.sprite(this.MAP_DRAW_OFFSET_X - 1, this.MAP_DRAW_OFFSET_Y - 1, map_bg_red_border);
        // Default black background colour (If the grid somehow bugs out and doesn't show I guess...)
        game.phaser.add.sprite(this.MAP_DRAW_OFFSET_X, this.MAP_DRAW_OFFSET_Y, map_bg_black);


        // Create the map of tiles
        this.map = this.game.phaser.add.renderTexture(map_world_width, map_world_height, 'map');
        this.game.phaser.add.sprite(this.MAP_DRAW_OFFSET_X, this.MAP_DRAW_OFFSET_Y, this.map);
    },

    setupTiledBackground: function(){
        // Set the empty tile sprite - the lonely initialized sprite
        this.emptyTile = game.phaser.make.sprite(0, 0, 'tile-empty-16');

        // Make the tiled background 1 tile larger on width/height so the cutoff for when it's offset doesn't show empty space
        var tiled_background = game.phaser.make.renderTexture((this.MAP_WIDTH + 1) * this.TILE_SIZE, (this.MAP_HEIGHT + 1) * this.TILE_SIZE, 'empty-tiled-background');
        for (var x = 0; x < this.MAP_WIDTH + 1; x++) {
            for (var y = 0; y < this.MAP_HEIGHT + 1; y++) {
                tiled_background.renderRawXY(this.emptyTile, x * this.TILE_SIZE, y * this.TILE_SIZE);
            }
        }

        // Make the tiled background a single image
        this.emptyTileBackground = this.game.phaser.make.image(0, 0, tiled_background);

        // Now draw the empty grid on to the map
        this.renderEmptyTileBackground();
    },

    setupPlayer: function(player){
        var sprite = game.phaser.make.sprite(0, 0, player.sprite.key);
        this.player = {
            sprite: sprite,
            position: {
                x: 0,
                y: 0
            },
            size: player.sprite.size,
            center: player.sprite.center,
            is_visible: true
        };
    },


    /**
     * Adding Functions
     * **/

    addRoom: function(room_data){
        // Construct the tilemap from the data in tile-data.js
        // var tilemap = 'tilemap-' + room_data.tilemap;

        // Set the room type we are adding
        this.roomTypes[room_data.room_id] = {dimensions: room_data};//, tilemap: tilemap};

        // Add the normal tilemap, the placeable and nonplaceable textures
        // ToDo: This might be slightly inefficient creating it every time, but it's only called 5 times at the start of the
        // ToDo: game - so it's not too bad... [but a future improvement]
        var tileMapKeys = [
            {
                tilemap: room_data.tilemap.tilemapKey,
                key_suffix: ''
            },
            {
                tilemap: Tilemaps.ghostRoom.placeable.tilemapKey,
                key_suffix: Tilemaps.ghostRoom.placeable.suffix
            },
            {
                tilemap: Tilemaps.ghostRoom.nonplaceable.tilemapKey,
                key_suffix: Tilemaps.ghostRoom.nonplaceable.suffix
            }
        ];

        // Loop over all the different tileMapKeys for this room
        for(var i=0; i<tileMapKeys.length; i++){
            var tilemap_temp = tileMapKeys[i];

            // Create a unique key based on the room_key and the specific tileMapKey we're using
            var key = room_data.room_id + tilemap_temp.key_suffix;

            // Create a texture the size of the room
            var room_cache = game.phaser.make.renderTexture(room_data.w * this.TILE_SIZE, room_data.h * this.TILE_SIZE, key);
            for(var y=0; y<room_data.data2.length; y++){
                for(var x=0; x<room_data.data2[y].length; x++){
                    // Get the tile data at location [x,y]
                    var tile_data = room_data.data2[y][x];

                    // If it's set to nothing, then skip and go to the next tile
                    if(tile_data == '') continue;

                    // Render the right room piece
                    room_cache.renderRawXY(this.tileMapTypes[tilemap_temp.tilemap][tile_data], x * this.TILE_SIZE, y * this.TILE_SIZE);
                }
            }

            // Create a single image from the tiles we've been building
            this.roomRendersCache[key] = this.game.phaser.make.image(0, 0, room_cache);
        }
    },


    addMob: function(mob){
        this.mobTypes[mob.id] = {mob: mob, render: this.game.phaser.make.sprite(0, 0, mob.sprite.key)};
    },

    // Creates a tilemap and the individual tile pieces [top-left => tl, bottom-middle => bm, etc...]
    createTilemap: function(tilemap_key){
        this.tileMapTypes[tilemap_key] = {};
        var tilemap_positions = ['tl', 'tm', 'tr', 'mr', 'br', 'bm', 'bl', 'ml', 'mm'];

        for(var i = 0; i < tilemap_positions.length; i++) {
            // Create a single sprite for the tileMap piece
            this.tileMapTypes[tilemap_key][tilemap_positions[i]] = game.phaser.make.sprite(0, 0, tilemap_key);
            // This sets a textures key to have different frames
            this.tileMapTypes[tilemap_key][tilemap_positions[i]].frameName = tilemap_positions[i];
        }
    },


    /**
     * Render Functions
     * **/

    // This draws the empty tile background
    renderEmptyTileBackground: function() {
        // Clear the drawn rooms
        this.map.clear();

        // Get the map offset (mod TILE_SIZE)
        var offset = this.getMapDrawOffset();
        // but offset it minus a tile size x/y as if it overlaps it will miss out the first row/column
        offset.x -= this.TILE_SIZE;
        offset.y -= this.TILE_SIZE;

        // Render the single background image
        this.map.renderRawXY(this.emptyTileBackground, offset.x, offset.y);
    },

    // This draws all rooms onto the map
    renderAllRooms: function(){
        // For each room, render it in the correct position on the map
        // ToDo: Check if the room is on the map before placing (less computation)
        for(var i = 0; i < this.mapRooms.length; i++){
            var r = this.mapRooms[i];
            var render_x = r.x * this.TILE_SIZE + this.mapScrollOffsetX;
            var render_y = r.y * this.TILE_SIZE + this.mapScrollOffsetY;

            this.map.renderRawXY(this.roomRendersCache[r.room_id], render_x, render_y);
        }
    },

    // Draws the ghostRoom!
    renderGhostRoom: function(){
        // If the mouse is not inside the map, don't render the ghostRoom
        if(!this.isMouseInsideMap()) return;

        // So this references the main game object and then the builderObject to see if a tile is selected
        // ToDo: If anyone has any other idea how to check if a tile is selected - be my guest and tell me
        if(this.game.builderObject.which_tile_to_place != -1 && this.game.builderObject.typeOfTileToPlace == 'mob'){
            var btn = this.game.builderObject.mobButtons[this.game.builderObject.which_tile_to_place];

            var mob_id = btn.mob_id;
            var mob = this.mobTypes[mob_id];

            var tile = this.getTileFromMouseXY();
            var mouse = getAccurateCoords();
            mouse.x -= this.MAP_DRAW_OFFSET_X;
            mouse.y -= this.MAP_DRAW_OFFSET_Y;

            mouse.x -= 8;
            mouse.y -= 8;

            // console.log("renderGhostRoom ["+mouse.x+","+mouse.y+"]");
            //
            // var draw_x = (tile.x) * this.TILE_SIZE + this.mapScrollOffsetX;
            // var draw_y = (tile.y) * this.TILE_SIZE + this.mapScrollOffsetY;

            this.map.renderRawXY(mob.render, mouse.x, mouse.y);


        }else if(this.game.builderObject.which_tile_to_place != -1 && this.game.builderObject.typeOfTileToPlace == 'room') {
            // Get which button is selected
            var btn = this.game.builderObject.buttons[this.game.builderObject.which_tile_to_place];

            // Each button references a room
            var room_id = btn.room_id;
            // Finally get the room from the button - coolio!
            var room = this.roomTypes[room_id];


            // Get the tileXY from mouse coords
            var tile = this.getTileFromMouseXY();

            // Where should the room be placed
            var draw_x = (tile.x - room.dimensions.center.x) * this.TILE_SIZE + this.mapScrollOffsetX;
            var draw_y = (tile.y - room.dimensions.center.y) * this.TILE_SIZE + this.mapScrollOffsetY;

            // Check which ghostRoom should be displayed
            if(this.canPlaceRoom(room_id, tile.x, tile.y)) {
                this.map.renderRawXY(this.roomRendersCache[room_id + Tilemaps.ghostRoom.placeable.suffix], draw_x, draw_y);
            }else{
                this.map.renderRawXY(this.roomRendersCache[room_id + Tilemaps.ghostRoom.nonplaceable.suffix], draw_x, draw_y);
            }
        }
    },

    renderPlayer: function(){
        var draw_x = this.player.position.x - this.player.center.x + this.mapScrollOffsetX;
        var draw_y = this.player.position.y - this.player.center.y + this.mapScrollOffsetY;

        this.map.renderRawXY(this.player.sprite, draw_x, draw_y);
    },

    renderMobs: function(){
        for(var i = 0; i < this.mapMobs.length; i++){
            var mob = this.mapMobs[i];

            var render_x = mob.x + this.mapScrollOffsetX; // (mob.x) * this.TILE_SIZE + this.mapScrollOffsetX;
            var render_y = mob.y + this.mapScrollOffsetY; // (mob.y) * this.TILE_SIZE + this.mapScrollOffsetY;

            this.map.renderRawXY(mob.mob_type.render, render_x, render_y);
        }
    },

    redrawEverything: function(){
        this.game.invalidated = true;
        // this.render();
    },

    // You kill this function - nothing ever redraws onto the map
    render: function(){
        this.renderEmptyTileBackground();
        this.renderAllRooms();
        this.renderMobs();
        this.renderPlayer();
        this.renderGhostRoom();

        // Debug to see how often things are redrawn..
        this.game.redraws += 1;
        document.getElementById('redraw-count').innerHTML = this.game.redraws;
    },


    /**
     * Mouse and Keyboard handlers
     * **/

    onDown: function(e) {
        // If the mouse is inside the map when the user clicks?
        if(this.isMouseInsideMap()){
            // If there is a room-tile selected inside the [game.builderObject]
            if(this.game.builderObject.isPlacingMob()){
                var btn = this.game.builderObject.getActiveButton();
                var mob = this.mobTypes[btn.mob_id];

                // var tile = this.getTileFromMouseXY();
                var pos = this.getMobPosXY(mob.mob.sprite.size);
                if(this.canPlaceMob(btn.mob_id, pos)){
                    this.placeMob(btn.mob_id, pos, true);
                }

            }else if(this.game.builderObject.isPlacingRoom()){
                var btn = this.game.builderObject.buttons[this.game.builderObject.which_tile_to_place];

                // Get the tile XY where the room should be placed
                var tile = this.getTileFromMouseXY();

                // If a room can be placed - then place it?
                if(this.canPlaceRoom(btn.room_id, tile.x, tile.y)){
                    this.placeRoom(btn.room_id, tile.x, tile.y, true);
                }
            }else {
                // Can only navigate the map if you're not placing objects
                this.isDragging = true;

                // Get the mouse coords
                var pointer = this.game.phaser.input.activePointer;

                // Set the current drag position to the mouse position
                this.dragCurrentX = pointer.x;
                this.dragCurrentY = pointer.y;
            }
        }
    },

    // Called when the mouse press is released
    onUp: function(e){
        if(this.isDragging) {
            this.isDragging = false;

            // Other functionality when the player has stopped dragging
        }
    },

    // Called when the mouse is moved
    onMove: function(e){

        if(this.isDragging) {
            // Mouse coords
            var pointer = this.game.phaser.input.activePointer;

            // Difference in position from last drag
            var difference_x = this.dragCurrentX - pointer.x;
            var difference_y = this.dragCurrentY - pointer.y;

            // Update the new drag coords
            this.dragCurrentX = pointer.x;
            this.dragCurrentY = pointer.y;

            // Update the map offset
            this.mapScrollOffsetX -= difference_x;
            this.mapScrollOffsetY -= difference_y;

            // Redraw things
            this.redrawEverything();
        }else{
            // Is a tile selected
            if(this.game.builderObject.which_tile_to_place != -1){

                // If it's inside the map
                if(this.isMouseInsideMap()){

                    if(this.game.builderObject.typeOfTileToPlace == 'mob'){
                        this.redrawEverything();
                    }else if(this.game.builderObject.typeOfTileToPlace == 'room') {
                        var tile = this.getTileFromMouseXY();

                        // If the ghostRoom draw XY has changed from last time
                        if (this.ghostRoomX != tile.x || this.ghostRoomY != tile.y) {
                            this.ghostRoomX = tile.x;
                            this.ghostRoomY = tile.y;

                            // Redraw all the things!
                            this.redrawEverything();
                        }
                    }
                }else{
                    // If the mouse is not inside the map - we don't want to show the ghost room
                    if(this.ghostRoomX != -1 || this.ghostRoomY != -1) {
                        // Set the coords to a dummy value only used for a check
                        this.ghostRoomX = -1;
                        this.ghostRoomY = -1;

                        // Redraw everything again
                        this.redrawEverything();
                    }
                }
            }
        }
    },

    // Called when a keyboard press happens
    keyOnDown: function(e){
      if(e.key == "r"){
          // Rotation of ghost-room... somehow
      }
    },

    /** Placing things on map functions **/
    canPlaceMob: function(mob_id, position){
        // var tile = this.getTileFromMouseXY();
        // var mob = this.game.builderObject.getActiveButton();
        var mob = this.mobTypes[mob_id];

        for(var i = 0; i < this.mapRooms.length; i++) {
            var r = this.mapRooms[i];

            if(isXYWithinBoundingBox(position.x, position.y, r.abs_bb)){
                // If there are no other mobs (or items)
                return true;
            }
        }

        //If we get here - it is outside of a room
        return false;
    },
    placeMob: function(mob_id, position, send_message){
        var mob_type = this.mobTypes[mob_id];
        var map_mob_id = this.mapMobs.length;

        console.log("placeMob");

        var center = {
            x: position.x + mob_type.mob.sprite.size.width / 2,
            y: position.y + mob_type.mob.sprite.size.height / 2
        };

        var unity_position = {
            x: center.x / this.TILE_SIZE,
            y: center.y / this.TILE_SIZE
        };

        this.mapMobs[map_mob_id] = {
            id: map_mob_id,
            mob_id: mob_id,
            x: position.x,
            y: position.y,
            center: center,
            mob_type: mob_type,
            unity_position: unity_position
        };

        console.log("Placed mob:");
        console.log(this.mapMobs[map_mob_id]);

        // Debug print info
        console.log("Creating mob:\nType: "+mob_type.mob.sprite.key+"\nX: "+center.x+", Y: "+center.y+"\nUX: "+unity_position.x+", UY: "+unity_position.y);

        if(send_message){
            var data = {
                "command": "spawnMob",
                "options": {
                    "objectId": mob_id,
                    "xPos": unity_position.x,
                    "zPos": unity_position.y,
                    "id": map_mob_id
                }
            };
            this.game.sendMessage('create-mob', data);
        }
    },


    /**
     * Room Functionality
     * **/

    // Checks whether a room can fit at the coordinates specified
    canPlaceRoom: function(room_id, x, y){
        var room = this.roomTypes[room_id];
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


    placeRoomAtTopLeft: function(room_id, x, y, send_message) {
        var room = this.roomTypes[room_id];

        this.placeRoom(room_id, x + room.dimensions.center.x, y + room.dimensions.center.y, send_message);
    },
    placeRoom: function(room_id, x, y, send_message){
        // Get the room data from the roomTypes array
        var room = this.roomTypes[room_id];

        // Basic way of getting the next index
        var map_room_id = this.mapRooms.length;

        // Get the bounding box from the current x/y and room data
        var bounding_box = this.getBoundingBoxForRoom(x, y, room);
        var absolute_bb = this.getAbsBoundingBoxForRoom(x, y, room);

        // Add the room into the world room array
        // ToDo: Change what mapRooms[x] contains as I'm pretty sure some of this is unnecessary
        this.mapRooms[map_room_id] = {
            room_id: room_id,
            x: x - room.dimensions.center.x,
            y: y - room.dimensions.center.y,
            x2: x,
            y2: y,
            bb: bounding_box,
            abs_bb: absolute_bb,
            room: room
        };

        // Should we send a message to the Unity client?
        if(send_message) {
            // Broadcast a message to the VR client
            var data = {
                objectId: room_id,
                xPos: x - room.dimensions.center.x,
                yPos: y - room.dimensions.center.y
            };
            this.game.sendMessage("create-room", data);
        }

        // Repaint the world with the new room
        this.redrawEverything();
    },

    setPlayerPosition: function(msg){
        this.player.position.x = msg.xPos * this.TILE_SIZE;
        this.player.position.y = msg.zPos * this.TILE_SIZE;

        console.log("px="+this.player.position.x+", py="+this.player.position.y);

        this.game.invalidated = true;
    },
    /**
     * Helper Functions
     */

    removeAllRooms: function(){
        while(this.mapRooms.length){
            this.mapRooms.pop();
        }
    },

    // This gets the current map scroll offset mod the TILE_SIZE - so it returns [0 -> (TILE_SIZE -1)] inclusive
    getMapDrawOffset: function(){
        var x = this.mapScrollOffsetX % this.TILE_SIZE;
        var y = this.mapScrollOffsetY % this.TILE_SIZE;
        return {
            x: (x < 0 ? x + this.TILE_SIZE : x),
            y: (y < 0 ? y + this.TILE_SIZE : y)
        };
    },

    // This returns the tile X/Y of where the current mouse point is
    // !!! This does not return nothing if it is not inside the map !!!
    getTileFromMouseXY: function(){
        var mouse = getAccurateCoords();
        return {
            x: Math.floor((mouse.x - this.MAP_DRAW_OFFSET_X - this.mapScrollOffsetX) / this.TILE_SIZE),
            y: Math.floor((mouse.y - this.MAP_DRAW_OFFSET_Y - this.mapScrollOffsetY) / this.TILE_SIZE)
        };
    },

    getMobPosXY: function(mob_size){
        var mouse = getAccurateCoords();
        mouse.x -= (this.MAP_DRAW_OFFSET_X + mob_size.width / 2) + this.mapScrollOffsetX;
        mouse.y -= (this.MAP_DRAW_OFFSET_Y + mob_size.height / 2) + this.mapScrollOffsetY;
        return mouse;
    },

    // Do two bounding boxes collide?
    doesBoundingBoxesCollide: function(bb1, bb2){
        return !((bb2.x1 > bb1.x2) || (bb2.x2 < bb1.x1) || (bb2.y1 > bb1.y2) || (bb2.y2 < bb1.y1));
    },

    // Returns a bounding box for a room at a specific location
    getBoundingBoxForRoom: function(x, y, room){
        // top left coordinates of the room
        var top_left_x = x - room.dimensions.center.x;
        var top_left_y = y - room.dimensions.center.y;

        // bottom right coordinates of the room
        var bottom_right_x = top_left_x + room.dimensions.w - 1;
        var bottom_right_y = top_left_y + room.dimensions.h - 1;

        return {
            x1: top_left_x,
            y1: top_left_y,
            x2: bottom_right_x,
            y2: bottom_right_y
        };
    },
    getAbsBoundingBoxForRoom: function(x, y, room) {
        var bb = this.getBoundingBoxForRoom(x, y, room);
        bb.x1 *= 16;
        bb.y1 *= 16;
        bb.x2 *= 16;
        bb.y2 *= 16;
        return bb;
    },

    // Returns true if the mouse is inside the map area (not the whole map viewport)
    isMouseInsideMap: function(){
        var mouse = getAccurateCoords();
        return isPointWithinRect(mouse, this.mapDimensions);
    }
};