var PI_HALF = Math.PI/2;

Main.appendPrototype({

    ghostroom_bb: null,
    ghostroom_obj: null,
    ghostroom_red: null,
    ghostroom_green: null,
    ghostroom_selected_room_key: null,
    ghostroom_rot: 0,
    ghostroom_can_place: false,

    ghostroom_preload: function(){
    },

    ghostroom_room_selected: function(key){
        this.ghostroom_selected_room_key = key;

        this.ghostroom_obj = this.room_types[key];
        this.ghostroom_red = this.ghostroom_create_room(key + '-red');
        this.ghostroom_green = this.ghostroom_create_room(key + '-green');

        this.ghostroom_bb = {x1: 0, y1: 0, x2: 0, y2: 0};

        this.ghostroom_rot = 0;

        this.ghostroom_update();
    },
    ghostroom_room_unselected: function(){
        if(this.ghostroom_red == null) return;

        this.ghostroom_red.destroy();
        this.ghostroom_green.destroy();
    },

    ghostroom_create_room: function(key){
        var room = this.game.add.image(0, 0, key, null, this.map_group);
        room.scale.setTo(0.5);
        room.pivot.x = 64;
        room.pivot.y = 64;
        return room;
    },

    ghostroom_update_room_positions: function(pos){
        this.ghostroom_red.x = pos.x; this.ghostroom_red.y = pos.y;
        this.ghostroom_green.x = pos.x; this.ghostroom_green.y = pos.y;

        this.ghostroom_bb = {
            x1: pos.x - this.ghostroom_obj.scaled.cx, y1: pos.y - this.ghostroom_obj.scaled.cy,
            x2: pos.x + this.ghostroom_obj.scaled.cx, y2: pos.y + this.ghostroom_obj.scaled.cy
        }
    },

    ghostroom_mouse_move: function(){
        this.ghostroom_update();
    },

    ghostroom_update: function(){
        if(this.ghostroom_red != null){
            var tile = this.get_tile_xy();

            var pos = {x: tile.x * 16, y: tile.y * 16};

            this.ghostroom_update_room_positions(pos);
            this.ghostroom_check_room_collisions(pos);
        }
    },

    ghostroom_check_room_collisions: function(pos){
        this.ghostroom_can_place = true;
        for(var i=0; i<this.rooms.length; i++){
            var room = this.rooms[i];

            if(Utility.doBoundingBoxesCollide(room.bb, this.ghostroom_bb)){
                this.ghostroom_can_place = false;
                break;
            }
        }

        if(this.ghostroom_can_place){
            // Check for grid snap points
            var grid_snap_points = this.ghostroom_generate_snap_points(pos);

            this.ghostroom_can_place = false;
            for(var i=0; i<grid_snap_points.length; i++){
                var gsp = grid_snap_points[i];
                if(this.gridsnap_is_point_in_list(gsp)){
                    this.ghostroom_can_place = true;
                    break;
                }
            }
        }

        if(this.ghostroom_can_place){
            this.ghostroom_red.alpha = 0;
            this.ghostroom_green.alpha = 1;
        }else{
            this.ghostroom_red.alpha = 1;
            this.ghostroom_green.alpha = 0;
        }
    },

    ghostroom_generate_snap_points: function(pos){
        var grid_snap_points = [];
        for(var i=0; i<this.ghostroom_obj.door_positions.length; i++){
            var door_pos = this.ghostroom_obj.door_positions[i];
            var translated_door_pos = this.ghostroom_translate_door_pos(door_pos);

            var pos2 = {x: pos.x + translated_door_pos.x, y: pos.y + translated_door_pos.y};
            grid_snap_points.push(pos2);
        }
        return grid_snap_points;
    },

    ghostroom_translate_door_pos: function(door_pos){
        var door_index = -1;
        for(var i=0; i<DOORS.length; i++){
            if(door_pos.x == DOORS[i].x && door_pos.y == DOORS[i].y){ door_index = i; break; }
        }
        door_index += this.ghostroom_rot;
        if(door_index > 3) door_index -= 4;

        return DOORS[door_index];
    },



    ghostroom_rotate_rooms: function(){
        if(this.ghostroom_red == null) return;

        this.ghostroom_rot = (this.ghostroom_rot + 1) % 4;
        var rot = this.ghostroom_red.rotation + PI_HALF;

        this.ghostroom_red.rotation = rot;
        this.ghostroom_green.rotation = rot;
    },

    ghostroom_keyOnDown: function(e) {
        if(e.keyCode == Phaser.Keyboard.R){
            this.ghostroom_rotate_rooms();
            this.ghostroom_update();
        }
    },

    ghostroom_try_to_place: function(){
        if(this.ghostroom_red == null) return;

        if(this.ghostroom_can_place){
            var pos = {x: this.ghostroom_red.position.x, y: this.ghostroom_red.position.y};
            this.place_room(this.ghostroom_selected_room_key, pos.x, pos.y, this.ghostroom_red.rotation, true);

            this.ghostroom_redraw();
        }


    },

    ghostroom_redraw: function(){
        if(this.ghostroom_red == null) return;

        var pos = this.ghostroom_red.position;
        var rot = this.ghostroom_rot;

        this.ghostroom_red.destroy();
        this.ghostroom_green.destroy();

        this.ghostroom_room_selected(this.ghostroom_selected_room_key);

        this.ghostroom_update_room_positions(pos);

        for(var i=0; i<rot; i++) this.ghostroom_rotate_rooms();
    },


    ghostroom_debug: function(){
    }
});