using UnityEngine;

public class projectile : MonoBehaviour
{
    int shotSpeed;
    bool isHit;
    protected Sprite hitSprite;
    protected RuntimeAnimatorController hitAnimator;
    GameObject player;

    public projectile(int shotSpeed)
    {
        this.shotSpeed = shotSpeed;
    }

    void Awake()
    {
        hitSprite = Resources.Load<Sprite>("Warped Shooting Fx/Pixel Art/Hits/Hit-4/hits-4-1");
        hitAnimator = Resources.Load<RuntimeAnimatorController>("Warped Shooting Fx/Pixel Art/Hits/Hit-4/hits-4-1 (1)");
        player = GameObject.FindGameObjectWithTag("Player");
        isHit = false;
    }

    void Update()
    {
        if (!isHit)
            gameObject.transform.Translate(new Vector2(1, 0) * shotSpeed * Time.deltaTime);

        //out of bounds optimization
        if (Mathf.Abs(gameObject.transform.position.x) > 50 || Mathf.Abs(gameObject.transform.position.y) > 50)
            Destroy(gameObject);
    }

    protected void collide(Collision2D other)
    {
        if(player.GetComponent<playerBehavior>().ms < 20)
        {
            isHit = true;
            gameObject.transform.position = other.GetContact(0).point;
            gameObject.GetComponent<SpriteRenderer>().sprite = hitSprite;
            gameObject.GetComponent<Animator>().runtimeAnimatorController = hitAnimator;
            Destroy(gameObject.GetComponent<CapsuleCollider2D>());
        }
    }

    protected void Disappear() //used in animation event
    { 
        Destroy(gameObject);
    }
}

//protected functions can be accessed by subclasses but not unrelated classe and other things.
//virtual functions allow themselves to be overridden in subclasses