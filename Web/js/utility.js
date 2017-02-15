var Utility = {

    unityRotToWebRot: function(rot){
        return (rot - 180) * (Math.PI / 180);
    },
    webRotToUnityRot: function(rot){
        return (rot * 180 / Math.PI) - 180;
    },

    isPointWithinRect: function(point, rect){
        return ( point.x >= rect.x && point.y >= rect.y && point.x <= rect.x + rect.w && point.y <= rect.y + rect.h );
    },

    doBoundingBoxesCollide: function(bb1, bb2){
        return !((bb2.x1 >= bb1.x2) || (bb2.x2 <= bb1.x1) || (bb2.y1 >= bb1.y2) || (bb2.y2 <= bb1.y1));
    },

    get_bounding_box_from_room: function(x, y, room_id){
        var room_type = main.room_types[room_id];
        return {
            x1: x - room_type.scaled.cx,
            y1: y - room_type.scaled.cy,
            x2: x + (room_type.scaled.w - room_type.scaled.cx),
            y2: y + (room_type.scaled.h - room_type.scaled.cy)
        };
    },



    /** DEBUG **/
    DEBUG_MESSAGES: true,

    debug: function(){
        if(this.DEBUG_MESSAGES){
            for (var i = 0; i < arguments.length; i++){
                console.log(arguments[i]);
            }
        }
    }
};