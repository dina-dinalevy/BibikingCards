using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class bibiCards : ScriptableObject
{
	public List<bibiCard> UnitySheet; // Replace 'EntityType' to an actual type that is serializable.
	//public List<EntityType> WorkSheet; // Replace 'EntityType' to an actual type that is serializable.
}
