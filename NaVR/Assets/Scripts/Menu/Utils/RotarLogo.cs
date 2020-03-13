using UnityEngine;

public class RotarLogo : MonoBehaviour
{
    public GameObject logo, logo2;
    int x = 0;	

	void Update ()
    {
        if(logo != null)
            logo.transform.Rotate(0, 80 * Time.deltaTime, 0);
        if (logo2 != null)
            logo2.transform.Rotate(0, 80 * Time.deltaTime, 0);
    }
}
