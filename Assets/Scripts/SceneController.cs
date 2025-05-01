using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public void gotoLevel1Game()
    {
        SceneManager.LoadScene(SceneData.level1game);
    }

    public void gotoLevel2Game()
    {
        SceneManager.LoadScene(SceneData.level2game);
    }

    public void gotoPaddyField()
    {
        SceneManager.LoadScene(SceneData.paddyfield);
    }
    

    public void gotoBadge()
    {
        SceneManager.LoadScene(SceneData.level1badge);
    }

    public void gotoLevel2Game2()
    {
        SceneManager.LoadScene(SceneData.level2game2);
    }

    public void gotoLevel2Game3()
    {
        SceneManager.LoadScene(SceneData.level2game3);
    }


}
