using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonShroudSetter : MonoBehaviour
{
	public CustomRenderTexture tex;
	public RenderTexture mask;
	public RenderTexture main;
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