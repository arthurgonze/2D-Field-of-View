using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABehavior : MonoBehaviour
{
    private enum Behavior
    {
        Hunt,
        CallForReinforcements,
        RunToAlly
    }

    [SerializeField] private Behavior behavior = Behavior.Hunt;
    [SerializeField] float speed = 2f;
    [SerializeField] float lookToPlayerSpeed = 10000f;
    [SerializeField] Collider2D[] reinforcements;
    [SerializeField] float reinforcementsDetectionRadius;
    [SerializeField] LayerMask reinforcementsMask;

    //cached references
    private FOV fov;
    private FOVEnemyRotation fovRotation;

    //variables
    Transform[] presas;
    float step;
    bool goToPosition = false;
    Vector2 targetPosition = new Vector2();
    Behavior originalBehavior;

    // Start is called before the first frame update
    void Start()
    {
        fov = this.GetComponent<FOV>();
        fovRotation = this.GetComponent<FOVEnemyRotation>();
        originalBehavior = behavior;
    }

    // Update is called once per frame
    void Update()
    {
        step = speed * Time.deltaTime;
        presas = fov.visibleTargets.ToArray();

        switch (behavior)
        {
            case Behavior.Hunt:
                Hunt();
                break;
            case Behavior.RunToAlly:
                RunToAlly();
                break;
            case Behavior.CallForReinforcements:
                CallForReinforcements();
                break;
        }

        if(goToPosition)
        {
            GoToPosition();
            if((Vector2)this.transform.position == targetPosition)
            {
                behavior = originalBehavior;
                goToPosition = false;
                if (fovRotation)
                {
                    fovRotation.enabled = true;
                }
            }
        }
    }

    private void Hunt()
    {
        if (presas.Length > 0)
        {
            
            this.transform.position = Vector2.MoveTowards(this.transform.position, presas[0].transform.position, step);
            FocusFOVonPlayer();
        }

        if (fovRotation)
        {
            fovRotation.enabled = true;
        }
    }

    private void FocusFOVonPlayer()
    {
        if (fovRotation)
        {
            fovRotation.enabled = false;
        }

        Vector2 playerPos = presas[0].transform.position - transform.position;
        float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.AngleAxis(angle, Vector3.forward),
            lookToPlayerSpeed * Time.deltaTime);
    }

    private void RunToAlly()
    {
        reinforcements = Physics2D.OverlapCircleAll(transform.position,
           reinforcementsDetectionRadius,
           reinforcementsMask,
           -Mathf.Infinity,
           Mathf.Infinity);

        Collider2D reinforcement = new Collider2D();

        if(presas.Length > 0 && reinforcements.Length > 1)
        {
            if(reinforcement == null)
            {
                foreach(Collider2D collider in reinforcements)
                {
                    if (!collider.gameObject.Equals(this.gameObject))
                    {
                        reinforcement = collider;
                    }
                }
            }

            Vector2 posAliado = new Vector2(reinforcement.transform.position.x, reinforcement.transform.position.y - 1);
            this.transform.position = Vector2.MoveTowards(this.transform.position, posAliado, step);
            FocusFOVonPlayer();
        }

        if(fovRotation)
        {
            fovRotation.enabled = true;
        }
    }

    private void CallForReinforcements()
    {
        reinforcements = Physics2D.OverlapCircleAll(transform.position,
           reinforcementsDetectionRadius,
           reinforcementsMask,
           -Mathf.Infinity,
           Mathf.Infinity);

        if(presas.Length > 0 && reinforcements.Length > 1)
        {
            FocusFOVonPlayer();
            RunToAlly();
            foreach (Collider2D collider in reinforcements)
            {
                if (!collider.gameObject.Equals(this.gameObject))
                {
                    collider.GetComponent<IABehavior>().behavior = Behavior.Hunt;
                    collider.GetComponent<IABehavior>().targetPosition = presas[0].transform.position;
                    collider.GetComponent<IABehavior>().goToPosition = true;
                    collider.GetComponent<IABehavior>().presas = presas;
                    collider.GetComponent<IABehavior>().FocusFOVonPlayer();
                }
            }
        }
        if (fovRotation)
        {
            fovRotation.enabled = true;
        }

    }

    private void GoToPosition()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, targetPosition, step);
    }
}
