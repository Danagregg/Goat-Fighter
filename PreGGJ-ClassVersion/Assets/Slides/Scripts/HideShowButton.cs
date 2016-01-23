using UnityEngine;

public class HideShowButton : MonoBehaviour
{
    [SerializeField]    GameObject[]    _gameObjToToggle;

    public void Execute()
    {
        foreach( var gameObj in _gameObjToToggle )
        {
            gameObj.SetActive( !gameObj.activeSelf );
        }
    }
}
