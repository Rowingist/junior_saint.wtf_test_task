using UnityEngine;

namespace Codebase.Infrastructure.AssetManagement
{
  public class AssetProvider : IAssetProvider
  {
    public GameObject Instantiate(GameObject template, Vector3 at) => 
      Object.Instantiate(template, at, Quaternion.identity);
    
    public GameObject Instantiate(GameObject template, Transform under) => 
      Object.Instantiate(template, under.position, Quaternion.identity, under);

    public GameObject Instantiate(GameObject template, GameObject under) => 
      Object.Instantiate(template, under.transform);

    public GameObject Instantiate(string path)
    {
      var template = Resources.Load<GameObject>(path);
      return Object.Instantiate(template);
    }
    
    public GameObject Instantiate(string path, Vector3 at)
    {
      var template = Resources.Load<GameObject>(path);
      return Object.Instantiate(template, at, Quaternion.identity);
    }

    public GameObject Instantiate(string path, Transform under)
    {
      var template = Resources.Load<GameObject>(path);
      return Object.Instantiate(template, under);
    }
  }
}