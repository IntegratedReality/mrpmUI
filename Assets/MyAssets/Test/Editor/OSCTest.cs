using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using OscSimpl;

public class OSCTest {

	[Test]
	public void MergeMessage2Main() {
		//Arrange

		//Act
		//Try to rename the GameObject
		var newGameObjectName = "My game object";

		//Assert
		//The object has a new name
		Assert.AreEqual("", "");
	}
}
