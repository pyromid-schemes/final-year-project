
var Property = {
    // A parent object of the game
    game: null,

    create: function(GameObject){
        //Reference the main game object
        this.game = GameObject;

        // Set the background colour for the section
        game.graphics.beginFill(0x35c148, 1);
        game.graphics.drawRect(game.propertyView.x, game.propertyView.y, game.propertyView.w, game.propertyView.h);
        game.graphics.endFill();
    }
};