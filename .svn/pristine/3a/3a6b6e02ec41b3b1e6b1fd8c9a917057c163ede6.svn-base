using UnityEngine;
using System.Collections;
using System.Threading;
using System.IO;
using System.Net;
using System;

//TODO 下载完成后校验md5或者sha

/// <summary>
/// 通过http断点下载资源
/// </summary>
public class FilePointDownLoad 
{
	//下载进度
	public float progress{get; private set;}

	//涉及子线程要注意,Unity关闭的时候子线程不会关闭，所以要有一个标识
	private bool isStop;

	//子线程负责下载，否则会阻塞主线程，Unity界面会卡主
	private Thread thread;

	//表示下载是否完成
	public bool isDone{get; private set;}


	/// <summary>
	/// 下载方法(断点续传)
	/// </summary>
	/// <param name="url">URL下载地址</param>
	/// <param name="savePath">Save path保存路径+文件名</param>
	/// <param name="callBack">Call back回调函数</param>
	public void DownLoad(string url, string savePath, Action callBack)
	{
		isStop = false;
		isDone = false;

		//开启子线程下载,使用匿名方法
		thread = new Thread(delegate() 
		{
            string _saveDir = savePath.Substring(0, savePath.LastIndexOf('/'));

			//判断保存路径是否存在
			if(!Directory.Exists(_saveDir))
			{
				//没有就创建
				Directory.CreateDirectory(_saveDir);
			}

            //路径+下载的文件名
            string filePath = savePath;

			//使用流操作文件
			FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

			//获取文件现在的长度
			long fileLength = fs.Length;

			//获取下载文件的总长度
			long totalLength = GetLength(url);

			//如果没下载完
			if(fileLength < totalLength)
			{
				//断点续传核心，设置本地文件流的起始位置
				fs.Seek(fileLength, SeekOrigin.Begin);

				HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;

				//断点续传核心，设置远程访问文件流的起始位置
				request.AddRange((int)fileLength);
				Stream  stream = request.GetResponse().GetResponseStream();

				byte[] buffer = new byte[1024];
				//使用流读取内容到buffer中
				//注意方法返回值代表读取的实际长度,并不是buffer有多大，stream就会读进去多少
				int length = stream.Read(buffer, 0, buffer.Length);
				while(length > 0)
				{
					//如果Unity客户端关闭，停止下载,在OnApplicationQuit时要记得设置isStop＝true
					if(isStop) break;
					//将内容再写入本地文件中
					fs.Write(buffer, 0, length);
					//计算进度
					fileLength += length;
					progress = (float)fileLength / (float)totalLength;
					UnityEngine.Debug.Log(progress);
					//类似尾递归
					length = stream.Read(buffer, 0, buffer.Length);
				}
				stream.Close();
				stream.Dispose();

			}
			else
			{
				progress = 1;
			}
			fs.Close();
			fs.Dispose();
			//如果下载完毕，执行回调
			if(progress == 1)
			{
				isDone = true;
				if(callBack != null) 
					callBack();
			}

		});
		//开启子线程
		thread.IsBackground = true;
		thread.Start();
	}


	/// <summary>
	/// 获取下载文件的大小
	/// </summary>
	/// <returns>The length.</returns>
	/// <param name="url">URL.</param>
	long GetLength(string url)
	{
		HttpWebRequest requet = HttpWebRequest.Create(url) as HttpWebRequest;
		requet.Method = "HEAD";
		HttpWebResponse response = requet.GetResponse() as HttpWebResponse;
		return response.ContentLength;
	}

	public void Close()
	{
		isStop = true;

//		thread.Abort ();
//		thread = null;
		//一般来说，很多人都会使用Abort方法来终止线程，其实这种做法并不可取！如果你的线程正在操作临界资源，很有可能会造成资源没有正确释放而出现死锁问题。
		//正确的做法应该是使用标记来终止线程的执行。
	}
	
}