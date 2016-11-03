var ConnectSocket = require('./ConnectSocket.js')
var MAX_HP = 100;
var MAX_MP = 100;
var BASE_ATTACK = 10;
function BattlePad(connection, queue) {
    var self = this;
    this.battleManager;
    this.playerNumber;
    this.HP;
    this.MP;
    this.attack;
    this.socket = new ConnectSocket(connection);

    this.socket.on("Ready", function () {
        queue.add(self);
    });
    
    this.socket.on("Setting", function (data) {
        self.HP = data.maxHP;
        self.MP = data.maxMP;
        self.attack = data.attack;
         self.battleManager.boardCastHPMP();
    });

    this.socket.on("GetDamage", function () {
        if(self.battleManager){
            self.battleManager.onHit(self.playerNumber);
        }
    });
    
    this.socket.on("UseSkill", function (data) {
        if(self.battleManager){
            self.MP -= data.MP;
            if(data.name == "PowerUp"){
                self.attack = self.attack * data.powerValue;	
                setTimeout(function () {
                    self.attack = self.attack /  data.powerValue;
                }, data.skillTime);
            }
            self.battleManager.boardCastHPMP();
        }
    });

    this.socket.onClose(function () {
        queue.remove(self);
    });
}

BattlePad.prototype.init = function (playerNumber, manager) {
    this.playerNumber = playerNumber;
    this.battleManager = manager;

    this.socket.send("OnPair", this.playerNumber);
}

module.exports = BattlePad;
