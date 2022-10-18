using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#nullable enable

public class GameManagement : MonoBehaviour
{
    [field: SerializeField] public Transform? FromPosition { get; set; }    
    [field: SerializeField] public Transform? ToPosition { get; set; }  
    
    [field: SerializeField, Range(0.1f, 10f)] public float Speed { get; set; }  
    [field: SerializeField] public float CreateInterval { get; set; } 

    [field: SerializeField] public GameObject? ObjectPrefab { get; set; }
    
    
    public void ChangeSpeed(string Speed) {
        if(float.TryParse(Speed, out float value))
            this.Speed = value;
    }
    public void ChangeInterval(string Interval) {
        if(float.TryParse(Interval, out float value))
            this.CreateInterval = value;
    }
    public void ChangeDistance(string Distance) {
        if(float.TryParse(Distance, out float value))
        if(ToPosition!=null)
        if(FromPosition!=null)
            ToPosition.localPosition = 
                FromPosition.position + 
                new Vector3(0, value * 100, 0);
    }



    private void Start() => StartCoroutine(CubeCreator());
    IEnumerator CubeCreator(){
        while(true){
            StartCoroutine(MoveToPoint());
            yield return new WaitForSecondsRealtime(CreateInterval);
        }
    }

    IEnumerator MoveToPoint(){
        if(ObjectPrefab == null) yield break;
        
        var TargetObject = GameObject.Instantiate(ObjectPrefab, FromPosition?.position ?? new Vector3(0, 0, 0), Quaternion.identity);
        var Speed = this.Speed;
        var ToPosition = this.ToPosition?.position ?? new Vector3(0, 0, 50);

        yield return new WaitForSecondsRealtime(1);
        
        while(Vector3.Distance(TargetObject.transform.position, ToPosition) > 0.1f){
            TargetObject.transform.position = Vector3.MoveTowards(TargetObject.transform.position, ToPosition, Speed / 10);

            yield return new WaitForFixedUpdate();
        }
        TargetObject.GetComponent<Animator>().SetTrigger("Exit");
        yield return new WaitForSecondsRealtime(1);
        Destroy(TargetObject);
    }
}
