
var Builder = {
    game: null,// = GameObject;

    // var buttons;

    tile_selector: null,

    which_tile_to_place: null,


    create: function(GameObject){
        this.game = GameObject;
        // console.log("Builder ");
        // console.log(this.game);
        // Set the background colour for the section
        game.graphics.beginFill(0xdc6363, 1);
        game.graphics.drawRect(game.builderView.x, game.builderView.y, game.builderView.w, game.builderView.h);
        game.graphics.endFill();


        //Setup builder buttons
        game.buttons = {};

        this.createButton(600, 40, 'builder-button-chengy', 'room-1');
        this.createButton(600, 80, 'builder-button-chengy4doors', 'room-2');


        this.tile_selector = game.phaser.add.sprite(0, 0, 'builder-button-selector');
        this.tile_selector.alpha = 0;


        this.which_tile_to_place = -1;

    },

    createButton: function(x, y, sprite_path, room_id){
        var btn = game.phaser.add.sprite(x, y, sprite_path);
        btn.inputEnabled = true;
        btn.events.onInputDown.add(this.listener, this);
        game.buttons[sprite_path] = {tile: btn, room_id: room_id};
    },

    listener: function(){
        var sprite = arguments[0];
        var key = sprite.key;
        // var obj = hashmap[key];

        if(this.which_tile_to_place == key){
            this.tile_selector.alpha = 0;
            this.which_tile_to_place = -1;
            return;
        }
        this.which_tile_to_place = key;

        if(this.which_tile_to_place.length > 0){
            var obj = game.buttons[key];
            this.tile_selector.x = obj.tile.x - 2;
            this.tile_selector.y = obj.tile.y - 2;

            this.tile_selector.alpha = 1;
        }
    }
};