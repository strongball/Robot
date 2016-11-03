function BattleQueue() {
    this.waitlist = [];
}

BattleQueue.prototype.add = function (player) {
    for(var i = 0; i < this.waitlist.length; i++){
        if(this.waitlist[i] === player){
            return;
        }
    }

    this.waitlist.push(player);
    if(this.waitlist.length >= 2){
        this.Paired();
    }
    console.log((new Date()) + ' BattleQueue: ' + this.waitlist.length + ' players.');
}

BattleQueue.prototype.remove = function (player) {
    var self = this;
    this.waitlist.forEach(function (ele, i) {  
        if(ele === player){
            self.waitlist.splice(i, 1);
            return;
        }
    });
}

BattleQueue.prototype.Paired = function () {
    new BattleManager(this.waitlist.shift(), this.waitlist.shift());
    console.log((new Date()) + ' Some one start game ');
}

function BattleManager(player1, player2) {
    this.p1 = player1;
    this.p2 = player2;

    this.p1.init("p1", this);
    this.p2.init("p2", this);

    //this.boardCastHPMP();
}

BattleManager.prototype.boardCastHPMP = function () {
    var data = {
        p1:{
            HP: this.p1.HP,
            MP: this.p1.MP
        },
        p2:{
            HP: this.p2.HP,
            MP: this.p2.MP
        }
    }

    if(data.p1.HP && data.p2.HP){
        this.p1.socket.send("UpdataHPMP", data);
        this.p2.socket.send("UpdataHPMP", data);
    }
}

BattleManager.prototype.onHit = function (player) {
    var attacker;
    var defancer;

    if(player == this.p1.playerNumber){
        attacker = this.p2;
        defancer = this.p1;
    }else{
        attacker = this.p1;
        defancer = this.p2;
    }
	
    defancer.HP -= attacker.attack;

    if(defancer.HP <= 0){
        this.EndGame(attacker);
    }

    this.boardCastHPMP();
}

BattleManager.prototype.EndGame = function (winner) {
    this.p1.socket.send("EndGame", winner.playerNumber);
    this.p2.socket.send("EndGame", winner.playerNumber);
    delete this;
}

module.exports = BattleQueue;
