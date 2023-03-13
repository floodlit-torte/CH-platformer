using UnityEngine;

public class Lantern : MonoBehaviour
{
    private PLayer player;
    private void Start()
    {
        player = FindObjectOfType<PLayer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.CollectLantern();
        Destroy(gameObject);
    }
}
