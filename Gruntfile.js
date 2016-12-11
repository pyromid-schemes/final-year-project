module.exports = function(grunt) {
    grunt.initConfig({
        uglify: {
            build: {
                files: {
                    'Web/js/bin/game.min.js': [
                        'Web/js/data/tilemaps.js', // Need to compile tilemaps first as other files use it...
                        'Web/js/data/*', // then the rest of the data files
                        'Web/js/*.js' // finally compile every other game file
                    ],
                    'Web/js/bin/networking.min.js' : [
                        'Web/js/sockets/unityClient.js',
                        'Web/js/sockets/webSockets.js'
                    ]
                }
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-uglify');

    grunt.registerTask('build', ['uglify']);
    grunt.registerTask('default', ['build']); //, 'watch']);
};