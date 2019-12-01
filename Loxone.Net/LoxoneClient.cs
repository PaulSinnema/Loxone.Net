using Loxone.Api;
using Loxone.Api.Data.Message;
using Loxone.Net.Data;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Loxone.Net {
	public class LoxoneClient : IDisposable {

		private readonly string _serverIp;
		private readonly int _port;

		private string _userName;
		private string _passWord;
		private string _credentials;

		private string _publicKey;


		private LoxoneMiniserverConnection _connection;

		public LoxoneClient(string serverIp, int port = 80) {

			string[] parts = serverIp.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

			_serverIp = parts[0];
			_port = port;

			if (parts.Length == 2) {
				int prt;
				if (int.TryParse(parts[1], out prt)) {
					_port = prt;
				}
			}
		}

		public static string GetIP(string serialNr) {
			System.Net.WebClient client = new System.Net.WebClient();
			string response = client.DownloadString($"http://dns.loxonecloud.com/?getip&snr={serialNr}&json=true");
			DnsRecord rec = JsonConvert.DeserializeObject<DnsRecord>(response);
			return rec.IP;
		}


		public void Dispose() {
			if (_connection != null) {
				this.Close();
				_connection = null;
			}
		}

		private LoxoneData _data = null;
		public LoxoneData Data {
			get {
				if (_data == null) _data = new LoxoneData(this);
				return _data;
			}
		}

		public bool IsOpen() {
			return _connection != null;
		}

		public async Task<bool> Open(string userName, string passWord) {
			if (_connection != null) return false;

			_userName = userName;
			_passWord = passWord;


			_connection = new LoxoneMiniserverConnection(_serverIp, _port, _userName, _passWord);
			if (!await _connection.Connect()) return false;

			_data = new LoxoneData(this);
			_data.Update(_connection.LoxData);

			_connection.OnMessage += _connection_OnMessage;

			return true;

		}


		public bool Close() {
			if (_connection == null) return false;
			_connection.OnMessage -= _connection_OnMessage;
			_connection.Close();
			_connection = null;
			return true;
		}


		private void _connection_OnMessage(object sender, Driver.EventArgs.OnEventTableMessageEventArgs e) {

			if (e.Message is EventTableOfValueStates evt) {
				foreach (string key in evt.Values.Keys) {

					Control ctrl = this.Data.Controls.FirstOrDefault(c => key.Equals(c.Uid));
					if (ctrl == null) ctrl = this.Data.Controls.FirstOrDefault(c => c.HasState(key));
					if (ctrl != null) {
						ctrl.SetState(key, evt.Values[key]);
					} else {
						//Console.WriteLine($"{ctrl?.Name ?? key} = {evt.Values[key]}");
					}

				}
			} else if (e.Message is EventTableOfTextStates txt) {
				foreach(var key in txt.Values.Keys) {

					string val = txt.Values[key].Text;

					Control ctrl = this.Data.Controls.FirstOrDefault(c => key.Uuid.Equals(c.Uid));
					if (ctrl == null) ctrl = this.Data.Controls.FirstOrDefault(c => c.HasState(key.Uuid));
					if (ctrl != null) {
						ctrl.SetState(key.Uuid, val);
					} else {
						//Console.WriteLine($"{ctrl?.Name ?? key.Uuid} = {val}");
					}

				}
			}

		}


		//private void _webSocket_OnMessage(object sender, WebSocketSharp.MessageEventArgs e) {



		//	if ((e.IsBinary) && (e.RawData[0] == 0x03)) {
		//		_lastHeader = new LoxoneHeader();
		//		_lastHeader.MessageType = (LoxoneMessageType)e.RawData[1];
		//		_lastHeader.MessageSize = e.RawData[7] * 256 * 256 * 256 + e.RawData[6] * 256 * 256 + e.RawData[5] * 256 + e.RawData[4];
		//		//Console.WriteLine($"Header : {_lastHeader.MessageType} : {_lastHeader.MessageSize}");
		//	} else if (_lastHeader != null) {
		//		if (_lastHeader.MessageType == LoxoneMessageType.Text) {
		//			LoxoneTextMessage msg = Newtonsoft.Json.JsonConvert.DeserializeObject<LoxoneTextMessage>(e.Data);

		//			LoxoneCmd cmd = _cmds.FirstOrDefault(c => c.Cmd.Equals(msg.LL.Control));
		//			if (cmd != null) cmd.SetMessage(msg);

		//			Console.WriteLine(msg);
		//		} else if (_lastHeader.MessageType == LoxoneMessageType.KeepAlive) {
		//			Console.WriteLine("keepalive responded");
		//		} else if (_lastHeader.MessageType == LoxoneMessageType.ValueState) {
		//			string uid = Tools.GetUuid(e.RawData);
		//			UInt64 val = Tools.GetUInt64(e.RawData, 16);

		//			Control ctrl = this.Data.Controls.FirstOrDefault(c => uid.Equals(c.Uid));
		//			if (ctrl == null) ctrl = this.Data.Controls.FirstOrDefault(c => c.HasState(uid));
		//			if (ctrl != null) {
		//				ctrl.SetState(uid, val);
		//			} else {
		//				//Console.WriteLine($"{ctrl?.Name ?? uid} = {val}");
		//			}
		//		} else if (_lastHeader.MessageType == LoxoneMessageType.TextState) {
		//			string uid = Tools.GetUuid(e.RawData);
		//			string icon = Tools.GetUuid(e.RawData, 16);
		//			UInt32 size = Tools.GetUInt32(e.RawData, 32);

		//			string txt = ASCIIEncoding.ASCII.GetString(e.RawData, 36, e.RawData.Length - 36);

		//			Control ctrl = this.Data.Controls.FirstOrDefault(c => uid.Equals(c.Uid));
		//			if (ctrl == null) ctrl = this.Data.Controls.FirstOrDefault(c => c.HasState(uid));
		//			if (ctrl != null) {
		//				ctrl.SetState(uid, txt);
		//			} else {
		//				//Console.WriteLine($"{ctrl?.Name ?? uid} = {txt}");
		//			}

		//		} else if (_lastHeader.MessageType == LoxoneMessageType.Binary) {
		//			Console.WriteLine("LoxAPP3.json");
		//			LoxoneStructure msg = new LoxoneStructure();
		//			((LoxoneStructure)msg).Read(e.Data);
		//			this.Data.Update(msg as LoxoneStructure);
		//		}
		//		_lastHeader = null;
		//	} else {
		//		Console.WriteLine(e.Data);
		//	}

		//}



		//private string _lastCmd = string.Empty;
		//private LoxoneHeader _lastHeader = null;
		//private TaskCompletionSource<LoxoneMessage> _tcsMessage = null;


		//public bool Send(string cmd) {
		//		if (_connection == null) return false;

		//		Console.WriteLine($"Send : {cmd}");

		//		_lastCmd = cmd;
		//		_webSocket.Send(cmd.Replace("[HASH]", _credentials));
		//		//LoxoneMessage msg = await _tcsMessage.Task;

		//		//_lastCmd = string.Empty;
		//		//if (msg is LoxoneTextMessage) {
		//		//	if (((LoxoneTextMessage)msg).LL.Code != 200) return false;
		//		//}
		//		return true;
		//	}



		internal async Task<string> WriteValue(string uuid, object value) {
			if (_connection == null) return null;
			Console.Write($"WriteValue : {uuid}/{value}");

			var response = await _connection.WriteValue(uuid, value);

			Console.WriteLine($" -> {response.Data.Value}");

			return response?.Data.Value;
		}

		internal async Task<string> ReadValue(string uuid) {
			if (_connection == null) return null;
			Console.Write($"ReadValue : {uuid}");

			var response = await _connection.ReadValue(uuid);

			Console.WriteLine($" -> {response.Data.Value}");

			return response?.Data.Value;
		}


	

	}
}
