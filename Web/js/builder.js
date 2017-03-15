

var Builder = function(game, main){
    this.game = game;
    this.main = main;
};

Builder.prototype = {

    buttons: [],
    mobButtons: [],
    which_tile_to_place:-1,
    tile_selector: null,
    typeOfTileToPlace: null,

    cooldown_buttons: [],

    create: function(){
        this.add_room_btn(650, 20, Rooms.room1);
        this.add_room_btn(650, 70, Rooms.room2);
        this.add_room_btn(700, 20, Rooms.room3);
        this.add_room_btn(700, 70, Rooms.room4);
        this.add_room_btn(650, 120, Rooms.room5);

        this.add_mob_btn(650, 180, Mobs.ant);
        this.add_mob_btn(700, 180, Mobs.bear);
        this.add_mob_btn(650, 230, Mobs.skellyCheng);

        this.tile_selector = this.game.add.sprite(0, 0, 'builder-button-selector');
        this.tile_selector.alpha = 0;

        window.builderScene = this;
    },

    add_room_btn: function(x, y, room){
        var btn = this.game.add.sprite(x, y, room.room_id);
        btn.scale.set(32 / room.w);
        btn.inputEnabled = true;
        btn.events.onInputDown.add(this.listener, this);
        this.buttons[room.room_id] = {tile: btn, id: room.room_id};
    },
    add_mob_btn: function(x, y, mob){
        var btn = this.game.add.sprite(x, y, mob.builderButton.key);
        btn.inputEnabled = true;
        btn.events.onInputDown.add(this.listener, this);
        this.mobButtons[mob.builderButton.key] = {tile: btn, id: mob.sprite.key, mob: mob};
    },


    keyOnDown: function(e){
        if(e.keyCode == Phaser.Keyboard.ESC){
            this.unselectTile();
            this.hideTileSelector();
        }
    },

    listener: function(e){
        var key = e.key;

        if(this.is_button_on_cooldown(key)) return;

        this.unselectTile();

        if(this.which_tile_to_place == key){
            this.hideTileSelector();
        }else{
            this.selectTile(key);
        }
    },
    selectTile: function(key){
        this.which_tile_to_place = key;


        var obj;
        if(key in this.buttons){
            obj = this.buttons[key];
            this.typeOfTileToPlace = 'room';
            this.main.ghostroom_room_selected(key);
        }else if(key in this.mobButtons){
            obj = this.mobButtons[key];
            this.typeOfTileToPlace = 'mob';
            this.main.mob_selected(obj.mob, true);
        }else{
            throw new Error("Error selecting tile with key ["+key+"]");
        }


        this.tile_selector.x = obj.tile.x - 2;
        this.tile_selector.y = obj.tile.y - 2;
        this.tile_selector.alpha = 1;
    },
    deselectAll: function(){
        this.unselectTile();
        this.hideTileSelector();
    },
    unselectTile: function(){
        this.main.ghostroom_room_unselected();
        this.main.mob_unselected();
    },
    hideTileSelector: function(){
        this.which_tile_to_place = -1;
        this.tile_selector.alpha = 0;
    },

    is_tile_selected: function(){
        return this.which_tile_to_place != -1;
    },


    startTimeDelay: function(){
        var key = this.which_tile_to_place;
        if(key == -1) return;

        var mob_btn = this.mobButtons[key];
        var delay = mob_btn.mob.spawnDelay;

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

