using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseManager : MonoBehaviour
{
    public GameManager gameManager;
    public float shelfSlotsRemaining { get; private set; }
    public GameObject storagePackagePrefab;
    public float casesOfCheese { get; private set; }


    private List<WarehouseShelf> shelvesBuilt = new List<WarehouseShelf>();
  
    // Start is called before the first frame update
    void Start()
    {
        shelfSlotsRemaining = 0;
        casesOfCheese = 0;

        WarehouseShelf.ShelfBuilt += WarehouseShelf_ShelfBuilt;
        WarehouseShelf.ShelfDestroyed += WarehouseShelf_ShelfDestroyed;
        CheeseMaker.MakeCheeseRequest += CheeseMaker_MakeCheeseRequest;
        CheeseMaker.CheeseMade += CheeseMaker_CheeseMade;
        CheeseCollector.PackageInCollector += CheeseCollector_PackageInCollector;
    }

    private void CheeseCollector_PackageInCollector(Package obj)
    {
        Destroy(obj.gameObject);
        gameManager.AddMoney(150);
    }

    private void CheeseMaker_CheeseMade()
    {
        RemovePackagesFromShelves(1);
    }

    private bool CheeseMaker_MakeCheeseRequest()
    {
        if(casesOfCheese > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void WarehouseShelf_ShelfDestroyed(WarehouseShelf obj)
    {
        shelvesBuilt.Remove(obj);
        UpdateShelfSlotsRemaining();
    }

    private void WarehouseShelf_ShelfBuilt(WarehouseShelf obj)
    {
        shelvesBuilt.Add(obj);
        UpdateShelfSlotsRemaining();
    }

    private void UpdateShelfSlotsRemaining()
    {
        shelfSlotsRemaining = 0;
        foreach (WarehouseShelf shelf in shelvesBuilt)
        {
            shelfSlotsRemaining += shelf.shelfSlotsRemaining;
        }
    }

    public void AddPackagesToShelves(int numberOfPackages)
    {
        List<WarehouseShelf> openShelves = new List<WarehouseShelf>();
        openShelves = shelvesBuilt.FindAll(x => !x.IsShelfFull());

        if(openShelves.Count > 0)
        {
            for (int i = 0; i < numberOfPackages; i++)
            {
                int random = UnityEngine.Random.Range(0, openShelves.Count);
                GameObject newPackage = Instantiate(storagePackagePrefab);
                openShelves[random].FillRandomSlot(newPackage);
                openShelves = shelvesBuilt.FindAll(x => x.shelfSlotsRemaining > 0);
            }

            casesOfCheese += numberOfPackages;
        }

        UpdateShelfSlotsRemaining();
    }


    public void RemovePackagesFromShelves(int numberOfPackages)
    {
        List<WarehouseShelf> shelvesWithCheese = new List<WarehouseShelf>();
        shelvesWithCheese = shelvesBuilt.FindAll(x => !x.IsShelfEmpty());

        if (shelvesWithCheese.Count > 0)
        {
            for (int i = 0; i < numberOfPackages; i++)
            {
                int random = UnityEngine.Random.Range(0, shelvesWithCheese.Count);
                shelvesWithCheese[random].RemoveRandomSlot();
                shelvesWithCheese = shelvesBuilt.FindAll(x => !x.IsShelfEmpty());
            }

            casesOfCheese -= numberOfPackages;
        }

        UpdateShelfSlotsRemaining();
    }
}
