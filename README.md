# <NAME>
## A prototype by Pyromid Schemes

### Networking API

#### webSockets.js

This is the "low-level" connection API for using websockets.
- `openWebSocket(config)`: opens a websocket connection to the url specified in an `<input id="url" type="text">` on port 9998.
  - `config`: is used to configure functions to be called on certain events.
  ```javascript
  var config = {
 	 "onOpen": function() { console.log("Websocket open!"); },
 	 "onMessage": {
 		 "worldStatus": function(msg) { // is an array of rooms spawned in the Unity world
 			 // msg[i].objectId // msg[i].xPos // msg[i].zPos
 		 },
 		 "vrPosition": function(msg) { // has the xPos and zPos of the VR player at 30Hz
 			 // msg.xPos // msg.zPos
 		 }
 	 },
 	 "onClose": function() { console.log("Websocket closed!"); }
  }
  ```
- `closeWebSocket()`: cleanly terminates the websocket connection.

#### unityClient.js

This builds on top of `webSockets.js` and provides helpful methods for sending different types of commands.
- `UnityClient.buildCommand(objectId, xPos, zPos)`: sends a correctly formatted buildCommand to Unity for an object of prefab type `objectId` at position (`xPos`, `zPos`).
- `UnityClient.spawnMobCommand(objectId, xPos, zPos, id)`: sends a correctly formatted spawnMobCommand to Unity to spawn a mob associated to `id` using prefab type `objectId` at (`xPos`, `zPos`).
