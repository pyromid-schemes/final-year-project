/*
 @author Daniel Jackson (dj233)
 */

var Splash = function(game){
};

Splash.prototype = {

    preload: function(){
        this.game.load.image('splash', 'assets/logo/pyromid-inverted.png');
    },

    create: function() {
        this.logo = this.game.add.image(0, 0, 'splash');

        // Set the logo to the center of the image
        this.logo.pivot.x = 177;
        this.logo.pivot.y = 190;

        // Set the logo to center of the canvas
        this.logo.position.x = 400;
        this.logo.position.y = 300;

        // Make the original scale small
        this.logo.scale.setTo(0.2, 0.2);

        // Add a spin and scale tween
        this.game.add.tween(this.logo).to({angle: 1080}, 2000, Phaser.Easing.Linear.None, true);
        this.game.add.tween(this.logo.scale).to({x: 1.5, y: 1.5}, 2000, Phaser.Easing.Quadratic.In, true);

        // set a timeout to exit to main menu
        var self = this;
        setTimeout(function () {
            self.game.add.tween(self.logo).to({alpha: 0}, 800, Phaser.Easing.Linear.None, true);
            self.game.add.tween(self.logo.scale).to({x: 0, y: 0}, 800, Phaser.Easing.Linear.None, true);

            setTimeout(function () {
                self.goToMain();
            }, 800);
        }, 2000);


        this.game.input.keyboard.onDownCallback = function(e){
            self.goToMain();
        };
    },

    // function to skip to the main game
    startingMain: false,
    goToMain: function(){
        if(!this.startingMain){
            this.state.start('Main');
            this.startingMain = true;
        }
    }
};