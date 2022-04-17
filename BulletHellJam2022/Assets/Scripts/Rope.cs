using UnityEngine;

public class Rope : MonoBehaviour
{
    //Player's Rope Start
    public Rigidbody2D hook;
    
    //Rope Variables
    public GameObject linkPrefab;
    public int links = 7;
    public GameObject playerRope;

    //End of the Player's Tether
    private Rigidbody2D playerRopeEnd;

    private void Start()
    {
        GenerateRope();
    }

    //Generate an inactive rope during runtime. Object pooling
    public void GenerateRope()
    {
        Rigidbody2D previousRB = hook;
        for (int i = 0; i < links; i++)
        {
            GameObject link = Instantiate(linkPrefab, playerRope.transform);
            HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
            joint.connectedBody = previousRB;

            previousRB = link.GetComponent<Rigidbody2D>();
        }
        playerRopeEnd = previousRB;
        playerRope.SetActive(false);
    }
    
    //Connecting between player and interactable during runtime
    public void GenerateRope(HingeJoint2D target)
    {
        playerRope.SetActive(true);
        target.connectedBody = playerRopeEnd;
    }

    //Connecting two non-player objects
    public void GenerateRope(GameObject firstTarget, GameObject secondTarget)
    {
        //Clear current tether
        secondTarget.GetComponent<HingeJoint2D>().connectedBody = null;
        playerRope.SetActive(false);

        //Generate Rope between targets
        Rigidbody2D previousRB = firstTarget.GetComponent<Rigidbody2D>();
        for (int i = 0; i < links; i++)
        {
            GameObject link = Instantiate(linkPrefab, transform);
            HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
            joint.connectedBody = previousRB;

            previousRB = link.GetComponent<Rigidbody2D>();
        }
        secondTarget.GetComponent<HingeJoint2D>().connectedBody = previousRB;
    }
}
