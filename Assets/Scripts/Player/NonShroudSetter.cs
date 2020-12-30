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

	private RenderTexture copyOfMain;

	private void Awake()
	{
		// ClearOutRenderTexture(tex);
		ClearOutRenderTexture(main);
		ClearOutRenderTexture(mask);
	}

	private void Start()
	{
		copyOfMain = GenerateTexture();

		StartCoroutine(CopyTexture());

		mat.SetTexture(LastTex, copyOfMain);
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

	private IEnumerator CopyTexture()
	{
		while (true)
		{
			Graphics.Blit(main, copyOfMain);

			yield return new WaitForSeconds(0.01f);
		}
	}

	private RenderTexture GenerateTexture()
	{
		RenderTexture rt = new RenderTexture(
			main.width,
			main.height,
			0,
			main.format);
		rt.filterMode = main.filterMode;
		rt.antiAliasing = main.antiAliasing;

		return rt;
	}
}