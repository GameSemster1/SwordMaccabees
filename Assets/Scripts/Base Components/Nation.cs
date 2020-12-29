using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nation : MonoBehaviour
{
	public Type type;

	public enum Type
	{
		Jewish,
		Greek,
		NonJewHostile,
		NonJewNotHostile
	}

	private readonly HashSet<KeyValuePair<Type, Type>> hostility = new HashSet<KeyValuePair<Type, Type>>
	{
		new KeyValuePair<Type, Type>(Type.Jewish, Type.Greek),
		new KeyValuePair<Type, Type>(Type.Greek, Type.Jewish),
		new KeyValuePair<Type, Type>(Type.NonJewHostile, Type.Jewish),
		new KeyValuePair<Type, Type>(Type.Jewish, Type.NonJewHostile)
	};

	public bool IsHostile(Nation other)
	{
		return hostility.Contains(new KeyValuePair<Type, Type>(type, other.type));
	}
}