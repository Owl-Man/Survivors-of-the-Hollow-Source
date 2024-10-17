using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Button removeResourceBtn;

    private List<Resource> _resources = new List<Resource>();

    private void Start()
    {
        CameraFocus.OnShelterEnter.AddListener(DisableRemoveResourceBtn);
        CameraFocus.OnShelterExit.AddListener(EnableRemoveResourceBtn);
    }

    public void AddResource(Resource resource)
    {
        _resources.Add(resource);

        UpdateState();
    }

    public void RemoveResource(Resource resource)
    {
        if (_resources.Count == 0) return;
        
        _resources.Remove(resource);
        
        resource.Disconnect();

       UpdateState();
    }

    public void RemoveLastResource()
    {
        if (_resources.Count == 0) return;
        
        _resources[^1].Disconnect();
        
        _resources.RemoveAt(_resources.Count - 1);

        UpdateState();
    }

    private void UpdateState() => removeResourceBtn.interactable = _resources.Count != 0;

    private void EnableRemoveResourceBtn() => removeResourceBtn.gameObject.SetActive(true);
    
    private void DisableRemoveResourceBtn() => removeResourceBtn.gameObject.SetActive(false);
}