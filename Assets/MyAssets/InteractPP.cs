using UnityEngine;
using System.Collections;

public class InteractPP : CAVE2Interactable {

    public enum HoldingStyle { ButtonPress, ButtonHold };

    [SerializeField]
    bool grabbed;

    [SerializeField]
    CAVE2.Button grabButton = CAVE2.Button.Button3;

    [SerializeField]
    CAVE2.InteractionType grabStyle = CAVE2.InteractionType.Any;

    [SerializeField]
    HoldingStyle holdInteraction = HoldingStyle.ButtonHold;

    [SerializeField]
    bool allowWandCollision = true;

    [SerializeField]
    bool centerOnWand = false;

    bool usedGravity;

    [SerializeField]
    RigidbodyConstraints constraints;

    FixedJoint joint;

    bool wasGrabbed;

    Queue previousPositions = new Queue();

    [SerializeField]
    Transform grabber;
    Collider[] grabberColliders;

    int grabbingWandID;

    [Header("Visuals")]
    GameObject pointingOverHighlight;
    new MeshRenderer renderer;

    [SerializeField]
    bool showPointingOver = true;

    [SerializeField]
    float highlightScaler = 1.05f;

    [SerializeField]
    Mesh defaultMesh;

    [SerializeField]
    Material pointingOverMaterial;

    Color originalPointingMatColor;

    [SerializeField]
    bool showTouchingOver = true;

    [SerializeField]
    Material touchingOverMaterial;

    Color originalTouchingMatColor;

    public GameObject pottyOpen;
    public GameObject pottyClosed;
    public bool doorOpened;


