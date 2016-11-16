var UnityClient = {
	"buildCommand": function(objectName) {
		var command = {
			"command": "build",
			"options": {
				"objectName": objectName
			}
		};
		sendMessage(JSON.stringify(command));
	}
};
