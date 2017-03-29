/*
 @author Daniel Jackson (dj233)
 */

var Builder = function(game, main){
    this.game = game;
    this.main = main;
};

Builder.prototype = {

    // Contains all the room buttons
    roomButtons: [],
    // Contains the mob buttons
    mobButtons: [],

    // Index for which tile to place
    which_tile_to_place:-1,
    // The glow for when a tile is selected
    tile_selector: null,
    // The type of tile 'room' or 'mob'
    typeOfTileToPlace: null,

    // A list of all active cooldowns
    cooldown_buttons: [],

    // Phaser.create override
    create: function(){
        // Add the room buttons at locations
        this.add_room_btn(650, 20, Rooms.room1);
        this.add_room_btn(650, 70, Rooms.room2);
        this.add_room_btn(700, 20, Rooms.room3);
        this.add_room_btn(700, 70, Rooms.room4);
        this.add_room_btn(650, 120, Rooms.room5);

        // Add our mob to the panel
        this.add_mob_btn(650, 230, Mobs.skellyCheng);

        // Setup the tile glow
        this.tile_selector = this.game.add.sprite(0, 0, 'builder-button-selector');
        this.tile_selector.alpha = 0;

        // Global for accessing functions inside Phaser scoped functions
        window.builderScene = this;
    },

    // Adding a room button to the panel
    add_room_btn: function(x, y, room){
        var btn = this.game.add.sprite(x, y, room.room_id);
        btn.scale.set(32 / room.w);
        btn.inputEnabled = true;
        btn.events.onInputDown.add(this.listener, this);
        this.roomButtons[room.room_id] = {tile: btn, id: room.room_id};
    },
    // Adding a mob button to the panel
    add_mob_btn: function(x, y, mob){
        var btn = this.game.add.sprite(x, y, mob.builderButton.key);
        btn.inputEnabled = true;
        btn.events.onInputDown.add(this.listener, this);
        this.mobButtons[mob.builderButton.key] = {tile: btn, id: mob.sprite.key, mob: mob};
    },

    // Handler for a key press
    keyOnDown: function(e){
        if(e.keyCode == Phaser.Keyboard.ESC){
            this.unselectTile();
            this.hideTileSelector();
        }
    },

    // Listener for when a button is pressed
    listener: function(e){
        var key = e.key;

        // If the current button is on cooldown, then exit the function
        if(this.is_button_on_cooldown(key)) return;

        // Always unselect a tile
        this.unselectTile();

        // But reselect the new tile
        if(this.which_tile_to_place == key){
            this.hideTileSelector();
        }else{
            this.selectTile(key);
        }
    },

    // Takes a key of a tile and selects it
    selectTile: function(key){
        this.which_tile_to_place = key;

        var obj;
        if(key in this.roomButtons){
            obj = this.roomButtons[key];
            this.typeOfTileToPlace = 'room';
            this.main.ghostroom_room_selected(key);
        }else if(key in this.mobButtons){
            obj = this.mobButtons[key];
            this.typeOfTileToPlace = 'mob';
            this.main.mob_selected(obj.mob, true);
        }else{
            throw new Error("Error selecting tile with key ["+key+"]");
        }

        // Set the tile glow to the currently selected tile
        this.tile_selector.x = obj.tile.x - 2;
        this.tile_selector.y = obj.tile.y - 2;
        this.tile_selector.alpha = 1;
    },
    // Global deselect for builder
    deselectAll: function(){
        this.unselectTile();
        this.hideTileSelector();
    },
    // Global unselect tile for builder
    unselectTile: function(){
        this.main.ghostroom_room_unselected();
        this.main.mob_unselected();
    },
    // Global hide for selector
    hideTileSelector: function(){
        this.which_tile_to_place = -1;
        this.tile_selector.alpha = 0;
    },

    // Global method for if a tile is selected
    is_tile_selected: function(){
        return this.which_tile_to_place != -1;
    },

    // Button cooldown functionality
    startTimeDelay: function(){
        var key = this.which_tile_to_place;
        if(key == -1) return;

        var mob_btn = this.mobButtons[key];
        var delay = mob_btn.mob.spawnDelay;

        if(delay == 0) return;

        // Add the mob key to a 'cooldown' list, which is checked when a btn is pressed to see if it is on cooldown
        this.cooldown_buttons.push(key);

        // Create the grey cooldown overlay
        var btn = this.game.add.image(mob_btn.tile.x + 32, mob_btn.tile.y, '1x1');
        btn.width = -32;
        btn.height = 32;
        btn.alpha = 0.5;

        // Tween the width over the lifetime of the cooldown
        this.game.add.tween(btn).to({width: 0}, delay, Phaser.Easing.Linear.None, true);

        // Remove the grey overlay and remove the mob's key from the cooldown button list
        var self = this;
        setTimeout(function(){
            var index = self.cooldown_buttons.indexOf(key);
            self.cooldown_buttons.splice(index, 1);

            btn.destroy();
        }, delay);

        // Now the button is on cooldown, deselect it
        this.deselectAll();
    },

    // Checks whether a btn key is on cooldown
    is_button_on_cooldown: function(key){
        return this.cooldown_buttons.indexOf(key) != -1;
    }
};

