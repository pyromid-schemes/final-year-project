
var Builder = {
    game: null,

    // This holds all of the clickable buttons in the UI
    buttons: {},

    mobButtons: {},

    tile_selector: null,

    // There is initially no room-tile selected
    which_tile_to_place: -1,


    create: function(GameObject){
        //Reference the main game object
        this.game = GameObject;

        // Set the background colour for the section
        game.graphics.beginFill(0xdc6363, 1);
        game.graphics.drawRect(game.builderView.x, game.builderView.y, game.builderView.w, game.builderView.h);
        game.graphics.endFill();

        // Setup the buttons which place rooms
        this.createButton(600, 40, Rooms.chengy_room);
        this.createButton(600, 80, Rooms.chengy_room4doors);
        this.createButton(640, 40, Rooms.chengy_room_5x3);
        this.createButton(640, 80, Rooms.chengy_room_door_up);
        this.createButton(680, 40, Rooms.l_shape_room);


        this.createMobButton(600, 160, Mobs.ant);
        this.createMobButton(640, 160, Mobs.bear);


        // Create the 'selector' so that when a room tile is selected it can be reflect it's state
        this.tile_selector = game.phaser.add.sprite(0, 0, 'builder-button-selector');
        this.tile_selector.alpha = 0;
    },

    /** Adding buttons to the view **/

    // Creates a button
    createButton: function(x, y, room){
        var btn = game.phaser.add.sprite(x, y, room.builderButton.key);
        btn.inputEnabled = true;
        btn.events.onInputDown.add(this.listener, this);
        this.buttons[room.builderButton.key] = {tile: btn, room_id: room.room_id};
    },

    createMobButton: function(x, y, mob_button){
        var btn = game.phaser.add.sprite(x, y, mob_button.builderButton.key);
        btn.inputEnabled = true;
        btn.events.onInputDown.add(this.listener, this);
        this.mobButtons[mob_button.builderButton.key] = {tile: btn, mob_id: mob_button.id}
    },

    // When a button is clicked, figure out which button it was and set some variables
    listener: function(){
        var sprite = arguments[0];
        var key = sprite.key;

        if(this.which_tile_to_place == key){
            return this.unselectTile(); //Unselect the tile and return - saves a line of code.
        }
        this.selectTile(key);
    },

    isPlacingSomething: function(){
        return this.which_tile_to_place != -1;
    },
    isPlacingRoom: function(){
        return this.isPlacingSomething()  && this.typeOfTileToPlace == 'room'
    },
    isPlacingMob: function(){
        return this.isPlacingSomething()  && this.typeOfTileToPlace == 'mob'
    },

    getActiveButton: function(){
        if(this.typeOfTileToPlace == 'room')
            return this.game.builderObject.buttons[this.game.builderObject.which_tile_to_place];
        if(this.typeOfTileToPlace == 'mob')
            return this.game.builderObject.mobButtons[this.game.builderObject.which_tile_to_place];
        return null;
    },

    selectTile: function(key){
        var obj;
        if(key in this.buttons){
            obj = this.buttons[key];
            this.typeOfTileToPlace = 'room'
        }else if(key in this.mobButtons){
            obj = this.mobButtons[key];
            this.typeOfTileToPlace = 'mob'
        }else{
            throw new Error("Error selecting tile with key ["+key+"]");
        }

        // Get the button object
        // Update the tile selector to be in the correct position of the tile
        this.tile_selector.x = obj.tile.x - 2;
        this.tile_selector.y = obj.tile.y - 2;
        // Show the tile selector
        this.tile_selector.alpha = 1;

        // Update this so that all the systems know a room-tile is selected
        this.which_tile_to_place = key;
    },
    unselectTile: function(){
        this.tile_selector.alpha = 0;
        this.which_tile_to_place = -1;

        this.game.mapObject.redrawEverything();
    },

    keyOnDown: function(e){
        if(e.key == "Escape"){
            this.unselectTile();
        }
    }
};