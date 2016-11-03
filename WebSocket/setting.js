var os=require('os')

function setting (){
    this.ip = getIP();
    this.port = 8001;
    this.publicPort = 80;
    this.dir = "./public";
    this.imgDir = "/img/";
}

function getIP() {
    var ifaces=os.networkInterfaces();
    for (var dev in ifaces){
        for(var i = 0; i < ifaces[dev].length; i++ ){
            var details = ifaces[dev][i];
            if (details.family=='IPv4' && details.address != "127.0.0.1") {
                return details.address;
            }
        }
    }
}

module.exports = new setting();