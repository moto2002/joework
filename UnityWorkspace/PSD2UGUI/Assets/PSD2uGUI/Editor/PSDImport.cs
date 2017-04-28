/*-------------------------------------------------------------------------------
Copyright 2013-2014 Dreamset Studios, Mario Rodriguez Garcia
	All Rights Reserved.
		
This is PUBLISHED PROPRIETARY SOURCE CODE of Dreamset Studios and Mario Rodriguez
García.
the contents of this file may not be disclosed to third parties, copied or
duplicated in any form, in whole or in part, without the prior written
permission of Dreamset Studios and Mario Rodríguez García.
---------------------------------------------------------------------------------*/


using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using PSD2uGUI;
using System.IO;

[System.Serializable]
public class PSDImport : EditorWindow
{
		private static bool validator = true;
		private static bool physicsEnabled = false;
		private static int multiplier = 1;
		private static int choice = 1;

		private static float pu = 100;

		private Object dir = null;
		private string[] options = new string[] {"2D Game", "uGUI"};

		private const string importButton = "Import your 2D Interface";



		// This method allow to show the new Editor Window.
		[MenuItem("Edit/Interface/PSD2uGUI")]
		private static void showEditor ()
		{
				// "False" parameter value is usefull if we want to make an attacheable panel window
				EditorWindow.GetWindow<PSDImport> (false, "PSD2uGUI");
		}


		// This method allows to tell to Unity IDE if the new menu item is available
		[MenuItem("Edit/Interface/PSD2uGUI", true)]
		private static bool showEditorValidator ()
		{
				return validator;
		}				


		// This method is called when GUI to be shown is selected by the user 
		public void OnGUI ()
		{
				// Loading Icon
				Sprite sprite = AssetDatabase.LoadAssetAtPath ("Assets/PSD2uGUI/Logo-Icono.png", typeof(Sprite)) as Sprite;
		
				EditorGUILayout.BeginHorizontal ();

				if (sprite) {
						Rect r = new Rect (5, 5, sprite.textureRect.width, sprite.textureRect.height);
						EditorGUI.DrawPreviewTexture (r, sprite.texture);
				}

				GUILayout.Space (sprite.textureRect.width + 10);

				EditorGUILayout.BeginVertical ();
				EditorGUILayout.LabelField ("Import options");
				choice = GUILayout.SelectionGrid (choice, options, options.Length, EditorStyles.radioButton);

				switch (choice) {
				case 1:
						break;

				default:
						// This part of UI depends on the developer chooses sprite builder
						pu = EditorGUILayout.FloatField ("Pixel unit", 100f);
						multiplier = EditorGUILayout.IntField ("Depth multiplier", multiplier);
						physicsEnabled = false; //EditorGUILayout.Toggle("Enable Physics", physicsEnabled);
						break;
				}

				dir = EditorGUILayout.ObjectField ("Image dir", dir, typeof(Object), false) as Object;

				if (GUILayout.Button (importButton)) {

						if (dir != null) {
								string dirPath = AssetDatabase.GetAssetPath (dir);
							
								if (!System.IO.Directory.Exists (dirPath)) {
										Debug.LogWarning ("The image directory provided does not exists");
								} else {
										// The directory exists, so we assume it will have the images needed.
										bool flag = false;
										DirectoryInfo di = new DirectoryInfo (dirPath);
										FileInfo[] fi = di.GetFiles ();
										foreach (FileInfo file in fi) {
												Importer importer = new Importer (file.FullName, dirPath);

												if (importer != null) {
														if (importer.canContinue ()) {
																flag = true;
																Layer [] layers = importer.getLayersFromFile ();

																// Loading sprites. It works with SpriteContainer
																SpriteContainer spriteContainer = new SpriteContainer ();
																SpriteLoader sl = new SpriteLoader (spriteContainer);
																sl.ToString (); // Avoiding "not used" warning message

																// Now he have the target screen resolution of the psd design
																// Adapting the GameView panel to the resolution imported from the psdDigest.txt file
																InterfaceBuilder ib;
																string[] splitted = file.Name.Split ('.');
																switch (choice) {
																case 1:
																		
																		ib = new UGUIBuilder (importer.numberOfElements, spriteContainer, null, false, splitted [0]);
																		Presets.Instance ();
																		break;
																default:
																		ib = new SpriteBuilder (pu, multiplier, importer.numberOfElements, physicsEnabled, spriteContainer, null, splitted [0]);
																		break;
																}
																if (layers != null) {
																		// Creating the scene objects
																		ib.build2DPreview (layers, ib.root);
																}
																spriteContainer.Destroy ();
														}
												}
										}
										if (!flag)
												Debug.LogWarning ("No '.toParse' file to read at " + dirPath);
								}
						} else
								Debug.LogWarning ("The image directory has not been set. Please, drop the directory where PSD2uGUI will try to find the UI images");
				}
				EditorGUILayout.LabelField ("This is the basic Editor Window of PSD2uGUI");
				EditorGUILayout.LabelField ("Drop the folder where the images will be imported from. This folder must be under Resources directory");
				EditorGUILayout.LabelField ("From here you will be able to import your layers of each '.toParse' file the asset finds under the folder you've dropped");
				EditorGUILayout.LabelField ("If you want to use uGUI, you should edit your PSD by using the layer naming conventions following the user guide or Sample.PSD");

				EditorGUILayout.EndVertical ();
				EditorGUILayout.EndHorizontal ();
				Repaint ();
		}

	
		public static void toggleValidator ()
		{
				validator = !validator;
		}


		public void OnSelectionChange ()
		{
				//_selectedGameObjects = Selection.gameObjects.Where(go == go.
		}
}

