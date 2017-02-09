var Splash = function(game){
};

Splash.prototype = {

    preload: function(){
        this.game.load.image('splash', 'assets/logo/pyromid.png');
    },

    create: function(){
        this.game.add.image(0, 0, 'splash');

        var self = this;
        setTimeout(function(){
            self.state.start('Main');
        }, 1000);
    }
};