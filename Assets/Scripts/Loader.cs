using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader {
    public enum Scene {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    private static Scene _targetScene;

    public static void LoadScene(Scene targetScene) {
        _targetScene = targetScene;

        SceneManager.LoadScene(nameof(Scene.LoadingScene));
    }

    public static void LoaderCallback() {
        SceneManager.LoadScene(_targetScene.ToString());
    }
}