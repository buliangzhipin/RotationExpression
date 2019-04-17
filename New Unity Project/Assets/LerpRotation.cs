using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LerpRotation : MonoBehaviour
{
    private float firstx,firsty,firstz,firstw;

    private float secondx,secondy,secondz,secondw;

    public InputField x,y,z,w;
    public InputField x2,y2,z2,w2;

    public void ShowAnimation()
    {
        firstx = float.Parse(x.text);
        firsty = float.Parse(y.text);
        firstz = float.Parse(z.text);
        firstw = float.Parse(w.text);
        secondx = float.Parse(x2.text);
        secondy = float.Parse(y2.text);
        secondz = float.Parse(z2.text);
        secondw = float.Parse(w2.text);

        var original = new Quaternion(firstx,firsty,firstz,firstw);
        var rotated = new Quaternion(secondx,secondy,secondz,secondw);
        StartCoroutine(Rotation(original,rotated));

    }

    IEnumerator Rotation(Quaternion original,Quaternion rotated)
    {
        for(float t = 0f;t <1f;t+=0.01f)
        {
            this.transform.rotation = Quaternion.Lerp(original,rotated,t);
            yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }
}
