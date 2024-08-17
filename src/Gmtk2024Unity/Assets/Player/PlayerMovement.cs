using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _speed = 5f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0)
        {
            transform.position += horizontal * Vector3.right * _speed * Time.deltaTime;
        }
        if (horizontal < 0)
        {
            transform.position += horizontal * Vector3.right * _speed * Time.deltaTime;
        }

        var vertical = Input.GetAxis("Vertical");
        if (vertical > 0)
        {
            transform.position += vertical * Vector3.up * _speed * Time.deltaTime;
        }
        if (vertical < 0)
        {
            transform.position += vertical * Vector3.up * _speed * Time.deltaTime;
        }
    }
}
