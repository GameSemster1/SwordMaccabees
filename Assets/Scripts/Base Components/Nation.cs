using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for determining the nation of a unit or building.
/// </summary>
public class Nation : MonoBehaviour
{
	[SerializeField] [Tooltip("This unit's nation.")]
	private Type type;

	/// <summary>
	/// An enum of possible nations.
	/// </summary>
	private enum Type
	{
		Jewish,
		Greek,
		NonJewHostile,
		NonJewNotHostile
	}

	/// <summary>
	/// The set of hostility relations.
	/// </summary>
	private readonly HashSet<KeyValuePair<Type, Type>> hostility = new HashSet<KeyValuePair<Type, Type>>
	{
		new KeyValuePair<Type, Type>(Type.Jewish, Type.Greek),
		new KeyValuePair<Type, Type>(Type.Greek, Type.Jewish),
		new KeyValuePair<Type, Type>(Type.NonJewHostile, Type.Jewish),
		new KeyValuePair<Type, Type>(Type.Jewish, Type.NonJewHostile)
	};

	/// <summary>
	/// Is 'other' hostile?
	/// </summary>
	public bool IsHostile(Nation other)
	{
		return hostility.Contains(new KeyValuePair<Type, Type>(type, other.type));
	}
}