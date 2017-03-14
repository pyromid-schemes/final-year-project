using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World
{
    public class Torch : MonoBehaviour
    {
        private GameObject offTorch;
        private GameObject onTorch;
        private bool lit;

        private readonly float SPAWN_CHANCE = 0.4f;

        void Start()
        {
            GetSubTorchModels();
            if (Random.value < SPAWN_CHANCE) {
                onTorch.SetActive(true);
                lit = true;
            }
            else {
                offTorch.SetActive(true);
            }
        }

        private void GetSubTorchModels()
        {
            foreach (Transform child in transform) {
                if (child.name.Equals("torch_1_empty")) {
                    offTorch = child.gameObject;
                } else if (child.name.Equals("torch_1")) {
                    onTorch = child.gameObject;
                }
            }
            if (offTorch == null || onTorch == null) {
                throw new ArgumentException("This parent torch has doesn't contain a torch and empty torch.");
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Equals("HeldTorch") && !lit) {
                offTorch.SetActive(false);
                onTorch.SetActive(true);
                lit = true;
            }
        }
    }
}