var app = require('express')();
var server = require('http').createServer();
var WebSocketServer = require('websocket').server;

var bodyParser = require('body-parser');
var busboy = require('connect-busboy');

var url = require("url");
var fs = require('fs');

var ServerStatus = require('./ServerStatus.js');
var WaiterRobert = require('./WaiterRobert.js');
var Manager = require('./Manager.js');
var setting = require('./setting.js');

var BattleQueue = require('./BattleQueue.js');
var BattlePad = require('./BattlePad.js');

app.listen(setting.publicPort, function () {
	console.log((new Date()) + " PublicServer Open On: ", setting.ip + ":" + setting.publicPort);
});
// parse application/x-www-form-urlencoded
app.use(bodyParser.urlencoded({ extended: false }))
app.use(busboy());
// parse application/json
app.use(bodyParser.json())

app.post('/upload', function(req, res) {
	// console.log(req);
    req.pipe(req.busboy);
    req.busboy.on('file', function (fieldname, file, filename) {
		console.log((new Date()) + " Uploading: " + filename);
        //Path where image will be uploaded
        fstream = fs.createWriteStream(setting.dir + setting.imgDir + filename);
        file.pipe(fstream);
        fstream.on('close', function () {   
        	res.writeHead(200, {'Content-Type': 'text/html'}); 
        	res.end(setting.ip + ":" + setting.port+"/"+filename);
        });
    });
});
app.get('/',function(request, response){ //我們要處理URL為 "/" 的HTTP GET請求
	console.log((new Date()) + ' Request ');
	fs.readFile(setting.dir+"/frame/socket.html", function(error, data) {
		if (error){
			response.writeHead(404);
			response.write("opps this doesn't exist - 404");
		} else {
			response.writeHead(200, {"Content-Type": "text/html"});
			response.write(data, "utf8");
		}
		response.end();
	});
});


app.get('/:filename', function (req, res) {
	var options = {
		root: setting.dir + setting.imgDir,
		dotfiles: 'deny',
		headers: {
			'x-timestamp': Date.now(),
			'x-sent': true
		}
	};
	var fileName = req.params.filename;
	res.sendFile(fileName, options, function (err) {
		if (err) {
			res.status(err.status).end();
			console.log((new Date()) + ' Error ' + err);
		}
		else {
			console.log((new Date()) + ' Sent ' + fileName);
		}
	});
});
app.get('/download/:filename', function (req, res) {
	var options = {
		root: setting.dir + setting.imgDir,
		dotfiles: 'deny',
		headers: {
			'x-timestamp': Date.now(),
			'x-sent': true,
			"Content-Type": "application/octet-stream"
		}
	};
	var fileName = req.params.filename;
	res.sendFile(fileName, options, function (err) {
		if (err) {
			res.status(err.status).end();
			console.log((new Date()) + ' Error ' + err);
		}
		else {
			console.log((new Date()) + ' Sent ' + fileName);
		}
	});
});



var serverStatus = new ServerStatus();
var battleQueue = new BattleQueue();
server.listen(setting.port,function(){
	console.log((new Date()) + " WebSocketServer Open On: ", setting.ip + ":" + setting.port);
	//consoleManage();
});

wsServer = new WebSocketServer({
    httpServer: server,
	maxReceivedMessageSize: 1024*1024*10,
    autoAcceptConnections: false
});
 
function originIsAllowed(origin) {
  // put logic here to detect whether the specified origin is allowed. 
  return true;
}

wsServer.on('request', function(request) {
    if (!originIsAllowed(request.origin)) {
      // Make sure we only accept requests from an allowed origin 
      request.reject();
      console.log((new Date()) + ' Connection from origin ' + request.origin + ' rejected.');
      return;
    }
    console.log((new Date()) + ' Request Protocols ' + request.requestedProtocols);

    request.requestedProtocols.forEach(function(e) {
        if(e == "waiterrobot"){
            var connection = request.accept(e, request.origin);
            console.log((new Date()) + ' Accept Protocols ' + e);
            serverStatus.addWaiter(new WaiterRobert(connection));
        }else if(e == "manager"){
			var connection = request.accept(e, request.origin);
			console.log((new Date()) + ' Accept Protocols ' + e);
            serverStatus.addManager(new Manager(connection,serverStatus));
		}else if(e == "battle"){
			var connection = request.accept(e, request.origin);
			console.log((new Date()) + ' Accept Protocols ' + e);
			new BattlePad(connection, battleQueue);
		}

    });
});


// const readline = require('readline');
// const rl = readline.createInterface({
// 	input: process.stdin,
// 	output: process.stdout
// });


function printStatus(){
	console.log("WaiterRobert:");
	var datas = serverStatus.getWaiterStatus();

	datas.forEach(function(data, index){
		var str = "name: "  + data.name
				+ " | mood: "  + data.mood
				+ " | power: "  + data.power;
		console.log(str);
	});
}

function consoleManage(){
	rl.question('*************Watch info ? ', function(){
		printStatus();
		consoleManage();
	});
}