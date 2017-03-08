
var main = Main.prototype;

Main.appendPrototype({
    healthbar_preload: function(){
        this.game.load.image('healthbar-green', 'assets/healthbar/green.png');
        this.game.load.image('healthbar-red', 'assets/healthbar/red.png');
    },

    healthbar_add: function(x, y){
        return new Healthbar(this, x, y);
    }
});

var BASE_SCALE = 0.07;

var FULL_BAR_DEPLETION_TIME = 1000; //in ms

var Healthbar = function(main, x, y){

    var red = null;
    var green = null;

    var pivot = {x: 128, y: 16};
    var pos = {x: x, y: y};
    var GREEN_SCALE = BASE_SCALE;

    var can_update = true;

    redraw();

    function setPosition(x, y){
        pos = {x: x, y: y};
        green.position.x = x; green.position.y = y;
        red.position.x = x; red.position.y = y;
    }

    function setPercentage(percentage){
        if( !can_update ) return;

        var travel = (green.scale.x - (BASE_SCALE * percentage)) * FULL_BAR_DEPLETION_TIME / BASE_SCALE;
        if( travel >= 0 && travel < 0.0000001 ) return;

        GREEN_SCALE = BASE_SCALE * percentage;
        main.game.add.tween(green.scale).to({x: GREEN_SCALE, y: BASE_SCALE}, travel, Phaser.Easing.Linear.None, true);

        can_update = false;
        setTimeout(function(){
            can_update = true;
        }, travel);
    }


    function create_bar(pos, sprite_key, pivot, scale){
        var img = main.game.add.image(pos.x, pos.y, sprite_key, null, main.map_group);
        img.pivot.x = pivot.x;
        img.pivot.y = pivot.y;
        img.scale.set(scale, BASE_SCALE);
        return img;
    }

    function redraw(){
        destroy();

        red = create_bar(pos, 'healthbar-red', pivot, BASE_SCALE);
        green = create_bar(pos, 'healthbar-green', pivot, GREEN_SCALE);
    }

    function destroy(){
        if(red != null) red.destroy();
        if(green != null) green.destroy();
    }

    return {
        setPosition: setPosition,
        setPercentage: setPercentage,
        redraw: redraw,
        destroy: destroy
    }
};