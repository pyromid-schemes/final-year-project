/**
 * Message sending
 * **/
var Messages = {
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
    send: {
        buildRoom: function(data){
            Utility.debug("send.buildRoom", data);
            UnityClient.buildCommand(data.objectId, data.xPos, data.zPos, data.rot);
        },
        placeMob: function(data){
            Utility.debug("send.placeMob", data);
            UnityClient.spawnMobCommand(data.objectId, data.xPos, data.zPos, data.id);
        }
    }
};