/*******************************************
 * Author : hanxianming
 * Date   : 2016-4-11
 * Use    : 
 *******************************************/

package
{
	import flash.filesystem.File;
	
	import fairygui.editor.plugin.ICallback;
	import fairygui.editor.plugin.IFairyGUIEditor;
	import fairygui.editor.plugin.IPublishData;
	import fairygui.editor.plugin.IPublishHandler;
	
	public class BatExecutePlugin implements IPublishHandler
	{
		
		private var _editor:IFairyGUIEditor;
		
		public function BatExecutePlugin(editor:IFairyGUIEditor)
		{
			_editor = editor;
		}
		
		public function doExport(data:IPublishData, callback:ICallback):Boolean
		{
			
			var bat_file_string:String = _editor.project.customProperties["bat_files"];
			if (bat_file_string == null || bat_file_string == "")
			{
				return false;
			}
			
			var bat_files:Array = bat_file_string.split(";");
			for (var i:int = 0; i < bat_files.length; i++) 
			{
				try {
					var path:String = bat_files[i];
					var st:File  = new File(data.filePath);
					st = st.resolvePath(path);
					st.openWithDefaultApplication();
				}
				catch(err:Error) {
					callback.addMsg(err.message);
					callback.callOnFail();
				}
			}
			
			callback.callOnSuccess();
			
			return true;
		}
	}
}