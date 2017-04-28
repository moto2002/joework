/*******************************************
 * Author : hanxianming
 * Date   : 2016-3-23
 * Use    : 
 *******************************************/

package
{
	import flash.filesystem.File;
	
	import fairygui.editor.plugin.ICallback;
	import fairygui.editor.plugin.IFairyGUIEditor;
	import fairygui.editor.plugin.IPublishData;
	import fairygui.editor.plugin.IPublishHandler;
	
	public final class AutoGenerateCodePlugin implements IPublishHandler
	{
		
		private var packageObjByGid:Object = {};
		private var packageObjByClassName:Object = {};
		
		private var _editor:IFairyGUIEditor;
		
		private var _prefix:String = "UI_";
		
		public function AutoGenerateCodePlugin(editor:IFairyGUIEditor)
		{
			_editor = editor;
		}
		
		
		/**
		 * 组件输出类定义列表。这是一个Map，key是组件id，value是一个结构体，例如：
		 * {
		 * 		classId : "8swdiu8f",
		 * 		className ： "AComponent",
		 * 		superClassName : "GButton",
		 * 		members : [
		 * 			{ name : "n1" : type : "GImage" },
		 * 			{ name : "list" : type : "GList" },
		 * 			{ name : "a1" : type : "GComponent", src : "Component1" },
		 * 			{ name : "a2" : type : "GComponent", src : "Component2", pkg : "Package2" },
		 * 		]
		 * }
		 * 注意member里的name并没有做排重处理。
		 */
		
		public function doExport(data:IPublishData, callback:ICallback):Boolean
		{
			
			if(_editor.project.customProperties["gen_code"]!="true")
				return false;
			
			var classCodes:Array = [];
			var bindCodes:Array = [];
			var allBindCodes:Array = [];
			var sameNameCheck:Object;
			
			var code_path:String = _editor.project.customProperties["code_path"];
			if (code_path == "" || code_path == null)
			{
				callback.addMsg("请指定导出路径 code_path");
				return false;
			}

			code_path = new File(data.filePath).resolvePath(code_path).nativePath;
			
			var codeFolder:File = new File(data.filePath);
			var bindPackage:String = "viewuicode." + PinYinUtils.toPinyin(data.targetUIPackage.name);
			codeFolder = codeFolder.resolvePath(code_path + File.separator + getFilePackage(bindPackage));
			
			try
			{
				codeFolder.deleteDirectory(true)
			} 
			catch(error:Error) 
			{
				
			}
			
			
			if(!codeFolder.exists)
				codeFolder.createDirectory();
			
			
			
			allBindCodes.push("package");
			allBindCodes.push("{");
			allBindCodes.push("");
			allBindCodes.push("\tpublic class GUICodeBinders");
			allBindCodes.push("\t{");
			allBindCodes.push("\t\tpublic static function bindAll():void");
			allBindCodes.push("\t\t{");
			
			var projectXML:XML = new XML(FileTool.readFile(_editor.project.basePath + File.separator + "project.xml"));
			for each (var j:XML in (projectXML.packages as XMLList).children()) 
			{
				var publishPath:String = _editor.project.basePath + File.separator + j.@name;
				var packageXML:XML = new XML(FileTool.readFile(publishPath + File.separator + "package.xml"));
				
				var publishPackageName:String = PinYinUtils.toPinyin(packageXML.publish.@name);
				allBindCodes.splice(2, 0, "\timport viewuicode." + publishPackageName + ".Binder_" + publishPackageName);
				allBindCodes.push("\t\t\tBinder_" + publishPackageName + ".bindAll();");
				
				for each (var i:XML in (packageXML.resources as XMLList).children()) 
				{
					packageObjByGid[String(i.@id)] = i;
					packageObjByClassName[String(i.@name)] = {xml:i, packageName:String(j.@name)};
				}
			}
			
			allBindCodes.push("\t\t}");
			allBindCodes.push("\t}");
			allBindCodes.push("}");
			
			FileTool.writeFile(code_path + File.separator + "GUICodeBinders.as", allBindCodes.join("\r\n"));
			
			var binderName:String = "Binder_" + PinYinUtils.toPinyin(data.targetUIPackage.name);
			bindCodes.push("package " + bindPackage);
			bindCodes.push("{");
			bindCodes.push("\timport fairygui.UIObjectFactory;");
			bindCodes.push("");
			bindCodes.push("\tpublic class " + binderName);
			bindCodes.push("\t{");
			bindCodes.push("\t\tpublic static function bindAll():void");
			bindCodes.push("\t\t{");
			
			var sameBindImportCheck:Object = {};
			
			for each(var classInfo:Object in data.outputClasses)
			{
				sameNameCheck = {};
				classCodes.length = 0;
				
				var className:String = _prefix + PinYinUtils.toPinyin(classInfo.className); //你也可以加个前缀后缀啥的
				
				var classFilePathName:String = PinYinUtils.toPinyin(getPackageName(classInfo.classId));
				var packageName:String = (classFilePathName == "" ? bindPackage : (bindPackage + "." + classFilePathName));
				
				var bindImport:String = "\timport " + packageName + "." + className;
				if (sameBindImportCheck[bindImport] == null)
				{
					sameBindImportCheck[bindImport] = true;
					bindCodes.splice(2, 0, bindImport);
				}
				
				classCodes.push("package " + packageName);
				classCodes.push("{");
				classCodes.push("\timport fairygui.*;");
				classCodes.push("");
				classCodes.push("\tpublic class " + className + " extends " + (classInfo.customSuperClassName?classInfo.customSuperClassName:classInfo.superClassName));
				classCodes.push("\t{");
				
				classCodes.push("\t\tpublic static const url:String = " + "\"ui://" + data.targetUIPackage.id + classInfo.classId + "\";");
				classCodes.push("\n");
				var memberImportSameCheck:Object = {};
				
				for each(var memberInfo:Object in classInfo.members)
				{
					if (!checkIsUseDefaultName(memberInfo.name))
					{
						var existed:* = sameNameCheck[memberInfo.type + "#" + memberInfo.name];
						if(existed!=undefined)
						{
							existed++;
							memberInfo.name = memberInfo.name + existed;						
						}
						sameNameCheck[memberInfo.type + "#" + memberInfo.name] = 1;
						
						if(memberInfo.src)
						{
							memberInfo.type = _prefix + memberInfo.src;
							var memberImport:String = "\timport " + PinYinUtils.toPinyin(getPackageNameByClassName(memberInfo.src)) + "." + PinYinUtils.toPinyin(memberInfo.type);
							if (memberImportSameCheck[memberImport] == null)
							{
								memberImportSameCheck[memberImport] = true;
								classCodes.splice(2, 0, memberImport);
							}
						}
						
						if(memberInfo.type=="Controller")
						{
							classCodes.push("\t\tpublic var c_" + PinYinUtils.toPinyin(memberInfo.name) + ":" + PinYinUtils.toPinyin(memberInfo.type) + ";");
						}
						else if(memberInfo.type=="Transition")
						{
							classCodes.push("\t\tpublic var t_" + PinYinUtils.toPinyin(memberInfo.name) + ":" + PinYinUtils.toPinyin(memberInfo.type) + ";");
						}
						else
						{
							classCodes.push("\t\tpublic var m_" + PinYinUtils.toPinyin(memberInfo.name) + ":" + PinYinUtils.toPinyin(memberInfo.type) + ";");
						}
						
					}
				}
				
				classCodes.push("");
				classCodes.push("\t\tpublic static function createInstance():"+className);
				classCodes.push("\t\t{");
				classCodes.push("\t\t\treturn " + className + "(UIPackage.createObject(\"" + PinYinUtils.toPinyin(data.targetUIPackage.name) + "\",\"" + classInfo.className + "\"));");
				classCodes.push("\t\t}");
				classCodes.push("");
				classCodes.push("\t\tpublic function " + className + "()");
				classCodes.push("\t\t{");
				classCodes.push("\t\t}");
				classCodes.push("");
				classCodes.push("\t\tprotected override function constructFromXML(xml:XML):void");
				classCodes.push("\t\t{");
				classCodes.push("\t\t\tsuper.constructFromXML(xml);");
				classCodes.push("");
				
				
				var childIndex:int = 0;
				var controllerIndex:int = 0;
				var transitionIndex:int = 0;
				for each(memberInfo in classInfo.members)
				{
					if(memberInfo.type=="Controller")
					{
						if (!checkIsUseDefaultName(memberInfo.name))
						{
							classCodes.push("\t\t\tc_" + PinYinUtils.toPinyin(memberInfo.name) + " = this.getControllerAt(" + controllerIndex + ");");
						}
						controllerIndex++;
					}
					else if(memberInfo.type=="Transition")
					{
						if (!checkIsUseDefaultName(memberInfo.name))
						{
							classCodes.push("\t\t\tt_" + PinYinUtils.toPinyin(memberInfo.name) + " = this.getTransitionAt(" + transitionIndex + ");");	
						}
						transitionIndex++;
					}
					else
					{
						if (!checkIsUseDefaultName(memberInfo.name))
						{
							classCodes.push("\t\t\tm_" + PinYinUtils.toPinyin(memberInfo.name) + " = " + PinYinUtils.toPinyin(memberInfo.type) + "(this.getChildAt(" + childIndex + "));");
						}
						childIndex++;
					}
				}				
				classCodes.push("\t\t}");
				classCodes.push("\t}");
				classCodes.push("}");
				
				FileTool.writeFile(codeFolder.nativePath + File.separator + getFilePackage(classFilePathName) + File.separator + className + ".as", classCodes.join("\r\n"));
				
//				bindCodes.push("\t\t\tUIObjectFactory.setPackageItemExtension(\"ui://" + data.targetUIPackage.id + classInfo.classId
//					+ "\"," + className + ");");
				
				bindCodes.push("\t\t\tUIObjectFactory.setPackageItemExtension(" + className + ".url, " + className + ");");
			}
			
			bindCodes.push("\t\t}");
			bindCodes.push("\t}");
			bindCodes.push("}");
			
			FileTool.writeFile(codeFolder.nativePath + File.separator + binderName + ".as", bindCodes.join("\r\n"));
			
			callback.callOnSuccess();
			return true;
		}
		
		
		private function getFilePackage(packageStr:String):String
		{
			return packageStr.replace(new RegExp("\\.", "g"), File.separator);
		}
		
		private function getPackageName(classId:String):String
		{
			var packages:Vector.<String> = new Vector.<String>();
			var packageName:String = "";
			var folderId:String = packageObjByGid[classId].@folder;
			while (folderId != "" && folderId != null)
			{
				packages.push(packageObjByGid[folderId].@name);
				folderId = packageObjByGid[folderId].@folder;
			}
			
			if (packages.length > 0)
			{
				packages.reverse();
				packageName = packages.join("."); 
			}
			
			return packageName;
		}
		
		private function getPackageNameByClassName(className:String):String
		{
			var obj:Object = packageObjByClassName[className];
			if (obj == null)
			{
				return "";
			}
			
			var smallPackageName:String = getPackageName(obj.xml.@id);
			
			return "viewuicode." + obj.packageName + (smallPackageName == "" ? "" : ("." + smallPackageName));
		}
		
		private function checkIsUseDefaultName(name:String):Boolean
		{
			if (name.charAt(0) == "n" || name.charAt(0) == "c" || name.charAt(0) == "t")
			{
				return _isNaN(name.slice(1));
			}
			return false;
		}
		
		private function _isNaN(str:String):Boolean
		{
			if (isNaN(parseInt(str)))
			{
				return false;
			}
			return true;
		}
	}
	
	
}