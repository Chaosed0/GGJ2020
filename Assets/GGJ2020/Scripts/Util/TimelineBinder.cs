using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;
using Cinemachine.Timeline;

public class TimelineBinder : MonoBehaviour
{
    [System.Serializable]
    public class Binding
    {
        [System.Serializable]
        public enum Type
        {
            GenericTrack,
            AnimationTrack,
            CinemachineTrack,
            CinemachineClip,
        }

        public string name;
        public Type type;

        [Sirenix.OdinInspector.ShowIf("type", Type.CinemachineTrack)]
        public CinemachineBrain cinemachineBrain;

        [Sirenix.OdinInspector.ShowIf("type", Type.CinemachineClip)]
        public CinemachineVirtualCamera cinemachineVCam;

        [Sirenix.OdinInspector.ShowIf("type", Type.AnimationTrack)]
        public Animator animator;

        [Sirenix.OdinInspector.ShowIf("type", Type.GenericTrack)]
        public GameObject gameObject;
    }

    [SerializeField]
    private List<Binding> bindings = new List<Binding>();

    private Dictionary<string, Binding> bindingMap = new Dictionary<string, Binding>();

    private void Awake()
    {
        foreach (Binding binding in bindings)
        {
            bindingMap[binding.name] = binding;
        }
    }

    public void BindToPlayableDirector(PlayableDirector playableDirector)
    {
        foreach (var output in playableDirector.playableAsset.outputs)
        {
            Binding binding = null;
            if (output.sourceObject is CinemachineTrack)
            {
                CinemachineTrack cinemachineTrack = output.sourceObject as CinemachineTrack;
                if (bindingMap.TryGetValue(cinemachineTrack.name, out binding))
                {
                    playableDirector.SetGenericBinding(cinemachineTrack, binding.cinemachineBrain);
                }

                foreach (var clip in cinemachineTrack.GetClips())
                {
                    if (bindingMap.TryGetValue(clip.displayName, out binding))
                    {
                        var cinemachineShot = clip.asset as CinemachineShot;
                        playableDirector.SetReferenceValue(cinemachineShot.VirtualCamera.exposedName, binding.cinemachineVCam);
                    }
                }
            }
            else if (output.sourceObject is AnimationTrack)
            {
                AnimationTrack animationTrack = output.sourceObject as AnimationTrack;
                if (bindingMap.TryGetValue(animationTrack.name, out binding))
                {
                    playableDirector.SetGenericBinding(animationTrack, binding.animator);
                }
            }
            else if (output.sourceObject != null)
            {
                if (bindingMap.TryGetValue(output.sourceObject.name, out binding))
                {
                    playableDirector.SetGenericBinding(output.sourceObject, binding.gameObject);
                }
            }
        }
    }
}
