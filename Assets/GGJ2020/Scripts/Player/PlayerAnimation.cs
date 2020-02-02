using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private Player player = null;

    [SerializeField]
    private AnimationCurve moveAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [SerializeField]
    private AnimationCurve rejectAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [SerializeField]
    private float animationTime = 0.25f;

    private Coroutine coroutine = null;

    private void OnValidate()
    {
        this.Autofill(ref spriteRenderer, true);
        this.Autofill(ref player, true, AutofillMode.LookInParents);
    }

    private void Start()
    {
        player.OnPositionChanged += OnPositionChanged;
        player.OnPositionRejected += OnPositionRejected;
    }

    private void OnPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(AnimateCoroutine(oldPosition, newPosition, moveAnimationCurve));
    }

    private void OnPositionRejected(Vector3 oldPosition, Vector3 newPosition, Collider2D collider)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(AnimateCoroutine(oldPosition, newPosition, rejectAnimationCurve));
    }

    private IEnumerator AnimateCoroutine(Vector3 oldPosition, Vector3 newPosition, AnimationCurve animationCurve)
    {
        float elapsed = 0f;
        while (elapsed < animationTime)
        {
            float lerp = animationCurve.Evaluate(elapsed / animationTime);
            spriteRenderer.transform.position = Vector3.Lerp(oldPosition, newPosition, lerp);

            yield return null;

            elapsed += Time.deltaTime;
        }
    }
}
