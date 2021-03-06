/*
 @author Daniel Jackson (dj233)
 */

Main.appendPrototype({

    // room vars and types
    rooms: [],
    room_types: [],

    // Preload all the rooms
    rooms_preload: function(){
        var self = this;

        Object.keys(Rooms).forEach(function(k){
            self.preload_room(Rooms[k]);
        });
    },

    // Create a room
    rooms_create: function(){
        var self = this;

        Object.keys(Rooms).forEach(function(k){
            self.addRoomType(Rooms[k]);
        });
    },


    /** Creation of rooms **/
    addRoomType: function(room_data){
        this.room_types[room_data.room_id] = room_data;
    },

    preload_room: function(room){
        this.game.load.image(room.assets.normal.key, room.assets.normal.path);
        this.game.load.image(room.assets.green.key, room.assets.green.path);
        this.game.load.image(room.assets.red.key, room.assets.red.path);
    },

    rooms_debug: function(){
    }
});