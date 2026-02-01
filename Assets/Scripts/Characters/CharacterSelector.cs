using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelector : MonoBehaviour
{
    CharacterController lastHovered;
    public PlayerMovement playerMovement;
    public Camera cam;

    void Update()
    {
        bool isHovering = false;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit rayHit, 1000f);

            if (rayHit.collider != null)
            {
                CharacterController controller = rayHit.transform.GetComponentInParent<CharacterController>();

                if (controller != null)
                {
                    isHovering = true;

                    if (lastHovered != controller)
                    {
                        if (lastHovered != null)
                        {
                            lastHovered.SetHovered(false);
                        }

                        controller.SetHovered(true);
                        lastHovered = controller;
                    }
                }
            }
        }

        if (!isHovering)
        {
            if (lastHovered != null)
            {
                lastHovered.SetHovered(false);
            }

            lastHovered = null;
        }

        if (Input.GetMouseButtonDown(0) && lastHovered != null)
        {
            playerMovement.SetTargetPosition(new Vector2(lastHovered.transform.position.x, lastHovered.transform.position.z));
            WindowHolder.Instance.SpawnIdentityScreen(lastHovered.character, Input.mousePosition);
        }
    }
}
