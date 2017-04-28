package 
{
	import flash.filesystem.File;
	import flash.utils.ByteArray;
	
	import fairygui.editor.plugin.ICallback;
	import fairygui.editor.plugin.IFairyGUIEditor;
	import fairygui.editor.plugin.IPublishData;
	import fairygui.editor.plugin.IPublishHandler;
	
	/**
	 * 演示了一个将所有文件不打包直接输出的发布插件。注意：这里没有实现对旧输出做清理。
	 */
	public class ExportNoZipPlugIn implements IPublishHandler
	{
		private var _editor:IFairyGUIEditor;
		
		public function ExportNoZipPlugIn(editor:IFairyGUIEditor)
		{
			_editor = editor;
		}

		public function doExport(data:IPublishData, callback:ICallback):Boolean
		{
			//只有在项目自定义属性里设置了no_zip=true才处理
			if(_editor.project.customProperties["no_zip"]!="true")
				return false;
			
			//不再使用默认的输出
			data.preventDefault();
			
			var file:File;
			var str:String;
			
			for(var key1:String in data.outputDesc)
			{
				var descData:* = data.outputDesc[key1];
				if(descData is XML)
					str = (descData as XML).toXMLString();
				else
					str = String(descData);
//				Utils.saveString(data.filePath + File.separator + key1, str);
			}
			
			for(var key2:String in data.outputRes)
			{
				var ba:ByteArray = data.outputRes[key2];
//				Utils.saveByteArray(data.filePath + File.separator + key2, ba);
			}
			
			if(data.sprites)
			{
//				Utils.saveString(data.filePath + File.separator + "sprites.bytes", data.sprites);
			}
			
			callback.callOnSuccess();
			return true;
		}
	}
}