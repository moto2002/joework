using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Water
{	
	/// <summary>
	/// 只支持水的折射,通过Water4 修改.
	/// </summary>
	[ExecuteInEditMode] // Make water live-update even when not in play mode
	public class WaterRefract : MonoBehaviour
	{
		
		public bool disablePixelLights = true;
		public int textureSize = 256;
		public float clipPlaneOffset = 0.07f;
		public LayerMask refractLayers = -1;
		
		
		private Dictionary<Camera, Camera> m_RefractionCameras = new Dictionary<Camera, Camera>(); // Camera -> Camera table
		private RenderTexture m_RefractionTexture;
		private int m_OldRefractionTextureSize;
		private static bool s_InsideWater;
		
		
		// This is called when it's known that the object will be rendered by some
		// camera. We render reflections / refractions and do other updates here.
		// Because the script executes in edit mode, reflections for the scene view
		// camera will just work!
		public void OnWillRenderObject()
		{
			if (!enabled || !GetComponent<Renderer>() || !GetComponent<Renderer>().sharedMaterial ||
			    !GetComponent<Renderer>().enabled)
			{
				return;
			}
			
			Camera cam = Camera.current;
			if (!cam)
			{
				return;
			}
			
			// Safeguard from recursive water reflections.
			if (s_InsideWater)
			{
				return;
			}
			s_InsideWater = true;
			
			Camera refractionCamera;
			CreateWaterObjects(cam, out refractionCamera);
			
			// find out the reflection plane: position and normal in world space
			Vector3 pos = transform.position;
			Vector3 normal = transform.up;
			
			// Optionally disable pixel lights for reflection/refraction
			int oldPixelLightCount = QualitySettings.pixelLightCount;
			if (disablePixelLights)
			{
				QualitySettings.pixelLightCount = 0;
			}
			
			UpdateCameraModes(cam, refractionCamera);
			
			refractionCamera.worldToCameraMatrix = cam.worldToCameraMatrix;
			
			// Setup oblique projection matrix so that near plane is our reflection
			// plane. This way we clip everything below/above it for free.
			Vector4 clipPlane = CameraSpacePlane(refractionCamera, pos, normal, -1.0f);
			refractionCamera.projectionMatrix = cam.CalculateObliqueMatrix(clipPlane);
			
			refractionCamera.cullingMask = ~(1 << 4) & refractLayers.value; // never render water layer
			refractionCamera.targetTexture = m_RefractionTexture;
			refractionCamera.transform.position = cam.transform.position;
			refractionCamera.transform.rotation = cam.transform.rotation;
			refractionCamera.Render();
			GetComponent<Renderer>().sharedMaterial.SetTexture("_RefractionTex", m_RefractionTexture);
			
			// Restore pixel light count
			if (disablePixelLights)
			{
				QualitySettings.pixelLightCount = oldPixelLightCount;
			}
			
			s_InsideWater = false;
		}
		
		
		// Cleanup all the objects we possibly have created
		void OnDisable()
		{
			if (m_RefractionTexture)
			{
				DestroyImmediate(m_RefractionTexture);
				m_RefractionTexture = null;
			}
			foreach (var kvp in m_RefractionCameras)
			{
				DestroyImmediate((kvp.Value).gameObject);
			}
			m_RefractionCameras.Clear();
		}
		
		
		// This just sets up some matrices in the material; for really
		// old cards to make water texture scroll.
		void Update()
		{
			if (!GetComponent<Renderer>())
			{
				return;
			}
			Material mat = GetComponent<Renderer>().sharedMaterial;
			if (!mat)
			{
				return;
			}
			
			Vector4 waveSpeed = mat.GetVector("WaveSpeed");
			float waveScale = mat.GetFloat("_WaveScale");
			Vector4 waveScale4 = new Vector4(waveScale, waveScale, waveScale * 0.4f, waveScale * 0.45f);
			
			// Time since level load, and do intermediate calculations with doubles
			double t = Time.timeSinceLevelLoad / 20.0;
			Vector4 offsetClamped = new Vector4(
				(float)Math.IEEERemainder(waveSpeed.x * waveScale4.x * t, 1.0),
				(float)Math.IEEERemainder(waveSpeed.y * waveScale4.y * t, 1.0),
				(float)Math.IEEERemainder(waveSpeed.z * waveScale4.z * t, 1.0),
				(float)Math.IEEERemainder(waveSpeed.w * waveScale4.w * t, 1.0)
				);
			
			mat.SetVector("_WaveOffset", offsetClamped);
			mat.SetVector("_WaveScale4", waveScale4);
		}
		
		void UpdateCameraModes(Camera src, Camera dest)
		{
			if (dest == null)
			{
				return;
			}
			// set water camera to clear the same way as current camera
			dest.clearFlags = src.clearFlags;
			dest.backgroundColor = src.backgroundColor;
			if (src.clearFlags == CameraClearFlags.Skybox)
			{
				Skybox sky = src.GetComponent<Skybox>();
				Skybox mysky = dest.GetComponent<Skybox>();
				if (!sky || !sky.material)
				{
					mysky.enabled = false;
				}
				else
				{
					mysky.enabled = true;
					mysky.material = sky.material;
				}
			}
			// update other values to match current camera.
			// even if we are supplying custom camera&projection matrices,
			// some of values are used elsewhere (e.g. skybox uses far plane)
			dest.farClipPlane = src.farClipPlane;
			dest.nearClipPlane = src.nearClipPlane;
			dest.orthographic = src.orthographic;
			dest.fieldOfView = src.fieldOfView;
			dest.aspect = src.aspect;
			dest.orthographicSize = src.orthographicSize;
		}
		
		
		// On-demand create any objects we need for water
		void CreateWaterObjects(Camera currentCamera, out Camera refractionCamera)
		{
			refractionCamera = null;
			// Refraction render texture
			if (!m_RefractionTexture || m_OldRefractionTextureSize != textureSize)
			{
				if (m_RefractionTexture)
				{
					DestroyImmediate(m_RefractionTexture);
				}
				m_RefractionTexture = new RenderTexture(textureSize, textureSize, 16);
				m_RefractionTexture.name = "__WaterRefraction" + GetInstanceID();
				m_RefractionTexture.isPowerOfTwo = true;
				m_RefractionTexture.hideFlags = HideFlags.DontSave;
				m_OldRefractionTextureSize = textureSize;
			}
			
			// Camera for refraction
			m_RefractionCameras.TryGetValue(currentCamera, out refractionCamera);
			if (!refractionCamera) // catch both not-in-dictionary and in-dictionary-but-deleted-GO
			{
				GameObject go =
					new GameObject("Water Refr Camera id" + GetInstanceID() + " for " + currentCamera.GetInstanceID(),
					               typeof(Camera), typeof(Skybox));
				refractionCamera = go.GetComponent<Camera>();
				refractionCamera.enabled = false;
				refractionCamera.transform.position = transform.position;
				refractionCamera.transform.rotation = transform.rotation;
				refractionCamera.gameObject.AddComponent<FlareLayer>();
				go.hideFlags = HideFlags.HideAndDontSave;
				m_RefractionCameras[currentCamera] = refractionCamera;
			}
		}
		

		// Given position/normal of the plane, calculates plane in camera space.
		Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
		{
			Vector3 offsetPos = pos + normal * clipPlaneOffset;
			Matrix4x4 m = cam.worldToCameraMatrix;
			Vector3 cpos = m.MultiplyPoint(offsetPos);
			Vector3 cnormal = m.MultiplyVector(normal).normalized * sideSign;
			return new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos, cnormal));
		}

	}
}