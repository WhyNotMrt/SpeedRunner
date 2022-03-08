using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public GameObject cam;
    public float parallaxEffect;

    private float length, startpos;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector2(startpos + dist, transform.position.y);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;

    }
}
