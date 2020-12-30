using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that is in charge of setting the 'non-shroud' CustomRenderTexture's fields.
/// </summary>
public class NonShroudSetter : MonoBehaviour
{
	// [SerializeField] private CustomRenderTexture tex;
	[SerializeField] private RenderTexture mask;
	[SerializeField] private RenderTexture main;

	[SerializeField] private Material mat;
	private static readonly int MaskTex = Shader.PropertyToID("_MaskTex");
	private static readonly int LastTex = Shader.PropertyToID("_LastTex");

	private void Awake()
	{
		// ClearOutRenderTexture(tex);
		ClearOutRenderTexture(main);
		ClearOutRenderTexture(mask);
	}

	private void Start()
	{
		mat.SetTexture(LastTex, main);
		mat.SetTexture(MaskTex, mask);
	}

	private static void ClearOutRenderTexture(RenderTexture renderTexture)
	{
		RenderTexture rt = RenderTexture.active;
		RenderTexture.active = renderTexture;
		GL.Clear(true, true, Color.clear);
		RenderTexture.active = rt;
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		// Read pixels from the source RenderTexture, apply the material, copy the updated results to the destination RenderTexture
		Graphics.Blit(src, dest, mat);
	}
}