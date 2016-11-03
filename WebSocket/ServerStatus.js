function ServerStatus(){
	var self = this;
	this.waiters = []
    this.managers = []

	this.addManager = function(m){
		this.managers.push(m);
        m.socket.onClose(function () {
            self.delManager(m);
        });
	}

	this.delManager = function(m){
		this.managers.forEach(function(val, i, array){
			if(val == m){
				self.managers.splice(i, 1);
			}
		});
	}

	this.addWaiter = function(w){
		this.waiters.push(w);
		w.onDataChange(function (data) {
			self.boardcastManager("Updata", data);
		});

        w.socket.onClose(function () {
            self.delWaiter(w);
        });
	}

	this.delWaiter = function(w){
		this.waiters.forEach(function(val, i, array){
			if(val == w){
				self.waiters.splice(i, 1);
				self.boardcastManager("DelWaiter", val.getStatus());
			}
		});
	}

	this.getWaiterStatus = function(){
		var data = [];
		for (var i = 0; i < self.waiters.length; i++) {
			data.push(self.waiters[i].getStatus());
		}
		return data;
	}

	this.boardcast = function(event, data){
		this.waiters.forEach(function(val, i, array){
			val.socket.send(event, data);
		});
	}

	this.boardcastManager = function(event, data){
		this.managers.forEach(function(val, i, array){
			val.socket.send(event, data);
		});
	}
}

ServerStatus.prototype.onChange = function () {
    this.managers.forEach(function(val, i, array){
        val.onChange();
    });
};

ServerStatus.prototype.changeOrder = function (waiterId, mealId, status) {
    this.waiters.forEach(function(w, i, array){
        if(w.id == waiterId){
			w.changeOrder(mealId, status);
		}
    });
};

ServerStatus.prototype.serviceDone = function (waiterId) {
    this.waiters.forEach(function(w, i, array){
        if(w.id == waiterId){
			w.serviceDone();
		}
    });
};

module.exports = ServerStatus;