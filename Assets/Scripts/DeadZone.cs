using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        GM.instance.LoseLife();
    }
}
