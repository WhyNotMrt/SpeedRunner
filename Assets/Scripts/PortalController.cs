using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public string levelToLoad;

    private LayerMask lm;

    // Start is called before the first frame update
    void Start()
    {
        lm = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer  == lm) SceneManager.LoadScene(levelToLoad);
    }
}
