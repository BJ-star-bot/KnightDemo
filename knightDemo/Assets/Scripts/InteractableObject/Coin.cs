using System;
using Unity.Mathematics;
using UnityEditor.Build.Content;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public String collision_tag = "Player";
    public AudioClip pickup_sound;
    public GameObject pick_up_spark;
    public float volume = 0.7f;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(collision_tag))
        {
            Debug.Log("coin");
            Destroy(gameObject);
            text_score.Instance.add_score();
            AudioSource.PlayClipAtPoint(pickup_sound, transform.position, volume);
            if (pick_up_spark)
            {
                GameObject newone=Instantiate(pick_up_spark, transform.position, Quaternion.identity);
                Destroy(newone, 1f);
            }
        }
    }

}
