using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

    public int playerDamage;

    private Animator animator;
    private bool skipMove;
    private Transform target;
    
	// Use this for initialization
	protected override void Start () {//dodawanie do listy i wyznaczanie pozycji celu (gracza)
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}
	
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)//pomijanie co 2 ruchu żeby zombie poruszały się wolniej niż gracz
        {
            skipMove = false;
            return;
        }
        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }
    public void MoveEnemy()//poruszanie się w kierunku gracza
    {
        int xDir = 0;
        int yDir = 0;

        
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }
    protected override void OnCantMove<T>(T component)//jeśli nie można się poruszyć zakładamy ze trafiliśmy w gracza i zadajemy mu obrażenia 
    {
        Player hitPlayer = component as Player;
        animator.SetTrigger("EnemyAttack");
        hitPlayer.LoseFood(playerDamage);

    }
}
