var UnityClient = {
	"buildCommand": function(objectId, xPos, zPos) {
		var command = {
			"command": "build",
			"options": {
				"objectId": objectId,
				"xPos": xPos,
				"zPos": zPos
			}
		};
		sendMessage(JSON.stringify(command));
	}
};
