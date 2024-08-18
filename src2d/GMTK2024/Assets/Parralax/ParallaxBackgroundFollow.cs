using UnityEngine;

public class ParallaxBackgroundFollow : MonoBehaviour
{
    private float length, startpos;
    private GameObject cam;

    [SerializeField] float parallaxEffect;

    void Start()
    {
        cam = Camera.main.gameObject;
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
