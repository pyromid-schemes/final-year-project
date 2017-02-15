/**
 * Message sending
 * **/
var Messages = {
    receive: {
        worldStatus: function(data){
            window.mainScene.worldStatus(data);
        },
        vrPositionUpdate: function(data){
            window.mainScene.setPlayerPosition(data);
        },
        mobPositions: function(data){
            Utility.debug("mob positions", data);
            main.mobPositions(data);
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