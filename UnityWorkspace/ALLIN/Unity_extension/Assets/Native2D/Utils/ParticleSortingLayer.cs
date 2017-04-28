using UnityEngine;
using System.Collections;

/// <summary>
/// Native2D的particle进行深度排序.
/// </summary>
[ExecuteInEditMode]
public class ParticleSortingLayer : MonoBehaviour
{

	public int orderInLayer = 0;

	private Renderer m_renderer;

	void Start()
	{
		m_renderer = GetComponent<ParticleSystem> ().GetComponent<Renderer> ();
		if (m_renderer) {
			// Set the sorting layer of the particle system.
			m_renderer.sortingLayerName = "foreground";
			m_renderer.sortingOrder = orderInLayer;
		}
	}
	
	#if UNITY_EDITOR
	void Update()
	{
		if (m_renderer && m_renderer.sortingOrder!=orderInLayer)
			m_renderer.sortingOrder = orderInLayer;
	}
	#endif
}