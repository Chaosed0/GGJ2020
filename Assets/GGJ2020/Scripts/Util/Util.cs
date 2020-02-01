using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Util
{
    private static System.Random rng = new System.Random();

    public static List<Transform> GetAllChildren(Transform transform)
    {
        var children = new List<Transform>();
        GetAllChildrenRecursive(transform, children);
        return children;
    }

    private static void GetAllChildrenRecursive(Transform transform, List<Transform> children)
    {
        foreach (Transform child in transform)
        {
            children.Add(child);
            GetAllChildrenRecursive(child, children);
        }
    }

    public static void OnNextFrame(MonoBehaviour mb, System.Action action)
    {
        mb.StartCoroutine(OnNextFrameEnumerator(action));
    }

    public static IEnumerator OnNextFrameEnumerator(System.Action action)
    {
        yield return null;
        action();
    }

    public static Coroutine ExecuteAfterTime(MonoBehaviour mb, float time, System.Action action)
    {
        return mb.StartCoroutine(ExecuteAfterTimeEnumerator(time, action));
    }

    public static IEnumerator ExecuteAfterTimeEnumerator(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    public static float XZDistance(Vector3 v1, Vector3 v2)
    {
        v1.y = 0f;
        v2.y = 0f;
        return Vector3.Distance(v1, v2);
    }

    public static Gradient GradientWithSingleColor(Color color)
    {
        Gradient gradient = new Gradient();
        gradient.colorKeys = new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) };
        gradient.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(color.a, 0f), new GradientAlphaKey(color.a, 1f) };
        return gradient;
    }

    public static Vector3 CameraLookAtPositionOnXZ(Camera camera, float y)
    {
        float d = (y - camera.transform.position.y / camera.transform.forward.y);
        return camera.transform.forward * d + camera.transform.position;
    }

    public static Vector3 WithZeroY(Vector3 vec)
    {
        vec.y = 0f;
        return vec;
    }

    public static T RandomElementOf<T>(List<T> list) where T : class
    {
        if (list.Count == 0)
        {
            return null;
        }

        return list[(int)UnityEngine.Random.Range(0f, list.Count)];
    }

    public static void Shuffle<T>(IList<T> list)
    {
        Shuffle(list, Util.rng);
    }

    public static void Shuffle<T>(IList<T> list, System.Random random)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static Vector3 EaseInOut(Vector3 initial, Vector3 final, float time, float duration)
    {
        return new Vector3(Util.EaseInOut(initial.x, final.x, time, duration),
                           Util.EaseInOut(initial.y, final.y, time, duration),
                           Util.EaseInOut(initial.z, final.z, time, duration));
    }

    public static float EaseInOut(float initial, float final, float time, float duration)
    {
        float change = final - initial;
        time /= duration / 2;
        if (time < 1f) return change / 2 * time * time + initial;
        time--;
        return -change / 2 * (time * (time - 2) - 1) + initial;
    }

    public static float EaseIn(float initial, float final, float time, float duration)
    {
        time /= duration;
        float change = final - initial;
        return change * time * time + initial;
    }

    public static float EaseOut(float initial, float final, float time, float duration)
    {
        time /= duration;
        float change = final - initial;
        return change * time * (2 - time) + initial;
    }

    public static T[] ToArray<T>(IEnumerable<T> enumerable)
    {
        T[] arr = new T[enumerable.Count()];

        int index = 0;
        foreach (T elem in enumerable)
        {
            arr[index++] = elem;
        }

        return arr;
    }

    public static List<T> FindAllNonUnityObjectsOfType<T>() where T : class
    {
        var list = new List<T>();

        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            list.AddRange(FindNonUnityObjectsOfTypeInScene<T>(scene));
        }

        return list;
    }

    public static IEnumerable<T> FindNonUnityObjectsOfTypeInScene<T>(UnityEngine.SceneManagement.Scene scene) where T : class
    {
        Debug.Assert(scene != null);

        foreach (Transform transform in AllTransformsInScene(scene))
        {
            foreach (Component component in transform.GetComponents(typeof(T)))
            {
                T val = component as T;
                if (val != null)
                {
                    yield return val;
                }
            }
        }
    }

    public static List<T> FindObjectsOfTypeInScene<T>(UnityEngine.SceneManagement.Scene scene) where T : UnityEngine.Object
    {
        Debug.Assert(scene != null);
        var list = new List<T>();

        foreach (Transform transform in AllTransformsInScene(scene))
        {
            T val = transform.GetComponent<T>();
            if (val != null)
            {
                list.Add(val);
            }
        }

        return list;
    }

    public static T FindObjectOfTypeInScene<T>(UnityEngine.SceneManagement.Scene scene) where T : UnityEngine.Object
    {
        Debug.Assert(scene != null);

        foreach (Transform transform in AllTransformsInScene(scene))
        {
            T val = transform.GetComponent<T>();
            if (val != null)
            {
                return val;
            }
        }

        return null;
    }

    public static IEnumerable<Transform> AllTransformsInScene(Scene scene)
    {
        Debug.Assert(scene != null);

        foreach (GameObject go in scene.GetRootGameObjects())
        {
            foreach (Transform transform in AllTransformsUnder(go.transform))
            {
                yield return transform;
            }
        }
    }

    private static Queue<Transform> transformQueue = new Queue<Transform>();
    public static IEnumerable<Transform> AllTransformsUnder(Transform transform)
    {
        transformQueue.Clear();
        transformQueue.Enqueue(transform);

        while (transformQueue.Count > 0)
        {
            Transform dequeued = transformQueue.Dequeue();
            yield return dequeued;
            foreach (Transform child in dequeued)
            {
                transformQueue.Enqueue(child);
            }
        }
    }

    public static Vector2 i2f(Vector2Int vec2i)
    {
        return new Vector2(vec2i.x, vec2i.y); 
    }

    public static string MakeIdentifier()
    {
        return MakeIdentifier(rng);
    }

    public static string MakeIdentifier(System.Random random)
    {
        byte[] buffer = new byte[16];
        random.NextBytes(buffer);
        return new System.Guid(buffer).ToString();
    }

    public static float GetAnimationLength(Animator animator, string clipName)
    {
        if (animator == null)
        {
            return 0f;
        }

        float time = -1f;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == clipName)
            {
                time = ac.animationClips[i].length;
                break;
            }
        }

        if (time < 0)
        {
            Debug.LogWarning($"Couldn't find animation with clip name {clipName}, ensure that's the clip's name (rather than the state)");
            return 0f;
        }

        return time;
    }

    // Integrate area under AnimationCurve between start and end time
    public static float IntegrateCurve(AnimationCurve curve, float startTime, float endTime, int steps)
     {
         return Integrate(curve.Evaluate, startTime, endTime, steps);
     }
 
     // Integrate function f(x) using the trapezoidal rule between x=x_low..x_high
     public static float Integrate(Func<float, float> f, float x_low, float x_high, int N_steps)
     {
         float h = (x_high - x_low) / N_steps;
         float res = (f(x_low) + f(x_high)) / 2;
         for (int i = 1; i < N_steps; i++)
         {
             res += f(x_low + i * h);
         }
         return h * res;
     }

    public static Vector3 three(Vector2 vec)
    {
        return new Vector3(vec.x, vec.y);
    }

    public static Vector2 two(Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    public static IEnumerator ExecuteAfterAnimationEnumerator(Animator animator, string animationStateName, float timeout, System.Action action)
    {
        float timer = 0f;
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName))
        {
            yield return null;
            timer += Time.deltaTime;

            if (timer >= timeout)
            {
                Debug.LogWarning($"{animator.name} hit timeout while waiting to enter animation {animationStateName}, is the animator state name set correctly?");
                break;
            }
        }

        timer = 0f;
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.IsName(animationStateName) &&
            (stateInfo.normalizedTime < 1f && !Mathf.Approximately(stateInfo.normalizedTime, 1f)))
        {
            yield return null;
            timer += Time.deltaTime;

            if (timer >= timeout)
            {
                Debug.LogWarning($"{animator.name} hit timeout while in animation {animationStateName}, is the animation looping?");
                break;
            }

            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        action?.Invoke();
    }
}
