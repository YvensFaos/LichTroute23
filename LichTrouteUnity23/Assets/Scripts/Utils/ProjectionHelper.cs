using UnityEngine;

namespace Utils
{
    public class ProjectionHelper : MonoBehaviour
    {
        [SerializeField]
        private GameObject windowsObject;
        [SerializeField]
        private GameObject projectionDebugViewer;
        [SerializeField] 
        private Vector3 resetPosition;

        private Transform windowTransform;

        private void Awake()
        {
            windowTransform = windowsObject.transform;
        }

        private void Update()
        {
            //Toggle the Project Debug Viewer visibility. 
            if (Input.GetKeyDown(KeyCode.D))
            {
                projectionDebugViewer.SetActive(!projectionDebugViewer.activeSelf);
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetResolution();
            }

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                var scale = 0.01f;
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || 
                    Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand))
                {
                    scale = .005f;
                }
                var localScale = windowTransform.localScale;
                
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    localScale.x -= scale;
                    windowTransform.localScale = localScale;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    localScale.x += scale;
                    windowTransform.localScale = localScale;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    localScale.y += scale;
                    windowTransform.localScale = localScale;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    localScale.y -= scale;
                    windowTransform.localScale = localScale;
                }
            }
            else
            {
                var move = 1.0f;
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || 
                    Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand))
                {
                    move = 0.1f;
                }
            
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    windowTransform.Translate(Vector3.left * move);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    windowTransform.Translate(Vector3.right * move);
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    windowTransform.Translate(Vector3.up * move);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    windowTransform.Translate(Vector3.down * move);
                }
            }
        }

        public override string ToString()
        {
            var transformString = $"{windowTransform.position.ToString()} {windowTransform.localScale.ToString()}";
            return transformString;
        }

        public void ResetResolution()
        {
            windowTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            windowTransform.position = resetPosition;
        }
    }
}
