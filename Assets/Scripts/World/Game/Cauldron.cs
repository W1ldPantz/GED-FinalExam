using System;
using System.Collections;
using System.Collections.Generic;
using Effects;
using UnityEngine;
using Utility;

namespace World.Game
{

    public struct PlacedIngredient
    {
        public readonly Prop MyProp;
        public readonly Rigidbody MyRb;
        public PlacedIngredient(Prop prop, Rigidbody rb)
        {
            MyProp = prop;
            MyRb = rb;
        }
    }

    public class Cauldron : MonoBehaviour
    {
        /*TODO:
        1. Worldspace UI
          - Factory to make the UI
          - UI will show up on Ingredients, Potions and the Cauldron (Only if the Cauldron has items in it - Conditional)
          - The Cauldron UI will contain two buttons (Undo and Brew)
          - On brew, just spawn in some random basic potion
        */

        [SerializeField] private int maxIngredients = 3;
        [SerializeField] private Potion potion;
      //  [SerializeField] private CauldronFactory factory;

        private Material _material;
        private Collider _trigger;
        
        private Stack<PlacedIngredient> ingredients = new();
        
        public Stack<PlacedIngredient> Ingredients => ingredients;
        
        private static readonly Collider[] Results = new Collider[7];

        private static readonly WaitForSeconds Delay = new WaitForSeconds(1);

        public Action OnIngredientsChanged;
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            _material = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material;
            _trigger = transform.GetChild(1).GetComponent<Collider>();
            _material.SetFloat(StaticUtility.FillID, (float)ingredients.Count / maxIngredients);
            OnIngredientsChanged += () => _material.SetFloat(StaticUtility.FillID, (float)ingredients.Count / maxIngredients);
        }

        private void OnTriggerEnter(Collider other)
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb == null || !rb.TryGetComponent(out Prop myProp) || !myProp.gameObject.activeInHierarchy) return;


            if (ingredients.Count >= maxIngredients)
            {
                Eject(myProp, rb);
                return;
            }

            myProp.gameObject.SetActive(false);
            ingredients.Push(new PlacedIngredient(myProp, rb));

            if (ingredients.Count > 2)
            {
                Potion myPotion = Instantiate(potion, transform.position, Quaternion.identity);
                myPotion.InitializeEffect(EffectFactory.BuildEffect(ref ingredients));
                gameObject.SetActive(false);
                
            }

            OnIngredientsChanged.Invoke();
        }

        
        
        private void Eject(Prop prop, Rigidbody rb)
        {
            rb.AddForce(rb.linearVelocity * -5 , ForceMode.Impulse);
        }

        [ContextMenu("Eject")]
        public void Undo()
        {
            if (!ingredients.TryPop(out PlacedIngredient current)) return; 
            Eject(current.MyProp, current.MyRb);
            OnIngredientsChanged.Invoke();
            StopAllCoroutines();
            StartCoroutine(HandleCooldown());
        }

        IEnumerator HandleCooldown()
        {
            _trigger.enabled = false;
            yield return Delay;
            _trigger.enabled = true;

            int size = Physics.OverlapBoxNonAlloc(_trigger.transform.position, _trigger.bounds.extents, Results,
                _trigger.transform.rotation);

            if (size != 0)
            {
                for (int i = 0; i < size; i++)
                {
                    Rigidbody rb = Results[i].attachedRigidbody;
                    if (rb && Results[i].attachedRigidbody.TryGetComponent(out Prop prop) &&
                        prop.gameObject.activeInHierarchy)
                    {
                        if (ingredients.Count < maxIngredients)
                        {
                            ingredients.Push(new PlacedIngredient(prop, rb));
                            OnIngredientsChanged.Invoke();
                        }
                    }
                }
            }
        }
    }
}
