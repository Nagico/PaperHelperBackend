var ShareDB = require("sharedb");
var WebSocket = require("ws");
var WebSocketStream = require("websocket-push-stream");

var redisHost = process.env.REDIS_HOST || "localhost";
var redisPort = process.env.REDIS_PORT || 6379;
var mongoHost = process.env.MONGO_HOST || "localhost";
var mongoPort = process.env.MONGO_PORT || 27017;

var RedisClient = require("redis").createClient({url: `redis://${redisHost}:${redisPort}`});
RedisClient.on("error", function (err) {
  console.log("Error " + err);
});


var redisPubsub = require('sharedb-redis-pubsub')({client: RedisClient});
const db = require('sharedb-mongo')(`mongodb://${mongoHost}:${mongoPort}/sharedb`);
var otJson = require("ot-json1");
// ShareDB 可以支持多种 OT 类型，例如 JSON 文本，普通文本，富文本等
// 具体文档可以查看 https://github.com/ottypes/docs
// 这里使用普通文本类型 ot-text
ShareDB.types.register(otJson.type);

var backend = new ShareDB({
  db: db,  // db would be your mongo db or other storage location
  pubsub: redisPubsub
});
// 监听 9090 端口
var wss = new WebSocket.Server({ port: 9090 }, () => {
  console.log("WebSocket Server Created at 9090.");
});

// 监听新的客户端 WebSocket 链接，对新的链接进行封装，交给 ShareDB 处理
wss.on("connection", function (ws) {
  // ShareDB 使用 stream 来与客户端实现通信，从而可以忽略底层的传输方式
  // 这里使用 WebSocket 作为底层的传输方式，保证实时数据传输
  let stream = new WebSocketStream(ws);
  // ShareDB 的前后端通信的数据以 JSON 格式传输
  // 服务端收到数据后，需要将数据从 JSON 转换为 Object
  // 并通过放入 stream 的缓存区 (push)，交给 ShareDB 处理
  ws.on("message", function (msg) {
    // 可以在这里对 msg 进行筛选，自定义的数据可以另行处理，不交给 ShareDB 处理
    console.log(JSON.parse(msg));
    stream.push(JSON.parse(msg));
  });
  // 使 ShareDB 监听新的链接
  backend.listen(stream);
});
