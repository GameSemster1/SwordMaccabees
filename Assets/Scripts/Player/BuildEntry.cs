using System;
using UnityEngine;

[Serializable]
public class BuildEntry
{
	public string text;
	public Sprite image;
	public Action action;
	
	public virtual float FillAmount => 0;
}