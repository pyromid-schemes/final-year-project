module.exports = function(grunt) {
    require('load-grunt-tasks')(grunt); // load all grunt tasks instead of one by one.

    var js_uglify_game = [
        'Web/js/data/tilemaps.js', // Need to compile tilemaps first as other files use it...
        'Web/js/data/*', // then the rest of the data files
        'Web/js/*.js' // finally compile every other game file
    ];
    var js_uglify_networking = [
        'Web/js/sockets/unityClient.js',
        'Web/js/sockets/webSockets.js'
    ];

    grunt.initConfig({
        uglify: {
            build: {
                files: {
                    'Web/js/bin/game.min.js': js_uglify_game,
                    'Web/js/bin/networking.min.js' : js_uglify_networking
                }
            }
        },
        watch: {
            options: {
                spawn: true
            },
            javascript: {
                options: {
                    spawn: false
                },
                files: [
                    js_uglify_game,
                    js_uglify_networking
                ],
                tasks: ['uglify']
            }
        }
    });

    grunt.registerTask('build', ['uglify']);
    grunt.registerTask('default', ['build', 'watch']);
};