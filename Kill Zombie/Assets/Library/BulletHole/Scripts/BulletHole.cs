/// <summary>
/// BulletHole.cs
/// Author: MutantGopher
/// Attach this script to bullet hole prefabs. It handles bullet hole automatic destruction and fading.
/// </summary>

using UnityEngine;
using System.Collections;

public class BulletHole : MonoBehaviour
{
	public GameObject bulletHoleMesh;			// The GameObject that has the actual mesh
	private Color targetColor;					// The color to which the bullet hole wants to change
	


	// Use this for initialization
	void Start()
	{
		// Initialize the targetColor
		targetColor = bulletHoleMesh.GetComponent<Renderer>().material.color;
		targetColor.a = 0.0f;
	}
	

	// Update is called once per frame
	void Update()
	{
		Invoke("DestroyBulletHole", 5f);
	}

	void DestroyBulletHole()
    {
		Destroy(transform.gameObject);
    }

}

