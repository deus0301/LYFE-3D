using System.ComponentModel;
using UnityEditor.UI;
using UnityEngine;

public class RevertPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 ogScale = new(1, 1, 1);
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        player = GameObject.Find("Player Body");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(player))
        {
            player.transform.localScale = ogScale;
            Destroy(gameObject);
        }
    }
}
