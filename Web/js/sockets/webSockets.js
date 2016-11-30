var ws;
var readyToSend = false;

/**
* @config Object that contains the onOpen, onMessage and onClose functions
* that will be used for this WebSocket.
*/
function openWebSocket(ipAddress, config) {
	ws = new WebSocket("ws://" + ipAddress + ":9998");
	ws.onopen = function() {
		readyToSend = true;
		config.onOpen();
	};
	ws.onmessage = function(evt) {
		var reader = new FileReader();
		reader.addEventListener("loadend", function(e) {
			var msg = JSON.parse(e.target.result.substring(1));
			if (msg.hasOwnProperty("command")) {
				switch (msg.command) {
					case "worldStatus":
						config.onMessage.worldStatus(msg.objects);
						break;
				}
			}
		});
		reader.readAsText(evt.data);
	};
	ws.onclose = function(onClose) {
		readyToSend = false;
		config.onClose();
	};
}

function closeWebSocket() {
	ws.close();
}

function sendMessage(msg) {
	if (readyToSend) {
		ws.send(" " + msg);
	}
}
