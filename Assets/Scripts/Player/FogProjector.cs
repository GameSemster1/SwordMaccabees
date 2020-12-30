using System.Collections;
using UnityEngine;

/// <summary>
/// A class that is in charge of the fog projector.
/// Based on https://andrewhungblog.wordpress.com/2018/06/23/implementing-fog-of-war-in-unity/.
/// </summary>
public class FogProjector : MonoBehaviour
{
	[SerializeField] private Material projectorMaterial;
	[SerializeField] private float blendSpeed;
	[SerializeField] private int textureScale;

	[SerializeField] private RenderTexture fogTexture;

	private RenderTexture prevTexture;
	private RenderTexture currTexture;
	private Projector projector;

	private float blendAmount;
	private static readonly int PrevTexture = Shader.PropertyToID("_PrevTexture");
	private static readonly int CurrTexture = Shader.PropertyToID("_CurrTexture");
	private static readonly int Blend = Shader.PropertyToID("_Blend");

	private void Awake()
	{
		ClearOutRenderTexture(fogTexture);

		// fogTexture.format = RenderTextureFormat.ARGB32;
		projector = GetComponent<Projector>();
		projector.enabled = true;

		prevTexture = GenerateTexture();
		currTexture = GenerateTexture();

		// Projector materials aren't instanced, resulting in the material asset getting changed.
		// Instance it here to prevent us from having to check in or discard these changes manually.
		projector.material = new Material(projectorMaterial);

		projector.material.SetTexture(PrevTexture, prevTexture);
		projector.material.SetTexture(CurrTexture, currTexture);

		StartNewBlend();
	}

	RenderTexture GenerateTexture()
	{
		RenderTexture rt = new RenderTexture(
			fogTexture.width * textureScale,
			fogTexture.height * textureScale,
			0,
			fogTexture.format);
		rt.filterMode = FilterMode.Bilinear;
		rt.antiAliasing = fogTexture.antiAliasing;

		return rt;
	}

	private void StartNewBlend()
	{
		StopCoroutine(BlendFog());
		blendAmount = 0;
		// Swap the textures
		Graphics.Blit(currTexture, prevTexture);
		Graphics.Blit(fogTexture, currTexture);

		StartCoroutine(BlendFog());
	}

	IEnumerator BlendFog()
	{
		while (blendAmount < 1)
		{
			// increase the interpolation amount
			blendAmount += Time.deltaTime * blendSpeed;
			// Set the blend property so the shader knows how much to lerp
			// by when checking the alpha value
			projector.material.SetFloat(Blend, blendAmount);
			yield return null;
		}

		// once finished blending, swap the textures and start a new blend
		StartNewBlend();
	}

	private static void ClearOutRenderTexture(RenderTexture renderTexture)
	{
		RenderTexture rt = RenderTexture.active;
		RenderTexture.active = renderTexture;
		GL.Clear(true, true, Color.clear);
		RenderTexture.active = rt;
	}
}