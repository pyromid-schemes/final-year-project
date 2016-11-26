
var Builder = {
    game: null,

    // This holds all of the clickable buttons in the UI
    buttons: {},

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
        this.createButton(600, 40, 'builder-button-chengy', 'room-1');
        this.createButton(600, 80, 'builder-button-chengy4doors', 'room-2');
        this.createButton(640, 40, 'builder-button-lava5x3', 'room-3');
        this.createButton(640, 80, 'builder-button-chengy-door-up', 'room-4');


        // Create the 'selector' so that when a room tile is selected it can be reflect it's state
        this.tile_selector = game.phaser.add.sprite(0, 0, 'builder-button-selector');
        this.tile_selector.alpha = 0;
    },

    // Creates a button
    createButton: function(x, y, sprite_path, room_id){
        var btn = game.phaser.add.sprite(x, y, sprite_path);
        btn.inputEnabled = true;
        btn.events.onInputDown.add(this.listener, this);
        this.buttons[sprite_path] = {tile: btn, room_id: room_id};
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

    selectTile: function(key){
        // Get the button object
        var obj = this.buttons[key];
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
    }
};