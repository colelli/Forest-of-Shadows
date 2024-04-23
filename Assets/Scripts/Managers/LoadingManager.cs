using UnityEngine.SceneManagement;

public static class LoadingManager {

    public enum Scene {
        MainMenuScene,
        LoadingScene,
        LobbyScene,
        GameScene
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene) {
        LoadingManager.targetScene = targetScene;
        AudioManager.Instance.StopAllSounds();
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoadingManagerCallback() {
        SceneManager.LoadScene(targetScene.ToString());

        if(targetScene.ToString() == Scene.GameScene.ToString()) {
            GameManager.Instance.ChangeState(GameManager.GameState.GamePlaying);
        }
    }

}
