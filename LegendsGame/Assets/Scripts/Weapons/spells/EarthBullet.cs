﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBullet : MonoBehaviour
{
    private int speed = 1;
    public float DestroyTime;

    private void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }
    private IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            speed += 1;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //Particle.Play();
        Destroy(gameObject);
    }
    private void Start()
    {
        StartCoroutine(Shoot());
        StartCoroutine(Delete());
    }

    public IEnumerator Delete()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);
    }
}