    private void Start()
    {
        // Visuals
        pointingOverHighlight = new GameObject("wandHighlight");
        pointingOverHighlight.transform.parent = transform;
        pointingOverHighlight.transform.position = transform.position;
        pointingOverHighlight.transform.rotation = transform.rotation;
        pointingOverHighlight.transform.localScale = Vector3.one * highlightScaler;

        if (defaultMesh == null)
        {
            defaultMesh = GetComponent<MeshFilter>().mesh;
        }
        pointingOverHighlight.AddComponent<MeshFilter>().mesh = defaultMesh;
        MeshCollider wandCollider = gameObject.AddComponent<MeshCollider>();
        wandCollider.inflateMesh = defaultMesh;
        wandCollider.convex = true;
        wandCollider.isTrigger = true;

        renderer = pointingOverHighlight.AddComponent<MeshRenderer>();

        if (pointingOverMaterial == null)
        {
            // Create a basic highlight material
            pointingOverMaterial = new Material(Shader.Find("Standard"));
            pointingOverMaterial.SetColor("_Color", new Color(0, 1, 1, 0.25f));
            pointingOverMaterial.SetFloat("_Mode", 3); // Transparent
            pointingOverMaterial.SetFloat("_Glossiness", 0);
        }
        else
        {
            pointingOverMaterial = new Material(pointingOverMaterial);
        }
        if (touchingOverMaterial == null)
        {
            // Create a basic highlight material
            touchingOverMaterial = new Material(Shader.Find("Standard"));
            touchingOverMaterial.SetColor("_Color", new Color(0, 1, 1, 0.25f));
            touchingOverMaterial.SetFloat("_Mode", 3); // Transparent
            touchingOverMaterial.SetFloat("_Glossiness", 0);
        }
        else
        {
            touchingOverMaterial = new Material(touchingOverMaterial);
        }
        originalPointingMatColor = pointingOverMaterial.color;
        originalTouchingMatColor = touchingOverMaterial.color;

        renderer.sharedMaterial = pointingOverMaterial;

        renderer.enabled = false;
    }
    void Update()
    {
        // Interaction
        UpdateWandOverTimer();

        if(holdInteraction == HoldingStyle.ButtonHold && CAVE2.Input.GetButtonUp(grabbingWandID, grabButton) && grabbed )
        {
            OnWandGrabRelease();
        }

        // Visuals
        if (showPointingOver)
        {
            if (wandPointing)
            {
                renderer.sharedMaterial = pointingOverMaterial;
                pointingOverMaterial.color = originalPointingMatColor;
                renderer.enabled = true;
            }
            else
            {
                renderer.enabled = false;
            }
        }
        if (showTouchingOver)
        {
            if (wandTouching)
            {
                renderer.sharedMaterial = touchingOverMaterial;
                touchingOverMaterial.color = originalTouchingMatColor;
                renderer.enabled = true;
            }
            else if(!wandPointing)
            {
                renderer.enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        if( grabbed )
        {
            previousPositions.Enqueue(GetComponent<Rigidbody>().position);

            // Save only the last 5 frames of positions
            if(previousPositions.Count > 5)
            {
                previousPositions.Dequeue();
            }
        }
        else if(wasGrabbed)
        {
            Vector3 throwForce = Vector3.zero;
            for (int i = 0; i < previousPositions.Count; i++ )
            {
                Vector3 s1 = (Vector3)previousPositions.Dequeue();
                Vector3 s2 = GetComponent<Rigidbody>().position;
                
                if ( previousPositions.Count > 0 )
                    s2 = (Vector3)previousPositions.Dequeue();
                throwForce += (s2 - s1);
            }
            GetComponent<Rigidbody>().AddForce(throwForce * 10, ForceMode.Impulse);
            wasGrabbed = false;
        }
    }

    new void OnWandButtonDown(CAVE2.WandEvent evt)
    {
        if( evt.button == grabButton)
        {
            if (!grabbed && (evt.interactionType == grabStyle || grabStyle == CAVE2.InteractionType.Any))
            {
                grabber = CAVE2.GetWandObject(evt.wandID).transform;
                OnWandGrab();
                grabbingWandID = evt.wandID;
            }
            else if(grabbed && holdInteraction == HoldingStyle.ButtonPress)
            {
                OnWandGrabRelease();
            }
        }
    }

    void OnWandGrab()
    {

        if (GetComponent<Rigidbody>() && transform.parent != grabber )
        {
        	            	
            // Check if grabbing object already is grabbing something else
            if (grabber.GetComponentInChildren<CAVE2WandInteractor>().GrabbedObject(gameObject))
            {
            	
                // Disable collisions between grabber and collider while held
                grabberColliders = grabber.root.GetComponentsInChildren<Collider>();
                foreach (Collider c in grabberColliders)
                {
                    Physics.IgnoreCollision(c, GetComponent<Collider>(), true);
                }

                if (centerOnWand)
                {
                   // transform.position = grabber.transform.position;
                }

               // usedGravity = GetComponent<Rigidbody>().useGravity;
               // GetComponent<Rigidbody>().useGravity = false;
                //joint = gameObject.AddComponent<FixedJoint>();
               // joint.connectedBody = grabber.GetComponentInChildren<Rigidbody>();
               // joint.breakForce = float.PositiveInfinity;
               // joint.breakTorque = float.PositiveInfinity;
            }

        }
                    
           	
        if (doorOpened == true){
        	pottyClosed.SetActive(true);
        	pottyOpen.SetActive(false);
        	doorOpened = false;
        }
        if (doorOpened == false){
        	pottyClosed.SetActive(false);
        	pottyOpen.SetActive(true);
        	doorOpened = true;
        }
 
        grabbed = true;
    }


    
  private  void OnWandGrabRelease()
    {
    	
        grabber.GetComponentInChildren<CAVE2WandInteractor>().ReleaseObject(gameObject);

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().useGravity = usedGravity;
            Destroy(joint);
        }

        // Re-enable collisions between grabber and collider after released
        if (grabberColliders != null)
        {
            foreach (Collider c in grabberColliders)
            {
                Physics.IgnoreCollision(c, GetComponent<Collider>(), false);
            }
        }

        grabbed = false;
        wasGrabbed = true;
    }



    
    private void OnCollisionEnter(Collision collision)
    {
        if (!allowWandCollision && collision.gameObject.GetComponent<CAVE2WandInteractor>())
        {
            Collider[] grabberColliders = collision.transform.root.GetComponentsInChildren<Collider>();
            foreach (Collider c in grabberColliders)
            {
                Physics.IgnoreCollision(c, GetComponent<Collider>(), true);
            }
        }
    }
    
}
