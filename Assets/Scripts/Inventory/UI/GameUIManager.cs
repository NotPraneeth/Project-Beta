using Inventory.Core;
using Inventory.UI;
using UnityEngine;

namespace UI
{
    public enum UIMode
    {
        Gameplay,
        Storage,
        Menu
    }

    public sealed class GameUIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameplayHUD;

        [SerializeField]
        private GameObject storagePanel;

        [SerializeField]
        private GameObject menuPanel;

        [SerializeField]
        private InventoryUI storageInventoryUI;

        [SerializeField]
        private PlayerInputHandler playerInput;

        public UIMode CurrentMode { get; private set; }

        private void Start()
        {
            gameplayHUD.SetActive(true);
            storagePanel.SetActive(false);
            menuPanel.SetActive(false);

            CurrentMode = UIMode.Gameplay;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerInput.EnableInput();
        }

        /// <summary>
        /// Opens a storage inventory.
        /// </summary>
        public void ShowStorage(InventoryComponent inventory)
        {
            CurrentMode = UIMode.Storage;

            playerInput.DisableInput();

            storageInventoryUI.SetInventory(inventory);

            storagePanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void HideStorage()
        {
            CurrentMode = UIMode.Gameplay;

            playerInput.EnableInput();

            storagePanel.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void ToggleStorage(InventoryComponent inventory)
        {
            if (CurrentMode == UIMode.Storage)
            {
                HideStorage();
            }
            else
            {
                ShowStorage(inventory);
            }
        }

        public void OpenMenu()
        {
            CurrentMode = UIMode.Menu;

            playerInput.DisableInput();

            menuPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void CloseMenu()
        {
            CurrentMode = UIMode.Gameplay;

            playerInput.EnableInput();

            menuPanel.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}