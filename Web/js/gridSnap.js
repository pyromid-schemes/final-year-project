/*
 @author Daniel Jackson (dj233)
 */

Main.appendPrototype({

    // A list of gridsnap points
    grid_snap_points: [],

    // Preload the debug circle
    gridsnap_preload: function(){
        this.game.load.image('gridsnap-circle', 'assets/gridsnap/circle.png');
    },

    // Add a GSP (Grid snap point) to the list
    gridsnap_add_point: function(pos){
        this.grid_snap_points.push(pos);
    },

    // debug print
    gridsnap_print: function(){
        console.log("gridsnap_print::");
        for(var i=0; i<this.grid_snap_points.length; i++){
            console.log(this.grid_snap_points[i]);
        }
    },

    // Add a GSP debug dot to the game
    gridsnap_add_dot: function(pos){
        var img = this.game.add.image(pos.x, pos.y, 'gridsnap-circle', null, this.map_group);
        img.pivot.x = 32;
        img.pivot.y = 32;
        img.scale.set(0.3);
        return img;
    },

    // Debug, show all the available grid snap point's
    gridsnap_show_dots: function(){
        var temp_points = [];
        for(var i=0; i<this.grid_snap_points.length; i++){
            var p = this.grid_snap_points[i];
            temp_points.push(this.gridsnap_add_dot(p));
        }

        var TIME_TO_START_FADE = 500;
        var ALPHA_FADE_TIME = 1000;

        var self = this;
        setTimeout(function(){
            for(var i=0; i<temp_points.length; i++){
                self.game.add.tween(temp_points[i]).to({alpha: 0}, ALPHA_FADE_TIME, Phaser.Easing.Linear.None, true);
            }

            setTimeout(function(){
                for(var i=0; i<temp_points.length; i++) {
                    temp_points[i].destroy();
                }
            }, ALPHA_FADE_TIME);
        }, TIME_TO_START_FADE);
    },

    // Lookup for whether a point is within the list
    gridsnap_is_point_in_list: function(p){
        for(var i=0; i<this.grid_snap_points.length; i++){
            var p1 = this.grid_snap_points[i];
            if (p.x == p1.x && p.y == p1.y) return true;
        }
        return false;
    }
});