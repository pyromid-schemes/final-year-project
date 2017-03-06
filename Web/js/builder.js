

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

    create: function(){
        this.add_room_btn(650, 20, Rooms.room1.room_id);
        this.add_room_btn(650, 70, Rooms.room2.room_id);
        this.add_room_btn(700, 20, Rooms.room3.room_id);

        this.add_mob_btn(650, 180, Mobs.ant);
        this.add_mob_btn(700, 180, Mobs.bear);
        this.add_mob_btn(650, 230, Mobs.skellyCheng);


        this.tile_selector = this.game.add.sprite(0, 0, 'builder-button-selector');
        this.tile_selector.alpha = 0;

        window.builderScene = this;
    },

    add_room_btn: function(x, y, key){
        var btn = this.game.add.sprite(x, y, key);
        btn.scale.set(0.25);
        btn.inputEnabled = true;
        btn.events.onInputDown.add(this.listener, this);
        this.buttons[key] = {tile: btn, id: key};
    },
    add_mob_btn: function(x, y, mob){
        var btn = this.game.add.sprite(x, y, mob.builderButton.key);
        btn.inputEnabled = true;
        btn.events.onInputDown.add(this.listener, this);
        this.mobButtons[mob.builderButton.key] = {tile: btn, id: mob.sprite.key, mob: mob};
    },


    keyOnDown: function(e, self){
        if(e.keyCode == Phaser.Keyboard.ESC){
            this.unselectTile();
            this.hideTileSelector();
        }
    },

    listener: function(e){
        var key = e.key;

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
    }
};

