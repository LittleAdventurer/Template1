using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    protected Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
}
