namespace Menu.Events
{
	using UnityEngine;
	using System.Collections;
	using UnityEngine.UI;
	using UnityEngine.SceneManagement;

	public class SceneLoaderEvents : MonoBehaviour {

		private bool loadScene = false;
		public string scene;
		public Text loadingText;

		void Update() {

			if (loadScene == false) {
				loadScene = true;
				StartCoroutine(LoadNewScene());
			}

			if (loadScene == true) {
				loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

			}

		}

		IEnumerator LoadNewScene() {
			AsyncOperation async = SceneManager.LoadSceneAsync(scene);

			while (!async.isDone) {
				yield return null;
			}

		}

	}
}