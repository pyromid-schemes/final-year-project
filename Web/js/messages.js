/*
 @author Daniel Jackson (dj233)
 */

/**
 * Message sending
 * **/
var Messages = {
    // A set of functions to be called when receiving data from Unity
    receive: {
        worldStatus: function(data){
            window.mainScene.worldStatus(data);
        },
        vrPositionUpdate: function(data){
            window.mainScene.setPlayerData(data);
        },
        mobPositions: function(data){
            window.mainScene.mobPositions(data);
        }
    },
    // A set of functions to be called with data to send to Unity
    send: {
        buildRoom: function(data){
            UnityClient.buildCommand(data.objectId, data.xPos, data.zPos, data.rot);
        },
        placeMob: function(data){
            UnityClient.spawnMobCommand(data.objectId, data.xPos, data.zPos, data.id);
        }
    }
};