
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
        game.phaser.load.image('builder-button-lava5x3', 'assets/buttons/lava-5x3.png');
        game.phaser.load.image('builder-button-chengy-door-up', 'assets/buttons/chengy-room-door-up.png');
        // Selector
        game.phaser.load.image('builder-button-selector', 'assets/buttons/selector.png');


        /** World view section */
        // Tiles
        game.phaser.load.image('tile-empty-16', 'assets/buttons/empty-tile-16.png');
        // Tilemaps
        game.phaser.load.atlas('tilemap-chengy', 'assets/tilemaps/chengy-room.png', 'assets/tilemaps/tilemap.json');
        game.phaser.load.atlas('tilemap-lava', 'assets/tilemaps/lava-room.png', 'assets/tilemaps/tilemap.json');

    },

    create: function(){
        game.phaser.canvas.oncontextmenu = function (e) { e.preventDefault(); };

        // Initialize the graphics object for drawing things
        var graphics = game.phaser.add.graphics(0, 0);
        window.graphics = graphics;
        game.graphics = graphics;

        // Setup separate viewports
        game.mapView = {x: 0, y: 0, w: 550, h: 500};
        game.builderView = {x: 550, y: 0, w: 250, h: 600};
        game.propertyView = {x:0, y: 500, w: 550, h: 100};

        // Setup object classes
        game.mapObject = Map; game.mapObject.create(game);
        game.builderObject = Builder; game.builderObject.create(game);
        game.propertyObject = new Property(game);

        // Input callbacks
        game.phaser.input.onDown.add(game.onDown, this);
        game.phaser.input.onUp.add(game.onUp, this);
        game.phaser.input.addMoveCallback(game.onMove, this);
        game.phaser.input.addMoveCallback(game.onMove, this);
    },

    // On update
    update: function() {
    },

    // On render
    render: function(){
    },

    // On mouse press
    onDown: function(e){
        var mouseCoords = getAccurateCoords(game.phaser.input.activePointer);

        // If the click is within the mapView - delegate to the Map object
        if(isPointWithinRect(mouseCoords, game.mapView)){
            game.mapObject.onDown(e);
        }
    },
    // On mouse press released
    onUp: function(e){
        game.mapObject.onUp(e);
    },
    // On mouse moved
    onMove: function(e){
        game.mapObject.onMove(e);
    },

    // Sends a message to the Unity client
    sendMessage: function(objectId, xPos, yPos){
        UnityClient.buildCommand(objectId, xPos, yPos);
    }
};
game.start();

//So... phaser seems to have a dodge mouse-to-coordinate system - this corrects that
function getAccurateCoords(pointer){
    return {x: pointer.x - 1, y: pointer.y - 2};
}
//This checks whether a point is within a rect
function isPointWithinRect(point, rect){
    return ( point.x >= rect.x && point.y >= rect.y && point.x < rect.x + rect.w && point.y < rect.y + rect.h );
}
