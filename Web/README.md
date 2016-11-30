# Final year - Web frontend

## Install 

If npm is installed run the following from the root:
```
npm install
```

This should install socket io, express and a bunch of gruntfile components

## Make changes to the code
You may need to install the Grunt command line interface to run the grunt tasks (Only run this if `grunt build` doesn't work)
```
npm install -g grunt-cli
```

If you make a change to a JS file inside Web you will need to recompile the `/bin/game.min.js`  

Run this command from anywhere in the directory:
```
grunt build
```

## Run 

If node is installed (and all changes have been compiled) run the following from the root:
```
node Web/server.js
```

You should then be able to go to `localhost:3000` in your browser and it will run the web frontend