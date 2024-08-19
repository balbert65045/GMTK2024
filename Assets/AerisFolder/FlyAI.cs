using UnityEngine;

public class FlyAI : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float radius = 5f;

    Vector3 spawnLocation;
    Vector3 target;
    Fly fly;

    enum FlyAIState
    {
        RANDOM_MOVEMENT,
        IDLE,
        EATEN
    }

    FlyAIState aiState = FlyAIState.IDLE;

    void Start()
    {
        spawnLocation = this.gameObject.transform.position;
        aiState = FlyAIState.RANDOM_MOVEMENT;
        fly = GetComponent<Fly>();
    }

    void Update()
    {
        RunAI();
    }

    public virtual void RunAI()
    {
        switch (aiState)
        {
            case FlyAIState.RANDOM_MOVEMENT:
                RunRandomMovement();
                break;
            case FlyAIState.IDLE:
                break;
            case FlyAIState.EATEN:
                break;
        }
    }

    public virtual void RunRandomMovement()
    {
        if (fly.GetIsEaten()) aiState = FlyAIState.EATEN;
        if (target == null || this.transform.position == target)
            SetRandomTarget();
        else
            this.transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), target, speed * Time.deltaTime);
    }

    void SetRandomTarget()
    {
        target = spawnLocation + (Vector3)(radius * UnityEngine.Random.insideUnitCircle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetRandomTarget();
    }
}