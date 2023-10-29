using UnityEngine;

namespace Codebase.Infrastructure.AssetManagement
{
  public class AssetProvider : IAssetProvider
  {
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