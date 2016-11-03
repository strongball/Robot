var fs = require('fs');
var setting = require('./setting.js');

function ConnectSocket(socket){
    var self = this;
    this.socket = socket;
    this.eventListener = [];
    this.onDisconnect = [];

    this.socket.on('message', function(message) {
        if (message.type === 'utf8') {
            var obj = JSON.parse( message.utf8Data);
            self.eventListener.forEach(function(ele) {
                if(ele.event == obj.event){      
                    ele.callback(obj.data);
                }
            })
        }
        else if (message.type === 'binary') {
            console.log(new Date() + ' Received Binary Message of ' + message.binaryData.length + ' bytes');
            var filename = new Date().getTime() +".png"
            fs.writeFile(setting.dir + setting.imgDir + filename, message.binaryData);
            if(setting.publicPort == 80){
                
                self.send("upload", setting.ip+"/download/"+filename);
            }else{
                
                self.send("upload", setting.ip+":"+setting.publicPort+"/download/"+filename);
            }
            //connection.sendBytes(message.binaryData);
        }
    });

    this.socket.on('close', function(reasonCode, description) {
        console.log((new Date()) + ' Peer ' + self.socket.remoteAddress + ' disconnected.' + reasonCode);
        self.onDisconnect.forEach(function(fn){
            fn();
        });
    });
}

ConnectSocket.prototype.on = function (event, callback) {
    this.eventListener.push({
        event: event,
        callback: callback
    })
}

ConnectSocket.prototype.send = function (event, data) {
    this.socket.send(JSON.stringify({
        event: event,
        data: data
    }));
}

ConnectSocket.prototype.onClose = function (callback) {
    this.onDisconnect.push(callback);
}

module.exports = ConnectSocket;