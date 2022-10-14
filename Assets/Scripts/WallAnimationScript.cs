using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAnimationScript : MonoBehaviour {

    private float timer;

    [SerializeField] private float moveScale = 0.2f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < 0.6)
        {
            transform.position += Vector3.up * moveScale * Time.deltaTime;
        }

        if (timer > 3.0)
        {
            transform.position += Vector3.down * moveScale * Time.deltaTime;
        }

        if (timer > 6.0)
        {
            Destroy(this.gameObject);
        }
    }

}
