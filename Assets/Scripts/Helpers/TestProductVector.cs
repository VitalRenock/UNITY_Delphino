using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProductVector : MonoBehaviour
{
	public Transform Target;

	private void OnDrawGizmos()
	{
		if (Target == null)
			return;

		// Position de Target ds l'espace locale du transform.
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(transform.position + (Target.position - transform.position) , 0.2f);

		// Dans l'axe, 2 fois plus loin de la distance entre Target et transform.
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position + ((Target.position - transform.position) * 2), 0.2f);

		// Dans l'axe, À 1 unité du transform.
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position + ((Target.position - transform.position).normalized), 0.2f);

		// Dans l'axe, À 1 unité du transform.
		Gizmos.color = Color.magenta;
		Gizmos.DrawSphere(transform.position - ((Target.position - transform.position).normalized), 0.2f);

		// Dans l'axe, À 1 unité derrière Target.
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(Target.position + ((Target.position - transform.position).normalized), 0.2f);

		// Dans l'axe, À 1 unité derrière Target.
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(Target.position - ((Target.position - transform.position).normalized), 0.2f);


		//Gizmos.color = Color.cyan;
		//Gizmos.DrawSphere(Target.position - transform.position, 0.2f);


		//Gizmos.color = Color.magenta;
		//Gizmos.DrawSphere(transform.position + (transform.position + Target.position), 0.2f);

		//Gizmos.color = Color.red;
		//Gizmos.DrawSphere((transform.position + Target.position) * 2, 0.2f);
	}
}
