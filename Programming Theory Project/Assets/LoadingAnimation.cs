using System.Collections;
using TMPro;
using UnityEngine;


public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    private WaitForSeconds halfSecond = new WaitForSeconds(0.5f);

    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        while (true)
        {
            loadingText.text = "Loading";
            yield return halfSecond;
            loadingText.text = "Loading.";
            yield return halfSecond;
            loadingText.text = "Loading..";
            yield return halfSecond;
            loadingText.text = "Loading...";
            yield return halfSecond;
        }
    }
}
