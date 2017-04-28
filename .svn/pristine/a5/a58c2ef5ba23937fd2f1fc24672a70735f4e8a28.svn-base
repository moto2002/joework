using com.tencent.pandora.MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace com.tencent.pandora
{
	public class NetLogic
	{
		public class DownloadRequest
		{
			public string url = string.Empty;

			public int size;

			public string md5 = string.Empty;

			public string destFile = string.Empty;

			public int curRedirectionTimes;

			public Action<int, Dictionary<string, object>> action;
		}

		private static uint streamReportSeqNo;

		private static ushort atmReportPort = 5692;

		private List<IPAddress> atmIPAddresses = new List<IPAddress>();

		private int lastTryAtmIPAddressIndex;

		private long atmUniqueSocketId = -1L;

		private bool isAtmConnecting;

		private int lastConnectAtmTime = -1;

		private Queue atmPendingRequests = Queue.Synchronized(new Queue());

		private ushort brokerPort;

		private string brokerHost = string.Empty;

		private string[] brokerAltIPs = new string[0];

		private List<IPAddress> brokerIPAddresses = new List<IPAddress>();

		private int lastTryBorkerIPAddressIndex;

		private long brokerUniqueSocketId = -1L;

		private bool isBrokerConnecting;

		private int lastConnectBrokerTime = -1;

		private Queue brokerPendingRequests = Queue.Synchronized(new Queue());

		private Queue resultQueue = Queue.Synchronized(new Queue());

		private Queue downloadRequestQueue = Queue.Synchronized(new Queue());

		private Queue logicDriveQueue = Queue.Synchronized(new Queue());

		private Dictionary<string, IPHostEntry> dnsCache = new Dictionary<string, IPHostEntry>();

		private bool isDownloadingPaused;

		private NetFrame netFrame = new NetFrame();

		public void Init()
		{
			this.netFrame.Init();
		}

		public void Destroy()
		{
			Logger.DEBUG(string.Empty);
			this.netFrame.Destroy();
		}

		public void Logout()
		{
			Logger.DEBUG(string.Empty);
			this.netFrame.Reset();
			this.resultQueue.Clear();
			this.isDownloadingPaused = false;
			this.atmIPAddresses.Clear();
			this.lastTryAtmIPAddressIndex = 0;
			this.atmUniqueSocketId = -1L;
			this.lastConnectAtmTime = -1;
			this.isAtmConnecting = false;
			this.atmPendingRequests.Clear();
			this.brokerPort = 0;
			this.brokerHost = string.Empty;
			this.brokerAltIPs = new string[0];
			this.brokerIPAddresses.Clear();
			this.lastTryBorkerIPAddressIndex = 0;
			this.brokerUniqueSocketId = -1L;
			this.lastConnectBrokerTime = -1;
			this.isBrokerConnecting = false;
			this.brokerPendingRequests.Clear();
			this.downloadRequestQueue.Clear();
			this.logicDriveQueue.Clear();
		}

		public void EnqueueDrive(Message msg)
		{
			this.logicDriveQueue.Enqueue(msg);
		}

		public void EnqueueResult(Message msg)
		{
			this.resultQueue.Enqueue(msg);
		}

		public void Drive()
		{
			if (this.isDownloadingPaused)
			{
				return;
			}
			this.netFrame.Drive();
			while (this.resultQueue.get_Count() > 0)
			{
				Message message = this.resultQueue.Dequeue() as Message;
				if (message.action != null)
				{
					message.action.Invoke(message.status, message.content);
				}
			}
			int num = Utils.NowSeconds();
			if (this.atmUniqueSocketId < 0L && !this.isAtmConnecting && this.lastConnectAtmTime + 5 < num)
			{
				this.CheckAtmSession();
			}
			if (this.brokerUniqueSocketId < 0L && !this.isBrokerConnecting && this.lastConnectBrokerTime + 5 < num)
			{
				this.CheckBrokerSession();
			}
			this.TrySendAtmReport();
			this.TrySendBrokerRequest();
			while (this.logicDriveQueue.get_Count() > 0)
			{
				Message message2 = this.logicDriveQueue.Dequeue() as Message;
				if (message2.action != null)
				{
					message2.action.Invoke(message2.status, message2.content);
				}
			}
			while (this.downloadRequestQueue.get_Count() > 0)
			{
				NetLogic.DownloadRequest request = this.downloadRequestQueue.Dequeue() as NetLogic.DownloadRequest;
				Action<int, Dictionary<string, object>> action = delegate(int downloadRet, Dictionary<string, object> content)
				{
					if (downloadRet == 100)
					{
						Logger.DEBUG(string.Empty);
						string url = content.get_Item("locationUrl") as string;
						this.AddDownload(url, request.size, request.md5, request.destFile, request.curRedirectionTimes + 1, request.action);
					}
					else
					{
						Logger.DEBUG(string.Empty);
						Message message3 = new Message();
						message3.status = downloadRet;
						message3.action = request.action;
						message3.content = content;
						this.EnqueueResult(message3);
					}
				};
				this.DownloadFile(request.url, request.size, request.md5, request.destFile, request.curRedirectionTimes, action);
			}
		}

		private void CheckAtmSession()
		{
			if (this.atmIPAddresses.get_Count() == 0)
			{
				string atmHost = "jsonatm.broker.tplay.qq.com";
				Action<IPHostEntry> resultAction = delegate(IPHostEntry entry)
				{
					Logger.DEBUG(atmHost + " dns done");
					if (entry != null && entry.get_AddressList().Length > 0)
					{
						IPAddress[] addressList = entry.get_AddressList();
						for (int i = 0; i < addressList.Length; i++)
						{
							IPAddress iPAddress = addressList[i];
							this.atmIPAddresses.Add(iPAddress);
						}
					}
					else
					{
						string[] array = new string[]
						{
							"101.226.129.205",
							"140.206.160.193",
							"182.254.10.86",
							"115.25.209.29",
							"117.144.245.201"
						};
						for (int j = 0; j < array.Length; j++)
						{
							string text = array[j];
							this.atmIPAddresses.Add(IPAddress.Parse(text));
						}
					}
					this.lastConnectAtmTime = 0;
				};
				this.lastConnectAtmTime = Utils.NowSeconds();
				this.AsyncGetHostEntry(atmHost, resultAction);
				return;
			}
			try
			{
				Action<int, Dictionary<string, object>> statusChangedAction = delegate(int status, Dictionary<string, object> content)
				{
					this.isAtmConnecting = false;
					if (status == 0)
					{
						this.atmUniqueSocketId = (long)content.get_Item("uniqueSocketId");
					}
					else
					{
						this.atmUniqueSocketId = -1L;
					}
				};
				this.lastConnectAtmTime = Utils.NowSeconds();
				this.lastTryAtmIPAddressIndex = (this.lastTryAtmIPAddressIndex + 1) % this.atmIPAddresses.get_Count();
				long num = this.SpawnTCPSession(this.atmIPAddresses.get_Item(this.lastTryAtmIPAddressIndex), NetLogic.atmReportPort, new ATMHandler(statusChangedAction));
				if (num > 0L)
				{
					Logger.DEBUG(string.Empty);
					this.isAtmConnecting = true;
				}
				else
				{
					Logger.ERROR(string.Empty);
				}
			}
			catch (Exception ex)
			{
				Logger.ERROR(ex.get_StackTrace());
			}
		}

		private void CheckBrokerSession()
		{
			if (this.brokerIPAddresses.get_Count() == 0)
			{
				this.lastConnectBrokerTime = Utils.NowSeconds();
				return;
			}
			try
			{
				Action<int, Dictionary<string, object>> statusChangedAction = delegate(int status, Dictionary<string, object> content)
				{
					this.isBrokerConnecting = false;
					if (status == 0)
					{
						this.brokerUniqueSocketId = (long)content.get_Item("uniqueSocketId");
					}
					else
					{
						this.brokerUniqueSocketId = -1L;
					}
				};
				Action<int, Dictionary<string, object>> packetRecvdAction = delegate(int status, Dictionary<string, object> content)
				{
					this.ProcessBrokerResponse(status, content);
				};
				this.lastConnectBrokerTime = Utils.NowSeconds();
				this.lastTryBorkerIPAddressIndex = (this.lastTryBorkerIPAddressIndex + 1) % this.brokerIPAddresses.get_Count();
				long num = this.SpawnTCPSession(this.brokerIPAddresses.get_Item(this.lastTryBorkerIPAddressIndex), this.brokerPort, new BrokerHandler(statusChangedAction, packetRecvdAction));
				if (num > 0L)
				{
					Logger.DEBUG(this.brokerIPAddresses.get_Item(this.lastTryBorkerIPAddressIndex).ToString());
					this.isBrokerConnecting = true;
				}
				else
				{
					Logger.ERROR(string.Empty);
				}
			}
			catch (Exception ex)
			{
				Logger.ERROR(ex.get_StackTrace());
			}
		}

		private void TrySendAtmReport()
		{
			if (this.atmUniqueSocketId > 0L)
			{
				while (this.atmPendingRequests.get_Count() > 0)
				{
					Logger.DEBUG(string.Empty);
					PendingRequest pendingRequest = this.atmPendingRequests.Dequeue() as PendingRequest;
					this.netFrame.SendPacket(this.atmUniqueSocketId, pendingRequest.data);
				}
			}
			else
			{
				int num = Utils.NowSeconds();
				while (this.atmPendingRequests.get_Count() > 0)
				{
					PendingRequest pendingRequest2 = this.atmPendingRequests.Peek() as PendingRequest;
					if (num <= pendingRequest2.createTime + 10)
					{
						break;
					}
					Logger.WARN(string.Empty);
					this.atmPendingRequests.Dequeue();
				}
			}
		}

		private void TrySendBrokerRequest()
		{
			if (this.brokerUniqueSocketId > 0L)
			{
				while (this.brokerPendingRequests.get_Count() > 0)
				{
					Logger.DEBUG(string.Empty);
					PendingRequest pendingRequest = this.brokerPendingRequests.Dequeue() as PendingRequest;
					this.netFrame.SendPacket(this.brokerUniqueSocketId, pendingRequest.data);
				}
			}
			else
			{
				int num = Utils.NowSeconds();
				while (this.brokerPendingRequests.get_Count() > 0)
				{
					PendingRequest pendingRequest2 = this.brokerPendingRequests.Peek() as PendingRequest;
					if (num <= pendingRequest2.createTime + 5)
					{
						break;
					}
					Logger.WARN(string.Empty);
					this.brokerPendingRequests.Dequeue();
				}
			}
		}

		public void SetBroker(ushort port, string host, string ip1, string ip2)
		{
			Logger.DEBUG(string.Concat(new string[]
			{
				"port=",
				port.ToString(),
				" host=",
				host,
				" ip1=",
				ip1,
				" ip2=",
				ip2
			}));
			this.brokerPort = port;
			this.brokerHost = host;
			this.brokerAltIPs = new string[]
			{
				ip1,
				ip2
			};
			Action<IPHostEntry> resultAction = delegate(IPHostEntry entry)
			{
				try
				{
					Logger.DEBUG(this.brokerHost + " dns done");
					if (entry != null && entry.get_AddressList().Length > 0)
					{
						IPAddress[] addressList = entry.get_AddressList();
						for (int i = 0; i < addressList.Length; i++)
						{
							IPAddress iPAddress = addressList[i];
							this.brokerIPAddresses.Add(iPAddress);
						}
					}
					else
					{
						string[] array = this.brokerAltIPs;
						for (int j = 0; j < array.Length; j++)
						{
							string text = array[j];
							if (text.get_Length() > 0)
							{
								this.brokerIPAddresses.Add(IPAddress.Parse(text));
							}
						}
					}
					this.lastConnectBrokerTime = 0;
				}
				catch (Exception ex)
				{
					Logger.ERROR(ex.get_Message() + ": " + ex.get_StackTrace());
				}
			};
			this.AsyncGetHostEntry(this.brokerHost, resultAction);
		}

		public void SetDownloadingPaused(bool status)
		{
			this.isDownloadingPaused = status;
		}

		public long SpawnTCPSession(IPAddress addr, ushort port, TCPSocketHandler handler)
		{
			Logger.DEBUG(string.Empty);
			return this.netFrame.AsyncConnect(addr, port, handler);
		}

		public void GetRemoteConfig(Action<int, Dictionary<string, object>> action)
		{
			Logger.DEBUG(string.Empty);
			try
			{
				string configUrl = "http://apps.game.qq.com/cgi-bin/api/tplay/" + Pandora.Instance.GetUserData().sAppId + "_cloud.cgi";
				Logger.DEBUG(configUrl);
				Uri uri = new Uri(configUrl);
				Action<IPHostEntry> resultAction = delegate(IPHostEntry entry)
				{
					Logger.DEBUG(configUrl + " dns done");
					if (entry != null && entry.get_AddressList().Length > 0)
					{
						IPAddress[] addressList = entry.get_AddressList();
						int num = new Random().Next(addressList.Length);
						long num2 = this.SpawnTCPSession(addressList[num], (ushort)uri.get_Port(), new ConfigHandler(action, configUrl));
						if (num2 > 0L)
						{
							Logger.DEBUG(addressList[num].ToString());
						}
						else
						{
							Logger.ERROR(string.Empty);
							action.Invoke(-1, new Dictionary<string, object>());
						}
					}
					else
					{
						Logger.ERROR(string.Empty);
						action.Invoke(-1, new Dictionary<string, object>());
					}
				};
				this.AsyncGetHostEntry(uri.get_Host(), resultAction);
			}
			catch (Exception ex)
			{
				action.Invoke(-1, new Dictionary<string, object>());
				Logger.ERROR(ex.get_Message() + ":" + ex.get_StackTrace());
			}
		}

		public int SendPacket(long uniqueSocketId, byte[] content)
		{
			Logger.DEBUG(uniqueSocketId.ToString());
			return this.netFrame.SendPacket(uniqueSocketId, content);
		}

		public void Close(long uniqueSocketId)
		{
			Logger.DEBUG(uniqueSocketId.ToString());
			this.netFrame.Close(uniqueSocketId);
		}

		public void AddDownload(string url, int size, string md5, string destFile, int curRedirectionTimes, Action<int, Dictionary<string, object>> action)
		{
			Logger.DEBUG(url);
			NetLogic.DownloadRequest downloadRequest = new NetLogic.DownloadRequest();
			downloadRequest.url = url;
			downloadRequest.size = size;
			downloadRequest.md5 = md5;
			downloadRequest.destFile = destFile;
			downloadRequest.curRedirectionTimes = curRedirectionTimes;
			downloadRequest.action = action;
			this.downloadRequestQueue.Enqueue(downloadRequest);
		}

		private void DownloadFile(string url, int size, string md5, string destFile, int curRedirectionTimes, Action<int, Dictionary<string, object>> action)
		{
			Logger.DEBUG(url);
			try
			{
				Uri uri = new Uri(url);
				Action<IPHostEntry> resultAction = delegate(IPHostEntry entry)
				{
					Logger.DEBUG(url + " dns done");
					if (entry != null && entry.get_AddressList().Length > 0)
					{
						IPAddress[] addressList = entry.get_AddressList();
						int num = new Random().Next(addressList.Length);
						long num2 = this.SpawnTCPSession(addressList[num], (ushort)uri.get_Port(), new DownloadHandler(url, size, md5, destFile, curRedirectionTimes, action));
						if (num2 > 0L)
						{
							Logger.DEBUG(addressList[num].ToString());
						}
						else
						{
							Logger.ERROR(string.Empty);
							action.Invoke(-1, new Dictionary<string, object>());
						}
					}
					else
					{
						Logger.ERROR(string.Empty);
						action.Invoke(-1, new Dictionary<string, object>());
					}
				};
				this.AsyncGetHostEntry(uri.get_Host(), resultAction);
			}
			catch (Exception ex)
			{
				action.Invoke(-1, new Dictionary<string, object>());
				Logger.ERROR(ex.get_Message() + ":" + ex.get_StackTrace());
			}
		}

		public void AsyncGetHostEntry(string host, Action<IPHostEntry> resultAction)
		{
			Logger.DEBUG(host);
			try
			{
				IPAddress iPAddress = null;
				if (IPAddress.TryParse(host, ref iPAddress))
				{
					IPHostEntry iPHostEntry = new IPHostEntry();
					iPHostEntry.set_AddressList(new IPAddress[]
					{
						iPAddress
					});
					resultAction.Invoke(iPHostEntry);
				}
				else if (this.dnsCache.ContainsKey(host))
				{
					IPHostEntry iPHostEntry2 = this.dnsCache.get_Item(host);
					resultAction.Invoke(iPHostEntry2);
				}
				else
				{
					Action<IAsyncResult> action = delegate(IAsyncResult ar)
					{
						try
						{
							string text = (string)ar.get_AsyncState();
							Logger.DEBUG(text + " end");
							IPHostEntry entry = Dns.EndGetHostEntry(ar);
							Logger.DEBUG(text + " end, entry.AddressList.Length=" + entry.get_AddressList().Length.ToString());
							Action<int, Dictionary<string, object>> action2 = delegate(int status, Dictionary<string, object> content)
							{
								string text2 = content.get_Item("host") as string;
								IPHostEntry iPHostEntry3 = content.get_Item("entry") as IPHostEntry;
								Action<IPHostEntry> action3 = content.get_Item("resultAction") as Action<IPHostEntry>;
								this.dnsCache.set_Item(text2, entry);
								action3.Invoke(entry);
							};
							Message message = new Message();
							message.status = 0;
							message.content.set_Item("host", text);
							message.content.set_Item("entry", entry);
							message.content.set_Item("resultAction", resultAction);
							message.action = action2;
							this.EnqueueDrive(message);
						}
						catch (Exception ex2)
						{
							Logger.ERROR(ex2.get_Message() + ":" + ex2.get_StackTrace());
						}
					};
					Logger.DEBUG(host + " begin");
					Dns.BeginGetHostEntry(host, new AsyncCallback(action.Invoke), host);
				}
			}
			catch (Exception ex)
			{
				resultAction.Invoke(null);
				Logger.ERROR(ex.get_Message() + ":" + ex.get_StackTrace());
			}
		}

		public void StreamReport(string jsonData)
		{
			Logger.DEBUG(string.Empty);
			try
			{
				PendingRequest pendingRequest = new PendingRequest();
				pendingRequest.createTime = Utils.NowSeconds();
				pendingRequest.seqNo = NetLogic.streamReportSeqNo++;
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				UserData userData = Pandora.Instance.GetUserData();
				dictionary.set_Item("seq_id", pendingRequest.seqNo);
				dictionary.set_Item("cmd_id", 5000);
				dictionary.set_Item("type", 1);
				dictionary.set_Item("from_ip", "10.0.0.108");
				dictionary.set_Item("process_id", 1);
				dictionary.set_Item("mod_id", 10);
				dictionary.set_Item("version", Pandora.Instance.GetSDKVersion());
				dictionary.set_Item("body", jsonData);
				dictionary.set_Item("app_id", userData.sAppId);
				string text = Json.Serialize(dictionary);
				string text2 = MinizLib.Compress(text.get_Length(), text);
				byte[] array = Convert.FromBase64String(text2);
				int num = IPAddress.HostToNetworkOrder(array.Length);
				byte[] bytes = BitConverter.GetBytes(num);
				byte[] array2 = new byte[bytes.Length + array.Length];
				Array.Copy(bytes, 0, array2, 0, bytes.Length);
				Array.Copy(array, 0, array2, bytes.Length, array.Length);
				pendingRequest.data = array2;
				this.atmPendingRequests.Enqueue(pendingRequest);
			}
			catch (Exception ex)
			{
				Logger.ERROR(ex.get_StackTrace());
			}
		}

		public void CallBroker(uint callId, string jsonData, int cmdId)
		{
			Logger.DEBUG(string.Empty);
			try
			{
				PendingRequest pendingRequest = new PendingRequest();
				pendingRequest.createTime = Utils.NowSeconds();
				pendingRequest.seqNo = callId;
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				UserData userData = Pandora.Instance.GetUserData();
				dictionary.set_Item("seq_id", callId);
				dictionary.set_Item("cmd_id", cmdId);
				dictionary.set_Item("type", 1);
				dictionary.set_Item("from_ip", "10.0.0.108");
				dictionary.set_Item("process_id", 1);
				dictionary.set_Item("mod_id", 10);
				dictionary.set_Item("version", Pandora.Instance.GetSDKVersion());
				dictionary.set_Item("body", jsonData);
				dictionary.set_Item("app_id", userData.sAppId);
				string text = Json.Serialize(dictionary);
				string text2 = MinizLib.Compress(text.get_Length(), text);
				byte[] array = Convert.FromBase64String(text2);
				int num = IPAddress.HostToNetworkOrder(array.Length);
				byte[] bytes = BitConverter.GetBytes(num);
				byte[] array2 = new byte[bytes.Length + array.Length];
				Array.Copy(bytes, 0, array2, 0, bytes.Length);
				Array.Copy(array, 0, array2, bytes.Length, array.Length);
				pendingRequest.data = array2;
				this.brokerPendingRequests.Enqueue(pendingRequest);
			}
			catch (Exception ex)
			{
				Logger.ERROR(ex.get_StackTrace());
			}
		}

		public void ProcessBrokerResponse(int status, Dictionary<string, object> content)
		{
			Logger.DEBUG(string.Empty);
			try
			{
				long num = (long)content.get_Item("type");
				if (num == 1L)
				{
					Logger.ERROR("recv invalid type[" + num.ToString() + "] from broker");
				}
				else if (num == 2L)
				{
					int num2 = (int)((long)content.get_Item("cmd_id"));
					uint callId = (uint)((long)content.get_Item("seq_id"));
					int num3 = num2;
					if (num3 != 5001)
					{
						if (num3 != 9000)
						{
							Logger.ERROR("recv invalid cmdId[" + num2.ToString() + "] from broker");
						}
						else
						{
							Logger.DEBUG("recv lua request rsp, seqId[" + callId.ToString() + "]");
							Dictionary<string, object> dictionary = Json.Deserialize(content.get_Item("body") as string) as Dictionary<string, object>;
							dictionary.set_Item("netRet", 0);
							string result = Json.Serialize(dictionary);
							CSharpInterface.ExecCallback(callId, result);
						}
					}
					else
					{
						Logger.DEBUG("recv statistics rsp, seqId[" + callId.ToString() + "]");
						string json = content.get_Item("body") as string;
						Dictionary<string, object> dictionary2 = Json.Deserialize(json) as Dictionary<string, object>;
						int num4 = (int)((long)dictionary2.get_Item("ret"));
						if (num4 != 0)
						{
							Logger.ERROR(string.Concat(new object[]
							{
								"recv error statistics rsp, ret[",
								num4.ToString(),
								"] errMsg[",
								dictionary2.get_Item("err_msg")
							}) + "]");
						}
					}
				}
				else
				{
					Logger.ERROR("recv invalid type[" + num.ToString() + "] from broker");
				}
			}
			catch (Exception ex)
			{
				Logger.ERROR(ex.get_StackTrace());
			}
		}
	}
}
