/*
 @author Daniel Jackson (dj233)
 */

// Node.js basic server setup

/**
 * Application configuration variables
 */
// Node server port
var PORT = 3000;

// Requirements for starting the node server
var express = require('express');
var app     = express();
var http    = require('http').Server(app);
var io      = require('socket.io')(http);


// All front-end files should be placed in the public directory
app.use(express.static(__dirname));


console.log("Launching server from: "+__dirname);
http.listen(PORT, function(){
    console.log('listening on *:'+PORT);
});