using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Control
{
    public enum State
    {
        Move, Scale, Adjust
    }
    
    public class CanvasPlacerControl : MonoBehaviour
    {
        [SerializeField]
        private List<CanvasControlledElement> controlled;
        [SerializeField]
        private float moveSpeed = 1.0f;
        [SerializeField]
        private float scaleSpeed = 1.0f;
        [SerializeField, ReadOnly]
        private CanvasControlledElement currentElement;
        [SerializeField] 
        private State currentState;

        private int index;
        
        private void Start()
        {
            index = 0;
            currentElement = controlled[index];
        }

        private void Update()
        {
            void UpdateElement()
            {
                currentElement = controlled[index]; 
                currentElement.AnimateOutline();
            }
            
            ChangeState();
            
            if (Input.GetKeyUp(KeyCode.Q))
            {
                index = (++index) % controlled.Count;
                UpdateElement();
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                index = (--index) % controlled.Count;
                if (index < 0)
                {
                    index = controlled.Count - 1;
                }
                UpdateElement();
            }
            
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");

            switch (currentState)
            {
                case State.Move:
                {
                    var currentSpeed = moveSpeed;

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        currentSpeed *= 2.0f;
                    }

                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        currentSpeed /= 2.0f;
                    }
                    if (Mathf.Abs(horizontalInput) > 0.0f || Mathf.Abs(verticalInput) > 0.0f)
                    {
                        var movement = new Vector2(horizontalInput, verticalInput) * (currentSpeed * Time.deltaTime);
                        controlled[index].Move(movement);    
                    }
                }
                    break;
                case State.Scale:
                    var currentScale = scaleSpeed;

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        currentScale *= 2.0f;
                    }

                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        currentScale /= 2.0f;
                    }
                    if (Mathf.Abs(horizontalInput) > 0.0f || Mathf.Abs(verticalInput) > 0.0f)
                    {
                        var scale = new Vector3(horizontalInput, verticalInput, 0.0f) * (currentScale * Time.deltaTime);
                        controlled[index].Scale(scale);    
                    }
                    break;
                case State.Adjust:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
           
        }

        private void ChangeState()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                currentState = State.Move;
            }
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                currentState = State.Scale;
            }
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                currentState = State.Adjust;
            }
        }
    }
}
