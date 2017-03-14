
/** CONSTANTS **/
var MAP_DRAW_OFFSET_X = 0;
var MAP_DRAW_OFFSET_Y = 0;

var SCROLL_SPEED = 0.014;
var SCROLL_MIN = 0.55;
var SCROLL_MAX = 1.7;
var SCROLL_DEFAULT = 0.85;

var TILE_SIZE = 16;

var MAP_MASK = {
    x: 0, y: 0, w: 36 * 16, h: 32 * 16
};

var Main = function(game){
};

Main.prototype = {
    builder: null,

    game_zoom: 1,
    map_group: null,
    game_data: null,
    map_draw_offset: null,
    map_scroll_offset: {x: 0, y: 0},
    map_dimensions: null,
    empty_background: null,

    is_dragging: false,
    drag_current: {x: 0, y: 0},

    /** GHOST ROOM STUFF **/
    currently_selected_tile_type: null,
    ghost_room: null,

    /** GHOST MOB **/
    ghost_mob: null,

    /* Mob data */
    mob_types: [],
    mobs: [],
    mob_id_count: 0,

    preload: function(){
        var self = this;

        this.game.load.image('empty-tile', 'assets/empty-tile.png');
        this.game.load.image('empty-tile64', 'assets/empty-tile64.png');
        this.game.load.image('builder-button-selector', 'assets/selector.png');

        Object.keys(Mobs).forEach(function(k){
            self.preload_mob(Mobs[k]);
        });

        this.game.load.image(Player.sprite.key, Player.sprite.image);

        this.healthbar_preload();
        this.gridsnap_preload();
        this.rooms_preload();
    },
    preload_mob: function(mob){
        this.game.load.image(mob.sprite.key, mob.sprite.image);
        this.game.load.image(mob.builderButton.key, mob.builderButton.image);
    },

    create: function(){
        var graphics = this.game.add.graphics(0, 0);
        graphics.beginFill(0xccccff, 1);
        graphics.drawRect(0, 0, 800, 600);
        graphics.endFill();

        this.create_tiled_background();

        /** DEBUG **/
        this.game_data = document.getElementById('data');
        this.gd2 = document.getElementById('data2');
        this.gd3 = document.getElementById('data3');

        this.game.input.onDown.add(this.onDown, this);
        this.game.input.onUp.add(this.onUp, this);
        this.game.input.addMoveCallback(this.onMove, this);

        var self = this;
        this.game.input.keyboard.onDownCallback = function(e){
            self.keyOnDown(e, self);
        };
        document.getElementById('game').addEventListener("mousewheel", function(e){
            self.mouseWheel(e);
        }, false);

        // Create the builder object
        this.builder = new Builder(this.game, this); this.builder.create();


        this.rooms_create();

        Object.keys(Mobs).forEach(function(k){
            self.addMobType(Mobs[k]);
        });


        // Move the map to the center
        this.scrollMap({x: -15 * 16, y: -16 * 14});
        // Create a dummy room
        this.place_room('room2', 0, 0, 0, false);


        // Setup and move the player to [0,0]
        this.setupPlayer(Player);
        this.setPlayerData({xPos: 0, zPos: 0, currentHealth: 1, maxHealth: 1});


        window.mainScene = this;
    },

    addMobType: function(mob_data){
        this.mob_types[mob_data.id] = mob_data;
    },

    update: function(){
        if(this.ghost_mob != null){
            var tile = this.get_world_xy();

            this.ghost_mob.x = tile.x;
            this.ghost_mob.y = tile.y;
        }
    },

    onDown: function(e){
        if(this.isMouseInsideMap()){
            if(!window.builderScene.is_tile_selected()){
                this.is_dragging = true;
                this.drag_current = {x: this.game.input.activePointer.x, y: this.game.input.activePointer.y};
            }

            // If the map is not dragging - TRY to place a room
            if(!this.is_dragging){
                this.ghostroom_try_to_place();

                if(this.ghost_mob != null){
                    var world = this.get_world_xy();
                    this.place_mob(this.currently_selected_tile_type, world.x, world.y, true);
                }
            }
        }
    },
    onUp: function(e){
        this.is_dragging = false;
    },
    onMove: function(e){
        if(this.is_dragging){
            var pointer = this.game.input.activePointer;
            var diff = {x: this.drag_current.x - pointer.x, y: this.drag_current.y - pointer.y};

            this.drag_current.x = pointer.x;
            this.drag_current.y = pointer.y;

            this.scrollMap(diff);

        }else{
            this.ghostroom_mouse_move();
        }
    },

    // ToDo: remove magic numbers
    scrollMap: function(diff){
        this.map_group.pivot.x += diff.x / this.game_zoom;
        this.map_group.pivot.y += diff.y / this.game_zoom;

        var diff2 = {x: this.map_group.pivot.x - (this.map_dimensions.w / 2), y: this.map_group.pivot.y - (this.map_dimensions.h / 2)};
        var diff3 = {x: Math.floor(diff2.x / 16), y: Math.floor(diff2.y / 16)};

        this.empty_background.pivot.x = (this.map_dimensions.w / 2) - diff3.x*16;
        this.empty_background.pivot.y = (this.map_dimensions.h / 2) - diff3.y*16;
    },

    mouseWheel: function(e){
        var delta = this.game.input.mouse.wheelDelta * -SCROLL_SPEED;
        this.game_zoom = Math.min(SCROLL_MAX, Math.max(SCROLL_MIN, this.game_zoom + delta));

        // Set the 'zoom' on the map
        this.map_group.scale.set(this.game_zoom);
    },

    keyOnDown: function(e, self){
        this.builder.keyOnDown(e);

        this.ghostroom_keyOnDown(e);


        if(e.keyCode == Phaser.Keyboard.D){
            this.gridsnap_show_dots();
        }
    },

    /* Initial setup */
    create_tiled_background: function() {
        var map_w = 100;
        var map_h = 100;
        var empty_tile = this.game.make.sprite(0, 0, 'empty-tile');

        this.map_draw_offset = {x: MAP_DRAW_OFFSET_X, y: MAP_DRAW_OFFSET_Y};
        this.map_dimensions = {x: this.map_draw_offset.x, y: this.map_draw_offset.y, w: map_w * 16, h: map_h * 16};

        this.map_group = this.game.add.group();

        var background = this.game.make.renderTexture((map_w + 1) * 16, (map_h + 1) * 16, 'empty-background');
        for (var x = 0; x < map_w + 1; x++) {
            for (var y = 0; y < map_h + 1; y++) {
                background.renderRawXY(empty_tile, x * 16, y * 16);
            }
        }


        var mask = this.game.add.graphics(this.map_draw_offset.x, this.map_draw_offset.y);
        mask.beginFill(0xffffff);
        mask.drawRect(MAP_MASK.x, MAP_MASK.y, MAP_MASK.w, MAP_MASK.h);
        this.map_group.mask = mask;

        var world_center = {x: MAP_MASK.w , y: MAP_MASK.h };
        this.empty_background = this.game.add.image(world_center.x, world_center.y, background, null, this.map_group);
        this.empty_background.pivot.x = world_center.x;
        this.empty_background.pivot.y = world_center.y;


        var pivot = {x: MAP_MASK.w/2, y: MAP_MASK.h/2};

        this.map_group.x = this.map_draw_offset.x + MAP_MASK.w/2;
        this.map_group.y = this.map_draw_offset.y + MAP_MASK.h/2;
        this.map_group.pivot.x = pivot.x;
        this.map_group.pivot.y = pivot.y;

        this.game_zoom = SCROLL_DEFAULT;
        this.map_group.scale.set(SCROLL_DEFAULT);
    },


    /** Room placing */
    room_selected: function(room_key, placeable){
        this.room_unselected();

        this.currently_selected_tile_type = room_key;

        var placeable_suffix = (placeable ? '-green' : '-red');

        this.ghost_room = this.game.add.image(0, 0, room_key + placeable_suffix, null, this.map_group);
        this.ghost_room.scale.setTo(0.5);
        this.ghost_room.pivot.x = 64;
        this.ghost_room.pivot.y = 64;
    },
    room_unselected: function(){
        if(this.ghost_room != null){
            this.ghost_room.destroy();
            this.ghost_room = null;
            this.currently_selected_tile_type = null;
        }
    },
    place_room: function(room_id, x, y, rot, send_message){
        var room_data = this.room_types[room_id];

        if(room_data == null) throw new Error("Cannot place room with id: "+room_id);


        var img = this.game.add.image(x, y, room_id, null, this.map_group);
        img.scale.set(room_data.scale);
        img.pivot.x = room_data.center.x;
        img.pivot.y = room_data.center.y;

        img.rotation = rot;
        if(this.ghost_room != null) {
            this.room_selected(this.currently_selected_tile_type, false);
            this.ghost_room.rotation = rot;
        }

        var id = this.rooms.length;

        var bb = {
            x1: x - room_data.scaled.cx, y1: y - room_data.scaled.cy,
            x2: x + room_data.scaled.cx, y2: y + room_data.scaled.cy
        };


        this.rooms.push({
            room_id: room_id,
            x: x,
            y: y,
            room_type: room_data,
            room: img,
            bb: bb
        });



        if(send_message){
            var rot =  Utility.webRotToUnityRot(rot);
            Messages.send.buildRoom({objectId: room_id, xPos: x / 16, zPos: y / 16, rot: rot});
        }

        this.redrawPlayer();

        for(var i=0; i<room_data.door_positions.length; i++){
            var door_pos = this.ghostroom_translate_door_pos(room_data.door_positions[i]);
            var pos = {x: x + door_pos.x, y: y + door_pos.y};
            this.gridsnap_add_point(pos);
        }
    },

    // ToDo: Fix the rooms id and array bug...
    removeAllRooms: function(){
        for(var i=0; i<this.rooms.length; i++) this.rooms[i].room.destroy();
        this.rooms = [];
    },

    /** MOB PLACING **/
    mob_selected: function(mob, placeable){
        this.mob_unselected();

        this.currently_selected_tile_type = mob.id;

        this.ghost_mob = this.game.add.image(0, 0, mob.sprite.key, null, this.map_group);
        this.ghost_mob.pivot.x = mob.sprite.size.width/2;
        this.ghost_mob.pivot.y = mob.sprite.size.height/2;

        var size = 16 / mob.sprite.size.width;
        this.ghost_mob.scale.set(size, size);
    },
    mob_unselected: function(){
        if(this.ghost_mob != null){
            this.ghost_mob.destroy();
            this.ghost_mob = null;
            this.currently_selected_tile_type = null;
        }
    },
    place_mob: function(mob_type, x, y, send_message){
        var mob_data = this.mob_types[mob_type];
        if(mob_data == null) throw new Error("No mob data for ["+mob_type+"]");


        // Check mob is inside any rooms bb
        var can_place_mob = false;
        for(var i=0; i<this.rooms.length; i++){
            var r = this.rooms[i];
            if(Utility.isPointWithinBB({x: x, y: y}, r.bb)){
                can_place_mob = true;
                break;
            }
        }

        console.log("can place mob: "+can_place_mob);
        if(can_place_mob == false) return;


        var mob = this.game.add.image(x, y, mob_data.sprite.key, null, this.map_group);
        mob.pivot.x = mob_data.sprite.size.width / 2;
        mob.pivot.y = mob_data.sprite.size.height / 2;

        mob.scale.setTo(mob_data.sprite.scale, mob_data.sprite.scale);


        var healthbar = this.healthbar_add(x, y - 14);

        var mob_instance = {
            mob_id: mob_type,
            x: x,
            y: y,
            mob_type: mob_data,
            mob: mob,
            id: this.mob_id_count++,
            healthbar: healthbar
        };

        this.mobs.push(mob_instance);

        if(send_message){
            Messages.send.placeMob({objectId: mob_data.id, xPos: x/TILE_SIZE, zPos: y/TILE_SIZE, id: mob_instance.id});
        }

        this.builder.startTimeDelay();
        // this.builder.deselectAll();

        return mob_instance;
    },

    /** PLAYER **/
    setupPlayer: function(player){
        var sprite = this.game.add.image(0, 0, player.sprite.key, null, this.map_group);
        sprite.pivot.x = player.sprite.size.half_w;
        sprite.pivot.y = player.sprite.size.half_h;
        sprite.scale.setTo(16/player.sprite.size.height);

        var healthbar = this.healthbar_add(0, 0);

        this.player = {
            sprite: sprite,
            position: {
                x: 0,
                y: 0
            },
            size: player.sprite.size,
            center: player.sprite.center,
            healthbar: healthbar
        };
    },
    setPlayerData: function(msg) {
        this.player.position.x = msg.xPos * TILE_SIZE;
        this.player.position.y = msg.zPos * TILE_SIZE;

        this.player.sprite.position.x = this.player.position.x;
        this.player.sprite.position.y = this.player.position.y;

        this.player.sprite.rotation = Utility.unityRotToWebRot(msg.rot - 180); // -180 temp fix

        this.updatePlayerHealthbar();
        this.updatePlayerHealthbarPercent(msg.currentHealth / msg.maxHealth);
    },

    // ToDo: Should refactor this and the two above to be nicer...
    redrawPlayer: function(){
        if(this.player == null) return;

        var rot = this.player.sprite.rotation;
        var player_key = this.player.sprite.key;
        this.player.sprite.destroy();
        this.player.sprite = this.game.add.image(0, 0, player_key, null, this.map_group);
        this.player.sprite.pivot.x = this.player.size.half_w;
        this.player.sprite.pivot.y = this.player.size.half_h;
        this.player.sprite.position.x = this.player.position.x;
        this.player.sprite.position.y = this.player.position.y;
        this.player.sprite.scale.setTo(16/this.player.size.height);
        this.player.sprite.rotation = rot;

        this.updatePlayerHealthbar();
        this.player.healthbar.redraw();
    },

    updatePlayerHealthbar: function(){
        var pos = {
            x: this.player.sprite.position.x,
            y: this.player.sprite.position.y - 14
        };
        this.player.healthbar.setPosition(pos.x, pos.y);
    },
    updatePlayerHealthbarPercent: function(percentage){
        this.player.healthbar.setPercentage(percentage);
    },

    /**
     * UTILITY
     * **/




    isMouseInsideMap: function() {
        var mouse = this.getAccurateCoords();
        return Utility.isPointWithinRect(mouse, MAP_MASK);
    },
    getAccurateCoords: function(){
        var pointer = this.game.input.activePointer;
        return {x: pointer.x - 1, y: pointer.y - 2};
    },

    // ToDo: make this nicer/more descriptive?
    get_tile_xy: function() {
        var mouse = this.getAccurateCoords();

        /** Actual usable code **/
        var map_relative = {x: mouse.x - this.map_draw_offset.x, y: mouse.y - this.map_draw_offset.y};
        var mr_percent = {x: map_relative.x / MAP_MASK.w , y: map_relative.y / MAP_MASK.h};
        var mrp_true = {x: mr_percent.x - 0.50, y: mr_percent.y - 0.50};


        var cur_map_dim = this.get_cur_map_dim();

        var tile_new = {x: this.map_group.pivot.x + (mrp_true.x * cur_map_dim.w), y: this.map_group.pivot.y + (mrp_true.y * cur_map_dim.h)};
        var tile_xy = {x: Math.floor(tile_new.x / 16) , y: Math.floor(tile_new.y / 16)};

        /** DEBUG **/
        this.gd2.innerHTML = "Tile Global XY["+tile_new.x+","+tile_new.y+"]";
        this.gd3.innerHTML = "mgx/mgy:"+this.map_group.x+","+this.map_group.y+"   mgpx/mgpy:"+this.map_group.pivot.x+","+this.map_group.pivot.y;

        return tile_xy;
    },

    get_world_xy: function(){
        var mouse = this.getAccurateCoords();

        var map_relative = {x: mouse.x - this.map_draw_offset.x, y: mouse.y - this.map_draw_offset.y};
        var mr_percent = {x: map_relative.x / MAP_MASK.w , y: map_relative.y / MAP_MASK.h };
        var mrp_true = {x: mr_percent.x - 0.50, y: mr_percent.y - 0.50};

        var cur_map_dim = this.get_cur_map_dim();

        var tile_new = {x: this.map_group.pivot.x + (mrp_true.x * cur_map_dim.w), y: this.map_group.pivot.y + (mrp_true.y * cur_map_dim.h)};

        return tile_new;
    },

    get_cur_map_dim: function(){
        return {w: MAP_MASK.w / this.game_zoom, h: MAP_MASK.h / this.game_zoom};
    },

    // ToDo: Get mob positions with room positions
    worldStatus: function(data){
        this.removeAllMobs();
        this.removeAllRooms();
        for(var i=0; i<data.length; i++){
            var rot = Utility.unityRotToWebRot(data[i].rot); // converts Unity rotation to correct web rotation
            this.place_room(data[i].objectId, data[i].xPos * TILE_SIZE, data[i].zPos * TILE_SIZE, rot, false);
        }
        this.updatePlayerHealthbarPercent(1);
        this.redrawPlayer();
    },


    /** Mob positions **/
    mobPositions: function(data){
        for(var i=0; i<data.length; i++){
            if(data[i].dead){
                this.deleteMob(data[i].id);
            }else {
                var health_percentage = (data[i].currentHealth / data[i].maxHealth);
                this.updateMob(data[i].id, data[i].objectId, {x: data[i].xPos * TILE_SIZE, y: data[i].zPos * TILE_SIZE}, data[i].rot, health_percentage);
            }
        }
    },
    updateMob: function(id, type, pos, rot, hp){
        var index = this.findMobIndex(id);
        if( index == -1) { // place mob
            var mob = this.place_mob(type, pos.x, pos.y, false);
            mob.id = id;
        }else {
            var mob = this.mobs[index];
            mob.mob.position.x = pos.x;
            mob.mob.position.y = pos.y;
            mob.mob.rotation = Utility.unityRotToWebRot(rot - 180);

            pos.y -= 14;

            mob.healthbar.setPercentage(hp);
            mob.healthbar.setPosition(pos.x, pos.y);
            mob.healthbar.redraw();
        }
    },
    deleteMob: function(id){
        console.log("deleteMob (id = "+id+")");
        var index = this.findMobIndex(id);
        if( index == -1 ) return null;

        this.mobs[index].healthbar.destroy();
        this.mobs[index].mob.destroy();
        this.mobs.splice(index, 1);
    },
    findMobIndex: function(id){
        for(var i=0; i<this.mobs.length; i++){
            if(this.mobs[i].id == id) return i;
        }
        return -1;
    },
    removeAllMobs: function(){
        for(var i=0; i<this.mobs.length; i++) {
            this.mobs[i].healthbar.destroy();
            this.mobs[i].mob.destroy();
        }
        this.mobs = [];
    }
};
main = Main.prototype; // Set 'window.main' to the whole Main object #JS #Singletons #BestCodePractice #Globals

Main.appendPrototype = function(src) {
    for (var prop in src) {
        this.prototype[prop] = src[prop];
    }
};
