using UnityEngine;

public class CenterPlayer : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player Body");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
}
