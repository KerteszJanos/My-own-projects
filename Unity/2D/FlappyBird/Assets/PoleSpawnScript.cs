using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PoleSpawnScript : MonoBehaviour
{
	public GameObject pole;
	public float spawnRate = 2;
	private float timer = 0;
	public float hightOffset = 10;

	// Start is called before the first frame update
	void Start()
	{
		spawnPole();
	}

	// Update is called once per frame
	void Update()
	{
		if (timer < spawnRate)
		{
			timer += Time.deltaTime;
		}
		else
		{
			spawnPole();
			timer = 0;
		}
	}

	void spawnPole()
	{
		float lowestPoint = transform.position.y - hightOffset;
		float highestPoint = transform.position.y + hightOffset;

		Instantiate(pole, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
	}
}
