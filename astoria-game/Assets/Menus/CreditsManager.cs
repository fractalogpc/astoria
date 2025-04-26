using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreditsManager : MonoBehaviour
{

    // I have a goofy idea for how to do this; noting it for later.
    // Place credit objects in order under this transform in hierarchy.
    // Move them to their positions, from a position outside the screen (their position - middle of screen * 10) or something.
    // If an object is named "Newline", then clear all previous objects off the screen before placing the next one.
    // I think it'll look fun.

    [SerializeField] private Transform _creditsParent;
    [SerializeField] private float _meanDuration = 1f; // Duration of the move
    [SerializeField] private AnimationCurve _curve; // Animation curve for the movement
    [SerializeField] private Transform _camera; // Reference to the camera
    [SerializeField] private float _durationVariation = 0.5f; // Variation in duration for each credit object
    [SerializeField] private float _moveDistance = 10f; // Distance to move the credits off-screen
    [SerializeField] private float _startDelay = 0.5f; // Delay before starting the credits
    [SerializeField] private float _newlineDelay = 1f; // Delay before clearing previous credits

    private Vector3[] _startPositions; // Start positions of the credits

    private void Awake()
    {
        _startPositions = new Vector3[_creditsParent.childCount];

        foreach (Transform credit in _creditsParent)
        {
            credit.gameObject.SetActive(false); // Hide all credits initially
            _startPositions[credit.GetSiblingIndex()] = credit.position; // Store the start position of each credit
        }

    }

    public void StartCredits()
    {
        StartCoroutine(MoveCreditsCoroutine());
    }

    public void StopCredits()
    {
        StopAllCoroutines(); // Stop all coroutines to prevent any ongoing credit movements
        StartCoroutine(ResetCreditsCoroutine()); // Reset the credits to their original positions
    }

    private IEnumerator ResetCreditsCoroutine()
    {
        yield return new WaitForSeconds(_startDelay);
        foreach (Transform credit in _creditsParent)
        {
            credit.gameObject.SetActive(false); // Hide all credits
            credit.position = _startPositions[credit.GetSiblingIndex()]; // Reset position to start position
        }
        yield return null; // Wait for the end of the frame
    }

    private IEnumerator MoveCreditsCoroutine()
    {
        yield return new WaitForSeconds(_startDelay); // Wait for the specified delay before starting the credits
        List<Transform> activeCredits = new List<Transform>();
        foreach (Transform credit in _creditsParent)
        {
            if (credit.gameObject.name == "Newline") {
                // Move all previous credits off the screen simultaneously
                yield return new WaitForSeconds(_newlineDelay); // Wait for the specified delay before clearing previous credits

                float elapsedTime2 = 0f;
                float duration2 = _meanDuration + Random.Range(-_durationVariation, _durationVariation);

                Vector3[] startPositions = new Vector3[activeCredits.Count];
                Vector3[] targetPositions = new Vector3[activeCredits.Count];
                for (int i = 0; i < activeCredits.Count; i++)
                {
                    startPositions[i] = activeCredits[i].position;
                    // Calculate target position !!
                    // Calculate distance in plane of camera
                    Vector3 difference2 = activeCredits[i].position - _camera.position;
                    float differenceX2 = Vector3.Dot(difference2, _camera.right);
                    float differenceY2 = Vector3.Dot(difference2, _camera.up);
                    Vector3 differenceProjected2 = _camera.right * differenceX2 + _camera.up * differenceY2;
                    targetPositions[i] = activeCredits[i].position + differenceProjected2.normalized * _moveDistance; // Move off-screen
                }

                while (elapsedTime2 < duration2)
                {
                    float t = elapsedTime2 / duration2;
                    float curveValue = _curve.Evaluate(t);
                    for (int i = 0; i < activeCredits.Count; i++)
                    {
                        activeCredits[i].position = Vector3.Lerp(startPositions[i], targetPositions[i], curveValue);
                    }
                    elapsedTime2 += Time.deltaTime;
                    yield return null;
                }

                for (int i = 0; i < activeCredits.Count; i++)
                {
                    activeCredits[i].gameObject.SetActive(false); // Hide the credits after moving them off-screen
                    activeCredits[i].position = startPositions[i]; // Reset position to start position
                }

                activeCredits.Clear(); // Clear the list of active credits
                continue; // Skip to the next credit
            }

            float elapsedTime = 0f;
            float duration = _meanDuration + Random.Range(-_durationVariation, _durationVariation);
            // Calculate target position !!
            // Calculate distance in plane of camera
            Vector3 difference = credit.position - _camera.position;
            float differenceX = Vector3.Dot(difference, _camera.right);
            float differenceY = Vector3.Dot(difference, _camera.up);
            Vector3 differenceProjected = _camera.right * differenceX + _camera.up * differenceY;
            Vector3 startPosition = credit.position + differenceProjected.normalized * _moveDistance; // Move off-screen
            Vector3 targetPosition = credit.position;
            credit.position = startPosition; // Set the initial position off-screen

            credit.gameObject.SetActive(true); // Show the credit object

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                float curveValue = _curve.Evaluate(t);
                credit.position = Vector3.Lerp(startPosition, targetPosition, curveValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            credit.position = targetPosition; // Ensure the final position is set

            activeCredits.Add(credit); // Add the credit to the list of active credits
        }
    }

}
