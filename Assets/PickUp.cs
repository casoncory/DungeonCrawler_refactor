﻿using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

	public GameObject[] pickUps;

	float[] probabilities = { 65f /*health*/, 30f /*armor*/, 5f /*power up*/ };

	public float pickupDensity = 2f;
	public float pickupDensityModifier = 0.97f; 

	private WorldController worldController;
	private World theWorld;

	// Use this for initialization
	void Start () {
		//find the maze generation script and get the board size
		worldController = gameObject.GetComponent<WorldController>();
		theWorld = worldController.getWorld;

		startGeneration();
	}

	public void startGeneration() {
		int numPickups = Mathf.FloorToInt((theWorld.Width+1)/pickupDensity); // Calc number pickUps
		Debug.Log("NUM PICKUPS: " + numPickups);

		for (int x = 1; x < theWorld.Width; ++x) {
			for (int y = 1; y < theWorld.Height; ++y) {

				if (x == 1 && y == 1) {
					continue;
				}

				if (theWorld.GetTileAt (x, y).Type == Tile.TileType.Floor && theWorld.GetTileAt(x,y).IsDeadEnd) {

					Debug.Log ("Tile x: " + x + " y: " + y + " is a DeadEnd");

					for (int i = 0; i < numPickups; i++) {
						int element = chooseRandom (probabilities);
						Instantiate (pickUps [element], new Vector2 (x, y), Quaternion.identity); 
						Debug.Log ("Creating PickUp at x: " + x + " y: " + y);
					}				
				}
			}
		}
		pickupDensity -= pickupDensityModifier; // Density (and numPickups) will be higher next level
	}

	int chooseRandom (float[] probs) {

		float total = 0;

		foreach (float elem in probs) {
			total += elem;
		}

		float randomPoint = Random.value * total;

		for (int i= 0; i < probs.Length; i++) {
			if (randomPoint < probs[i]) {
				return i;
			}
			else {
				randomPoint -= probs[i];
			}
		}
		return probs.Length - 1;
	}


}
