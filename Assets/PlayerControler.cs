using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float speed = 10;/*speed = 0.01f;*/ //public 외부 공용, private 비공개

    int[] scores = new int[5];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //for (int i = 1; i <= scores.Length; i++) scores[i-1] = i*10;
        for (int i = 0; i < scores.Length; i++) scores[i] = (i + 1) * 10;

        Debug.Log(scores[0]);
        Debug.Log(scores[1]);
        Debug.Log(scores[2]);
        Debug.Log(scores[3]);
        Debug.Log(scores[4]);


        //bool a = true;
        //bool b = false;
        //gameObject.SetActive(a || b);
        //Vector2 newPos = gameObject.transform.position;
        //newPos.x = newPos.x + 5;
        //transform.position = newPos;

        //transform.position = Vector3.one; //1,1,1

        //newPos.x = newPos.x + 5;
        //transform.position = newPos;

        //Debug.Log(newPos.x);
        //Debug.Log(newPos.y);

        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKey(KeyCode.UpArrow))
        // {
        //     this.transform.Translate(0, speed, 0);
        // }
        //if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     this.transform.Translate(0, -speed, 0);

        // }
        //if (Input.GetKey(KeyCode.RightArrow))
        // {
        //     this.transform.Translate(speed, 0, 0);
        // }
        //if (Input.GetKey(KeyCode.LeftArrow))
        // {
        //     this.transform.Translate(-speed, 0, 0);
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, y, 0);
        transform.position += direction.normalized * speed * Time.deltaTime;

    }
}
