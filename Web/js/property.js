
var Property = function(GameObject){
    var game = GameObject;




    game.graphics.beginFill(0x35c148, 1);
    game.graphics.drawRect(game.propertyView.x, game.propertyView.y, game.propertyView.w, game.propertyView.h);
    game.graphics.endFill();
};