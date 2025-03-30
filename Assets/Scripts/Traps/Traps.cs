using System.Collections;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public enum TrapType { Platform, Spear, SpikeBall }
    
    public TrapType trapType;
    public float moveUpHeight = 3f;
    public float moveSpeed = 2f;
    public float delayBeforeReset = 2f;
    public float spearSpeed = 5f;
    public float spikeBallSpeed = 3f;

    private Vector3 originalPosition;
    private bool isTriggered = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            switch (trapType)
            {
                case TrapType.Platform:
                    StartCoroutine(MoveTrapUp());
                    break;
                case TrapType.Spear:
                    StartCoroutine(SpearUp());
                    break;
                case TrapType.SpikeBall:
                    StartCoroutine(RollSpikeBall());
                    break;
            }
        }
    }

    private IEnumerator MoveTrapUp()
    {
        float targetHeight = originalPosition.y + moveUpHeight;
        while (transform.position.y < targetHeight)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetHeight, transform.position.z), moveSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(delayBeforeReset);
        StartCoroutine(MoveTrapDown());
    }

    private IEnumerator MoveTrapDown()
    {
        while (transform.position.y > originalPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isTriggered = false;
    }

    private IEnumerator SpearUp()
    {
        float targetHeight = originalPosition.y + moveUpHeight;
        while (transform.position.y < targetHeight)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetHeight, transform.position.z), spearSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(delayBeforeReset);
        transform.position = originalPosition;
        isTriggered = false;
    }

    private IEnumerator RollSpikeBall()
    {
        float targetPosition = originalPosition.x + 10f;
        while (transform.position.x < targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition, transform.position.y, transform.position.z), spikeBallSpeed * Time.deltaTime);
            yield return null;
        }
        isTriggered = false;
    }
}
