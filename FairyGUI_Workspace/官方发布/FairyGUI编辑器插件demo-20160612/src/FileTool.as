package
{
	import flash.filesystem.File;
	import flash.filesystem.FileMode;
	import flash.filesystem.FileStream;
	import flash.utils.ByteArray;
	
	/**
	 *
	 * @author hanxianming
	 *
	 */
	public final class FileTool
	{
		public function FileTool()
		{
		}
		
		//格式化路径
		public static function formatPath(path:String):String
		{
			return path;
		}
		
		/**取正则匹配值*/
		public static function regOf(str:String, reg:RegExp):String
		{
			var m:Array = String(str || "").match(reg);
			return !m ? "" : m[1];
		}
		
		/**读取文本文件*/
		public static function readFile(path:String, charSet:String = "utf-8"):String
		{
			var file:File = new File(FileTool.formatPath(path));
			return readFileByFile(file, charSet);
		}
		
		public static function readByteByFile(file:File):ByteArray
		{
			if (!file.exists)
			{
				return null;
			}
			var stream:FileStream = new FileStream();
			stream.open(file, FileMode.READ);
			var bytes:ByteArray = new ByteArray();
			stream.readBytes(bytes);
			stream.close();
			return bytes;
		}
		
		public static function readByte(path:String):ByteArray
		{
			var file:File = new File(FileTool.formatPath(path));
			if (!file.exists)
			{
				return null;
			}
			var stream:FileStream = new FileStream();
			stream.open(file, FileMode.READ);
			var bytes:ByteArray = new ByteArray();
			stream.readBytes(bytes);
			stream.close();
			return bytes;
		}
		
		public static function readFileByFile(file:File, charSet:String = "utf-8"):String
		{
			if (!file.exists)
			{
				return null;
			}
			
			var stream:FileStream = new FileStream();
			stream.open(file, FileMode.READ);
			
			var bytes:ByteArray = new ByteArray();
			stream.readBytes(bytes);
			
			if (bytes[0] == 239 && bytes[1] == 187 && bytes[2] == 191)
			{
				stream.position = 3;
			}
			else
			{
				stream.position = 0;
			}
			
			var str:String = stream.readMultiByte(stream.bytesAvailable, charSet);
			stream.close();
			
			return str;
		}
		
		/**保存文本文件*/
		public static function writeFile(path:String, content:String, charSet:String = "utf-8"):void
		{
			var stream:FileStream = new FileStream();
			var file:File = new File(FileTool.formatPath(path));
			stream.open(file, FileMode.WRITE);
			if (charSet == "utf-8-bom")
			{
				stream.writeByte(239);
				stream.writeByte(187);
				stream.writeByte(191);
			}
			stream.writeMultiByte(content, "utf-8");
			stream.close();
		}
		
		/**保存文本文件*/
		public static function writeByte(path:String, content:ByteArray):void
		{
			var stream:FileStream = new FileStream();
			var file:File = new File(FileTool.formatPath(path));
			stream.open(file, FileMode.WRITE);
			stream.writeBytes(content);
			stream.close();
		}
		
		/**获得子文件*/
		public static function getSubFiles(path:String):Array
		{
			var file:File = new File(FileTool.formatPath(path));
			if (!file.exists)
			{
				return [];
			}
			var files:Array = [];
			var arr:Array = file.getDirectoryListing();
			
			for (var i:int = 0; i < arr.length; i++)
			{
				var f:File = arr[i];
				if (!f.isDirectory)
				{
					files.push(f.name);
				}
			}
			return files;
		}
		
		public static function getSubFolders(path:String):Array
		{
			var file:File = new File(FileTool.formatPath(path));
			if (!file.exists)
			{
				return [];
			}
			var folders:Array = [];
			var arr:Array = file.getDirectoryListing();
			for (var i:int = 0; i < arr.length; i++)
			{
				var f:File = arr[i];
				if (f.isDirectory)
				{
					folders.push(f);
				}
			}
			return folders;
		}
		
		public static function getAllSubFilesAndFolders(path:String):Array
		{
			var file:File = new File(FileTool.formatPath(path));
			if (!file.exists)
			{
				return [];
			}
			return file.getDirectoryListing();
		}
		
		public static function getAllSubFules(path:String, files:Array):void
		{
			var folders:Array = FileTool.getAllSubFilesAndFolders(path);
			while (folders.length > 0)
			{
				var file:File = folders.pop();
				if (file.isDirectory)
				{
					getAllSubFules(file.url, files);
				}
				else
				{
					files.push(file);
				}
			}
		}
	}
}