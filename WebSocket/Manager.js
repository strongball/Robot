var ConnectSocket = require('./ConnectSocket.js')

function Manager(connection, ServerStatus){
	var self = this;
    this.serverStatus = ServerStatus;
    this.socket = new ConnectSocket(connection);

    // this.interval = setInterval(function() {
    //    self.socket.send("updata", self.ServerStatus.getWaiterStatus());
    // }, 1000);
    this.socket.send("AllWaiters", this.serverStatus.getWaiterStatus());

	this.socket.on('OrderChange', function(data){
        self.serverStatus.changeOrder(data.waiterId, data.mealId, data.status);
	});

    this.socket.on('ReceiveService', function(data){
        self.serverStatus.serviceDone(data.id);
	});

    this.socket.onClose(function () {
        //clearInterval(self.interval);
    });
}

module.exports = Manager;