using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// this class to controller gameplay Manager
// refer to https://awesometuts.com/blog/singletons-unity/ as reference
public class GameplayManager : GenericSingleton<GameplayManager>
{


    [SerializeField] private PostProcessVolume _postProcessVolume;
    private static Vignette _vignette;

    GameplayManager()
    {
        _postProcessVolume.profile.TryGetSettings(out _vignette);
    }
    public static void loadLevel1()
    {

    }

    public static void vignette(bool value)
    {
        _vignette.active = value;
    }


}
