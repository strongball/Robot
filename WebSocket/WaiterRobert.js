var ConnectSocket = require('./ConnectSocket.js')

var __IdCounter = 0;
function WaiterRobert(connection){
	var self = this;

	this.id = __IdCounter++;
	this.table = "unset";
	this.power = 0;
	this.mood = 0;
	this.service = false;
    this.socket = new ConnectSocket(connection);

	this.orderId = 0;
    this.orderList = [];
	this.dataListener = []

	this.getStatus = function(){
		return {
			id: self.id,
			table: self.table,
			power: self.power,
			mood: self.mood,
			service: self.service,
			orderList: self.orderList
		}
	}

	this.socket.on('Service', function(data){
		self.service = true;
		self.callDataListener();
	});

	this.socket.on('Emotion', function(data){
		self.mood = data;
		self.callDataListener();
	});

	this.socket.on('Power', function(data){
		self.power = data;
		self.callDataListener();
	});

	this.socket.on('order', function(data){
		self.orderList.push({
			id: self.orderId++,
			name: data.meal,
			status: "prepare"
		});
		self.callDataListener();
	});

	this.socket.on('setTable', function (data) {
		self.table = data;
		self.callDataListener();
	});

	this.socket.on('CheckOrder', function (data) {
		self.socket.send("OrderList", self.orderList);
	});
}

WaiterRobert.prototype.serviceDone = function () {
	this.service = false;
	this.callDataListener();
}

WaiterRobert.prototype.changeOrder = function (mealId, status) {
	var self = this;
	this.orderList.forEach(function(order) {
		if(order.id == mealId){
			order.status = status;
			self.callDataListener();
		}
	})
}

WaiterRobert.prototype.onDataChange = function (fn) {
	this.dataListener.push(fn);
	fn(this.getStatus());
}

WaiterRobert.prototype.callDataListener = function () {
	var self = this;
	this.dataListener.forEach(function(fn) {
		fn(self.getStatus());
	})
}

WaiterRobert.prototype.sendOrderList = function () {

}

module.exports = WaiterRobert;