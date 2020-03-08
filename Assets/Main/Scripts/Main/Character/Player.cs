using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    float forcePower = 200f, dis = 0, radius = 100;
    LineRenderer lR;
    RaycastHit hit;
    Transform directionTf;
    bool isMoving = false;
    float targetScale = 0, hitPointDis = 0;
    void Awake()
    {
        directionTf = gameObject.Child(0).transform;
        lR = gameObject.Gc<LineRenderer>();
    }

    void Update()
    {
        if (IsPlaying)
        {
            if (IsDown)
            {
                MouseButtonDown();
            }

            if (IsClick)
            {
                dis = V3.Dis(MP, mp);
                if (dis > radius)
                    mp = V3.Move(MP, mp, radius);
                directionTf.rotation = Q.Euler(0, Ang.LookForward(mp, MP), 0);
                Line();
            }

            if (IsUp)
            {
                A.LS.levels[A.LS.GetLevelIdx()].moveCount--;
                base.rb.RbConstraints(false, true, false, true, false, false);
                base.rb.AddForce(directionTf.forward.normalized * forcePower, ForceMode.Impulse);
                isMoving = true;
                targetScale = transform.localScale.x - (transform.localScale.x / 100 * 15f);
                print(targetScale);
                hitPointDis = V3.Dis(transform.position, hit.point);
            }

            if (isMoving)
            {
                // float a = M.Remap(transform.localScale.x, hitPointDis, 0, transform.localScale.x, targetScale);
                // print(a);
                // transform.localScale = V3.I * M.Remap(transform.localScale.x, transform.localScale.x, targetScale);
            }

            UpdateLine();
        }
    }

    void Line()
    {
        if (Physics.Raycast(transform.position, directionTf.forward, out hit, float.MaxValue))
        {
            if (hit.collider.gameObject)
            {
                lR.SetPosition(lR.positionCount - 1, hit.point);
            }
        }
    }

    void UpdateLine()
    {
        lR.SetPosition(0, transform.position);
        if (hit.collider)
        {
            lR.SetPosition(lR.positionCount - 1, hit.point);
        }
    }

    public void MouseButtonDown()
    {
        mp = MP;
        lR.SetPosition(0, transform.position);
    }

    void OnCollisionEnter(Collision other)
    {
        if (IsPlaying)
        {
            base.rb.FreezeAll();
            if (A.LS.levels[A.LS.GetLevelIdx()].moveCount <= 0)
                A.GC.GameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.Tag("Flag"))
        {
            A.GC.LevelCompleted();
        }
    }
}