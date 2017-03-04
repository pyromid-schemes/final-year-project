Main.appendPrototype({


    // ghostroom: null,
    ghostroom_red: null,
    ghostroom_green: null,
    ghostroom_selected_room_key: null,


    ghostroom_preload: function(){
        // this.game.load.image('gridsnap-circle', 'assets/gridsnap/circle.png');
    },

    ghostroom_room_selected: function(key){
      Utility.debug("ghostroom_room_selected", key);

        this.ghostroom_selected_room_key = key;

        // this.ghostroom = this.ghostroom_create_room(key);
        this.ghostroom_red = this.ghostroom_create_room(key + '-red');
        this.ghostroom_green = this.ghostroom_create_room(key + '-green');

        this.ghostroom_update();
    },

    ghostroom_create_room: function(key){
        var room = this.game.add.image(0, 0, key, null, this.map_group);
        room.scale.setTo(0.5);
        room.pivot.x = 64;
        room.pivot.y = 64;
        return room;
    },

    ghostroom_update_room_positions: function(x, y){
        // this.ghostroom.x = x; this.ghostroom.y = y;
        this.ghostroom_red.x = x; this.ghostroom_red.y = y;
        this.ghostroom_green.x = x; this.ghostroom_green.y = y;
    },

    ghostroom_mouse_move: function(){
        this.ghostroom_update();
    },

    ghostroom_update: function(){
        if(this.ghostroom_red != null){
            var tile = this.get_tile_xy();

            var tx16 = tile.x * 16;
            var ty16 = tile.y * 16;

            this.ghostroom_update_room_positions(tx16, ty16);

            this.ghostroom_check_room_collisions();
        }
    },

    ghostroom_check_room_collisions: function(){

    },



    ghostroom_debug: function(){
    }
});