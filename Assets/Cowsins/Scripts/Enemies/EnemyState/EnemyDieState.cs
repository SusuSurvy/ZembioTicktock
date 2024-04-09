using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class EnemyDieState : EnemyStateBase
{
    public override void Update(float dt)
    {
      
    }
    public EnemyDieState(PlayerMovement player, ZombieController zombie) : base(player, zombie)
    {
    }
}
