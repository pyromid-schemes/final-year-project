
var game = {
    phaser: null,
    start: function() {
        this.phaser = new Phaser.Game(800, 600, Phaser.AUTO, 'game', {
            preload: this.preload,
            create: this.create,
            update: this.update,
            render: this.render
        });
    },

    mapObject: null,
    builderObject: null,
    propertyObject: null,


    mapView: null,
    builderView: null,
    propertyView: null,

    preload: function() {
        /** Builder view section */
        // Buttons
        game.phaser.load.image('builder-button-chengy', 'assets/buttons/button4.png');
        game.phaser.load.image('builder-button-chengy4doors', 'assets/buttons/chengy-room-4doors.png');
        // Selector
        game.phaser.load.image('builder-button-selector', 'assets/buttons/selector.png');


        /** World view section */
        // Tiles
        game.phaser.load.image('tile-empty-16', 'assets/buttons/empty-tile-16.png');
        // Tilemaps
        game.phaser.load.atlas('tilemap-chengy', 'assets/tilemaps/chengy-room.png', 'assets/tilemaps/tilemap.json');

    },

    create: function(){
        game.phaser.canvas.oncontextmenu = function (e) { e.preventDefault(); };

        // Initialize the graphics object for drawing things
        var graphics = game.phaser.add.graphics(0, 0);
        window.graphics = graphics;
        game.graphics = graphics;


        game.mapView = {x: 0, y: 0, w: 550, h: 500};
        game.builderView = {x: 550, y: 0, w: 250, h: 600};
        game.propertyView = {x:0, y: 500, w: 550, h: 100};

        // Setup object classes
        game.mapObject = Map; game.mapObject.create(game);//new Map(game);
        game.builderObject = Builder; game.builderObject.create(game);//new Builder(game);
        game.propertyObject = new Property(game);


        game.phaser.input.onDown.add(onDown, this);
        game.phaser.input.onUp.add(onUp, this);
        game.phaser.input.addMoveCallback(onMove, this);
        // game.phaser.input.

    },

    update: function() {
    },

    render: function(){
    },


    sendMessage: function(objectId, xPos, yPos){
        UnityClient.buildCommand(objectId, xPos, yPos);
    }
};
game.start();




/*
    builder_view ->
        button 1 ->
            makes room 1 -> tilemap_1
            makes 3x3 of '0' in the map_data




    [...000..]
    [11.000..]
    [11.000..]
    [11......]

    rooms[0] = [3x3, room_1] @ [x: 3, y: 0, w: 3, h: 3]
    rooms[1] = [2x3, room_2]
 */





function onDown(e){
    var mouseCoords = getAccurateCoords(game.phaser.input.activePointer);

    if(isPointWithinRect(mouseCoords, game.mapView)){
        game.mapObject.onDown(e);
    }
}

function onUp(e){
    game.mapObject.onUp(e);
}

function onMove(e){
    game.mapObject.onMove(e);
}

function getAccurateCoords(pointer){
    return {x: pointer.x - 1, y: pointer.y - 2};
}
function isPointWithinRect(point, rect){
    return ( point.x >= rect.x && point.y >= rect.y && point.x < rect.x + rect.w && point.y < rect.y + rect.h );
}









