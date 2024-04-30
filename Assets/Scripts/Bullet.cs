using System;
using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class Bullet : EnemyHealth
{
    private Vector3 _dic;
    public float speed = 1;
    private PlayerMovement _player;
    private float _currentTime;
    public void SetTarget(PlayerMovement target)
    {
        _player = target;
        _currentTime = 0;
        _dic = target.transform.position + new Vector3(0, 0.9f, 0) - transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > 5)
        {
            EnemyManager.Instance.DestroyBullet(this);
        }
        else
        {
            transform.Translate(_dic * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _player.transform)
        {
            _player.Damage(5);
            EnemyManager.Instance.DestroyBullet(this);
        }
    }
}
