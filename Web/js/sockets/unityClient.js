var UnityClient = {
	"buildCommand": function(objectId, xPos, zPos, rot) {
		var command = {
			"command": "build",
			"options": {
				"objectId": objectId,
				"xPos": xPos,
				"zPos": zPos,
				"rot": rot
			}
		};
		sendMessage(JSON.stringify(command));
	},
	"spawnMobCommand": function(objectId, xPos, zPos, id) {
        var command = {
            "command": "spawnMob",
            "options": {
                "objectId": objectId,
                "xPos": xPos,
                "zPos": zPos,
                "id": id
            }
        };
        sendMessage(JSON.stringify(command));
    }
};
