using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that is in charge of setting the 'non-shroud' CustomRenderTexture's fields.
/// </summary>
public class NonShroudSetter : MonoBehaviour
{
	[SerializeField] private CustomRenderTexture tex;
	[SerializeField] private RenderTexture mask;
	[SerializeField] private RenderTexture main;
	private static readonly int MaskTex = Shader.PropertyToID("_MaskTex");
	private static readonly int MainTex = Shader.PropertyToID("_MainTex");

	private void Awake()
	{
		ClearOutRenderTexture(tex);
		ClearOutRenderTexture(main);
		ClearOutRenderTexture(mask);
	}

	private void Start()
	{
		tex.material.SetTexture(MainTex, main);
		tex.material.SetTexture(MaskTex, mask);
	}

	private static void ClearOutRenderTexture(RenderTexture renderTexture)
	{
		RenderTexture rt = RenderTexture.active;
		RenderTexture.active = renderTexture;
		GL.Clear(true, true, Color.clear);
		RenderTexture.active = rt;
	}
}