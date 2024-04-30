using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : ZombieEnemy
{
   public override float GetAttackDis()
   {
      return 7;
   }

   public override void SetAttackState()
   {
      SetState(new  BossAttackState(playerMovement, this));
   }
   
   public override void GetChaseAniName()
   {
      base.GetChaseAniName();
      int random = UnityEngine.Random.Range(0, 2);
      if (random == 0)
      {
         ChaseAni = "run";
      }
      else
      {
         ChaseAni = "walk";
      }
   }
}
